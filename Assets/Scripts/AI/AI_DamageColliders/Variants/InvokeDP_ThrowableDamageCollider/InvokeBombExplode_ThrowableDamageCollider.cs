using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class InvokeBombExplode_ThrowableDamageCollider : InvokeDP_ThrowableDamageCollider
    {
        [Header("Bomb Config.")]
        public LayerMask _affectedLayers;
        public float bombRayHitLength = 0.35f;
        [ReadOnlyInspector] public float bombRadius;

        [Header("Not Serizialized.")]
        [ReadOnlyInspector, SerializeField] Collider[] hitColliders = new Collider[1];  /// 'Public' is test only

        private void Update()
        {
            if (_referingRuntimeThrowWeapon.isStartCheckingHitObstacles)
            {
                GetBombHitPoint();
            }
        }
        
        void GetBombHitPoint()
        {
            //Debug.DrawRay(mTransform.position, mTransform.forward * bombRayHitLength, Color.black);
            
            if (Physics.Raycast(mTransform.position, mTransform.forward, out RaycastHit hit, bombRayHitLength, _affectedLayers))
            {
                GameObject _hitObject = hit.transform.gameObject;
                if (_hitObject == _ai.gameObject)
                    return;

                if (_hitObject.layer == _layerManager.playerLayer)
                {
                    GetBombExplodeParticle().OnDamageParticleEffectSpecificPosition(hit.point, Quaternion.LookRotation(playerStates.mTransform.up).eulerAngles);
                }
                else if (_hitObject.layer == _layerManager.walkable)
                {
                    Vector3 groundToBombDir = mTransform.position - _hitObject.transform.position;

                    Vector3 _effectSpawnPos = hit.point;
                    _effectSpawnPos.y += 0.2f;

                    if (Vector3.Dot(_hitObject.transform.up, groundToBombDir) > 0)
                    {
                        /// ground is facing upward.
                        GetBombExplodeParticle().OnDamageParticleEffectSpecificPosition(_effectSpawnPos, Quaternion.LookRotation(_hitObject.transform.up).eulerAngles);
                    }
                    else
                    {
                        /// ground is facing downward.
                        GetBombExplodeParticle().OnDamageParticleEffectSpecificPosition(_effectSpawnPos, Quaternion.LookRotation(-_hitObject.transform.up).eulerAngles);
                    }
                }
                else
                {
                    Vector3 _turnDir = mTransform.position - hit.point;
                    GetBombExplodeParticle().OnDamageParticleEffectSpecificPosition(hit.point, Quaternion.LookRotation(_turnDir).eulerAngles);
                }

                OnDamageParticleSpawned();
            }
            else
            {
                int totalHitNum = Physics.OverlapSphereNonAlloc(mTransform.position, bombRadius, hitColliders, _affectedLayers);
                if (totalHitNum > 0)
                {
                    if (hitColliders[0].gameObject == _ai.gameObject)
                        return;
                    
                    GetBombExplodeParticle().OnDamageParticleEffectSpecificPosition(mTransform.position, Quaternion.LookRotation(-mTransform.forward).eulerAngles);
                    OnDamageParticleSpawned();
                }
            }
        }

        AI_AreaDamageParticle_Base GetBombExplodeParticle()
        {
            AI_AreaDamageParticle_Base _bombExplodeParticle = _ai.aISessionManager._bombExplode_AreaDp_Pool.Get();
            _ai.SetCurrentDamageParticle(_bombExplodeParticle);
            return _bombExplodeParticle;
        }

        void OnDamageParticleSpawned()
        {
            _referingRuntimeThrowWeapon.isStartCheckingHitObstacles = false;
            _referingRuntimeThrowWeapon.gameObject.SetActive(false);

            BeginForecReturnToPoolWait();

            void BeginForecReturnToPoolWait()
            {
                LeanTween.value(0, 1, 0.1f).setOnComplete(_referingRuntimeThrowWeapon.ForceReturnToPool);
            }
        }
        
        #region Init.
        public override void Setup(AIManager _ai)
        {
            BaseInit(_ai);
            bombRadius = mTransform.localScale.x * 0.5f + 0.05f;
        }

        void BaseInit(AIManager aiManager)
        {
            _collider.enabled = false;
            _collider.isTrigger = true;

            _ai = aiManager;
            playerStates = _ai.playerStates;

            _layerManager = LayerManager.singleton;
            gameObject.layer = _layerManager.enemyDamageColliderLayer;

            _referingRuntimeThrowWeapon = _ai.currentThrowableWeapon;
            mTransform = transform;
        }
        #endregion

        #region On Draw Gizmos.
        //private void OnDrawGizmos()
        //{
        //    //Debug.Log("transform.lossyScale = " + transform.lossyScale);
        //    //Debug.Log("transform.localScale = " + transform.localScale);
        //    Gizmos.DrawWireSphere(mTransform.position, 0.075f);
        //}
        #endregion
    }
}