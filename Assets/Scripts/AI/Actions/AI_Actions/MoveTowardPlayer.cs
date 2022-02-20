using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Move Toward Player")]
    public class MoveTowardPlayer : AIAction
    {
        public override void Execute(AIManager ai)
        {
            ai.MoveTowardPlayer();
        }
    }
}