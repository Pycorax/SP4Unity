using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class EmpoweredArrow : Projectile
{
    //2 Tiles 
    public int explodeRadius = 2;

	// Use this for initialization
	protected override void Start ()
    {
        //pName = "Explosive Arcane Arrow";
        Damage = 8;
        Speed = 600;
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

    private void OnCollisionEnter2D(Collider2D other)
    {
        // On collision, start explosion

    }
}
