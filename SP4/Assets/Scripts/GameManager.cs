using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Check if level has ended
    private bool levelended = false;

    // Objective
    private bool objectivestarted = false;

    //Getter and setter
    public bool LevelEnded { get { return levelended; } set { levelended = value; } }
    public bool ObjeciveStarted { get { return objectivestarted; } set { objectivestarted = value; } }

    // Use this for initialization
    void Start()
    {
        HighScoreSystem.Instance.DownloadScores();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
