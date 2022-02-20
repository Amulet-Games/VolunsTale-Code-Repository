using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Is Knock Down Exit Parry Execution Transition")]
    public class IsKnockDownExitParryExecutionTransition : Condition
    {
        public override bool CheckAICondition(AIStateManager aiStates)
        {
            return aiStates.aiManager.IsKnockDownExitParryExecutionTransition();
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}