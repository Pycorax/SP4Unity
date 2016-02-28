using UnityEngine;
using System.Collections;

public class Heart : Collectible
{
    public int Healing = 30;
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
            player.Heal(Healing);
        }
        base.OnTriggerEnter2D(other);
    }
}
