using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/FixedTicks/AI_TurnWithAgent_FixedTick")]
    public class AI_TurnWithAgent_FixedTick : AI_TurnWithAgent
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.TurningWhileIdleWithIK();
        }
    }
}