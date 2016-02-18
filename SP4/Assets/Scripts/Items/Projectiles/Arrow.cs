using UnityEngine;
using System.Collections;

public class Arrow : Projectile {
    
	// Use this for initialization
	void Start () {
        pName = "Arrow";
        pDamage = 5;
        //Tiles/sec?
        pSpeed = 2;
        //Load Animation
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Use(Vector2 direction)
    {
    }
}
