using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class FleeingState : FSMState
    {

        protected override void exit()
        {

        }

        protected override void init()
        {
            parent.Speed = 100.0f;
        }

        protected override void update()
        {
            
        }
    }
}
