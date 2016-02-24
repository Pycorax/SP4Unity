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

    // Update is called once per frame
    void Update()
    {

    }
}
