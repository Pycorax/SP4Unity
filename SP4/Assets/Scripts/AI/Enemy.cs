using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        [Tooltip("Parent GameObject that holds children GameObjects that act as waypoints on the level.")]
        public WaypointManager WaypointMap;
        [Tooltip("A list of references to the players in the game.")]
        public List<GameObject> PlayerList;
        
        public Waypoint FinalTargetWaypoint;

        // Health
        public int MaxHealth = 100;
        private int health;

        // Movement
        public float Speed = 200.0f;

        // AI
        FSMState currentState;                      // Stores the current game state
        Waypoint currentWaypoint;
        Waypoint currentTargetWaypoint;             // Stores a reference to the current target waypoint

        // Getters
        public int Health { get { return health; } }

        // Use this for initialization
        void Start()
        {
            health = MaxHealth;

            // Get the nearest waypoint and head to it
            currentWaypoint = WaypointMap.FindNearestWaypoint(transform.position);
            currentTargetWaypoint = WaypointMap.GetNearestWaypointToGoTo(currentWaypoint, FinalTargetWaypoint);

            Debug.Log(currentWaypoint);
            Debug.Log(currentTargetWaypoint);
        }

        // Update is called once per frame
        void Update()
        {
            //if (currentState != null)
            //{
            //    currentState.AIUpdate();
            //}

            if (Vector2.Distance(currentTargetWaypoint.transform.position, transform.position) > 10.0f)
            {
                // Get direction to the target
                Vector2 dir = currentTargetWaypoint.transform.position - transform.position;
                dir.Normalize();

                // Head to the target
                transform.position += (Vector3)dir * 500.0f * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
            }
            else
            {
                // If we reached, decide the next point
                currentTargetWaypoint = WaypointMap.GetNearestWaypointToGoTo(currentWaypoint, FinalTargetWaypoint);
            }
        }

        /// <summary>
        /// Function to initialize the player position.
        /// </summary>
        /// <param name="position">The position on the map to spawn the enemy.</param>
        public void Init(Vector3 position)
        {
            // Set the enemy position
            transform.position = position;

            // TODO: Reset the current state
            // changeCurrentState(new IdleState());
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
