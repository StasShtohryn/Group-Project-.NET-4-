using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Client
{
    internal class Client
    {
        TcpClient tcpClient;
        IPEndPoint remoteEndPoint;

        public Client(string ip, int port)
        {
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            tcpClient = new TcpClient();
        }

        public void ConnectToServer()
        {
            try
            {
                tcpClient.Connect(remoteEndPoint);
                if (!tcpClient.Connected)
                {
                    throw new Exception("Connection was not established");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Send(string msg)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(msg);

            await tcpClient.GetStream().WriteAsync(Encoding.UTF8.GetBytes(bytes.Length.ToString()));

            await tcpClient.GetStream().WriteAsync(bytes);
            await tcpClient.GetStream().FlushAsync();
        }

        public async Task<string> Recive()
        {
            byte[] buf = new byte[4];

            await tcpClient.GetStream().ReadAsync(buf, 0, 4);
            int size = BitConverter.ToInt32(buf);

            byte[] data = new byte[size];
            await tcpClient.GetStream().ReadAsync(data, 0, size);

            return Encoding.UTF8.GetString(buf);
        }

        public void Dispose()
        {
            tcpClient?.Dispose();
        }
    }
}
