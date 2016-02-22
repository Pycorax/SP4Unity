using UnityEngine;
using System.Collections;

public class FlyingSword : Projectile
{

    // Use this for initialization
    protected override void Start()
    {
        //pName = "Flying Sword";
        //Damage = 10;
        //Load Animation
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Activate(Transform data, Weapon shooter, Vector2 direction, float distTillDespawn)
    {
        base.Activate(data, shooter, direction, distTillDespawn);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy.Enemy>() != null)
        {
            //If Collided with Enemy Unit
            //Reduce Enemy HP (currently no function for that)
            Enemy.Enemy enemy = collision.gameObject.GetComponent<Enemy.Enemy>();
        }
        else if (collision.gameObject.GetComponent<RPGPlayer>() != null)
        {

        }
        else
        {
            Disable();
        }
    }
}
