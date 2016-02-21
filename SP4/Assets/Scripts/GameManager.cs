using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Check if level has ended
    public bool LevelEnded = false;

    // Objective
    private bool objectiveStarted = false;

	// Use this for initialization
	void Start ()
    {
        HighScoreSystem.Instance.DownloadScores();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
