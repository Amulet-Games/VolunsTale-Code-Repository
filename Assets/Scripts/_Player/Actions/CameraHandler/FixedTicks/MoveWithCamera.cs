using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Camera Handler/Actions/FixedTicks/MoveWithCamera")]
    public class MoveWithCamera : CameraAction
    {
        ///* Whenever you move want to move the camera whether it's because of collision or zoom effect,
        ///* You want to move the Main Camera itself instead of camHandler position,
        ///* Here I move camHandler position to follow player is the 'Only One Case'.
        ///* In the future if you want to move camera for collision,
        ///* Plz use "_mainCameraTransform.localPosition".

        [Header("Config.")]
        public float moveSpeed = 9;

        public override void Execute(CameraHandler camHandler)
        {
            Transform camHandlerTrans = camHandler._mTransform;

            /// Target position is Player's mTransform position.
            camHandlerTrans.position = Vector3.Lerp(camHandlerTrans.position, camHandler.states.mTransform.position, camHandler._fixedDelta * moveSpeed);
        }
    }
}
