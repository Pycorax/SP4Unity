using UnityEngine;
using System.Collections;


public class Crossbow : Weapon {

    public Sprite CrossbowSprite;
    public Arrow arrow;
    public EmpoweredArrow empoweredArrow;

	// Use this for initialization
	void Start () {
        Name = "Crossbow";
        
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
        //Play Firing Animation
        arrow.MoveTowards(direction);
    }
}
