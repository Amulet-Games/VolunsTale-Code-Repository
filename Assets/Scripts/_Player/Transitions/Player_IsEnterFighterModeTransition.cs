using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Player/Player Is Enter Fighter Mode Transition")]
    public class Player_IsEnterFighterModeTransition : Condition
    {
        public override bool CheckCondition(StateManager states)
        {
            return states._isTwoHandFistAttacking;
        }

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return false;
        }
    }
}