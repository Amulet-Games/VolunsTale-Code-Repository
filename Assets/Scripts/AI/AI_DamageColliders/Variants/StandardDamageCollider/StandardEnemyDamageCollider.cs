using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class StandardEnemyDamageCollider : BaseEnemyDamageCollider
    {
        public override void Setup(AIManager aiManager)
        {
            _collider.enabled = false;
            _collider.isTrigger = true;

            _ai = aiManager;
            playerStates = aiManager.playerStates;

            _layerManager = LayerManager.singleton;
            gameObject.layer = _layerManager.enemyDamageColliderLayer;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == _layerManager.playerLayer)
            {
                if (playerStates.isInvincible)
                    return;

                playerStates._hitPoint = other.ClosestPoint(transform.position);
                playerStates.OnDamageColliderHit(this);
            }
        }

        public void KMA_OnDamageColliderHit()
        {
            playerStates.OnDamageColliderHit(this);
        }
    }
}