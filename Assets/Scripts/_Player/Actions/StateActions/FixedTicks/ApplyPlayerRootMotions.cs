using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/FixedTicks/ApplyPlayerRootMotions")]
    public class ApplyPlayerRootMotions : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.ApplyRootMotions();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}