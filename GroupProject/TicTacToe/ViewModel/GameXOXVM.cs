using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Client.Model;
using Client.Utilities;

namespace Client.ViewModel
{
    class GameXOXVM : Utilities.ViewModelBase
    {
        List<string> Buttones;
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
            set { buttonesString.Bt5 = value; OnPropertyChanged(); }
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
        }

        public ICommand Bts2 => bt_2 ??= new RelayCommand(BTp2, CanExecuteMethod);

        private async void BTp2(object obj)
        {
            await Client.SendAsync("2");
        }

        public ICommand Bts3 => bt_3 ??= new RelayCommand(BTp3, CanExecuteMethod);

        private async void BTp3(object obj)
        {
            await Client.SendAsync("3");
        }

        public ICommand Bts4 => bt_4 ??= new RelayCommand(BTp4, CanExecuteMethod);

        private async void BTp4(object obj)
        {
            await Client.SendAsync("4");
        }

        public ICommand Bts5 => bt_5 ??= new RelayCommand(BTp5, CanExecuteMethod);

        private async void BTp5(object obj)
        {
            await Client.SendAsync("5");
        }

        public ICommand Bts6 => bt_6 ??= new RelayCommand(BTp6, CanExecuteMethod);

        private async void BTp6(object obj)
        {
            await Client.SendAsync("6");
        }

        public ICommand Bts7 => bt_7 ??= new RelayCommand(BTp7, CanExecuteMethod);

        private async void BTp7(object obj)
        {
            await Client.SendAsync("7");
        }

        public ICommand Bts8 => bt_8 ??= new RelayCommand(BTp8, CanExecuteMethod);

        private async void BTp8(object obj)
        {
            await Client.SendAsync("8");
        }

        public ICommand Bts9 => bt_9 ??= new RelayCommand(BTp9, CanExecuteMethod);

        private async void BTp9(object obj)
        {
            await Client.SendAsync("9");
        }

        private RelayCommand _surrender;
        private RelayCommand _offer_a_draw;
        private RelayCommand _Ask_for_a_pause;

        public ICommand Surrender => _surrender ??= new RelayCommand(surrenderBt, CanExecuteMethod);

        private async void surrenderBt(object obj)
        {
            await Client.SendAsync("Surrender");
        }

        public ICommand OfferADraw => _offer_a_draw ??= new RelayCommand(offer_a_drawBt, CanExecuteMethod);

        private async void offer_a_drawBt(object obj)
        {
            await Client.SendAsync("Draw");
        }

        public ICommand AskForAPause => _Ask_for_a_pause ??= new RelayCommand(Ask_For_A_PauseBt, CanExecuteMethod);

        private async void Ask_For_A_PauseBt(object obj)
        {
            await Client.SendAsync("Pause");
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

        
        async Task StartGame()
        {
            
            string enemySymbol = string.Empty;
            string mySymbol = await StaticClient.Client.ReciveAsync();
            
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
                

            while(true)
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

                else
                {
                    MessageBox.Show(answer);
                    break;
                }
            }

        }

        bool CanExecuteMethod(object? param)
        {
            return isMyTurn;
        }

        TicTacToe.Client.Client Client;
        public GameXOXVM()
        {
            Client = StaticClient.Client;

            isMyTurn = false;
            _ = StartGame();
            
        }
    }
}
