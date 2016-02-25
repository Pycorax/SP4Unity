public class CrossbowBlueprint : WeaponBlueprint
{
    public override Weapon GenerateRandom()
    {
        Crossbow worstVariant = (Crossbow)WorstPossibleVariant;
        Crossbow bestVariant = (Crossbow)BestPossibleVariant;

        // We cannot generate something if we have no valid upper or lower bounds
        if (worstVariant == null || bestVariant == null)
        {
            return null;
        }

        // Instantiate a weapon for us to use
        Crossbow generatedCrossbow = Instantiate(worstVariant);

        // Create a Weapon reference for use with setCommonStats()
        Weapon generatedWeapon = generatedCrossbow;

        setCommonStats(ref generatedWeapon);

        return generatedCrossbow;
    }
}
