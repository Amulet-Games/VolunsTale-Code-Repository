using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Update Neglect Action")]
    public class UpdateNeglectAction : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.UpdateActionInNeglectState();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
            throw new System.NotImplementedException();
        }
    }
}