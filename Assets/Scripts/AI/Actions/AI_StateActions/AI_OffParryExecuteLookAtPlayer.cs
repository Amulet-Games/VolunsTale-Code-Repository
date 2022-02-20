using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_OffParryExecuteLookAtPlayer")]
    public class AI_OffParryExecuteLookAtPlayer : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.OffParryExecuteLookAtPlayer();
        }

        public override void Execute(StateManager states)
        {
        }
    }
}