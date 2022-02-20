using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Handle Player Agent Foot Step")]
    public class HandlePlayerAgentFootStep : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.HandlePlayerAgentFootStep();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}