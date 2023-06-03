using System.Net;
using TicTacToe.ServerClient;

namespace ServerProject
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Server server = new Server(IPAddress.Loopback, 1234);
                //await server.DownloadLoginsAsync();
                server.StartListening(10);
                await server.StartAcceptAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }
    }
}