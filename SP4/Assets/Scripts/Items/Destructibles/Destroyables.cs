using UnityEngine;

public abstract class Destroyables : Item
{
    [Tooltip("A reference to a GameManager for statistics tracking.")]
    public GameManager Manager;

    public override bool Use()
	{
        return true;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Projectile>() != null)
        {
            anim.enabled = true;
        }
    }
}
