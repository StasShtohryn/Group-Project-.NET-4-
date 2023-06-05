using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Model;
using Client.Utilities;

namespace Client.ViewModel
{
    class GameXOXVM : Utilities.ViewModelBase
    {
        List<string> Buttones = new List<string>(9);
        ButtonesString buttonesString = new ButtonesString();
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
        public String B6
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
        public ICommand Bts1 => bt_1 ??= new RelayCommand(BTp1);

        private async void BTp1(object obj)
        {
            throw new NotImplementedException();
        }

        public ICommand Bts2 => bt_2 ??= new RelayCommand(BTp2);

        private async void BTp2(object obj)
        {
            throw new NotImplementedException();
        }

        public ICommand Bts3 => bt_3 ??= new RelayCommand(BTp3);

        private async void BTp3(object obj)
        {
            throw new NotImplementedException();
        }

        public ICommand Bts4 => bt_4 ??= new RelayCommand(BTp4);

        private async void BTp4(object obj)
        {
            throw new NotImplementedException();
        }

        public ICommand Bts5 => bt_5 ??= new RelayCommand(BTp5);

        private async void BTp5(object obj)
        {
            throw new NotImplementedException();
        }

        public ICommand Bts6 => bt_6 ??= new RelayCommand(BTp6);

        private async void BTp6(object obj)
        {
            throw new NotImplementedException();
        }

        public ICommand Bts7 => bt_7 ??= new RelayCommand(BTp7);

        private async void BTp7(object obj)
        {
            throw new NotImplementedException();
        }

        public ICommand Bts8 => bt_8 ??= new RelayCommand(BTp8);

        private async void BTp8(object obj)
        {
            throw new NotImplementedException();
        }

        public ICommand Bts9 => bt_9 ??= new RelayCommand(BTp9);

        private async void BTp9(object obj)
        {
            throw new NotImplementedException();
        }

        private RelayCommand _surrender;
        private RelayCommand _offer_a_draw;
        private RelayCommand _Ask_for_a_pause;

        public ICommand Surrender => _surrender ??= new RelayCommand(surrenderBt);

        private async void surrenderBt(object obj)
        {
            throw new NotImplementedException();
        }

        public ICommand OfferADraw => _offer_a_draw ??= new RelayCommand(offer_a_drawBt);

        private async void offer_a_drawBt(object obj)
        {
            throw new NotImplementedException();
        }

        public ICommand AskForAPause => _Ask_for_a_pause ??= new RelayCommand(Ask_For_A_PauseBt);

        private async void Ask_For_A_PauseBt(object obj)
        {
            throw new NotImplementedException();
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


        public GameXOXVM()
        {
            var client = StaticClient.Client;
            var value = client.ReciveAsync();
        }
    }
}
