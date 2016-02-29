using UnityEngine;
using System.Collections;
using System;

public class Endless : Objectives
{
    public RPGPlayer RefPlayer1, RefPlayer2;
    public EnemyManager RefEnemyManager;
    public WaypointManager RefWaypointManager;
    public float SpawnInterval = 2.0f;
    public int SpawnCount = 1;

    // Update is called once per frame
    protected override void Update ()
    {
        // TODO: Spawn enemy with time interval
        GameObject goEnemy = RefEnemyManager.GetComponent<ResourceManager>().Fetch();
        if (goEnemy)
        {
            Enemy.Enemy e = goEnemy.GetComponent<Enemy.Enemy>();
        }
	}

    protected override void finish()
    {
        // NothingMuch to do here
    }

    protected override bool parseParamString(string[] parameters)
    {
        return true;
    }

    public override bool IsAchieved()
    {
        return !RefPlayer1.IsAlive && !RefPlayer2.IsAlive;
    }
}
