using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gachapon : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int RandomInt()
    {
        int randnum;

        randnum = Random.Range(0, 1000);

        return randnum;
    }

    public Item RandomItem(List<Item> itemList)
    {
        return null;
    }

}
