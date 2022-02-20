using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/AI/Ticks/AI_IsGrounded")]
    public class AI_IsGrounded : StateActions
    {
        [Header("Configuration")]
        public float groundedDis = 1.4f;
        public float onAirDis = 1f;
        public float onGroundedThershold = 0.18f;
        public float offGroundPlayAnimThershold = 1f; 
        public int _updateRate = 25;
        public bool drawDebugRay;

        RaycastHit hit;

        public override void AIExecute(AIStateManager _aiStates)
        {
            if (_aiStates.ignoreGroundCheck)
                return;

            if (_aiStates._frameCount % _updateRate == 0)
            {
                Vector3 origin = _aiStates.mTransform.position;
                origin.y += .7f;

                float dis = groundedDis;
                if (!_aiStates.isGrounded)
                {
                    dis = onAirDis;
                }

                Vector3 dir = -_aiStates.aiManager.vector3Up;

                //if (drawDebugRay)
                //{
                //    Debug.DrawRay(origin, dir * dis, Color.black);
                //}
                
                if (Physics.SphereCast(origin, .3f, dir, out hit, dis, _aiStates._layerManager._enemyGroundCheckMask))
                {
                    float YValue = _aiStates.mTransform.position.y - hit.point.y;

                    //if (_aiStates.gameObject.name == "grunt_Marksman_31")
                    //{
                    //    Debug.Log("YValue = " + YValue);
                    //    Debug.Log("hit.point.y = " + hit.point.y);
                    //    Debug.Log("hit = " + hit.transform.gameObject.name);
                    //}

                    if (YValue < onGroundedThershold)
                    {
                        //Debug.Log("Is Grounded is true.");
                        _aiStates.SetIsGroundStatusToTrue();
                    }
                    else
                    {
                        if (YValue > offGroundPlayAnimThershold)
                        {
                            _aiStates.SetIsGroundStatusToFalseWithAnim();
                        }
                        else
                        {
                            //Debug.Log("Is Grounded is false.");
                            _aiStates.SetIsGroundStatusToFalse();
                        }
                    }
                }
                else
                {
                    _aiStates.SetIsGroundStatusToFalseWithAnim();
                }
            }
        }

        public override void Execute(StateManager states)
        {
        }
    }
}