using UnityEngine;

public abstract class Weapon : Item {

    [Tooltip("The damage the weapon does.")]
    public int Damage;
    [Tooltip("The range of the weapon in tile map units.")]
    public float Range;
    [Tooltip("The fire rate of the weapon in hits/minute.")]
    public float FireRate = 0.0f;
    [Tooltip("If the weapon is fired by holding down.")]
    public bool HeldDownUsable = false;

    public ProjectileManager RefProjectileManager { get { return refProjectileManager; } set { refProjectileManager = value; } }
    private ProjectileManager refProjectileManager;

    // Animation
    static protected int animAttack = Animator.StringToHash("Attack");
    static protected int animWithdraw = Animator.StringToHash("Withdraw");

    /// <summary>
    /// The actual recorded time between shots in milliseconds
    /// </summary>
    private float useTimeDelta = 0.0f;

    // Getters
    public bool CanUse { get { return useTimeDelta >= FireRate; } }

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();

        // Initialize the fire rate based on the values provided
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
        // Update the time between hits
        if (useTimeDelta < FireRate)
        {
            useTimeDelta += (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
        }
	}

    public override bool Use()
    {
        return Use(Vector2.zero);
    }

    /// <summary>
    /// Use this function to stop usage on  weapons that are held down
    /// </summary>
    public virtual void Unuse() { }

    /// <summary>
    /// Use this function to hold back the weapon when another weapon is being used
    /// </summary>
    public void Withdraw()
    {
        // Update weapon animation
        if (anim != null)
        {
            anim.SetTrigger(animWithdraw);
        }
    }

    public virtual bool Use(Vector2 direction)
    {
        // Check if can use
        if (CanUse)
        {
            // Reset the timer
            useTimeDelta = 0.0f;

            // Update weapon animation
            if (anim != null)
            {
                anim.SetTrigger(animAttack);
            }

            return true;
        }

        // No fire rate
        return false;
    }

    /// <summary>
    /// Abstract function to handle what happens when a weapon syncs with another.
    /// </summary>
    /// <param name="other">The weapon whose attack hit this weapon.</param>
    /// <param name="details">The list of extra parameters you wish to provide.</param>
    protected abstract void combinedUse(Weapon other, params object[] details);

    /// <summary>
    /// Use this function to trigger a combined attack when a collision is not a trigger.
    /// </summary>
    /// <param name="other">The weapon to trigger with.</param>
    public void CombinedUse(Weapon other, params object[] details)
    {
        combinedUse(other, details);
    }
}
