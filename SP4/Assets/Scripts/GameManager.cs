﻿using System;
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

    [Tooltip("A reference to the TileMap")]
    public GameTileMap GameTileMapReference;

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
        // Start the music
        SoundManager.PlayBackgroundMusic(SoundManager.BackgroundMusic.Beep_Beepe);

        // Load the the tilemap file
        GameTileMapReference.Load();

        // Set up the objective
        setObjective();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void setObjective()
    {
        // Create an Objective based on the type provided by the TileMap
        switch (GameTileMapReference.ObjectiveType)
        {
            case Objectives.Type.KillAll:
                CurrentObjective = gameObject.AddComponent<Kill_Enemy>();
                break;
            case Objectives.Type.CollectCoins:
                CurrentObjective = gameObject.AddComponent<Collect_Coins>();
                break;
            case Objectives.Type.NoDamage:
                CurrentObjective = gameObject.AddComponent<No_Dmg_Taken>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // Initialize the Objective with the string provided
        if (CurrentObjective.ParseParamString(GameTileMapReference.ObjectiveParams) == false)
        {
            throw new UnityException("Failed to load Objective parameters!");
        }

        // Tie the Objective with this GameManager
        CurrentObjective.Manager = this;
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
