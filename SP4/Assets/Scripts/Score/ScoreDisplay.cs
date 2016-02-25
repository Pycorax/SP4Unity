using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreDisplay : MonoBehaviour {

    private SortedList sortedList;

    private List<ScoreEntry> scoreList = new List<ScoreEntry>();


	// Use this for initialization
	void Start () {

        HighScoreSystem.Instance.ServerAPIAddress = "http://catbang.kahwei.xyz/api/Score";

        HighScoreSystem.Instance.DownloadScores();

        foreach (var i in HighScoreSystem.Instance.Scores)
        {
            Debug.Log(i.Name + ", " + i.Score);
        }

        //Add Scores to ScoreList
        //if (HighScoreSystem.Instance.Scores != null)
        //{
        //    scoreList.AddRange(HighScoreSystem.Instance.Scores);
        //}
        //foreach(var i in HighScoreSystem.Instance.Scores)
        //{
        //    scoreList.Add(i);
        //}

        /*                 [[UNTESTED]]
        *Sort the Scores
        *Uses default comparer, which is found in ScoreEntry Class
        *IT SHOULD, NOTE SHOULD, sort by Score
        */
        //scoreList.Sort();

        //foreach (var i in scoreList)
        //{
        //    Debug.Log(i.Name + ", " + i.Score);
        //}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void displayScores()
    {

    }
}
