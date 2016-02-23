using UnityEngine;

public class EmpoweredArrow : Projectile
{
    [Tooltip("Reference to the ExplosionManager for getting Explosion objects")]
    public ResourceManager ExplosionManager;

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

    public override void Activate(Transform data, Weapon shooter, Vector2 direction, float distTillDespawn)
    {
        base.Activate(data, shooter, direction, distTillDespawn);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        // Start explosion
        var explosion = ExplosionManager.Fetch();
        explosion.SetActive(true);

        // -- Set Explosion Position
        Vector2 newPos = other.transform.position;
        explosion.transform.position = newPos;
    }
}
