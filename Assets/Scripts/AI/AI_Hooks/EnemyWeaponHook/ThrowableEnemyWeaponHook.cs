using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ThrowableEnemyWeaponHook : BaseEnemyWeaponHook
    {
        [Header("Throwable Refs.")]
        [ReadOnlyInspector] public ThrowableEnemyDamageCollider e_throwableDamageCollider;

        public void Setup(AIManager _ai)
        {
            ai = _ai;

            e_throwableDamageCollider = GetComponentInChildren<ThrowableEnemyDamageCollider>();
            e_throwableDamageCollider.Setup(ai);
        }

        public void ReSetup(AIManager _ai)
        {
            /// Execute when Weapon is Poolable.

            ai = _ai;
            e_throwableDamageCollider._ai = ai;
            e_throwableDamageCollider._referingRuntimeThrowWeapon = ai.currentThrowableWeapon;
        }

        public override void SetColliderStatusToTrue()
        {
            e_throwableDamageCollider._collider.enabled = true;
        }

        public override void SetColliderStatusToFalse()
        {
            e_throwableDamageCollider._collider.enabled = false;
        }
    }
}