﻿using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class AlertState : FSMState
    {
        private Waypoint previousWaypoint;
        private double changestatetimer;
        private int Deciding;

        protected override void exit()
        {

        }

        protected override void init()
        {
            // Alert speed is faster than normal speed
            parent.Speed = 1000.0f;
            // Previous waypoit will be enemy current waypoint
            previousWaypoint = parent.CurrentWaypoint;
            //Enemy will now move to its next destination
            parent.FinalTargetWaypoint = parent.CurrentWaypoint.GetRandomNeighbour();
        }

        protected override void update()
        {
            changestatetimer += TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
            if (changestatetimer >= 10)
            {
                parent.changeCurrentState(new PatrolState());
                return;
            }

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
