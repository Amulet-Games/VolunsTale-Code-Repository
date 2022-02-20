using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SA
{
    [CreateAssetMenu(menuName = "Mono Actions/Search For Lockon States")]
    public class SearchForLockonStates : MonoAction
    {
        [Header("Configuration")]
        public float targetInAngleThershold = 0.7f;
        public float mixLockonRequirementSmallestDot = 0.8f;
        
        [NonSerialized]
        List<AIStateManager> potentialStates = new List<AIStateManager>();
        [NonSerialized]
        List<float> potentialStatesCompareValues = new List<float>();
        [NonSerialized]
        List<AIStateManager> backupStates = new List<AIStateManager>();
        [NonSerialized]
        List<float> backupStatesCompareValues = new List<float>();

        public override void Execute(StateManager states)
        {
            if (states.p_lockon_input)
            {
                if (states.isLockingOn)
                {
                    states.SetIsLockingOnStatusToFalse();
                }
                else
                {
                    FindLockableTargets(states);
                }
            }
            else
            {
                states.ValidateCheckLockonTarget();
            }
        }

        void FindLockableTargets(StateManager states)
        {
            /// find the nearest AI States
            float _compare_value = 100;
            int _potentialTargetCount = 0;
            int _backupTargetCount = 0;
            AIStateManager _resultState = null;

            int totalHitNum = states.GetTargetWithinLockableSphere();
            if (totalHitNum > 0)
            {
                if (totalHitNum == 1)
                {
                    LockonWithSingleTarget();
                }
                else
                {
                    #region Lock on With Multiple Target.
                    switch (states._preferLockonMode)
                    {
                        case LockonToEnemyModeEnum.Prioritize_Distance:
                            _compare_value = 100;   /// We want to find target that closet to player, so we start from a big value of 100.
                            for (int i = 0; i < totalHitNum; i++)
                            {
                                LockonWithDistancePiroritize(i);
                            }
                            break;

                        case LockonToEnemyModeEnum.Prioritize_Angle:
                            _compare_value = 0;     /// We want to find target that dot product is closer to 1, so we start from a small value of 0.
                            for (int i = 0; i < totalHitNum; i++)
                            {
                                LockonWithAnglePiroritize(i);
                            }
                            break;

                        case LockonToEnemyModeEnum.MixWithDistance:
                            OnLockonWithDistanceMix();
                            for (int i = 0; i < totalHitNum; i++)
                            {
                                LockonWithMixRequirement(i);
                            }

                            if (_potentialTargetCount > 0)
                            {
                                FindTargetWithinAngleInDistance(_potentialTargetCount);
                            }
                            else
                            {
                                FindTargetOutsideAngleInDistance();
                            }
                            break;

                        case LockonToEnemyModeEnum.MixWithAngle:
                            OnLockonWithAngleMix();
                            for (int i = 0; i < totalHitNum; i++)
                            {
                                LockonWithMixRequirement(i);
                            }

                            if (_potentialTargetCount > 0)
                            {
                                FindTargetWithinAngleInAngle(_potentialTargetCount);
                            }
                            else
                            {
                                FindTargetOutsideAngleInAngle();
                            }
                            break;
                    }

                    ConfirmResult();
                    #endregion
                }
            }

            #region Multiple Targets.
            void LockonWithDistancePiroritize(int _index)
            {
                /// calculate a dot product from hitColliders[i] to MainCamera forward transform(facing direction).
                Transform _mainCameraTransform = CameraHandler.singleton._mainCameraTransform;
                Vector3 _mainCameraForwardDir = _mainCameraTransform.forward;
                _mainCameraForwardDir.y = 0;

                Vector3 dirFromCamToHitCollider = (states.lockableHitColliders[_index].transform.position - _mainCameraTransform.position);
                dirFromCamToHitCollider.y = 0;
                dirFromCamToHitCollider = dirFromCamToHitCollider.normalized;
                //Debug.DrawRay(_mainCameraTransform.position, dirFromCamToHitCollider, Color.black);
                //Debug.Break();

                /// if hitColliders[i] is away from player camera fov, continue to next hitCollider...
                float dot = Vector3.Dot(_mainCameraForwardDir, dirFromCamToHitCollider); //Debug.Log("hitColliders[i] = " + states.lockableHitColliders[_index].gameObject + "dot = " + dot);
                if (dot < targetInAngleThershold)
                    return;

                /// if hitColliders[i] is inside player camera fov, check if there is obstacles in between them.
                if (states.WallsInBetweenLockableCollider_Multiple_OutDistance(_index, out float _outDistance))
                    return;

                /// if there is no obstacles in between, find the closet enemy in distance as target.
                if (_outDistance < _compare_value)
                {
                    _compare_value = _outDistance;
                    _resultState = states.lockableHitColliders[_index].GetComponent<AIStateManager>();
                    //Debug.Log("_resultState = " + _resultState);
                }
            }

            void LockonWithAnglePiroritize(int _index)
            {
                /// calculate a dot product from hitColliders[i] to MainCamera forward transform(facing direction).
                Transform _mainCameraTransform = CameraHandler.singleton._mainCameraTransform;
                Vector3 _mainCameraForwardDir = _mainCameraTransform.forward;
                _mainCameraForwardDir.y = 0;

                Vector3 dirFromCamToHitCollider = (states.lockableHitColliders[_index].transform.position - _mainCameraTransform.position).normalized;
                dirFromCamToHitCollider.y = 0;

                /// if hitColliders[i] is away from player camera fov, continue to next hitCollider...
                float dot = Vector3.Dot(_mainCameraForwardDir, dirFromCamToHitCollider); //Debug.Log("hitColliders[i] = " + states.lockableHitColliders[_index].gameObject + "dot = " + dot);
                if (dot < targetInAngleThershold)
                    return;
                
                /// if hitColliders[i] is inside player camera fov, check if there is obstacles in between them.
                if (states.WallsInBetweenLockableCollider_Multiple(_index))
                    return;

                /// if there is no obstacles in between, find the closet enemy in angle as target.
                if (dot > _compare_value)
                {
                    _compare_value = dot;
                    _resultState = states.lockableHitColliders[_index].GetComponent<AIStateManager>();
                    //Debug.Log("aiStates = " + _resultState);
                }
            }

            #region Mix Distance / Angle
            void OnLockonWithDistanceMix()
            {
                potentialStates.Clear();
                potentialStatesCompareValues.Clear();
                backupStates.Clear();
                backupStatesCompareValues.Clear();
                _compare_value = 100;
            }

            void LockonWithMixRequirement(int _index)
            {
                /// calculate a dot product from hitColliders[i] to MainCamera forward transform(facing direction).
                Transform _mainCameraTransform = CameraHandler.singleton._mainCameraTransform;
                Vector3 _mainCameraForwardDir = _mainCameraTransform.forward;
                _mainCameraForwardDir.y = 0;

                Vector3 dirFromCamToHitCollider = (states.lockableHitColliders[_index].transform.position - _mainCameraTransform.position).normalized;
                dirFromCamToHitCollider.y = 0;

                /// if hitColliders[i] is away from player camera fov, continue to next hitCollider...
                float dot = Vector3.Dot(_mainCameraForwardDir, dirFromCamToHitCollider);
                if (dot < targetInAngleThershold)
                    return;
                
                /// if hitColliders[i] is inside player camera fov, check if there is obstacles in between them.
                if (states.WallsInBetweenLockableCollider_Multiple_OutDistance(_index, out float _outDistance))
                    return;
                
                if (dot > mixLockonRequirementSmallestDot)
                {
                    potentialStates.Add(states.lockableHitColliders[_index].GetComponent<AIStateManager>());
                    potentialStatesCompareValues.Add(dot);
                    _potentialTargetCount++;
                }
                else
                {
                    backupStates.Add(states.lockableHitColliders[_index].GetComponent<AIStateManager>());
                    backupStatesCompareValues.Add(_outDistance);
                    _backupTargetCount++;
                }
            }

            void FindTargetWithinAngleInDistance(int _index)
            {
                for (int i = 0; i < _index; i++)
                {
                    /// Here you want to find the closet target near player, so it's smaller than compare value of 100, best to near 0.
                    if (potentialStatesCompareValues[i] < _compare_value)
                    {
                        _compare_value = potentialStatesCompareValues[i];
                        _resultState = potentialStates[i];
                    }
                }
            }

            void FindTargetOutsideAngleInDistance()
            {
                for (int i = 0; i < _backupTargetCount; i++)
                {
                    if (backupStatesCompareValues[i] < _compare_value)
                    {
                        /// Here you want to find the closet target near player, so it's smaller than compare value of 100, best to near 0.
                        _compare_value = backupStatesCompareValues[i];
                        _resultState = backupStates[i];
                    }
                }
            }

            void OnLockonWithAngleMix()
            {
                potentialStates.Clear();
                potentialStatesCompareValues.Clear();
                backupStates.Clear();
                backupStatesCompareValues.Clear();
                _compare_value = 0;
            }

            void FindTargetWithinAngleInAngle(int _index)
            {
                for (int i = 0; i < _index; i++)
                {
                    /// Here you want to find the closet target in camera vision, so it's bigger than compare value of 0, best to near value of 1.
                    if (potentialStatesCompareValues[i] > _compare_value)
                    {
                        _compare_value = potentialStatesCompareValues[i];
                        _resultState = potentialStates[i];
                    }
                }
            }

            void FindTargetOutsideAngleInAngle()
            {
                _compare_value = 100;
                for (int i = 0; i < _backupTargetCount; i++)
                {
                    if (backupStatesCompareValues[i] < _compare_value)
                    {
                        /// Here you want to find the closet target near player, so it's smaller than compare value of 100, best to near 0.
                        _compare_value = backupStatesCompareValues[i];
                        _resultState = backupStates[i];
                    }
                }
            }
            #endregion

            void ConfirmResult()
            {
                if (_resultState != null)
                {
                    states.SetIsLockingOnStatusToTrue(_resultState);
                }
            }
            #endregion

            #region Single Target.
            void LockonWithSingleTarget()
            {
                /// calculate a dot product from hitColliders[i] to MainCamera forward transform(facing direction).
                Transform _mainCameraTransform = CameraHandler.singleton._mainCameraTransform;
                Vector3 _mainCameraForwardDir = _mainCameraTransform.forward;
                _mainCameraForwardDir.y = 0;

                Vector3 dirFromCamToHitCollider = (states.lockableHitColliders[0].transform.position - _mainCameraTransform.position).normalized;
                dirFromCamToHitCollider.y = 0;

                /// if hitColliders[i] is away from player camera fov, continue to next hitCollider...
                float dot = Vector3.Dot(_mainCameraForwardDir, dirFromCamToHitCollider);
                //Debug.Log("hitColliders[i] = " + hitColliders[0].gameObject + "dot = " + dot);

                /// if hitColliders[i] is inside player camera fov, check if there is obstacles in between them.
                if (dot > targetInAngleThershold)
                {
                    //Debug.Log("hitColliders[i] = " + hitColliders[i]);
                    //Debug.Log("aiStates = " + aiStates);

                    /// if there is no wall between hitColliders[i] and player...
                    if (!states.WallsInBetweenLockableCollider_Single())
                    {
                        states.SetIsLockingOnStatusToTrue(states.lockableHitColliders[0].GetComponent<AIStateManager>());
                    }
                }
            }
            #endregion
        }
    }
}

