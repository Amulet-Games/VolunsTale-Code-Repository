using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Execute Searched Interactable")]
    public class ExecuteSearchedInteractable : AIAction
    {
        public override void Execute(AIManager ai)
        {
            ai.ExecuteInteractable();
        }
    }
}