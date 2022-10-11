using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ship_Captain_Crew_Game
{
    //Class that determines how well a player scored in a round by holding the data for having a ship, captain, crew and the cargo value.
    //Could not think of a better name - ship is kinda weird in a way since it has the bool "HasShip".
    public class Ship
    {
        public bool HasShip;
        public bool HasCaptain;
        public bool HasCrew;

        public bool HasAllShipFeatures { get { return HasShip && HasCaptain && HasCrew; } }
        public Action<List<int>> UpdateStatusDelegate;
        private Dictionary<string, int> shipCaptainCrewDiceValueDict;

        private int cargoValue;
        public int CargoValue => cargoValue;

        public Ship() 
        { 
            //Determine which dice value represents ship, captain and crew by checking the settings.
            if(GameSettings.Instance.BowToStern)
            {
                shipCaptainCrewDiceValueDict = new Dictionary<string, int>()
                {
                    { GameData.Ship,GameSettings.SHIP_BOWTOSTERN_DICE_VALUE },
                    { GameData.Captain,GameSettings.CAPTAIN_BOWTOSTERN_DICE_VALUE },
                    { GameData.Crew,GameSettings.CREW_BOWTOSTERN_DICE_VALUE },
                };
            }
            else
            {
                shipCaptainCrewDiceValueDict = new Dictionary<string, int>()
                {
                    { GameData.Ship,GameSettings.SHIP_DEFAULT_DICE_VALUE },
                    { GameData.Captain,GameSettings.CAPTAIN_DEFAULT_DICE_VALUE },
                    { GameData.Crew,GameSettings.CREW_DEFAULT_DICE_VALUE },
                };
            } 
        }

        public void SetCargoValue(DiceRoll diceRoll)
        {
            cargoValue = diceRoll.DiceList.OrderByDescending(x => x).Take(2).Sum();
        }

        //Determines which "features" the ship gets from given a dice roll.
        //Must get ship, captain, crew in that order. 
        //Returns a struct that holds information about the ship updating.
        public ShipUpdateResponse UpdateShipFeatures(DiceRoll diceRoll)
        {
            int numUpdatedFeature = 0;
            string returnMessage = string.Empty;

            //Try find Ship.
            int shipValueIndex = diceRoll.DiceList.IndexOf(shipCaptainCrewDiceValueDict[GameData.Ship]);
            if (shipValueIndex != -1 && !HasShip)
            {
                HasShip = true;
                diceRoll.DiceList.RemoveAt(shipValueIndex);

                numUpdatedFeature++;
                returnMessage = $"You found your {GameData.Ship}!";
            }

            //Try find Captain.
            int captainValueIndex = diceRoll.DiceList.IndexOf(shipCaptainCrewDiceValueDict[GameData.Captain]);
            if (captainValueIndex != -1 && HasShip)
            {
                HasCaptain = true;
                diceRoll.DiceList.RemoveAt(captainValueIndex);

                numUpdatedFeature++;
                returnMessage = $"You found your {GameData.Ship} and {GameData.Captain}!";
            }

            //Try find Crew.
            int crewValueIndex = diceRoll.DiceList.IndexOf(shipCaptainCrewDiceValueDict[GameData.Crew]);
            if (crewValueIndex != -1 && HasCaptain)
            {
                HasCrew = true;
                diceRoll.DiceList.RemoveAt(crewValueIndex);

                numUpdatedFeature++;
                returnMessage = $"You found your {GameData.Ship}, {GameData.Captain} and {GameData.Crew}!";
            }

            return new ShipUpdateResponse(returnMessage, numUpdatedFeature);
        }

        public void DisplayShipStatus()
        {
            string tick = ((char)0x221A).ToString();
            string X = "X";

            Console.WriteLine($"Ship : {(HasShip ? tick : X)}  Captain : {(HasCaptain ? tick : X)}  Crew : {(HasCrew ? tick : X)}\n");
        }

        public void DisplayCargoValue()
        {
            Console.WriteLine($"Your cargo value is {cargoValue}.\n");
        }
    }

    public struct ShipUpdateResponse
    {
        public string Message;
        public int NumFeaturesUpdated;
        public bool FeaturesWereUpdated { get { return NumFeaturesUpdated > 0; } }

        public ShipUpdateResponse(string message, int featuresUpdated)
        {
            Message = message;
            NumFeaturesUpdated = featuresUpdated;
        }
    }
}