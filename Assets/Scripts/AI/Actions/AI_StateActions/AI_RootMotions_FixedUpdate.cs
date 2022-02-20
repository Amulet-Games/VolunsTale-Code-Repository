using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/FixedTicks/AI_RootMotions_FixedUpdate")]
    public class AI_RootMotions_FixedUpdate : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.AI_HandleRootMotions_FixedUpdate();
        }

        public override void Execute(StateManager states)
        {
        }
    }
}