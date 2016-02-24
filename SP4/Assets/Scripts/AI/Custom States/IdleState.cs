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
            timer += TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
            if(timer >= 5)
            {
                healenemy();
                timer = 0;
            }
        }

        private void healenemy()
        {
            parent.Heal(2);
            Debug.Log(parent.Health);
        }
    }
}
