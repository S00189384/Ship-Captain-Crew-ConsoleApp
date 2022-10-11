using System;
using System.Collections.Generic;
using System.Linq;

namespace Ship_Captain_Crew_Game
{
    //Keeps track of player scores in a round.
    public class GameManager
    {
        private List<PlayerScore> playerScoreList = new List<PlayerScore>();
        private PlayerScore currentHighestScore;

        public GameManager() {  }

        public bool ArePlayersDrawn() => playerScoreList.Count(playerscore => playerscore.Score == currentHighestScore.Score) > 1;
        public List<PlayerScore> GetTiedPlayers() => playerScoreList.FindAll(playerscore => playerscore.Score == currentHighestScore.Score);

        public void AddPlayerScore(PlayerScore newPlayerScore)
        {
            playerScoreList.Add(newPlayerScore);

            if (currentHighestScore == null)
                currentHighestScore = newPlayerScore;

            if (newPlayerScore.Score > currentHighestScore.Score)
                currentHighestScore = newPlayerScore;
        }

        public void DisplayPlayerScores()
        {
            Console.WriteLine("Player Scores");
            foreach (var score in playerScoreList.OrderByDescending(score => score.Score))
            {
                Console.WriteLine(score);
            }
            Console.WriteLine();
        }

        public void DisplayWinningPlayer()
        {
            Console.WriteLine($"The winner is {currentHighestScore.Name}!!!");
        }

        public void DisplayTiedPlayers()
        {
            List<PlayerScore> tiedPlayerList = GetTiedPlayers();
            string tiedPlayersString = string.Empty;

            for (int i = 0; i < tiedPlayerList.Count; i++)
            {
                if(i == tiedPlayerList.Count - 1)
                    tiedPlayersString += $"and {tiedPlayerList[i].Name}.";
                else
                    tiedPlayersString += $"{tiedPlayerList[i].Name}, ";
            }

            Console.Write($"This round ended in a tie between {tiedPlayersString}\n");
        }
    }
}