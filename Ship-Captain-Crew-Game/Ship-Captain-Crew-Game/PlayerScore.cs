using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ship_Captain_Crew_Game
{
    public class PlayerScore
    {
        public string Name;
        public int Score;

        public PlayerScore(int score, string name)
        {
            Score = score;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name} scored {Score}.";
        }
    }
}
