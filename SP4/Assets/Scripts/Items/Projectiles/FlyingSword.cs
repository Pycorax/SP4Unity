using UnityEngine;

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

    public override void Activate(Transform data, Weapon shooter, Vector2 direction, float distTillDespawn)
    {
        base.Activate(data, shooter, direction, distTillDespawn);
        shooter.gameObject.SetActive(false);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Enemy.Enemy>() != null)
        {
            //If Collided with Enemy Unit
            //Reduce Enemy HP (currently no function for that)
            Enemy.Enemy enemy = other.gameObject.GetComponent<Enemy.Enemy>();

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
        else if (other.gameObject.GetComponent<RPGPlayer>() != null)
        {
            // Collision handled by player
        }
        else if (other.gameObject.GetComponent<Projectile>() != null)
        {
            // Do not allow collisions with self
            return;
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
