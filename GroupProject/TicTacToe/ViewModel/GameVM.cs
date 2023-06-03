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
            //_pageModel = new PageModel();
            //CustomerID = "das";
            //PoswordLoggins = "2222";
        }

        private RelayCommand open_Game;
        public ICommand OpenGame => open_Game ??= new RelayCommand(OpenGameX);
        private async void OpenGameX(object commandParameter)
        {

            StaticVisableAndEnableElementsOnView.EnamleOnGame = System.Windows.Visibility.Hidden;
        }
    }
}
