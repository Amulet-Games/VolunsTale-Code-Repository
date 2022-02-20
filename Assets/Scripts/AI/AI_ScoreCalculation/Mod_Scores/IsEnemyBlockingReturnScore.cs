using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/IsEnemyBlockingReturnScore")]
    public class IsEnemyBlockingReturnScore : AI_ScoreFactor
    {
        private void OnEnable()
        {
            Notes = "Returns return score when EnemyBlockingMod \"isEnemyBlocking\" is true.";
        }

        public override int Calculate(AIManager ai)
        {
            if (ai.GetIsEnemyBlockingBool())
            {
                return m_ReturnScore;
            }

            return 0;
        }
    }
}