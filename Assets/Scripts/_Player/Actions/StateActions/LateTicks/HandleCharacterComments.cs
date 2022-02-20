using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/LateTicks/Handle Character Comments")]
    public class HandleCharacterComments : StateActions
    {
        public override void Execute(StateManager states)
        {
            states._commentHandler.LateTick();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}