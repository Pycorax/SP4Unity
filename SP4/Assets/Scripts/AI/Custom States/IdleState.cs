using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class IdleState : FSMState
    {
        private double timer;

        protected override void exit()
        {

        }

        protected override void init()
        {

        }

        protected override void update()
        {
            // If timer is more than 5 seconds, enemy will heal itself
            timer += TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
            if(timer >= 5)
            {
                healenemy();
                timer = 0;
            }

            //Enemy will rotate in idle state
            parent.gameObject.transform.Rotate(0, 0, 0.3f);

            Changeofstates();
        }

        private void healenemy()
        {
            // Will heal the enemy for a certain amount
            parent.Heal(2);
            Debug.Log(parent.Health);
        }

        private void Changeofstates()
        {

        }
    }
}
