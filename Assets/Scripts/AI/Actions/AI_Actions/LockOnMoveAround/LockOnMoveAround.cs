using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/LockOn MoveAround")]
    public class LockOnMoveAround : AIAction
    {
        public override void Execute(AIManager ai)
        {
            ai.SetNewLockonPositionToAgent();
        }
    }
}