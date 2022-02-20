using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/ForceScore_ScoreCalculation")]
    public class ForceScore_ScoreCalculation : AI_ScoreFactor
    {
        public override int Calculate(AIManager ai)
        {
            return m_ReturnScore;
        }

    }
}