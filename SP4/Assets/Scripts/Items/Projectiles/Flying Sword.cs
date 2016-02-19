using UnityEngine;
using System.Collections;

public class FlyingSword : Projectile {

	// Use this for initialization
	protected override void Start () {
        pName = "Flying Sword";
        pDamage = 10;
        //Load Animation
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}

    public override void Use(Vector2 direction)
    {
    }
}
