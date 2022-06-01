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

        public string name;

        public Ship(int x, int y, string name)
        {
            this.x = x;
            this.y = y;
            this.name = name;
        }


    }
}
