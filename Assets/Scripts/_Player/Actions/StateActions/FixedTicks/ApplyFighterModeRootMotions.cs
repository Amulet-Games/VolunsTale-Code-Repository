using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/FixedTicks/ApplyFighterModeRootMotions")]
    public class ApplyFighterModeRootMotions : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.ApplyFighterModeRootMotions();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}