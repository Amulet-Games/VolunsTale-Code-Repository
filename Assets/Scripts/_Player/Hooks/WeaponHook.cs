using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class WeaponHook : MonoBehaviour
    {
        [Header("Status.")]
        [HideInInspector] public DamageCollider dmgCollider;

        public void Init(RuntimeWeapon _runtimeWeapon)
        {
            dmgCollider = transform.GetChild(0).GetComponent<DamageCollider>();
            
            dmgCollider._runtimeWeapon = _runtimeWeapon;
            dmgCollider.Init();
        }
        
        public void SetColliderStatusToTrue()
        {
            dmgCollider.SetColliderStatusToTrue();
        }

        public void SetColliderStatusToFalse()
        {
            dmgCollider._collider.enabled = false;
        }
    }
}
