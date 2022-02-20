using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Passive Actions/Switch Weapon Ready Passive Action")]
    public class SwitchWeaponReady_PA : AIPassiveAction
    {
        public override void Execute(AIManager ai)
        {
            ai.SwitchWeaponReadyPassiveAction();
        }
    }
}