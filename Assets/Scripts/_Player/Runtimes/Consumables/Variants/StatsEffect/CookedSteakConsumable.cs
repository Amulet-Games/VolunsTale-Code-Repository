using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class CookedSteakConsumable : StatsEffectConsumable
    {
        public override void ExecuteStatsEffect(StatsAttributeHandler _statsHandler)
        {
            _statsHandler.Modify_Multi_Stamina_Value(_referedStatsEffectItem.effectAmount);
        }

        public override void OnCompleteReverseEffect(StatsAttributeHandler _statsHandler)
        {
            _statsHandler.Modify_Multi_Stamina_Value(-_referedStatsEffectItem.effectAmount);
        }
    }
}