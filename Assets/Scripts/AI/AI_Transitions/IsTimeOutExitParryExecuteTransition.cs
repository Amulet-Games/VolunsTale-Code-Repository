using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Is Time Out Exit Parry Execute Transition")]
    public class IsTimeOutExitParryExecuteTransition : Condition
    {
        [Tooltip("The length of time that this enemy will wait for player to execute parry execution.")]
        public float _parryExecuteWaitRate;

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return aiState.aiManager.IsTimeOutExitParryExecuteTransition(_parryExecuteWaitRate);
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}