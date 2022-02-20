using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class LayerManager : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField]
        public int playerLayer = 8;

        [Header("Enemy")]
        [SerializeField]
        public int enemyLayer = 9;
        [SerializeField]
        public int enemyRootLayer = 10;
        [SerializeField]
        public int enemyBlenderLayer = 11;
        [SerializeField]
        public int enemyParticlesLayer = 12;

        [Header("Damage Colliders")]
        [SerializeField]
        public int playerDamageColliderLayer = 13;
        [SerializeField]
        public int enemyDamageColliderLayer = 14;

        [Header("Interactables")]
        [SerializeField]
        public int playerInteractableLayer = 15;
        [SerializeField]
        public int enemyInteractableLayer = 16;

        [Header("World")]
        [SerializeField]
        public int walkable = 17;               // => walkable & stepable
        [SerializeField]
        public int unwalkableLayer = 18;        // => unwalkable & unstepable
        [SerializeField]
        public int walkable_UnStepable = 19;    // => walkable & unstepable
        [SerializeField]
        public int enemyRagdollLayer = 20;
        [SerializeField]
        public int unwalkableUnCameraCollidableLayer = 21;
        [SerializeField]
        public int enemyDeadLayer = 22;
        [SerializeField]
        public int commentaryZoneLayer = 23;
        [SerializeField]
        public int playerSkinnedMeshLayer = 29;
        [SerializeField]
        public int defaultLayer = 0;
        
        [Header("Player Masks")]
        public LayerMask _playerStepCheckMask;
        public LayerMask _playerGroundCheckMask;
        public LayerMask _cameraCollisionIncludedMask;     /// objs camera can collider with.
        public LayerMask _lockonTargetMask;
        public LayerMask _lockonObstaclesMask;
        public LayerMask _surroundLookAtIKTargetMask;

        [Header("Enemy Masks")]
        public LayerMask _enemyGroundCheckMask;
        public LayerMask _enemyInteractableMask;
        public LayerMask _enemyRigidbodyExcludeMask;
        public LayerMask _returnalProjectileObstaclesMask;
        public LayerMask _raycastCheckBehindWallMask;
        public LayerMask _fixDirectionMoveObstaclesMask;
        public LayerMask _damageParticleCollidableMask;
        public LayerMask _executionObstaclesMask;

        public static LayerManager singleton;
        private void Awake()
        {
            if (singleton == null)
                singleton = this;
            else
                Destroy(this);
        }
        
        public bool IsInLayer(int _layer, LayerMask _layerMask)
        {
            return _layerMask == (_layerMask | (1 << _layer));
        }
    }
}