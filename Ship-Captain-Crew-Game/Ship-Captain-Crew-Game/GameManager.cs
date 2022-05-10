using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ship_Captain_Crew_Game
{
    public class GameManager
    {
        private List<PlayerScore> playerScoreList = new List<PlayerScore>();
        private List<PlayerScore> tiedPlayerList = new List<PlayerScore>();
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
            foreach (var score in playerScoreList)
            {
                Console.WriteLine(score);
            }
        }

        public void DisplayWinningPlayer()
        {
            //Console.WriteLine($"The winner is {currentHighestScore.Name}!!!");
        }
    }
}
