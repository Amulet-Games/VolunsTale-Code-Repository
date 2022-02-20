using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Is Dead Exit Parry Execute Transition")]
    public class IsDeadExitParryExecuteTransition : Condition
    {
        public override bool CheckAICondition(AIStateManager aiStates)
        {
            return aiStates.aiManager.IsDeadExitParryExecuteTransition();
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}