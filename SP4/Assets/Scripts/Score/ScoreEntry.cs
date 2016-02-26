public class ScoreEntry
{
    public string Name;
    public int Score;

    public bool IsValid { get { return Name != "" && Score >= 0; } }

    public ScoreEntry(string name = "", int score = -1)
    {
        Name = name;
        Score = score;
    }


}
