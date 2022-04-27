using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//https://www.dicegamedepot.com/ship-captain-and-crew-dice-game-rules/

namespace Ship_Captain_Crew_Game
{
    internal class Program
    {
        const string YES_INPUT = "y";
        const string NO_INPUT = "n";

        static void Main(string[] args)
        {
            GenerateTestTurn();

            Console.ReadKey();
        }


        private static void GenerateTestTurn()
        {
            Turn turn = new Turn();
            Ship ship = new Ship();

            string playerName = "Player1";

            Console.WriteLine($"{playerName}'s Turn");
            Console.WriteLine();

            while(turn.IsActive)
            {
                if(turn.HasRollsRemaining)
                {
                    //Show how many turns are left.
                    turn.DisplayRollsRemaining();

                    //Roll.
                    AskToRollDiceToGetAllShipFeatures();
                    DiceRoll currentDiceRoll = GenerateDiceRoll(turn.NumDiceAvailable, ref turn.RollsRemaining);

                    //Update ship.
                    ShipUpdateResponse shipUpdate = UpdateShip(ship, currentDiceRoll);

                    //Display ship status.
                    ship.DisplayShipStatus();

                    //If ship was updated. Tell player and reduce dices that can be thrown.
                    if(shipUpdate.FeaturesWereUpdated)
                    {
                        Console.WriteLine(shipUpdate.Message);
                        turn.NumDiceAvailable -= shipUpdate.NumFeaturesUpdated;
                    }

                    if(ship.HasAllShipFeatures)
                    {
                        //Find cargo value based on dice left on "table".
                        ship.SetCargoValue(currentDiceRoll);
                        //Display this value.
                        ship.DisplayCargoValue();
                        //Keep asking if they wish to gamble their cargo value.
                        RepeatedlyAskForRollCargoUpdate(turn,ship);
                    }
                }
                else
                {
                    //Show them cargo value. 
                    if(!ship.HasAllShipFeatures)
                        ship.DisplayCargoValue();

                    turn.IsActive = false;
                }

            }
            
            Console.WriteLine("Turn ended.");
        }

        private static void RepeatedlyAskForRollCargoUpdate(Turn turn,Ship ship)
        {
            while (turn.HasRollsRemaining && turn.IsActive)
            {
                string input = string.Empty;

                while (input != YES_INPUT && input != NO_INPUT)
                {
                    Console.WriteLine("Do you wish to roll to get a better cargo? (y/n)");
                    input = Console.ReadLine().ToLower();
                    Console.WriteLine();

                    if (input == YES_INPUT)
                    {
                        //Roll dice for better cargo score attempt.
                        DiceRoll currentDiceRoll = GenerateDiceRoll(turn.NumDiceAvailable, ref turn.RollsRemaining);
                        ship.SetCargoValue(currentDiceRoll);
                        ship.DisplayCargoValue();
                    }
                    else if (input == NO_INPUT)
                    {
                        turn.IsActive = false;
                    }
                }
            } 
        }


        private static DiceRoll GenerateDiceRoll(int numDice,ref int rollsRemaining)
        {
            DiceRoll diceRoll = new DiceRoll(numDice);

            //Roll Dice.
            diceRoll.Roll();
            diceRoll.DisplayDiceValues();

            rollsRemaining--;

            return diceRoll;
        }
        public static ShipUpdateResponse UpdateShip(Ship ship,DiceRoll diceRoll)
        {
            ShipUpdateResponse updateResponse = ship.UpdateShipFeatures(diceRoll);
            return updateResponse;
        }

        private static void AskToRollDiceToGetAllShipFeatures()
        {
            Console.WriteLine("Enter y to roll dice.");
            string input = string.Empty;
            while (input != YES_INPUT)
            {
                input = Console.ReadLine().ToLower();
            }
        }
    }
}
