using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_AnimWithAgent")]
    public class AI_AnimWithAgent : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.AnimWithAgent();
        }

        public override void Execute(StateManager states)
        {
        }
    }
}