using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_PatrolWithAgent")]
    public class AI_PatrolWithAgent : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.PatrolWithAgent();
        }

        public override void Execute(StateManager states)
        {
            throw new System.NotImplementedException();
        }
    }
}