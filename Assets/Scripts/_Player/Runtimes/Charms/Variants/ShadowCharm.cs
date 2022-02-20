using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ShadowCharm : RuntimeCharm
    {
        public override void ChangeStatsWithCharm(StateManager _states)
        {
            _states.isIncreaseBackstabDmg = true;
        }

        public override void UndoCharmStatsChanges(StateManager _states)
        {
            _states.isIncreaseBackstabDmg = false;
        }
    }
}