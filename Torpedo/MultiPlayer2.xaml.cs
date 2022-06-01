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
        public MultiPlayer2()
        {
            InitializeComponent();
            mapGenerator.GenerateEmptyMap(player2Canvas);
            
        }

        private void Player2(object sender, RoutedEventArgs e)
        {
            player2(this, e);
        }
    }
}
