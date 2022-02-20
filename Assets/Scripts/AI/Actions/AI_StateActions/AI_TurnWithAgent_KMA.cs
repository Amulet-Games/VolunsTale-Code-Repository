using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_TurnWithAgent_KMA")]
    public class AI_TurnWithAgent_KMA : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.TurnWithAgent_KMA();
        }

        public override void Execute(StateManager states)
        {
        }
    }
}