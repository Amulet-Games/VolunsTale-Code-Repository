using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI Injured Tick Hook")]
    public class AI_Injured_TickHook : StateActions
    {
        public override void AIExecute(AIStateManager aiState)
        {
            aiState.aiManager.InjuredStateTick();
        }

        public override void Execute(StateManager states)
        {
        }
    }
}