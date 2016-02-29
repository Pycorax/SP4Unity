using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerSettings stores info about the player that is important
/// during and outside gameplay.
/// </summary>
public class PlayerSettings : MonoBehaviour
{
    // Storage
    private Inventory inventory = new Inventory();

    // Skinnning
    private List<Skin> skinsInventory = new List<Skin>();

    // Statistics
    private int coins = 0;
    private int enemiesKilled = 0;

    // Save/Load Keys
    private string coinsKey = "coins";
    private string enemiesKey = "enemiesKey";

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

    public void Save()
    {
        PlayerPrefs.SetInt(coinsKey, coins);
        PlayerPrefs.SetInt(enemiesKey, enemiesKilled);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        coins = getPlayerPrefInt(coinsKey);
        enemiesKilled = getPlayerPrefInt(enemiesKey);
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
