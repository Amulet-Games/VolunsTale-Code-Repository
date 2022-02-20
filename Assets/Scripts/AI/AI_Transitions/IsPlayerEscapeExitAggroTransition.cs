using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/IsPlayerEscapeExitAggroTransition")]
    public class IsPlayerEscapeExitAggroTransition : Condition
    {
        public override bool CheckAICondition(AIStateManager aiState)
        {
            return aiState.aiManager.IsExitAggroTransition_ByDistance();
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}