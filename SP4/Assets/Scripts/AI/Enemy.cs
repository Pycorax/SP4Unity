using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Enemy : Character
    {
        [Tooltip("Reference to the enemy's manager to do statistic updates.")]
        public EnemyManager Manager;
        [Tooltip("Parent GameObject that holds children GameObjects that act as waypoints on the level.")]
        public WaypointManager WaypointMap;
        [Tooltip("A list of references to the players in the game.")]
        public List<GameObject> PlayerList;
        [Tooltip("The time delay between waypoint updates.")]
        public float WaypointUpdateDelay = 2.0f;
        [Tooltip("Debug. To test Waypoint system. Allows setting of FinalTargetWaypoint at runtime.")]
        public Waypoint FinalTargetWaypointDebug;
        [Tooltip("The rotation offset from the original sprite direction to get the sprite to face right. This is used for calculating the correct direction of the player sprite.")]
        public float RotationSpriteOffset = -90.0f;
        [Tooltip("Debug. Enable this to use FinalTargetWaypointDebug to override FinalTargetWaypoint at runtime.")]
        public bool InspectorDebugging = false;
        [Tooltip("Enemy Damage to the player.")]
        public int EnemyDamage = 1;

        /// <summary>
        /// 
        /// </summary>
        public Waypoint FinalTargetWaypoint
        {
            get { return finalTargetWaypoint; }
            set
            {
                // Null Checking
                if (value == null)
                {
                    finalTargetWaypoint = null;
                    return;
                }

                // Sets the FinalTargetWaypoint
                finalTargetWaypoint = value;

                // Calculate the Waypoint to go to
                currentTargetWaypoint = WaypointMap.GetNearestWaypointToGoTo(currentWaypoint, FinalTargetWaypoint);

                // Reset Timer
                waypointUpdateTimer = 0.0f;
            }
        }

        // Movement
        public float Speed = 100.0f;

        // AI
        private Waypoint finalTargetWaypoint = null;
        private FSMState currentState;                                      // Stores the current game state
        private Waypoint currentWaypoint;
        private Waypoint currentTargetWaypoint;                             // Stores a reference to the current target waypoint
        private float waypointUpdateTimer = 0.0f;                           // Use to time the delays between Waypoint updates
        private const float DISTANCE_CHECK_ACCURARCY = 10.0f;               // Used to do "reached position" checking

        // Animation
        internal int animAttack = Animator.StringToHash("Attack");
        internal int animMoving = Animator.StringToHash("Moving");
        internal int animAlive = Animator.StringToHash("Alive");
        private Vector2 prevPosition;

        // Debug
        private Vector3 lastTargetPosition;

        // Getters
        internal Waypoint CurrentWaypoint { get { return currentWaypoint; } }

        //Components
        internal Animator animator;

        // Use this for initialization
        protected override void Start()
        {
            // Initialize Components
            animator = GetComponent<Animator>();

            // Set default values
            Reset();
        }

        protected void OnDrawGizmos()
        {
            if (currentTargetWaypoint != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, currentTargetWaypoint.transform.position);
                Gizmos.DrawSphere(currentTargetWaypoint.transform.position, 1.0f);
            }
            if (finalTargetWaypoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, finalTargetWaypoint.transform.position);
                Gizmos.DrawSphere(finalTargetWaypoint.transform.position, 1.0f);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, lastTargetPosition);
            Gizmos.DrawSphere(lastTargetPosition, 1.0f);
        }

        // Update is called once per frame
        protected override void Update()
        {
            // Base Update
            base.Update();

            // Check for Death
            if (IsAlive)
            {
                // Update the FSM
                if (currentState != null)
                {
                    currentState.Update();
                }

                // Update Waypoint movement if a target is specified
                waypointUpdate();

                // Update Movement Animation
                if (prevPosition != (Vector2)transform.position)
                {
                    // Update the previous position
                    prevPosition = transform.position;
                    
                    // Update the animator
                    animator.SetBool(animMoving, true);
                }
                else
                {
                    // Update the animator
                    animator.SetBool(animMoving, false);
                }
            }
            // Don't repeatedly go dead
            else if (!(currentState is DeadState))
            {
                changeCurrentState(new DeadState());
            }
        }

        /// <summary>
        /// Function to initialize the Enemy from code.
        /// </summary>
        /// <param name="position">The position on the map to spawn the enemy.</param>
        /// <param name="waypointMap">The object that manages the list of Waypoints and updates them.</param>
        /// <param name="listOfPlayers">A list of all Players in the scene.</param>
        /// <param name="enemyManager">The parent that manages this Enemy.</param>
        public void Init(Vector3 position, WaypointManager waypointMap, List<GameObject> listOfPlayers, EnemyManager enemyManager)
        {
            // Set the WaypointMap
            WaypointMap = waypointMap;

            // Set the list of players
            PlayerList = listOfPlayers;

            // Set the Enemy Manager (parent manager)
            Manager = enemyManager;

            // Reset Values
            Reset(position);
        }

        /// <summary>
        /// Use this function to reset this enemy to it's default values except it's position.
        /// </summary>
        public void Reset()
        {
            // Base Start (Health)
            base.Start();

            // Set the Waypoint that we are nearest to right now
            if (WaypointMap != null)
            {
                currentWaypoint = WaypointMap.FindNearestWaypoint(transform.position);
            }

            // Set the default state
            changeCurrentState(new IdleState());
        }

        /// <summary>
        /// Use this function to reset this enemy to it's default values at a specified position.
        /// </summary>
        /// <param name="position">The position you wish to spawn this enemy at.</param>
        public void Reset(Vector2 position)
        {
            // Set the enemy position
            transform.position = position;

            // Reset enemy stats
            Reset();
        }

        /// <summary>
        /// Use this function in the animator to deactive the enemy.
        /// </summary>
        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Use this function to get the nearest player.
        /// </summary>
        /// <returns>A reference to the player nearest to this enemy.</returns>
        internal RPGPlayer getNearestPlayer()
        {
            float dist;
            return getNearestPlayer(out dist);
        }

        /// <summary>
        /// Use this function to get the nearest player.
        /// </summary>
        /// <param name="distSqrToPlayer">An out parameter that allows you to obtain the square distance to the player.</param>
        /// <returns>A reference to the player nearest to this enemy.</returns>
        internal RPGPlayer getNearestPlayer(out float distSqrToPlayer)
        {
            // Determine nearest player to chase
            float shortestDist = -1.0f;
            GameObject nearestPlayer = null;
            foreach (var player in PlayerList)
            {
                float distToPlayer = (transform.position - player.transform.position).sqrMagnitude;

                if (shortestDist < 0.0f || shortestDist > distToPlayer)
                {
                    shortestDist = distToPlayer;
                    nearestPlayer = player;
                }
            }

            // Return the distance squared to the player via out
            distSqrToPlayer = shortestDist;

            // Return the reference to the nearest player
            return nearestPlayer.GetComponent<RPGPlayer>();
        }

        /// <summary>
        /// This function will set the enemy's rotation based on the direction passed in.
        /// </summary>
        /// <param name="dir">The direction you want the enemy to rotate to look at.</param>
        internal void updateRotation(Vector2 dir)
        {
            // Ensure an actual direction is provided
            if (dir != Vector2.zero)
            {
                // Calculate the angle using Atan2 and add RotationSpriteOffset due to realign with original sprite direction
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + RotationSpriteOffset;

                // Set the rotation according to a calculation based on the angle
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        #region Waypoint

        /// <summary>
        /// Function to update the waypoint movement based on Dijkstra's Algorithm
        /// </summary>
        internal void waypointUpdate()
        {
            // Set to Debug values for testing
            if (InspectorDebugging && FinalTargetWaypointDebug != null)
            {
                FinalTargetWaypoint = FinalTargetWaypointDebug;
            }

            // If we still have some place to go...
            if (FinalTargetWaypoint != null)
            {
                // Update the next waypoint to go to
                // -- Update Timer
                waypointUpdateTimer += (float) TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
                // -- Timer Check
                if (waypointUpdateTimer > WaypointUpdateDelay)
                {
                    currentTargetWaypoint = WaypointMap.GetNearestWaypointToGoTo(currentWaypoint, FinalTargetWaypoint);
                    // -- Reset Timer
                    waypointUpdateTimer = 0.0f;
                }

                // Check if there is a path/Waypoint to go on
                if (currentTargetWaypoint != null)
                {
                    // Check if we have reached the Waypoint
                    if (reachedWaypoint(currentTargetWaypoint))
                    {
                        // Update the current Waypoint
                        currentWaypoint = WaypointMap.FindNearestWaypoint(transform.position);
                    }
                    else
                    {
                        // Get direction to the target
                        Vector2 dir = currentTargetWaypoint.transform.position - transform.position;
                        dir.Normalize();

                        // Head to the target
                        transform.position += (Vector3)dir * Speed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

                        // Update Rotation
                        updateRotation(dir);
                    }
                }
                else
                {
                    // If there is no more path, we stop deciding to go there
                    FinalTargetWaypoint = null;
                }
            }

            // Update the Debug Values
            if (InspectorDebugging)
            {
                FinalTargetWaypointDebug = FinalTargetWaypoint;
            }
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

            Debug.Log("Changed to " + currentState.GetType().ToString());
        }


        /// <summary>
        /// Function to move to a location. Takes delta time and speed into consideration. 
        /// </summary>
        /// <param name="pos">The location to move to.</param>
        internal void moveTo(Vector2 pos)
        {
            // Calculate the direction to this position
            Vector2 dir = (pos - (Vector2)transform.position);

            // Only attempt to move if we are not near the same position already
            if (dir.sqrMagnitude > transform.lossyScale.x * transform.lossyScale.x)
            {
                // Get the unit directional vector
                dir.Normalize();

                // Calculate the delta movement
                Vector2 movement = dir * Speed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

                // Move there
                transform.position += new Vector3(movement.x, movement.y, 0.0f);

                // Update rotation
                updateRotation(dir);
            }

            // Update the last targeted position to move to
            lastTargetPosition = pos;
        }

        /// <summary>
        /// Function to check if the Enemy has reached a particular Waypoint
        /// </summary>
        /// <param name="w">The Waypoint to check with.</param>
        /// <returns>If Enemy has reached Waypoint w</returns>
        internal bool reachedWaypoint(Waypoint w)
        {
            // Check if we have reached the Waypoint
            return reachedPoint(w.transform.position);
        }

        /// <summary>
        /// Function to check if the Enemy has reached a particular point in the world
        /// </summary>
        /// <param name="point">The point to check with.</param>
        /// <returns>If Enemy has reached the point</returns>
        internal bool reachedPoint(Vector3 point)
        {
            // Determine the distance
            float distToTargetSquared = (transform.position - point).sqrMagnitude;

            // Check if we have reached the Waypoint
            return distToTargetSquared < DISTANCE_CHECK_ACCURARCY * DISTANCE_CHECK_ACCURARCY;
        }
        #endregion

        #region Collision

        private void OnCollisionEnter2D(Collision2D other)
        {
            RPGPlayer player = other.gameObject.GetComponent<RPGPlayer>();
            Projectile proj = other.gameObject.GetComponent<Projectile>();
            Sword sword = other.gameObject.GetComponent<Sword>();

            if (player != null)
            {
                player.Injure(EnemyDamage);
            }
            else if (proj != null)
            {
                // Injury from Projectile
                Injure(proj.Damage);

                // Remove the Projectile
                proj.Disable();
            }
            //Collided with Sword
            else if(sword != null)
            {
                Injure(sword.Damage);
            }
        }

        #endregion

    }
}

