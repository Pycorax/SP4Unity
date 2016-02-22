using UnityEngine;

public class Wand : Weapon
{
    public Lightning lightning;
	// Use this for initialization
	protected override void Start () {
        Name = "Wand";
        Damage = 3;

        //1 Tile
        Range = 5;

        //1 per second
        FireRate = 1;
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}

    public override bool Use(Vector2 direction)
    {
        // Use the base class Use() to do fire rate control
        bool usable = base.Use(direction);

        // If we are able to use it this round...
        if (usable)
        {
            // ...do what we have to do
            //Fire a Lightning Bolt Projectile
            //Check for Collision with any other weapons
            lightning.MoveTowards(direction);
        }

        // Return the value back
        return usable;
    }

    protected override void combinedUse(Weapon other, params object[] details)
    {
        
    }
}
