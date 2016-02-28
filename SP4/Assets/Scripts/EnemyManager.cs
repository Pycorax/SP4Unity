using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    [Tooltip("A reference to the WaypointMap for enemies to be initialized with.")]
    public WaypointManager WaypointMap;
    [Tooltip("A reference to Player1 for enemies to track.")]
    public RPGPlayer RefPlayer1;
    [Tooltip("A reference to Player2 for enemies to track.")]
    public RPGPlayer RefPlayer2;

    // Enemy Statistics
    private int enemiesKilled = 0;

    // Enemy Resource Manager
    private ResourceManager list;

    // Getters
    public int EnemiesKilled { get { return enemiesKilled; } }

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

    // Use this function to confirm a kill
    public void ConfirmKill()
    {
        enemiesKilled++;
    }

    private void setCommonData(GameObject go)
    {
        Enemy.Enemy e = go.GetComponent<Enemy.Enemy>();
        if (e)
        {
            e.Manager = this;
            e.WaypointMap = WaypointMap;
            e.PlayerList.Add(RefPlayer1.gameObject);
            e.PlayerList.Add(RefPlayer2.gameObject);
        }
    }
}
