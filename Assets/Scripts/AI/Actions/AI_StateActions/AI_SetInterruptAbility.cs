using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_SetInterruptAbility")]
    public class AI_SetInterruptAbility : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.ResetIsInvincibleStatus();
        }

        public override void Execute(StateManager states)
        {
            //throw new System.NotImplementedException();
        }
    }
}