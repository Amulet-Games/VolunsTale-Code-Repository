using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Calculate Front Step Height")]
    public class CalculateFrontStepHeight : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.CalculateFrontHeight();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
            throw new System.NotImplementedException();
        }
    }
}