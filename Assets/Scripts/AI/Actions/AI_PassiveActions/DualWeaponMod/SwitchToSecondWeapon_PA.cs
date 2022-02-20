using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{ 
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Passive Actions/Switch To Second Weapon Passive Action")]
    public class SwitchToSecondWeapon_PA : AIPassiveAction
    {
        public override void Execute(AIManager ai)
        {
            ai.SetCurrentSecondWeaponAfterAggro();
            ai.SwitchTo_SW_SetStatus();
        }
    }
}