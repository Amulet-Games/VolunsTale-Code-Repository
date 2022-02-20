using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Anim With Player")]
    public class AnimWithPlayer : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.SetPlayerAnim();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}