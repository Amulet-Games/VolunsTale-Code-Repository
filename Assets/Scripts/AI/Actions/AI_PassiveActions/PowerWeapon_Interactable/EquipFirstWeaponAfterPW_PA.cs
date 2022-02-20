using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Passive Actions/Equip First Weapon After PW.")]
    public class EquipFirstWeaponAfterPW_PA : AIPassiveAction
    {
        public override void Execute(AIManager ai)
        {
            ai.Equip_FW_AfterUsedPowerWeapon();
        }
    }
}