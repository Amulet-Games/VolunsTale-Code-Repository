using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Is Enter Wait For Animation End Transition")]
    public class IsEnterWaitForAnimationEndTransition : Condition
    {
        public override bool CheckAICondition(AIStateManager aiStates)
        {
            return aiStates.IsEnterWaitForAnimationEndTransition();
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}