using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        [Tooltip("Parent GameObject that holds children GameObjects that act as waypoints on the level.")]
        public GameObject WaypointContainer;
        [Tooltip("A list of references to the players in the game.")]
        public List<GameObject> PlayerList;

        // Health
        public int MaxHealth = 100;
        private int health;

        // Movement
        public float Speed = 200.0f;

        // AI
        FSMState currentState;                      // Stores the current game state

        // Getters
        public int Health { get { return health; } }

        // Use this for initialization
        void Start()
        {
            health = MaxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            if (currentState != null)
            {
                currentState.AIUpdate();
            }
        }

        // Function to change the state of this Enemy
        internal void changeCurrentState(FSMState state)
        {
            currentState.AIExit();
            currentState = state;
            currentState.AIInit(this);
        }

        // Function to move to a location. Takes delta time and speed into consideration.
        internal void moveTo(Vector2 pos)
        {
            // Calculate the direction to this position
            Vector2 dir = (pos - (Vector2)transform.position);

            // Only attempt to move if we are not on the same position already
            if (dir != Vector2.zero)
            {
                // Get the unit directional vector
                dir.Normalize();

                // Calculate the delta movement
                Vector2 movement = dir * Speed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

                // Move there
                transform.Translate(movement);
            }
        }
    }
}
