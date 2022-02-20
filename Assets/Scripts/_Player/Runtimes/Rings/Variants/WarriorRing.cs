using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class WarriorRing : RuntimeRing
    {
        public override void ChangeStatsWithRing(StatsAttributeHandler _statsHandler)
        {
            _statsHandler.Modify_Multi_Strength_Value(2);
        }

        public override void UndoRingStatsChanges(StatsAttributeHandler _statsHandler)
        {
            _statsHandler.Modify_Multi_Strength_Value(-2);
        }
    }
}