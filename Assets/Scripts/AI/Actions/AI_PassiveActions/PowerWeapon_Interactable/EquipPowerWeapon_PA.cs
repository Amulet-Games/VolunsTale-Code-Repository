using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Passive Actions/Equip Power Weapon")]
    public class EquipPowerWeapon_PA : AIPassiveAction
    {
        public override void Execute(AIManager ai)
        {
            ai.EquipPowerWeapon();
        }
    }
}