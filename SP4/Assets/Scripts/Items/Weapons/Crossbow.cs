using UnityEngine;
using System.Collections;


public class Crossbow : Weapon {

    public Sprite CrossbowSprite;
    public Arrow arrow;
    public EmpoweredArrow empoweredArrow;


    public LayerMask notToHit;

    float timeToFire = 0;

    Transform firePoint;
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
        if(FireRate == 0)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {

            }
        }
	}

    public override void Use(Vector2 direction)
    {
        //Fire Projectile from projectile class
        //Play Firing Animation
        arrow.MoveTowards(direction);
    }
}
