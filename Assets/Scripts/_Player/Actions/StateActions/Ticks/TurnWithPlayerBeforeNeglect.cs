using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Turn With Player Before Neglect")]
    public class TurnWithPlayerBeforeNeglect : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.TurnWithPlayerBeforeNeglect();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
            throw new System.NotImplementedException();
        }

    }
}