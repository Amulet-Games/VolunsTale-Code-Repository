using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_Gnd_Proj_DamageParticle : AI_DamageParticle
    {
        [Header("Dp Id.")]
        public int _proj_Dp_ID;

        [Header("Tracking.")]
        public float trackingMoveSpeed;
        public float trackingRate;

        [Header("Positioning")]
        public Vector3 spawningLocalPosition;
        public float _YAxisOffset;
        
        [Header("On Ground.")]
        public float _rayStartHeight = 0.7f;
        public float _rayLength = 1;

        [Header("Off Particles.")]
        public float offParticleWaitRate;
        [ReadOnlyInspector] public float offParticleWaitTimer;

        [Header("Status.")]
        [ReadOnlyInspector] public float trackingTimer;
        [ReadOnlyInspector] public bool isParticleStopped;
        [ReadOnlyInspector] public bool projectileHasEnded;
        [ReadOnlyInspector] public LayerManager _layerManager;

        [Header("Triggering DP Id.")]
        public int _trigger_area_dp_Id;

        #region Privates.
        int _rayNumHit;

        Vector3 _rayOrigin;
        Vector3 _trackingPos;
        Vector3 _overwriteYValuePos;

        RaycastHit[] _groundCheckHit = new RaycastHit[1];
        #endregion

        #region Callback Methods.
        public void OnTriggerEnter(Collider other)
        {
            if (projectileHasEnded)
                return;

            if (_layerManager.IsInLayer(other.gameObject.layer, _layerManager._damageParticleCollidableMask))
            {
                Vector3 _triggerPos;
                AI_AreaDamageParticle_Base _area_dp;

                StopParticlePlaying();

                GetTargetDamageParticle();

                TriggerDamageParticleInPos();

                void GetTargetDamageParticle()
                {
                    _area_dp = _aiSessionManager.GetSinglesAreaDP_ById(_trigger_area_dp_Id);
                    ai.SetCurrentDamageParticle(_area_dp);
                }

                void TriggerDamageParticleInPos()
                {
                    _triggerPos = transform.position;
                    _triggerPos.y += _YAxisOffset;

                    _area_dp.OnDamageParticleEffectSpecificPosition(_triggerPos, _playerStates.vector3Zero);
                }
            }
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
            HandleTrackingPlayer();
            HandleOffParticlesTimer();
        }

        void HandleTrackingPlayer()
        {
            trackingTimer += _aiSessionManager._delta;
            if (trackingTimer >= trackingRate)
            {
                StopParticlePlaying();
            }
            else
            {
                TrackingPlayer();
            }
        }

        void StopParticlePlaying()
        {
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            isParticleStopped = true;
            projectileHasEnded = true;
        }

        void HandleOffParticlesTimer()
        {
            if (isParticleStopped)
            {
                offParticleWaitTimer += _aiSessionManager._delta;
                if (offParticleWaitTimer >= offParticleWaitRate)
                {
                    OffDamageParticleEffect();
                }
            }
        }
        #endregion

        #region Tracking.
        void TrackingPlayer()
        {
            TrackPlayer_OnGround();

            MoveTowardTrackingPosition();
        }

        void TrackPlayer_OnGround()
        {
            float _aiYPos = ai.mTransform.position.y;

            _trackingPos = _playerStates.mTransform.position;
            _trackingPos.y = _aiYPos;

            _rayOrigin = transform.position;
            _rayOrigin.y = _aiYPos + _rayStartHeight;

            //Debug.DrawRay(_rayOrigin, -ai.vector3Up * _rayLength, Color.red);
            _rayNumHit = Physics.RaycastNonAlloc(_rayOrigin, -ai.vector3Up, _groundCheckHit, _rayLength, _layerManager._enemyGroundCheckMask);
            if (_rayNumHit > 0)
            {
                /// If We Hit Something...
                //Debug.Log("hit.point = " + _groundCheckHit[0].point);
                _trackingPos.y = _groundCheckHit[0].point.y + _YAxisOffset;
            }
        }

        void MoveTowardTrackingPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, _trackingPos, trackingMoveSpeed * _aiSessionManager._delta);

            _overwriteYValuePos = transform.position;
            _overwriteYValuePos.y = _trackingPos.y;
            transform.position = _overwriteYValuePos;
        }
        #endregion

        #region On Area Damage Effect.
        public override void OnDamageParticleEffect()
        {
            // sub class
            On_ProjDp_ResetStatus();
            On_Dp_ResetTrans();

            /*Has to set the transform first.*/
            AddToActiveDpList();
        }

        void On_ProjDp_ResetStatus()
        {
            projectileHasEnded = false;
            isParticleStopped = false;

            trackingTimer = 0;
            offParticleWaitTimer = 0;
        }

        void On_Dp_ResetTrans()
        {
            transform.parent = null;
            
            Vector3 _enemyFwdEuler = ai.mTransform.eulerAngles;
            _enemyFwdEuler.x = 0;
            _enemyFwdEuler.z = 0;
            transform.eulerAngles = _enemyFwdEuler;

            transform.position = ai.mTransform.position + spawningLocalPosition;

            gameObject.SetActive(true);
        }
        #endregion

        #region Off Area Damage Effect.
        void OffDamageParticleEffect()
        {
            // base class
            RemoveFromActiveDpList();
            ReturnToBackpack();
        }
        
        void ReturnToBackpack()
        {
            transform.parent = _aiSessionManager._ai_dpHub_Backpack;
            transform.localPosition = spawningLocalPosition;
            gameObject.SetActive(false);
        }
        #endregion

        #region Init.
        public override void Init()
        {
            InitReference();
            InitStatus();
        }

        void InitReference()
        {
            Base_InitReference();
            _layerManager = LayerManager.singleton;
        }

        void InitStatus()
        {
            projectileHasEnded = false;
            trackingTimer = 0;
        }
        #endregion
    }
}