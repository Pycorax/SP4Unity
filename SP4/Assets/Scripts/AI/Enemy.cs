using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Enemy : Character
    {
        [Tooltip("Parent GameObject that holds children GameObjects that act as waypoints on the level.")]
        public WaypointManager WaypointMap;
        [Tooltip("A list of references to the players in the game.")]
        public List<GameObject> PlayerList;
        [Tooltip("The target waypoint that the enemy will go towards.")]
        public Waypoint FinalTargetWaypoint;

        // Movement
        public float Speed = 200.0f;

        // AI
        private FSMState currentState;                                      // Stores the current game state
        private Waypoint currentWaypoint;
        private Waypoint currentTargetWaypoint;                             // Stores a reference to the current target waypoint
        private const float DISTANCE_CHECK_ACCURARCY = 10.0f;               // Used to do "reached position" checking

        // Getters
        internal Waypoint CurrentWaypoint { get { return currentWaypoint; } }

        // Use this for initialization
        protected override void Start()
        {
            // Base Start
            base.Start();

            // Set the Waypoint that we are nearest to right now
            currentWaypoint = WaypointMap.FindNearestWaypoint(transform.position);

            // Set the default state
            changeCurrentState(new ChaseState());
        }

        // Update is called once per frame
        protected override void Update()
        {
            // Base Update
            base.Update();

            // Update the FSM
            if (currentState != null)
            {
                currentState.Update();
            }
            
            // Update Waypoint movement if a target is specified
            waypointUpdate();
        }

        #region Waypoint
        /// <summary>
        /// Function to update the waypoint movement based on Dijkstra's Algorithm
        /// </summary>
        internal void waypointUpdate()
        {
            // If we still have some place to go...
            if (FinalTargetWaypoint != null)
            {
                // Update the next waypoint to go to
                currentTargetWaypoint = WaypointMap.GetNearestWaypointToGoTo(currentWaypoint, FinalTargetWaypoint);

                // Check if there is a path/Waypoint to go on
                if (currentTargetWaypoint != null)
                {
                    // Check if we have reached the Waypoint
                    if (reachedWaypoint(currentTargetWaypoint))
                    {
                        // Get direction to the target
                        Vector2 dir = currentTargetWaypoint.transform.position - transform.position;
                        dir.Normalize();

                        // Head to the target
                        transform.position += (Vector3)dir * 500.0f * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
                    }
                    else
                    {
                        // Update the current Waypoint
                        currentWaypoint = WaypointMap.FindNearestWaypoint(transform.position);                        
                    }
                }
                else
                {
                    // If there is no more path, we stop deciding to go there
                    FinalTargetWaypoint = null;
                }
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

        /// <summary>
        /// Function to change the state of this Enemy
        /// </summary>
        /// <param name="state"></param>
        internal void changeCurrentState(FSMState state)
        {
            // If there was a previous state, clear it
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = state;
            currentState.Init(this);
        }


        /// <summary>
        /// Function to move to a location. Takes delta time and speed into consideration. 
        /// </summary>
        /// <param name="pos">The location to move to.</param>
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

        /// <summary>
        /// Function to check if the Enemy has reached a particular Waypoint
        /// </summary>
        /// <param name="w">The Waypoint to check with.</param>
        /// <returns>If Enemy has reached Waypoint w</returns>
        internal bool reachedWaypoint(Waypoint w)
        {
            // Determine the distance
            float distToTargetSquared = (transform.position - w.transform.position).sqrMagnitude;

            // Check if we have reached the Waypoint
            return distToTargetSquared > DISTANCE_CHECK_ACCURARCY * DISTANCE_CHECK_ACCURARCY;
        }
        #endregion
    }
}
