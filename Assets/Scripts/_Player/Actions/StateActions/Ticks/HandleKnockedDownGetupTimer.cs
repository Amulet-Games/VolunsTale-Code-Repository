using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Handle Knocked Down Getup Timer")]
    public class HandleKnockedDownGetupTimer : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.HandleGetupTimeCounter();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}