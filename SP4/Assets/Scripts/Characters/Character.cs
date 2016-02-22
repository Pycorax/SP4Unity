using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Tooltip("The maximum health of the character.")]
    public int MaxHealth = 100;

    // Health
    protected int health;

    // Getters
    public int Health { get { return health; } }

    // Use this for initialization
    protected virtual void Start ()
    {
        health = MaxHealth;
	}

    // Update is called once per frame
    protected virtual void  Update ()
    {
	
	}

    #region Health

    public void Injure(int damage)
    {
        // Error checks
        if (damage < 0)
        {
            throw new UnityException("Please don't use Injure() to heal!");
        }

        health -= damage;

        // Clamp the health so we don't go crazy with the health accidentally
        health = Mathf.Clamp(health, 0, MaxHealth);
    }

    public void Heal(int healing)
    {
        // Error checks
        if (healing < 0)
        {
            throw new UnityException("Please don't use Heal() to injure!");
        }

        health += healing;

        // Clamp the health so we don't go crazy with the health accidentally
        health = Mathf.Clamp(health, 0, MaxHealth);
    }

    #endregion
}
