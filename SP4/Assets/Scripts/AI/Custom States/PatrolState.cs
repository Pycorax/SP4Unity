using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class PatrolState : FSMState
    {
        private Waypoint previousWaypoint;
        private int Deciding;

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
            //parent.getNearestPlayer()
            //Make sure the parent is at the final way point
            if (parent.FinalTargetWaypoint == null)
            {
                //Enemy has now reached a waypoint, will randomize if he wants to change state
                Changeofstates();

                if (Deciding == 0)
                {
                    //MOVE!
                    parent.FinalTargetWaypoint = parent.CurrentWaypoint.GetRandomNeighbour(previousWaypoint);

                    //set ur previous waypoint to current waypoint
                    previousWaypoint = parent.CurrentWaypoint;
                }
            }
            float distance = Vector3.Distance(parent.transform.position, parent.getNearestPlayer().transform.position);
            if (distance <= 120.0f)
            {
                parent.changeCurrentState(new ChaseState());
            }
        }

        private void Changeofstates()
        {
            Deciding = Random.Range(0, 2);
            switch (Deciding)
            {
                case 0:
                    break;
                case 1:
                    parent.changeCurrentState(new IdleState());
                    break;
            }
        }
    }
}
