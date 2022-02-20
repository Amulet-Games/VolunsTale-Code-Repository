using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_OverlapBoxDamageColliderTest")]
    public class AI_OverlapBoxDamageColliderTest : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiManager.MonitorOverlapBoxDamageCollider();
        }

        public override void Execute(StateManager states)
        {
        }
    }
}