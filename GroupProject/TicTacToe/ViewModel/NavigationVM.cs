using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Utilities;
using System.Windows.Input;
using Client.ViewModel;

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

        public ICommand LogginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand GameCommand { get; set; }

        private void Loggin(object obj) => CurrentView = new LogginVM();
        private void Register(object obj) => CurrentView = new Registration();
        private void Game(object obj) => CurrentView = new GameVM();
        public NavigationVM()
        {
            LogginCommand = new RelayCommand(Loggin);
            RegisterCommand = new RelayCommand(Register);
            GameCommand = new RelayCommand(Game);

            //// Startup Page
            CurrentView = new LogginVM();
        }
    }
}
