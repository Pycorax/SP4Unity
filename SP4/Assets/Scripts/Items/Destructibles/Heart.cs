using UnityEngine;
using System.Collections;

public class Heart : Destroyables
{
    public int Healing = 30;
    public float AnimSpeed = 0.5f;
    public Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = AnimSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
