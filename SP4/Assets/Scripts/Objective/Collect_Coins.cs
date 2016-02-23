using UnityEngine;
using System.Collections;

public class Collect_Coins : Objectives
{
    private int coins = 0;
    public int requiredCoins = 50;
    private int coinsNeeded = 0;

    public override void Start()
    {
        coinsNeeded = player.Coins + requiredCoins;
    }

    public override bool IsAchieved()
    {
        if (coins >= coinsNeeded)
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

    public override void Update()
    {
        coins = player.Coins;
    }

}
