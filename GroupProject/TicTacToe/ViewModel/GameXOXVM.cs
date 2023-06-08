using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Client.Model;
using Client.Utilities;
using UserModel;

namespace Client.ViewModel
{
    class GameXOXVM : Utilities.ViewModelBase
    {
        ButtonesString buttonesString = new ButtonesString();
        bool isMyTurn;
        public String Bt1
        {
            get { return buttonesString.Bt1; }
            set { buttonesString.Bt1 = value; OnPropertyChanged(); }
        }
        public String Bt2
        {
            get { return buttonesString.Bt2; }
            set { buttonesString.Bt2 = value; OnPropertyChanged(); }
        }
        public String Bt3
        {
            get { return buttonesString.Bt3; }
            set { buttonesString.Bt3 = value; OnPropertyChanged(); }
        }
        public String Bt4
        {
            get { return buttonesString.Bt4; }
            set { buttonesString.Bt4 = value; OnPropertyChanged(); }
        }
        public String Bt5
        {
            get { return buttonesString.Bt5; }
            set { buttonesString.Bt5 = value; OnPropertyChanged(); }
        }
        public String Bt6
        {
            get { return buttonesString.Bt6; }
            set { buttonesString.Bt6 = value; OnPropertyChanged(); }
        }
        public String Bt7
        {
            get { return buttonesString.Bt7; }
            set { buttonesString.Bt7 = value; OnPropertyChanged(); }
        }
        public String Bt8
        {
            get { return buttonesString.Bt8; }
            set { buttonesString.Bt8 = value; OnPropertyChanged(); }
        }
        public String Bt9
        {
            get { return buttonesString.Bt9; }
            set { buttonesString.Bt9 = value; OnPropertyChanged(); }
        }

        private RelayCommand bt_1;
        private RelayCommand bt_2;
        private RelayCommand bt_3;
        private RelayCommand bt_4;
        private RelayCommand bt_5;
        private RelayCommand bt_6;
        private RelayCommand bt_7;
        private RelayCommand bt_8;
        private RelayCommand bt_9;
        public ICommand Bts1 => bt_1 ??= new RelayCommand(BTp1, CanExecuteMethod);

        private async void BTp1(object obj)
        {
            await Client.SendAsync("1");
            Bt1 = mySymbol;
            isMyTurn = false;
        }

        public ICommand Bts2 => bt_2 ??= new RelayCommand(BTp2, CanExecuteMethod);

        private async void BTp2(object obj)
        {
            await Client.SendAsync("2");
            Bt2 = mySymbol;
            isMyTurn = false;
        }

        public ICommand Bts3 => bt_3 ??= new RelayCommand(BTp3, CanExecuteMethod);

        private async void BTp3(object obj)
        {
            await Client.SendAsync("3");
            Bt3 = mySymbol;
            isMyTurn = false;
        }

        public ICommand Bts4 => bt_4 ??= new RelayCommand(BTp4, CanExecuteMethod);

        private async void BTp4(object obj)
        {
            await Client.SendAsync("4");
            Bt4 = mySymbol;
            isMyTurn = false;
        }

        public ICommand Bts5 => bt_5 ??= new RelayCommand(BTp5, CanExecuteMethod);

        private async void BTp5(object obj)
        {
            await Client.SendAsync("5");
            Bt5 = mySymbol;
            isMyTurn = false;
        }

        public ICommand Bts6 => bt_6 ??= new RelayCommand(BTp6, CanExecuteMethod);

        private async void BTp6(object obj)
        {
            await Client.SendAsync("6");
            Bt6 = mySymbol;
            isMyTurn = false;
        }

        public ICommand Bts7 => bt_7 ??= new RelayCommand(BTp7, CanExecuteMethod);

        private async void BTp7(object obj)
        {
            await Client.SendAsync("7");
            Bt7 = mySymbol;
            isMyTurn = false;
        }

        public ICommand Bts8 => bt_8 ??= new RelayCommand(BTp8, CanExecuteMethod);

        private async void BTp8(object obj)
        {
            await Client.SendAsync("8");
            Bt8 = mySymbol;
            isMyTurn = false;
        }

        public ICommand Bts9 => bt_9 ??= new RelayCommand(BTp9, CanExecuteMethod);

        private async void BTp9(object obj)
        {
            await Client.SendAsync("9");
            Bt9 = mySymbol;
            isMyTurn = false;
        }


        private RelayCommand _press_button;
        public ICommand PressButton => _press_button ??= new RelayCommand(PressButtonAsync, CanExecuteMethod);

        private async void PressButtonAsync(object obj)
        {
            await Client.SendAsync(obj.ToString());
            isMyTurn = false;
        }

        //private RelayCommand _surrender;
        //private RelayCommand _offer_a_draw;
        //private RelayCommand _Ask_for_a_pause;
        private RelayCommand _send_message;
        private RelayCommand _new_game;

        //public ICommand Surrender => _surrender ??= new RelayCommand(surrenderBt, CanExecuteMethod);

        //private async void surrenderBt(object obj)
        //{
        //    await Client.SendAsync("Surrender");
        //}

        //public ICommand OfferADraw => _offer_a_draw ??= new RelayCommand(offer_a_drawBt, CanExecuteMethod);

        //private async void offer_a_drawBt(object obj)
        //{
        //    await Client.SendAsync("Draw");
        //}

        //public ICommand AskForAPause => _Ask_for_a_pause ??= new RelayCommand(Ask_For_A_PauseBt, CanExecuteMethod);

