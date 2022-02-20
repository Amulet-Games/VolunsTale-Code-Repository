using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ExecutionStandardEnemyDamageCollider : StandardEnemyDamageCollider
    {
        private void OnTriggerEnter(Collider other)
        {
            if (_ai.GetIsExecutePresentAttack())
            {
                bool _hasHitObstacle = false;

                if (_layerManager.IsInLayer(other.gameObject.layer, _layerManager._executionObstaclesMask))
                {
                    _hasHitObstacle = true;
                }

                if (other.gameObject.layer == _layerManager.playerLayer)
                {
                    _ai.SetIsExecutePresentAttackToFalse();

                    if (playerStates.isInvincible)
                        return;

                    if (_hasHitObstacle)
                    {
                        playerStates._hitPoint = other.ClosestPoint(transform.position);
                        playerStates.OnDamageColliderHit(this);
                    }
                    else
                    {
                        _ai.OnSucessfulCaughtPlayer();
                    }
                }
            }
            else
            {
                if (other.gameObject.layer == _layerManager.playerLayer)
                {
                    if (playerStates.isInvincible)
                        return;

                    playerStates._hitPoint = other.ClosestPoint(transform.position);
                    playerStates.OnDamageColliderHit(this);
                }
            }
        }
    }
}