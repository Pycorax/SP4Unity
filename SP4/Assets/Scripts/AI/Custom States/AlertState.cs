namespace Enemy
{
    public class AlertState : FSMState
    {
        private Waypoint previousWaypoint;
        private double changestatetimer;
        private const float speedy = 150.0f;

        protected override void exit()
        {

        }

        protected override void init()
        {
            // Alert speed is faster than normal speed
            parent.Speed = speedy;
            decideNextPatrolPoint();
        }

        protected override void update()
        {
            changestatetimer += TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

            //After 10 seconds in alert state, enemy will go back to patrol state
            if (changestatetimer >= 10)
            {
                parent.changeCurrentState(new PatrolState());
                return;
            }
            else
            {
                //Check if the nearest player is within distance to attack
                float distanceSqr = (parent.transform.position - parent.getNearestPlayer().transform.position).sqrMagnitude;
                if (distanceSqr <= 50000.0f)
                {
                    parent.changeCurrentState(new ChaseState());
                    return;
                }
            }


            //Make sure the parent is at the final way point
            if (parent.FinalTargetWaypoint == null)
            {
                decideNextPatrolPoint();
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
