using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    /// <summary>
    /// Function to obtain a path in a list of waypoints using the Dijkstra Algorithm
    /// </summary>
    /// <param name="waypointList">The list of all the waypoints in the scene.</param>
    /// <param name="current">The current waypoint that the path should start from.</param>
    /// <param name="target">The resulting waypoint to reach in the path.</param>
    /// <returns>A stack that contains the path. If no path is possible, null is returned instead.</returns>
    public static Stack<Waypoint> Dijkstra(List<Waypoint> waypointList, Waypoint current, Waypoint target)
    {
        // Don't need to calculate, you're already here
        if (current == target)
        {
            return null;
        }

        // Dictionary to store the distance to each Waypoint associated to the Waypoint
        Dictionary<Waypoint, float> distToWaypoint = new Dictionary<Waypoint, float>();
        // Dictionary to store the previous Waypoint accessed to access the key Waypoint
        Dictionary<Waypoint, Waypoint> lastWaypointBefore = new Dictionary<Waypoint, Waypoint>();
        // The list of unanalyzed waypoints we are operating on
        List<Waypoint> unanalyzedWaypoints = new List<Waypoint>();

        // Initialize the dictionary with default values
        foreach (Waypoint w in waypointList)
        {
            // We don't know where this is yet so it's safe to say it's really far away
            distToWaypoint[w] = Mathf.Infinity;
            // We don't know how to get there
            lastWaypointBefore[w] = null;
            // Add into the dictionary
            unanalyzedWaypoints.Add(w);
        }

        // Set the current position to have a distance-to of 0. We are on it after all
        distToWaypoint[current] = 0;

        // Look through all the waypoints we have yet to analyze
        while (unanalyzedWaypoints.Count > 0)
        {
            // Find the Waypoint with the smallest distance, it will be the one we analyze (on first run, this will be our current)
            float shortestDist = Mathf.Infinity;
            Waypoint waypointToAnalyze = null;
            foreach (Waypoint w in unanalyzedWaypoints)
            {

                if (distToWaypoint[w] <= shortestDist)
                {
                    shortestDist = distToWaypoint[w];
                    waypointToAnalyze = w;
                }
            }

            // We are analyzing it, so remove from the list we are analyzing
            unanalyzedWaypoints.Remove(waypointToAnalyze);

            // If we reached the target, we are done
            if (waypointToAnalyze == target)
            {
                // Create a stack to store the path
                Stack<Waypoint> path = new Stack<Waypoint>();

                // Retrace the path to get the path until there is no nodes before it
                while (lastWaypointBefore[waypointToAnalyze] != null)
                {
                    path.Push(waypointToAnalyze);
                    waypointToAnalyze = lastWaypointBefore[waypointToAnalyze];
                }

                return path;
            }

            // If it is still Mathf.Infinity, means we somehow analyzed all the near ones and their neighbours. So if this one was not a neighbour of any
            // of the near ones, it means that it is probably inaccessible.
            if (distToWaypoint[waypointToAnalyze] == Mathf.Infinity)
            {
                // If Infinity is already the smallest distance... The rest will all be infinity, it is pointless to continue
                break;
            }

            // Loop through all of this Waypoint's neighbours
            foreach (Waypoint neighbour in waypointToAnalyze.Neighbours)
            {
                // To calculate the distance to each neighbour through this Waypoint
                float distFromHere = distToWaypoint[waypointToAnalyze] + Vector2.Distance(waypointToAnalyze.transform.position, neighbour.transform.position);

                // If this is a shorter route, we save the details...
                if (distFromHere < distToWaypoint[neighbour])
                {
                    // ... of the distance to get here
                    distToWaypoint[neighbour] = distFromHere;
                    // ... of how we got here
                    lastWaypointBefore[neighbour] = waypointToAnalyze;
                }
            }
        }

        // We went through the entire list and failed to find a solution
        return null;
    }
}
