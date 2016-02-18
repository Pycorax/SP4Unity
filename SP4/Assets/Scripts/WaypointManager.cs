using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointManager : MonoBehaviour
{
	[Tooltip("The radius that the ray trace will be using. Specify the enemy size as this will be used for enemy path finding.")]
	public float WaypointRayTraceRadius = 100.0f;
    [Tooltip("Set to true if you want to initialize immediately on start up. If not, you will need to call SyncWaypoints() manually.")]
    public bool SyncOnStartUp = true;

	// Holds a list of waypoints that we set up in Start() to return later
	private List<Waypoint> waypointList;
	
	// Getters
	public List<Waypoint> Waypoints { get { return waypointList;} }

	// Use this for initialization
	void Start ()
	{
        // If indicated to sync on start up, then we do it, if not, give the dev the control
        if (SyncOnStartUp)
        {
            SyncWaypoints();
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

    public void SyncWaypoints()
    {
        // If a List<> was not generated before, create one now
        if (waypointList == null)
        {
            waypointList = new List<Waypoint>(transform.childCount);
        }
        else
        {
            // Clear the list if we are syncing waypoints again so that we don't have duplicates
            waypointList.Clear();
        }

        // Generate a list of waypoints
        foreach (Transform go in transform)
        {
            Waypoint w = go.GetComponent<Waypoint>();
            if (go != null)
            {
                waypointList.Add(w);
            }
        }

        // Initialize all the waypoints
        foreach (Waypoint w in waypointList)
        {
            w.SetUpConnections(waypointList, WaypointRayTraceRadius);
        }
    }
}
