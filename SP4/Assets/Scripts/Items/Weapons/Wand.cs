using UnityEngine;

public class Wand : Weapon
{
    public Lightning lightning;
	// Use this for initialization
	protected override void Start () {
        Name = "Wand";
        Damage = 3;

        //1 Tile
        Range = 5;

        //1 per second
        FireRate = 1;
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
    }

    public override bool Use(Vector2 direction)
    {
        return false;
    }

    protected override void combinedUse(Weapon other, params object[] details)
    {
        
    }
}
