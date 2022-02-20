using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "WeaponAction/Buff Action/Axe Buff Action.")]
    public class AxeBuffAction : TimedWeaponBuffAction
    {
        public override void ExecuteWeaponBuffEffect(StatsAttributeHandler _statsHandler)
        {
            _statsHandler._attPowMulti_weaponArt = _statsHandler.b_attPowMulti_weaponArt * effectAmount;
        }

        public override void OnCompleteReverseEffect(StatsAttributeHandler _statsHandler)
        {
            _statsHandler._attPowMulti_weaponArt = _statsHandler.b_attPowMulti_weaponArt;
        }
    }
}