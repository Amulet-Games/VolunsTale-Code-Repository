using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class ItemActions : ScriptableObject
    {
        // ItemActions responsible for Changing Current States and do whatever things are in Execute();
        
        public bool actionIsLeft;
        public float attackActionDamageOutput;

        public abstract void Execute(StateManager states);
    }
}
