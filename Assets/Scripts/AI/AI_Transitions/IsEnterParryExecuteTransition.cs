using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Is Enter Parry Execute Transition")]
    public class IsEnterParryExecuteTransition : Condition
    {
        public override bool CheckAICondition(AIStateManager aiStates)
        {
            return aiStates.aiManager.IsEnterParryExecuteTransition();
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}