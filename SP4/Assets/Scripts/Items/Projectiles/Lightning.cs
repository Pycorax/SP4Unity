using UnityEngine;

public class Lightning : Projectile
{
    public ResourceManager RefEnemyManager;
    public TileMap RefTileMap;
    public int MaxBounce = 3;
    public float DetectionRadius = 2;
    private Enemy.Enemy target = null;
    private Enemy.Enemy prevTarget = null;
    private int bounceCount = 0;

	// Use this for initialization
	protected override void Start ()
    {
        //pName = "Lightning Bolt";
        Damage = 3;
        //Load Animation
	}

    // Update is called once per frame
    protected override void Update()
    {
        if (target)
        {
            // Move towards target
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Speed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game));

            if (transform.position == target.transform.position)
            {
                // Projectile on target
                //Reduce Enemy HP (currently no function for that)
                Enemy.Enemy enemy = target.gameObject.GetComponent<Enemy.Enemy>();

                // Find new target
                if (prevTarget && enemy != prevTarget)
                {
                    enemy.Injure(Damage);
                    prevTarget = enemy;
                    target = findNearestEnemy();
                    if (!target)
                    {
                        // No new target found
                        Disable();
                    }
                }
                else if (!prevTarget)
                {
                    prevTarget = enemy;
                    // No previous target
                    target = findNearestEnemy();
                    if (!target)
                    {
                        // No new target found
                        Disable();
                    }
                }
            }
        }
        else
        {
            base.Update();
        }
    }

    public override void Activate(Transform data, Weapon shooter, Vector2 direction, float distTillDespawn)
    {
        base.Activate(data, shooter, direction, distTillDespawn);
    }

    public void Activate(Transform data, Weapon shooter, Vector2 direction, int chainTimes, float distTillDespawn)
    {
        base.Activate(data, shooter, direction, distTillDespawn);

        MaxBounce = chainTimes;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy.Enemy>() != null)
        {
            Enemy.Enemy enemy = collision.gameObject.GetComponent<Enemy.Enemy>();
            if (!target)
            {
                target = enemy;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else if (collision.gameObject.GetComponent<RPGPlayer>() != null)
        {
            // Collision handled by player
        }
        else if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            // Do not do collision against same type
        }
        else
        {
            Disable();
        }
    }

    public override void Disable()
    {
        base.Disable();
        bounceCount = 0;
        target = prevTarget = null;
    }

    private Enemy.Enemy findNearestEnemy()
    {
        if (bounceCount >= MaxBounce)
        {
            return null;
        }
        ++bounceCount;
        // TODO: Find new target
        float nearestDist = 0.0f;
        Enemy.Enemy nearestEnemy = null;
        float range = DetectionRadius * RefTileMap.TileSize;
        // Temp disable previous target collider so that raycast works
        prevTarget.GetComponent<BoxCollider2D>().enabled = false;

        foreach (GameObject go in RefEnemyManager.ActiveResourcesList)
        {
            Enemy.Enemy e = go.GetComponent<Enemy.Enemy>();
            // Cannot assign to the current enemy you're on and have to be within range specified
            if (go != prevTarget.gameObject /*(e.transform.position - transform.position).sqrMagnitude <= rangeSqr*/)
            {
                LayerMask mask = 1<<12;
                RaycastHit2D raycast = Physics2D.Raycast(transform.position, (go.transform.position - transform.position).normalized, range, mask.value);
                if (raycast)
                {
                    // Within range and not on current collided enemy
                    if (nearestEnemy)
                    {
                        // Nearest exists, check distance
                        if (raycast.distance < nearestDist)
                        {
                            nearestDist = raycast.distance;
                            nearestEnemy = raycast.collider.gameObject.GetComponent<Enemy.Enemy>();
                        }
                    }
                    else
                    {
                        // Nearest does not exists, direct assign
                        nearestDist = raycast.distance;
                        nearestEnemy = raycast.collider.gameObject.GetComponent<Enemy.Enemy>();
                    }
                }
            }
        }
        // Enable previous target collider so that everything is back to normal
        prevTarget.GetComponent<BoxCollider2D>().enabled = true;

        return nearestEnemy;
    }


}
