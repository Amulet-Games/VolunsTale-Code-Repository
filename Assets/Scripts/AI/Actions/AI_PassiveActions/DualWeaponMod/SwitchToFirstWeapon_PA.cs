using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Passive Actions/Switch To First Weapon Passive Action")]
    public class SwitchToFirstWeapon_PA : AIPassiveAction
    {
        public override void Execute(AIManager ai)
        {
            ai.SetCurrentFirstWeaponAfterAggro();
            ai.SwitchTo_FW_SetStatus();
        }
    }
}