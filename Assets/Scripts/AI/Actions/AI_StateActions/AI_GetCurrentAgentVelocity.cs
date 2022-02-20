using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_GetCurrentAgentVelocity")]
    public class AI_GetCurrentAgentVelocity : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.GetCurrentAgentVelocity();
        }

        public override void Execute(StateManager states)
        {
        }
    }
}