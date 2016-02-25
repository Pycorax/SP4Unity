using UnityEngine;

public abstract class WeaponBlueprint : MonoBehaviour
{
    [Tooltip("A prefab containing the worst possible stats for each field that this weapon could have. The actual weapon will be generated using this prefab as a base.")]
    public Weapon WorstPossibleVariant;

    [Tooltip("A prefab containing the best possible stats for each field that this weapon could have.")]
    public Weapon BestPossibleVariant;


    // Use this for initialization
    void Start()
    {
        if (WorstPossibleVariant.GetType() != BestPossibleVariant.GetType())
        {
            throw new UnityException("Weapon worst and best variants canno tbe different weapon types!");
        }
    }

    /// <summary>
    /// Function to generate a Weapon with random stats between WorstPossible Variant and BestPossibleVariant
    /// </summary>
    /// <returns>A randomized Weapon.</returns>
    public abstract Weapon GenerateRandom();

    /// <summary>
    /// Use this function to set the basic values for a weapon to be generated
    /// </summary>
    /// <param name="result">The Weapon to set the common stats for.</param>
    protected void setCommonStats(ref Weapon weap)
    {
        weap.Range = Random.Range(WorstPossibleVariant.Range, BestPossibleVariant.Range + 1.0f);
        weap.Damage = Random.Range(WorstPossibleVariant.Damage, BestPossibleVariant.Damage + 1);
        weap.FireRate = Random.Range(WorstPossibleVariant.FireRate, BestPossibleVariant.FireRate + 1.0f);
    }
}
