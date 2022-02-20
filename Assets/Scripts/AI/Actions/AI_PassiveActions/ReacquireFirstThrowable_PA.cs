using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Passive Actions/Reacquire First Throwable Passive Action")]
    public class ReacquireFirstThrowable_PA : AIPassiveAction
    {
        public override void Execute(AIManager ai)
        {
            ai.ReacquireFirstThrowableWithAnim();
        }
    }
}