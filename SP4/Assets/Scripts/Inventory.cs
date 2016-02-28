using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, ISavable
{
    private List<Weapon> inventory = new List<Weapon>();

    // ISavable
    private string invPrefixKey = "inventory_";

    // Getters
    public List<Weapon> PlayerInventory { get { return inventory; } }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool AddItem(Weapon weapon)
    {
        if (inventory.Count <= 4)
        {
            inventory.Add(weapon);
            weapon.gameObject.SetActive(false);

            return true;
        }

        return false;
    }

    public void RemoveItem(Weapon weapon)
    {
        if (inventory.Count < 0)
        {
            return;
        }
        else
        {
            inventory.Remove(weapon);
        }
    }

    public void Save()
    {
        foreach (var weap in inventory)
        {
            
        }
    }

    public void Load()
    {
        throw new System.NotImplementedException();
    }
}
