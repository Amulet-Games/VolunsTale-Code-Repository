using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Mono Actions/Handle Locomotion Stats")]
    public class HandleLocomotionStats : MonoAction
    {
        public override void Execute(StateManager states)
        {
            states.HandleLocomotionStats();
        }
    }
}