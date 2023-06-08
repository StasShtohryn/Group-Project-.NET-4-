using Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public static class StaticVisableAndEnableElementsOnView
    {
        public static System.Windows.Visibility EnamleOnGame { get; set; }
        public static System.Windows.Visibility EnamleOnGamePage { get; set; }
        public static System.Windows.Visibility EnamleOnLoggingGame { get; set; }
        public static System.Windows.Visibility EnamleOnButtonGame { get; set; }
        public static bool Enablebuttongame { get; set; }
        public static bool NonStart { get; set; }
        public static bool DesableElemet_Loggin_Register { get; set; }
        public static object Loggin { get => _Loggin;}
        public static object Registration { get => _Registration;}
        public static object GameVM { get => _GameVM; }

        private static object _Loggin = new LogginVM();
        private static object _Registration = new Registration();
        private static object _GameVM = new GameVM();

    }
}
