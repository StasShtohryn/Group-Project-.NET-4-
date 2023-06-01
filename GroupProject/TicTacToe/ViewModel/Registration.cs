﻿using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Model;
using System.Windows.Input;
using System.Net.Sockets;
using System.Windows;

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
        public string UTPallDate //Display message in UI
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

        private async void PerformRegistreOnServer(object commandParameter)
        {
            UTPallDate = CustomerID + " " + PoswordLoggins;

            
            try
            {
                StaticClient.Init("127.0.0.1", 1234);
            }
            catch (Exception)
            {
                //retry
                StaticClient.Dispose();
                StaticClient.Init("127.0.0.1", 1234);
            }

            

            await StaticClient.Send("Register" + "\t" + CustomerID + "\t" + PoswordLoggins);

            string answer = await StaticClient.Recive();

            if (answer.Equals("OK")) 
            { 

            }
            else if (answer.Equals("This login already exists"))
            {
                throw new Exception("This login already exists");
            }


        }
    }
}
