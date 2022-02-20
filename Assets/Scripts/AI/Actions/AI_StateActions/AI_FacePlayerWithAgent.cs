using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_FacePlayerWithAgent")]
    public class AI_FacePlayerWithAgent : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.FacePlayerWithAgent();
        }

        public override void Execute(StateManager states)
        {
        }
    }
}