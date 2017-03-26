﻿using UnityEngine;

namespace Enemy
{
    public class ChaseState : FSMState
    {
        private const float PATROL_THRESHOLD = 2000.0f;
        private float damageDelayDuration = 0.0f;
        private const float damageDelay = 1.0f;
        private const float CHASE_SPEED = 200.0f;
        private int lineOfSightLayerMask = 1;

        protected override void exit()
        {

        }

        protected override void init()
        {
            parent.Speed = CHASE_SPEED;
            lineOfSightLayerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Environment"));
        }

        protected override void update()
        {
            // Update damage delay
            damageDelayDuration += (float) TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

            // Determine nearest player to chase
            var playerToChase = parent.getNearestPlayer();

            //Check if the nearest player is within distance to attack
            float distance = Vector3.Distance(parent.transform.position, parent.getNearestPlayer().transform.position);
            if (distance  >= PATROL_THRESHOLD)
            {
                parent.changeCurrentState(new PatrolState());
                return;
            }

            // Have we found one nearby?
            if (playerToChase != null)
            {
                // Determine which waypoint the player is at
                Waypoint playerWaypoint = parent.WaypointMap.FindNearestWaypoint(playerToChase.transform.position);
                // If we are near the same waypoint
                if (playerWaypoint == parent.CurrentWaypoint)
                {
                    // We reached, attack
                    if (parent.reachedPoint(playerToChase.transform.position))
                    {
                        // Check if delay is long enough
                        if (damageDelayDuration > damageDelay)
                        {
                            playerToChase.Injure(parent.EnemyDamage);

                            // Reset timer
                            damageDelayDuration = 0.0f;
                        }
                    }
                    else // We still have to go to it
                    {
                        // If we cannot see the bugger, go back to patrolling
                        const float ViewRange = 10000.0f;
                        var chaseDir = (playerToChase.transform.position - parent.transform.position).normalized;
                        var hit = Physics2D.Raycast(parent.transform.position, chaseDir, ViewRange, lineOfSightLayerMask);
                        if (hit.rigidbody != null && hit.rigidbody.gameObject == playerToChase.gameObject)
                        {
                            // Go back to patrolling
                            //parent.changeCurrentState(new PatrolState());
                            parent.moveTo(playerToChase.transform.position - playerToChase.transform.lossyScale * 0.5f);
                        }

                        // Just go after it
                    }
                }
                else
                {
                    // Go towards the player
                    parent.FinalTargetWaypoint = parent.WaypointMap.GetNearestWaypointToGoTo(parent.CurrentWaypoint, playerWaypoint);
                }
            }

            if (parent.Health < parent.MaxHealth * 0.5f)
            {
                parent.changeCurrentState(new FleeingState());
                return;
            }
        }
    }
}
