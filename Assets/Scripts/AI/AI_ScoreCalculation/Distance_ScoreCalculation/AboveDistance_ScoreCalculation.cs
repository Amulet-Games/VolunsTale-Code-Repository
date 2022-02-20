using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/Above Distance Score Calculation")]
    public class AboveDistance_ScoreCalculation : AI_ScoreFactor
    {
        // As the title suggest, this one will only return m_Return Score
        // when enemy and player distance is above specified value.

        [Header("Distance to be above.")]
        public float specificDistance;

        public override int Calculate(AIManager ai)
        {
            if (ai.distanceToTarget > specificDistance)
            {
                return m_ReturnScore;
            }
            else
            {
                return 0;
            }
        }
    }
}