using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ReacquireSecondThrowablePassiveAction

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Passive Actions/Reacquire Second Throwable Passive Action")]
    public class ReacquireSecondThrowable_PA : AIPassiveAction
    {
        public override void Execute(AIManager ai)
        {
            ai.ReacquireSecondThrowableWithAnim();
        }
    }
}
