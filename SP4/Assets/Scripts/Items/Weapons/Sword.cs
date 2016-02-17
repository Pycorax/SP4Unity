using UnityEngine;
using System.Collections;

public class Sword : Weapon {

    public Sprite SwordSprite;

	// Use this for initialization
	void Start () {

        Name = "Sword";
        Damage = 10;

        //1 Tile
        Range = 1;
        Width = 1;

        //1 per second
        FireRate = 1;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Use(Vector2 direction)
    {
        //Play Attack Animation
        //Attack whatever is in the grid in front of player
        //Reduce the hp of that thing by Damage
    }
}
