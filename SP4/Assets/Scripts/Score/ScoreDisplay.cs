using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

    private ScoreEntry[] scoreArray;
    private int scoreCount;

    public Text [] DisplayScore = new Text[5];

	// Use this for initialization
	void Start () {

        HighScoreSystem.Instance.ServerAPIAddress = "http://catbang.kahwei.xyz/api/Score";

        //Sending Scores to check

        //HighScoreSystem.Instance.SendScore(new ScoreEntry("Jesus", 4));
        //HighScoreSystem.Instance.SendScore(new ScoreEntry("Bitch", 64));
        //HighScoreSystem.Instance.SendScore(new ScoreEntry("Christ", 24));
        //HighScoreSystem.Instance.SendScore(new ScoreEntry("Darth", 12));
        //HighScoreSystem.Instance.SendScore(new ScoreEntry("Vader", 131));
        HighScoreSystem.Instance.SendScore(new ScoreEntry("My Name is Jogan Cena.", 1369));
        
        //Download Scores
        HighScoreSystem.Instance.DownloadScores();

        scoreCount = HighScoreSystem.Instance.Scores.Count;

        scoreArray = new ScoreEntry[scoreCount];
        
        scoreCount = Mathf.Clamp(scoreCount, 0, 5);
        //Add top 5 scores to scorelist

        HighScoreSystem.Instance.Scores.CopyTo(0, scoreArray, 0, scoreCount);
        

	}
	
	// Update is called once per frame
	void Update () {
        displayScores();
	}

    public void displayScores()
    {
        for(int i = 0; i < scoreCount; i++)
        {
            DisplayScore[i].text = scoreArray[i].Name + "..............." + scoreArray[i].Score.ToString();
        }
    }
}
