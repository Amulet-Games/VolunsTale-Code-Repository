using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/Distance Thershold Score Calculation")]
    public class DistanceThershold_ScoreCalculation : AI_ScoreFactor
    {
        [Header("Distance")]
        public float desiredDistance;
        public float distanceThershold;

        private void OnEnable()
        {
            Notes = "This is same as distance score except it won't return any score if AI's disToPlayer is less than thershold distance.";
        }

        public override int Calculate(AIManager ai)
        {
            float t = 0;
            float disToPlayer = ai.distanceToTarget;

            if (disToPlayer < distanceThershold)
                return 0;

            if (disToPlayer > desiredDistance)
            {
                t = Mathf.Clamp01(desiredDistance / disToPlayer);
            }
            else
            {
                t = Mathf.Clamp01(disToPlayer / desiredDistance);
            }

            return Mathf.RoundToInt(m_ReturnScore * t);
        }
    }
}