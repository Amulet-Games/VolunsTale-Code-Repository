using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class WindCapePowerup : RuntimePowerup
    {
        public override void ChangeStatsWithPowerup(StatsAttributeHandler _statsHandler)
        {
            _statsHandler._run_speed += 2f;
        }

        public override void UndoPowerupStatsChanges(StatsAttributeHandler _statsHandler)
        {
            _statsHandler._run_speed -= 2f;
        }
    }
}