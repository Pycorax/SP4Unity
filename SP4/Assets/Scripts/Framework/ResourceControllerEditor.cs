#if UNITY_EDITOR

using System;
using UnityEditor;

[CustomEditor(typeof(ResourceController))]
public class ResourceControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // The ResourceController to manipulate
        ResourceController RC = (ResourceController)target;

        // Store the Enum details
        var enumType = typeof(ResourceController.Resource);
        var enumNames = Enum.GetNames(enumType);

        // If Managers is available to be set (Not-Runtime)
        if (RC.Managers != null)
        {
            // Loop through all managers and show a field to allow editing
            for (int i = 0; i < Enum.GetValues(enumType).Length; i++)
            {
                RC.Managers[i] = (ResourceManager)EditorGUILayout.ObjectField(enumNames[i], RC.Managers[i], typeof(ResourceManager), true);
            }
        }
        else
        {
            EditorGUILayout.LabelField("Runtime modification is disallowed.");
        }
    }
}

#endif