using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // The ResourceController to manipulate
        SoundManager SM = (SoundManager)target;


        EditorGUILayout.PrefixLabel("Background Music");

        // Store the Enum details
        var bgmEnumType = typeof(SoundManager.BackgroundMusic);
        var bgmEnumNames = Enum.GetNames(bgmEnumType);

        // If Managers is available to be set (Not-Runtime)
        if (SM.BackgroundMusicToLoad != null)
        {
            // Loop through all managers and show a field to allow editing
            for (int i = 0; i < Enum.GetValues(bgmEnumType).Length; i++)
            {
                SM.BackgroundMusicToLoad[i] = (AudioClip)EditorGUILayout.ObjectField(bgmEnumNames[i], SM.BackgroundMusicToLoad[i], typeof(AudioClip), true);
            }
        }

        EditorGUILayout.Separator();
        EditorGUILayout.PrefixLabel("Sound Effects");
        // Store the Enum details
        var sfxEnumType = typeof(SoundManager.SoundEffect);
        var sfxEnumNames = Enum.GetNames(sfxEnumType);

        // If Managers is available to be set (Not-Runtime)
        if (SM.SoundEffectsToLoad != null)
        {
            // Loop through all managers and show a field to allow editing
            for (int i = 0; i < Enum.GetValues(sfxEnumType).Length; i++)
            {
                SM.SoundEffectsToLoad[i] = (AudioClip)EditorGUILayout.ObjectField(sfxEnumNames[i], SM.SoundEffectsToLoad[i], typeof(AudioClip), true);
            }
        }
    }
}
