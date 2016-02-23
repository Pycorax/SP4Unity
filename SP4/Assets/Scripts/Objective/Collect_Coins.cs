using UnityEngine;
using System.Collections;

public class Collect_Coins : Objectives
{
    private int coins;
    public int requiredCoins = 50;

    public override void Start()
    {
        coins = player.Coins + requiredCoins;
    }

    public override bool IsAchieved() 
    {
        if(coins >= requiredCoins)
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
        Debug.Log("Collect_Coins Complete()");
    }

}
