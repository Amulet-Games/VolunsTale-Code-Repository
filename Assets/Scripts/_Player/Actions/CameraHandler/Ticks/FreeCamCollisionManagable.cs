using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Camera Handler/Managable/I Camera Collision Managable/Free Camera Collision Managable")]
    public class FreeCamCollisionManagable : ScriptableObject, ICameraCollisionManagable
    {
        public void Execute()
        {
            CameraHandler.singleton.HandleCameraCollision();
        }
    }
}