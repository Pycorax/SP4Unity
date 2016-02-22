using UnityEngine;
using System.Collections;

public class Table : Destroyables
{
    public float AnimSpeed = 0.5f;
    public Animator anim;

    // Use this for initialization
    protected override void Start()
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

    public void Onhit(GameObject gameobject)
    {
        anim.enabled = true;
    }
}
