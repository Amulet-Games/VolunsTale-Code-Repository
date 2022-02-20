using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class ThrowReturnalProjectileMod : AIMod
    {
        [HideInInspector]
        public bool showThrowReturnalProjectileMod;
        
        [SerializeField]
        [Tooltip("The amount of time without randomized that enemy will need to wait before throwing projectile again.")]
        private float stdThrowWaitRate = 0;

        [SerializeField]
        [Tooltip("The amount of time to cut or add on \"stdThrowWaitRate\".")]
        private float throwWaitRateRandomizeAmount = 0;

        [SerializeField]
        [Tooltip("Set \"isThrowProjectile\" to true when enemy goes aggro, prevent enemy to throw projectile whenever they first seen player.")]
        private bool checkHasThrownProjectileInit;

        [SerializeField]
        [Tooltip("The length of distance that this projectile will travel when it thrown, it used to add on top of the distance to the player.")]
        private float targetThrowingDistance;

        [SerializeField]
        private float targetThrowingHeight;

        [SerializeField]
        [Tooltip("The direction that projectile will rotate itself when it is thrown.")]
        private Vector3 projectileRotateDir;

        [SerializeField]
        private float projectileThrownSpeed;

        [SerializeField]
        private float projectileRotateSpeed;

        [SerializeField]
        [Tooltip("The amount of time projectile will return to enemy once it's thrown.")]
        private float projectileReturnWaitRate;

        [SerializeField]
        [Tooltip("The length of distance from destination to the projectile which will consider the projectile has reached to destination.")]
        private float reachedDestinationThershold = 1.2f;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The finalized amount of time that enemy will need to wait before he can throwing projectile again")]
        private float finalizedThrowWaitTime = 0;
        
        [SerializeField, ReadOnlyInspector]
        public bool hasThrownProjectile;

        [SerializeField, ReadOnlyInspector]
        public bool isThrowingProjectile;

        [SerializeField, ReadOnlyInspector]
        public bool isDestinationReached;

        [SerializeField, ReadOnlyInspector]
        private bool isProjectileReturning;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The time that projectile will stay once it reached the targetDistance.")]
        private float projectileReturnWaitTimer;

        [SerializeField, ReadOnlyInspector]
        private float distanceToDestination;

        [SerializeField, ReadOnlyInspector]
        private float timeStartedLerping;

        [SerializeField, ReadOnlyInspector]
        private float timeSinceLerpingStarted;

        [SerializeField, ReadOnlyInspector]
        private Vector3 startingThrowPosition;
        [SerializeField, ReadOnlyInspector]
        private Vector3 targetThrowDestination;

        #region Non Serialized.

        #region AIManager.
        [NonSerialized] public float _delta;
        [NonSerialized] public AIManager _ai;
        [NonSerialized] public Transform _ai_mTransform;
        [NonSerialized] public Transform _currentWeaponTransform;
        #endregion

        #region Anim Hash.
        [NonSerialized] public int e_throw_ReturnalProjectile_start_hash;
        [NonSerialized] int e_throw_ReturnalProjectile_end_hash;
        #endregion

        #endregion

        /// INIT

        public void ThrowReturnalProjectileModInit(AIManager _ai)
        {
            this._ai = _ai;
            _ai_mTransform = _ai.mTransform;

            e_throw_ReturnalProjectile_start_hash = _ai.hashManager.e_throw_ReturnalProjectile_start_hash;
            e_throw_ReturnalProjectile_end_hash = _ai.hashManager.e_throw_ReturnalProjectile_end_hash;
        }

        /// TICK.

        #region Returnal Projectile Logic.
        public void MonitorThrownProjectile()
        {
            if (isThrowingProjectile || isDestinationReached)
            {
                if (!isDestinationReached)
                {
                    SetIsThrowningProjectileStatus(true);
                }
                else
                {
                    SetIsDestinationReachedStatus(true);
                }
            }
        }

        public void SetIsThrowningProjectileStatus(bool _isThrowingProjectile)
        {
            if (_isThrowingProjectile)
            {
                if (!isThrowingProjectile)
                {
                    isThrowingProjectile = true;

                    if (!isProjectileReturning)
                    {
                        SetupStartingStatus();
                        SetTargetThrowDestination();
                        SetIsThrowProjectileToTrue();
                    }

                    timeStartedLerping = Time.time;
                }
                else
                {
                    UpdateDistanceStatus();
                    LerpTowardThrowPosition();
                    RotateProjectile();

                    if (distanceToDestination < reachedDestinationThershold)
                    {
                        /// When projectile arrived to destination.
                        if (!isProjectileReturning)
                        {
                            SetIsDestinationReachedStatus(true);
                        }
                        /// When projectile returned to enemy's hand.
                        else
                        {
                            CatchReturnalProjectile();
                            _ai.PlayAnimationCrossFade(e_throw_ReturnalProjectile_end_hash, 0.2f, true);
                        }
                    }
                }
            }
        }
        
        void ReturnToStartingTransform()
        {
            _ai.ParentReturnProjectileToHand();
            _currentWeaponTransform = null;
        }

        void SetupStartingStatus()
        {
            _currentWeaponTransform = _ai.currentWeapon.transform;
            _ai.currentWeapon.transform.parent = null;

            startingThrowPosition = _currentWeaponTransform.localPosition;
        }

        void SetTargetThrowDestination()
        {
            targetThrowDestination = _ai_mTransform.position + (_ai.vector3Up * targetThrowingHeight) + _ai_mTransform.forward * targetThrowingDistance;
            UpdateDistanceStatus();
        }
        
        void UpdateDistanceStatus()
        {
            distanceToDestination = Vector3.Distance(_currentWeaponTransform.localPosition, targetThrowDestination);
        }

        void LerpTowardThrowPosition()
        {
            timeSinceLerpingStarted = Time.time - timeStartedLerping;
            float percentageComplete = timeSinceLerpingStarted / projectileThrownSpeed;
            _currentWeaponTransform.localPosition = Vector3.Lerp(_currentWeaponTransform.localPosition, targetThrowDestination, percentageComplete);
        }

        void RotateProjectile()
        {
            _currentWeaponTransform.Rotate(projectileRotateDir * projectileRotateSpeed * _delta);
        }
        
        void SetIsDestinationReachedStatus(bool _isDestinationReached)
        {
            if (_isDestinationReached)
            {
                if (!isDestinationReached)
                {
                    OnIsDestinationReached();
                    SetTargetReturnDestination();
                }
                else
                {
                    MonitorProjectileReturnWait();
                    RotateProjectile();
                }
            }
            else
            {
                OffIsDestinationReached();
                SetIsThrowningProjectileStatus(true);
            }
        }

        void SetTargetReturnDestination()
        {
            targetThrowDestination = startingThrowPosition;
            UpdateDistanceStatus();
        }

        void CatchReturnalProjectile()
        {
            ResetTimeStatus();
            ResetDistanceStatus();
            OffIsThrewProjectile();
            ReturnToStartingTransform();
        }

        void MonitorProjectileReturnWait()
        {
            projectileReturnWaitTimer += _delta;
            if (projectileReturnWaitTimer >= projectileReturnWaitRate)
            {
                projectileReturnWaitTimer = 0;
                SetIsDestinationReachedStatus(false);
            }
        }

        #region Reset.
        void ResetTimeStatus()
        {
            projectileReturnWaitTimer = 0;
            timeSinceLerpingStarted = 0;
            timeStartedLerping = 0;
        }

        void ResetDistanceStatus()
        {
            distanceToDestination = 0;
        }

        void OffIsThrewProjectile()
        {
            isProjectileReturning = false;
            isThrowingProjectile = false;
        }

        void OnIsDestinationReached()
        {
            isDestinationReached = true;
            isProjectileReturning = false;
            isThrowingProjectile = false;
        }

        void OffIsDestinationReached()
        {
            isDestinationReached = false;
            isProjectileReturning = true;
        }
        #endregion

        #endregion

        #region Throw Wait Logic.
        public void ThrowReturnalProjectileWaitTimeCount()
        {
            if (hasThrownProjectile)
            {
                finalizedThrowWaitTime -= _delta;
                if (finalizedThrowWaitTime <= 0)
                {
                    finalizedThrowWaitTime = 0;
                    hasThrownProjectile = false;
                }
            }
        }

        public void ReturnProjectileWhenHitObstacles()
        {
            SetIsDestinationReachedStatus(true);
        }

        void SetIsThrowProjectileToTrue()
        {
            hasThrownProjectile = true;
            RandomizeWithAddonValue(throwWaitRateRandomizeAmount, stdThrowWaitRate, ref finalizedThrowWaitTime);
        }
        #endregion

        #region Get Anim Hash.
        public int GetThrowReturnalProjectileHash()
        {
            return e_throw_ReturnalProjectile_start_hash;
        }
        #endregion

        public void ThrowReturnalProjectileGoesAggroReset()
        {
            if (checkHasThrownProjectileInit)
                hasThrownProjectile = true;

            RandomizeWithAddonValue(throwWaitRateRandomizeAmount, stdThrowWaitRate, ref finalizedThrowWaitTime);
        }

        public void ThrowReturnalProjectileExitAggroReset()
        {
            CatchReturnalProjectile();
            hasThrownProjectile = false;
            isDestinationReached = false;
            _currentWeaponTransform = null;
        }
    }
}