using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Camera Handler/Managable/I Camera Rotate Managable/Cam Lockon Rotate")]
    public class CamLockonRotateManagable : ScriptableObject, ICameraRotateManagable
    {
        [Header("Lockon Rotate Speed")]
        public float yLockonRotateSpeed = 8;
        public float xLockonRotateSpeed = 2;
        
        public void Execute(CameraHandler camHandler)
        {
            if (camHandler.states._lockonBodyBoneTransform == null)
                return;

            StateManager states = camHandler.states;
            Vector3 dirToBodyBone = states._lockonBodyBoneTransform.position - camHandler._mTransform.position;

            // Root
            NormalRootRotation();

            // Pivot
            if (states._lockonState.applyControllerCameraYMovement)
            {
                FollowEnemyPivotRotation();
            }
            else
            {
                NormalPivotRotation();
            }

            #region Root Rotation.
            void NormalRootRotation()
            {
                Transform _camHandlerTrans = camHandler._mTransform;

                dirToBodyBone.y = 0;
                Quaternion targetRot = Quaternion.LookRotation(dirToBodyBone);

                _camHandlerTrans.rotation = Quaternion.Slerp(_camHandlerTrans.rotation, targetRot, camHandler._fixedDelta * yLockonRotateSpeed);
                camHandler._currentYAngle = _camHandlerTrans.localEulerAngles.y;
            }
            #endregion

            #region Pivot Rotation.
            void NormalPivotRotation()
            {
                Transform _pivotTrans = camHandler._pivotTransform;

                _pivotTrans.localRotation = Quaternion.Slerp(_pivotTrans.localRotation, Quaternion.identity, camHandler._fixedDelta * xLockonRotateSpeed);
                camHandler._currentXAngle = _pivotTrans.localEulerAngles.x;
            }

            void FollowEnemyPivotRotation()
            {
                Transform _pivotTrans = camHandler._pivotTransform;

                float disToPlayer = states._lockonState.aiManager.distanceToTarget;
                Vector3 specialDirToEnemy = dirToBodyBone;

                if (disToPlayer < 1.5f)
                {
                    specialDirToEnemy.y -= 2f;
                }
                else if (disToPlayer > 1.5f && disToPlayer < 3)
                {
                    specialDirToEnemy.y -= 1;
                }
                else
                {
                    specialDirToEnemy.y -= 0.5f;
                }

                Quaternion targetRot = Quaternion.LookRotation(specialDirToEnemy);
                _pivotTrans.rotation = Quaternion.Slerp(_pivotTrans.rotation, targetRot, camHandler._fixedDelta * xLockonRotateSpeed);
                camHandler._currentXAngle = _pivotTrans.localEulerAngles.x;
            }
            #endregion
        }
    }
}
