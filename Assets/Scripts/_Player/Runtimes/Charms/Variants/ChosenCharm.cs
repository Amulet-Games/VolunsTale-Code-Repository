using System;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    class ChosenCharm : RuntimeCharm
    {
        public override void ChangeStatsWithCharm(StateManager _states)
        {
            _states.isReduceDmgTaken = true;
        }

        public override void UndoCharmStatsChanges(StateManager _states)
        {
            _states.isReduceDmgTaken = false;
        }
    }
}
