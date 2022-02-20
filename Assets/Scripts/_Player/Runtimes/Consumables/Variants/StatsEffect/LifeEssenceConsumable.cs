using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class LifeEssenceConsumable : StatsEffectConsumable
    {
        public override void ExecuteStatsEffect(StatsAttributeHandler _statsHandler)
        {
            _statsHandler.IncrementPlayerHealth(_referedStatsEffectItem.effectAmount);
        }
    }
}