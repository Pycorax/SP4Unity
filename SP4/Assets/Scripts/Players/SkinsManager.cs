using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{
    [Tooltip("A list of available Skins.")]
    public List<GameObject> SkinsList;

    // Private store of all instantiated skins
    private readonly List<GameObject> instantiatedSkins = new List<GameObject>(); 

    void Start()
    {
        // Clear the List in case
        instantiatedSkins.Clear();

        // Load the skins
        foreach (var s in SkinsList)
        {
            // Create a skin from the list
            var skin = Instantiate(s);
            skin.transform.parent = transform;
            
            // Get the Skin componenet and load the skin
            var skinComp = skin.GetComponent<Skin>();
            if (skinComp != null)
            {
                skinComp.Load();
            }

            // Add the Object to the list
            instantiatedSkins.Add(skin);
        }
    }

    public Skin GetRandomSkin()
    {
        // Get a list of skin components
        var skins = (from skin in instantiatedSkins where skin.GetComponent<Skin>() != null select skin.GetComponent<Skin>()).ToList();

        // Add up the total probabilties
        int totalProbability = skins.Sum(s => s.Chance);

        // Calculate probability
        int probability = Random.Range(0, totalProbability);

        // Decide which skin to spawn
        int lowerCutOff = totalProbability - skins[skins.Count - 1].Chance;
        int upperCutOff = totalProbability;
        for (int i = skins.Count - 1; i >= 0; --i)
        {
            // If the probability calculated fits in here...
            if (probability > lowerCutOff && probability <= upperCutOff)
            {
                // Generate and return the Skin
                return skins[i];
            }

            // Calibrate lowerCutOff and upperCutOff for next set
            if (i > 0)
            {
                lowerCutOff -= skins[i - 1].Chance;
                upperCutOff -= skins[i].Chance;
            }
        }

        return null;
    }
}
