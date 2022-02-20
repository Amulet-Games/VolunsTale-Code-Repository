using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Player/Player Is Exit Fighter Mode Transition")]
    public class Player_IsExitFighterModeTransition : Condition
    {
        public override bool CheckCondition(StateManager states)
        {
            return states.IsExitFighterModeTransition();
        }

        public override bool CheckAICondition(AIStateManager aiState)
        {
            return false;
        }
    }
}