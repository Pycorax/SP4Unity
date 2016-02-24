using UnityEngine;

public class Shield : Weapon
{
    [Tooltip("The number of arrrows to reflect on an Arrow Barrage.")]
    public int BarrageArrows = 5;
    [Tooltip("The range of the arrows to reflect on an Arrow Barrage.")]
    public float BarrageRange = 10.0f;
    [Tooltip("The field of view angle where arrows would be fired from for an Arrow Barrage.")]
    public float BarrageFOV = 130.0f;

    public Sprite BigShield;
    private SpriteRenderer spriteRenderer;

    Transform firePoint;

    // Animation
    private bool shieldOut = false;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        firePoint = transform.FindChild("FirePoint");
        spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();
	}

    public override bool Use(Vector2 direction)
    {
        // Set the animation
        if (!shieldOut)
        {
            shieldOut = true;
            anim.SetBool(animAttack, true);
        }

        return true;
    }

    public override void Unuse()
    {
        if (shieldOut)
        {
            shieldOut = false;
            anim.SetBool(animAttack, false);
        }
    }

    protected override void combinedUse(Weapon other, params object[] details)
    {
        // Destroy the projectile
        // -- Find Projectile
        Projectile projectile = null;
        foreach (var o in details)
        {
            // We found it
            if (o is Projectile)
            {
                // Store it
                projectile = o as Projectile;
                break;
            }
        }
        #region Arrow Barrage

        if (other is Crossbow)
        {
            // -- Check if we found a projectile
            if (projectile != null)
            {
                // Destroy it
                projectile.Disable();
            }

            // Spawn Barrage of Arrows
            float barrageLeftAngle = (180 - BarrageFOV) * 0.5f;     // Dictates where we should start shooting from
            float degreeOfDifference = BarrageFOV / BarrageArrows;      // Get the angle in degrees between each arrow's direction
            var parent = GetComponentInParent<RPGPlayer>();         // Handle to thhe weapon's parent to get user direction

            // We got a handle to the parent?
            if (parent != null)
            {
                // Determine the Right Vector where we start shooting from
                Vector2 right = new Vector2(parent.CurrentDirection.y, -parent.CurrentDirection.x);

                // Shoot every arrow we need to shoot
                for (int i = 0; i < BarrageArrows; i++)
                {
                    // Fetch an arrow
                    var arrow = RefProjectileManager.FetchArrow().GetComponent<Arrow>();

                    // If we are able to get an arrow
                    if (arrow)
                    {

                        // Determine the angle of this shot
                        float arrowAngle = barrageLeftAngle + (degreeOfDifference * i);
                        // Get a rotation to calculate the direction vector
                        Quaternion rot = Quaternion.AngleAxis(arrowAngle, new Vector3(0.0f, 0.0f, 1.0f));
                        
                        // Calculate the direction vector
                        Vector2 dir = (rot * right).normalized;
                        //Debug.DrawLine(firePoint.position, firePoint.position + (Vector3)(dir * BarrageRange), Color.red, 5.0f);

                        // Shoot the arrow
                        arrow.Activate(firePoint, this, dir, Quaternion.FromToRotation(Vector2.up, dir), BarrageRange * RefProjectileManager.GetComponent<TileMap>().TileSize);
                    }
                }
            }
            
        }
        #endregion  

        #region Big Shield
        else if (other is Wand)
        {
            // -- Check if we found a projectile
            if (projectile != null)
            {
                // Destroy it
                projectile.Disable();
            }

            // Set the shield to be larger
            spriteRenderer.sprite = BigShield;
        }
        #endregion
    }
}
