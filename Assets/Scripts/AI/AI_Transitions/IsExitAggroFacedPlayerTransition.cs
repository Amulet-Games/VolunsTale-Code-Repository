using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/IsExitFacedPlayerTransition")]
    public class IsExitAggroFacedPlayerTransition : Condition
    {
        public float idleStateTransitRate;

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return aiState.aiManager.ByDistance_IsExitAggroFacedPlayerTransition(idleStateTransitRate);
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}