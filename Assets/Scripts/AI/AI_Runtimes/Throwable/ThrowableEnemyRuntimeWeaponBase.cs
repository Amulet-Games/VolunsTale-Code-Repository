using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class ThrowableEnemyRuntimeWeaponBase : EnemyRuntimeWeapon
    {
        [Header("Configuration")]
        public float maxLifeTime = 6f;

        [Header("Runtime Weapon Status.")]
        [ReadOnlyInspector] public float lifeTime;
        [ReadOnlyInspector] public bool isThrowableInited;
        [ReadOnlyInspector] public bool isStartCheckingHitObstacles;

        [Header("Child gameObject ref.")]
        [ReadOnlyInspector, SerializeField] protected Transform _throwableRuntimeChildTransform;
        protected Vector3 runtimePrefabChildPos;
        protected Vector3 runtimePrefabChildEuler;

        #region Tick.
        public void ReturnToPoolTick()
        {
            lifeTime += _ai._delta;
            if (lifeTime > maxLifeTime)
            {
                lifeTime = 0;
                ReturnToPool();
                _ai.aISessionManager.RemoveThrowableFromActiveList(this);
            }
        }
        #endregion

        public void OnTriggerCheckObstaclesHit()
        {
            if (isStartCheckingHitObstacles)
            {
                isStartCheckingHitObstacles = false;

                CreateThrowableBrokenImpactEffect();
                ForceReturnToPool();
            }
        }

        /// ABSTRACT
        public abstract void ThrowWeapon();

        public abstract void ReturnToPool();

        public abstract void ForceReturnToPool();

        protected abstract void CreateThrowableBrokenImpactEffect();
        
        /// VIRTUAL 
        public void SetupRuntimeChildTransform()
        {
            _throwableRuntimeChildTransform = transform.GetChild(0);
            runtimePrefabChildPos = _throwableRuntimeChildTransform.localPosition;
            runtimePrefabChildEuler = _throwableRuntimeChildTransform.localEulerAngles;
        }

        public void ResetRuntimeChildTransform()
        {
            _throwableRuntimeChildTransform.localPosition = runtimePrefabChildPos;
            _throwableRuntimeChildTransform.localEulerAngles = runtimePrefabChildEuler;
        }
    }
}