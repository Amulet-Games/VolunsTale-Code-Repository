using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Mono Actions/Monitor Weapon Switch Wait")]
    public class MonitorWeaponSwitchWait : MonoAction
    {
        public override void Execute(StateManager states)
        {
            states.MonitorWeaponSwitch();
        }
    }
}