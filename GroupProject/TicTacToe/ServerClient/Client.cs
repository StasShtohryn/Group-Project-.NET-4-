using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace TicTacToe.ServerClient
{
    internal class Client
    {
        TcpClient tcpClient;

        public Client(string ip, int port)
        {
            tcpClient = new TcpClient(ip, port);
        }

        public async Task Send(JsonObject obj)
        {
            string json_text = JsonSerializer.Serialize(obj);

            byte[] bytes = Encoding.UTF8.GetBytes(json_text);

            await tcpClient.GetStream().WriteAsync(Encoding.UTF8.GetBytes(bytes.Length.ToString()));

            await tcpClient.GetStream().WriteAsync(Encoding.UTF8.GetBytes(json_text));
        }

        public async Task<JsonObject> Recive()
        {
            byte[] buf = new byte[1024];

            await tcpClient.GetStream().ReadAsync(buf);
            int size = int.Parse(Encoding.UTF8.GetString(buf));

            do
            {
                await tcpClient.GetStream().ReadAsync(buf);
                size--;

            } while (size > 0);

            JsonObject? obj = JsonSerializer.Deserialize<JsonObject>(Encoding.UTF8.GetString(buf));

            return obj;
        }
    }
}
