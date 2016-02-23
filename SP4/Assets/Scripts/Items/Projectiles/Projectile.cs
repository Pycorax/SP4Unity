using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage = 0;
    public float Speed = 0.0f;
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
        gameObject.SetActive(true);
        transform.position = data.position;
        transform.rotation = data.rotation;
        DistTillDespawn = distTillDespawn;
        Owner = shooter;
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
        Owner = null;
    }

}
