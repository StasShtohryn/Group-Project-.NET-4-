using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Client.Utilities;
using System.Windows.Input;
using Client.ViewModel;
using Client.Model;

namespace Client.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        private System.Windows.Visibility is_EnableViewMDS;

        public System.Windows.Visibility IsEnableView_MDS
        {
            get { return is_EnableViewMDS; }
            set { is_EnableViewMDS = value; OnPropertyChanged(); }
        }

        
        
        public ICommand LogginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand GameCommand { get; set; }

        private void Loggin(object obj) => CurrentView = new LogginVM();
        private void Register(object obj) => CurrentView = new Registration();
        private void Game(object obj) => CurrentView = new GameVM();
        public NavigationVM()
        {
            StaticVisableAndEnableElementsOnView.EnamleOnGame = System.Windows.Visibility.Visible;
            LogginCommand = new RelayCommand(Loggin);
            RegisterCommand = new RelayCommand(Register);
            GameCommand = new RelayCommand(Game);
            //// Startup Page
            CurrentView = new LogginVM();
            Task.Run(() =>
            {
               while (true)
                {
                    IsEnableView_MDS = StaticVisableAndEnableElementsOnView.EnamleOnGame;
                    if (IsEnableView_MDS == System.Windows.Visibility.Hidden)
                    {
                       
                        break;
                    }
                }
            });
        }
    }
}
