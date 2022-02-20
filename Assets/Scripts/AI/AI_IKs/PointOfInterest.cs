using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI IKs/Point Of Interest")]
    public class PointOfInterest : ScriptableObject
    {
        public LookAtEventTypeEnum lookAtEventType;

        [Range(0, 1)]
        public float lookAtEventWeight;

        [ReadOnlyInspector]
        public Transform pointToLookAt;

        public enum LookAtEventTypeEnum
        {
            LookAtCurrentTarget
        }
    }
}