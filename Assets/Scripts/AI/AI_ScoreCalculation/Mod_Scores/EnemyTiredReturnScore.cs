using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/Enemy Tired Return Score")]
    public class EnemyTiredReturnScore : AI_ScoreFactor
    {
        private void OnEnable()
        {
            Notes = "Returns returnScore when enemy \"isTired\" bool is false.\nOtherwise returns 0.";
        }

        public override int Calculate(AIManager ai)
        {
            if(ai.GetIsEnemyTiredBool())
            {
                return m_ReturnScore;
            }

            return 0;
        }
        
    }
}