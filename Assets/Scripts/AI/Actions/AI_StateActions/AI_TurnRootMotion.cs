using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/FixedTicks/AI_TurnRootMotion")]
    public class AI_TurnRootMotion : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.ApplyTurnRootMotion();
        }
        
        public override void Execute(StateManager states)
        {
            throw new System.NotImplementedException();
        }
    }
}