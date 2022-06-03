using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for MapGenerator.xaml
    /// </summary>
    public partial class MapGenerator : UserControl
    {
        List<Vector> AddToList(int x, int y, int len, bool isVertical)
        {
            List<Vector> list = new List<Vector>();

            if (isVertical)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= len; j++)
                    {
                        int tmp1;
                        int tmp2;

                        if (x + i < 0 || x + i > 9)
                        {
                            tmp1 = x;
                        }
                        else
                        {
                            tmp1 = x + i;
                        }

                        if (y + j < 0 || y + j > 9)
                        {
                            tmp2 = y;
                        }
                        else
                        {
                            tmp2 = y + j;
                        }

                        list.Add(new Vector(tmp1, tmp2));
                    }
                }
            }
            else
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= len; j++)
                    {
                        int tmp1;
                        int tmp2;

                        if (x + j < 0 || x + j > 9)
                        {
                            tmp1 = x;
                        }
                        else
                        {
                            tmp1 = x + j;
                        }

                        if (y + i < 0 || y + i > 9)
                        {
                            tmp2 = y;
                        }
                        else
                        {
                            tmp2 = y + i;
                        }

                        list.Add(new Vector(tmp1, tmp2));
                    }
                }
            }

            return list;
        }

        List<Ship> GenerateShipPositions(List<Vector> list, int len, int direction, string name)
        {
            Random random = new Random();
            List<Ship> map = new List<Ship>();
            int validPlacementCount;
            int x, y;

            if (direction % 2 == 0) // vertical
            {
                do
                {
                    validPlacementCount = 0;
                    x = random.Next(0, 9);
                    y = random.Next(0, 10 - len);

                    for (int i = 0; i < len; i++)
                    {
                        if (!list.Contains(new Vector(x, y + i)))
                        {
                            validPlacementCount++;
                        }
                    }

                } while (validPlacementCount != len);

                for (int i = 0; i < len; i++)
                {
                    map.Add(new Ship(x, y + i, name));
                }
                invalidPositions.AddRange(AddToList(x, y, len, true));
            }
            else                         // horizontal
            {
                do
                {
                    validPlacementCount = 0;
                    x = random.Next(0, 10 - len);
                    y = random.Next(0, 9);

                    for (int i = 0; i < len; i++)
                    {
                        if (!list.Contains(new Vector(x + i, y)))
                        {
                            validPlacementCount++;
                        }
                    }
                } while (validPlacementCount != len);

                for (int i = 0; i < len; i++)
                {
                    map.Add(new Ship(x + i, y, name));
                }
                invalidPositions.AddRange(AddToList(x, y, len, false));
            }
            return map;
        }



        List<Vector> invalidPositions;
        public MapGenerator()
        {
            InitializeComponent();

        }

        public void LoadPlayerShips(Button[,] buttons, bool isPlayer)
        {
            invalidPositions = new List<Vector>();
            List<Ship> shipPositions = new List<Ship>();
            Random random = new Random();

            string name;
            //  "Destroyer", "Cruiser", "Submarine", "Battleship", "Carrier"
            for (int i = 5; i > 1; i--)
            {
                switch (i)
                {
                    case 5:
                        name = "Carrier";
                        break;
                    case 4:
                        name = "Battleship";
                        break;
                    case 3:
                        name = "Submarine";
                        break;
                    default:
                        name = "Cruiser";
                        break;
                }

                shipPositions.AddRange(GenerateShipPositions(invalidPositions, i, random.Next(), name));
            }

            shipPositions.AddRange(GenerateShipPositions(invalidPositions, 3, random.Next(), "Destroyer"));

            if (isPlayer)
            {
                foreach (var i in shipPositions)
                {
                    buttons[i.x, i.y].Background = Brushes.MediumSeaGreen;
                    buttons[i.x, i.y].Name = "btn_" + i.x + "_" + i.y + "_" + i.name;
                }
            }
            else
            {
                foreach (var i in shipPositions)
                {
                    buttons[i.x, i.y].Name = "btn_" + i.x + "_" + i.y + "_" + i.name;
                    //buttons[i.x, i.y].Background = Brushes.Blue;
                }
            }
        }

        public void LoadEnemyShips(Button[,] player, Button[,] enemy)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    player[i, j].Name = enemy[i, j].Name;
                }
            }
        }

        public void GenerateEmptyMap(Canvas canvas, Button[,] buttons)
        {
            double width, height = 0;

            for (int i = 0; i < 10; i++)
            {
                width = 0;
                for (int j = 0; j < 10; j++)
                {
                    Button btn = new Button();
                    btn.Name = "btn_" + i.ToString() + "_" + j.ToString() + "_Water";
                    btn.Height = canvas.Height / 10;
                    btn.Width = canvas.Width / 10;
                    buttons[i, j] = btn;
                    Canvas.SetLeft(buttons[i, j], width);
                    Canvas.SetTop(buttons[i, j], height);
                    width += canvas.Width / 10;
                    canvas.Children.Add(btn);

                }
                height += 40;

            }

        }

    }
}
