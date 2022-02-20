using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/Specific Distance Score Calculation")]
    public class SpecificDistance_ScoreCalculation : AI_ScoreFactor
    {
        // Different between normal distance score Calculation and this is,
        // This one return the highest score when distance is equal to specific distance.
        // For example if the specific distance is 7, distance higher or lower than 7 will decrease the score.
        // if distance is 0 score will be 0, if distance is 14 score will be 0 too

        [Header("Return Score")]
        public int startScore;
        public int endScore;

        [Header("Specific Distance")]
        public int specificDistance;
        public int distanceThersholdLimit;  // if distance between player and this enemy distance is higher or equal than this, function will return 

        public override int Calculate(AIManager ai)
        {
            if (ai.distanceToTarget > distanceThersholdLimit)
                return 0;

            float reVal = 0;
            float t = (ai.distanceToTarget - specificDistance) / specificDistance;
            if (ai.distanceToTarget > specificDistance)
                reVal = startScore + (endScore - startScore) * t;
            else
                reVal = startScore - (endScore - startScore) * t;

            return Mathf.RoundToInt(reVal);
        }
    }
}