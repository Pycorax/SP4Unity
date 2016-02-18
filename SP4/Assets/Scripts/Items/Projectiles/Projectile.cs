using UnityEngine;
using System.Collections;

public class Projectile : Weapon  {

    public string name;
    public int damage;
    public Animation animation;
    public float speed;

    public string pName { get { return name; } set { name = value; } }
    public int pDamage { get { return damage; } set { damage = value; } }
    public float pSpeed { get { return speed; } set { speed = value; } }

    // Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

    public override void Use(Vector2 direction)
    {
        MoveTowards(direction);
    }

    public virtual void MoveTowards(Vector2 direction)
    {
        // Ensure that the direction passed in is a direction
        direction.Normalize();

        // Calculate the new velocity
        Vector2 newVelocity = direction * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

        // Clamp the velocity
        newVelocity.x = Mathf.Clamp(newVelocity.x, 0, pSpeed);

        rigidBody.velocity = newVelocity;
    }


    /*
     * FUNCTION: OnCollisionEnter2D
     * params: Collision2D collision
     * 
     * brief: Perform Collision functions
     */
    public void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy.Enemy>() != null)
        {
            //If Collided with Enemy Unit
            //Reduce Enemy HP (currently no function for that)
        }
        else if(collision.gameObject.GetComponent<RPGPlayer>() != null)
        {
            //If Collided with other player
            //Combine Weapon Powerzz
            CombineUse(this, collision.gameObject.GetComponent<RPGPlayer>().getCurrentActiveWeapon());
        }
        /*
         * Colliding with TILE_WALL
         * else if(collision.gameObject.GetComponent<TILE.TILE_TYPE.TILE_WALL>())
         * {
         *     //Destroy Projectile
         *     Destroy(this);
         * }
         */
        else
        {
            OnBecameInvisible();
        }
    }

    //Destroy upon leaving camera screen space
    //Monobehaviour.OnBecameInvisible
    void OnBecameInvisible()
    {
        Destroy(this);
    }

}
