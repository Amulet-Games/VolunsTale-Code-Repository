using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/FixedTicks/Turn With Player While Neglect")]
    public class TurnWithPlayerWhileNeglect : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.TurnWithPlayerWhileNeglect();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
            throw new System.NotImplementedException();
        }

    }
}