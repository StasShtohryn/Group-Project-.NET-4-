using ServerProject.Services;
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
    public class Client : IDisposable
    {
        public int ClientNumber { get; set; }
        private User? _user;
        public User? User { private get => _user; set => _user = value; }

        public string Login
        {
            get => _user?.Login ?? string.Empty;
            set
            {
                if (_user.Login != value)
                {
                    _user.Login = value;
                    userService.UpdateUser(_user);
                }
            }
        }

        public string Password
        {
            get => _user?.Password ?? string.Empty;
            set
            {
                if (_user.Password != value)
                {
                    _user.Password = value;
                    userService.UpdateUser(_user);
                }
            }
        }
        public int Games
        {
            get => _user?.Games ?? 0;
            set
            {
                if (_user.Games != value)
                {
                    _user.Games = value;
                    userService.UpdateUser(_user);
                }
            }
        }

        public int Wins
        {
            get => _user?.Wins ?? 0;
            set
            {
                if (_user.Wins != value)
                {
                    _user.Wins = value;
                    userService.UpdateUser(_user);
                }
            }
        }

        public int Defeats
        {
            get => _user?.Defeats ?? 0;
            set
            {
                if (_user.Defeats != value)
                {
                    _user.Defeats = value;
                    userService.UpdateUser(_user);
                }
            }
        }

        public int Draws
        {
            get => _user?.Draws ?? 0;
            set
            {
                if (_user.Draws != value)
                {
                    _user.Draws = value;
                    userService.UpdateUser(_user);
                }
            }
        }

        public IUserService? userService { get; set; }

        public TcpClient TcpClient { get; set; }

        public TcpClient? ChatClient { get; set; }

        public Client(TcpClient client)
        {
            TcpClient = client;
        }

        public async Task SendChatMsgAsync(string msg)
        {
            if (ChatClient != null)
            {
                var buffer = Encoding.UTF8.GetBytes(msg);
                var stream = ChatClient?.GetStream();
                int size = buffer.Length;
                await stream.WriteAsync(BitConverter.GetBytes(size));
                await stream.WriteAsync(buffer);
                await stream.FlushAsync();
            }

        }

        public async Task<string> ReceiveChatMsgAsync()
        {
            var stream = ChatClient.GetStream();
            var size = new byte[4];
            await stream.ReadAsync(size, 0, size.Length);
            var buffer = new byte[BitConverter.ToInt32(size)];
            await stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
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
            ChatClient?.Close();
        }
    }
}
