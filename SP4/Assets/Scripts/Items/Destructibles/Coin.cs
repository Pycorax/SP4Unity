using UnityEngine;
using System.Collections;

public class Coin : Collectible
{
    public int CoinAmount = 1;
    public float AnimSpeed = 0.5f;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        anim.speed = AnimSpeed;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        RPGPlayer player = other.gameObject.GetComponent<RPGPlayer>();
        if (player != null)
        {
            if (Manager)
            {
                Manager.NotifyCoinCollected(CoinAmount);
            }
            player.PlayerSettingsReference.AddCoins(CoinAmount);
        }
        base.OnTriggerEnter2D(other);
    }
}
