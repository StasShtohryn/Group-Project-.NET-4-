using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Client
{
    internal class Client
    {
        TcpClient tcpClient;

        public Client(string ip, int port)
        {
            tcpClient = new TcpClient(ip, port);
        }

        public async Task Send(string msg)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(msg);

            await tcpClient.GetStream().WriteAsync(Encoding.UTF8.GetBytes(bytes.Length.ToString()));

            await tcpClient.GetStream().WriteAsync(bytes);
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
