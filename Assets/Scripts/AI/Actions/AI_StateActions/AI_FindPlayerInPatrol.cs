using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_FindPlayerInPatrol")]
    public class AI_FindPlayerInPatrol : StateActions
    {
        public override void AIExecute(AIStateManager aiState)
        {
            aiState.aiManager.FindPlayerInPatrol();
        }
        
        public override void Execute(StateManager states)
        {
        }
    }
}