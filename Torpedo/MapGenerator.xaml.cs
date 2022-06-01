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

        List<Ship> GenerateShipPositions(List<Vector> list, int len, int direction, SolidColorBrush shipColor)
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
                    map.Add(new Ship(x, y + i, shipColor));
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
                    map.Add(new Ship(x + i, y, shipColor));
                }
                invalidPositions.AddRange(AddToList(x, y, len, false));
            }
            return map;
        }


        Button[,] buttons = new Button[10, 10];
        char[] c = new char[10] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
        List<Vector> invalidPositions = new List<Vector>();
        public MapGenerator()
        {
            InitializeComponent();

        }

        public void GenerateEmptyMap(Canvas canvas)
        {
            double width, height = 0;

            for (int i = 0; i < 10; i++)
            {
                width = 0;
                for (int j = 0; j < 10; j++)
                {
                    Button btn = new Button();
                    btn.Name = c[j] + (i + 1).ToString();
                    btn.Height = canvas.Height / 10;
                    btn.Width = canvas.Width / 10;
                    btn.Click += GetClickedButton;
                    buttons[i, j] = btn;
                    Canvas.SetLeft(buttons[i, j], width);
                    Canvas.SetTop(buttons[i, j], height);
                    width += canvas.Width / 10;
                    canvas.Children.Add(btn);
                }
                height += 40;

            }

            List<Ship> shipPositions = new List<Ship>();
            Random random = new Random();

            SolidColorBrush shipColor;

            for (int i = 5; i > 0; i--)
            {
                switch (i)
                {
                    case 5:
                        shipColor = Brushes.Green;
                        break;
                    case 4:
                        shipColor = Brushes.ForestGreen;
                        break;
                    case 3:
                        shipColor = Brushes.SeaGreen;
                        break;
                    case 2:
                        shipColor = Brushes.MediumSeaGreen;
                        break;
                    default:
                        shipColor = Brushes.MediumAquamarine;
                        break;
                }

                shipPositions.AddRange(GenerateShipPositions(invalidPositions, i, random.Next(), shipColor));
            }

            foreach (var i in shipPositions)
            {
                buttons[i.x, i.y].Background = i.shipColor;
            }

        }

        private void GetClickedButton(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            System.Diagnostics.Debug.WriteLine(button.Name);
        }
    }
}
