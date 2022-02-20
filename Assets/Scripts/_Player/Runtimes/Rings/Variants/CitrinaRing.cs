using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class CitrinaRing : RuntimeRing
    {
        public override void ChangeStatsWithRing(StatsAttributeHandler _statsHandler)
        {
            _statsHandler.Modify_Multi_Stamina_Value(100);
        }

        public override void UndoRingStatsChanges(StatsAttributeHandler _statsHandler)
        {
            _statsHandler.Modify_Multi_Stamina_Value(-100);
        }
    }
}