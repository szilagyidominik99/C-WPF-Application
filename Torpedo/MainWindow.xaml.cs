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
        Button[,] player1Buttons = new Button[10, 10];
        Button[,] player2Buttons = new Button[10, 10];
        Button[,] player1ButtonsClickEnable = new Button[10, 10];
        Button[,] player2ButtonsClickEnable = new Button[10, 10];

        public MainWindow()
        {
            InitializeComponent();
            StartGame();
            mapGenerator.GenerateEmptyMap(mPlayer1.player1Canvas, player1Buttons);
            mapGenerator.GenerateEmptyMap(mPlayer1.player1CanvasHelper, player2ButtonsClickEnable);
            mapGenerator.GenerateEmptyMap(mPlayer2.player2Canvas, player2Buttons);
            mapGenerator.GenerateEmptyMap(mPlayer2.player2CanvasHelper, player1ButtonsClickEnable);

            mapGenerator.LoadPlayerShips(player1Buttons);
            mapGenerator.LoadPlayerShips(player2Buttons);
            mapGenerator.LoadEnemyShips(player1ButtonsClickEnable, player1Buttons);
            mapGenerator.LoadEnemyShips(player2ButtonsClickEnable, player2Buttons);

            GetClickedButton();
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

        public void GetClickedButton()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    player1ButtonsClickEnable[i, j].Click += MultiPlayer1_Click;
                    player2ButtonsClickEnable[i, j].Click += MultiPlayer2_Click;
                }
            }
        }

        private void MultiPlayer1_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            System.Diagnostics.Debug.WriteLine(button.Name);
            button.Background = Brushes.Red;
        }

        private void MultiPlayer2_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            System.Diagnostics.Debug.WriteLine(button.Name);
            button.Background = Brushes.Red;
        }
    }
}
