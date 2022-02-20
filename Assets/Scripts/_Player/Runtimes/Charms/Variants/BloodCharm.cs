using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BloodCharm : RuntimeCharm
    {
        public override void ChangeStatsWithCharm(StateManager _states)
        {
            _states.isGivenExtraEustusFlask = true;
        }

        public override void UndoCharmStatsChanges(StateManager _states)
        {
            _states.isGivenExtraEustusFlask = false;
        }
    }
}