using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/IsFoundPlayerTransition")]
    public class IsFoundPlayerTransition : Condition
    {
        public override bool CheckAICondition(AIStateManager aIState)
        {
            return aIState._hasFoundPlayer;
        }

        public override bool CheckCondition(StateManager state)
        {
            return false;
        }
    }
}