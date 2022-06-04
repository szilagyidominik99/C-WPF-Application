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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Grid grid = new Grid();
        SinglePlayerName singlePlayerName = new SinglePlayerName();
        MultiplayersName multiplayerName = new MultiplayersName();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnStartSinglePlayerMode(object sender, RoutedEventArgs e)
        {
            Content = grid;
            grid.Children.Clear();
            grid.Children.Add(singlePlayerName);

        }

        private void OnStartMultiPlayerMode(object sender, RoutedEventArgs e)
        {
            Content = grid;
            grid.Children.Clear();
            grid.Children.Add(multiplayerName);
        }

    }
}
