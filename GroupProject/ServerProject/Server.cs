using System;
using System.Collections.Generic;
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
    public class Server
    {
        TcpListener listener;
        TcpListener messageListener;
        List<User> users;
        public Dictionary<string, string> logins;

        public Dictionary<string, TcpClient> currentClients;
        public Server(IPAddress address, int port)
        {
            listener = new TcpListener(address, port);
            users = new List<User>();
            logins = new Dictionary<string, string>();
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
                users.Add(new U)
            }

        }

        public async Task SaveLoginsAsync()
        {
            if (!File.Exists("Logins.txt"))
                File.Create("Logins.txt");
            StringBuilder sb = new StringBuilder();
            foreach (var item in logins)
            {
                sb.Append(item.Key + "\t");
                sb.Append(item.Value + "\n");
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
                    await SendMsgAsync(client1, "X");
                    await SendMsgAsync(client2, "O");
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
                    if (logins.ContainsKey(login))
                    {
                        JsonObject json = new JsonObject();
                        json.Add("Response", "Fail");
                        string text = JsonSerializer.Serialize(json);
                        await SendMsgAsync(tcpClient, "This login already exists");
                        tcpClient.Close();
                        continue;
                    }
                    await SendMsgAsync(tcpClient, "OK");
                    logins[login] = password;
                }
                currentClients.Add(login, tcpClient);
                return tcpClient;
            }
        }

        private async Task StartGameAsync(TcpClient client1, TcpClient client2)
        {
            int[] board = new int[9];
        }

        //public async Task HandleClientAsync(string login, TcpClient tcpClient)
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            string data = await ReceiveMsgAsync(tcpClient);
        //            if (data.Equals("Close connection"))
        //            {
        //                return;
        //            }
        //            var addressee = data.Split("\t", StringSplitOptions.RemoveEmptyEntries)[0];
        //            var message = data.Split("\t", StringSplitOptions.RemoveEmptyEntries)[1];
        //            if (currentClients.ContainsKey(addressee))
        //            {
        //                var msg = login + ":  " + message;
        //                var task1 = SendMsgAsync(currentClients[addressee], msg);
        //                Task.WaitAll(task1);
        //            }
        //            else
        //            {
        //                await SendMsgAsync(tcpClient, "Message wasn't sent");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        currentClients.Remove(login);
        //        tcpClient.Close();
        //    }
        //}

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
            if (logins.ContainsKey(login))
            {
                if (logins[login].Equals(pass))
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
