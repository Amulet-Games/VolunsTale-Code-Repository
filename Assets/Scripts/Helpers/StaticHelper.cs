using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public static class StaticHelper
    {
        public static float FloatLerp(float starting_value,float ending_value,float t)
        {
            Mathf.Clamp01(t);
            return starting_value + (ending_value - starting_value) * t;
        }

        public static Vector3 NormalizedVector3(Vector3 targetVector3)
        {
            float inversedMagnitude = InvSqrt(targetVector3.sqrMagnitude);
            return targetVector3 * inversedMagnitude;
        }

        // CONVERT A ANGLE DEGREE TO VECTOR3 
        public static Vector3 GetDirFromAngle(float angleInDegrees)
        {
            //float range = Random.Range(minRange, maxRange);
            Vector3 calculatedPos = new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
            return calculatedPos;
        }

        public static Vector3 GetDirFromAngle(Transform objectTransform, float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
                angleInDegrees += objectTransform.eulerAngles.y;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        // CALCULATE A NEW VECTOR2 AFTER CERTAIN DEGREE OF ROTATION
        public static Vector2 GetNewRotatedVector2(Vector2 oldDir, int degree)
        {
            return new Vector2(
            oldDir.x * Mathf.Cos(degree * Mathf.Deg2Rad) - oldDir.y * Mathf.Sin(degree * Mathf.Deg2Rad),
            oldDir.x * Mathf.Sin(degree * Mathf.Deg2Rad) + oldDir.y * Mathf.Cos(degree * Mathf.Deg2Rad));
        }

        // CALCULATE A NEW VECTOR3 AFTER CERTAIN DEGREE OF ROTATION AROUND Y AXIS
        public static Vector3 GetNewRotatedVector3(Vector3 oldDir, float degree)
        {
            return new Vector3(
                oldDir.x * Mathf.Cos(degree * Mathf.Deg2Rad) + oldDir.z * Mathf.Sin(degree * Mathf.Deg2Rad),
                oldDir.y,
                -oldDir.x * Mathf.Sin(degree * Mathf.Deg2Rad) + oldDir.z * Mathf.Cos(degree * Mathf.Deg2Rad)
                );
        }

        private static float InvSqrt(float number)
        {
            unsafe
            {
                float y = number;

                long i = *(long *) &y;
                i = (i >> 1);
                i = 0x5f3759df - i;

                y = *(float *) &i;

                return y = y * (1.5F - (0.5f * number * y * y));
            }
        }
    }
}