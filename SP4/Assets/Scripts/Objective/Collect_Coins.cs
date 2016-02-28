using UnityEngine;
using System.Collections;

public class Collect_Coins : Objectives
{
    [Tooltip("The number of coins required to be collected.")]
    public int RequiredCoins = 50;

    public override bool IsAchieved()
    {
        if (Manager.CoinsCollected >= RequiredCoins)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected override void finish()
    {
        Debug.Log("Collect_Coins Complete()");
    }
}
