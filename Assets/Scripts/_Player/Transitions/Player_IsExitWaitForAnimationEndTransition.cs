using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA 
{
    [CreateAssetMenu(menuName = "Conditions/Player/Player Is Exit Wait For Animation End Transition.")]
    public class Player_IsExitWaitForAnimationEndTransition : Condition
    {
        public override bool CheckCondition(StateManager states)
        {
            // If Neglecting is false...
            return states.IsExitWaitForAnimationEndTransition();
        }

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return false;
        }
    }
}
