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
        List<string> ships;

        public Scores(int hits, int misses, List<string> ships)
        {
            this.hits = hits;
            this.misses = misses;
            this.ships = ships;
        }

        private string WriteShips()
        {
            StringBuilder sb = new StringBuilder();

            foreach (string sh in ships)
            {
                sb.Append(sh + "\n");
            }

            return sb.ToString();
        }

        public override string? ToString()
        {
            return "Hits: " + hits + "\nMisses: " + misses + "\nShips:\n\n" + WriteShips();
        }
    }
}
