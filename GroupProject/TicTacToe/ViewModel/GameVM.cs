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
            StaticVisableAndEnableElementsOnView.EnamleOnGame = System.Windows.Visibility.Hidden;
        }
        private RelayCommand open_Game;
        public ICommand OpenGame => open_Game ??= new RelayCommand(OpenGameX);
        private async void OpenGameX(object commandParameter)
        {
            StaticVisableAndEnableElementsOnView.Enablebuttongame = false;
            Task.Run(async () =>
            {
                await Start();
            });
        }
    }
}
