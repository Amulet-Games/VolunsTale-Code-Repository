using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class MonoAction : ScriptableObject
    {
        public abstract void Execute(StateManager states);
    }
}
