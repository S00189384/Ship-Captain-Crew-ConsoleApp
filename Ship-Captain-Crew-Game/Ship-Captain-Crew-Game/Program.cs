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
        [ThreadStatic()]
        private static Turn turn;
        [ThreadStatic()]
        private static Ship ship;

        private static DiceRoll diceRoll;
        private static Menu mainMenu;
        private static Menu settingsMenu;

        private static object locker = new object();

        private const string YES_INPUT = "y";
        private const string NO_INPUT = "n";

        private const int NUM_PLAYERS = 3;
        private static List<Thread> playerThreads = new List<Thread>();

        private static List<PlayerScore> playerScoreList = new List<PlayerScore>();

        static void Main(string[] args)
        {

            int number = Utilities.AskForNumberWithinRange(0, 5);
            Console.WriteLine(number);






            CreateMenus();
            mainMenu.DisplayMenu(showTitle: true);


            //StartGame();

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

                new MenuOption("Select Number of Players", AskToSelectNumberOfPlayers),
                new MenuOption("yyyy", null),
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

        private static void AskToSelectNumberOfPlayers()
        {
            Console.WriteLine($"Enter the number of players to play: (Min {GameData.MIN_PLAYERS_CAN_PLAY}, Max {GameData.MAX_PLAYERS_CAN_PLAY})");
            string input = Console.ReadLine();
        }
        #endregion

        private static void StartGame()
        {
            CreatePlayerThreads();
            playerThreads.ForEach(thread => thread.Start());
            playerThreads.ForEach(thread => thread.Join());
        }

        private static void CreatePlayerThreads() 
        {
            for (int i = 0; i < NUM_PLAYERS; i++)
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
                                //Ask if they wish to gamble their cargo value.
                                //AskForRollCargoUpdate(turn, ship);
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
                    turn.End();
                }
            }
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
            while (input != YES_INPUT)
            {
                input = Console.ReadLine().ToLower();
            }
        }
    }
}