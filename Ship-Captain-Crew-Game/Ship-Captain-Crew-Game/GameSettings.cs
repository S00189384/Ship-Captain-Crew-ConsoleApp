using System;

namespace Ship_Captain_Crew_Game
{
    //A settings manager class that allows other classes to get access to the current game settings.
    //Only two game settings so far - number of players and bow to stern option.
    //Did not have time to add more.
    public class GameSettings
    {
        private static readonly object padlock = new object();

        public const string ISOLATED_STORAGE_FOLDER_NAME = "SettingsFolder";
        public const string ISOLATED_STORAGE_FILE_NAME = "Settings.txt";

        public const bool BOW_TO_STERN_DEFAULT_SETTING = false;
        public const int MIN_PLAYERS_CAN_PLAY = 3;
        public const int MAX_PLAYERS_CAN_PLAY = 5;

        public const int SHIP_DEFAULT_DICE_VALUE = 6;
        public const int CAPTAIN_DEFAULT_DICE_VALUE = 5;
        public const int CREW_DEFAULT_DICE_VALUE = 4;

        public const int SHIP_BOWTOSTERN_DICE_VALUE = 1;
        public const int CAPTAIN_BOWTOSTERN_DICE_VALUE = 2;
        public const int CREW_BOWTOSTERN_DICE_VALUE = 3;

        public bool BowToStern;
        public int NumPlayers;

        public string GetBowToSternStatus { get { return BowToStern == false ? "disabled" : "enabled"; } } 


        private static GameSettings instance;
        public static GameSettings Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GameSettings();
                    }
                    return instance;
                }
            }
        }
        public GameSettings() { }
        public void SetDefaultSettings()
        {
            instance.BowToStern = BOW_TO_STERN_DEFAULT_SETTING;
            instance.NumPlayers = MIN_PLAYERS_CAN_PLAY;
        }
        public void UpdateSettingsFromJSON(SettingsDTO settingsDTO)
        {
            BowToStern = settingsDTO.BowToStern;
            NumPlayers = settingsDTO.NumPlayers;
        }
    }

    //A settings class which is used to save / read json from isolated storage.
    [Serializable]
    public class SettingsDTO
    {
        public bool BowToStern;
        public int NumPlayers;

        public SettingsDTO(bool bowToStern, int numPlayers)
        {
            BowToStern = bowToStern;
            NumPlayers = numPlayers;
        }
    }
}
