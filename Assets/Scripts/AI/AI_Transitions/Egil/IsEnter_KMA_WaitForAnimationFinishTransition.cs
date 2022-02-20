using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Is Enter KMA Wait For Animation End Transition")]
    public class IsEnter_KMA_WaitForAnimationFinishTransition : Condition
    {
        public override bool CheckAICondition(AIStateManager aiStates)
        {
            return aiStates.IsEnter_KMA_WaitForAnimationFinishTransition();
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}