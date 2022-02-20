using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/IK With Player")]
    public class IKWithPlayer : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.LocoIKStateTick();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}