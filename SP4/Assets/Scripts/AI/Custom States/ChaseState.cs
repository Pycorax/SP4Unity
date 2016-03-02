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
            //Debug.Log("ChaseState()");

            // Determine nearest player to chase
            var playerToChase = parent.getNearestPlayer();

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
                    if (parent.Health < 50)
                    {
                        parent.changeCurrentState(new FleeingState());
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
}
