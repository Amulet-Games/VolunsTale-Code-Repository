using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Player/Player Control By Agent Transition")]
    public class Player_ControlByAgentTransition : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            return state._isControlByAgent;
        }

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return false;
        }
    }
}