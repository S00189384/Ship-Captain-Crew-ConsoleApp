using System;
using System.Collections.Generic;
using System.Linq;

namespace Ship_Captain_Crew_Game
{
    //Class that represents a numbered menu with options.

    public class Menu
    {
        public string Title;
        public Dictionary<int, MenuOption> optionCallbackDict = new Dictionary<int, MenuOption>();

        public Menu(string title,params MenuOption[] menuOptions)
        {
            Title = title;

            for (int i = 0; i < menuOptions.Length; i++)
            {
                optionCallbackDict.Add(i + 1, menuOptions[i]);
            }
        }

        public void DisplayMenu(bool showTitle = false)
        {
            if (showTitle)
                Console.WriteLine(Title + "\n");

            int enteredOption = -1;
            int lowestOption = optionCallbackDict.Keys.ElementAt(0);
            int highestOption = optionCallbackDict.Keys.ElementAt(optionCallbackDict.Count - 1);

            while (enteredOption < lowestOption || enteredOption > highestOption)
            {
                DisplayOptions();

                //Ask user for input.
                Console.WriteLine("Enter option: ");
                int validInput = UserInputManager.AskForNumberWithinRange(lowestOption, highestOption);
                InterpretInput(validInput);
            }
        }

        private void DisplayOptions()
        {
            foreach (KeyValuePair<int, MenuOption> entry in optionCallbackDict)
            {
                Console.WriteLine($"{entry.Key}) {entry.Value.Description}");
            }
        }

        public void InterpretInput(int input)
        {
            optionCallbackDict[input].Callback?.Invoke();
        }
    }

    public struct MenuOption
    {
        public string Description;
        public Action Callback;

        public MenuOption(string description, Action callback)
        {
            Description = description;
            Callback = callback;
        }
    }
}