using UnityEngine;

public abstract class Weapon : Item {

    [Tooltip("The damage the weapon does.")]
    public int Damage;
    [Tooltip("The range of the weapon in Unity units.")]
    public float Range;
    [Tooltip("The fire rate of the weapon in hits/minute.")]
    public float FireRate;

    /// <summary>
    /// The time between shots in milliseconds
    /// </summary>
    private float fireRate;
    /// <summary>
    /// The actual recorded time between shots in milliseconds
    /// </summary>
    private float useTimeDelta = 0.0f;

    // Getters
    public bool CanUse { get { return useTimeDelta >= fireRate; } }

	// Use this for initialization
	protected override void Start ()
    {
        // Initialize the fire rate based on the values provided
        fireRate = 1000 / (FireRate / 60);
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
        // Update the time between hits
        useTimeDelta += (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
	}

    public override bool Use()
    {
        // Use on self?
        return Use(Vector2.zero);
    }

    public virtual bool Use(Vector2 direction)
    {
        // Check if can use
        if (CanUse)
        {
            // Reset the timer
            useTimeDelta = 0.0f;
            return true;
        }

        // No fire rate
        return false;
    }

    /// <summary>
    /// Abstract function to handle what happens when a weapon syncs with another.
    /// </summary>
    /// <param name="other">The weapon whose attack hit this weapon.</param>
    /// <param name="details">The list of extra parameters you wish to provide.</param>
    protected abstract void combinedUse(Weapon other, params object[] details);

    /// <summary>
    /// Use this function to trigger a combined attack when a collision is not a trigger.
    /// </summary>
    /// <param name="other">The weapon to trigger with.</param>
    public void CombinedUse(Weapon other, params object[] details)
    {
        combinedUse(other, details);
    }

    //UPON Collision, assign weapon hit as 'weapon'
    //public virtual void CombineUse(Projectile projectile, Weapon weapon)
    //{
    //    //=======WAND USED ON SWORD======//
    //    //===================WAYNE===================//
    //    //============WAND USED ON SWORD============//
    //    if(projectile is Lightning && weapon is Sword)
    //    {
    //        //=======BUFFED SWORD=======//
    //        /*
    //         *  Deals Increased Damage
    //         *  Larger Range
    //         */
    //        //==========================//
    //        Name = "Big Sword";
    //        Damage = 15;
    //        Range = 2;
    //        FireRate = 2;

    //        //Load another Sprite here
    //    }

    //    //==========WAND USED ON SHIELD=========//
    //    else if (projectile is Lightning && weapon is Shield)
    //    {
    //        //=======BUFFED SHIELD=======//
    //        /*
    //         *  Wider range, 
    //         *  able to block more 
    //         *  projectiles and attacks.
    //         */
    //        //===========================//
    //        Name = "Big Shield";
    //        Width = 3;
    //    }
        
    //    //=========CROSSBOW USED ON WAND=========//
    //    else if(projectile is Arrow && weapon is Wand)
    //    {
    //        //=======EXPLOSIVE ARCANE SHOT=======//
    //        /*
    //         * Deals increased damage
    //         * Larger Range
    //         * Explodes in an AOE around
    //         * first enemy it collides with
    //         */
    //        //===================================//
    //        Name = "EXPLOSIVE AKBAR SHOT";
    //        //Replace Projectile Object with Special Projectile Object
    //        //set its stats
    //        Damage = 10;
    //    }

    //    //==========CROSSBOW USED ON SWORD==========//
    //    else if (projectile is Arrow && weapon is Sword)
    //    {
    //        //=======Piercing Flying Blade=======//
    //        /*
    //         * Deals increased damage
    //         * Fires a sword in a straight line
    //         * 
    //         * ENDS when collides with enemy, 
    //         * dealing damage.
    //         * OR
    //         * Colliding with a wall
    //         */
    //        //===================================//
    //        Name = "FLYING RAIJIN NO JUTSU";
    //        //Spawn Projectile ("Sword") at player
    //        //Set Direction to player Direction
    //        //Move forward and collide
    //        //Do damage if collides properly
    //        Damage = 10;
    //    }

    //    //==========CROSSBOW USED ON SHIELD==========//
    //    else if (projectile is Arrow && weapon is Shield)
    //    {
    //        //===========Arrow Barrage===========//
    //        /*
    //         * Deals increased damage
    //         * 5 Arrows Spawn from the shield
    //         * Dispersing at a 10-15 degree angle
    //         * 
    //         * ENDS when collides with enemy, 
    //         * dealing damage.
    //         * OR
    //         * Colliding with a wall
    //         */
    //        //===================================//
    //        Name = "Arrow Barrage";
    //        //Despawn Arrow
    //        //Spawn 5 Arrows
    //        //Set Directions

    //        //Move forward and collide
    //        //Do damage if collides properly
    //        Damage = 8;
    //    }

    //    //NO NEED FOR PROJECTILE HERE

    //    //==========SWORD USED ON SHIELD==========//
    //    else if (this is Sword && weapon is Shield)
    //    {
    //        //===========Impale===========//
    //        /*
    //         * Instantly kills the enemy
    //         * In Between Sword and Shield
    //         */
    //        //============================//
    //        Name = "Impale";
    //        //Set Enemy HP->0
    //    }
    //}
}
