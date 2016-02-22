using UnityEngine;
using System.Collections;

public class Arrow : Projectile
{
	// Use this for initialization
	protected override void Start ()
    {
        //pName = "Arrow";
        Damage = 5;
        Speed = 500;
        //Load Animation
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
	}

    public override void Activate(Vector3 position, Vector3 rotation, Vector2 direction)
    {
        base.Activate(position, rotation, direction);
    }
}
