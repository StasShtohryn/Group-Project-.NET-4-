using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.ServerClient
{
    public class Server
    {
        TcpListener listener;

        public Server(IPEndPoint localEndPoint)
        {
            listener = new TcpListener(localEndPoint);
        }
    }
}
