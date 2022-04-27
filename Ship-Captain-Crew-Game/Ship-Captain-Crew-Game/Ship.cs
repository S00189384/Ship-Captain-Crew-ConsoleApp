using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ship_Captain_Crew_Game
{
    public class Ship
    {
        public bool HasShip;
        public bool HasCaptain;
        public bool HasCrew;

        public bool HasAllShipFeatures { get { return HasShip && HasCaptain && HasCrew; } }
        public Action<List<int>> UpdateStatusDelegate;

        private int cargoValue;
        public int CargoValue => cargoValue;

        public Ship() { }

        public void SetCargoValue(DiceRoll diceRoll)
        {
            cargoValue = diceRoll.DiceList.OrderByDescending(x => x).Take(2).Sum();
        }

        public ShipUpdateResponse UpdateShipFeatures(DiceRoll diceRoll)
        {
            int numUpdatedFeature = 0;
            string returnMessage = string.Empty;

            //Try find Ship Value.
            int shipValueIndex = diceRoll.DiceList.IndexOf(GameData.SHIP_DICE_VALUE);
            if (shipValueIndex != -1 && !HasShip)
            {
                HasShip = true;
                diceRoll.DiceList.RemoveAt(shipValueIndex);

                numUpdatedFeature++;
                returnMessage += $"You found your {GameData.Ship}";
            }

            //Try find Captain Value.
            int captainValueIndex = diceRoll.DiceList.IndexOf(GameData.CAPTAIN_DICE_VALUE);
            if (captainValueIndex != -1 && HasShip)
            {
                HasCaptain = true;
                diceRoll.DiceList.RemoveAt(captainValueIndex);

                numUpdatedFeature++;
                returnMessage += returnMessage == string.Empty ? $"You found your {GameData.Captain}" : $", {GameData.Captain}";
            }

            //Try find Crew Value.
            int crewValueIndex = diceRoll.DiceList.IndexOf(GameData.CREW_DICE_VALUE);
            if (crewValueIndex != -1 && HasCaptain)
            {
                HasCrew = true;
                diceRoll.DiceList.RemoveAt(crewValueIndex);

                numUpdatedFeature++;
                returnMessage += returnMessage == string.Empty ? $"You found your {GameData.Crew}" : $" and {GameData.Crew}";
            }

            if (returnMessage != string.Empty)
                returnMessage += "!";

            return new ShipUpdateResponse(returnMessage, numUpdatedFeature);
        }

        public void DisplayShipStatus()
        {
            string tick = ((char)0x221A).ToString();
            string X = "X";

            Console.WriteLine($"Ship : {(HasShip ? tick : X)}  Captain : {(HasCaptain ? tick : X)}  Crew : {(HasCrew ? tick : X)}");
            Console.WriteLine();
        }

        public void DisplayCargoValue()
        {
            Console.WriteLine($"Your cargo value is {cargoValue}.");
            Console.WriteLine();
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
