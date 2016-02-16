using System;
using UnityEngine;

namespace Enemy
{
    public class ChaseState : FSMState
    {
        protected override void exit()
        {
            throw new NotImplementedException();
        }

        protected override void init()
        {
            throw new NotImplementedException();
        }

        protected override void update()
        {
            // Determine nearest player to chase
            float shortestDist = -1.0f;
            GameObject playerToChase = null;
            foreach (var player in parent.PlayerList)
            {
                float distToPlayer = (parent.transform.position - player.transform.position).sqrMagnitude;

                if (shortestDist < 0.0f || shortestDist > distToPlayer)
                {
                    shortestDist = distToPlayer;
                    playerToChase = player;
                }
            }

            // Have we found one nearby?
            if (playerToChase != null)
            {
                parent.moveTo(playerToChase.transform.position);
            }
        }
    }
}
