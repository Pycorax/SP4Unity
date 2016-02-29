using System;
using System.Linq;
using UnityEngine;

public class Gachapon : MonoBehaviour
{
    public enum Spawnable
    {
        Crossbow,
        Shield,
        Sword,
        Wand
    }

    [Tooltip("The cost of a Gachapon capsule.")]
    public int Cost;

    [Tooltip("A reference to a Skins Manager.")]
    public SkinsManager SkinsManagerReference;

    [Tooltip("The list of weapon blueprints for the weapon to generate.")]
    public WeaponBlueprint[] WeaponBlueprints = new WeaponBlueprint[Enum.GetNames(typeof(Spawnable)).Length];

    [Tooltip("The list of percentages for spawning each weapon.")]
    public int[] WeaponSpawnRates = new int[Enum.GetNames(typeof(Spawnable)).Length];

	// Use this for initialization
	void Start ()
	{
	    GetRandomSkin();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Skin GetRandomSkin()
    {
        return SkinsManagerReference.GetRandomSkin();
    }

    public Weapon GetRandomWeapon()
    {
        // Add up the total probabilties
        int totalProbability = WeaponSpawnRates.Sum();

        // Calculate probability
        int probability = UnityEngine.Random.Range(0, totalProbability);

        // Decide what type of weapon to spawn
        int lowerCutOff = totalProbability - WeaponSpawnRates[Enum.GetNames(typeof(Spawnable)).Length - 1];
        int upperCutOff = totalProbability;
        for (int i = Enum.GetNames(typeof(Spawnable)).Length - 1; i >= 0; --i)
        {
            // If the probability calculated fits in here...
            if (probability > lowerCutOff && probability <= upperCutOff)
            {
                // Generate and return the weapon
                return WeaponBlueprints[i].GenerateRandom();
            }

            // Calibrate lowerCutOff and upperCutOff for next set
            if (i > 0)
            {
                lowerCutOff -= WeaponSpawnRates[i - 1];
                upperCutOff -= WeaponSpawnRates[i];
            }
        }

        return null;
    }

}