        //private async void Ask_For_A_PauseBt(object obj)
        //{
        //    await Client.SendAsync("Pause");
        //}

        public ICommand SendMessage => _send_message ??= new RelayCommand(SendMessageAsync);

        async void SendMessageAsync(object obj)
        {
            await MessagesClient.SendAsync(currentMessage);
            Messages.Add("You:  " + currentMessage);
            currentMessage = string.Empty;

        }


        public ICommand NewGame => _new_game ??= new RelayCommand(StartNewGame, CanExecuteNewGameMethod);

        private async void StartNewGame(object obj)
        {
            await Client.SendAsync("Start Game");

            while (true)
            {
                string answer = await Client.ReciveAsync();
                //Message = answer;

                if (answer == "OK")
                {
                    IsGameOver = false;
                    ClearBoard();
                    _ = StartGame();
                    break;
                }
                else if (answer == "Waiting for second player...")
                    continue;


            }
        }


        public String TextMach
        {
            get { return buttonesString.TextMach; }
            set { buttonesString.TextMach = value; OnPropertyChanged(); }
        }
        public String TextDefeatMach
        {
            get { return buttonesString.TextDefeatMach; }
            set { buttonesString.TextDefeatMach = value; OnPropertyChanged(); }
        }
        public String TextVictoryMach
        {
            get { return buttonesString.TextVictoryMach; }
            set { buttonesString.TextVictoryMach = value; OnPropertyChanged(); }
        }
        public String TextSurenderMach
        {
            get { return buttonesString.TextSurenderMach; }
            set { buttonesString.TextSurenderMach = value; OnPropertyChanged(); }
        }

        string curMsg;
        public String currentMessage
        {
            get { return curMsg; }
            set { curMsg = value; OnPropertyChanged(); }
        }


        async Task StartGame()
        {
            //isMyTurn = true;
            string user_json = await Client.ReciveAsync();
            User = JsonSerializer.Deserialize<UserModel.User>(user_json);

            enemySymbol = string.Empty;
            mySymbol = await StaticClient.Client.ReciveAsync();

            if (mySymbol == "X")
            {
                enemySymbol = "O";
                isMyTurn = true;
            }

            else
            {
                enemySymbol = "X";
                isMyTurn = false;
            }


            _ = ReciveMessageAsync();
            while (true)
            {
                string answer = await StaticClient.Client.ReciveAsync();

                if (answer.Equals("1"))
                    Bt1 = enemySymbol;

                else if (answer.Equals("2"))
                    Bt2 = enemySymbol;

                else if (answer.Equals("3"))
                    Bt3 = enemySymbol;

                else if (answer.Equals("4"))
                    Bt4 = enemySymbol;

                else if (answer.Equals("5"))
                    Bt5 = enemySymbol;

                else if (answer.Equals("6"))
                    Bt6 = enemySymbol;

                else if (answer.Equals("7"))
                    Bt7 = enemySymbol;

                else if (answer.Equals("8"))
                    Bt8 = enemySymbol;

                else if (answer.Equals("9"))
                    Bt9 = enemySymbol;

                else if (answer.Equals("Request to draw"))
                {
                    var res = MessageBox.Show(answer, "Request to draw", MessageBoxButton.YesNo);

                    if (res == MessageBoxResult.Yes)
                        await Client.SendAsync("Agree");
                    else
                        await Client.SendAsync("Disagree");

                    isMyTurn = false;
                    continue;
                }
                else if (answer.Equals("Disagree"))
                {
                    MessageBox.Show("Opponent rejected");
                }

                else
                {
                    MessageBox.Show(answer);
                    IsGameOver = true;
                    break;
                }
                isMyTurn = true;

            }

        }

        private bool CanExecuteMethod(object? param)
        {
            if (param != null)
            {
                string str = param.ToString();
                return isMyTurn && str != "X" && str != "O";
            }
            return isMyTurn;
        }

        private bool CanExecuteNewGameMethod(object? param)
        {
            return IsGameOver;
        }

        async Task ReciveMessageAsync()
        {
            while (true)
            {
                string msg = await MessagesClient.ReciveAsync();
                Messages.Add(msg);
            }

        }

        void ClearBoard()
        {
            Bt1 = string.Empty;
            Bt2 = string.Empty;
            Bt3 = string.Empty;
            Bt4 = string.Empty;
            Bt5 = string.Empty;
            Bt6 = string.Empty;
            Bt7 = string.Empty;
            Bt8 = string.Empty;
            Bt9 = string.Empty;

            currentMessage = string.Empty;
            Messages.Clear();
        }



        TicTacToe.Client.Client Client;
        TicTacToe.Client.Client MessagesClient;

        public ObservableCollection<string> Messages;

        UserModel.User user;


        public UserModel.User User
        {
            get { return user; }
            set 
            { 
                if (user != value)
                {
                    user = value;

                    TextMach = user.Games.ToString();
                    TextDefeatMach = user.Defeats.ToString();
                    TextVictoryMach = user.Wins.ToString();
                    TextSurenderMach = user.Draws.ToString();
                }
                   

            }
        }

        private bool IsGameOver = false;

        private string mySymbol;
        private string enemySymbol;

        public GameXOXVM()
        {
            Messages = new ObservableCollection<string>();
            Client = StaticClient.Client;
            MessagesClient = StaticMessageClient.Client;
            isMyTurn = false;
            Messages = new();
            _ = StartGame();

        }
    }
}
