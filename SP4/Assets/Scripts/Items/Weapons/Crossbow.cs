using UnityEngine;


public class Crossbow : Weapon
{
    Transform firePoint;

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
            /*
            Debug.Log("Shooting");
            // Ensure that the direction passed in is a direction
            direction.Normalize();

            // Calculate the new velocity
            Vector2 newVelocity = direction * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

            // Clamp the velocity
            newVelocity.x = Mathf.Clamp(newVelocity.x, 0, projectileMaxSpeed);

            rigidBody.velocity = newVelocity;

            Vector2 firePointPos = new Vector2(firePoint.position.x, firePoint.position.y);

            RaycastHit2D hit = Physics2D.Raycast(firePointPos, direction, 100, notToHit);

            Debug.DrawLine(firePointPos, direction * 100);
            */

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
        #region Explosive Arcane Shot (Wand => Crossbow)
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
                // Create the Empowered Arrow
                var empoweredArrow = RefProjectileManager.FetchEmpoweredArrow().GetComponent<EmpoweredArrow>();

                // Get a handle to the owner of this weapon to get shoot direction
                var parent = GetComponentInParent<RPGPlayer>();

                // Initialize and fire the Empowered Arrow
                if (empoweredArrow && parent)
                {
                    empoweredArrow.Activate(firePoint, this, parent.CurrentDirection, Range * RefProjectileManager.GetComponent<TileMap>().TileSize);
                }

                // Destroy the existing Arrow
                arrow.transform.gameObject.SetActive(false);
            }
        }
        #endregion
    }
}
