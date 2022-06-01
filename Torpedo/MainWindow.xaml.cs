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
        
        SelectGameMode gameMode = new SelectGameMode();
        MultiPlayer1 mPlayer1 = new MultiPlayer1();
        MultiPlayer2 mPlayer2 = new MultiPlayer2();
        MapGenerator mapGenerator = new MapGenerator();
        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }

        public void StartGame()
        {
            Content = grid;
            grid.Children.Add(gameMode);
            gameMode.multiplayer += new EventHandler(Player1);
        }

        private void Player1(object? sender, EventArgs e)
        {
            grid.Children.Clear();
            grid.Children.Add(mPlayer1);
            mPlayer1.player1 += new EventHandler(Player2);
        }

        private void Player2(object? sender, EventArgs e)
        {
            grid.Children.Clear();
            grid.Children.Add(mPlayer2);
            mPlayer2.player2 += new EventHandler(Player1);
        }
    }
}
