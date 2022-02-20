using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ThrowableEnemyRuntimeBomb : ThrowableEnemyRuntimeWeapon
    {
        [Header("Bomb Velocity")]
        public float mixmumVelocity;
        public float lowestVelocity;
        [ReadOnlyInspector] public float currentVelocity;

        [Header("Distance Thershold")]
        public float thrownBombClosetThershold = 4.5f;

        [Header("Player Movement Prediction Offset")]
        public float offset;

        public override void ThrowWeapon()
        {
            transform.parent = null;
            rb.isKinematic = false;

            e_hook.SetColliderStatusToTrue();
            isStartCheckingHitObstacles = true;

            _ai.aISessionManager.AddThrowableToActiveList(this);

            if (_ai.distanceToTarget <= thrownBombClosetThershold)
            {
                rb.AddForce(_ai.mTransform.forward * 400);
            }
            else if (_ai.distanceToTarget <= _ai.aggro_Thershold)
            {
                StateManager _playerStates = _ai.playerStates;

                float targetOffset = _playerStates._isRunning ? offset + 1 : offset;
                Vector3 playPredictOffset = _playerStates.moveDirection * (targetOffset * _playerStates.moveAmount);
                Vector3 tarDir = (_playerStates.mTransform.position + playPredictOffset + _ai.vector3Up * 2.5f) - transform.position;

                Vector3 lookDir = Quaternion.LookRotation(tarDir).eulerAngles;
                lookDir.x += 45f;
                _throwableRuntimeChildTransform.transform.eulerAngles = lookDir;

                float t = _ai.distanceToTarget / _ai.aggro_Thershold;
                currentVelocity = lowestVelocity + (mixmumVelocity - lowestVelocity) * t;
                rb.AddForce(tarDir * currentVelocity);
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

            referedPool.ReturnToPool(this);
            _ai.aISessionManager.RemoveThrowableFromActiveList(this);
        }

        protected override void CreateThrowableBrokenImpactEffect()
        {
        }
    }
}