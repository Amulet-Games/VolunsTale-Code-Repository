using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class ThrowableEnemyRuntimePowerWeapon : ThrowableEnemyRuntimeWeaponBase
    {
        [Header("Child. Drag and Drops")]
        public ThrowableEnemyWeaponHook e_hook;

        [Header("Refs.")]
        [ReadOnlyInspector] public ThrowableEnemyRuntimePowerWeaponPool referedPool;

        [Header("Duability")]
        [SerializeField]
        private int maxDuability = 5;
        [SerializeField]
        private int duability = 0;
        
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

        #region Power Weapon.
        public void DepletePowerWeaponDuability()
        {
            duability--;
            if (duability < 0)
            {
                lifeTime = 0;
                _ai.SetIsCurrentPowerWeaponBrokeToTrue();
            }
        }

        public void DepletePowerWeaponDuabiltiyByAmount(int _amount)
        {
            duability -= _amount;
            if (duability < 0)
            {
                lifeTime = 0;
                _ai.SetIsCurrentPowerWeaponBrokeToTrue();
            }
        }

        public void ResetDuability()
        {
            duability = maxDuability;
        }
        #endregion
        
        #region Setup.
        public void SetupRuntimePowerWeapon(ThrowableEnemyRuntimePowerWeaponPool _pool)
        {
            referedPool = _pool;

            ParentPowerWeaponUnderHand();
            SetupWeaponHook();
            SetupRigidbody();

            SetupRuntimeChildTransform();
            
            ResetDuability();

            isThrowableInited = true;

            void SetupWeaponHook()
            {
                e_hook.Setup(_ai);
            }
        }

        public void ReSetupRuntimePowerWeapon()
        {
            ParentPowerWeaponUnderHand();
            ReSetupWeaponHook();
            ReSetupRigidbody();

            ResetRuntimeChildTransform();

            ResetDuability();
            
            void ReSetupWeaponHook()
            {
                e_hook.ReSetup(_ai);
            }

            void ReSetupRigidbody()
            {
                rb.isKinematic = true;
            }
        }

        void ParentPowerWeaponUnderHand()
        {
            ParentEnemyWeaponUnderHand();
            gameObject.SetActive(true);
        }
        #endregion
    }
}