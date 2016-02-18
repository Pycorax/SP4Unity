using UnityEngine;
using System.Collections;

public class Shield : Weapon {

    public Sprite shieldSprite;

	// Use this for initialization
	void Start () {
        Name = "Shield";
        Damage = 1;

        //1 Tile
        Range = 1;
        Width = 1;

        //1 per second
        FireRate = 2;


	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Redirects the missile in the player holding the Shield current Direction//
    public override void Use(Vector2 direction)
    {
        //Destroy EnemyProjectiles Upon Collision
    }
   
}
