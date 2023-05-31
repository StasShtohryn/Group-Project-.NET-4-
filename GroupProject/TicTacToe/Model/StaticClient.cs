using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    internal static class StaticClient
    {
        static TicTacToe.Client.Client Client;

        public static void Init(string host, int port)
        {
            Client = new(host, port);
        }

        public static async Task Send(string message)
        {
            await Client.Send(message);
        }

        public static async Task<string> Recive()
        {
            return await Client.Recive();
        }

        public static void Dispose()
        {
            Client.Dispose();
        }
    }
}
