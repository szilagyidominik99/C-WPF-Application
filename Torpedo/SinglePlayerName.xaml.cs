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

        Vector firstHit = new Vector();
        Vector secondHit = new Vector();
        List<Vector> invalidPositions = new List<Vector>();

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

            Random random = new Random();

            if (random.Next() % 2 == 0)
            {
                computerTurn = true;
                MessageBoxResult result = MessageBox.Show("Computer starts!");
                computerMoves();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("You start!");
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
                    //computerMoves();
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

            if (invalidPositions.Contains(new Vector(x, y)))
            {
                if (x == 10)
                {
                    secondHit.X = firstHit.X - 1;
                }
                if (y == 10)
                {
                    secondHit.Y = firstHit.Y - 1;
                }
                if (x == -1)
                {
                    secondHit.X = firstHit.X + 1;
                }
                if (y == -1)
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

        async Task PutTaskDelay(int miliseconds)
        {
            await Task.Delay(miliseconds);
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
                    AfterWin();
                }

                System.Diagnostics.Debug.WriteLine(button.Name);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Wait for the computer to make his moves!");
            }
        }

        public static void ShowShips()
        {
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    string[] splited = player2Buttons[i, j].Name.Split("_");

                    if(splited[3] != "Water" && player2Buttons[i, j].Background != Brushes.Red)
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
