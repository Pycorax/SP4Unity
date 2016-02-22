using UnityEngine;
using System.Collections;

public class FlyingSword : Projectile
{

	// Use this for initialization
	protected override void Start ()
    {
        //pName = "Flying Sword";
        Damage = 10;
        //Load Animation
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
    }

    public override void Activate(Vector2 direction)
    {
    }
}
