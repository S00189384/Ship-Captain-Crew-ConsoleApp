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
        private const int DICE_VALUE_PRINT_DELAY = 75;

        public int NumberOfDice;
        public List<int> DiceList = new List<int>();

        public DiceRoll(int NumberOfDice)
        {
            this.NumberOfDice = NumberOfDice;
        }

        public List<int> RollDice()
        {
            for (int i = 0; i < NumberOfDice; i++)
            {
                Dice dice = new Dice();
                Thread.Sleep(10);
                dice.Roll();
                DiceList.Add(dice.Value);
            }

            return DiceList;
        }

        public void DisplayDiceValues()
        {
            Console.WriteLine("Rolling Dice:");

            for (int i = 0; i < NumberOfDice; i++)
            {
                string text = $"{DiceList[i]}";
                if (i < NumberOfDice - 1)
                    text += " - ";

                Console.Write(text);
                Thread.Sleep(DICE_VALUE_PRINT_DELAY);
            }

            Console.WriteLine();
        }
    }
}
