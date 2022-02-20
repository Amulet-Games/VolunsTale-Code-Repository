using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Player/Player Is Enter Knock Down State Transition")]
    public class Player_IsEnterKnockDownStateTransition : Condition
    {
        public override bool CheckCondition(StateManager states)
        {
            return states._startGetupCounting;
        }

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return false;
        }
    }
}