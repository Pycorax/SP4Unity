using UnityEngine;
using System.Collections;

public class Collect_Coins : Objectives
{
    private int coins = 0;
    public int requiredCoins = 50;
 
    public override bool IsComplete() 
    {
        return (coins >= requiredCoins);
    }
 
    public override void Complete() 
    {
    }
 
    public void OnTriggerEnter2D(Collider2D other) 
    {
        if (string.Equals(other.tag, "Coin")) {
            coins++;
            Destroy(other.gameObject);
        }
    }
}
