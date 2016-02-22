using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class PatrolState : FSMState
    {
        public Waypoint Initwaypoint;
        public Waypoint Finalwaypoint;

        protected override void exit()
        {

        }

        protected override void init()
        {

        }

        protected override void update()
        {
            if (parent.CurrentWaypoint == Initwaypoint)
            {
                parent.FinalTargetWaypoint = Finalwaypoint;
            }
            else if (parent.CurrentWaypoint == Finalwaypoint)
            {
                parent.FinalTargetWaypoint = Initwaypoint;
            }
        }
    }
}
