using UnityEngine;

public class Shield : Weapon
{
    [Tooltip("The number of arrrows to reflect on an Arrow Barrage.")]
    public int BarrageArrows = 5;

    // Use this for initialization
	protected override void Start ()
    {
        Name = "Shield";
        Damage = 1;

        //1 Tile
        Range = 1;

        //1 per second
        FireRate = 2;
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
    }

    public override bool Use(Vector2 direction)
    {
        return false;
    }


    protected override void combinedUse(Weapon other, params object[] details)
    {
        // Destroy the projectile
        // -- Find Projectile
        Projectile projectile = null;
        foreach (var o in details)
        {
            // We found it
            if (o is Projectile)
            {
                // Store it
                projectile = o as Projectile;
                break;
            }
        }
        #region Arrow Barrage

        if (other is Crossbow)
        {
            // -- Check if we found a projectile
            if (projectile != null)
            {
                // Destroy it
                projectile.Disable();
            }

            // Barrage of Arrows
            for(int i = 0; i < BarrageArrows; i++)
            {
                var arrow = RefProjectileManager.FetchArrow().GetComponent<Arrow>();

                var parent = GetComponentInParent<RPGPlayer>();

                if(arrow && parent)
                {
                    arrow.Activate(transform, this, parent.CurrentDirection, Range * RefProjectileManager.GetComponent<TileMap>().TileSize);
                }
            }
            
        }
        #endregion  

        #region Big Shield
        else if (other is Wand)
        {
            // -- Check if we found a projectile
            if (projectile != null)
            {
                // Destroy it
                projectile.Disable();
            }

            // Set the shield to be larger
            
        }
        #endregion
    }
}
