using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Model;
using System.Windows.Input;

namespace Client.ViewModel
{
    class Registration : Utilities.ViewModelBase
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
        public string UTPallDate
        {
            get { return RFG; }
            set { RFG = value; OnPropertyChanged(); }
        }


        public Registration()
        {
            _pageModel = new PageModel();
            //CustomerID = "das";
            //PoswordLoggins = "2222";
        }


        private RelayCommand registreOnServer;
        public ICommand RegistreOnServer => registreOnServer ??= new RelayCommand(PerformRegistreOnServer);

        private void PerformRegistreOnServer(object commandParameter)
        {
            UTPallDate = CustomerID + " " + PoswordLoggins;
        }
    }
}
