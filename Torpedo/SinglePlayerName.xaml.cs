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
    /// Interaction logic for SinglePlayerName.xaml
    /// </summary>
    public partial class SinglePlayerName : UserControl
    {
        public event EventHandler startSP;

        Grid grid = new Grid();

        MapGenerator mapGenerator = new MapGenerator();
        SinglePlayer singlePlayer = new SinglePlayer();

        static Button[,] player1Buttons = new Button[10, 10];
        static Button[,] player2Buttons = new Button[10, 10];
        List<string> pShips = new List<string>();
        List<string> cShips = new List<string>();

        Vector firstHit = new Vector();
        Vector secondHit = new Vector();
        List<Vector> invalidPositions = new List<Vector>();



        Scores pScores;
        Scores cScores;

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
        int pHits = 0;
        int pMisses = 0;
        int cHits = 0;
        int cMisses = 0;

        bool hunterMode = false;
        bool nextHitFlag = false;
        bool computerTurn;

        public SinglePlayerName()
        {
            InitializeComponent();
        }

        private void OnStartGame(object sender, RoutedEventArgs e)
        {
            if (playerName.Text.Equals(""))
            {
                MessageBox.Show("Give your name!");
            }
            else if (MultiplayersName.HasSpecialChars(playerName.Text))
            {
                MessageBox.Show("Give me your name witouth a spacial character!");
            }
            else
            {
                StartSinglePlayerGame();
                startSP(this, e);
            }
        }

        private void Player(object? sender, EventArgs e)
        {
            grid.Children.Clear();
            grid.Children.Add(singlePlayer);
            singlePlayer.playerLabel.Content = playerName.Text;
        }

        private void CalcScores()
        {
            pScores = new Scores(pHits, pMisses, pShips);
            cScores = new Scores(cHits, cMisses, cShips);
            singlePlayer.playerScores.Content = pScores.ToString();
            singlePlayer.computerScores.Content = cScores.ToString();
        }

        public async void StartSinglePlayerGame()
        {
            startSP += Player;
            Content = grid;
            System.Diagnostics.Debug.WriteLine("Singleplayer started");

            mapGenerator.GenerateEmptyMap(singlePlayer.playerCanvas, player1Buttons);
            mapGenerator.GenerateEmptyMap(singlePlayer.computerCanvas, player2Buttons);
            mapGenerator.LoadPlayerShips(player1Buttons, true);
            mapGenerator.LoadPlayerShips(player2Buttons, false);

            await PutTaskDelay(200);

            pShips.Add("Carrier");
            pShips.Add("Battleship");
            pShips.Add("Cruiser");
            pShips.Add("Submarine");
            pShips.Add("Destroyer");

            cShips.Add("Carrier");
            cShips.Add("Battleship");
            cShips.Add("Cruiser");
            cShips.Add("Submarine");
            cShips.Add("Destroyer");

            Scores pScores = new Scores(0, 0, pShips);
            Scores cScores = new Scores(0, 0, cShips);

            CalcScores();

            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                invalidPositions.Add(new Vector(-1, i));
                invalidPositions.Add(new Vector(i, -1));
                invalidPositions.Add(new Vector(10, i));
                invalidPositions.Add(new Vector(i, 10));
            }

            if (random.Next() % 2 == 0)
            {
                computerTurn = true;
                MessageBoxResult result = MessageBox.Show("Computer starts!");
                ComputerMoves();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("You start!");
                computerTurn = false;
            }

            GetSPClickedButton();
        }

        public void AfterWin()
        {
            SinglePlayerName repeat = new SinglePlayerName();
            Content = grid;
            grid.Children.Clear();
            grid.Children.Add(repeat);
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

        void AddToInvalidPositions(int x, int y)
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

        private void RandomAiMode()
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
                    cHits++;
                    CalcScores();
                    System.Diagnostics.Debug.WriteLine("RANDOM HIT: " + player1Buttons[x, y].Name);
                    firstHit = new Vector(x, y);
                    AddToInvalidPositions(x, y);
                    hunterMode = true;
                    ComputerMoves();

                    break;
                case "Battleship":
                    battleship1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    cHits++;
                    CalcScores();
                    System.Diagnostics.Debug.WriteLine("RANDOM HIT: " + player1Buttons[x, y].Name);
                    firstHit = new Vector(x, y);
                    AddToInvalidPositions(x, y);
                    hunterMode = true;
                    ComputerMoves();

                    break;
                case "Submarine":
                    submarine1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    cHits++;
                    CalcScores();
                    System.Diagnostics.Debug.WriteLine("RANDOM HIT: " + player1Buttons[x, y].Name);
                    firstHit = new Vector(x, y);
                    AddToInvalidPositions(x, y);
                    hunterMode = true;
                    ComputerMoves();

                    break;
                case "Cruiser":
                    cruiser1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    cHits++;
                    CalcScores();
                    System.Diagnostics.Debug.WriteLine("RANDOM HIT: " + player1Buttons[x, y].Name);
                    firstHit = new Vector(x, y);
                    AddToInvalidPositions(x, y);
                    hunterMode = true;
                    ComputerMoves();

                    break;
                case "Destroyer":
                    destroyer1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    cHits++;
                    CalcScores();
                    System.Diagnostics.Debug.WriteLine("RANDOM HIT: " + player1Buttons[x, y].Name);
                    firstHit = new Vector(x, y);
                    AddToInvalidPositions(x, y);
                    hunterMode = true;
                    ComputerMoves();

                    break;
                default:
                    computerTurn = !computerTurn;
                    cMisses++;
                    CalcScores();
                    System.Diagnostics.Debug.WriteLine("RANDOM MISS: " + player1Buttons[x, y].Name);
                    player1Buttons[x, y].Background = Brushes.DarkGray;
                    invalidPositions.Add(new Vector(x, y));
                    //computerMoves();

                    break;
            }
        }

        private void HunterAiMode()
        {
            Random random = new Random();
            int x, y;

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
            }
            else
            {
                if (secondHit.X > firstHit.X)
                {
                    secondHit.X++;
                }
                else if (secondHit.X < firstHit.X)
                {
                    secondHit.X--;
                }

                if (secondHit.Y > firstHit.Y)
                {
                    secondHit.Y++;
                }
                else if (secondHit.Y < firstHit.Y)
                {
                    secondHit.Y--;
                }
            }

            x = (int)(secondHit.X);
            y = (int)(secondHit.Y);

            if (invalidPositions.Contains(new Vector(x, y)))
            {
                if (firstHit.X < secondHit.X)
                {
                    secondHit.X = firstHit.X - 1;
                }
                else if (firstHit.X > secondHit.X)
                {
                    secondHit.X = firstHit.X + 1;
                }

                if (firstHit.Y < secondHit.Y)
                {
                    secondHit.Y = firstHit.Y - 1;
                }
                else if (firstHit.Y > secondHit.Y)
                {
                    secondHit.Y = firstHit.Y + 1;
                }

                x = (int)(secondHit.X);
                y = (int)(secondHit.Y);
            }

            string[] splited = player1Buttons[x, y].Name.Split("_");

            switch (splited[3])
            {
                case "Carrier":
                    carrier1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    cHits++;
                    nextHitFlag = true;
                    invalidPositions.Add(new Vector(x, y));
                    System.Diagnostics.Debug.WriteLine("AI HIT: " + player1Buttons[x, y].Name);

                    if (carrier1 == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Your Carrier is destroyed ");
                        pShips.Remove("Carrier");
                        nextHitFlag = false;
                        hunterMode = false;
                        secondHit = new Vector();
                    }

                    CalcScores();
                    AddToInvalidPositions(x, y);
                    ComputerMoves();

                    break;
                case "Battleship":
                    battleship1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    cHits++;
                    nextHitFlag = true;
                    invalidPositions.Add(new Vector(x, y));
                    System.Diagnostics.Debug.WriteLine("AI HIT: " + player1Buttons[x, y].Name);

                    if (battleship1 == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Your Battleship is destroyed ");
                        pShips.Remove("Battleship");
                        nextHitFlag = false;
                        hunterMode = false;
                        secondHit = new Vector();
                    }

                    CalcScores();
                    AddToInvalidPositions(x, y);
                    ComputerMoves();

                    break;
                case "Submarine":
                    submarine1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    cHits++;
                    nextHitFlag = true;
                    invalidPositions.Add(new Vector(x, y));
                    System.Diagnostics.Debug.WriteLine("AI HIT: " + player1Buttons[x, y].Name);

                    if (submarine1 == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Your Submarine is destroyed ");
                        pShips.Remove("Submarine");
                        nextHitFlag = false;
                        hunterMode = false;
                        secondHit = new Vector();
                    }

                    CalcScores();
                    AddToInvalidPositions(x, y);
                    ComputerMoves();

                    break;
                case "Cruiser":
                    cruiser1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    cHits++;
                    nextHitFlag = true;
                    invalidPositions.Add(new Vector(x, y));
                    System.Diagnostics.Debug.WriteLine("AI HIT: " + player1Buttons[x, y].Name);

                    if (cruiser1 == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Your Cruiser is destroyed ");
                        pShips.Remove("Cruiser");
                        nextHitFlag = false;
                        hunterMode = false;
                        secondHit = new Vector();
                    }

                    CalcScores();
                    AddToInvalidPositions(x, y);
                    ComputerMoves();

                    break;
                case "Destroyer":
                    destroyer1--;
                    player1Buttons[x, y].Background = Brushes.Red;
                    cHits++;
                    nextHitFlag = true;
                    invalidPositions.Add(new Vector(x, y));
                    System.Diagnostics.Debug.WriteLine("AI HIT: " + player1Buttons[x, y].Name);

                    if (destroyer1 == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Your Destroyer is destroyed ");
                        pShips.Remove("Destroyer");
                        nextHitFlag = false;
                        hunterMode = false;
                        secondHit = new Vector();
                    }

                    CalcScores();
                    AddToInvalidPositions(x, y);
                    ComputerMoves();

                    break;
                default:
                    computerTurn = !computerTurn;
                    cMisses++;
                    CalcScores();
                    nextHitFlag = false;
                    invalidPositions.Add(new Vector(x, y));
                    player1Buttons[x, y].Background = Brushes.DarkGray;
                    System.Diagnostics.Debug.WriteLine("AI MISS: " + player1Buttons[x, y].Name);
                    //computerMoves();

                    break;
            }
            if (destroyer1 == 0 && cruiser1 == 0 && submarine1 == 0 && battleship1 == 0 && carrier1 == 0)
            {
                MessageBoxResult result = MessageBox.Show("You lost!");
                AfterWin();

                return;
            }
        }

        private async void ComputerMoves()
        {
            await PutTaskDelay(1000);

            if (hunterMode) // hunting for a hit ship
            {
                HunterAiMode();
            }
            else //random guessing
            {
                RandomAiMode();
            }

        }

        async Task PutTaskDelay(int miliseconds)
        {
            await Task.Delay(miliseconds);
        }

        private void SinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (!computerTurn)
            {
                string[] splited = button.Name.ToString().Split("_");
                //  "Destroyer", "Cruiser", "Submarine", "Battleship", "Carrier"

                switch (splited[3])
                {
                    case "Carrier":
                        carrier2--;
                        button.Background = Brushes.Red;
                        pHits++;

                        if (carrier2 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Enemy Carrier destroyed");
                            cShips.Remove("Carrier");
                        }

                        CalcScores();

                        break;
                    case "Battleship":
                        battleship2--;
                        button.Background = Brushes.Red;
                        pHits++;

                        if (battleship2 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Enemy Battleship destroyed ");
                            cShips.Remove("Battleship");
                        }

                        CalcScores();

                        break;
                    case "Submarine":
                        submarine2--;
                        button.Background = Brushes.Red;
                        pHits++;

                        if (submarine2 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Enemy Submarine destroyed ");
                            cShips.Remove("Submarine");
                        }

                        CalcScores();

                        break;
                    case "Cruiser":
                        cruiser2--;
                        button.Background = Brushes.Red;
                        pHits++;

                        if (cruiser2 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Enemy Cruiser destroyed ");
                            cShips.Remove("Cruiser");
                        }

                        CalcScores();

                        break;
                    case "Destroyer":
                        destroyer2--;
                        button.Background = Brushes.Red;
                        pHits++;

                        if (destroyer2 == 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Enemy Destroyer destroyed");
                            cShips.Remove("Destroyer");
                        }

                        CalcScores();

                        break;
                    default:
                        pMisses++;
                        computerTurn = !computerTurn;
                        button.Background = Brushes.DarkGray;
                        CalcScores();
                        ComputerMoves();

                        break;
                }

                if (destroyer2 == 0 && cruiser2 == 0 && submarine2 == 0 && battleship2 == 0 && carrier2 == 0)
                {
                    MessageBoxResult result = MessageBox.Show("You Win");
                    AfterWin();
                }

                System.Diagnostics.Debug.WriteLine(button.Name);
                button.Click -= SinglePlayer_Click;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Wait for the computer to make his moves!");
            }
        }

        public static void ShowShips()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    string[] splited = player2Buttons[i, j].Name.Split("_");

                    if (splited[3] != "Water" && player2Buttons[i, j].Background != Brushes.Red)
                    {
                        player2Buttons[i, j].Background = Brushes.Blue;
                    }
                }
            }
        }

        public static void HideShips()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (player2Buttons[i, j].Background == Brushes.Blue)
                    {
                        player2Buttons[i, j].Background = Brushes.LightGray;
                    }
                }
            }
        }

    }
}
