using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Calculate Front Step Height While Neglect")]
    public class CalculateFrontStepHeightWhileNeglect : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.CalculateFrontHeightWhileNeglect();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
            throw new System.NotImplementedException();
        }
    }
}