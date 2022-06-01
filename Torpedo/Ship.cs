using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Torpedo
{
    class Ship
    {
        public int x;
        public int y;

        public SolidColorBrush shipColor;

        public Ship(int x, int y, SolidColorBrush shipColor)
        {
            this.x = x;
            this.y = y;
            this.shipColor = shipColor;
        }


    }
}
