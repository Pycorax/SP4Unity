using UnityEngine;

namespace Enemy
{
    public abstract class FSMState : MonoBehaviour
    {
        // Reference to the enemy that this state controls
        protected Enemy parent;
        
        // Use this for initialization
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {
        }

        public void AIInit(Enemy _parent)
        {
            parent = _parent;
            init();
        }

        public void AIUpdate()
        {
            update();
        }

        public void AIExit()
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
