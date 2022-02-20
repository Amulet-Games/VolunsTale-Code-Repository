using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ReturnalStandardEnemyDamageCollider : StandardEnemyDamageCollider
    {
        private void OnTriggerEnter(Collider other)
        {
            if (_ai.GetHasThrownProjectileStatus())
            {
                if (_layerManager.IsInLayer(other.gameObject.layer, _layerManager._returnalProjectileObstaclesMask))
                {
                    _ai.ReturnProjectileWhenHitObstacles();
                }
            }

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