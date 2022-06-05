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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for MultiplayersName.xaml
    /// </summary>
    public partial class MultiplayersName : UserControl
    {
        public event EventHandler startMP;

        Grid grid = new Grid();

        MapGenerator mapGenerator = new MapGenerator();
        MultiPlayer1 player1 = new MultiPlayer1();
        MultiPlayer2 player2 = new MultiPlayer2();
        ShowScores showScores = new ShowScores();
        Button[,] player1Buttons = new Button[10, 10];
        Button[,] player2Buttons = new Button[10, 10];
        Button[,] player1ButtonsClickEnable = new Button[10, 10];
        Button[,] player2ButtonsClickEnable = new Button[10, 10];
        List<Player> _data = new List<Player>();

        
        //string workingDirectory = Environment.CurrentDirectory ;
        string AuthenticationFileName = Environment.CurrentDirectory + @"\data.json";

        int destroyer1 = 3;
        int destroyer2 = 3;
        int cruiser1 = 2;
        int cruiser2 = 2;
        int submarine1 = 3;
        int submarine2 = 3;
        int battleship1 = 4;
        int battleship2 = 4;
        int carrier1 = 5;
        int carrier2 = 5;

        bool p1Win = true;
        bool playerTurn = false;

        public MultiplayersName()
        {
            InitializeComponent();
        }

        private void OnStartGame(object sender, RoutedEventArgs e)
        {
            if (HasSpecialChars(player1Name.Text) || HasSpecialChars(player2Name.Text))
            {
                MessageBox.Show("Give me your name witouth a spacial character!");
            }
            else if(player1Name.Text.Equals("") || player2Name.Text.Equals(""))
            {
                MessageBox.Show("Give your name!");
            }else
            {
                StartMultiPlayerGame();
                startMP(this, e);
            }
        }

        public static bool HasSpecialChars(string yourString)
        {
            return yourString.Any(ch => !Char.IsLetterOrDigit(ch));
        }

        public void StartMultiPlayerGame()
        {
            player1.againButton1.Visibility = Visibility.Hidden;
            player2.againButton2.Visibility = Visibility.Hidden;
            player1.exitButton1.Visibility = Visibility.Hidden;
            player2.exitButton2.Visibility = Visibility.Hidden;
            startMP += Player1;
            Content = grid;
            System.Diagnostics.Debug.WriteLine("Multiplayer started");

            mapGenerator.GenerateEmptyMap(player1.player1Canvas, player1Buttons);
            mapGenerator.GenerateEmptyMap(player1.player1CanvasHelper, player2ButtonsClickEnable);
            mapGenerator.GenerateEmptyMap(player2.player2Canvas, player2Buttons);
            mapGenerator.GenerateEmptyMap(player2.player2CanvasHelper, player1ButtonsClickEnable);

            mapGenerator.LoadPlayerShips(player1Buttons, true);
            mapGenerator.LoadPlayerShips(player2Buttons, true);
            mapGenerator.LoadEnemyShips(player1ButtonsClickEnable, player1Buttons);
            mapGenerator.LoadEnemyShips(player2ButtonsClickEnable, player2Buttons);

            GetMPClickedButton();
        }

        private void AfterWin()
        {

            player1.againButton1.Visibility = Visibility.Visible;
            player2.againButton2.Visibility = Visibility.Visible;
            player1.exitButton1.Visibility = Visibility.Visible;
            player2.exitButton2.Visibility = Visibility.Visible;

            player1.againButton1.Click += AgainButton_Click;
            player2.againButton2.Click += AgainButton_Click;

            player1.exitButton1.Click += ExitButton_Click;
            player2.exitButton2.Click += ExitButton_Click;

            if (!File.Exists(AuthenticationFileName))
            {
                File.WriteAllText(AuthenticationFileName, JsonConvert.SerializeObject(_data));
            }

            var jsonData = System.IO.File.ReadAllText(AuthenticationFileName);

            var dat = JsonConvert.DeserializeObject<List<Player>>(jsonData)
                                  ?? new List<Player>();

               //jatekosk hozzadasa
            Player p1 = new Player();
            Player p2 = new Player();

            p1.Name = player1Name.Text;
            p2.Name = player2Name.Text;

            if(p1Win)
            {
                p1.Score += 1;
                p2.Score += 0;
            }
            else
            {
                p1.Score += 0;
                p2.Score += 1;
            }

            if (dat.Exists(n => n.Name == p1.Name))
            {
                if (p1Win)
                {
                    dat.Where(w => w.Name == p1.Name).ToList().ForEach(i => i.Score += 1);
                }

            }
            else
            {
                dat.Add(p1);
            }


            if (dat.Exists(n => n.Name == p2.Name))
            {
                if (!p1Win)
                {
                    dat.Where(w => w.Name == p2.Name).ToList().ForEach(i => i.Score += 1);
                }
            }
            else
            {
                dat.Add(p2);
            }

            // Update json data string
            jsonData = JsonConvert.SerializeObject(dat);
            System.IO.File.WriteAllText(AuthenticationFileName, jsonData);

            
        }


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            grid.Children.Clear();
            grid.Children.Add(showScores);
            var jsonData = System.IO.File.ReadAllText(AuthenticationFileName);

            var Jdata = JsonConvert.DeserializeObject<List<Player>>(jsonData)
                                  ?? new List<Player>();

            foreach(Player asd in Jdata)
            {
                showScores.listB.Items.Add(asd.ToString());
            }
            
        }

        private void AgainButton_Click(object sender, RoutedEventArgs e)
        {
            MultiplayersName repeat = new MultiplayersName();
            Content = grid;
            grid.Children.Clear();
            grid.Children.Add(repeat);
        }

        private void Player1(object? sender, EventArgs e)
        {
            player1.player1Label.Content = player1Name.Text;
            player2.player2 -= Player1;
            grid.Children.Clear();
            grid.Children.Add(player1);
            player1.player1 += Player2;       
        }

        private void Player2(object? sender, EventArgs e)
        {
            player2.player2Label.Content = player2Name.Text;
            player1.player1 -= Player2;
            grid.Children.Clear();
            grid.Children.Add(player2);
            player2.player2 += Player1;     
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
                        button.Background = Brushes.DarkGray;

                        break;
                }

                int x = Int32.Parse(splited[1]);
                int y = Int32.Parse(splited[2]);
                player1Buttons[x, y].Background = button.Background;
                button.Click -= MultiPlayer1_Click;

                if (destroyer1 == 0 && cruiser1 == 0 && submarine1 == 0 && battleship1 == 0 && carrier1 == 0)
                {
                    MessageBoxResult result = MessageBox.Show("You Win");
                    //score1 += 1;
                    AfterWin();

                    p1Win = true;
                }

                System.Diagnostics.Debug.WriteLine(button.Name);
            }
            else
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
                        button.Background = Brushes.DarkGray;

                        break;
                }

                int x = Int32.Parse(splited[1]);
                int y = Int32.Parse(splited[2]);
                player2Buttons[x, y].Background = button.Background;
                button.Click -= MultiPlayer2_Click;

                if (destroyer2 == 0 && cruiser2 == 0 && submarine2 == 0 && battleship2 == 0 && carrier2 == 0)
                {
                    MessageBoxResult result = MessageBox.Show("You Win");
                    p1Win = false;
                    AfterWin();
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

