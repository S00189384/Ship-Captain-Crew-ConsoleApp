using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

//A class to represent a dice throw on a table. 
//Contains data for the values of each dice and how much dice to be thrown.
//The value of each die thrown is randomly generated.
//Also contains methods to display the dice values that were generated / thrown.

namespace Ship_Captain_Crew_Game
{
    public class DiceRoll
    {
        public int NumberOfDice;
        public List<int> DiceList = new List<int>();

        public DiceRoll(int NumberOfDice)
        {
            this.NumberOfDice = NumberOfDice;
        }

        public List<int> Roll()
        {
            Random rng = new Random();
            for (int i = 0; i < NumberOfDice; i++)
            {
                DiceList.Add(rng.Next(GameData.MIN_DICE_VALUE, GameData.MAX_DICE_VALUE + 1));
                Thread.Sleep(10);
            }

            return DiceList;
        }

        public void DisplayDiceValues()
        {
            Console.WriteLine("Rolling Dice:");

            foreach (var dice in DiceList)
            {
                Thread.Sleep(75);
                Console.Write($"{dice} - ");
            }

            Console.WriteLine();
        }
    }
}
