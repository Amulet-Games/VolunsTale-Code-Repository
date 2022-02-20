using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/Angle_ScoreCalculation")]
    public class Angle_ScoreCalculation : AI_ScoreFactor
    {
        [Header("Angles")]
        [SerializeField]
        [Tooltip("This option determines how much angle(degree) from enemy to player you want in order to perform the represent AI Action, Note that only \"45\" option is able to cover all the angles(360 degrees), others will have a gap from different directions.")]
        AngleOptionsTypeEnum angleOptions = AngleOptionsTypeEnum.normal_30;

        [Header("Directions")]
        [SerializeField]
        [Tooltip("This option determines which direction would angle options calculation be based from.")]
        DirectionOptionsTypeEnum directionOptions = DirectionOptionsTypeEnum.front;

        [Header("Distance Thershold")]
        [Tooltip("AI's \"disToPlayer\" value should be lower or equal to this value")]
        public float validDistance;

        private void OnEnable()
        {
            Notes = "Returns return score when AI's \"disToPlayer\" is lower or equal to validDistance and \"angleToPlayer\" is inside the minAngle and maxAngle range.";
        }

        public override int Calculate(AIManager ai)
        {
            int retVal = 0;

            if (ai.distanceToTarget <= validDistance)
            {
                if (directionOptions == DirectionOptionsTypeEnum.whole360)
                {
                    retVal = m_ReturnScore;
                }
                else if (ai.CheckTargetInAngle(directionOptions, angleOptions))
                {
                    retVal = m_ReturnScore;
                }
            }

            return retVal;
        }
    }
}