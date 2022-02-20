using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class FightersDrinkConsumable : StatsEffectConsumable
    {
        public override void ExecuteStatsEffect(StatsAttributeHandler _statsHandler)
        {
            _statsHandler._attPowMulti_consumable += _statsHandler.b_attPowMulti_consumable * _referedStatsEffectItem.effectAmount;
        }

        public override void OnCompleteReverseEffect(StatsAttributeHandler _statsHandler)
        {
            _statsHandler._attPowMulti_consumable -= _statsHandler.b_attPowMulti_consumable * _referedStatsEffectItem.effectAmount;
        }
    }
}