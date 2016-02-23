using UnityEngine;
using System.Collections;

public class No_Dmg_Taken : Objectives
{

    public override bool IsAchieved()
    {
        if(player.Health == player.MaxHealth)
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
        Debug.Log("No_Dmg_Taken Complete()");
    }
}
