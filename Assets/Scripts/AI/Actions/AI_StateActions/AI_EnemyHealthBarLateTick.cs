using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/LateTicks/AI_EnemyHealthBarLateTick")]
    public class AI_EnemyHealthBarLateTick : StateActions
    {
        public override void AIExecute(AIStateManager aiStates)
        {
            aiStates.aiDisplayManager.LateTick();
        }

        public override void Execute(StateManager states)
        {
            throw new System.NotImplementedException();
        }
    }
}