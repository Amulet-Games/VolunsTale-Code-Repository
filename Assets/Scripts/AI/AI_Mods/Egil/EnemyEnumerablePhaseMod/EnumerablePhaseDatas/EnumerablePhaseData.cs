using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class EnumerablePhaseData : ScriptableObject
    {
        public abstract void SetNewPhaseData(AIManager _ai);
    }
}