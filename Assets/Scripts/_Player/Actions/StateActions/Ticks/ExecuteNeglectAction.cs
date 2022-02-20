using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Execute Neglect Action")]
    public class ExecuteNeglectAction : StateActions
    {
        public override void Execute(StateManager _states)
        {
            _states.ExecuteNeglectAction();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}