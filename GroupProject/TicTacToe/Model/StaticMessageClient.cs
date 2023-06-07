namespace Client.Model
{
    internal static class StaticMessageClient
    {

        private static TicTacToe.Client.Client client;

        public static TicTacToe.Client.Client Client
        {
            get { return client; }
            set { client = value; }
        }

    }
}
