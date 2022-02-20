using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Player/Wait For Agent Interaction To End  Transition")]
    public class Player_WaitForAgentInteractionToEnd : Condition
    {
        public override bool CheckCondition(StateManager states)
        {
            if (!states._isAllowAgentInteraction)
            {
                states.OffAgentInteractionResetBools();
                return true;
            }

            return false;
        }

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return false;
        }
    }
}