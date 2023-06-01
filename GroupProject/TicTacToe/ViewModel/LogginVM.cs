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
    class LogginVM : Utilities.ViewModelBase
    {

        private string RFG;
        private readonly PageModel _pageModel;
        public String CustomerID
        {
            get { return _pageModel.CustomerLoggin; }
            set { _pageModel.CustomerLoggin = value; OnPropertyChanged(); }
        }
        public String PoswordLoggins
        {
            get { return _pageModel.PoswordLoggin; }
            set { _pageModel.PoswordLoggin = value; OnPropertyChanged(); }
        }
        public string UTPallDate //Display message in UI
        {
            get { return RFG; }
            set { RFG = value; OnPropertyChanged(); }
        }

        public LogginVM()
        {
            _pageModel = new PageModel();
            //CustomerID = "das";
            //PoswordLoggins = "2222";
        }

        private RelayCommand logginOn_Server;
        public ICommand LogginOnServer => logginOn_Server ??= new RelayCommand(PerformLogginOnServer);
        private async void PerformLogginOnServer(object commandParameter)
        {
            UTPallDate = CustomerID + " " + PoswordLoggins;
        }

    }
}
