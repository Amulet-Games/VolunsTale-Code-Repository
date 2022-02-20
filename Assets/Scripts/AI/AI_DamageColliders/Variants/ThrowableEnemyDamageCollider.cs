using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ThrowableEnemyDamageCollider : BaseEnemyDamageCollider
    {
        [Header("Refs.")]
        [ReadOnlyInspector] public ThrowableEnemyRuntimeWeaponBase _referingRuntimeThrowWeapon;

        public override void Setup(AIManager aiManager)
        {
            _collider.enabled = false;
            _collider.isTrigger = true;

            _ai = aiManager;
            playerStates = _ai.playerStates;

            _layerManager = LayerManager.singleton;
            gameObject.layer = _layerManager.enemyDamageColliderLayer;

            _referingRuntimeThrowWeapon = _ai.currentThrowableWeapon;
        }

        private void OnTriggerEnter(Collider other)
        {
            _referingRuntimeThrowWeapon.OnTriggerCheckObstaclesHit();

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