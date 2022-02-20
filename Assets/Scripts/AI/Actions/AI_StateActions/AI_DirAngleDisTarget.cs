using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_DirAngleDis2Target")]
    public class AI_DirAngleDisTarget : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.UpdateDirAngleDisTarget();
        }

        public override void Execute(StateManager states)
        {
        }
    }
}