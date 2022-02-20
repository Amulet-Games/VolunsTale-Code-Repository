using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_TurnWithAgent")]
    public class AI_TurnWithAgent : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.TurnWithAgent();
        }
        
        public override void Execute(StateManager states)
        {
        }
    }
}