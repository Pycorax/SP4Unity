using UnityEngine;

namespace Enemy
{
    public class ChaseState : FSMState
    {
        protected override void exit()
        {
            
        }

        protected override void init()
        {
            
        }

        protected override void update()
        {
            // Determine nearest player to chase
            float shortestDist = -1.0f;
            GameObject playerToChase = null;
            foreach (var player in parent.PlayerList)
            {
                float distToPlayer = (parent.transform.position - player.transform.position).sqrMagnitude;

                if (shortestDist < 0.0f || shortestDist > distToPlayer)
                {
                    shortestDist = distToPlayer;
                    playerToChase = player;
                }
            }

            // Have we found one nearby?
            if (playerToChase != null)
            {
                // Determine which waypoint the player is at
                Waypoint playerWaypoint = parent.WaypointMap.FindNearestWaypoint(playerToChase.transform.position);
                // If we are near the same waypoint
                if (playerWaypoint == parent.CurrentWaypoint)
                {
                    // Just go after him
                    parent.moveTo(playerToChase.transform.position);
                } 
                else
                {
                    // Go towards the player
                    parent.FinalTargetWaypoint = parent.WaypointMap.GetNearestWaypointToGoTo(parent.CurrentWaypoint, playerWaypoint);
                }
            }
        }
    }
}
