using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_IsInteractingTick")]
    public class AI_IsInteractingTick : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.IsInteracting_Tick();
        }

        public override void Execute(StateManager states)
        {
            throw new System.NotImplementedException();
        }
    }
}