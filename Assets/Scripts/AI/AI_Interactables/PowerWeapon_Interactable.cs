using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PowerWeapon_Interactable : AI_Interactable
    {
        [Header("Power Weapon Configuration")]
        public ThrowableEnemyRuntimePowerWeaponPool powerWeaponPool;
        public AI_PowerWeaponProfile pw_profile;
        
        public override void Execute(AIManager ai)
        {
            ai.ExecutePowerWeaponInteractable(this);
        }
    }
}