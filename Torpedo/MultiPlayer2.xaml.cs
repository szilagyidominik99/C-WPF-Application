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
    /// Interaction logic for MultiPlayer2.xaml
    /// </summary>
    public partial class MultiPlayer2 : UserControl
    {
        MapGenerator mapGenerator = new MapGenerator();

        public event EventHandler player2;
        public static Button[,] player2Buttons = new Button[10, 10];
        public static Button[,] enemyButtons = new Button[10, 10];
        public MultiPlayer2()
        {
            InitializeComponent();
            mapGenerator.GeneratePlayerMap(player2Canvas, player2Buttons);
            GetClickedButton();
            mapGenerator.GeneratePlayerMap(player2CanvasHelper, enemyButtons);

        }

        public void GetClickedButton()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    player2Buttons[i, j].Click += MultiPlayer2_Click;
                }
            }
        }

        private void MultiPlayer2_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            System.Diagnostics.Debug.WriteLine(button.Name);
        }

        private void Player2(object sender, RoutedEventArgs e)
        {
            player2(this, e);
        }
    }
}