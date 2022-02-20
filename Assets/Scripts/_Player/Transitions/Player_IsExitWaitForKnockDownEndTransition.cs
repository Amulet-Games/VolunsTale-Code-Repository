using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Player/Player Is Exit Wait For Knock Down End Transition.")]
    public class Player_IsExitWaitForKnockDownEndTransition : Condition
    {
        public override bool CheckCondition(StateManager states)
        {
            return states.IsExitWaitForKnockedDownEndTransition();
        }

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return false;
        }
    }
}