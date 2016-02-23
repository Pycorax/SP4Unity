using UnityEngine;

public class ScoreTest : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	    HighScoreSystem.Instance.ServerAPIAddress = "http://catbang.kahwei.xyz/api/Score";
        //HighScoreSystem.Instance.SendScore(new ScoreEntry("Test", 4));
        //HighScoreSystem.Instance.SendScore(new ScoreEntry("Test", 64));
        //HighScoreSystem.Instance.SendScore(new ScoreEntry("Test", 24));
        HighScoreSystem.Instance.DownloadScores();
        foreach (var i in HighScoreSystem.Instance.Scores)
        {
            Debug.Log(i.Name + ": " + i.Score);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
