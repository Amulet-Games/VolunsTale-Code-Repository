using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "INeglect Input Action/Roll Action")]
    public class RollAction : ScriptableObject, INeglectInputAction
    {
        public void Execute(StateManager _states)
        {
            _states.RollAction();
        }
    }
}