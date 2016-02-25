using UnityEngine;

public class Sword : Weapon
{
    [Tooltip("The duration of BigSword")]
    public float BigSwordDuration = 5.0f;

    //Time in BigSword Mode
    private float bigSwordTimer = 0.0f;

    //Boolean to detect if Sword is in BigSword Mode
    private bool isBigSword = false;

    //Animation
    //private readonly int animBigSword = Animator.StringToHash("")

    private SpriteRenderer spriteRenderer;
    public Sprite BigSword;
    public Sprite NormalSword;


	// Use this for initialization
	protected override void Start () {

        base.Start();
        Damage = 10;

        spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();

        //1 Tile

        //1 per second

	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
        if (isBigSword)
        {
            if (bigSwordTimer >= 0.0f)
            {
                bigSwordTimer += (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
                Debug.Log(bigSwordTimer);
                if (bigSwordTimer >= BigSwordDuration)
                {
                    bigSwordTimer = 0.0f;
                    spriteRenderer.sprite = NormalSword;
                    isBigSword = false;
                }
            }
        }
    }

    public override bool Use(Vector2 direction)
    {
        anim.SetBool(animAttack, true);
        return true;
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

        #region Flying Sword

        if (other is Crossbow)
        {
            // Check if we can launch a flying sword
            if (gameObject.activeSelf)
            {
                // Launch a Flying Sword
                var flyingsword = RefProjectileManager.FetchFlyingSword().GetComponent<FlyingSword>();
                var parent = GetComponentInParent<RPGPlayer>();

                if (flyingsword && parent)
                {
                    flyingsword.Activate(transform, this, parent.CurrentDirection, Range * RefProjectileManager.GetComponent<TileMap>().TileSize);
                }
            }
        }

        #endregion

        #region Big Sword
        else if (other is Wand)
        {
            // Set the sword to be larger
            spriteRenderer.sprite = BigSword;
            Damage = 20;
            isBigSword = true;
        }
        #endregion
    }
}
