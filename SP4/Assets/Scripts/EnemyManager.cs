using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public WaypointManager WaypointMap;
    private ResourceManager list;

	// Use this for initialization
	void Start ()
    {
        list = GetComponent<ResourceManager>();
        if (list)
        {
            foreach (GameObject go in list.ResourceList)
            {
                setCommonData(go);
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void setCommonData(GameObject go)
    {
        Enemy.Enemy e = go.GetComponent<Enemy.Enemy>();
        if (e)
        {
            e.WaypointMap = WaypointMap;
        }
    }
}
