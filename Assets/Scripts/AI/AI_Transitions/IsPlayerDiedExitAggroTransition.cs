using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/IsPlayerDiedExitAggroTransition")]
    public class IsPlayerDiedExitAggroTransition : Condition
    {
        public override bool CheckAICondition(AIStateManager aiState)
        {
            return aiState.aiManager.IsExitAggroTransition_OnPlayerDead();
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}