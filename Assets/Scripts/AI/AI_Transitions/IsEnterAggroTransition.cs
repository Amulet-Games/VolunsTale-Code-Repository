using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Is Enter Aggro Transition")]
    public class IsEnterAggroTransition : Condition
    {
        [SerializeField]
        float aggroTransitRate = 1f;

        [SerializeField]
        float aggroTransitAngle = 5;

        public override bool CheckAICondition(AIStateManager aiStates)
        {
            return aiStates.aiManager.IsEnterAggroTransition(aggroTransitRate, aggroTransitAngle);
        }

        public override bool CheckCondition(StateManager state)
        {
            throw new System.NotImplementedException();
        }
    }
}