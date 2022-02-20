using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class TurnWithPlayer : StateActions
    {
        [Header("Configuration")]
        public float normalTurnSpeed = 8f;
        public float lockonTurnSpeed = 8f;

        protected void TurnWithMoveDirection(StateManager states)
        {
            Quaternion targetRotation;
            
            if (states.moveAmount != 0)
            {
                targetRotation = Quaternion.Slerp(states.transform.rotation, Quaternion.LookRotation(states.moveDirection), states.moveAmount * normalTurnSpeed * states._delta);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(states.mTransform.forward);
            }

            states.mTransform.rotation = targetRotation;
        }

        protected void TurnWithLockonDirection(StateManager states)
        {
            Quaternion targetRotation = Quaternion.Slerp(states.transform.rotation, Quaternion.LookRotation(states._dirToLockonStates), lockonTurnSpeed * states._delta);
            states.mTransform.rotation = targetRotation;
        }
    }
}