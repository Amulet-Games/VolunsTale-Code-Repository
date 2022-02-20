using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class InvokeDp_ThrowableEnemyWeaponHook : BaseEnemyWeaponHook
    {
        [Header("InvokeDp Throwable Refs.")]
        [ReadOnlyInspector] public InvokeDP_ThrowableDamageCollider e_invokeDp_damageCollider;

        public void Setup(AIManager _ai)
        {
            ai = _ai;

            e_invokeDp_damageCollider = GetComponentInChildren<InvokeDP_ThrowableDamageCollider>();
            e_invokeDp_damageCollider.Setup(ai);
        }

        public void ReSetup(AIManager _ai)
        {
            /// Execute when Weapon is Poolable.
            
            ai = _ai;
            e_invokeDp_damageCollider._ai = ai;
            e_invokeDp_damageCollider._referingRuntimeThrowWeapon = ai.currentThrowableWeapon;
        }

        public override void SetColliderStatusToTrue()
        {
            e_invokeDp_damageCollider._collider.enabled = true;
        }

        public override void SetColliderStatusToFalse()
        {
            e_invokeDp_damageCollider._collider.enabled = false;
        }
    }
}