//A class to hold data about the game that will not change.
//Can be accessed from any class.

namespace Ship_Captain_Crew_Game
{
    public static class GameData
    {
        public const int NUM_PLAYERS = 3;
        public const int MAX_NUM_DICE = 5;
        public const int MAX_ROLLS_PER_TURN = 3;
        public const int MAX_TURNS = 3;
        public const int MIN_DICE_VALUE = 1;
        public const int MAX_DICE_VALUE = 6;

        public const int SHIP_DICE_VALUE = 6;
        public const int CAPTAIN_DICE_VALUE = 5;
        public const int CREW_DICE_VALUE = 4;

        public const string Ship = "Ship";
        public const string Captain = "Captain";
        public const string Crew = "Crew";

        //public static Dictionary<string, int> DiceValueShipFeatureDict = new Dictionary<string, int>() { { Ship, 6 }, { Captain, 5 }, { Crew, 4 } };
    }
}
