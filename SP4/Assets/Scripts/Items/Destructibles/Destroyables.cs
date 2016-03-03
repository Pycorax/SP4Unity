using UnityEngine;

public abstract class Destroyables : Item
{
    [Tooltip("A reference to a GameManager for statistics tracking.")]
    public GameManager Manager;

    public override bool Use()
	{
        return true;
	}

    public void Destroy()
    {
        anim.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Projectile>() != null || other.GetComponent<Explosion>() != null)
        {
            Destroy();
        }
    }
}
