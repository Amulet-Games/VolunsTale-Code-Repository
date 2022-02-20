using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Camera Handler/Actions/Ticks/Apply Lockon Zoom Effect")]
    public class ApplyLockonZoomEffect : CameraAction
    {
        [Header("Configuration")]
        public float speed = 8;
        public float zoomedFov = 44;
        public float normalFov = 41;
        public float leastZoomEffectAppliedDistance = 1.5f;
        
        public override void Execute(CameraHandler camHandler)
        {
            StateManager states = camHandler.states;

            if (!states._lockonBodyBoneTransform)
            {
                ResetFovValue(camHandler);
                return;
            }
            
            if (states._lockonState.aiManager.distanceToTarget <= leastZoomEffectAppliedDistance)
                return;

            if (states._lockonState.applyControllerCameraZoom)
            {
                IncreaseFovValue(camHandler);
            }
            else
            {
                ResetFovValue(camHandler);
            }
        }

        void ResetFovValue(CameraHandler camHandler)
        {
            Camera mainCamera = camHandler.mainCamera;

            if (mainCamera.fieldOfView - normalFov >= 0.05f)
            {
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, normalFov, camHandler._delta * speed);
            }
            else
            {
                mainCamera.fieldOfView = normalFov;
            }
        }

        void IncreaseFovValue(CameraHandler camHandler)
        {
            Camera mainCamera = camHandler.mainCamera;

            if (zoomedFov - mainCamera.fieldOfView >= 0.05f)
            {
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, zoomedFov, camHandler._delta * speed);
            }
            else
            {
                mainCamera.fieldOfView = zoomedFov;
            }
        }
    }
}