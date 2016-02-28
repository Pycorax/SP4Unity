using UnityEngine;
using System.Collections;

public class Coin : Destroyables
{
    public int CoinAmount = 2;
    public float AnimSpeed = 0.5f;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        anim.speed = AnimSpeed;
    }

    public void OnHit()
    {
        anim.enabled = true;

        // Notify reached exit
        Manager.NotifyCoinCollected(CoinAmount);
    }
}
