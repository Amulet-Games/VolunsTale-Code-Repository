using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Is Enter Wait For Parry Execution End Transition")]
    public class IsEnterWaitForParryExecutionEndTransition : Condition
    {
        public override bool CheckAICondition(AIStateManager aiStates)
        {
            return aiStates.aiManager.IsEnterWaitForParryExecutionEndTransition();
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}
