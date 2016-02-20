using UnityEngine;
using System.Collections;

public class Kill_Enemy : Objectives
{
    public int EnemiesToKill = 10;
    RPGPlayer player;
    private int enemiesReset;

    void Awake()
    {
        enemiesReset = player.EnemyKilled + EnemiesToKill;
    }

    public override bool IsAchieved()
    {
        return (player.EnemyKilled >= enemiesReset);
    }

    public override void Complete()
    {

    }
}
