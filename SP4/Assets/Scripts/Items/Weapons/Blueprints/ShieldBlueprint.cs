using UnityEngine;

public class ShieldBlueprint : WeaponBlueprint
{
    public override Weapon GenerateRandom()
    {
        Shield worstVariant = (Shield)WorstPossibleVariant;
        Shield bestVariant = (Shield)BestPossibleVariant;

        // We cannot generate something if we have no valid upper or lower bounds
        if (worstVariant == null || bestVariant == null)
        {
            return null;
        }

        // Instantiate a weapon for us to use
        Shield generatedShield = Instantiate(worstVariant);

        // Create a Weapon reference for use with setCommonStats()
        Weapon generatedWeapon = generatedShield;

        setCommonStats(ref generatedWeapon);

        // Arrow Barage
        generatedShield.BarrageArrows = Random.Range(worstVariant.BarrageArrows, bestVariant.BarrageArrows + 1);
        generatedShield.BarrageRange = Random.Range(worstVariant.BarrageRange, bestVariant.BarrageRange + 1.0f);
        generatedShield.BarrageFOV = Random.Range(worstVariant.BarrageFOV, bestVariant.BarrageFOV + 1.0f);

        // Big Shield
        generatedShield.BigShieldDuration = Random.Range(worstVariant.BigShieldDuration, bestVariant.BigShieldDuration + 1.0f);

        return generatedShield;
    }
}
