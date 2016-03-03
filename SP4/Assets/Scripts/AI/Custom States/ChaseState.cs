using UnityEngine;

namespace Enemy
{
    public class ChaseState : FSMState
    {
        private const float speedy = 200.0f;

        protected override void exit()
        {

        }

        protected override void init()
        {
            parent.Speed = speedy;
        }

        protected override void update()
        {
            // Determine nearest player to chase
            var playerToChase = parent.getNearestPlayer();

            //Check if the nearest player is within distance to attack
            float distance = Vector3.Distance(parent.transform.position, parent.getNearestPlayer().transform.position);
            if (distance  >= 2000.0f)
            {
                parent.changeCurrentState(new PatrolState());
                return;
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

            if (parent.Health < 50)
            {
                parent.changeCurrentState(new FleeingState());
                return;
            }
        }
    }
}
