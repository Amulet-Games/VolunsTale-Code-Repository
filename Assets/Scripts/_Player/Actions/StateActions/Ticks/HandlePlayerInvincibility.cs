using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Handle Player Invincibility")]
    public class HandlePlayerInvincibility : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.HandlePlayerInvincibility();
        }

        public override void AIExecute(AIStateManager aIState)
        {
        }
    }
}