using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using User;

namespace TicTacToe.ServerClient
{
    public class Server
    {
        TcpListener listener;
        TcpListener messageListener;
        public Dictionary<string, string> logins;

        public Dictionary<User, TcpClient> currentClients;
        public Server(IPAddress address, int port)
        {
            listener = new TcpListener(address, port);
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
                logins[temp[0]] = temp[1];
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
                            await SendMsgAsync(tcpClient, "This login already exists");
                            tcpClient.Close();
                            continue;
                        }
                        await SendMsgAsync(tcpClient, "OK");
                        logins[login] = password;
                    }
                    currentClients.Add(login, tcpClient);
                    _ = HandleClientAsync(login, tcpClient);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task HandleClientAsync(string? login, TcpClient tcpClient)
        {
            try
            {
                while (true)
                {
                    string data = await ReceiveMsgAsync(tcpClient);
                    if (data.Equals("Close connection"))
                    {
                        return;
                    }
                    var addressee = data.Split("\t", StringSplitOptions.RemoveEmptyEntries)[0];
                    var message = data.Split("\t", StringSplitOptions.RemoveEmptyEntries)[1];
                    if (currentClients.ContainsKey(addressee))
                    {
                        var msg = login + ":  " + message;
                        var task1 = SendMsgAsync(currentClients[addressee], msg);
                        Task.WaitAll(task1);
                    }
                    else
                    {
                        await SendMsgAsync(tcpClient, "Message wasn't sent");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                currentClients.Remove(login);
                tcpClient.Close();
            }
        }
    }
}
