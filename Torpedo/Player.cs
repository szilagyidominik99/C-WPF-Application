﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo
{
    internal class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }


        public override string? ToString()
        {
            return "Player:\t" +Name + "\t\t\t Total Wins:\t" + Score;
        }
    }

    
}
