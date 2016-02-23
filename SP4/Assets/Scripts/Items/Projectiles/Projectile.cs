using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public int Damage = 0;
    public float Speed = 0.0f;
    [Tooltip("Use this to adjust the range of a specific projectile.")]
    public float RangeMultiplier = 1.0f;
    private float DistTillDespawn;

    // Owner
    private Weapon owner = null;
    public Weapon Owner { get { return owner; } set { owner = value; } }

    // Bounds
    public float LeftBound { get { return transform.position.x - transform.localScale.x * 0.5f; } }
    public float RightBound { get { return transform.position.x + transform.localScale.x * 0.5f; } }
    public float TopBound { get { return transform.position.y + transform.localScale.y * 0.5f; } }
    public float BottomBound { get { return transform.position.y - transform.localScale.y * 0.5f; } }

    // Use this for initialization
    protected virtual void Start()
    {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
    protected virtual void Update()
    {
        // Deduct distance
        DistTillDespawn -= GetComponent<Rigidbody2D>().velocity.magnitude * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
        if (DistTillDespawn <= 0.0f)
        {
            Disable();
        }
	}

    public virtual void Activate(Transform data, Weapon shooter, Vector2 direction, float distTillDespawn)
    {
        // Activate this Projectile
        gameObject.SetActive(true);
        // Initialize Transforms
        transform.position = data.position;
        transform.rotation = data.rotation;
        // Set the range
        DistTillDespawn = distTillDespawn * RangeMultiplier;
        // Store a reference to the owner
        Owner = shooter;
        // Add Collision Exceptions
        SetCollisionWithOwners(false);
        // Start moving the Projectile
        MoveTowards(direction);
    }

    public void MoveTowards(Vector2 direction)
    {
        // Ensure that the direction passed in is a direction
        direction.Normalize();

        // Calculate the new velocity
        Vector2 newVelocity = direction * /*(float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game) **/ Speed;

        // Clamp the velocity
        //newVelocity.x = Mathf.Clamp(newVelocity.x, 0, Speed);

        GetComponent<Rigidbody2D>().velocity = newVelocity;
    }


    /*
     * FUNCTION: OnCollisionEnter2D
     * params: Collision2D collision
     * 
     * brief: Perform Collision functions
     */
    public virtual void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy.Enemy>() != null)
        {
            //If Collided with Enemy Unit
            //Reduce Enemy HP (currently no function for that)
            Enemy.Enemy enemy = collision.gameObject.GetComponent<Enemy.Enemy>();
            enemy.Injure(Damage);
            Disable();
        }
        else if(collision.gameObject.GetComponent<RPGPlayer>() != null)
        {
            // Collision handled by player
        }
        else
        {
            Disable();
        }
    }
    
    public virtual void Disable()
    {
        gameObject.SetActive(false);
        // Strip Collison Exceptions
        SetCollisionWithOwners(true);
        // Disown the Owner
        Owner = null;
    }

    /// <summary>
    /// Use this function to set if this projectile should collide with it's owners
    /// </summary>
    /// <param name="collision">If true, collision with owners are enabled.</param>
    public void SetCollisionWithOwners(bool collision)
    {
        // Remove Collision Exceptions
        var thisCollider = GetComponent<Collider2D>();

        // -- Ensure that the weapon won't crash with the projectile
        Physics2D.IgnoreCollision(thisCollider, Owner.GetComponent<Collider2D>(), !collision);

        // -- Ensure that the weapon's owner won't crash with the projectile
        var weapOwner = Owner.transform.GetComponentInParent<RPGPlayer>();
        if (weapOwner != null)
        {
            Physics2D.IgnoreCollision(thisCollider, weapOwner.GetComponent<Collider2D>(), !collision);
        }
    }

}
