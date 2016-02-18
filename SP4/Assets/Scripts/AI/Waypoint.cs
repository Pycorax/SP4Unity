using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    // Storage of Connected Way Points
    private List<Waypoint> neighbours;

    // Neighbour Selection
    private const float NEIGHBOURING_DIST = 1000.0f;
    private const float NEIGHBOURING_DIST_SQUARED = NEIGHBOURING_DIST * NEIGHBOURING_DIST;

    // Component
    private new Collider2D collider;
    private SpriteRenderer spriteRenderer;

    // Getters
    public List<Waypoint> Neighbours { get { return neighbours; } }

#if RAYCAST_DEBUG
    // Debugging
    public Waypoint waypoint;
#endif

    // Use this for initialization
    void Start ()
    {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Hide the sprite rendering if we are going to play
#if  !WAYPOINT_DEBUG
        spriteRenderer.enabled = false;
#endif
    }
	
	// Update is called once per frame
	void Update ()
    {
#if RAYCAST_DEBUG
        if (waypoint)
        {
            if(HaveLineOfSight(this, waypoint, transform.localScale.x))
            {
                GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
            }
        }
#endif
    }

    /// <summary>
    /// Use this function to set up the connections between this waypoints and the rest.
    /// </summary>
    /// <param name="closeByWaypoints">
    /// A list of waypoints that are near enough to be neighbours. Use GetDistanceNeighbours() with a list of all waypoints to obtain this list.
    /// </param>
    /// <param name="aiRadius">
    /// Size of the enemy that will be using these waypoints
    /// </param>
    public void SetUpConnections(List<Waypoint> closeByWaypoints, float aiRadius)
    {
        // Look for neighbours
        var list = from waypoint
                   in closeByWaypoints
                   where HaveLineOfSight(this, waypoint, aiRadius)
                   select waypoint;

        // Store this list of neighbours
        neighbours = list.ToList();
    }

    /// <summary>
    /// Get the neighbours within the range of NEIGHBOURING_DIST in a list of allWaypoints
    /// </summary>
    /// <param name="allWaypoints">List of all the waypoints in the scene.</param>
    /// <returns>List of all waypoints withint he range of NEIGHBOURING_DIST</returns>
    public List<Waypoint> GetDistanceNeighbours(List<Waypoint> allWaypoints)
    {
        // Look for distance-neighbours
        var list = from waypoint
                   in allWaypoints
                   where GetSqrDistanceTo(this, waypoint) <= NEIGHBOURING_DIST_SQUARED
                   select waypoint;

        // Return this list of neighbours
        if (list.Any())
        {
            return list.ToList();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Function to go through all neighbour waypoints and return a reference to the nearest waypoint
    /// </summary>
    /// <param name="exceptions">The list of Waypoints you wish to exclude from checking.</param>
    /// <returns>The waypoint that is the nearest to this waypoint.</returns>
    public Waypoint GetNearestNeighbour(List<Waypoint> exceptions = null)
    {
        Waypoint nearestWaypoint = null;
        float nearestDist = float.MaxValue;

        foreach (Waypoint w in neighbours)
        {
            // Check for exceptions if exceptions are provided
            if (exceptions != null)
            {
                // Check if this waypoint is supposed to be exempted
                foreach (Waypoint exception in exceptions)
                {
                    // If this is an exception, skip to the next one
                    if (w == exception)
                    {
                        continue;
                    }
                }
            }

            // Calculate the distance to the waypoint
            float dist = (w.transform.position - transform.position).sqrMagnitude;

            // Check if this is lower than the previous nearest
            if (nearestWaypoint == null || dist < nearestDist)
            {
                nearestWaypoint = w;
                nearestDist = (w.transform.position - transform.position).sqrMagnitude;
            }
        }

        return nearestWaypoint;
    }

    /// <summary>
    /// Function to calculate the square distance between W1 and W2
    /// </summary>
    public static float GetSqrDistanceTo(Waypoint w1, Waypoint w2)
    {
        // Do not compare with the same object
        if (w1 == w2)
        {
            return 0.0f;
        }
        
        // Calculate the distance
        return (w2.transform.position - w1.transform.position).sqrMagnitude;
    }

    /// <summary>
    /// Function to calculate the direction from W1 to W2
    /// </summary>
    public static Vector2 GetDirection(Waypoint w1, Waypoint w2)
    {
        // Do not compare with the same object
        if (w1 == w2)
        {
            return Vector2.zero;
        }

        // Calculate the direction
        Vector2 dir = w2.transform.position - w1.transform.position;
        if (dir != Vector2.zero)
        {
            dir.Normalize();
        }

        return dir;
    }

    /// <summary>
    /// Function to calculate if 2 waypoints have a line of sight to each other
    /// </summary>
    public static bool HaveLineOfSight(Waypoint w1, Waypoint w2, float lineOfSightWidth)
    {
        // Store the enabled status of the collider
        bool colliderEnabled = false;

        // Disable the origin object's collider so that the raycast dosesn't hit the origin object
        if (w1.collider)
        {
            // Save the collider state for restoration later
            colliderEnabled = w1.collider.enabled;
            // Disable the collider for ray casting
            w1.collider.enabled = false;
        }

        // Define the LayerMask for Circle Cast checking to only check Waypoint and Environment masks
        int layerMask = (1 << LayerMask.NameToLayer("Waypoint")) | (1 << LayerMask.NameToLayer("Environment"));

        // Circle Cast check for Line of Sight
        RaycastHit2D castInfo = Physics2D.CircleCast(w1.transform.position, lineOfSightWidth, GetDirection(w1, w2), NEIGHBOURING_DIST, layerMask);

        // Reset the origin object's collider back to original
        if (w1.collider)
        {
            w1.collider.enabled = colliderEnabled;
        }

#if RAYCAST_DEBUG
        Debug.DrawLine(w1.transform.position, castInfo.collider.transform.position, Color.yellow, 5.0f, false);
#endif

        // If we are able to collide with w2, means we have a straight line towards it
        return castInfo.collider == w2.GetComponent<Collider2D>();
    }

}
