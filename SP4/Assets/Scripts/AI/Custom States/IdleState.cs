using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class IdleState : FSMState
    {

        protected override void exit()
        {

        }

        protected override void init()
        {

        }

        protected override void update()
        {
            //Heal Enemy While Idle
            //When Enemy is in IdleState for more than 2 seconds
            //Heal for 2hp per second.
            parent.InvokeRepeating("healenemy", 2, 1);
        }

        protected void healenemy()
        {
            parent.Heal(2);
        }
    }
}
