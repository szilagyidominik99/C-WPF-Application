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
        SinglePlayer sPlayer = new SinglePlayer();
        MapGenerator mapGenerator = new MapGenerator();
        Button[,] player1Buttons = new Button[10, 10];
        Button[,] player2Buttons = new Button[10, 10];
        Button[,] player1ButtonsClickEnable = new Button[10, 10];
        Button[,] player2ButtonsClickEnable = new Button[10, 10];

        //  "destroyer", "cruiser", "submarine", "battleship", "carrier"
        //      ^            ^           ^            ^            ^
        //      1            2           3            4            5

        int destroyer1 = 1;
        int destroyer2 = 1;
        int cruiser1 = 2;
        int cruiser2 = 2;
        int submarine1 = 3;
        int submarine2 = 3;
        int battleship1 = 4;
        int battleship2 = 4;
        int carrier1 = 5;
        int carrier2 = 5;

        int score1 = 0;
        int score2 = 0;

        bool playerTurn = false;

        public MainWindow()
        {
            InitializeComponent();


            StartMultiPlayerGame();
        }

        public void StartSinglePlayerGame()
        {
            Content = grid;
            grid.Children.Add(gameMode);
            gameMode.singlepalyer += Player;

            mapGenerator.GenerateEmptyMap(sPlayer.playerCanvas, player1Buttons);
            mapGenerator.GenerateEmptyMap(sPlayer.computerCanvas, player2Buttons);

            mapGenerator.LoadPlayerShips(player1Buttons, true);
            mapGenerator.LoadPlayerShips(player2Buttons, false);

            GetSPClickedButton();
        }

        public void StartMultiPlayerGame()
        {
            Content = grid;
            grid.Children.Add(gameMode);
            gameMode.multiplayer += Player1;

            mapGenerator.GenerateEmptyMap(mPlayer1.player1Canvas, player1Buttons);
            mapGenerator.GenerateEmptyMap(mPlayer1.player1CanvasHelper, player2ButtonsClickEnable);
            mapGenerator.GenerateEmptyMap(mPlayer2.player2Canvas, player2Buttons);
            mapGenerator.GenerateEmptyMap(mPlayer2.player2CanvasHelper, player1ButtonsClickEnable);


            mapGenerator.LoadPlayerShips(player1Buttons, true);
            mapGenerator.LoadPlayerShips(player2Buttons, true);
            mapGenerator.LoadEnemyShips(player1ButtonsClickEnable, player1Buttons);
            mapGenerator.LoadEnemyShips(player2ButtonsClickEnable, player2Buttons);

            GetMPClickedButton();
        }

        private void Player(object? sender, EventArgs e)
        {
            grid.Children.Clear();
            grid.Children.Add(sPlayer);
        }

        private void Player1(object? sender, EventArgs e)
        {
            mPlayer2.player2 -= Player1;
            grid.Children.Clear();
            grid.Children.Add(mPlayer1);
            mPlayer1.player1 += Player2;
        }

        private void Player2(object? sender, EventArgs e)
        {
            mPlayer1.player1 -= Player2;
            grid.Children.Clear();
            grid.Children.Add(mPlayer2);
            mPlayer2.player2 += Player1;
        }

        public void GetSPClickedButton()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    player2Buttons[i, j].Click += SinglePlayer_Click;
                }
            }
        }

        public void GetMPClickedButton()
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

        private void SinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            System.Diagnostics.Debug.WriteLine(button.Name);
            button.Background = Brushes.Blue;

        }

        private void MultiPlayer1_Click(object sender, RoutedEventArgs e)
        {
            if (playerTurn)
            {
                Button button = (Button)sender;


                string[] splited = button.Name.ToString().Split("_");
                //  "Destroyer", "Cruiser", "Submarine", "Battleship", "Carrier"

                switch (splited[3])
                {
                    case "Carrier":
                        carrier1--;
                        button.Background = Brushes.Red;
                        if (carrier1 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Carrier destroyed ");
                        }
                        break;
                    case "Battleship":
                        battleship1--;
                        button.Background = Brushes.Red;
                        if (battleship1 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Battleship destroyed ");
                        }
                        break;
                    case "Submarine":
                        submarine1--;
                        button.Background = Brushes.Red;

                        if (submarine1 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Submarine destroyed ");
                        }
                        break;
                    case "Cruiser":
                        cruiser1--;

                        button.Background = Brushes.Red;
                        if (cruiser1 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Cruiser destroyed ");
                        }
                        break;
                    case "Destroyer":
                        destroyer1--;
                        button.Background = Brushes.Red;
                        if (destroyer1 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Destroyer destroyed");
                        }
                        break;
                    default:
                        playerTurn = !playerTurn;
                        button.Background = Brushes.Gray;
                        break;
                }

                int x = Int32.Parse(splited[1]);
                int y = Int32.Parse(splited[2]);
                player1Buttons[x, y].Background = button.Background;
                button.Click -= MultiPlayer1_Click;

                if (destroyer1 == 0 && cruiser1 == 0 && submarine1 == 0 && battleship1 == 0 && carrier1 == 0)
                {
                    MessageBoxResult result = MessageBox.Show("You Win");
                    score1 += 1;
                    return;
                }

                System.Diagnostics.Debug.WriteLine(button.Name);
            }else
            {
                MessageBoxResult result = MessageBox.Show("Other player's turn!");
            }
        }

        private void MultiPlayer2_Click(object sender, RoutedEventArgs e)
        {
            if (!playerTurn)
            {
                Button button = (Button)sender;
                string[] splited = button.Name.ToString().Split("_");
                //  "Destroyer", "Cruiser", "Submarine", "Battleship", "Carrier"

                switch (splited[3])
                {
                    case "Carrier":
                        carrier2--;
                        button.Background = Brushes.Red;
                        if (carrier2 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Carrier destroyed ");
                        }
                        break;
                    case "Battleship":
                        battleship2--;
                        button.Background = Brushes.Red;
                        if (battleship2 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Battleship destroyed ");
                        }
                        break;
                    case "Submarine":
                        submarine2--;
                        button.Background = Brushes.Red;
                        if (submarine2 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Submarine destroyed ");
                        }
                        break;
                    case "Cruiser":
                        cruiser2--;
                        button.Background = Brushes.Red;
                        if (cruiser2 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Cruiser destroyed ");
                        }
                        break;
                    case "Destroyer":
                        destroyer2--;
                        button.Background = Brushes.Red;
                        if (destroyer2 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Destroyer destroyed");
                        }
                        break;
                    default:
                        playerTurn = !playerTurn;
                        button.Background = Brushes.Gray;
                        break;
                }

                int x = Int32.Parse(splited[1]);
                int y = Int32.Parse(splited[2]);
                player2Buttons[x, y].Background = button.Background;
                button.Click -= MultiPlayer2_Click;

                if (destroyer2 == 0 && cruiser2 == 0 && submarine2 == 0 && battleship2 == 0 && carrier2 == 0)
                {
                    MessageBoxResult result = MessageBox.Show("You Win");
                    score2 += 1;
                    return;
                }

                System.Diagnostics.Debug.WriteLine(button.Name);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Other player's turn!");
            }

        }
    }
}
