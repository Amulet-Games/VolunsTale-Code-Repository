using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/RandomNumber Score Calculation")]
    public class RandomNumber_ScoreCalculation : AI_ScoreFactor 
    {
        [Header("Percentage")]
        [Tooltip("The chances in percentage of this score factor will returns the \"ReturnScore\" beside 0.")]
        public int odds;

        [Header("Distance")]
        [Tooltip("The minimum range that only if enemy's \"disToPlayer\" is bigger than this value then it will returns \"ReturnScore\" .")]
        public float minRange;
        [Tooltip("The maximum range that only if enemy's \"disToPlayer\" is smaller than this value then it will returns \"ReturnScore\" .")]
        public float maxRange;

        private void OnEnable()
        {
            Notes = "Get a int from Random.Range and compare it to the odd, if it's value is lower or equal to the odd then returns return score. Otherwise returns 0." +
                    " It also Returns 0 If enemy's distance to player is exceeded outside from min and max range.";
        }

        public override int Calculate(AIManager ai)
        {
            if (ai.distanceToTarget > minRange && ai.distanceToTarget < maxRange)
            {
                if (Random.Range(1, 100f) <= odds)
                {
                    return m_ReturnScore;
                }
            }

            return 0;
        }
    }
}