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

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for SelectGameMode.xaml
    /// </summary>
    public partial class SelectGameMode : UserControl
    {
        public event EventHandler multiplayer;
        public event EventHandler singlepalyer;
        public SelectGameMode()
        {
            InitializeComponent();
        }

        private void OnStartSinglePlayerMode(object sender, RoutedEventArgs e)
        {
            singlepalyer(this, e);
        }

        private void OnStartMultiPlayerMode(object sender, RoutedEventArgs e)
        { 
            multiplayer(this, e); 
        }
    }
}
