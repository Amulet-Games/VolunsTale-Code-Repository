using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/HitCountActionReturnScore")]
    public class HitCountActionReturnScore : AI_ScoreFactor
    {
        private void OnEnable()
        {
            Notes = "Returns returnScore when enemy \"isHitCountEventTriggered\" bool is false.\nOtherwise returns 0.";
        }

        public override int Calculate(AIManager ai)
        {
            if(ai.GetIsHitCountEventTriggeredBool())
            {
                return m_ReturnScore;
            }

            return 0;
        }
    }
}