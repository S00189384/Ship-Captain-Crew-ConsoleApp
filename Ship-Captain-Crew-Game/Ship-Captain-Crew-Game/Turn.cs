using System;

//A turn is given to a player to allow them to roll dice and get a cargo score.
//Turn class keeps track of whether the turn is active, the amount of rolls remaining in the turn and the number of dice available.

namespace Ship_Captain_Crew_Game
{
    public class Turn
    {
        public bool IsActive;
        public int RollsRemaining;
        public int NumDiceAvailable;

        public bool HasRollsRemaining { get { return RollsRemaining > 0; } }

        public Turn()
        {
            IsActive = true;
            RollsRemaining = GameData.MAX_ROLLS_PER_TURN;
            NumDiceAvailable = GameData.MAX_NUM_DICE;
        }

        public void DisplayRollsRemaining()
        {
            Console.WriteLine($"Rolls remaining: {RollsRemaining}");
        }
    }
}
