using System.Linq;
using UnityEngine;


public class Crossbow : Weapon
{
    // State of the Crossbow
    private bool empowered = false;

    // Shooting
    private Transform firePoint;

    // Animation
    [Tooltip("The set of animations to use for normal mode.")]
    public RuntimeAnimatorController NormalAnimationSet;
    [Tooltip("The set of animations to use for empowered mode.")]
    public RuntimeAnimatorController EmpoweredAnimationSet;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        firePoint = transform.FindChild("FirePoint");
    }

    public override bool Use(Vector2 direction)
    {
        if (base.Use(direction))
        {
            Projectile toShoot = null;

            // Decide which type of arrow to shoot
            if (empowered)
            {
                // Fetch an Empowered Arrow
                toShoot = RefProjectileManager.FetchEmpoweredArrow().GetComponent<EmpoweredArrow>();


                // Play the sound
                SoundManager.PlaySoundEffect(SoundManager.SoundEffect.Weapon_Attack_1);

                // Turn off the empowerment
                setEmpowered(false);
            }
            else
            {
                // Fetch an Arrow
                toShoot = RefProjectileManager.FetchArrow().GetComponent<Arrow>();

                // Play the sound
                SoundManager.PlaySoundEffect(SoundManager.SoundEffect.Weapon_Attack_3);
            }

            // Error Checking
            if (toShoot != null) 
            {
                toShoot.Activate(firePoint, this, direction, Range * RefProjectileManager.GetComponent<TileMap>().TileSize);
                return true;
            }
        }
        return false;
    }

    protected override void combinedUse(Weapon other, params object[] details)
    {
        #region Explosive Arcane Shot (Wand => Crossbow)

        if (other is Wand)
        {
            // Find the projectile
            var arrow = details.OfType<Projectile>().FirstOrDefault();

            // Check if we found it
            if (arrow != null)
            {
                // Set the crossbow as an empowered crossbow
                setEmpowered(true);
            }

            // Play the sound
            SoundManager.PlaySoundEffect(SoundManager.SoundEffect.Combo_ArcaneShot);
        }

        #endregion
    }

    private void setEmpowered(bool empowerment)
    {
        // Set the bool
        empowered = empowerment;

        // Update the controller
        if (empowered)
        {
            anim.runtimeAnimatorController = EmpoweredAnimationSet;
        }
        else
        {
            anim.runtimeAnimatorController = NormalAnimationSet;
        }
    }
}
