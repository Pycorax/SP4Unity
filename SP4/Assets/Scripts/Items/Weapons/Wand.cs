using UnityEngine;
using System.Collections;

public class Wand : Weapon {
 
    public Sprite wandSprite;

	// Use this for initialization
	void Start () {
        Name = "Wand";
        Damage = 3;

        //1 Tile
        Range = 5;
        Width = 1;

        //1 per second
        FireRate = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Use(Vector2 direction)
    {
        //Fire a Lightning Bolt Projectile

    }
}
