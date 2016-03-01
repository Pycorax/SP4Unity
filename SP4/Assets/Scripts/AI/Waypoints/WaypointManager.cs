#define WAYPOINT_DEBUG

using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [Tooltip("The radius that the ray trace will be using. Specify the enemy size as this will be used for enemy path finding.")]
    public float WaypointRadius = 50.0f;
    [Tooltip("Set to true if you want to initialize immediately on start up. If not, you will need to call SyncWaypoints() manually.")]
    public bool SyncOnStartUp = true;
    [Tooltip("Set to true if you want Waypoints to always recalculate their neighbours every frame.")]
    public bool AlwaysRecalculate = false;
    [Tooltip("Design and debugging tool. When enabled, waypoint neighbours will be recalculated every frame and the connections will be rendered.")]
    public bool DrawConnections = false;
    [Tooltip("If true, the radius of the raytrace between Waypoints will be drawn.")]
    public bool GizmoWaypointRadiusDraw = true;
    [Tooltip("If true, the raytrace between Waypoints will be drawn.")]
    public bool GizmoWaypointRayDraw = true;
    [Tooltip("If true, the size of the Waypoints will automatically resize according to the specified radius. Else, the waypoint's local scale will be used.")]
    public bool AutoScaleRadius = true;
    [Tooltip("If true, then recheck children for Waypoints every second.")]
    public bool MessyWaypoints = false;
    [Tooltip("The ResourceManager for a LineManager")]
    public ResourceManager LineManager;

    // MessyWaypoints timer
    private float messyWaypointsTimer = 0.0f;
    private const float MESSY_WAYPOINTS_TIME_DELAY = 0.5f;

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
        // Means that the Waypoints will be moving all the time, so we need to reorganize and such
        if (MessyWaypoints)
        {
            // Update Timer
            messyWaypointsTimer += (float)TimeManager.GetDeltaTime(TimeManager.TimeType.SceneOrganization);

            // Reorganize if need be
            if (messyWaypointsTimer > MESSY_WAYPOINTS_TIME_DELAY)
            {
                // Clear the List
                waypointList.Clear();

                // Rebuild the List
                foreach (Transform t in transform)
                {
                    Waypoint w = t.GetComponent<Waypoint>();

                    if (w != null)
                    {
                        waypointList.Add(w);
                    }
                }

                // Reset the Timer
                messyWaypointsTimer = 0.0f;
            }
        }

        // Clear the Lines
        if (LineManager.ActiveResourcesList.Count > 0)
        {
            LineManager.ResetAll();
        }

        // Loop through each
        foreach (Waypoint w in waypointList)
        {
            // Error Checking
            if (w == null)
            {
                continue;
            }

            if (AlwaysRecalculate)
            {
                if (AutoScaleRadius)
                {
                    w.SetUpConnections(waypointList, WaypointRadius);
                }
                else
                {
                    w.SetUpConnections(waypointList, transform.localScale.x);
                }
            }

            if (DrawConnections)
            {
                // Draw the connections
                foreach (Waypoint neighbour in w.Neighbours)
                {
                    // Error Checking
                    if (neighbour == null)
                    {
                        continue;
                    }

                    var rsc = LineManager.Fetch();
                    if (rsc != null)
                    {
                        rsc.SetActive(true);
                        var line = rsc.GetComponent<TrackerLine>();
                        line.Init(w.gameObject, neighbour.gameObject);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Function to draw the paths of the waypoints on the screen. However, it does not work as the Collider 
    /// doesn't seem to be enabled in the UnityEditor. Unity bugs maybe.
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
            // Error Checking
            if (w == null)
            {
                continue;
            }

            // Recalculate all connections
            if (AlwaysRecalculate)
            {
                w.SetUpConnections(listOfWaypoints, WaypointRadius);
            }

            if (AutoScaleRadius)
            {
                // Set the size of each Waypoint to the WaypointRayTraceRadius
                w.transform.localScale = new Vector3(WaypointRadius * 2.0f, WaypointRadius * 2.0f, WaypointRadius * 2.0f);
            }

            // Set colour according to number of associations
            if (w.Neighbours.Count > 0)
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            // Draw a sphere to show the waypoint position
            Gizmos.DrawWireSphere(w.transform.position, WaypointRadius);
        }

        foreach (Waypoint w in listOfWaypoints)
        {
            // Draw the connections
            foreach (Waypoint neighbour in w.Neighbours)
            {
                // Error Checking
                if (neighbour == null)
                {
                    continue;
                }

                // Draw the radius lines
                if (GizmoWaypointRadiusDraw)
                {
                    // Calculate the direction perpendicular to the direction of the neighbour
                    Vector2 dir = w.transform.position - neighbour.transform.position;
                    if (dir != Vector2.zero)
                    {
                        dir.Normalize();
                    }
                    Vector2 right = new Vector2(dir.y, dir.x);

                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(w.transform.position + (Vector3)right * WaypointRadius, neighbour.transform.position + (Vector3)right * WaypointRadius);
                    Gizmos.DrawLine(w.transform.position - (Vector3)right * WaypointRadius, neighbour.transform.position - (Vector3)right * WaypointRadius);
                }

                // Draw the ray lines
                if (GizmoWaypointRayDraw)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(w.transform.position, neighbour.transform.position);
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

        // Initialize all the waypoints with that list
        foreach (Waypoint w in waypointList)
        {
            w.SetUpConnections(waypointList, WaypointRadius);
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
        // We can't go to ourselves
        if (currentPos == targetPos)
        {
            return null;
        }

        // Calculate and return the next point to go to
        Stack<Waypoint> path = Pathfinding.Dijkstra(waypointList, currentPos, targetPos);

        // A path was found...
        if (path != null && path.Count > 0)
        {
            return path.Peek();
        }

        // There is no more places to go
        return null;
    }
}
