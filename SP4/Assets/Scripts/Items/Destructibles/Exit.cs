using UnityEngine;
using System.Collections;

public class Exit : Destroyables
{
    public float AnimSpeed = 0.5f;
    public GameManager gamemanager;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        anim.speed = AnimSpeed;
        anim.enabled = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void Onhit()
    {
        anim.enabled = true;
    }

    private void LevelEnded()
    {
        Debug.Log("LevelEnded");
        gamemanager.LevelEnded = true;
    }
}
