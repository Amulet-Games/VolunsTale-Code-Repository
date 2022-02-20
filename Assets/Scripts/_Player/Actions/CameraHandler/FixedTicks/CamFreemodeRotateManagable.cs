using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Camera Handler/Managable/I Camera Rotate Managable/Cam Freemode Rotate")]
    public class CamFreemodeRotateManagable : ScriptableObject, ICameraRotateManagable
    {
        [Header("Clamp X Rotation.")]
        public bool isClampX = true;
        public float maxXValue = 35;
        public float minXValue = -25;

        [Header("Clamp Y Rotation.")]
        public bool isClampY;
        public float maxYValue;
        public float minYValue;

        [Header("Freemode Rotate Speed")]
        public float yRotateSpeed = 3;
        public float xRotateSpeed = 3;

        private float currentYAngle;
        private float currentXAngle;

        public void Execute(CameraHandler camHandler)
        {
            if (camHandler._currentXAngle > 180)
            {
                float newAngle = camHandler._currentXAngle - 360;
                if (newAngle < 0)
                {
                    camHandler._currentXAngle = newAngle;
                }
            }

            currentYAngle = camHandler._currentYAngle;
            currentXAngle = camHandler._currentXAngle;

            currentYAngle += camHandler.mouseX * yRotateSpeed * camHandler._fixedDelta;
            currentXAngle -= camHandler.mouseY * xRotateSpeed * camHandler._fixedDelta;

            if (isClampY)
            {
                currentYAngle = Mathf.Clamp(currentYAngle, minYValue, maxYValue);
            }

            if (isClampX)
            {
                currentXAngle = Mathf.Clamp(currentXAngle, minXValue, maxXValue);
            }

            camHandler._mTransform.localRotation = Quaternion.Euler(0, currentYAngle, 0);
            camHandler._pivotTransform.localRotation = Quaternion.Euler(currentXAngle, 0, 0);

            camHandler._currentYAngle = currentYAngle;
            camHandler._currentXAngle = currentXAngle;
        }
    }
}