using UnityEngine;
using System.Collections;

public class SpikeTrap : Destroyables
{
    public int dmg = 30;
    public float AnimSpeed = 0.1f;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<RPGPlayer>() != null)
        {
            other.gameObject.GetComponent<RPGPlayer>().Injure(dmg);
        }
    }
}