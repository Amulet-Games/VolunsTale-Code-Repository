using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_MoveWithAgent")]
    public class AI_MoveWithAgent : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.MoveWithAgent();
        }

        public override void Execute(StateManager states)
        {
            //throw new System.NotImplementedException();
        }
    }
}

