using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_PermitteParryExecution")]
    public class AI_PermitParryExecution : StateActions
    {
        [Tooltip("The distance that player need to be within in order to perform execution.")]
        public float _parryExecutePermitRange;

        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.PermitParryExecution(_parryExecutePermitRange);
        }

        public override void Execute(StateManager states)
        {
        }
    }
}