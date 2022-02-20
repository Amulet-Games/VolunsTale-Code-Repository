using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/Obstacles Check Score Calculation")]
    public class ObstaclesCheck_ScoreCalculation : AI_ScoreFactor
    {
        [Header("Check Interval, % frame Count.")]
        public int _checkRateInterval = 5;

        [Header("Obstacles Layer Mask.")]
        public LayerMask _obstacleCheck_ScoreCalculationMask;

        private void OnEnable()
        {
            Notes = "Return \"ReturnScore\" when Ray Not Hit Obstacles Objects," +
                    "Mainly Walkable, UnWalkable or UnStepable.";
        }

        public override int Calculate(AIManager ai)
        {
            if (ai._frameCount % _checkRateInterval == 0)
            {
                Vector3 _dir = (ai.playerStates.mTransform.position + ai.vector3Up) - ai.mTransform.position;
                float _dis = _dir.magnitude;

                //Debug.DrawRay(ai._mTransform.position, _dir, Color.black);
                if (Physics.Raycast(ai.mTransform.position, _dir, _dis, _obstacleCheck_ScoreCalculationMask))
                {
                    return 0;
                }
                else
                {
                    return m_ReturnScore;
                }
            }

            return 0;
        }
    }
}