using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class DeadState : FSMState
    {

        protected override void exit()
        {

        }

        protected override void init()
        {

            //Kill of enemy
            parent.gameObject.SetActive(false);
        }

        protected override void update()
        {

        }
    }
}
