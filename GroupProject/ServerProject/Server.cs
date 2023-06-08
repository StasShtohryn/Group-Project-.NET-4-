using Azure.Core.Serialization;
using ServerProject;
using ServerProject.Services;
using ServerProject.UserDb;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using UserModel;

namespace TicTacToe.ServerClient
{

    public enum GameResult
    {
        Continue = 0,
        Win = 1,
        Defeat = 2,
        Draw = 3
    }

    public class Server : IDisposable
    {
        TcpListener listener;
        TcpListener chatListener;
        TcpListener messageListener;
        IUserService _userService;
        object? _lockObject;

        Queue<Client> awaitingClients;
        //public List<Client> currentClients;
        public Server(IPAddress address, int port)
        {
            listener = new TcpListener(address, port);
            chatListener = new TcpListener(address, 4321);
            awaitingClients = new Queue<Client>();
            //currentClients = new List<Client>();
            _userService = new UserService(new UserDbContext());
            _lockObject = new object();
        }

        public void StartListening(int backlock)
        {
            listener.Start(backlock);
            chatListener.Start(backlock);
        }


        public async Task StartAcceptAsync()
        {
            while (true)
            {
                try
                {
                    var task1 = AcceptClientAsync();
                    var client = await task1;
                    _ = BeforeGamePreparing(client);
                }
                catch (Exception ex)
                {
                }

            }

        }

        private async Task<Client> AcceptClientAsync()
        {
            while (true)
            {
                var tcpClient = await listener.AcceptTcpClientAsync();
                var client = new Client(tcpClient);
                try
                {
                    var tmp = await client.ReceiveMsgAsync();
                    var action = tmp.Split("\t")[0];
                    var login = tmp.Split("\t")[1];
                    var password = tmp.Split("\t")[2];
                    User newUser = null!;
                    //if (currentClients.Count(x => x.Login == login) > 0)
                    //{
                    //    await client.SendMsgAsync("You have another active session");
                    //    client.Dispose();
                    //    continue;
                    //}
                    if (action.Equals("Log in"))
                    {
                        if (!CheckLogin(login, password))
                        {
                            await client.SendMsgAsync("Wrong login or password");
                            client.Dispose();
                            continue;
                        }
                        newUser = _userService.GetUsers()?.FirstOrDefault(u => u.Login == login && u.Password == password)!;
                        await client.SendMsgAsync("OK");
                    }
                    else if (action.Equals("Register"))
                    {
                        if (_userService.GetUsers().FirstOrDefault(u => u.Login == login) != null)
                        {
                            await client.SendMsgAsync("This login already exists");
                            client.Dispose();
                            continue;
                        }
                        await client.SendMsgAsync("OK");
                        newUser = new User { Login = login, Password = password };
                        await _userService.AddUser(newUser);
                    }
                    ChattingSettingsAsync(client);
                    client.User = newUser;
                    client.userService = _userService;
                    //currentClients.Add(client);
                    return client;
                }
                catch (Exception)
                {
                    client?.Dispose();
                }
                
            }
        }

        private void ChattingSettingsAsync(Client client)
        {
            lock (_lockObject!)
            {
                client.ChatClient = chatListener.AcceptTcpClientAsync().Result;
            }
        }

        private async Task BeforeGamePreparing(Client client2)
        {
            string startGame = await client2.ReceiveMsgAsync();
            if (startGame.Equals("Start Game"))
            {
                await CheckAwaitingClientsAsync(client2);
            }
            else if (startGame.Equals("Close connection"))
            {
                client2.Dispose();
                //currentClients.Remove(client2);
            }
        }

        private async Task CheckAwaitingClientsAsync(Client client2)
        {
            if (awaitingClients.Count != 0)
            {
                Client client1 = awaitingClients.Peek();
                if (client1.Login == client2.Login)
                {
                    awaitingClients.Enqueue(client2);
                    await client2.SendMsgAsync("Waiting for second player...");
                    return;
                }
                awaitingClients.Dequeue();
                try
                {
                    await client1.SendMsgAsync("OK");

                }
                catch (Exception)
                {
                    client1.Dispose();
                    //currentClients.Remove(client1);
                    await CheckAwaitingClientsAsync(client2);
                    return;
                }
                await client2.SendMsgAsync("OK");
                _ = StartGameAsync(client1, client2);
            }
            else
            {
                awaitingClients.Enqueue(client2);
                await client2.SendMsgAsync("Waiting for second player...");
                return;
            }
        }


