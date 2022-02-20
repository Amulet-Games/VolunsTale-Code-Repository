using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class WeaponAction : ScriptableObject, INeglectInputAction
    {
        [Header("Base Config.")]
        public AnimStateVariable targetAnimState;

        public abstract void Execute(StateManager _states);
    }
}