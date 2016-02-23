using UnityEngine;
using System.Collections;

public class FlyingSword : Projectile
{
    Enemy.Enemy prevTarget = null;
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

        if (!gameObject.activeSelf)
        {
            // Disabled
            Owner.gameObject.SetActive(true);
        }
    }

    public override void Activate(Transform data, Weapon shooter, Vector2 direction, float distTillDespawn)
    {
        base.Activate(data, shooter, direction, distTillDespawn);
        shooter.gameObject.SetActive(false);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy.Enemy>() != null)
        {
            //If Collided with Enemy Unit
            //Reduce Enemy HP (currently no function for that)
            Enemy.Enemy enemy = collision.gameObject.GetComponent<Enemy.Enemy>();

            if (prevTarget && enemy != prevTarget)
            {
                enemy.Injure(Damage);
                prevTarget = enemy;
            }
            else
            {
                prevTarget = enemy;
            }
        }
        else if (collision.gameObject.GetComponent<RPGPlayer>() != null)
        {
            // Collision handled by player
        }
        else
        {
            Owner.gameObject.SetActive(true);
            Disable();
        }
    }

    public override void Disable()
    {
        base.Disable();
        prevTarget = null;
    }
}
