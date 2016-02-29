using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class PatrolState : FSMState
    {
        private Waypoint previousWaypoint;
        private double changestatetimer;
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
            //Debug.Log("PatrolState()");

            //Check if the nearest player is within distance to attack
            //float distanceSqr = (parent.transform.position - parent.getNearestPlayer().transform.position).sqrMagnitude;
            //if (distanceSqr <= 50000.0f)
            //{
            //    parent.changeCurrentState(new ChaseState());
            //    return;
            //}

            changestatetimer += TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

            //Make sure the parent is at the final way point
            if (parent.FinalTargetWaypoint == null)
            {
                //Every 5 seconds, will check if the enemy want to change states
                if (changestatetimer >= 5)
                {
                    Changeofstates();
                    changestatetimer = 0;
                }

                if (Deciding == 0)
                {
                    //MOVE!
                    parent.FinalTargetWaypoint = parent.CurrentWaypoint.GetRandomNeighbour(previousWaypoint);
                    //set ur previous waypoint to current waypoint
                    previousWaypoint = parent.CurrentWaypoint;
                }
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
