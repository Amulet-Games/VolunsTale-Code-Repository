using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_PatrolRoute : MonoBehaviour
    { 
        public int patrolListId;
        public AI_PatrolPoint[] patrolPoints;

        public void SetupPatrolRoute()
        {
            if (patrolPoints.Length > 1)
            {
                for (int i = 0; i < patrolPoints.Length; i++)
                {
                    patrolPoints[i].SetPatrolPointPosition();
                }
            }
            else
            {
                patrolPoints[0].SetPatrolPointPosition();
            }
        }

        public AI_PatrolPoint GetClosetPatrolPoint(AIStateManager _aiStates)
        {
            float _dis = 100000;
            float _tempDis = 0;
            AI_PatrolPoint _tempPoint = null;

            for (int i = 0; i < patrolPoints.Length; i++)
            {
                _tempDis = Vector3.SqrMagnitude(patrolPoints[i]._localPosition - _aiStates.mTransform.position);
                if (_tempDis < _dis)
                {
                    _dis = _tempDis;
                    _tempPoint = patrolPoints[i];
                }
            }

            return _tempPoint;
        }
    }
}