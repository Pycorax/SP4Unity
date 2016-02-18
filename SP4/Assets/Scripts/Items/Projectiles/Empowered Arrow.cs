using UnityEngine;
using System.Collections;

public class EmpoweredArrow : Projectile {

    //2 Tiles 
    public int explodeRadius = 2;

	// Use this for initialization
	void Start () {
        pName = "Explosive Arcane Arrow";
        pDamage = 8;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Use(Vector2 direction)
    {

    }
}
