using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class EP_AI_PassiveAction : AIPassiveAction
    {
        public abstract void Init(AIManager _ai);
    }
}