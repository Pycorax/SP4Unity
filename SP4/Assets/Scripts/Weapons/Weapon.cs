using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {

    //Weapon Damage
    public int wDamage;
    //Weapon Name
    public string wName;
    //Weapon Attack Range (In Tiles)
    public int wRange;
    //Weapon Width (In Tiles)
    public int wWidth;
    //Weapon Firerate
    public int wFireRate;


    //GETTERS AND SETTERS LMAOKAI
    public int Damage { get { return wDamage; } set { wDamage = value; } }
    public string Name { get { return wName; } set { wName = value; } }
    public int Range { get { return wRange; } set { wRange = value; } }
    public int Width { get { return wWidth; } set { wWidth = value; } }
    public int FireRate { get { return wFireRate; } set { wFireRate = value; } }

    //public enum WEAPON_TYPE
    //{
    //    WEAPON_CROSSBOW,
    //    WEAPON_SWORD,
    //    WEAPON_SHIELD,
    //    WEAPON_WAND,
    //    WEAPON_HAMMER,
    //    NUM_TYPES
    //}

    //=============================================================//

	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

    public abstract void Use(Vector2 direction);

    public virtual void CombineUse(Weapon weapon)
    {

        //=======WAND USED ON SWORD======//
        if(this is Wand && weapon is Sword)
        {
            //=======BUFFED SWORD=======//
            /*
             *  Deals Increased Damage
             *  Larger Range
             */
            //==========================//
            Name = "Big Sword";
            Damage = 15;
            Range = 2;
            FireRate = 2;

            //Increase Sprite size for sword

            //Maybe Load Another Sprite?
        }

        //==========WAND USED ON SHIELD=========//
        else if(this is Wand && weapon is Shield)
        {
            //=======BUFFED SHIELD=======//
            /*
             *  Wider range, 
             *  able to block more 
             *  projectiles and attacks.
             */
            //===========================//
            Name = "Big Shield";
            Width = 3;
        }
        
        //=========CROSSBOW USED ON WAND=========//
        else if(this is Crossbow && weapon is Wand)
        {
            //=======EXPLOSIVE ARCANE SHOT=======//
            /*
             * Deals increased damage
             * Larger Range
             * Explodes in an AOE around
             * first faggot it collides with
             */
            //===================================//
            Name = "EXPLOSIVE AKBAR SHOT";
            //Replace Projectile Object with Special Projectile Object
            //set its stats
            Damage = 10;
        }

        //==========CROSSBOW USED ON SWORD==========//
        else if (this is Crossbow && weapon is Sword)
        {
            //=======Piercing Flying Blade=======//
            /*
             * Deals increased damage
             * Fires a sword in a straight line
             * 
             * ENDS when collides with enemy, 
             * dealing damage.
             * OR
             * Colliding with a wall
             */
            //===================================//
            Name = "FLYING RAIJIN NO JUTSU";
            //Spawn Projectile ("Sword") at player
            //Set Direction to player Direction
            //Move forward and collide
            //Do damage if collides properly
            Damage = 10;
        }

        //==========CROSSBOW USED ON SHIELD==========//
        else if (this is Crossbow && weapon is Shield)
        {
            //===========Arrow Barrage===========//
            /*
             * Deals increased damage
             * 5 Arrows Spawn from the shield
             * Dispersing at a 10-15 degree angle
             * 
             * ENDS when collides with enemy, 
             * dealing damage.
             * OR
             * Colliding with a wall
             */
            //===================================//
            Name = "Arrow Barrage";
            //Despawn Arrow
            //Spawn 5 Arrows
            //Set Directions

            //Move forward and collide
            //Do damage if collides properly
            Damage = 8;
        }

        //NO NEED FOR PROJECTILE HERE

        //==========SWORD USED ON SHIELD==========//
        else if (this is Sword && weapon is Shield)
        {
            //===========Impale===========//
            /*
             * Instantly kills the enemy
             * In Between Sword and Shield
             */
            //============================//
            Name = "Impale";
            //Set Enemy HP->0
        }
    }
}
