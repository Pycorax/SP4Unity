using UnityEngine;
using System.Collections;
using System;

public class Endless : Objectives
{
    [Tooltip("References to both players to determine the end of objective")]
    public RPGPlayer RefPlayer1, RefPlayer2;
    public EnemyManager RefEnemyManager;
    public WaypointManager RefWaypointManager;
    public float SpawnInterval = 2.0f;
    public float TimeTillDifficultyIncrease = 5.0f;
    public int InitialSpawnCount = 1;

    private int spawnCount;
    private float elapsedTime = 0.0f;
    private float spawnTimer = 0.0f;

    protected override void Start()
    {
        base.Start();
        spawnCount = InitialSpawnCount;
        Description = "Survive as long as possible!";
    }

    // Update is called once per frame
    protected override void Update ()
    {
        if (IsAchieved())
        {
            // TODO: Call Manager to end game immediately
        }

        float dt = (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
        elapsedTime += dt;
        // TODO: Increase difficulty over time
        spawnCount = (int)(elapsedTime % TimeTillDifficultyIncrease) + InitialSpawnCount;

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
        for (int spawnIndex = 0; spawnIndex < spawnCount; ++spawnIndex)
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
            return true;
        }
        return false;
    }
}
