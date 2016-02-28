using System;
using UnityEngine;
using System.Collections;

public class Collect_Coins : Objectives
{
    [Tooltip("The number of coins required to be collected.")]
    public int RequiredCoins = 50;

    public override bool IsAchieved()
    {
        return (Manager.CoinsCollected >= RequiredCoins);
    }

    protected override void finish()
    {
        Debug.Log("Collect_Coins Complete()");
    }

    protected override bool parseParamString(string[] parameters)
    {
        if (parameters.Length == 1)
        {
            RequiredCoins = Convert.ToInt32(parameters[0]);

            return true;
        }

        return false;
    }
}
