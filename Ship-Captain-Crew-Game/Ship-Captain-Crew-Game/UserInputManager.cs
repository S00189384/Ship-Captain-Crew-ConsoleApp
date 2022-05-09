using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ship_Captain_Crew_Game
{
    //A class that helps with certain user input things.
    //Contains methods to continually ask a user to enter certain values until they enter these values correctly.

    public static class UserInputManager
    {
        public const string YES_INPUT = "y";
        public const string NO_INPUT = "n";

        /// <summary>
        ///  Continually ask user for number between range of min and max inclusive and returns the input.
        /// </summary>
        public static int AskForNumberWithinRange(int min, int max)
        {
            int enteredOption = int.MinValue;

            while (enteredOption < min || enteredOption > max)
            {
                string enteredString = Console.ReadLine();
                bool isNumber = int.TryParse(enteredString, out enteredOption);
                if (isNumber && enteredOption >= min && enteredOption <= max)
                    return enteredOption;
                else
                    Console.WriteLine($"Please enter value between {min} and {max}. \n");
            }

            return enteredOption;
        }

        /// <summary>
        ///  Continually ask user for y/n input and returns the input.
        /// </summary>
        public static bool AskForBooleanValue()
        {
            string enteredString = string.Empty;
            while (enteredString != YES_INPUT || enteredString != NO_INPUT)
            {
                enteredString = Console.ReadLine().ToLower();

                if (enteredString == YES_INPUT)
                    return true;
                else if (enteredString == NO_INPUT)
                    return false;
                else
                    Console.WriteLine("Please enter either y or n. \n");
            }

            return false;
        }
    }
}
