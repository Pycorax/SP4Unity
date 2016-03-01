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
    
    private float elapsedTime = 0.0f;
    private float spawnTimer = 0.0f;

    // Update is called once per frame
    protected override void Update ()
    {
        float dt = (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
        elapsedTime += dt;
        // TODO: Increase difficulty over time
        
        // Counting timer and spawning between intervals
        if (spawnTimer < SpawnInterval)
        {
            spawnTimer += dt;
        }
        else
        {
            // Spawns enemy
            spawn();
            // Reset spawn timer
            spawnTimer = 0.0f;
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

    private void spawn()
    {
        // List of waypoints that enemies can spawn on
        var Waypoints = RefWaypointManager.Waypoints;

        // Loop for spawning multiple enemies
        for (int spawnIndex = 0; spawnIndex < SpawnCount; ++spawnIndex)
        {
            int random = UnityEngine.Random.Range(0, Waypoints.Count - 1);
            spawnSingle(Waypoints[random].transform.position);
        }
    }

    private bool spawnSingle(Vector3 pos)
    {
        GameObject enemy = RefEnemyManager.GetComponent<ResourceManager>().Fetch();
        if (enemy)
        {
            enemy.SetActive(true);
            enemy.transform.position = pos;
            Debug.Log("Spawned");
            return true;
        }
        return false;
    }
}
