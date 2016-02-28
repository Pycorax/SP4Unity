using UnityEngine;
using System.Collections;

public class Collectible : Item
{
    [Tooltip("A reference to a GameManager for statistics tracking.")]
    public GameManager Manager;

    public override bool Use()
    {
        return true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<RPGPlayer>() != null)
        {
            Deactivate();
        }
    }
}
