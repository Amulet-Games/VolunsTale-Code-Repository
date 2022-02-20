using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Player/Regain Control Back To Idle Transition")]
    public class RegainControlBackToIdleTransition : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            return state.RegainControlBackToIdleTransition();
        }

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return false;
        }
    }
}