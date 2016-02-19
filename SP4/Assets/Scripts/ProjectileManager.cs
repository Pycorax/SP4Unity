using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour {

    public GameObject ArrowBlueprint;
    public int ArrowPoolSize = 20;
    private List<GameObject> ArrowPool = new List<GameObject>();

    

	// Use this for initialization
	void Start () {
	
        for(int i = 0; i <  ArrowPoolSize; i++)
        {
            GameObject arrow = Instantiate(ArrowBlueprint);
            arrow.SetActive(false);
            ArrowPool.Add(arrow);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public GameObject FetchArrow()
    {
        foreach(GameObject arrow in ArrowPool)
        {
            if(!arrow.activeSelf)
            {
                return arrow;
            }
        }
        return null;
    }
}
