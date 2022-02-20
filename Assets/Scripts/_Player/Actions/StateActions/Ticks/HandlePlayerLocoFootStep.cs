using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Handle Player Loco Foot Step")]
    public class HandlePlayerLocoFootStep : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.HandlePlayerLocoFootStep();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}