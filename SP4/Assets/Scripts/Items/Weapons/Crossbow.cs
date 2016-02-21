using UnityEngine;
using System.Collections;


public class Crossbow : Weapon {

    public Sprite CrossbowSprite;
    public ProjectileManager PManager;
    public GameObject projectile;

    private float projectileSpeed;


    public LayerMask notToHit;

    float timeToFire = 0;

    Transform firePoint;

    public float ProjectileSpeed { get { return projectileSpeed; } set { projectileSpeed = value; } }

    public float projectileMaxSpeed = 5;

	// Use this for initialization
    protected override void Start () {
        Name = "Crossbow";
        
        Width = 1;

        Damage = 5;

        //1 per second
        FireRate = 0;

        

        firePoint = transform.FindChild("FirePoint");

        if(!firePoint)
        {
            Debug.LogError("No FirePoint");
        }
	}
	
	// Update is called once per frame
	protected override void Update () {
        //Testing Shooting
       
	}

    public override void Use(Vector2 direction)
    {
        //Fire Projectile from projectile class
        //Play Firing Animation
        //arrow.MoveTowards(direction);
        projectile = PManager.FetchArrow();

        
        
    }

    public void Shoot(Vector2 direction)
    {
        Debug.Log("Shooting");
        // Ensure that the direction passed in is a direction
        direction.Normalize();

        // Calculate the new velocity
        Vector2 newVelocity = direction * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

        // Clamp the velocity
        newVelocity.x = Mathf.Clamp(newVelocity.x, 0, projectileMaxSpeed);

        rigidBody.velocity = newVelocity;

        Vector2 firePointPos = new Vector2(firePoint.position.x, firePoint.position.y);

        RaycastHit2D hit = Physics2D.Raycast(firePointPos, direction, 100, notToHit);

        Debug.DrawLine(firePointPos, direction * 100);
    }
}
