using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Player Anim With Agent")]
    public class Player_AnimWithAgent : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.AnimWithAgent();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}