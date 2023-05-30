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

    public class Server
    {
        TcpListener listener;
        TcpListener messageListener;
        List<User> users;

        public Dictionary<string, TcpClient> currentClients;
        public Server(IPAddress address, int port)
        {
            listener = new TcpListener(address, port);
            users = new List<User>();
            currentClients = new Dictionary<string, TcpClient>();
        }

        public async Task DownloadLoginsAsync()
        {
            if (!File.Exists("Logins.txt"))
                return;
            string text = await File.ReadAllTextAsync("Logins.txt", Encoding.UTF8);
            var arr = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in arr)
            {
                var temp = item.Split("\t", StringSplitOptions.RemoveEmptyEntries);
                users.Add(new User(temp[0], temp[1]));
            }

        }

        public async Task SaveLoginsAsync()
        {
            if (!File.Exists("Logins.txt"))
                File.Create("Logins.txt");
            StringBuilder sb = new StringBuilder();
            foreach (var item in users)
            {
                sb.Append(item.Login + "\t");
                sb.Append(item.Password + "\n");
            }
            File.WriteAllText("Logins.txt", sb.ToString());
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
                   var task2 = AcceptClientAsync();
                    task1.Start();
                    task2.Start();
                    Task.WaitAll(task1, task2);
                    var client1 = task1.Result;
                    var client2 = task2.Result;
                    _ = StartGameAsync(client1, client2);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        private async Task<TcpClient> AcceptClientAsync()
        {
            while (true)
            {
                var tcpClient = await listener.AcceptTcpClientAsync();
                var tmp = await ReceiveMsgAsync(tcpClient);
                var action = tmp.Split("\t")[0];
                var login = tmp.Split("\t")[1];
                var password = tmp.Split("\t")[2];
                if (action.Equals("Log in"))
                {
                    if (!CheckLogin(login, password))
                    {
                        await SendMsgAsync(tcpClient, "Wrong login or password");
                        tcpClient.Close();
                        continue;
                    }
                    await SendMsgAsync(tcpClient, "OK");
                }
                else if (action.Equals("Register"))
                {
                    if (users.FirstOrDefault(u=>u.Login==login)!=null)
                    {
                        JsonObject json = new JsonObject();
                        json.Add("Response", "Fail");
                        string text = JsonSerializer.Serialize(json);
                        await SendMsgAsync(tcpClient, "This login already exists");
                        tcpClient.Close();
                        continue;
                    }
                    await SendMsgAsync(tcpClient, "OK");
                    users.Add(new User(login, password));
                }
                currentClients.Add(login, tcpClient);
                return tcpClient;
            }
        }

        private async Task StartGameAsync(TcpClient client1, TcpClient client2)
        {
            int[] board = new int[9];
            try
            {
                await SendMsgAsync(client1, "X");
                await SendMsgAsync(client2, "O");
                while (true)
                {
                    int pos = int.Parse(await ReceiveMsgAsync(client1));
                    board[pos] = 1;
                    (bool endGame, GameResult res) = IsGameOver(board);
                    if (endGame)
                    {
                        await SendGameResultsAsync(res, client1, client2);
                        return;
                    }
                    await SendMsgAsync(client2, pos.ToString());
                    pos = int.Parse(await ReceiveMsgAsync(client2));
                    board[pos] = 2;
                    (endGame, res) = IsGameOver(board);
                    if (endGame)
                    {
                        await SendGameResultsAsync(res, client1, client2);
                        return;
                    }
                    await SendMsgAsync(client1, pos.ToString());
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                client1.Close();
                client2.Close();
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
            if (board.Where(f=>f==0).Count()==0)
                return (true, GameResult.Draw);
            return (false, GameResult.Continue);
        }

        private async Task SendGameResultsAsync(GameResult res, TcpClient client1, TcpClient client2)
        {
            if (res == GameResult.WinX)
            {
                await SendMsgAsync(client1, "Win");
                await SendMsgAsync(client2, "Loose");
            }
            else if (res == GameResult.WinO)
            {
                await SendMsgAsync(client1, "Loose");
                await SendMsgAsync(client2, "Win");
            }
            else
            {
                await SendMsgAsync(client1, "Draw");
                await SendMsgAsync(client2, "Draw");
            }
        }

        public async Task<string> ReceiveMsgAsync(TcpClient tcpClient)
        {
            var stream = tcpClient.GetStream();
            var size = new byte[4];
            await stream.ReadAsync(size, 0, size.Length);
            var buffer = new byte[BitConverter.ToInt32(size)];
            await stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        public async Task SendMsgAsync(TcpClient tcpClient, string? msg)
        {
            var buffer = Encoding.UTF8.GetBytes(msg);
            var stream = tcpClient.GetStream();
            int size = buffer.Length;
            await stream.WriteAsync(BitConverter.GetBytes(size));
            await stream.WriteAsync(buffer);
            await stream.FlushAsync();
        }

        private bool CheckLogin(string? login, string? pass)
        {
            User user;
            if ((user = users?.FirstOrDefault(u=>u.Login==login))!=null)
            {
                if (user.Password.Equals(pass))
                    return true;
            }
            return false;
        }

        public async Task StopListening()
        {
            foreach (var item in currentClients)
            {
                item.Value.Dispose();
            }
            listener.Stop();
        }
    }
}
