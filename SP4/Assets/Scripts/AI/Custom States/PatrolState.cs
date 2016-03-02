using UnityEngine;

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
            decideNextPatrolPoint();
        }

        protected override void update()
        {
            //Check if the nearest player is within distance to attack
            float distanceSqr = (parent.transform.position - parent.getNearestPlayer().transform.position).sqrMagnitude;
            if (distanceSqr <= 50000.0f)
            {
                parent.changeCurrentState(new ChaseState());
                return;
            }
            

            changestatetimer += TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
            // If there is no target, we have reached that target
            if (parent.FinalTargetWaypoint == null)
            {
                // Every 5 seconds, will check if the enemy want to change states
                if (changestatetimer >= 5)
                { 
                changeOfStates();
                changestatetimer = 0;
                }
            }        
        }

        private void changeOfStates()
        {
            Deciding = Random.Range(0, 2);
            switch (Deciding)
            {
                case 0:
                    decideNextPatrolPoint();
                    break;
                case 1:
                    parent.changeCurrentState(new IdleState());
                    break;
            }
        }

        private void decideNextPatrolPoint()
        {
            // Keep track of the previous patrol point to prevent backtracking
            previousWaypoint = parent.CurrentWaypoint;

            // Set our target to go to
            parent.FinalTargetWaypoint = parent.CurrentWaypoint.GetRandomNeighbour(previousWaypoint);
        }
    }
}
