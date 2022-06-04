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
    /// Interaction logic for SinglePlayer.xaml
    /// </summary>
    public partial class SinglePlayer : UserControl
    {

        public event EventHandler singlePlayer;

        int cnt = 0;

        public SinglePlayer()
        {
            InitializeComponent();
            
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S)
            {
                if(cnt % 2 == 0)
                {
                    ShowShips();
                }
                else
                {
                    HideShips();
                }
                cnt++;
            }
        }

        public void ShowShips()
        {
            System.Diagnostics.Debug.WriteLine("Show ships");
            SinglePlayerName.ShowShips();
        }

        public void HideShips()
        {
            System.Diagnostics.Debug.WriteLine("Hide ships");
            SinglePlayerName.HideShips();
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            name.Focus();
        }
    }
}
