using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class BaseLocoIKState : ScriptableObject
    {
        public abstract void Tick(StateManager _states);
        
        public abstract void OnRunningIK(StateManager _states);

        public abstract void OffRunningIK(StateManager _states);

        public abstract void OnTwoHanding(StateManager _states);

        public abstract void OffTwoHanding(StateManager _states);

        public abstract void OnDefense(StateManager _states);

        public abstract void OffDefense(StateManager _states);
        
        public abstract void OnLockon(StateManager _states);

        public abstract void OffLockon(StateManager _states);
    }
}