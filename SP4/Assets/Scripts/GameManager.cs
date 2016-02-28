using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("The current objective assigned to this level.")]
    public Objectives CurrentObjective;

    [Tooltip("A reference to the EnemyManager for tracking.")]
    public EnemyManager EnemiesManager;

    [Tooltip("Reference to a list of Players for tracking.")]
    public List<RPGPlayer> PlayerList;

    // Check if level has ended
    private bool reachedExit = false;

    // Number of Coins Collected
    private int coins;

    //Getter and setter
    public bool LevelEnded { get { return CurrentObjective.IsAchieved(); } }
    public int EnemiesKilled { get { return EnemiesManager.EnemiesKilled; } }
    public bool ReachedExit { get { return reachedExit; } }
    public int CoinsCollected { get { return coins; } }

    // Use this for initialization
    void Start()
    {
        //HighScoreSystem.Instance.DownloadScores();
        SoundManager.PlayBackgroundMusic(SoundManager.BackgroundMusic.Beep_Beepe);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Once the player has reached the exit, call this function to notify
    /// GameManager.
    /// </summary>
    public void NotifyReachedExit()
    {
        reachedExit = true;
    }

    /// <summary>
    /// If the player has left the exit, call this function to notify
    /// GameManager.
    /// </summary>
    public void NotifyLeftExit()
    {
        reachedExit = false;
    }

    public void NotifyCoinCollected(int coinsCollected)
    {
        coins += coinsCollected;
    }
}
