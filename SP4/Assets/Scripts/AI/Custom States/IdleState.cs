using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class IdleState : FSMState
    {
        private int Decide;
        private double changestatetimer;

        protected override void exit()
        {

        }

        protected override void init()
        {

        }

        protected override void update()
        {
            //Check if the nearest player is within distance to attack
            float distanceSqr = (parent.transform.position - parent.getNearestPlayer().transform.position).sqrMagnitude;
            if (distanceSqr <= 50000.0f * 50000.0f)
            {
                parent.changeCurrentState(new ChaseState());
                return;
            }


            changestatetimer += TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
            //Enemy will rotate in idle state
            parent.gameObject.transform.Rotate(0, 0, 0.4f);

            //Every 5 seconds, will check if the enemy want to change states
            if (changestatetimer >= 5)
            {
                Changeofstates();
                changestatetimer = 0;
            }
        }

        private void Changeofstates()
        {
            Decide = Random.Range(0, 3);
            switch (Decide)
            {
                case 0:
                    break;
                case 1:
                    parent.changeCurrentState(new PatrolState());
                    break;
                case 2:
                    parent.changeCurrentState(new AlertState());
                    break;
            }
        }
    }
}
