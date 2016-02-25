using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class PatrolState : FSMState
    {
        private Waypoint previousWaypoint;

        protected override void exit()
        {

        }

        protected override void init()
        {
            // Previous waypoit will be enemy current waypoint
            previousWaypoint = parent.CurrentWaypoint;
            //Enemy will now move to its next destination
            parent.FinalTargetWaypoint = parent.CurrentWaypoint.GetRandomNeighbour();
        }

        protected override void update()
        {
            //Make sure the parent is at the final way point
            if (parent.FinalTargetWaypoint == null)
            {
                //MOVE!
                parent.FinalTargetWaypoint = parent.CurrentWaypoint.GetRandomNeighbour(previousWaypoint);

                //set ur previous waypoint to current waypoint
                previousWaypoint = parent.CurrentWaypoint;
            }
        }
    }
}
