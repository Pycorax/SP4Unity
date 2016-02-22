using UnityEngine;
using System.Collections;

public class Table : Destroyables
{
    public float AnimSpeed = 0.5f;
    public Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = AnimSpeed;
        anim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onhit(GameObject Gameobject)
    {

    }
}
