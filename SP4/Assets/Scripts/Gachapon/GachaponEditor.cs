using System;
using UnityEditor;

[CustomEditor(typeof(Gachapon))]
public class GachaponEditor : Editor
{
    public override void OnInspectorGUI()
    {                                                                                        
        // The ResourceController to manipulate
        Gachapon gacha = (Gachapon)target;

        // Title
        EditorGUILayout.PrefixLabel("Weapon Blueprints");

        // Store the Enum details
        var weapType = typeof(Gachapon.Spawnable);
        var weapEnumNames = Enum.GetNames(weapType);

        // Loop through all Weapon Blueprints and show a field to allow editing
        for (int i = 0; i < Enum.GetValues(weapType).Length; i++)
        {
            EditorGUILayout.PrefixLabel(weapEnumNames[i]);
            gacha.WeaponBlueprints[i] = (WeaponBlueprint)EditorGUILayout.ObjectField(gacha.WeaponBlueprints[i], typeof(WeaponBlueprint), false);
        }

        // Split
        EditorGUILayout.Separator();

        // Title
        EditorGUILayout.PrefixLabel("Spawn Rates");
        // Loop through all probabilities and show a field to allow editing
        for (int i = 0; i < Enum.GetValues(weapType).Length; i++)
        {
            EditorGUILayout.PrefixLabel(weapEnumNames[i]);
            gacha.WeaponSpawnRates[i] = EditorGUILayout.IntField(gacha.WeaponSpawnRates[i]);
        }                                                                                        
        
    }
}