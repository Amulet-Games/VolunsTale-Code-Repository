using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class CitrusFruitConsumable : StatsEffectConsumable
    {
        public override void ExecuteStatsEffect(StatsAttributeHandler _statsHandler)
        {
            _statsHandler._stamina_recover += _statsHandler.b_stamina_recover * _referedStatsEffectItem.effectAmount;
        }

        public override void OnCompleteReverseEffect(StatsAttributeHandler _statsHandler)
        {
            _statsHandler._stamina_recover -= _statsHandler.b_stamina_recover * _referedStatsEffectItem.effectAmount;
        }
    }
}