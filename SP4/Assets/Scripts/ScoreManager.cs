using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public enum ScoreType
    {
        EnemyKill,
        Destructables,
        CombinedAtk,
        ExitBonus,
    }

    private static int currentScore;
    private static int[] scores =
        {
            100,
            50,
            1,
            200,
        };

    // Saving and Loading
    private static string currentScoreKey = "CurrentScore";

    // Getters
    public static int CurrentScore { get { return currentScore; } }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void AddCurrentScore(ScoreType score)
    {
        currentScore += scores[(int)score];
    }

    public static void ResetCurrentScore()
    {
        currentScore = 0;
    }

    public static void SaveScore()
    {
        PlayerPrefs.SetInt(currentScoreKey, currentScore);
        PlayerPrefs.Save();
    }

    public static void LoadScore()
    {
        if (PlayerPrefs.HasKey(currentScoreKey))
        {
            currentScore = PlayerPrefs.GetInt(currentScoreKey);
        }
        else
        {
            currentScore = 0;
        }
    }
}
