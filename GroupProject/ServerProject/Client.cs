using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserModel;

namespace ServerProject
{
    public  class Client: IDisposable
    {
        public int ClientNumber { get; set; }
        public User? User { get; set; }

        public TcpClient TcpClient { get; set; }

        public Client(TcpClient client) 
        {
            TcpClient = client;
        }

        public async Task SendUserAsync()
        {
            var objString = JsonSerializer.Serialize(User);
            await SendMsgAsync(objString);
        }
        public async Task SendMsgAsync(string? msg)
        {
            try
            {
                var buffer = Encoding.UTF8.GetBytes(msg);
                var stream = TcpClient.GetStream();
                int size = buffer.Length;
                await stream.WriteAsync(BitConverter.GetBytes(size));
                await stream.WriteAsync(buffer);
                await stream.FlushAsync();
            }
            catch (Exception)
            {

                throw new Exception(ClientNumber.ToString());
            }
           
        }

        public async Task<string> ReceiveMsgAsync()
        {
            try
            {
                var stream = TcpClient.GetStream();
                var size = new byte[4];
                await stream.ReadAsync(size, 0, size.Length);
                var buffer = new byte[BitConverter.ToInt32(size)];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
            catch (Exception)
            {

                throw new Exception(ClientNumber.ToString());
            }
           
        }

        public void Dispose()
        {
            TcpClient.Close();
        }
    }
}
