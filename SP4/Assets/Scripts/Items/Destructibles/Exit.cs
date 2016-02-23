using UnityEngine;
using System.Collections;

public class Exit : Destroyables
{
    public float AnimSpeed = 0.5f;
    public Animator anim;
    public GameManager gamemanager;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
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
