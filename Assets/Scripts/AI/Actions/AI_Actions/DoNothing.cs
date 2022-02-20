using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Do Nothing")]
    public class DoNothing : AIAction
    {
        public override void Execute(AIManager ai)
        {
        }
    }
}