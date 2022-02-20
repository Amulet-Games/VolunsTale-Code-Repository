using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class SurviorCapePowerup : RuntimePowerup
    {
        public override void ChangeStatsWithPowerup(StatsAttributeHandler _statsHandler)
        {
            _statsHandler._states.invincibleRate *= 1.4f;
        }

        public override void UndoPowerupStatsChanges(StatsAttributeHandler _statsHandler)
        {
            _statsHandler._states.invincibleRate /= 1.4f;
        }
    }
}
