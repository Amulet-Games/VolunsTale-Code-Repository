using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_MonitorMultiStageAttacks")]
    public class AI_MonitorMultiStageAttacks : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.MonitorMultiStageAttacks();
        }

        public override void Execute(StateManager state)
        {
            // throw new System.NotImplementedException();
        }
    }
}