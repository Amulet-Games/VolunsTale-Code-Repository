using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Handle Neglect Action Inputs")]
    public class HandleNeglectActionInputs : StateActions
    {
        public override void Execute(StateManager _states)
        {
            _states.HandleNeglectActionInputs();
        }
        
        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}