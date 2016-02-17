using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{

    private List<Weapon> inventory = new List<Weapon>();

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

    public void AddItem(Weapon weapon)
    {
        if (inventory.Count <= 4)
        {
            inventory.Add(weapon);
            weapon.gameObject.SetActive(false);
        }
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
}
