using System;
using UnityEditor;

[CustomEditor(typeof(Gachapon))]
public class GachaponEditor : Editor
{
    private bool showWeapBlueprints = true;
    private bool showSpawnRates = true;

    public override void OnInspectorGUI()
    {                                                                                        
        // The Gachapon script to manipulate
        Gachapon gacha = (Gachapon)target;

        // Store the Enum details
        var weapType = typeof(Gachapon.Spawnable);
        var weapEnumNames = Enum.GetNames(weapType);

        // Foldout Controllers
        showWeapBlueprints = EditorGUILayout.Foldout(showWeapBlueprints, "Weapon Blueprints");
        if (showWeapBlueprints)
        {
            // Loop through all Weapon Blueprints and show a field to allow editing
            for (int i = 0; i < Enum.GetValues(weapType).Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(weapEnumNames[i]);
                gacha.WeaponBlueprints[i] = (WeaponBlueprint)EditorGUILayout.ObjectField(gacha.WeaponBlueprints[i], typeof(WeaponBlueprint), false);
                EditorGUILayout.EndHorizontal();
            }
        }

        // Foldout
        showSpawnRates = EditorGUILayout.Foldout(showSpawnRates, "Spawn Rates");
        if (showSpawnRates)
        {
            // Loop through all probabilities and show a field to allow editing
            for (int i = 0; i < Enum.GetValues(weapType).Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(weapEnumNames[i]);
                gacha.WeaponSpawnRates[i] = EditorGUILayout.IntField(gacha.WeaponSpawnRates[i]);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}