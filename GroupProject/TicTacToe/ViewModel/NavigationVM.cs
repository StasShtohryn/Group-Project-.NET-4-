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


        private System.Windows.Visibility _desebleREG_Log;
        public System.Windows.Visibility DesebleREG_Log
        {
            get { return _desebleREG_Log; }
            set { _desebleREG_Log = value; OnPropertyChanged(); }
        }

        private System.Windows.Visibility _enableButtnosR_startGame;
        public System.Windows.Visibility EnableButtnosR_startGame
        {
            get { return _enableButtnosR_startGame; }
            set { _enableButtnosR_startGame = value; OnPropertyChanged(); }
        }
        public ICommand LogginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand GameCommand { get; set; }
        //public ICommand GameCommandXOX { get; set; }

        private void Loggin(object obj) => CurrentView = StaticVisableAndEnableElementsOnView.Loggin;
        private void Register(object obj) => CurrentView = StaticVisableAndEnableElementsOnView.Registration;
        private void Game(object obj) => CurrentView = StaticVisableAndEnableElementsOnView.GameVM;
        //private void GameXOX(object obj) => CurrentView = new GameXOXVM();
        public NavigationVM()
        {
            StaticVisableAndEnableElementsOnView.EnamleOnGame = System.Windows.Visibility.Visible;
            StaticVisableAndEnableElementsOnView.EnamleOnLoggingGame = System.Windows.Visibility.Visible;
            StaticVisableAndEnableElementsOnView.EnamleOnGamePage = System.Windows.Visibility.Hidden;
            StaticVisableAndEnableElementsOnView.EnamleOnButtonGame = System.Windows.Visibility.Hidden;
            
            //GameCommandXOX = new RelayCommand(GameXOX);
            //// Startup Page
            CurrentView = StaticVisableAndEnableElementsOnView.Loggin;
            Task.Run(() =>
            {
                if (StaticVisableAndEnableElementsOnView.NonStart == false)
                {
                    LogginCommand = new RelayCommand(Loggin);
                    RegisterCommand = new RelayCommand(Register);
                    GameCommand = new RelayCommand(Game);
                    while (true)
                    {
                        EnableButtnosR_startGame = StaticVisableAndEnableElementsOnView.EnamleOnButtonGame;
                        DesebleREG_Log = StaticVisableAndEnableElementsOnView.EnamleOnLoggingGame;
                        IsEnableView_MDS = StaticVisableAndEnableElementsOnView.EnamleOnGame;
                        IsEnableView_MDSPage = StaticVisableAndEnableElementsOnView.EnamleOnGamePage;
                        if (IsEnableView_MDS == System.Windows.Visibility.Hidden)
                        {
                            CurrentViewGame = new GameXOXVM();
                            IsEnableView_MDSPage = System.Windows.Visibility.Visible;
                            StaticVisableAndEnableElementsOnView.NonStart = true;
                            break;
                        }
                    }
                }
               
            });
        }
    }
}
