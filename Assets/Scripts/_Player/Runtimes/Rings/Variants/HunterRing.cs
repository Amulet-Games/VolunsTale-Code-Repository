using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HunterRing

namespace SA
{
    public class HunterRing : RuntimeRing
    {
        public override void ChangeStatsWithRing(StatsAttributeHandler _statsHandler)
        {
            _statsHandler.Modify_Multi_Hexes_Value(2);
        }

        public override void UndoRingStatsChanges(StatsAttributeHandler _statsHandler)
        {
            _statsHandler.Modify_Multi_Hexes_Value(-2);
        }
    }
}
