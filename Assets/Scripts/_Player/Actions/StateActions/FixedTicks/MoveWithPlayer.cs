using System;
using UnityEngine;
using System.Collections;

namespace SA
{
	[CreateAssetMenu(menuName = "State Actions/Player/FixedTicks/Move With Player")]
	public class MoveWithPlayer : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.MoveWithPlayerIdle();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
            throw new System.NotImplementedException();
        }
    }
}