using UnityEngine;
using System.Collections;

public class No_Dmg_Taken : Objectives
{
    RPGPlayer player;

    public override bool IsAchieved()
    {
        return (player.Health == player.MaxHealth);
    }

    public override void Complete()
    {

    }
}
