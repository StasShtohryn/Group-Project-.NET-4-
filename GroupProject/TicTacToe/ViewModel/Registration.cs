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
using System.Security.Cryptography;
using System.Text.Json;
using UserModel;
using System.Threading;

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

        private bool _EnableElementsVM;
        public bool EnableElementsVM
        {
            get { return _EnableElementsVM; }
            set { _EnableElementsVM = value; OnPropertyChanged(); }
        }


        public Registration()
        {
            _pageModel = new PageModel();

            StaticVisableAndEnableElementsOnView.DesableElemet_Loggin_Register = true;

            Task.Run(() =>
            {
                while (true)
                {
                    EnableElementsVM = StaticVisableAndEnableElementsOnView.DesableElemet_Loggin_Register;
                    if (EnableElementsVM == false)
                    {
                        break;
                    }
                }
            });

            //CustomerID = "das";
            //PoswordLoggins = "2222";
        }


        private RelayCommand registreOnServer;
        public ICommand RegistreOnServer => registreOnServer ??= new RelayCommand(PerformRegistreOnServer);

        private async void PerformRegistreOnServer(object commandParameter)
        {
            UTPallDate = CustomerID + " " + PoswordLoggins;

            //compute hash
            SHA256 sha256 = SHA256.Create();
            byte[] tmp = sha256.ComputeHash(Encoding.UTF8.GetBytes(PoswordLoggins));

            // Convert byte array to a string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < tmp.Length; i++)
            {
                builder.Append(tmp[i].ToString("x2"));
            }
            string passwordHash = builder.ToString();

            
            sha256.Dispose();

            try
            {
                StaticClient.Client?.Dispose();
                StaticClient.Client  = new TicTacToe.Client.Client("127.0.0.1", 1234);
                var client = StaticClient.Client;
                client.ConnectToServer();
                await client.SendAsync("Register" + "\t" + CustomerID + "\t" + passwordHash);

                string answer = await client.ReciveAsync();

                if (answer.Equals("OK"))
                {
                    StaticMessageClient.Client = new("127.0.0.1", 4321);
                    StaticMessageClient.Client.ConnectToServer();

                    UTPallDate = "Registration is successful";
                    StaticVisableAndEnableElementsOnView.DesableElemet_Loggin_Register = false;
                    Thread.Sleep(1500);
                    StaticVisableAndEnableElementsOnView.EnamleOnButtonGame = System.Windows.Visibility.Visible;
                    StaticVisableAndEnableElementsOnView.EnamleOnLoggingGame = System.Windows.Visibility.Hidden;
                }
                else if (answer.Equals("This login already exists"))
                {
                    throw new Exception("This login already exists");
                }
            }
            catch (Exception ex)
            {
                //retry
                UTPallDate = ex.Message;
                StaticClient.Client.Dispose();
            }


        }
    }
}
