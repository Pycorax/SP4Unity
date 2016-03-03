namespace Enemy
{
    public class DeadState : FSMState
    {

        protected override void exit()
        {

        }

        protected override void init()
        {
            // Notify the Manager of the kill
            parent.Manager.ConfirmKill();

            // Notify the Animator
            parent.animator.SetBool(parent.animAlive, false);

            // The animator will handle the deactivation of the enemy
        }

        protected override void update()
        {

        }
    }
}
