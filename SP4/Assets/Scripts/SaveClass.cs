using System;
using UnityEngine;

public class SaveClass
{
    public enum Save_Keys
    {
        Key_Coins,
        Key_Enemy_Killed,
        Key_Skin_Size,
        Key_Skin_URL,
        Key_Level,
        Key_Name,

        // Level editor (Create new)
        Key_Level_Editor_Creation, // True for creating new, False for loading
        Key_Level_Editor_Row,
        Key_Level_Editor_Col,

        // Weapon
        Key_Player1_Left,
        Key_Player1_Right,
        Key_Player2_Left,
        Key_Player2_Right,
    };

    public static string[] Keys = Enum.GetNames(typeof(Save_Keys));

    public static string GetKey(Save_Keys type)
    {
        return Keys[(int)type];
    }

    public static int GetPlayerPrefInt(Save_Keys type)
    {
        if (!PlayerPrefs.HasKey(type.ToString()))
        {
            throw new UnityException("Key '" + type + "' does not exist!");
        }

        return PlayerPrefs.GetInt(type.ToString());
    }

    public static string GetPlayerPrefString(Save_Keys type, string extra = "")
    {
        if (!PlayerPrefs.HasKey(type.ToString() + extra))
        {
            throw new UnityException("Key '" + type + "' does not exist!");
        }

        return PlayerPrefs.GetString(type.ToString() + extra);
    }

    public static float GetPlayerPrefFloat(Save_Keys type)
    {
        if (!PlayerPrefs.HasKey(type.ToString()))
        {
            throw new UnityException("Key '" + type + "' does not exist!");
        }

        return PlayerPrefs.GetFloat(type.ToString());
    }
}
