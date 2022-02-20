using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/FixedTicks/AI_ApplyParryExecutionRootMotion")]
    public class AI_HandleParryExecutionRootMotion : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.ApplyParryExecutionRootMotion();
        }

        public override void Execute(StateManager states)
        {
        }
    }
}