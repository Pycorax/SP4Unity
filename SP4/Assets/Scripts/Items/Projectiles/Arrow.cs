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

    public override void Activate(Transform data, Weapon shooter, Vector2 direction, float distTillDespawn)
    {
        base.Activate(data, shooter, direction, distTillDespawn);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
