using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/UpdateControlByAgentStateBools")]
    public class UpdateControlByAgentStateBools : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.UpdateControlByAgentStateBools();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}