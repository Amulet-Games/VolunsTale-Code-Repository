using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class VolunRing : RuntimeRing
    {
        public override void ChangeStatsWithRing(StatsAttributeHandler _statsHandler)
        {
            _statsHandler.Modify_Multi_Hp_Value(100);
        }

        public override void UndoRingStatsChanges(StatsAttributeHandler _statsHandler)
        {

            _statsHandler.Modify_Multi_Hp_Value(-100);
        }
    }
}
