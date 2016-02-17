using UnityEngine;
using System.Collections;


public class Crossbow : Weapon {

    private Sprite CrossbowSprite;

	// Use this for initialization
	void Start () {
        Name = "Crossbow";
        Damage = 8;

        //1 Tile
        Range = 7;
        Width = 1;

        //1 per second
        FireRate = 3;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Use(Vector2 direction)
    {
        //Fire Projectile from projectile class

    }
}
