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
            // Kill the enemy
            parent.gameObject.SetActive(false);

            // Notify the Manager of the kill
            parent.Manager.ConfirmKill();

            // Notify the Animator
            parent.animator.SetBool(parent.animAlive, false);
        }

        protected override void update()
        {

        }
    }
}
