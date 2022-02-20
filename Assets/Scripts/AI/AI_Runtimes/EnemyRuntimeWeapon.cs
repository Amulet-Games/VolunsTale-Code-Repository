using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class EnemyRuntimeWeapon : MonoBehaviour
    {
        [Header("Base Drag and Drops.")]
        public Rigidbody rb;

        [Header("Base Refs.")]
        [ReadOnlyInspector] public AIManager _ai;

        [Header("Base Status")]
        public GameObject _weaponBuffEffectFX;

        #region Child Setup.
        protected void SetupRigidbody()
        {
            rb.isKinematic = true;
        }
        #endregion
        
        #region Abstract.
        public abstract void ParentEnemyWeaponUnderHand();

        public abstract void SetColliderStatusToTrue();

        public abstract void SetColliderStatusToFalse();
        #endregion

        #region Sidearm.
        public virtual void SheathSidearmToPosition()
        {
        }

        public virtual void ParentEnemySidearmUnderHand()
        {
        }

        public virtual void SetSidearmColliderStatusToTrue()
        {
        }

        public virtual void SetSidearmColliderStatusToFalse()
        {
        }

        public virtual Transform GetSidearmTransform()
        {
            return null;
        }
        #endregion

        #region Throwable.
        public virtual void SetAICurrentThrowable()
        {
        }
        #endregion

        #region Egil KMA.
        public virtual void On_KMA_OverlapBoxHitPlayer()
        {
        }
        #endregion

        #region Enemy Evolve Mod.
        public void ActivateWeaponBuffEffect()
        {
            _weaponBuffEffectFX.SetActive(true);
        }

        public void DeactivateWeaponBuffEffect()
        {
            _weaponBuffEffectFX.SetActive(false);
        }
        #endregion
    }
}