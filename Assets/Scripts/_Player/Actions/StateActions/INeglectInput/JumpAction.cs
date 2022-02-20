using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "INeglect Input Action/Jump Action")]
    public class JumpAction : ScriptableObject, INeglectInputAction
    {
        public void Execute(StateManager _states)
        {
            _states.JumpAction();
        }
    }
}