namespace Enemy
{
    public abstract class FSMState
    {
        // Reference to the enemy that this state controls
        protected Enemy parent;

        public void Init(Enemy _parent)
        {
            parent = _parent;
            init();
        }

        public void Update()
        {
            update();
        }

        public void Exit()
        {
            exit();
        }

        /*
         * Functions to implement to create FSM State behaviour
         */
        protected abstract void init();
        protected abstract void update();
        protected abstract void exit();
    }
}
