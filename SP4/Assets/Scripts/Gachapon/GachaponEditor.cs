#if UNITY_EDITOR

using System;
using UnityEditor;

[CustomEditor(typeof(Gachapon))]
public class GachaponEditor : Editor
{
    private bool showSkins = true;
    private bool showWeapBlueprints = true;
    private bool showSpawnRates = true;

    public override void OnInspectorGUI()
    {                                                                                        
        // The Gachapon script to manipulate
        Gachapon gacha = (Gachapon)target;

        // Store the Enum details
        var weapType = typeof(Gachapon.Spawnable);
        var weapEnumNames = Enum.GetNames(weapType);

        // Cost
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Cost");
        gacha.Cost = EditorGUILayout.IntField(gacha.Cost);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        // Skins
        showSkins = EditorGUILayout.Foldout(showSkins, "Skins Manager");
        gacha.SkinsManagerReference = (SkinsManager)EditorGUILayout.ObjectField(gacha.SkinsManagerReference, typeof(SkinsManager), true);
;
        EditorGUILayout.Separator();

        // Weapons
        // -- Blueprints
        showWeapBlueprints = EditorGUILayout.Foldout(showWeapBlueprints, "Weapon Blueprints");
        if (showWeapBlueprints)
        {
            // Loop through all Weapon Blueprints and show a field to allow editing
            for (int i = 0; i < Enum.GetValues(weapType).Length; i++)
            {
                gacha.WeaponBlueprints[i] = (WeaponBlueprint)EditorGUILayout.ObjectField(weapEnumNames[i], gacha.WeaponBlueprints[i], typeof(WeaponBlueprint), false);
            }
        }

        // -- Chance
        showSpawnRates = EditorGUILayout.Foldout(showSpawnRates, "Spawn Rates");
        if (showSpawnRates)
        {
            // Loop through all probabilities and show a field to allow editing
            for (int i = 0; i < Enum.GetValues(weapType).Length; i++)
            {
                gacha.WeaponSpawnRates[i] = EditorGUILayout.IntField(weapEnumNames[i], gacha.WeaponSpawnRates[i]);
            }
        }
    }
}

#endif