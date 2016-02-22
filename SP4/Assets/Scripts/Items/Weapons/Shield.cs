using UnityEngine;

public class Shield : Weapon
{
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
	protected override void Update () {
	
	}

    public override bool Use(Vector2 direction)
    {
        // Use the base class Use() to do fire rate control
        bool usable = base.Use(direction);

        /*// If we are able to use it this round...
        if (usable)
        {
            // ...do what we have to do
        }*/

        // Return the value back
        return usable;
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

        if (other is Crossbow)
        {
            // -- Check if we found a projectile
            if (projectile != null)
            {
                // Destroy it
            }

            // Barrage of Arrows
        }
        else if (other is Wand)
        {
            // -- Check if we found a projectile
            if (projectile != null)
            {
                // Destroy it
            }

            // Set the shield to be larger
        }
    }
}
