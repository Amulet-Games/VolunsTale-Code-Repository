using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class ThrowableEnemyRuntimeWeapon : ThrowableEnemyRuntimeWeaponBase
    {
        [Header("Child. Drag and Drops")]
        public InvokeDp_ThrowableEnemyWeaponHook e_hook;

        [Header("Refs.")]
        [ReadOnlyInspector] public ThrowableEnemyRuntimeWeaponPool referedPool;
        
        #region Abstract.
        public override void ParentEnemyWeaponUnderHand()
        {
            _ai.isWeaponOnHand = true;

            transform.parent = _ai.anim.GetBoneTransform(HumanBodyBones.RightHand);
            transform.localPosition = _ai.vector3Zero;
            transform.localEulerAngles = _ai.vector3Zero;
            transform.localScale = _ai.vector3One;
        }

        public override void SetColliderStatusToTrue()
        {
            e_hook.SetColliderStatusToTrue();
        }

        public override void SetColliderStatusToFalse()
        {
            e_hook.SetColliderStatusToFalse();
        }
        #endregion

        #region Throwable.
        public override void SetAICurrentThrowable()
        {
            _ai.currentThrowableWeapon = this;
        }
        #endregion

        #region Setup.
        public void SetupThrowableRuntimeWeapon(ThrowableEnemyRuntimeWeaponPool _pool)
        {
            referedPool = _pool;

            SetupWeaponToPosition();
            SetupWeaponHook();
            SetupRigidbody();

            SetupRuntimeChildTransform();
            
            isThrowableInited = true;

            void SetupWeaponHook()
            {
                e_hook.Setup(_ai);
            }
        }

        public void ReSetupThrowableRuntimeWeapon()
        {
            SetupWeaponToPosition();
            ReSetupWeaponHook();
            ReSetupRigidbody();

            ResetRuntimeChildTransform();

            void ReSetupWeaponHook()
            {
                e_hook.ReSetup(_ai);
            }

            void ReSetupRigidbody()
            {
                rb.isKinematic = true;
            }
        }

        void SetupWeaponToPosition()
        {
            _ai.SetupWeaponToPosition(transform);
            gameObject.SetActive(true);
        }
        #endregion
    }
}