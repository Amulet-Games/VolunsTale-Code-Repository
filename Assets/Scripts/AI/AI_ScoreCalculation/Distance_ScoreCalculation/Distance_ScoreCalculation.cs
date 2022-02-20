using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/Distance Score Calculation")]
    public class Distance_ScoreCalculation : AI_ScoreFactor
    {
        [Header("Distance")]
        public float desiredDistance;

        public override int Calculate(AIManager ai)
        {
            float t = 0;

            float disToPlayer = ai.distanceToTarget <= 1 ? 1 : ai.distanceToTarget;

            if(disToPlayer > desiredDistance)
            {
                t = Mathf.Clamp01(desiredDistance / ai.distanceToTarget);
            }
            else
            {
                t = Mathf.Clamp01(ai.distanceToTarget / desiredDistance);
            }

            return Mathf.RoundToInt(m_ReturnScore * t);
        }
    }
}