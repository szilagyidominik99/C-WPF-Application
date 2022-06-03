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
    /// Interaction logic for MultiplayersName.xaml
    /// </summary>
    public partial class MultiplayersName : UserControl
    {
        public event EventHandler startMP;
        public MultiplayersName()
        {
            InitializeComponent();
        }

        private void OnStartGame(object sender, RoutedEventArgs e)
        {
            if (player1Name.Text.Equals("") || player2Name.Text.Equals(""))
            {
                MessageBox.Show("Give your name!");
            }
            else
            {
                startMP(this, e);
            }
            
        }

       
    }
}
