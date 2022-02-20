using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Profile/Lockon Move Around Profile")]
    public class AI_LockonMoveProfile : ScriptableObject
    {
        [Header("Pivot")]
        [Tooltip("Check this to calculate a new position each time base on player position, default is using enemy current position.")]
        public bool isMoveAroundPlayer;

        [Header("Angles")]
        [Tooltip("The range of degrees that this action will calculate it's new position based upon.")]
        public MoveAroundDegreeTypeEnum targetDegreeType;

        [Header("Distance")]
        [Tooltip("The range of distances that this action will calculate it's new position based upon.")]
        public float minRange = 0.5f;
        public float maxRange = 2.5f;
        
        public enum MoveAroundDegreeTypeEnum
        {
            front_180,
            left_180,
            right_180,
            back_180,
            whole_360,
            front_90,
            left_90,
            right_90,
        }

        public float GetRandomDegreeFromType()
        {
            float retVal = 0;

            switch (targetDegreeType)
            {
                case MoveAroundDegreeTypeEnum.front_180:
                    retVal = Random.Range(-90f, 90f);
                    break;

                case MoveAroundDegreeTypeEnum.left_180:
                    retVal = Random.Range(0f, -180f);
                    break;

                case MoveAroundDegreeTypeEnum.right_180:
                    retVal = Random.Range(0f, 180f);
                    break;

                case MoveAroundDegreeTypeEnum.back_180:
                    if (Random.Range(1, 3) > 1)
                    {
                        retVal = Random.Range(90f, 180f);
                    }
                    else
                    {
                        retVal = Random.Range(-90f, -180f);
                    }
                    break;

                case MoveAroundDegreeTypeEnum.whole_360:
                    retVal = Random.Range(-180f, 180f);
                    break;

                case MoveAroundDegreeTypeEnum.front_90:
                    retVal = Random.Range(-45f, 45f);
                    break;

                case MoveAroundDegreeTypeEnum.left_90:
                    retVal = Random.Range(-45f, -135f);
                    break;

                case MoveAroundDegreeTypeEnum.right_90:
                    retVal = Random.Range(45f, 135f);
                    break;

                default:
                    break;
            }

            return retVal;
        }

        public float GetRandomDistanceFromRange()
        {
            return Random.Range(minRange, maxRange);
        }
    }
}