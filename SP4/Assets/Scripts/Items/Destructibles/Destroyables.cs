using UnityEngine;
using System.Collections;

public class Destroyables : Item {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Use(Vector2 direction)
	{

	}

    public override void CombineUse(Projectile projectile, Weapon weapon)
    {
        throw new System.NotImplementedException();
    }

	//Animation
	public virtual void DestroyItem()
	{
		if(this is Heart)
		{
			//play animation HERE

		   //fucking kah wei doesnt have a player health
		   //3.8 gpa bois
		}
	}
}
