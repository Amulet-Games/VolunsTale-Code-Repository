using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/IsExitOffParryExecuteFacedPlayerTransition")]
    public class IsExitOffParryExecuteFacedPlayerTransition : Condition
    {
        public float _facePlayerWaitRate;

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return aiState.aiManager.IsExitParryExecuteFacedPlayerTransition(_facePlayerWaitRate);
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}