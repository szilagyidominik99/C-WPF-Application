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

        public MapGenerator()
        {
            InitializeComponent();

        }
        public void GenerateShipsbyColor(Canvas canvas, Button[,] buttons)
        { }
        public void GenerateShipsbyButtonName(Canvas canvas, Button[,] buttons)
        { }
        public void GenerateEmptyMap(Canvas canvas, Button[,] buttons)
        {
            double width, height = 0;

            for (int i = 0; i < 10; i++)
            {
                width = 0;
                for (int j = 0; j < 10; j++)
                {
                    Button btn = new Button();
                    btn.Name = "btn_" + i.ToString() + "_" + j.ToString();
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
