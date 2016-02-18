using UnityEngine;
using System.Collections;

public class Lightning : Projectile {

	// Use this for initialization
	void Start () {
        pName = "Lightning Bolt";
        pDamage = 3;
        //Load Animation
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Use(Vector2 direction)
    {
    }
    
}
