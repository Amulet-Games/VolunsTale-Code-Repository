using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Exit Wait For Animation End Transition")]
    public class IsExitWaitForAnimationEndTransition : Condition
    {
        public override bool CheckAICondition(AIStateManager aiStates)
        {
            return aiStates.IsExitWaitForAnimationEndTransition();
        }

        public override bool CheckCondition(StateManager state)
        {
            throw new System.NotImplementedException();
        }
    }
}