using Client.Model;
using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UserModel;

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


        private bool _EnableElementsVM;
        public bool EnableElementsVM
        {
            get { return _EnableElementsVM; }
            set { _EnableElementsVM = value; OnPropertyChanged(); }
        }


        public LogginVM()
        {
            _pageModel = new PageModel();
            StaticVisableAndEnableElementsOnView.DesableElemet_Loggin_Register = true;

            Task.Run(() =>
            {
                while (true)
                {
                    EnableElementsVM = StaticVisableAndEnableElementsOnView.DesableElemet_Loggin_Register;
                    if(EnableElementsVM == false )
                    {
                        break;
                    }
                }
            });
            //CustomerID = "das";
            //PoswordLoggins = "2222";
        }





        private RelayCommand logginOn_Server;
        public ICommand LogginOnServer => logginOn_Server ??= new RelayCommand(PerformLogginOnServer);
        private async void PerformLogginOnServer(object commandParameter)
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
                await client.SendAsync("Log in" + "\t" + CustomerID + "\t" + passwordHash);

                string answer = await client.ReciveAsync();

                if (answer.Equals("OK"))
                {
                    StaticMessageClient.Client = new("127.0.0.1", 4321);
                    StaticMessageClient.Client.ConnectToServer();

                    UTPallDate = "Login is successful";
                    StaticVisableAndEnableElementsOnView.DesableElemet_Loggin_Register = false;
                    Thread.Sleep(1500);
                    StaticVisableAndEnableElementsOnView.EnamleOnButtonGame = System.Windows.Visibility.Visible;
                    StaticVisableAndEnableElementsOnView.EnamleOnLoggingGame = System.Windows.Visibility.Hidden;
                }
                else
                {
                    throw new Exception(answer);
                }
            }
            catch (Exception ex)
            {
                //retry
                UTPallDate = ex.Message;
                StaticClient.Client?.Dispose();
            }

        }

    }
}
