using UnityEngine;
using System.Collections;

public class Sword : Weapon {

    public Sprite SwordSprite;

	// Use this for initialization
	void Start () {
	
        setName("Sword");
        setDamage(10);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Attack()
    {
        //Play Swinging Animation
        //Deal Damage?
    }
}
