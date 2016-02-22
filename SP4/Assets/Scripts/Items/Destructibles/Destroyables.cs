using UnityEngine;
using System.Collections;

public class Destroyables : Item {

	// Use this for initialization
    protected override void Start()
    {
	
	}
	
	// Update is called once per frame
    protected override void Update()
    {
	
	}

	public override void Use(Vector2 direction)
	{

	}

    public override void CombineUse(Projectile projectile, Weapon weapon)
    {
        throw new System.NotImplementedException();
    }
}
