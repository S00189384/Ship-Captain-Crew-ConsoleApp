using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ship_Captain_Crew_Game
{
    public class Dice
    {
        public const int MIN_DICE_VALUE = 1;
        public const int MAX_DICE_VALUE = 6;

        private int value;
        public int Value => value; 

        public int Roll()
        {
            Random rng = new Random();
            value = rng.Next(MIN_DICE_VALUE, MAX_DICE_VALUE + 1);
            return value;
        }
    }
}
