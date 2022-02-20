using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Player Move With Agent")]
    public class Player_MoveWithAgent : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.MoveWithAgent();
        }

        public override void AIExecute(AIStateManager aIState)
        {
        }
    }
}