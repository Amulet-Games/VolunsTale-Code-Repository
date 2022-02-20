using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_UpdateExecutionGetupWaitTime")]
    public class AI_UpdateExecutionGetupWaitTime : StateActions
    {
        public override void AIExecute(AIStateManager aiState)
        {
            aiState.aiManager.ExecutionGetupWaitTimeCount();
        }

        public override void Execute(StateManager states)
        {
        }
    }
}