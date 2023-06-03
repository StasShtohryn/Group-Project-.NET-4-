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
        private object _currentViewGame;
        
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public object CurrentViewGame
        {
            get { return _currentViewGame; }
            set { _currentViewGame = value; OnPropertyChanged(); }
        }
        private System.Windows.Visibility is_EnableViewMDS;
        private System.Windows.Visibility is_EnableViewMDPage;
        public System.Windows.Visibility IsEnableView_MDSPage
        {
            get { return is_EnableViewMDPage; }
            set { is_EnableViewMDPage = value; OnPropertyChanged(); }
            
        }
        public System.Windows.Visibility IsEnableView_MDS
        {
            get { return is_EnableViewMDS; }
            set { is_EnableViewMDS = value; OnPropertyChanged(); }
        }

        
        
        public ICommand LogginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand GameCommand { get; set; }
        public ICommand GameCommandXOX { get; set; }

        private void Loggin(object obj) => CurrentView = new LogginVM();
        private void Register(object obj) => CurrentView = new Registration();
        private void Game(object obj) => CurrentView = new GameVM();
        private void GameXOX(object obj) => CurrentView = new GameXOXVM();
        public NavigationVM()
        {
            StaticVisableAndEnableElementsOnView.EnamleOnGame = System.Windows.Visibility.Visible;
            StaticVisableAndEnableElementsOnView.EnamleOnGamePage = System.Windows.Visibility.Hidden;
            LogginCommand = new RelayCommand(Loggin);
            RegisterCommand = new RelayCommand(Register);
            GameCommand = new RelayCommand(Game);
            GameCommandXOX = new RelayCommand(GameXOX);
            //// Startup Page
            CurrentView = new LogginVM();
            CurrentViewGame = new GameXOXVM();
            Task.Run(() =>
            {
               while (true)
                {
                    IsEnableView_MDS = StaticVisableAndEnableElementsOnView.EnamleOnGame;
                    IsEnableView_MDSPage = StaticVisableAndEnableElementsOnView.EnamleOnGamePage;
                    if (IsEnableView_MDS == System.Windows.Visibility.Hidden)
                    {
                        IsEnableView_MDSPage = System.Windows.Visibility.Visible;
                        break;
                    }
                }
            });
        }
    }
}
