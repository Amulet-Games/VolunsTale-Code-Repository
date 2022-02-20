using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Handle Non Neglect Action Inputs")]
    public class HandleNonNeglectActionInputs : StateActions
    {
        [NonSerialized] float _twoHandSwitchTimer;

        public override void Execute(StateManager _states)
        {
            _states.HandleNonNeglectActionInputs();
        }
        
        public override void AIExecute(AIStateManager aiStates)
        {
            throw new System.NotImplementedException();
        }
    }
}