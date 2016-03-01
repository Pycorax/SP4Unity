using System;
using UnityEngine;
using System.Collections;

public class Kill_Enemy : Objectives
{
    public int RequiredKills = 10;

    protected override void Start()
    {
        base.Start();
        description = "Kill " + RequiredKills + " enemies!";
    }

    public override bool IsAchieved()
    {
        return (Manager.EnemiesKilled >= RequiredKills);
    }

    protected override void finish()
    {
        Debug.Log("Kill_Enemy Complete()");
    }

    protected override bool parseParamString(string[] parameters)
    {
        if (parameters.Length == 1)
        {
            RequiredKills = Convert.ToInt32(parameters[0]);

            return true;
        }

        return false;
    }
}
