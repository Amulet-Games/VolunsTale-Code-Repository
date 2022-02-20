using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Mono Actions/Lockon States Info Calculate")]
    public class LockonStatesInfoCalculate : MonoAction
    {
        public override void Execute(StateManager states)
        {
            Vector3 _statesFwd = states.mTransform.forward;
            Vector3 _statesZero = states.vector3Zero;

            if (states._lockonState != null)
            {
                /// LOCK_ON STATES
                // DIRECTION
                CalculateDirectionToLockonState();
                // ANGLE
                CalculateAngleToLockonState();
            }
            else
            {
                states._dirToLockonStates = _statesFwd;
            }
            
            void CalculateDirectionToLockonState()
            {
                Vector3 dirToStates = states._lockonState.mTransform.position - states.mTransform.position;
                dirToStates.y = 0;
                if (dirToStates == _statesZero)
                    dirToStates = _statesFwd;
                states._dirToLockonStates = dirToStates;
            }

            void CalculateAngleToLockonState()
            {
                states._angleToLockonStates = Vector3.Angle(_statesFwd, states._dirToLockonStates);
            }
        }
    }
}