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
        WinX = 1,
        WinO = 2,
        Draw = 3
    }

    public class Server:IDisposable
    {
        TcpListener listener;
        TcpListener messageListener;
        IUserService _userService;

        Queue<Client> awaitingClients;
        public List<Client> currentClients;
        public Server(IPAddress address, int port)
        {
            listener = new TcpListener(address, port);
            awaitingClients = new Queue<Client>();
            currentClients = new List<Client>();
            _userService = new UserService(new UserDbContext());
        }

        public void StartListening(int backlock)
        {
            listener.Start(backlock);
        }


        public async Task StartAcceptAsync()
        {
            try
            {
                while (true)
                {
                    var task1 = AcceptClientAsync();
                    var client = await task1;
                    _ = BeforeGamePreparing(client);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private async Task<Client> AcceptClientAsync()
        {
            while (true)
            {
                var tcpClient = await listener.AcceptTcpClientAsync();
                var client = new Client(tcpClient);
                var tmp = await client.ReceiveMsgAsync();
                var action = tmp.Split("\t")[0];
                var login = tmp.Split("\t")[1];
                var password = tmp.Split("\t")[2];
                User newUser = null!;
                if (currentClients.Count(x => x.User.Login == login) > 0)
                {
                    await client.SendMsgAsync( "You have another active session");
                    tcpClient.Close();
                    continue;
                }
                else if (action.Equals("Log in"))
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
                client.User = newUser;
                await client.SendUserAsync();
                currentClients.Add(client);
                return client;
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
            }
        }

        private async Task CheckAwaitingClientsAsync(Client client2)
        {
            Client client1;
            if (awaitingClients.Count != 0)
            {
                client1 = awaitingClients.Peek();
                awaitingClients.Dequeue();
                try
                {
                    await client1.SendMsgAsync("OK");

                }
                catch (Exception)
                {
                    awaitingClients.Enqueue(client2);
                    await client2.SendMsgAsync( "Waiting for second player...");
                    await CheckAwaitingClientsAsync(client2);
                    return;
                }
                await client2.SendMsgAsync( "OK");
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
                await client1.SendMsgAsync( "X");
                client1.ClientNumber = 1;
                await client2.SendMsgAsync( "O");
                client2.ClientNumber = 2;
                while (true)
                {
                    int pos = int.Parse(await client1.ReceiveMsgAsync());
                    board[pos] = 1;
                    (bool endGame, GameResult res) = IsGameOver(board);
                    if (endGame)
                    {
                        await SendGameResultsAsync(res, client1, client2);
                        break;
                    }
                    await client2.SendMsgAsync( pos.ToString());
                    pos = int.Parse(await client2.ReceiveMsgAsync());
                    board[pos] = 2;
                    (endGame, res) = IsGameOver(board);
                    if (endGame)
                    {
                        await SendGameResultsAsync(res, client1, client2);
                        break;
                    }
                    await client1.SendMsgAsync( pos.ToString());
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                client1.Dispose();
                client2.Dispose();
            }
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
            if (res == GameResult.WinX)
            {
                await client1.SendMsgAsync("Win");
                await client2.SendMsgAsync("Loose");
            }
            else if (res == GameResult.WinO)
            {
                await client1.SendMsgAsync("Loose");
                await client2.SendMsgAsync( "Win");
            }
            else
            {
                await client1.SendMsgAsync("Draw");
                await client2.SendMsgAsync("Draw");
            }
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
