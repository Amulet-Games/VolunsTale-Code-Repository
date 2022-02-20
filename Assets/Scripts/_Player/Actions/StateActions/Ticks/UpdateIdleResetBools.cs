using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Tick Reset Bools")]
    public class UpdateIdleResetBools : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.UpdateIdleStateResetBools();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
            throw new System.NotImplementedException();
        }
    }
}