using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Check if level has ended
    private bool levelended = false;

    // Objective
    private bool objectiveStarted = false;

    public bool LevelEnded { get { return levelended;} set { levelended = value;} }

	// Use this for initialization
	void Start ()
    {
        HighScoreSystem.Instance.DownloadScores();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
