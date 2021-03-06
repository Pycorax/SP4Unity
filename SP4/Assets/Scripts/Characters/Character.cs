﻿using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Tooltip("The maximum health of the character.")]
    public int MaxHealth = 100;

    [Tooltip("The maximum Energy of the character.")]
    public int MaxEnergy = 100;

    // Health
    public int health;

    // Energy
    public float energy;
    private double energyregen;

    // Getters
    public int Health { get { return health; } }
    public bool IsAlive { get { return health > 0; } }
    public float Energy { get { return energy; } }

    public GameObject healthBar;
    public GameObject energyBar;


    // Use this for initialization
    protected virtual void Start()
    {
        health = MaxHealth;
        energy = MaxEnergy;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //HealthBar Testing
        HealthBarUpdate(Health);
        EnergyBarUpdate();
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

        if (this is Enemy.Enemy)
        {
            // Play the sound
            SoundManager.PlaySoundEffect(SoundManager.SoundEffect.Hit_3);
        }
        else if (this is RPGPlayer)
        {

            // Play the sound
            SoundManager.PlaySoundEffect(SoundManager.SoundEffect.Hit_2);
        }
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
        currentHP = Mathf.Clamp(currentHP, 0, MaxHealth);
        if (healthBar != null)
        {
            healthBar.transform.localScale = new Vector3(currentHP, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        }
    }

    #endregion

    #region Energy

    public void EnergyBarUpdate()
    {
        if (!energyBar)
        {
            return;
        }  

        // if energy is not equal to max energy
        if (energy < MaxEnergy)
        {
            // Regen energy every 2 seconds game time
            energyregen += TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
            if (energyregen >= 1)
            {
                energy += 5;
                energyregen = 0;
            }
        }

        // Scaling of current energy
        float currentEnergy = (float)energy / (float)MaxEnergy;
        // Energy does not overflow
        energy = Mathf.Clamp(energy, 0, MaxEnergy);
        energyBar.transform.localScale = new Vector3(currentEnergy, energyBar.transform.localScale.y, energyBar.transform.localScale.z);
    }

    public void UseEnergy(float energyused)
    {
        // Error checks
        if (energyused < 0)
        {
            throw new UnityException("Enter a positive number");
        }

        energy -= energyused;

        // Clamp the Energy so it wont go negative
        energy = Mathf.Clamp(energy, 0, MaxEnergy);
    }

    #endregion
}
