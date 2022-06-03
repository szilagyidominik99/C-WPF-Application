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
        MultiPlayer1 mPlayer1 = new MultiPlayer1();
        MultiPlayer2 mPlayer2 = new MultiPlayer2();
        SinglePlayer sPlayer = new SinglePlayer();
        MapGenerator mapGenerator = new MapGenerator();
        MultiplayersName multiplayersName = new MultiplayersName();
        SinglePlayerName singlePlayerName = new SinglePlayerName();
        Button[,] player1Buttons = new Button[10, 10];
        Button[,] player2Buttons = new Button[10, 10];
        Button[,] player1ButtonsClickEnable = new Button[10, 10];
        Button[,] player2ButtonsClickEnable = new Button[10, 10];
        List<Vector> invalidPositions = new List<Vector>();
        Vector firstHit = new Vector();
        Vector secondHit = new Vector();

        //  "destroyer", "cruiser", "submarine", "battleship", "carrier"
        //      ^            ^           ^            ^            ^
        //      3            2           3            4            5

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

        int score1 = 0;
        int score2 = 0;

        bool playerTurn = false;
        bool hunterMode = false;
        bool nextHitFlag = false;
        bool computerTurn;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnStartSinglePlayerMode(object sender, RoutedEventArgs e)
        {
            StartSinglePlayerGame();
        }

        private void OnStartMultiPlayerMode(object sender, RoutedEventArgs e)
        {
    
            StartMultiPlayerGame();
        }

        public void StartSinglePlayerGame()
        {
            Content = grid;
            grid.Children.Clear();
            grid.Children.Add(singlePlayerName);
            singlePlayerName.startSP += Player;
            

            mapGenerator.GenerateEmptyMap(sPlayer.playerCanvas, player1Buttons);
            mapGenerator.GenerateEmptyMap(sPlayer.computerCanvas, player2Buttons);

            mapGenerator.LoadPlayerShips(player1Buttons, true);
            mapGenerator.LoadPlayerShips(player2Buttons, false);

            Random random = new Random();

            if (random.Next() % 2 == 0)
            {
                computerTurn = true;
                computerMoves();
            }
            else
            {
                computerTurn = false;
            }

            for (int i = 0; i < 10; i++)
            {
                invalidPositions.Add(new Vector(-1, i));
                invalidPositions.Add(new Vector(i, -1));
                invalidPositions.Add(new Vector(10, i));
                invalidPositions.Add(new Vector(i, 10));
            }

            GetSPClickedButton();
        }

        private void Player(object? sender, EventArgs e)
        {
            grid.Children.Clear();
            grid.Children.Add(sPlayer);
            sPlayer.playerLabel.Content = singlePlayerName.playerName.Text;
        }

        public void StartMultiPlayerGame()
        {
            Content = grid;
            grid.Children.Clear();
            grid.Children.Add(multiplayersName);
            
            multiplayersName.startMP += Player1;

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

       

        private void Player1(object? sender, EventArgs e)
        {
            mPlayer1.player1Label.Content = multiplayersName.player1Name.Text;
            mPlayer2.player2 -= Player1;
            grid.Children.Clear();
            grid.Children.Add(mPlayer1);
            mPlayer1.player1 += Player2;
        }

        private void Player2(object? sender, EventArgs e)
        {
            mPlayer2.player2Label.Content = multiplayersName.player2Name.Text;
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
        void addToInvalidPositions(int x, int y)
        {
            int x1 = x - 1;
            int x2 = x + 1;
            int y1 = y - 1;
            int y2 = y + 1;

            invalidPositions.Add(new Vector(x, y));
            invalidPositions.Add(new Vector(x2, y2));
            invalidPositions.Add(new Vector(x2, y1));
            invalidPositions.Add(new Vector(x1, y1));
            invalidPositions.Add(new Vector(x1, y2));
        }

        private void randomAiMode()
        {
            Random random = new Random();
            int x, y;

            do
            {
                x = random.Next(0, 10);
                y = random.Next(0, 10);
            } while (invalidPositions.Contains(new Vector(x, y)));

            string[] splited = player1Buttons[x, y].Name.Split("_");

            switch (splited[3])
            {
                case "Carrier":
                    carrier1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    System.Diagnostics.Debug.WriteLine("RANDOM HIT: " + player1Buttons[x, y].Name);
                    firstHit = new Vector(x, y);
                    addToInvalidPositions(x, y);
                    hunterMode = true;
                    computerMoves();
                    break;
                case "Battleship":
                    battleship1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    System.Diagnostics.Debug.WriteLine("RANDOM HIT: " + player1Buttons[x, y].Name);
                    firstHit = new Vector(x, y);
                    addToInvalidPositions(x, y);
                    hunterMode = true;
                    computerMoves();
                    break;
                case "Submarine":
                    submarine1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    System.Diagnostics.Debug.WriteLine("RANDOM HIT: " + player1Buttons[x, y].Name);
                    firstHit = new Vector(x, y);
                    addToInvalidPositions(x, y);
                    hunterMode = true;
                    computerMoves();
                    break;
                case "Cruiser":
                    cruiser1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    System.Diagnostics.Debug.WriteLine("RANDOM HIT: " + player1Buttons[x, y].Name);
                    firstHit = new Vector(x, y);
                    addToInvalidPositions(x, y);
                    hunterMode = true;
                    computerMoves();
                    break;
                case "Destroyer":
                    destroyer1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    System.Diagnostics.Debug.WriteLine("RANDOM HIT: " + player1Buttons[x, y].Name);
                    firstHit = new Vector(x, y);
                    addToInvalidPositions(x, y);
                    hunterMode = true;
                    computerMoves();
                    break;
                default:
                    computerTurn = !computerTurn;
                    System.Diagnostics.Debug.WriteLine("RANDOM MISS: " + player1Buttons[x, y].Name);
                    invalidPositions.Add(new Vector(x, y));
                    player1Buttons[x, y].Background = Brushes.DarkGray;
                    break;
            }
        }

        private void hunterAiMode()
        {
            Random random = new Random();
            int x = 0, y = 0;

            if (!nextHitFlag)
            {
                do
                {
                    int direction = random.Next(0, 4);

                    switch (direction)
                    {
                        case 0:
                            secondHit = new Vector(firstHit.X + 1, firstHit.Y);
                            break;
                        case 1:
                            secondHit = new Vector(firstHit.X - 1, firstHit.Y);
                            break;
                        case 2:
                            secondHit = new Vector(firstHit.X, firstHit.Y + 1);
                            break;
                        default:
                            secondHit = new Vector(firstHit.X, firstHit.Y - 1);
                            break;
                    }

                } while (invalidPositions.Contains(secondHit));

                invalidPositions.Add(secondHit);
            }
            else
            {
                if (secondHit.X > firstHit.X)
                    secondHit.X++;
                else if (secondHit.X < firstHit.X)
                    secondHit.X--;
                if (secondHit.Y > firstHit.Y)
                    secondHit.Y++;
                else if (secondHit.Y < firstHit.Y)
                    secondHit.Y--;
            }

            x = (int)(secondHit.X);
            y = (int)(secondHit.Y);

            string[] splited = player1Buttons[x, y].Name.Split("_");

            switch (splited[3])
            {
                case "Carrier":
                    carrier1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    nextHitFlag = true;

                    System.Diagnostics.Debug.WriteLine("AI HIT: " + player1Buttons[x, y].Name);
                    if (carrier1 == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Carrier destroyed ");
                        nextHitFlag = false;
                        hunterMode = false;
                        secondHit = new Vector();
                    }

                    addToInvalidPositions(x, y);
                    computerMoves();
                    break;
                case "Battleship":
                    battleship1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    System.Diagnostics.Debug.WriteLine("AI HIT: " + player1Buttons[x, y].Name);
                    nextHitFlag = true;

                    if (battleship1 == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("battleship destroyed ");
                        nextHitFlag = false;
                        hunterMode = false;
                        secondHit = new Vector();
                    }

                    addToInvalidPositions(x, y);
                    computerMoves();
                    break;
                case "Submarine":
                    submarine1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    System.Diagnostics.Debug.WriteLine("AI HIT: " + player1Buttons[x, y].Name);
                    nextHitFlag = true;

                    if (submarine1 == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Submarine destroyed ");
                        nextHitFlag = false;
                        hunterMode = false;
                        secondHit = new Vector();
                    }

                    addToInvalidPositions(x, y);
                    computerMoves();
                    break;
                case "Cruiser":
                    cruiser1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    System.Diagnostics.Debug.WriteLine("AI HIT: " + player1Buttons[x, y].Name);
                    nextHitFlag = true;

                    if (cruiser1 == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("cruiser destroyed ");
                        nextHitFlag = false;
                        hunterMode = false;
                        secondHit = new Vector();
                    }

                    addToInvalidPositions(x, y);
                    computerMoves();
                    break;
                case "Destroyer":
                    destroyer1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    System.Diagnostics.Debug.WriteLine("AI HIT: " + player1Buttons[x, y].Name);
                    nextHitFlag = true;

                    if (destroyer1 == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("destroyer destroyed ");
                        nextHitFlag = false;
                        hunterMode = false;
                        secondHit = new Vector();
                    }

                    addToInvalidPositions(x, y);
                    computerMoves();
                    break;
                default:
                    computerTurn = !computerTurn;
                    System.Diagnostics.Debug.WriteLine("AI MISS: " + player1Buttons[x, y].Name);
                    nextHitFlag = false;
                    invalidPositions.Add(new Vector(x, y));
                    player1Buttons[x, y].Background = Brushes.DarkGray;
                    break;

            }
        }

        async Task PutTaskDelay(int miliseconds)
        {
            await Task.Delay(miliseconds);
        }

        private async void computerMoves()
        {
            await PutTaskDelay(1000);

            if (hunterMode) // hunting for a hit ship
            {
                hunterAiMode();
            }
            else //random guessing
            {
                randomAiMode();
            }
        }

        private void SinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            if (!computerTurn)
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
                        computerTurn = !computerTurn;
                        computerMoves();
                        button.Background = Brushes.DarkGray;
                        break;
                }

                if (destroyer2 == 0 && cruiser2 == 0 && submarine2 == 0 && battleship2 == 0 && carrier2 == 0)
                {
                    MessageBoxResult result = MessageBox.Show("You Win");
                    score1 += 1;
                    return;
                }

                System.Diagnostics.Debug.WriteLine(button.Name);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Wait for the computer to make his moves!");
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
