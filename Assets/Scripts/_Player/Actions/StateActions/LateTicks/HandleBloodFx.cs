using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/LateTicks/Handle Blood Fx")]
    public class HandleBloodFx : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.HandleBloodFxTick();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}