using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//https://www.dicegamedepot.com/ship-captain-and-crew-dice-game-rules/

//Settings to do -
//No. players.
//Bow to stern - 123 - 654
//Dropping anchor - 

namespace Ship_Captain_Crew_Game
{
    internal class Program
    {
        //Data used during each players turn.
        [ThreadStatic()]
        private static Turn turn;
        [ThreadStatic()]
        private static Ship ship;

        private static DiceRoll diceRoll;

        //Menus.
        private static Menu mainMenu;
        private static Menu settingsMenu;

        //Settings.
        private static int numberOfPlayers = GameData.MIN_PLAYERS_CAN_PLAY;
        private static bool bowToStern = GameData.BOW_TO_STERN_DEFAULT_SETTING;

        //Threads.
        private static object locker = new object();
        private static List<Thread> playerThreads = new List<Thread>();

        private static List<PlayerScore> playerScoreList = new List<PlayerScore>();

        static void Main(string[] args)
        {
            //Read settings from Isolated Storage (if any) and save in memory.
            ReadSettings();

            //Create menus and show main menu.
            CreateMenus();
            mainMenu.DisplayMenu(showTitle: true);

            Console.WriteLine("End of main");
            Console.ReadKey();
        }

        private static void CreateMenus()
        {
            mainMenu = new Menu("Welcome to Ship, Captain and Crew!",

                new MenuOption("Play", StartGame),
                new MenuOption("Settings", OnSettingsSelected),
                new MenuOption("Quit", () => Environment.Exit(0)));

            settingsMenu = new Menu("Settings: ",

                new MenuOption("Select Number of Players", OnSelectNumberOfPlayersSelected),
                new MenuOption("Bow to Stern", OnBowToSternSelected),
                new MenuOption("Return", OnReturnFromSettingsSelected));
        }

        #region Menu Options Being Selected
        private static void OnSettingsSelected()
        {
            Console.Clear();
            settingsMenu.DisplayMenu(showTitle: true);
        }
        private static void OnReturnFromSettingsSelected()
        {
            Console.Clear();
            mainMenu.DisplayMenu(showTitle: true);
        }

        private static void OnSelectNumberOfPlayersSelected()
        {
            Console.Clear();
            Console.WriteLine($"Enter the number of players to play: (Min {GameData.MIN_PLAYERS_CAN_PLAY}, Max {GameData.MAX_PLAYERS_CAN_PLAY})");
            numberOfPlayers = UserInputManager.AskForNumberWithinRange(GameData.MIN_PLAYERS_CAN_PLAY, GameData.MAX_PLAYERS_CAN_PLAY);
            Console.Clear();
            //Save to settings.
        }

        private static void OnBowToSternSelected()
        {
            Console.Clear();
            Console.WriteLine("In Bow to Stern, 1, 2, and 3 are the ship, captain and crew instead of 6, 5 and 4.");
            string optionEnabledString = bowToStern == false ? "disabled" : "enabled";
            Console.WriteLine($"This option is currently {optionEnabledString}.");
            Console.WriteLine("Do you want to enable Bow to Stern? (y/n)");
            bowToStern = UserInputManager.AskForBooleanValue();
            Console.Clear();
        }
        #endregion

        private static void ReadSettings()
        {
            //Check isolated storage.




        }



        private static void StartGame()
        {
            Console.Clear();

            CreatePlayerThreads();
            playerThreads.ForEach(thread => thread.Start());
            playerThreads.ForEach(thread => thread.Join());
        }

        private static void CreatePlayerThreads() 
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Thread thread = new Thread(PlayTurn);
                thread.Name = $"Player {i + 1}";

                playerThreads.Add(thread);
            }
        }

        private static void PlayTurn()
        {
            Monitor.Enter(locker);
            try
            {
                turn = new Turn();
                ship = new Ship();

                while (!turn.HasEnded)
                {
                    string currentPlayerName = Thread.CurrentThread.Name;
                    Console.WriteLine($"#########{currentPlayerName}#########\n");

                    if (turn.HasRollsRemaining)
                    {
                        if (ship.HasAllShipFeatures) //Give option to roll if they have all ship features.
                        {
                            AskForRollCargoUpdate(turn, ship);
                        }
                        else //Otherwise ask to roll dice to try and get needed ship features.
                        {
                            //Show how many turns are left.
                            turn.DisplayRollsRemaining();

                            //Roll.
                            AskToRollDiceToGetAllShipFeatures();
                            diceRoll = GenerateDiceRoll(turn.NumDiceAvailable, ref turn.RollsRemaining);

                            //Update ship.
                            ShipUpdateResponse shipUpdate = UpdateShip(ship, diceRoll);

                            //Display ship status.
                            ship.DisplayShipStatus();

                            //If ship was updated. Tell player and reduce dices that can be thrown.
                            if (shipUpdate.FeaturesWereUpdated)
                            {
                                Console.WriteLine(shipUpdate.Message);
                                turn.NumDiceAvailable -= shipUpdate.NumFeaturesUpdated;
                            }

                            if (ship.HasAllShipFeatures)
                            {
                                //Find cargo value based on dice left on "table".
                                ship.SetCargoValue(diceRoll);
                                //Display this value.
                                ship.DisplayCargoValue();
                            }
                        }
                    }
                    else
                    {
                        //Show them cargo value. 
                        if (!ship.HasAllShipFeatures)
                            ship.DisplayCargoValue();

                        playerScoreList.Add(new PlayerScore(ship.CargoValue, currentPlayerName));
                        turn.End();
                    }

                    Monitor.Pulse(locker);
                    Monitor.Wait(locker);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} scored {ship.CargoValue}");
                Console.WriteLine("Turn ended.");
                Monitor.Pulse(locker);
                Monitor.Exit(locker);
            }
        }

        private static void AskForRollCargoUpdate(Turn turn, Ship ship)
        {
            //while (turn.HasRollsRemaining && !turn.HasEnded)

            string input = string.Empty;

            Console.WriteLine("Do you wish to roll to get a better cargo? (y/n)");
            bool userWantsToRollAgain = UserInputManager.AskForBooleanValue();
            if(userWantsToRollAgain)
            {
                //Roll dice for better cargo score attempt.
                DiceRoll currentDiceRoll = GenerateDiceRoll(turn.NumDiceAvailable, ref turn.RollsRemaining);
                ship.SetCargoValue(currentDiceRoll);
                ship.DisplayCargoValue();
            }
            else
                turn.End();


            //while (input != UserInputManager.YES_INPUT && input != UserInputManager.NO_INPUT)
            //{
            //    Console.WriteLine("Do you wish to roll to get a better cargo? (y/n)");
            //    input = Console.ReadLine().ToLower();
            //    Console.WriteLine();

            //    if (input == UserInputManager.YES_INPUT)
            //    {
            //        //Roll dice for better cargo score attempt.
            //        DiceRoll currentDiceRoll = GenerateDiceRoll(turn.NumDiceAvailable, ref turn.RollsRemaining);
            //        ship.SetCargoValue(currentDiceRoll);
            //        ship.DisplayCargoValue();
            //    }
            //    else if (input == UserInputManager.NO_INPUT)
            //    {
            //        turn.End();
            //    }
            //}
        }

        private static DiceRoll GenerateDiceRoll(int numDice,ref int rollsRemaining)
        {
            DiceRoll diceRoll = new DiceRoll(numDice);

            //Roll Dice.
            diceRoll.RollDice();
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
            while (input != UserInputManager.YES_INPUT)
            {
                input = Console.ReadLine().ToLower();
            }
        }
    }
}