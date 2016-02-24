using UnityEngine;
using System.Collections;

public class Table : Destroyables
{
    public float AnimSpeed = 0.5f;

    // Use this for initialization
    protected override void Start()
    {
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
}
