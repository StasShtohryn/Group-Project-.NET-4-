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
        public GameVM()
        {

        }
        async Task Start()
        {
            StaticVisableAndEnableElementsOnView.EnamleOnGame = System.Windows.Visibility.Hidden;
            //while (true)
            //{


            //    if( glo == conect2people)
            //    {
            //        StaticVisableAndEnableElementsOnView.EnamleOnGame = System.Windows.Visibility.Hidden;
            //        break;
            //    }
            //}
        }
        private RelayCommand open_Game;
        public ICommand OpenGame => open_Game ??= new RelayCommand(OpenGameX);
        private async void OpenGameX(object commandParameter)
        {

            Task.Run(async () =>
            {
                await Start();
            });
        }
    }
}
