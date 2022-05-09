using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ship_Captain_Crew_Game
{
    public class MainMenu
    {
        private Action playGameCallback;

        public MainMenu(Action playGameCallback) 
        {
            this.playGameCallback = playGameCallback;
        }

        public void Enable()
        {
            DisplayMenuTitle();
            ReadMainMenuInput();
        }

        private void DisplayMenuTitle() => Console.WriteLine("Welcome to Ship, Captain and Crew! \n");
        private void DisplaySettingsTitle() => Console.WriteLine("Settings: \n");
        private void DisplayGameOptions()
        {
            Console.WriteLine("1: Play game");
            Console.WriteLine("2: Settings");
            Console.WriteLine("3: Quit \n");
            Console.Write("Enter option: ");
        }
        private void DisplaySettingsOptions()
        {
            DisplaySettingsTitle();
            Console.WriteLine("1: Number of players");
            Console.WriteLine("2: ");
            Console.WriteLine("3: Return to main menu \n");
            Console.WriteLine("4: Quit \n");
            Console.Write("Enter option: ");
        }

        private void ReadMainMenuInput()
        {
            int option = -1;
            while (option < 0 || option > 3)
            {
                DisplayGameOptions();
                bool isNumber = int.TryParse(Console.ReadLine(), out option);
                if (isNumber && option > 0 && option < 4)
                    InterpretMenuOptionInput(option);
                else
                {
                    Console.WriteLine("Incorrect input try again.");
                }
            }
        }

        private void InterpretMenuOptionInput(int option)
        {
            switch (option)
            {
                case (1):
                    playGameCallback();
                    break;
                case (2):
                    DisplaySettingsOptions();
                    break;
                case (3):
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }
}
