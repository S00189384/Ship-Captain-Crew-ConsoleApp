namespace Ship_Captain_Crew_Game
{
    //Player score at end of round.
    public class PlayerScore
    {
        public string Name;
        public int Score;

        public PlayerScore(int score, string name)
        {
            Score = score;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name} : {Score}";
        }
    }
}