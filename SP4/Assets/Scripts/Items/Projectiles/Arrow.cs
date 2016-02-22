using UnityEngine;
using System.Collections;

public class Arrow : Projectile
{
	// Use this for initialization
	protected override void Start ()
    {

	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
	}

    public override void Activate(Vector3 position, Quaternion rotation, Vector2 direction, float distTillDespawn)
    {
        base.Activate(position, rotation, direction, distTillDespawn);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }
}
