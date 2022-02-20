using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Is Exit KMA Wait For Animation Finish Transition")]
    public class IsExit_KMA_WaitForAnimationFinishTransition : Condition
    {
        public override bool CheckAICondition(AIStateManager aiStates)
        {
            return aiStates.IsExit_KMA_WaitForAnimationFinishTransition();
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}