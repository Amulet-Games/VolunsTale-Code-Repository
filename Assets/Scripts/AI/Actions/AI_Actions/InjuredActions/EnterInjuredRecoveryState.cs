using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Enter Injured Recovery State.")]
    public class EnterInjuredRecoveryState : AIAction
    {
        public override void Execute(AIManager ai)
        {
            ai.EgilEnterInjuredState();
        }
    }
}