        private async Task StartGameAsync(Client client1, Client client2)
        {
            int[] board = new int[9];
            try
            {
                await client1.SendUserAsync();
                await client2.SendUserAsync();
                await client1.SendMsgAsync("X");
                client1.ClientNumber = 1;
                await client2.SendMsgAsync("O");
                client2.ClientNumber = 2;
                _ = StartChattingAsync(client1, client2);
                _ = StartChattingAsync(client2, client1);
                while (true)
                {
                    if (await SendCommandAsync(client1, client2, board))
                        break;
                    else if (await SendCommandAsync(client2, client1, board))
                        break;
                }
            }
            catch (Exception ex)
            {

                if (ex.Message == "1")
                {
                    await client2.SendMsgAsync("Second player is disabled. You won!");
                    await SetStats(client2, GameResult.Win);
                    await SetStats(client1, GameResult.Defeat);
                }
                else if (ex.Message == "2")
                {
                    await client1.SendMsgAsync("Second player is disabled. You won!");
                    await SetStats(client1, GameResult.Win);
                    await SetStats(client2, GameResult.Defeat);
                }
            }
            finally
            {
                client1.Dispose();
                //currentClients.Remove(client1);
                client2.Dispose();
                //currentClients.Remove(client2);
            }
        }

        private async Task<bool> SendCommandAsync(Client client1, Client client2, int[] board)
        {
            string msg = await client1.ReceiveMsgAsync();
            if (msg.Length == 1)
            {
                int pos = int.Parse(msg);
                board[pos-1] = client1.ClientNumber;
                (bool endGame, GameResult res) = IsGameOver(board);
                if (endGame)
                {
                    await SendGameResultsAsync(res, client1, client2);
                    return true;
                }
                await client2.SendMsgAsync(pos.ToString());
            }
            else if(msg.Equals("Surrender"))
            {
                await SendGameResultsAsync(GameResult.Defeat, client1, client2);
                return true;
            }
            else if (msg.Equals("Request to draw"))
            {
                await client2.SendMsgAsync(msg);
            }
            else if (msg.Equals("Agree"))
            {
                await SendGameResultsAsync(GameResult.Draw, client1, client2);
                return true;
            }
            else if (msg.Equals("Disagree"))
            {
                await client2.SendMsgAsync(msg);
            }
            return false;
        }

        private (bool, GameResult) IsGameOver(int[] board)
        {
            for (int i = 0; i < board.Length; i += 3)
            {
                if (board[i] == board[i + 1] && board[i] == board[i + 2] && board[i] != 0)
                    return (true, (GameResult)board[i]);
            }
            for (int i = 0; i < 3; i++)
            {
                if (board[i] == board[i + 3] && board[i] == board[i + 6] && board[i] != 0)
                    return (true, (GameResult)board[i]);
            }
            if (board[0] == board[4] && board[0] == board[8] && board[0] != 0)
                return (true, (GameResult)board[0]);
            if (board[2] == board[4] && board[2] == board[6] && board[2] != 0)
                return (true, (GameResult)board[2]);
            if (board.Where(f => f == 0).Count() == 0)
                return (true, GameResult.Draw);
            return (false, GameResult.Continue);
        }

        private async Task SendGameResultsAsync(GameResult res, Client client1, Client client2)
        {
            if (res == GameResult.Win)
            {
                await SetStats(client1, GameResult.Win);
                await SetStats(client2, GameResult.Defeat);
            }
            else if (res == GameResult.Defeat)
            {
                await SetStats(client1, GameResult.Defeat);
                await SetStats(client2, GameResult.Win);
            }
            else
            {
                await SetStats(client1, GameResult.Draw);
                await SetStats(client2, GameResult.Draw);
            }
        }
        private async Task SetStats(Client client, GameResult res)
        {
            try
            {
                if (res == GameResult.Win)
                {
                    client.Games += 1;
                    client.Wins += 1;
                    await client.SendMsgAsync("Win");
                }
                else if (res == GameResult.Defeat)
                {
                    client.Games += 1;
                    client.Defeats += 1;
                    await client.SendMsgAsync("Loss");
                }
                else
                {
                    client.Games += 1;
                    client.Defeats += 1;
                    await client.SendMsgAsync("Draw");
                }
            }
            catch (Exception ex)
            {

                client.Dispose();
                //currentClients.Remove(client);
            }
           
        }

        private async Task StartChattingAsync(Client client1, Client cliet2)
        {
            try
            {
                while (true)
                {
                    var msg = await client1.ReceiveChatMsgAsync();
                    msg = client1.Login + ":  " + msg;
                    await cliet2.SendChatMsgAsync(msg);
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task<string> ReceiveChatMsgAsync(TcpClient client)
        {
            var stream = client.GetStream();
            var size = new byte[4];
            await stream.ReadAsync(size, 0, size.Length);
            var buffer = new byte[BitConverter.ToInt32(size)];
            await stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        private bool CheckLogin(string? login, string? pass)
        {
            User user;
            if ((user = _userService.GetUsers()?.FirstOrDefault(u => u.Login == login)) != null)
            {
                if (user.Password.Equals(pass))
                    return true;
            }
            return false;
        }

        public void Dispose()
        {
            listener.Stop();
        }
    }
}
