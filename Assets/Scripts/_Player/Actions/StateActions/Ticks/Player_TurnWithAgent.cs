using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Player Turn With Agent")]
    public class Player_TurnWithAgent : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.TurnWithAgent();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}