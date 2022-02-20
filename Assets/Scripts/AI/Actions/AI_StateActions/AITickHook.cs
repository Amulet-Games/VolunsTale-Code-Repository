using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI Tick Hook")]
    public class AITickHook : StateActions
    {
        public override void AIExecute(AIStateManager aiState)
        {
            aiState.aiManager.Tick();
        }

        public override void Execute(StateManager states)
        {
        }
    }
}