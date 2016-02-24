using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class PatrolState : FSMState
    {
        private int Neighbourcount;
        private int randomnum;
        private Waypoint previousWaypoint;

        protected override void exit()
        {

        }

        protected override void init()
        {
            Neighbourcount = parent.CurrentWaypoint.Neighbours.Count;
            randomnum = Random.Range(0, Neighbourcount);
        }

        protected override void update()
        {
            previousWaypoint = parent.CurrentWaypoint;
            parent.FinalTargetWaypoint = parent.CurrentWaypoint.Neighbours[randomnum];
            if (parent.CurrentWaypoint == parent.FinalTargetWaypoint && parent.FinalTargetWaypoint != previousWaypoint)
            {
                //Generate another random number to go to
                Neighbourcount = parent.CurrentWaypoint.Neighbours.Count;
                randomnum = Random.Range(0, Neighbourcount);

                previousWaypoint = parent.CurrentWaypoint;
                parent.FinalTargetWaypoint = parent.CurrentWaypoint.Neighbours[randomnum];
            }
        }
    }
}
