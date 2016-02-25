public class WandBlueprint : WeaponBlueprint
{
    public override Weapon GenerateRandom()
    {
        Wand worstVariant = (Wand)WorstPossibleVariant;
        Wand bestVariant = (Wand)BestPossibleVariant;

        // We cannot generate something if we have no valid upper or lower bounds
        if (worstVariant == null || bestVariant == null)
        {
            return null;
        }

        // Instantiate a weapon for us to use
        Wand generatedWand = Instantiate(worstVariant);

        // Create a Weapon reference for use with setCommonStats()
        Weapon generatedWeapon = generatedWand;

        setCommonStats(ref generatedWeapon);

        return generatedWand;
    }
}
