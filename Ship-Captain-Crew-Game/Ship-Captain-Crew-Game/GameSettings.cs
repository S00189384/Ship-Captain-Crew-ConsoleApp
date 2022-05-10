using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ship_Captain_Crew_Game
{
    [Serializable]
    public class GameSettings
    {
        private static readonly object padlock = new object();
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
        public void UpdateSettings(bool bowToStern,int numPlayers)
        {
            BowToStern = bowToStern;
            NumPlayers = numPlayers;
        }
        public void UpdateSettingsFromJSON(SettingsDTO settingsDTO)
        {
            BowToStern = settingsDTO.BowToStern;
            NumPlayers = settingsDTO.NumPlayers;
        }


        public const string ISOLATED_STORAGE_FOLDER_NAME = "SettingsFolder";
        public const string ISOLATED_STORAGE_FILE_NAME = "Settings.txt";

        public const bool BOW_TO_STERN_DEFAULT_SETTING = false;
        public const int MIN_PLAYERS_CAN_PLAY = 2;
        public const int MAX_PLAYERS_CAN_PLAY = 4;

        public bool BowToStern;
        public int NumPlayers;
    }

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
