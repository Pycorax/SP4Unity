#define WAYPOINT_DEBUG

using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [Tooltip("The radius that the ray trace will be using. Specify the enemy size as this will be used for enemy path finding.")]
    public float WaypointRayTraceRadius = 50.0f;
    [Tooltip("Set to true if you want to initialize immediately on start up. If not, you will need to call SyncWaypoints() manually.")]
    public bool SyncOnStartUp = true;
    [Tooltip("Design and debugging tool. When enabled, waypoint neighbours will be recalculated every frame and the connections will be rendered.")]
    public bool DrawConnections = false;

    // Holds a list of waypoints that we set up in Start() to return later
    private List<Waypoint> waypointList;

    // Getters
    public List<Waypoint> Waypoints { get { return waypointList; } }

    // Use this for initialization
    void Start()
    {
        // If indicated to sync on start up, then we do it, if not, give the dev the control
        if (SyncOnStartUp)
        {
            SyncWaypoints();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DrawConnections)
        {
            // Loop through each
            foreach (Waypoint w in waypointList)
            {
                // Recalculate all connections
                //w.SetUpConnections(waypointList, WaypointRayTraceRadius);

                // Draw the connections
                foreach (Waypoint neighbour in w.Neighbours)
                {
                    Debug.DrawLine(w.transform.position, neighbour.transform.position, Color.yellow, 0.0f, false);
                }
            }
        }
    }
    /// <summary>
    /// Function to draw the paths of the waypoints on the screen. However, it does not work as the Collider 
    /// doesn't seem to be enabled in the UnityEditor. Unity bug maybe.
    /// </summary>
    void OnDrawGizmos()
    {
        // Get a list of all waypoints
        List<Waypoint> listOfWaypoints = new List<Waypoint>();
        foreach (Transform t in transform)
        {
            Waypoint w = t.GetComponent<Waypoint>();

            if (w != null)
            {
                listOfWaypoints.Add(w);
            }
        }

        // Loop through each
        foreach (Waypoint w in listOfWaypoints)
        {
            // Recalculate all connections
            w.SetUpConnections(listOfWaypoints, WaypointRayTraceRadius);
        }

        foreach (Waypoint w in listOfWaypoints)
        {
            // Draw the connections
            foreach (Waypoint neighbour in w.Neighbours)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(w.transform.position, neighbour.transform.position);
            }
        }
    }

    /// <summary>
    /// Function to add in a waypoint into the system.
    /// </summary>
    /// <param name="w">The waypoint to add into the system.</param>
    public void Add(Waypoint w)
    {
        w.transform.parent = transform;
    }

    /// <summary>
    /// Function to initialize all the Waypoints to link with one another
    /// </summary>
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

        // Initialize all the waypoints with that list
        foreach (Waypoint w in waypointList)
        {
            w.SetUpConnections(waypointList, WaypointRayTraceRadius);
        }
    }

    public Waypoint FindNearestWaypoint(Vector2 pos)
    {
        Waypoint nearestWaypoint = null;
        float nearestDist = float.MaxValue;

        foreach (Waypoint w in waypointList)
        {
            // Calculate the distance to the waypoint
            float dist = ((Vector2)w.transform.position - pos).sqrMagnitude;

            // Check if this is lower than the previous nearest
            if (nearestWaypoint == null || dist < nearestDist)
            {
                nearestWaypoint = w;
                nearestDist = ((Vector2)w.transform.position - pos).sqrMagnitude;
            }
        }

        return nearestWaypoint;
    }

    public Waypoint GetNearestWaypointToGoTo(Vector2 currentPos, Vector2 targetPos)
    {
        // Find the waypoint nearest to us
        Waypoint current = FindNearestWaypoint(currentPos);

        // Find the waypoint nearest to the target
        Waypoint target = FindNearestWaypoint(targetPos);

        // Calculate and return the next point to go to
        return GetNearestWaypointToGoTo(current, target);
    }

    public Waypoint GetNearestWaypointToGoTo(Waypoint currentPos, Waypoint targetPos)
    {
        // Calculate and return the next point to go to
        Stack<Waypoint> path = Pathfinding.Dijkstra(waypointList, currentPos, targetPos);

        // A path was found...
        if (path.Count > 0)
        {
            return path.Peek();
        }

        // There is no more places to go
        return null;
    }
}
