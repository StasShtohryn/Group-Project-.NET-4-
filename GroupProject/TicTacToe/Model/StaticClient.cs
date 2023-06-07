using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModel;

namespace Client.Model
{
    internal static class StaticClient
    {
        private static TicTacToe.Client.Client client;

        public static TicTacToe.Client.Client Client
        {
            get { return client; }
            set { client = value; }
        }

        public static bool OpenWindow { get; set; } = true;
    }
}
