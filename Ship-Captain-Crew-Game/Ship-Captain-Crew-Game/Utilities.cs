using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ship_Captain_Crew_Game
{
    public static class Utilities
    {
        public static int AskForNumberWithinRange(int min,int max)
        {
            int enteredOption = int.MinValue;

            while (enteredOption < min || enteredOption > max)
            {
                bool isNumber = int.TryParse(Console.ReadLine(), out enteredOption);
                if (isNumber && enteredOption >= min && enteredOption <= max)
                    return enteredOption;
                else
                    Console.WriteLine("Incorrect input try again. \n");
            }

            return 0;
        }
    }
}
