public class SwordBlueprint : WeaponBlueprint
{
    public override Weapon GenerateRandom()
    {
        Sword worstVariant = (Sword)WorstPossibleVariant;
        Sword bestVariant = (Sword)BestPossibleVariant;

        // We cannot generate something if we have no valid upper or lower bounds
        if (worstVariant == null || bestVariant == null)
        {
            return null;
        }

        // Instantiate a weapon for us to use
        Sword generatedSword = Instantiate(worstVariant);

        // Create a Weapon reference for use with setCommonStats()
        Weapon generatedWeapon = generatedSword;

        setCommonStats(ref generatedWeapon);

        return generatedSword;
    }
}
