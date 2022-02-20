using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class EnemyIKHandler : MonoBehaviour
    {
        [Header("RetargetSpeed")]
        public float headRigRetargetSpeed;
        public float bodyRigRetargetSpeed;
        [ReadOnlyInspector] public float currentBodyRigRetargetSpeed;
        public float maxBodyRigSpeedDistance = 1;
        public float minBodyRigSpeedDistance = 8;
        public float iKTargetRetargetSpeed;
        public float manuverIKRetargetSpeed = 3.5f;
        public float lookAtWeightLerpSpeed;
        public float _iKHelperPosAddOn = 0;

        float tarLookAtWeight;

        [Header("Thershold")]
        [Tooltip("The distance limit of how far will IK Target Stop following current target.")]
        public float iKRetargetStoppingDistance = 2f;

        [Tooltip("The distance of how far IK Target will reposition itself if IK Target stopped following current target.")]
        public float iKTargetRepositionDistance = 4f;

        [Tooltip("The height of how high IK Target will reposition itself if IK Target stopped following current target. Default value is 0.5f higher than next IK Target Position.")]
        public float iKTargetRepositionHeight = 1;

        [Header("Weight")]
        [ReadOnlyInspector] public float headRigWeight;
        [ReadOnlyInspector] public float bodyRigWeight;
        [SerializeField, ReadOnlyInspector] float curLookAtWeight;
        
        [Header("Bools")]
        [ReadOnlyInspector] public bool isUsingIK;
        [ReadOnlyInspector] public bool isEnemyForwardIK;
        [ReadOnlyInspector] public bool isManuverIK;
        [ReadOnlyInspector] public bool isIKRetargetStopped;

        public float applyLerpAfterStoppedRate;
        [ReadOnlyInspector] public float applyLerpAfterStoppedTimer;

        [Header("Refs")]
        [ReadOnlyInspector] public AIStateManager aIStates;
        [ReadOnlyInspector] public AIManager _ai;
        [ReadOnlyInspector] public Transform iKTarget;
        [ReadOnlyInspector] public Transform aIHeadTrans;

        public void Init(AIStateManager aiStates)
        {
            aIStates = aiStates;

            _ai = aIStates.aiManager;
            _ai.iKHandler = this;

            iKTarget = new GameObject().transform;
            iKTarget.name = "IKTarget";

            Transform aiStatesTrans = aiStates.mTransform;
            iKTarget.parent = aiStatesTrans;
            iKTarget.position = aiStatesTrans.position + (aiStatesTrans.forward * aiStates.aiManager.aggro_Thershold) + Vector3.up;
            iKTarget.rotation = Quaternion.identity;

            aIHeadTrans = aiStates.anim.GetBoneTransform(HumanBodyBones.Head);

            isUsingIK = true;
            isEnemyForwardIK = true;
        }

        public void Tick()
        {
            //Debug.Log("Tick");
            TurnOnHeadRigIK();

            SetIsIKRetargetStoppedStatus();
            
            _ai.anim.SetLookAtPosition(iKTarget.position);

            UpdateLookAtWeight();
            _ai.anim.SetLookAtWeight(curLookAtWeight, bodyRigWeight, headRigWeight, headRigWeight, 0.5f);
        }
        
        void SetIsIKRetargetStoppedStatus()
        {
            if (_ai.distanceToTarget <= iKRetargetStoppingDistance)
            {
                if (!isIKRetargetStopped)
                {
                    isIKRetargetStopped = true;
                    applyLerpAfterStoppedTimer = 0;
                }
                else
                {
                    UpdateStoppingDistanceIKPosition();
                }
            }
            else
            {
                if (isIKRetargetStopped)
                {
                    UpdateValidDistanceIKPosition();

                    applyLerpAfterStoppedTimer += aIStates._delta;
                    if (applyLerpAfterStoppedTimer >= applyLerpAfterStoppedRate)
                    {
                        isIKRetargetStopped = false;
                    }
                }
                else
                {
                    UpdateValidDistanceIKPosition();
                }
            }
        }

        void UpdateLookAtWeight()
        {
            if (isUsingIK)
            {
                tarLookAtWeight = 1;
            }
            else
            {
                tarLookAtWeight = 0;
            }

            if ((Mathf.Abs(tarLookAtWeight - curLookAtWeight) <= 0.025f) == false)
            {
                curLookAtWeight = Mathf.Lerp(curLookAtWeight, tarLookAtWeight, aIStates._delta * lookAtWeightLerpSpeed);
            }
            else
            {
                curLookAtWeight = tarLookAtWeight;
            }
        }

        #region Update IK Target Position.
        void UpdateStoppingDistanceIKPosition()
        {
            /* if the current target is too close to enemy,
             * IK Target should reposition itself to a little bit further back to current target. */
            iKTarget.position = Vector3.Lerp(iKTarget.position, GetStoppingDistanceIKPos(), aIStates._delta * iKTargetRetargetSpeed);
        }

        void UpdateValidDistanceIKPosition()
        {
            if (!isManuverIK)
            {
                iKTarget.position = Vector3.Lerp(iKTarget.position, GetValidDistanceIKPos(), aIStates._delta * iKTargetRetargetSpeed);
            }
            else
            {
                iKTarget.position = Vector3.Lerp(iKTarget.position, GetValidDistanceIKPos(), aIStates._delta * manuverIKRetargetSpeed);
            }
        }
        #endregion

        #region Get IK Target Pos.
        Vector3 GetValidDistanceIKPos()
        {
            /* if enemy is in patrol state, 
             * IK Target can just retarget back to enemy himself. */
            if (isEnemyForwardIK)
            {
                return GetNonAggroIKPos();
            }
            else
            {
                /* if enemy is close enough to player,
                 * IK Target should be following the enemy current target. */
                return GetCurrentTargetIKPos();
            }
        }

        Vector3 GetNonAggroIKPos()
        {
            return aIHeadTrans.position + aIHeadTrans.forward * 1.5f;
        }
        
        Vector3 GetCurrentTargetIKPos()
        {
            return _ai.targetPos + (_ai.vector3Up * _iKHelperPosAddOn);
        }

        Vector3 GetStoppingDistanceIKPos()
        {
            Vector3 nextIKTargetPos = _ai.vector3Zero;

            nextIKTargetPos = _ai.targetPos + _ai.vector3Up;
            nextIKTargetPos.y += 0.5f * iKTargetRepositionHeight;
            nextIKTargetPos = ((nextIKTargetPos - aIHeadTrans.position) / _ai.distanceToTarget) * iKTargetRepositionDistance;
            return nextIKTargetPos += aIHeadTrans.position;
        }
        #endregion

        #region On / Off Rig IK.
        public void TurnOnHeadRigIK()
        {
            if ((1 - headRigWeight) <= 0.025f)
            {
                headRigWeight = 1;
                
            }
            else
            {
                headRigWeight = Mathf.Lerp(headRigWeight, 1, aIStates._delta * headRigRetargetSpeed);
            }
        }

        public void TurnOffHeadRigIK()
        {
            if ((headRigWeight - 0) <= 0.025f)
            {
                headRigWeight = 0;
            }
            else
            {
                headRigWeight = Mathf.Lerp(headRigWeight, 0, aIStates._delta * headRigRetargetSpeed);
            }
        }

        public void TurnOffHeadRigIKWithTween()
        {
            LeanTween.value(headRigWeight, 0, 1f).setOnUpdate((value) => headRigRetargetSpeed = value);
        }

        public void TurnOnBodyRigIK()
        {
            if ((1 - bodyRigWeight) <= 0.025f)
            {
                bodyRigWeight = 1;
            }
            else
            {
                GetCurrentBodyRigRetargetSpeed();
                bodyRigWeight = Mathf.Lerp(bodyRigWeight, 1, aIStates._delta * currentBodyRigRetargetSpeed);
            }
        }

        public void TurnOffBodyRigIK()
        {
            if ((bodyRigWeight - 0) <= 0.025f)
            {
                bodyRigWeight = 0;
            }
            else
            {
                GetCurrentBodyRigRetargetSpeed();
                bodyRigWeight = Mathf.Lerp(bodyRigWeight, 0, aIStates._delta * currentBodyRigRetargetSpeed);
            }
        }

        public void GetCurrentBodyRigRetargetSpeed()
        {
            if (_ai.distanceToTarget < maxBodyRigSpeedDistance)
            {
                currentBodyRigRetargetSpeed = bodyRigRetargetSpeed;
            }
            else if (_ai.distanceToTarget > minBodyRigSpeedDistance)
            {
                currentBodyRigRetargetSpeed = 0;
            }
            else
            {
                currentBodyRigRetargetSpeed = (_ai.distanceToTarget / minBodyRigSpeedDistance * (0 - bodyRigRetargetSpeed)) + bodyRigRetargetSpeed;
            }
        }
        #endregion

        #region On Death.
        public void OnEnemyDeath()
        {
            iKTarget.gameObject.SetActive(false);
            isUsingIK = false;
            enabled = false;
        }

        public void OnEnemyRevive()
        {
            iKTarget.gameObject.SetActive(true);
            isUsingIK = true;
            enabled = true;
        }
        #endregion

        #region On Player Death.
        public void OnPlayerDeathExitAggroTurnOffIK()
        {
            isUsingIK = false;
            TurnOffHeadRigIKWithTween();
        }
        #endregion

        #region Regular Checkpoint Refresh.
        public void OnCheckpointRefresh_General()
        {
            isUsingIK = true;
            isEnemyForwardIK = true;

            isManuverIK = false;
            isIKRetargetStopped = false;

            currentBodyRigRetargetSpeed = 0;
            bodyRigWeight = 0;
        }
        #endregion

        #region Boss Checkpoint Refresh.
        public void OnCheckpointRefresh_Boss()
        {
            headRigWeight = 0;
            bodyRigWeight = 0;
            tarLookAtWeight = 0;

            isUsingIK = false;
            isManuverIK = false;
            isIKRetargetStopped = false;
        }
        #endregion
    }
}