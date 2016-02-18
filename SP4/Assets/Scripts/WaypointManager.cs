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
                w.SetUpConnections(waypointList, WaypointRayTraceRadius);

                // Draw the connections
                foreach (Waypoint neighbour in w.Neighbours)
                {
                    Debug.DrawLine(w.transform.position, neighbour.transform.position, Color.yellow, 0.0f, false);
                }
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

        // Initialize all the waypoints
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

        // Recursively attempt to reach the target
        return getWaypointToGetTo(current, target);
    }

    private Waypoint getWaypointToGetTo(Waypoint current, Waypoint target)
    {
        // Go to the shortest delta pos waypoint that is not a backtrack until no more routes can be found
        List<Waypoint> path = new List<Waypoint>();
        if (getWaypointToGetTo(current, target, ref path) && path.Count > 2/**/)
        {
            // Return the path
            return path[1];
        }       
        else
        {
            // There is no path there
            return null;
        }
    }

    private bool getWaypointToGetTo(Waypoint current, Waypoint target, ref List<Waypoint> backStack)
    {
        // Keep track of our back path
        backStack.Add(current);

        // Try with all children
        foreach (Transform t in current.transform)
        {
            // Check if it is a Waypoint
            Waypoint w = t.GetComponent<Waypoint>();
            if (w == null)
            {
                continue;
            }

            // Check if we've been to this place before

            // Is this the result?
            if (w == target)
            {
                return true;
            }

            // We managed to find it!
            if (getWaypointToGetTo(w, target, ref backStack))
            {
                return true;
            }
        }

        // If we reached this point, it means this node's children has nothing, so let's clear the backStack of us
        backStack.Remove(current);

        return false;
    }
}
