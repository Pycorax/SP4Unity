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
                // Destroy the existing Arrow
                arrow.Disable();

                // Create the Empowered Arrow
                var empoweredArrow = RefProjectileManager.FetchEmpoweredArrow().GetComponent<EmpoweredArrow>();

                // Get a handle to the owner of this weapon to get shoot direction
                var parent = GetComponentInParent<RPGPlayer>();

                // Initialize and fire the Empowered Arrow
                if (empoweredArrow && parent)
                {
                    // Fire the Empowered Arrow
                    empoweredArrow.Activate(firePoint, this, parent.CurrentDirection, Range * RefProjectileManager.GetComponent<TileMap>().TileSize);
                }
            }
        }

        #endregion
    }
}
