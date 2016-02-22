using UnityEngine;


public class Crossbow : Weapon
{
    //public GameObject projectile;

    //private float projectileSpeed;

    Transform firePoint;

    //public float ProjectileSpeed { get { return projectileSpeed; } set { projectileSpeed = value; } }

    //public float projectileMaxSpeed = 5;

    ////////////////////////////////////////////
    

	// Use this for initialization
    protected override void Start ()
    {
        firePoint = transform.FindChild("FirePoint");

        if(!firePoint)
        {
            Debug.LogError("No FirePoint");
        }
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
	}

    public override bool Use(Vector2 direction)
    {
        if (base.Use(direction))
        {
            // Shooting cancer
            /*Debug.Log("Shooting");
            // Ensure that the direction passed in is a direction
            direction.Normalize();

            // Calculate the new velocity
            Vector2 newVelocity = direction * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

            // Clamp the velocity
            newVelocity.x = Mathf.Clamp(newVelocity.x, 0, projectileMaxSpeed);

            rigidBody.velocity = newVelocity;

            Vector2 firePointPos = new Vector2(firePoint.position.x, firePoint.position.y);

            RaycastHit2D hit = Physics2D.Raycast(firePointPos, direction, 100, notToHit);

            Debug.DrawLine(firePointPos, direction * 100);*/

            GameObject p = RefProjectileManager.FetchArrow();
            if (p)
            {
                p.GetComponent<Arrow>().Activate(firePoint, this, direction, Range * RefProjectileManager.GetComponent<TileMap>().TileSize);
                return true;
            }
        }
        return false;
    }

    protected override void combinedUse(Weapon other, params object[] details)
    {
        // Explosive Arcane Shot
        if (other is Wand)
        {
            Projectile arrow = null;

            // Find the projectile
            foreach (var o in details)
            {
                // We found it
                if (o is Projectile)
                {
                    // Store it
                    arrow = o as Projectile;
                    break;
                }
            }

            // Check if we found it
            if (arrow != null)
            {
                // TODO: Change the projectile
            }
        }
    }
}
