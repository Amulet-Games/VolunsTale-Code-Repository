using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Player/Agent Interaction Transition")]
    public class Player_AgentInteractionTransition : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            return state._isAllowAgentInteraction;
        }

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return false;
        }
    }
}