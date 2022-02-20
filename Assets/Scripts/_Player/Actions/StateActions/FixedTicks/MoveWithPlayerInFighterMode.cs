using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/FixedTicks/MoveWithPlayerInFighterMode")]
    public class MoveWithPlayerInFighterMode : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.MoveWithPlayerInFighterMode();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}