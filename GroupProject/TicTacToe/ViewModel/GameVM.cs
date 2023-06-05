using Client.Model;
using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.ViewModel
{
    class GameVM : Utilities.ViewModelBase
    {
        string? message;

        public string Message 
        { 
            get => message;
            set 
            {
                if (message!=value)
                {
                    message = value;
                    OnPropertyChanged();
                }
            }  
        }

        private bool is_disableactivity;
        public bool IsDisableActivity
        {
            get { return is_disableactivity; }
            set { is_disableactivity = value; OnPropertyChanged(); }

        }
        async void VDDP()
        {
            while (true)
            {
                IsDisableActivity = StaticVisableAndEnableElementsOnView.Enablebuttongame;
                if (IsDisableActivity == false)
                {
                    break;
                }
            }
        }
        public GameVM()
        {
            StaticVisableAndEnableElementsOnView.Enablebuttongame = true;
            Task.Run(() =>
            {
                VDDP();
            });
        }
        async Task Start()
        {

            
            while (true)
            {
                await StaticClient.Client.SendAsync("Start Game");
                string answer = await StaticClient.Client.ReciveAsync();
                Message = answer;

                if (answer == "OK")
                {
                    StaticVisableAndEnableElementsOnView.EnamleOnGame = System.Windows.Visibility.Hidden;
                    break;
                }
                

            }

            

        }
        private RelayCommand open_Game;
        public ICommand OpenGame => open_Game ??= new RelayCommand(OpenGameX);



        private async void OpenGameX(object commandParameter)
        {
            StaticVisableAndEnableElementsOnView.Enablebuttongame = false;
            Task task = Task.Run(async () =>
            {
                await Start();
            });
            await task;
        }
    }
}
