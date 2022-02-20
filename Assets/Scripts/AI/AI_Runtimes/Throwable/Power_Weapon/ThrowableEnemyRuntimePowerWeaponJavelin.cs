using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ThrowableEnemyRuntimePowerWeaponJavelin : ThrowableEnemyRuntimePowerWeapon
    {
        [Header("Spear Velocity")]
        public float mixmumVelocity;
        public float lowestVelocity;
        [ReadOnlyInspector] public float currentVelocity;

        [Header("Distance Thershold")]
        public float thrownJavelinClosetThershold = 4.5f;

        [Header("Player Movement Prediction Offset")]
        public float offset;
        
        public override void ThrowWeapon()
        {
            transform.parent = null;
            rb.isKinematic = false;
            e_hook.SetColliderStatusToTrue();

            isStartCheckingHitObstacles = true;
            _ai.aISessionManager.AddThrowableToActiveList(this);

            _ai.OffAiming();

            Vector3 lookDir = _ai.vector3Zero;

            if (_ai.distanceToTarget <= thrownJavelinClosetThershold)
            {
                lookDir = _ai.playerStates.mTransform.position - _throwableRuntimeChildTransform.transform.position;
                _throwableRuntimeChildTransform.transform.eulerAngles = Quaternion.LookRotation(lookDir).eulerAngles;

                rb.AddForce(lookDir * 150);
            }
            else if (_ai.distanceToTarget <= _ai.aggro_Thershold)
            {
                Vector3 playPredictOffset = _ai.playerStates.moveDirection * offset;
                lookDir = (_ai.playerStates.mTransform.position + playPredictOffset + _ai.vector3Up /*1.5f*/) - _throwableRuntimeChildTransform.transform.position;
                _throwableRuntimeChildTransform.transform.eulerAngles = Quaternion.LookRotation(lookDir).eulerAngles;

                float t = _ai.distanceToTarget / _ai.aggro_Thershold;
                currentVelocity = lowestVelocity + (mixmumVelocity - lowestVelocity) * t;
                rb.AddForce(lookDir * currentVelocity);
            }
            else
            {
                lookDir = _ai.playerStates.mTransform.position - _throwableRuntimeChildTransform.transform.position;
                _throwableRuntimeChildTransform.transform.eulerAngles = Quaternion.LookRotation(lookDir).eulerAngles;

                rb.AddForce(lookDir * 200);
            }
        }

        public override void ReturnToPool()
        {
            e_hook.SetColliderStatusToFalse();

            gameObject.SetActive(false);
            referedPool.ReturnToPool(this);
        }

        public override void ForceReturnToPool()
        {
            lifeTime = 0;
            e_hook.SetColliderStatusToFalse();

            gameObject.SetActive(false);
            referedPool.ReturnToPool(this);
            _ai.aISessionManager.RemoveThrowableFromActiveList(this);
        }

        protected override void CreateThrowableBrokenImpactEffect()
        {
            JavelinBroken_AIGeneralEffect _effect = _ai.aISessionManager._javelinBroken_pool.Get();
            _effect.SpawnEffect_AfterThrowableHitPlayer(_throwableRuntimeChildTransform);
        }
    }
}