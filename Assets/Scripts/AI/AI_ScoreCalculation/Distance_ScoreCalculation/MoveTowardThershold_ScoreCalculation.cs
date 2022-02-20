using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/MoveToward Thershold Score Calculation")]
    public class MoveTowardThershold_ScoreCalculation : AI_ScoreFactor
    {
        [Header("Distance")]
        public float desiredDistance;

        private void OnEnable()
        {
            Notes = "This is same as distance Thershold score except the thershold is agent stopping distance.";
        }

        public override int Calculate(AIManager ai)
        {
            float t = 0;
            float disToPlayer = ai.distanceToTarget;

            if (disToPlayer < ai.agentStopDistance)
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