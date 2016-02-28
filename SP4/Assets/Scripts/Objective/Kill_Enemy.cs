using UnityEngine;
using System.Collections;

public class Kill_Enemy : Objectives
{
    public int RequiredKills = 10;

    public override bool IsAchieved()
    {
        return (Manager.EnemiesKilled >= RequiredKills);
    }

    protected override void finish()
    {
        Debug.Log("Kill_Enemy Complete()");
    }
}
