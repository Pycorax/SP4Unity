using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class IdleState : FSMState
    {
        private double timer;
        private double changestatetimer;
        private int Decide;

        protected override void exit()
        {

        }

        protected override void init()
        {

        }

        protected override void update()
        {

            // If timer is more than 3 seconds, enemy will heal itself
            timer += TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
            changestatetimer += TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

            if (timer >= 3)
            {
                healenemy();
                timer = 0;
            }

            //Enemy will rotate in idle state
            parent.gameObject.transform.Rotate(0, 0, 0.3f);

            //Every 5 seconds, will check if the enemy want to change states
            if(changestatetimer >= 5)
            {
                Changeofstates();
                changestatetimer = 0;
            }
        }

        private void healenemy()
        {
            // Will heal the enemy for a certain amount
            parent.Heal(2);
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
