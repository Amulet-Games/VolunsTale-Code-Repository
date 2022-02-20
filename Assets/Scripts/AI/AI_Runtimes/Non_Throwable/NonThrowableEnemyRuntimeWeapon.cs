using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class NonThrowableEnemyRuntimeWeapon : EnemyRuntimeWeapon
    {
        [Header("Child. Drag and Drops")]
        public StandardEnemyWeaponHook e_hook;

        [Header("Refs.")]
        [ReadOnlyInspector] public NonThrowableEnemyWeapon referedWeapon;
        [ReadOnlyInspector] public EnemyRuntimeSidearm linkedRuntimeSidearm;
        
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

        #region Sidearm.
        public void SetupLinkedRuntimeSideArm(EnemySidearm _referedSideArm)
        {
            linkedRuntimeSidearm = _referedSideArm.GetRuntimeSidearm();
            linkedRuntimeSidearm.Init(_ai);
        }

        public override Transform GetSidearmTransform()
        {
            return linkedRuntimeSidearm.transform;
        }

        public override void SheathSidearmToPosition()
        {
            linkedRuntimeSidearm.SheathSidearmToPosition(_ai);
        }

        public override void ParentEnemySidearmUnderHand()
        {
            linkedRuntimeSidearm.ParentEnemySidearmUnderHand(_ai);
        }

        public override void SetSidearmColliderStatusToTrue()
        {
            linkedRuntimeSidearm.e_weaponHook.SetColliderStatusToTrue();
        }

        public override void SetSidearmColliderStatusToFalse()
        {
            linkedRuntimeSidearm.e_weaponHook.SetColliderStatusToFalse();
        }
        #endregion

        #region Egil KMA.
        public override void On_KMA_OverlapBoxHitPlayer()
        {
            e_hook.On_KMA_OverlapBoxHitPlayer();
        }
        #endregion

        #region Setup.
        public void SetupNonThrowableRuntimeWeapon(NonThrowableEnemyWeapon _referedWeapon)
        {
            referedWeapon = _referedWeapon;

            SetupWeaponToPosition();
            SetupWeaponHook();
            SetupRigidbody();
        }

        public void SetupBossNonThrowableRuntimeWeapon(NonThrowableEnemyWeapon _referedWeapon)
        {
            referedWeapon = _referedWeapon;

            SetupWeaponHook();
            SetupRigidbody();
        }
        
        void SetupWeaponToPosition()
        {
            _ai.SetupWeaponToPosition(transform);
            gameObject.SetActive(true);
        }

        void SetupWeaponHook()
        {
            e_hook.Setup(_ai);
        }
        #endregion
    }
}