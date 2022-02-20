using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Anim With Player While Neglect")]
    public class AnimWithPlayerWhileNeglect : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.HandleNeglectStateMoveAnim();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}