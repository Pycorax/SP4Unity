﻿using System.Collections.Generic;
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
        [Tooltip("Debug. Enable this to use FinalTargetWaypointDebug to override FinalTargetWaypoint at runtime.")]
        public bool InspectorDebugging = false;

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

        internal RPGPlayer getNearestPlayer()
        {
            float dist;
            return getNearestPlayer(out dist);
        }

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

            Debug.Log(currentState.GetType().ToString());
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
            return distToTargetSquared < DISTANCE_CHECK_ACCURARCY * DISTANCE_CHECK_ACCURARCY;
        }
        #endregion

        #region Collision

        private void OnTriggerEnter2D(Collider2D other)
        {
            RPGPlayer player = other.gameObject.GetComponent<RPGPlayer>();
            Projectile proj = other.gameObject.GetComponent<Projectile>();
            Sword sword = other.gameObject.GetComponent<Sword>();

            if (player != null)
            {
                player.Injure(5);
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

