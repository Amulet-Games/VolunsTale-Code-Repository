using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class CrowCapePowerup : RuntimePowerup
    {
        public override void ChangeStatsWithPowerup(StatsAttributeHandler _statsHandler)
        {
            _statsHandler.b_stamina_recover += 15;
        }

        public override void UndoPowerupStatsChanges(StatsAttributeHandler _statsHandler)
        {
            _statsHandler.b_stamina_recover -= 15;
        }
    }
}
