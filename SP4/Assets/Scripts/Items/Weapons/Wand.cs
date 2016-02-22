using UnityEngine;

public class Wand : Weapon
{
    Transform firePoint;

    // Use this for initialization
    protected override void Start()
    {
        firePoint = transform.FindChild("FirePoint");

        if (!firePoint)
        {
            Debug.LogError("No FirePoint");
        }
    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
    }

    public override bool Use(Vector2 direction)
    {
        if (base.Use(direction))
        {
            GameObject p = RefProjectileManager.FetchLightning();
            if (p)
            {
                p.GetComponent<Lightning>().Activate(firePoint, this, direction, Range * RefProjectileManager.GetComponent<TileMap>().TileSize);
                return true;
            }
        }
        return false;
    }

    protected override void combinedUse(Weapon other, params object[] details)
    {
        
    }
}
