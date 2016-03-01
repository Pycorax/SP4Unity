#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    private bool showBGM = true;
    private bool showSFX = true;

    public override void OnInspectorGUI()
    {
        // The ResourceController to manipulate
        SoundManager SM = (SoundManager)target;

        // Fold Out
        showBGM = EditorGUILayout.Foldout(showBGM, "Background Music");

        // If Managers is available to be set (Not-Runtime)
        if (showBGM && SM.BackgroundMusicToLoad != null)
        {
            // Store the Enum details
            var bgmEnumType = typeof(SoundManager.BackgroundMusic);
            var bgmEnumNames = Enum.GetNames(bgmEnumType);

            // Loop through all managers and show a field to allow editing
            for (int i = 0; i < Enum.GetValues(bgmEnumType).Length; i++)
            {
                SM.BackgroundMusicToLoad[i] = (AudioClip)EditorGUILayout.ObjectField(bgmEnumNames[i], SM.BackgroundMusicToLoad[i], typeof(AudioClip), true);
            }
        }

        // Fold Out
        showSFX = EditorGUILayout.Foldout(showSFX, "Sound Effects");

        // If Managers is available to be set (Not-Runtime)
        if (showSFX && SM.SoundEffectsToLoad != null)
        {
            // Store the Enum details
            var sfxEnumType = typeof(SoundManager.SoundEffect);
            var sfxEnumNames = Enum.GetNames(sfxEnumType);

            // Loop through all managers and show a field to allow editing
            for (int i = 0; i < Enum.GetValues(sfxEnumType).Length; i++)
            {
                SM.SoundEffectsToLoad[i] = (AudioClip)EditorGUILayout.ObjectField(sfxEnumNames[i], SM.SoundEffectsToLoad[i], typeof(AudioClip), true);
            }
        }
    }
}

#endif