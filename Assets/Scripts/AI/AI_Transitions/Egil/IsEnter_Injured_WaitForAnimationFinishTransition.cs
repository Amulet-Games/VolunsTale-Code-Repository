using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Is Enter Injured Wait For Animation End Transition")]
    public class IsEnter_Injured_WaitForAnimationFinishTransition : Condition
    {
        public override bool CheckAICondition(AIStateManager aiStates)
        {
            return aiStates.IsEnter_Injured_WaitForAnimationFinishTransition();
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}