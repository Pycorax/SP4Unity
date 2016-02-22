﻿using UnityEngine;

public class Sword : Weapon
{
    
	// Use this for initialization
	protected override void Start () {

        Name = "Sword";
        Damage = 10;

        //1 Tile
        Range = 1;

        //1 per second
        FireRate = 1;

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

        if (other is Crossbow)
        {
            // -- Check if we found a projectile
            if (projectile != null)
            {
                // Destroy it
            }

            // Launch a Flying Sword
        }
        else if (other is Wand)
        {
            // -- Check if we found a projectile
            if (projectile != null)
            {
                // Destroy it
            }

            // Set the sword to be larger
        }
    }
}
