using UnityEngine;
using System.Collections;

public class Kill_Enemy : Objectives
{
    private int enemies = 0;
    private int Enemiesneeded = 0;
    public int requiredEnemies = 10;

    public override void Start()
    {
        Enemiesneeded = player.EnemyKilled + requiredEnemies;
    }

    public override bool IsAchieved()
    {
        if(enemies >= Enemiesneeded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Complete()
    {
        Debug.Log("Kill_Enemy Complete()");
    }

    public override void Update()
    {
        enemies = player.EnemyKilled;
    }
}
