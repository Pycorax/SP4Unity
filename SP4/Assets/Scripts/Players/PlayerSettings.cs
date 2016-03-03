using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// PlayerSettings stores info about the player that is important
/// during and outside gameplay.
/// </summary>
public class PlayerSettings : MonoBehaviour, ISavable
{
    // Storage
    private Inventory inventory = new Inventory();

    // Skinnning
    public GameObject SkinBlueprint;
    private List<Skin> skinsInventory = new List<Skin>();

    // Statistics
    private int coins = 50;
    private int enemiesKilled = 0;

    // Save/Load Keys
    private string coinsKey = "coinsKey";
    private string enemiesKey = "enemiesKey";
    private string skinSizeKey = "skinSizeKey";
    private string skinKey = "skinKey";

    // Getters
    public Inventory PlayerInventory { get { return inventory; } }
    public List<Skin> SkinsInventory { get { return skinsInventory; } } 
    public Skin CurrentFirstSkin { get; set; }
    public Skin CurrentSecondSkin { get; set; }
    public int Coins { get { return coins; } }
    public int EnemiesKilled { get { return enemiesKilled; } }

    public bool AddItem(Item item)
    {
        return inventory.AddItem(item as Weapon);
    }

    public void AddCoins(int coinsToAdd)
    {
        coins += coinsToAdd;
    }

    public bool TakeCoins(int coinsToTake)
    {
        // Enough money
        if (coinsToTake <= coins)
        {
            coins -= coinsToTake;

            return true;
        }

        // Not enough money
        return false;
    }

    public void Save()
    {
        PlayerPrefs.SetInt(SaveClass.GetKey(SaveClass.Save_Keys.Key_Coins), coins);
        PlayerPrefs.SetInt(SaveClass.GetKey(SaveClass.Save_Keys.Key_Enemy_Killed), enemiesKilled);
        PlayerPrefs.SetInt(SaveClass.GetKey(SaveClass.Save_Keys.Key_Skin_Size), skinsInventory.Count);

        // Skins Inventory
        for (int skinIndex = 0; skinIndex < skinsInventory.Count; ++skinIndex)
        {
            Skin skin = skinsInventory[skinIndex];
            skin.Save(skinIndex);
        }

        // Current Skins
        // -- First Player
        string firstSkinURL = null;
        if (CurrentFirstSkin != null)
        {
            firstSkinURL = CurrentFirstSkin.SkinSpriteUrl;
        }
        PlayerPrefs.SetString(SaveClass.GetKey(SaveClass.Save_Keys.Key_Player1_Skin), firstSkinURL);
        // -- Second Player
        string secondSkinURL = null;
        if (CurrentFirstSkin != null)
        {
            secondSkinURL = CurrentSecondSkin.SkinSpriteUrl;
        }
        PlayerPrefs.SetString(SaveClass.GetKey(SaveClass.Save_Keys.Key_Player2_Skin), secondSkinURL);

        PlayerPrefs.Save();
    }

    public void Load()
    {
        coins = SaveClass.GetPlayerPrefInt(SaveClass.Save_Keys.Key_Coins);
        enemiesKilled = SaveClass.GetPlayerPrefInt(SaveClass.Save_Keys.Key_Enemy_Killed);
        int skinCount = SaveClass.GetPlayerPrefInt(SaveClass.Save_Keys.Key_Skin_Size);

        // Before we add more to it, let's clear it
        skinsInventory.Clear();

        // Load in the skins
        for (int skinIndex = 0; skinIndex < skinCount; ++skinIndex)
        {
            GameObject goSkin = Instantiate(SkinBlueprint);
            Skin skin = goSkin.GetComponent<Skin>();
            if (skin)
            {
                skin.Load(skinIndex);
                skinsInventory.Add(skin);
            }
        }

        // Current Skins
        string firstSkinURL = PlayerPrefs.GetString(SaveClass.GetKey(SaveClass.Save_Keys.Key_Player1_Skin));
        string secondSkinURL = PlayerPrefs.GetString(SaveClass.GetKey(SaveClass.Save_Keys.Key_Player2_Skin));
        if (firstSkinURL != null)
        {
            CurrentFirstSkin = Instantiate(SkinBlueprint).GetComponent<Skin>();
            CurrentFirstSkin.SkinSpriteUrl = firstSkinURL;
        }
        if (secondSkinURL != null)
        {
            CurrentSecondSkin = Instantiate(SkinBlueprint).GetComponent<Skin>();
            CurrentSecondSkin.SkinSpriteUrl = secondSkinURL;
        }
    }

    public void AddToSkinStorage(Skin skin)
    {
        // Do not allow readding to a similar skin
        if (skinsInventory.Any(s => s.SkinSpriteUrl == skin.SkinSpriteUrl))
        {
            return;
        }

        skinsInventory.Add(skin);
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }

    private int getPlayerPrefInt(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            throw new UnityException("Key '" + key + "' does not exist!");
        }

        return PlayerPrefs.GetInt(key);
    }
}
