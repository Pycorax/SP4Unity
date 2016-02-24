using UnityEngine;

public class Sword : Weapon
{

    private SpriteRenderer spriteRenderer;
    public Sprite BigSword;
	// Use this for initialization
	protected override void Start () {

        Name = "Sword";
        Damage = 10;

        spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();

        //1 Tile

        //1 per second

	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
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
            // -- Check if we found a projectile
            if (projectile != null)
            {
                // Destroy it
                projectile.Disable();
            }

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
            // -- Check if we found a projectile
            if (projectile != null)
            {
                // Destroy it
                projectile.Disable();
            }

            // Set the sword to be larger
            spriteRenderer.sprite = BigSword;

        }
        #endregion
    }
}
