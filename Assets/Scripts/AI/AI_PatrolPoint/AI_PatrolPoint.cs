using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_PatrolPoint : MonoBehaviour
    {
        public int patrolPointId;
        public float onPointSearchWaitRate;
        [ReadOnlyInspector] public Vector3 _localPosition;

        public void SetPatrolPointPosition()
        {
            _localPosition = transform.localPosition;
        }
    }
}