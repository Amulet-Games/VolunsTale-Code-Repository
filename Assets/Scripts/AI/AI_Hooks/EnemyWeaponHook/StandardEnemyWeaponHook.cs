using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class StandardEnemyWeaponHook : BaseEnemyWeaponHook
    {
        [Header("Standard Refs.")]
        [ReadOnlyInspector] public StandardEnemyDamageCollider e_damageCollider;
        
        public void Setup(AIManager _ai)
        {
            ai = _ai;

            e_damageCollider = GetComponentInChildren<StandardEnemyDamageCollider>();
            e_damageCollider.Setup(ai);
        }

        public override void SetColliderStatusToTrue()
        {
            e_damageCollider._collider.enabled = true;
        }

        public override void SetColliderStatusToFalse()
        {
            e_damageCollider._collider.enabled = false;
        }

        public void On_KMA_OverlapBoxHitPlayer()
        {
            e_damageCollider.KMA_OnDamageColliderHit();
        }
    }
}