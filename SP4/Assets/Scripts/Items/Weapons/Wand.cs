using UnityEngine;
using System.Collections;

public class Wand : Weapon {
 
    public Sprite wandSprite;

    public Lightning lightning;
	// Use this for initialization
	protected override void Start () {
        Name = "Wand";
        Damage = 3;

        //1 Tile
        Range = 5;
        Width = 1;

        //1 per second
        FireRate = 1;
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}

    public override void Use(Vector2 direction)
    {
        //Fire a Lightning Bolt Projectile
        //Check for Collision with any other weapons
        lightning.MoveTowards(direction);
    }
}
