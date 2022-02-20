using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/PlayerDied_IsExitFacedPlayerTransition")]
    public class PlayerDied_IsExitFacedPlayerTransition : Condition
    {
        public float idleStateTransitRate = 1.25f;

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return aiState.aiManager.PlayerDied_IsExitAggroFacedPlayerTransition(idleStateTransitRate);
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}