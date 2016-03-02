using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class FleeingState : FSMState
    {
        private double timer;

        protected override void exit()
        {

        }

        protected override void init()
        {
            parent.Speed = 50.0f;
            parent.FinalTargetWaypoint = parent.CurrentWaypoint.GetFurthestNeighbour();
        }

        protected override void update()
        {
            if (parent.FinalTargetWaypoint = null)
            {
                // If timer is more than 3 seconds, enemy will heal itself
                timer += TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
                if (timer >= 3)
                {
                    healenemy();
                    timer = 0;
                }

                if(parent.health == 100)
                {
                    parent.changeCurrentState(new PatrolState());
                    return;
                }
            }
        }

        private void healenemy()
        {
            // Will heal the enemy for a certain amount
            parent.Heal(2);
        }
    }
}
