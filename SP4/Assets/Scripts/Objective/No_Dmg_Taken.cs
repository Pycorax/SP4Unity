﻿using UnityEngine;
using System.Collections;

public class No_Dmg_Taken : Objectives
{
    protected override void Start()
    {
        base.Start();
        description = "Do not take any damage!";
    }

    public override bool IsAchieved()
    {
        // Calculate the total health and max health
        int totalMaxHealth = 0;
        int totalHealth = 0;
        foreach (var player in Manager.PlayerList)
        {
            totalMaxHealth += player.MaxHealth;
            totalHealth += player.health;
        }

        // Only trigger the achievement if we have reached the exit and still have max health
        // I know this is buggy if the player regains health but this is a start
        return Manager.ReachedExit && (totalHealth == totalMaxHealth);
    }

    protected override void finish()
    {
    }

    protected override bool parseParamString(string[] parameters)
    {
        return true;
    }
}
