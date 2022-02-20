using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Player/Player Start Animation Transition")]
    public class Player_StartAnimationTransition : Condition
    {
        public override bool CheckCondition(StateManager states)
        {
            return states.IsEnterWaitForAnimationEndTransition();
        }

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return false;
        }
    }
}