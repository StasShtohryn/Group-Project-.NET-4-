using Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.View
{
    /// <summary>
    /// Логика взаимодействия для GameXOX.xaml
    /// </summary>
    public partial class GameXOX : UserControl
    {
        public GameXOX()
        {
            InitializeComponent();
            //DataContext = new GameXOX();
            //StaticVisableAndEnableElementsOnView.CorrectOpenWindow = DataContext;
        }
    }
}
