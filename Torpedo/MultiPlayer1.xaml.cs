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
    /// Interaction logic for MultiPlayer1.xaml
    /// </summary>
    public partial class MultiPlayer1 : UserControl
    {
        MapGenerator mapGenerator = new MapGenerator();
        public event EventHandler player1;
        Button[,] player1Buttons = new Button[10,10];
        public MultiPlayer1()
        {
            InitializeComponent();
            mapGenerator.GenerateEmptyMap(player1Canvas,player1Buttons);
            GetClickedButton();
        }

        public void GetClickedButton()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    player1Buttons[i, j].Click += MultiPlayer1_Click;
                }
            }
        }

        private void MultiPlayer1_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            System.Diagnostics.Debug.WriteLine(button.Name);
        }

        private void Player1(object sender, RoutedEventArgs e)
        {
            player1(this, e);
        }
    }
}
