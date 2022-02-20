using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Monitor IsGrounded")]
    public class MonitorIsGrounded : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.CheckIsGrounded();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}