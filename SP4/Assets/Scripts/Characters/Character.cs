using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Tooltip("The maximum health of the character.")]
    public int MaxHealth = 100;

    // Health
    public int health;

    // Getters
    public int Health { get { return health; } }

    public GameObject healthBar;


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

    public void HealthBarUpdate(int health)
    {
        float currentHP = (float)health / (float)MaxHealth;
        Mathf.Clamp(currentHP, 0, MaxHealth);
        healthBar.transform.localScale = new Vector3(currentHP, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    #endregion
}
