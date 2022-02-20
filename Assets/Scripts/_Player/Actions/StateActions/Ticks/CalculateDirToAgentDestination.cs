using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Calculate Dir To Agent Destination")]
    public class CalculateDirToAgentDestination : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.CalculateDirDisToDestination();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}