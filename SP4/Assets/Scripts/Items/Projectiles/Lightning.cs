using UnityEngine;
using System.Collections;

public class Lightning : Projectile
{
	// Use this for initialization
	protected override void Start ()
    {
        //pName = "Lightning Bolt";
        Damage = 3;
        //Load Animation
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
    
}
