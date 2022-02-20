using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/CharRotateBasedOnCamera")]
    public class CharRotateBasedOnCamera : StateActions
    {
        //public TransformVariable cameraTransform;
        //public float speed = 8;

        public override void Execute(StateManager states)
        {
            /*
            if (cameraTransform.value == null )
                return;
  
            Vector3 targetDir = cameraTransform.value.forward * states.vertical;
            targetDir += cameraTransform.value.right * states.horizontal;
            StaticHelper.NormalizedVector3(targetDir);

            targetDir.y = 0;
            if (targetDir == states.vector3Zero)
                targetDir = states.transform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(
                states.transform.rotation, tr,
                states.delta * states.moveAmount * speed);

            states.transform.rotation = targetRotation;
            */
        }

        public override void AIExecute(AIStateManager aIState)
        {
            //throw new NotImplementedException();
        }
    }
}
