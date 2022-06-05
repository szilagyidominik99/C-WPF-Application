using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo
{
    internal class Scores
    {
        int hits;
        int misses;
        int ships;

        public Scores(int hits, int misses, int ships)
        {
            this.hits = hits;
            this.misses = misses;
            this.ships = ships;
        }

        public override string? ToString()
        {
            return "Hits: " + hits + "\nMisses: " + misses + "\nShips: " + ships;
        }
    }
}
