using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Handle Roll When Control By Agent.")]
    public class HandleRollWhenControlByAgent : StateActions
    {
        public override void Execute(StateManager _states)
        {
            _states.HandleRollWhenControlByAgent();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}