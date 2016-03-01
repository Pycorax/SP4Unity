using System.Collections.Generic;
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
    public Skin CurrentSkin { get; set; }
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
        for (int skinIndex = 0; skinIndex < skinsInventory.Count; ++skinIndex)
        {
            Skin skin = skinsInventory[skinIndex];
            skin.Save(skinIndex);
        }
        PlayerPrefs.Save();
    }

    public void Load()
    {
        coins = SaveClass.GetPlayerPrefInt(SaveClass.Save_Keys.Key_Coins);
        enemiesKilled = SaveClass.GetPlayerPrefInt(SaveClass.Save_Keys.Key_Enemy_Killed);
    }

    public void AddToSkinStorage(Skin skin)
    {
        if (!skinsInventory.Contains(skin))
        {
            skinsInventory.Add(skin);
        }
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
