using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class FlashCharm : RuntimeCharm
    {
        public override void ChangeStatsWithCharm(StateManager _states)
        {
            _states.isExtendParryWindow = true;
        }

        public override void UndoCharmStatsChanges(StateManager _states)
        {
            _states.isExtendParryWindow = false;
        }
    }
}