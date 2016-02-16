using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    private int wDamage;
    private string wName;

    enum WEAPON_TYPE
    {
        WEAPON_CROSSBOW,
        WEAPON_SWORD,
        WEAPON_SHIELD,
        WEAPON_WAND,
        WEAPON_HAMMER,
        NUM_TYPES
    }

    //=============================================================//

    //=Getters and Setters for Private Variables wName and wDamage=//
    protected virtual void setName(string name)
    {
        wName = name;
    }
    protected virtual string getName()
    {
        return wName;
    }

    protected virtual void setDamage(int dmg)
    {
        wDamage = dmg;
    }
    protected virtual int getDamage()
    {
        return wDamage;
    }
    //=============================================================//


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
