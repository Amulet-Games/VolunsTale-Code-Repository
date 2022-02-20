using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class InvokeDP_ThrowableDamageCollider : BaseEnemyDamageCollider
    {
        [Header("Base Config.")]
        public float _damage;

        [Header("Refs.")]
        [ReadOnlyInspector] public ThrowableEnemyRuntimeWeaponBase _referingRuntimeThrowWeapon;
        [ReadOnlyInspector] public Transform mTransform;
    }
}