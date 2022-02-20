using System;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    class KangarooCharm : RuntimeCharm
    {
        public override void ChangeStatsWithCharm(StateManager _states)
        {
            _states.isIncreaseDmgWhenCombo = true;
        }

        public override void UndoCharmStatsChanges(StateManager _states)
        {
            _states.isIncreaseDmgWhenCombo = false;
        }
    }
}
