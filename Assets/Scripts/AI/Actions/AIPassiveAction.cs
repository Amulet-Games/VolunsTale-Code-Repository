using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class AIPassiveAction : ScriptableObject
    {
        public abstract void Execute(AIManager ai);
    }
}