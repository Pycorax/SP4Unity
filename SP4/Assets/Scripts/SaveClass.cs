using System;
using UnityEngine;

public class SaveClass
{
    public enum Save_Keys
    {
        Key_Coins,
        Key_Enemy_Killed,
        Key_Skin_Size,
        Key_Skin_URL
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

    public static string GetPlayerPrefString(Save_Keys type)
    {
        if (!PlayerPrefs.HasKey(type.ToString()))
        {
            throw new UnityException("Key '" + type + "' does not exist!");
        }

        return PlayerPrefs.GetString(type.ToString());
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
