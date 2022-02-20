using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/TwoPointDistance_ScoreCalculation")]
    public class TwoPointDistance_ScoreCalculation : AI_ScoreFactor
    {
        [Header("Desire Distance Range")]
        public float maxmiumRange;
        public float minimumRange;

        private void OnEnable()
        {
            Notes = "Return returnScores when the distance between AI to Target is within certain distance range.";
        }

        public override int Calculate(AIManager aiManager)
        {
            if (aiManager.distanceToTarget < maxmiumRange && aiManager.distanceToTarget > minimumRange)
            {
                return m_ReturnScore;
            }

            return 0;
        }
    }
}