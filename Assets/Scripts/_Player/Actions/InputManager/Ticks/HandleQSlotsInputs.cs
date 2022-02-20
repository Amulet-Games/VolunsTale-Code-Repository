using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Mono Actions/Handle QSlots Inputs")]
    public class HandleQSlotsInputs : MonoAction
    {
        public override void Execute(StateManager _states)
        {
            _states.HandleQSlotsInputs();
        }
    }
}