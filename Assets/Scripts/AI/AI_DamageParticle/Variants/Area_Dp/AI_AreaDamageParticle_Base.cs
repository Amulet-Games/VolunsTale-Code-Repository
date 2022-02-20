using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class AI_AreaDamageParticle_Base : AI_DamageParticle
    {
        [Header("Affect Config.")]
        public float _affectRadius;

        [Header("AOE Config.")]
        public float _spawnPosYBuffer;
        public bool isSpawnUnderneathPlayer;
        public bool isRepeatDamage;
        
        // sub class
        [Header("OverlapSphere Config.")]
        public int nonAllocHitColliderCount;
        public int overlapCheckIntervalCount;

        [Tooltip("The particle's playback time (in seconds) that starts overlap sphere check. You can check it's playback time in editor 'Particle Effect'.")]
        public float startCheckingTimeInSecond;
        [Tooltip("The particle's playback time (in seconds) that stops overlap sphere check. You can check it's playback time in editor 'Particle Effect'.")]
        public float stopCheckingTimeInSecond;
        
        [Header("Status.")]
        [ReadOnlyInspector] public bool _hasHurtPlayer;

        #region Non Serialized.
        [Header("Non Serialized.")]
        [ReadOnlyInspector, SerializeField] protected Collider[] hitColliders;
        protected LayerMask _playerLayerMask;
        #endregion
        
        #region Tick.
        protected void HandleOverlapSphere()
        {
            float _currentPlayTime = _particleSystem.time;
            if (_currentPlayTime >= startCheckingTimeInSecond && _currentPlayTime < stopCheckingTimeInSecond)
            {
                if (isRepeatDamage)
                {
                    RepeatOverlapSphereCheck();
                }
                else
                {
                    OverlapSphereCheck();
                }
            }
        }

        protected void RepeatOverlapSphereCheck()
        {
            if (!_playerStates.isInvincible)
            {
                if (_aiSessionManager._frameCount % overlapCheckIntervalCount == 0)
                {
                    int totalHitNum = Physics.OverlapSphereNonAlloc(transform.position, _affectRadius, hitColliders, _playerLayerMask);
                    if (totalHitNum > 0)
                    {
                        _playerStates._hitPoint = hitColliders[0].ClosestPoint(transform.position);
                        _playerStates.OnOverlapSphereHit(this);
                    }
                }
            }
        }

        protected void OverlapSphereCheck()
        {
            if (!_playerStates.isInvincible)
            {
                int totalHitNum = Physics.OverlapSphereNonAlloc(transform.position, _affectRadius, hitColliders, _playerLayerMask);
                if (totalHitNum > 0)
                {
                    _playerStates._hitPoint = hitColliders[0].ClosestPoint(transform.position);
                    _playerStates.OnOverlapSphereHit(this);
                    _hasHurtPlayer = true;
                }
            }
        }
        #endregion

        #region On Area Damage Effect.
        public override void OnDamageParticleEffect()
        {
            // base.
            AddToActiveDpList();

            // sub class
            On_Dp_ResetTrans();

            void On_Dp_ResetTrans()
            {
                transform.parent = null;
                transform.eulerAngles = _playerStates.vector3Zero;

                if (isSpawnUnderneathPlayer)
                {
                    Vector3 playerPredictOffset = _playerStates.moveDirection * 1.2f;
                    transform.position = _playerStates.mTransform.position + playerPredictOffset;
                }
                else
                {
                    Vector3 _enemyPos = ai.mTransform.position;
                    _enemyPos.y += _spawnPosYBuffer;
                    transform.position = _enemyPos;
                }

                gameObject.SetActive(true);
            }
        }
        
        public void OnDamageParticleEffectSpecificPosition(Vector3 _pos, Vector3 _eulers)
        {
            // base.
            AddToActiveDpList();

            // sub class
            SetupParticleTransformSpecificPosition();

            void SetupParticleTransformSpecificPosition()
            {
                transform.parent = null;
                transform.position = _pos;
                transform.eulerAngles = _eulers;
                gameObject.SetActive(true);
            }
        }
        #endregion

        #region Off Area Damage Effect.
        protected void Off_AreaDp_ResetStatus()
        {
            _hasHurtPlayer = false;
        }
        #endregion

        #region Init.
        protected void AreaDamageParticle_Init()
        {
            Base_InitReference();

            hitColliders = new Collider[nonAllocHitColliderCount];
            _playerLayerMask = (1 << _playerStates.gameObject.layer);
        }
        #endregion
    }
}