using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_DirAngleDisPatrol")]
    public class AI_DirAngleDisPlayer : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.UpdateDirAngleDisPlayer();
        }

        public override void Execute(StateManager states)
        {
        }
    }
}