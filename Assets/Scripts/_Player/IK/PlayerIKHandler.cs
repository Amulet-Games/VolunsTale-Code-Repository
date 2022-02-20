using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PlayerIKHandler : MonoBehaviour
    {
        [Header("Head IK Tween.")]
        public LeanTweenType _headIKEaseType;
        public float _headIKActivateTime;
        public float _headIKDeactivateTime;
        [ReadOnlyInspector, SerializeField] float _currentHeadIKWeight;

        [Header("Body IK Tween.")]
        public LeanTweenType _bodyIKEaseType;
        public float _bodyIKActivateTime;
        public float _bodyIKDeactivateTime;
        [ReadOnlyInspector, SerializeField] float _currentUpperBodyIKWeight;

        [Header("Hand IK Tween.")]
        public LeanTweenType _handIKEaseType;
        public LeanTweenType _handHelpersEaseType;
        public float _handIKActivateTime;
        public float _handIKRotateActivateTime;
        public float _IKJobMinimizeTime = 0.1f;
        public float _handHelpersMoveTime;
        [ReadOnlyInspector, SerializeField] float _currentLeftHandIKMoveWeight;
        [ReadOnlyInspector, SerializeField] float _currentRightHandIKMoveWeight;
        [ReadOnlyInspector, SerializeField] float _currentLeftHandIKRotateWeight;
        [ReadOnlyInspector, SerializeField] float _currentRightHandIKRotateWeight;
        [ReadOnlyInspector, SerializeField] public float _nextDesireLeftHandMoveWeight;
        [ReadOnlyInspector, SerializeField] public float _nextDesireRightHandMoveWeight;

        [Header("LookAt Helper Tween.")]
        public LeanTweenType _lookAtHelperEaseType;
        public float _lookAtHelperMoveTime;
        [ReadOnlyInspector] public float _nextDesiredLookAtWeight;

        [Header("LookAt Helper Pos.")]
        public Vector3 _IdleInitLookAtHelperPos;
        [ReadOnlyInspector] public Vector3 _currentIdleLookAtHelperPos;
        [ReadOnlyInspector] public bool _isCurrentIdleHeadOnly;
        [ReadOnlyInspector, SerializeField] bool _isLookAtHelperAwayFromIdlePos;

        [Header("Update Weight's Speed.")]
        public float _LookAtIKWeightLerpSpeed = 10;
        public float _HandIKWeightLerpSpeed = 8;

        [Header("Update LookAt Helper Pos Speed.")]
        public float _LookAtHelperDampSpeed = 0.15f;
        public float _lockonLookAtHelperDampSpeed = 0.2f;
        public float _LookAtHelperLockonMoveSpeed = 8;
        
        [Header("Update Hand IK Helper Pos Speed.")]
        public float _HandHelpersMoveDampSpeed;
        public float _HandHelpersRotateSlerpSpeed;

        [Header("LookAt Status.")]
        [ReadOnlyInspector] public bool isUsingHeadOnlyIK;
        [ReadOnlyInspector] public bool isUsingUpperBodyIK;
        [ReadOnlyInspector] public bool isNotUsingLookAtIK;
        
        [Header("LookAt Helpers.")]
        [ReadOnlyInspector] public Transform lookAtHelper;

        [Header("Hand Helpers.")]
        [ReadOnlyInspector] public Transform _lhHelper;
        [ReadOnlyInspector] public Transform _rhHelper;
        
        [Header("Manager Refs.")]
        [ReadOnlyInspector] public StateManager _states;
        [ReadOnlyInspector] public Animator anim;
        [NonSerialized] public SavableInventory _inventory;

        [Header("Handle IK Job.")]
        public List<HandleIKJob> currentHandleIKJobs = new List<HandleIKJob>();
        [ReadOnlyInspector] public bool _isHandleIKJobsEmpty;

        [Header("LocomotionIK States.")]
        public FreeForm_1H_LocoIKState _freeForm_1H_LocoIKState;
        public FreeForm_2H_LocoIKState _freeForm_2H_LocoIKState;
        public FreeFormDefense_1H_LocoIKState _freeFormDefense_1H_LocoIKState;
        public FreeFormDefense_2H_LocoIKState _freeFormDefense_2H_LocoIKState;
        public Lockon_1H_LocoIKState _lockon_1H_LocoIKState;
        public Lockon_2H_LocoIKState _lockon_2H_LocoIKState;
        public LockonDefense_1H_LocoIKState _lockonDefense_1H_LocoIKState;
        public LockonDefense_2H_LocoIKState _lockonDefense_2H_LocoIKState;

        [Header("Weapon IK Profile.")]
        public PlayerWeaponIKProfile _axeWeaponIKProfile;
        public PlayerWeaponIKProfile _shieldWeaponIKProfile;
        public PlayerWeaponIKProfile _fistWeaponIKProfile;
        [ReadOnlyInspector] public PlayerWeaponIKProfile _currentWeaponIKProfile;
        [ReadOnlyInspector, NonSerialized] public Weapon_Oppose1_Defense_Profile _currentOppose1DefenseProfile;

        [Header("Private.")]
        /// Weights.
        int _headWeightTweenId;
        int _bodyWeightTweenId;
        int _leftHandWeightMoveTweenId;
        int _leftHandWeightRotateTweenId;
        int _rightHandWeightMoveTweenId;
        int _rightHandWeightRotateTweenId;

        /// Positions.
        int _lookAtHelperTweenId;
        int _leftHandHelperMoveTweenId;
        int _leftHandHelperRotateTweenId;
        int _rightHandHelperMoveTweenId;
        int _rightHandHelperRotateTweenId;

        /// Smooth Damp Velocity Refs.
        Vector3 _LookAtHelperDampVelRef;
        Vector3 _LhHelperMoveDampVelRef;
        Vector3 _RhHelperMoveDampVelRef;

        #region External Tick.
        public void OnAnimatorIKTick()
        {
            LookAtIKTick();
            LeftHandIKTick();
            RightHandIKTick();
        }

        public void AnimatorHookTick()
        {
            if (!_isHandleIKJobsEmpty)
            {
                if (anim.GetBool(_states.p_IsHandleIKJobFinished_hash))
                {
                    if (currentHandleIKJobs.Count > 0)
                    {
                        ExecuteNewHandleIKJob();
                    }
                    else
                    {
                        OnHandleIKJobFinished();
                        _isHandleIKJobsEmpty = true;
                    }
                }
            }
        }
        #endregion

        #region Internal Tick.
        void LookAtIKTick()
        {
            anim.SetLookAtWeight(1, _currentUpperBodyIKWeight, _currentHeadIKWeight, _currentHeadIKWeight, 0.5f);
            anim.SetLookAtPosition(lookAtHelper.position);
        }

        void LeftHandIKTick()
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, _currentLeftHandIKMoveWeight);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, _currentLeftHandIKRotateWeight);

            anim.SetIKPosition(AvatarIKGoal.LeftHand, _lhHelper.position);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, _lhHelper.rotation);
        }

        void RightHandIKTick()
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, _currentRightHandIKMoveWeight);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, _currentRightHandIKRotateWeight);

            anim.SetIKPosition(AvatarIKGoal.RightHand, _rhHelper.position);
            anim.SetIKRotation(AvatarIKGoal.RightHand, _rhHelper.rotation);
        }
        #endregion

        #region Handle FreeForm Running LookAt / Goal IK.
        public void HandleFreeFormRunningIK_1H()
        {
            HandleIKWeightWithIsHeadOnly(_currentWeaponIKProfile._1hRunningIKHeadOnly);

            TweenLookAtHelperTargetPosition(_currentWeaponIKProfile._1hRunningLookAtPos);

            Handle_Lh_TargetIKGoalWeight(_currentWeaponIKProfile._1h_Lh_RunningGoal);
            Tween_Lh_HelperTargetGoal(_currentWeaponIKProfile._1h_Lh_RunningGoal);

            Handle_Rh_TargetIKGoalWeight(_currentWeaponIKProfile._1h_Rh_RunningGoal);
            Tween_Rh_HelperTargetGoal(_currentWeaponIKProfile._1h_Rh_RunningGoal);
        }

        public void HandleFreeFormRunningIK_2H()
        {
            HandleIKWeightWithIsHeadOnly(_currentWeaponIKProfile._2hRunningIKHeadOnly);

            TweenLookAtHelperTargetPosition(_currentWeaponIKProfile._2hRunningLookAtPos);

            Handle_Lh_TargetIKGoalWeight(_currentWeaponIKProfile._2h_Lh_RunningGoal);
            Handle_Rh_TargetIKGoalWeight(_currentWeaponIKProfile._2h_Rh_RunningGoal);

            Tween_Lh_HelperTargetGoal(_currentWeaponIKProfile._2h_Lh_RunningGoal);
            Tween_Rh_HelperTargetGoal(_currentWeaponIKProfile._2h_Rh_RunningGoal);
        }
        #endregion

        #region Handle FreeForm Defense Running LookAt / Goal IK.
        public void FreeForm_Oppose1DefenseRunningHandleIK()
        {
            HandleIKWeightWithIsHeadOnly(_currentOppose1DefenseProfile._1hDefenseRunningHeadOnly);

            TweenLookAtHelperTargetPosition(_currentOppose1DefenseProfile._1hDefenseRunningLookAtPos);

            Handle_Lh_TargetIKGoalWeight(_currentOppose1DefenseProfile._oppose1DefenseRunningGoal);
            Tween_Lh_HelperTargetGoal(_currentOppose1DefenseProfile._oppose1DefenseRunningGoal);
            
            Handle_Rh_TargetIKGoalWeight(_currentWeaponIKProfile._1h_Rh_WalkingGoal);
            Tween_Rh_HelperTargetGoal(_currentWeaponIKProfile._1h_Rh_WalkingGoal);
        }

        public void FreeForm_Light2DefenseRunningHandleIK()
        {
            HandleIKWeightWithIsHeadOnly(_currentWeaponIKProfile._2hDefenseRunningHeadOnly);

            TweenLookAtHelperTargetPosition(_currentWeaponIKProfile._2hDefenseRunningLookAtPos);

            Handle_Lh_TargetIKGoalWeight(_currentWeaponIKProfile._light2Defense_Lh_RunningGoal);
            Handle_Rh_TargetIKGoalWeight(_currentWeaponIKProfile._light2Defense_Rh_RunningGoal);

            Tween_Lh_HelperTargetGoal(_currentWeaponIKProfile._light2Defense_Lh_RunningGoal);
            Tween_Rh_HelperTargetGoal(_currentWeaponIKProfile._light2Defense_Rh_RunningGoal);
        }
        #endregion

        #region Update FreeForm Walking / Idle LootAt / Goal IK.

        #region FreeForm.
        public void UpdateFreeFormWalkingIK_1H()
        {
            if (_currentWeaponIKProfile._1hWalkingIKHeadOnly)
            {
                UpdateFreeFormIKHeadOnlyIKWeight_Max();
            }
            else
            {
                UpdateFreeFormIKUpperBodyIKWeight_Max();
            }

            UpdateFreeFormIKLookAtHelperLocalPosition(_currentWeaponIKProfile._1hWalkingLookAtPos);

            PlayerIKGoal _1h_Lh_WalkingGoal = _currentWeaponIKProfile._1h_Lh_WalkingGoal;
            UpdateLhHelperWeight(_1h_Lh_WalkingGoal._goalWeight);
            UpdateLhHelperLocation(_1h_Lh_WalkingGoal);

            PlayerIKGoal _1h_Rh_WalkingGoal = _currentWeaponIKProfile._1h_Rh_WalkingGoal;
            UpdateRhHelperWeight(_1h_Rh_WalkingGoal._goalWeight);
            UpdateRhHelperLocation(_1h_Rh_WalkingGoal);
        }

        public void UpdateFreeFormWalkingIK_2H()
        {
            if (_currentWeaponIKProfile._2hWalkingIKHeadOnly)
            {
                UpdateFreeFormIKHeadOnlyIKWeight_Max();
            }
            else
            {
                UpdateFreeFormIKUpperBodyIKWeight_Max();
            }

            UpdateFreeFormIKLookAtHelperLocalPosition(_currentWeaponIKProfile._2hWalkingLookAtPos);

            PlayerIKGoal _2h_Lh_WalkingGoal = _currentWeaponIKProfile._2h_Lh_WalkingGoal;
            UpdateLhHelperWeight(_2h_Lh_WalkingGoal._goalWeight);
            UpdateLhHelperLocation(_2h_Lh_WalkingGoal);

            PlayerIKGoal _2h_Rh_WalkingGoal = _currentWeaponIKProfile._2h_Rh_WalkingGoal;
            UpdateRhHelperWeight(_2h_Rh_WalkingGoal._goalWeight);
            UpdateRhHelperLocation(_2h_Rh_WalkingGoal);
        }

        public void UpdateFreeFormIdle_1H()
        {
            if (_currentWeaponIKProfile._1hDefaultHeadOnly)
            {
                UpdateFreeFormIKHeadOnlyIKWeight_Max();
            }
            else
            {
                UpdateFreeFormIKUpperBodyIKWeight_Max();
            }

            UpdateFreeFormIKLookAtHelperLocalPosition(_currentWeaponIKProfile._1hDefaultLookAtPos);

            PlayerIKGoal _1h_Lh_DefaultGoal = _currentWeaponIKProfile._1h_Lh_DefaultGoal;
            UpdateLhHelperWeight(_1h_Lh_DefaultGoal._goalWeight);
            UpdateLhHelperLocation(_1h_Lh_DefaultGoal);

            PlayerIKGoal _1h_Rh_DefaultGoal = _currentWeaponIKProfile._1h_Rh_DefaultGoal;
            UpdateRhHelperWeight(_1h_Rh_DefaultGoal._goalWeight);
            UpdateRhHelperLocation(_1h_Rh_DefaultGoal);
        }

        public void UpdateFreeFormIdle_2H()
        {
            if (_currentWeaponIKProfile._2hDefaultHeadOnly)
            {
                UpdateFreeFormIKHeadOnlyIKWeight_Max();
            }
            else
            {
                UpdateFreeFormIKUpperBodyIKWeight_Max();
            }

            UpdateFreeFormIKLookAtHelperLocalPosition(_currentWeaponIKProfile._2hDefaultLookAtPos);

            PlayerIKGoal _2h_Lh_DefaultGoal = _currentWeaponIKProfile._2h_Lh_DefaultGoal;
            UpdateLhHelperWeight(_2h_Lh_DefaultGoal._goalWeight);
            UpdateLhHelperLocation(_2h_Lh_DefaultGoal);

            PlayerIKGoal _2h_Rh_DefaultGoal = _currentWeaponIKProfile._2h_Rh_DefaultGoal;
            UpdateRhHelperWeight(_2h_Rh_DefaultGoal._goalWeight);
            UpdateRhHelperLocation(_2h_Rh_DefaultGoal);
        }
        #endregion

        #region FreeForm Surrounding LookAt.
        public void UpdateFreeForm_CustomizedLookAtIK(bool _isHeadOnly, float _targetLookAtWeight, Vector3 _targetLookAtPos)
        {
            if (_isHeadOnly)
            {
                SetUseHeadOnlyIKToTrue();
                UpdateFreeFormIKHeadOnlyIKWeight(_targetLookAtWeight);
            }
            else
            {
                SetUseUpperBodyIKToTrue();
                UpdateFreeFormIKUpperBodyIKWeight(_targetLookAtWeight);
            }

            UpdateFreeFormIKLookAtHelperPosition(_targetLookAtPos);
        }
        
        public void Update_1H_Walking_IKGoals_FreeFormSurrounding()
        {
            PlayerIKGoal _1h_Lh_WalkingGoal = _currentWeaponIKProfile._1h_Lh_WalkingGoal;
            UpdateLhHelperWeight(_1h_Lh_WalkingGoal._goalWeight);
            UpdateLhHelperLocation(_1h_Lh_WalkingGoal);

            PlayerIKGoal _1h_Rh_WalkingGoal = _currentWeaponIKProfile._1h_Rh_WalkingGoal;
            UpdateRhHelperWeight(_1h_Rh_WalkingGoal._goalWeight);
            UpdateRhHelperLocation(_1h_Rh_WalkingGoal);
        }

        public void Update_2H_Walking_IKGoals_FreeFormSurrounding()
        {
            PlayerIKGoal _2h_Lh_WalkingGoal = _currentWeaponIKProfile._2h_Lh_WalkingGoal;
            UpdateLhHelperWeight(_2h_Lh_WalkingGoal._goalWeight);
            UpdateLhHelperLocation(_2h_Lh_WalkingGoal);

            PlayerIKGoal _2h_Rh_WalkingGoal = _currentWeaponIKProfile._2h_Rh_WalkingGoal;
            UpdateRhHelperWeight(_2h_Rh_WalkingGoal._goalWeight);
            UpdateRhHelperLocation(_2h_Rh_WalkingGoal);
        }

        public void Update_1H_Idle_IKGoals_FreeFormSurrounding()
        {
            PlayerIKGoal _1h_Lh_DefaultGoal = _currentWeaponIKProfile._1h_Lh_DefaultGoal;
            UpdateLhHelperWeight(_1h_Lh_DefaultGoal._goalWeight);
            UpdateLhHelperLocation(_1h_Lh_DefaultGoal);

            PlayerIKGoal _1h_Rh_DefaultGoal = _currentWeaponIKProfile._1h_Rh_DefaultGoal;
            UpdateRhHelperWeight(_1h_Rh_DefaultGoal._goalWeight);
            UpdateRhHelperLocation(_1h_Rh_DefaultGoal);
        }

        public void Update_2H_Idle_IKGoals_FreeFormSurrounding()
        {
            PlayerIKGoal _2h_Lh_DefaultGoal = _currentWeaponIKProfile._2h_Lh_DefaultGoal;
            UpdateLhHelperWeight(_2h_Lh_DefaultGoal._goalWeight);
            UpdateLhHelperLocation(_2h_Lh_DefaultGoal);

            PlayerIKGoal _2h_Rh_DefaultGoal = _currentWeaponIKProfile._2h_Rh_DefaultGoal;
            UpdateRhHelperWeight(_2h_Rh_DefaultGoal._goalWeight);
            UpdateRhHelperLocation(_2h_Rh_DefaultGoal);
        }
        #endregion

        #region FreeForm Defense.
        public void UpdateFreeFormDefenseWalkingIK_1H()
        {
            if (_currentOppose1DefenseProfile._1hDefenseWalkingHeadOnly)
            {
                UpdateFreeFormIKHeadOnlyIKWeight_Max();
            }
            else
            {
                UpdateFreeFormIKUpperBodyIKWeight_Max();
            }

            UpdateFreeFormIKLookAtHelperLocalPosition(_currentOppose1DefenseProfile._1hDefenseWalkingLookAtPos);

            PlayerIKGoal _oppose1DefenseWalkingGoal = _currentOppose1DefenseProfile._oppose1DefenseWalkingGoal;
            UpdateLhHelperWeight(_oppose1DefenseWalkingGoal._goalWeight);
            UpdateLhHelperLocation(_oppose1DefenseWalkingGoal);

            PlayerIKGoal _1h_Rh_WalkingGoal = _currentWeaponIKProfile._1h_Rh_WalkingGoal;
            UpdateRhHelperWeight(_1h_Rh_WalkingGoal._goalWeight);
            UpdateRhHelperLocation(_1h_Rh_WalkingGoal);
        }

        public void UpdateFreeFormDefenseWalkingIK_2H()
        {
            if (_currentWeaponIKProfile._2hDefenseWalkingHeadOnly)
            {
                UpdateFreeFormIKHeadOnlyIKWeight_Max();
            }
            else
            {
                UpdateFreeFormIKUpperBodyIKWeight_Max();
            }

            UpdateFreeFormIKLookAtHelperLocalPosition(_currentWeaponIKProfile._2hDefenseWalkingLookAtPos);

            PlayerIKGoal _light2Defense_Lh_WalkingGoal = _currentWeaponIKProfile._light2Defense_Lh_WalkingGoal;
            UpdateLhHelperWeight(_light2Defense_Lh_WalkingGoal._goalWeight);
            UpdateLhHelperLocation(_light2Defense_Lh_WalkingGoal);

            PlayerIKGoal _light2Defense_Rh_WalkingGoal = _currentWeaponIKProfile._light2Defense_Rh_WalkingGoal;
            UpdateRhHelperWeight(_light2Defense_Rh_WalkingGoal._goalWeight);
            UpdateRhHelperLocation(_light2Defense_Rh_WalkingGoal);
        }

        public void UpdateFreeFormDefenseIdle_1H()
        {
            if (_currentOppose1DefenseProfile._1hDefenseDefaultHeadOnly)
            {
                UpdateFreeFormIKHeadOnlyIKWeight_Max();
            }
            else
            {
                UpdateFreeFormIKUpperBodyIKWeight_Max();
            }

            UpdateFreeFormIKLookAtHelperLocalPosition(_currentOppose1DefenseProfile._1hDefenseDefaultLookAtPos);

            PlayerIKGoal _oppose1DefenseDefaultGoal = _currentOppose1DefenseProfile._oppose1DefenseDefaultGoal;
            UpdateLhHelperWeight(_oppose1DefenseDefaultGoal._goalWeight);
            UpdateLhHelperLocation(_oppose1DefenseDefaultGoal);

            PlayerIKGoal _1h_Rh_DefaultGoal = _currentWeaponIKProfile._1h_Rh_DefaultGoal;
            UpdateRhHelperWeight(_1h_Rh_DefaultGoal._goalWeight);
            UpdateRhHelperLocation(_1h_Rh_DefaultGoal);
        }

        public void UpdateFreeFormDefenseIdle_2H()
        {
            if (_currentWeaponIKProfile._2hDefenseDefaultHeadOnly)
            {
                UpdateFreeFormIKHeadOnlyIKWeight_Max();
            }
            else
            {
                UpdateFreeFormIKUpperBodyIKWeight_Max();
            }

            UpdateFreeFormIKLookAtHelperLocalPosition(_currentWeaponIKProfile._2hDefenseDefaultLookAtPos);

            PlayerIKGoal _light2Defense_Lh_DefaultGoal = _currentWeaponIKProfile._light2Defense_Lh_DefaultGoal;
            UpdateLhHelperWeight(_light2Defense_Lh_DefaultGoal._goalWeight);
            UpdateLhHelperLocation(_light2Defense_Lh_DefaultGoal);

            PlayerIKGoal _light2Defense_Rh_DefaultGoal = _currentWeaponIKProfile._light2Defense_Rh_DefaultGoal;
            UpdateRhHelperWeight(_light2Defense_Rh_DefaultGoal._goalWeight);
            UpdateRhHelperLocation(_light2Defense_Rh_DefaultGoal);
        }
        #endregion

        #endregion

        #region Update Lockon Walking LookAt IK / Goal IK.
        public void UpdateLockonWalkingIK_1H(Vector3 _lookAtPosition)
        {
            UpdateLockonIKUpperBodyWeight();
            UpdateLockonIKLookAtHelperPosition(_lookAtPosition);
            
            PlayerIKGoal _1h_Lh_WalkingGoal = _currentWeaponIKProfile._1h_Lh_WalkingGoal;
            UpdateLhHelperWeight(_1h_Lh_WalkingGoal._goalWeight);
            UpdateLhHelperLocation(_1h_Lh_WalkingGoal);

            PlayerIKGoal _1h_Rh_WalkingGoal = _currentWeaponIKProfile._1h_Rh_WalkingGoal;
            UpdateRhHelperWeight(_1h_Rh_WalkingGoal._goalWeight);
            UpdateRhHelperLocation(_1h_Rh_WalkingGoal);
        }

        public void UpdateLockonWalkingIK_2H(Vector3 _lookAtPosition)
        {
            UpdateLockonIKUpperBodyWeight();
            UpdateLockonIKLookAtHelperPosition(_lookAtPosition);
            
            PlayerIKGoal _2h_Lh_WalkingGoal = _currentWeaponIKProfile._2h_Lh_WalkingGoal;
            UpdateLhHelperWeight(_2h_Lh_WalkingGoal._goalWeight);
            UpdateLhHelperLocation(_2h_Lh_WalkingGoal);

            PlayerIKGoal _2h_Rh_WalkingGoal = _currentWeaponIKProfile._2h_Rh_WalkingGoal;
            UpdateRhHelperWeight(_2h_Rh_WalkingGoal._goalWeight);
            UpdateRhHelperLocation(_2h_Rh_WalkingGoal);
        }

        public void UpdateLockonIdleIK_1H(Vector3 _lookAtPosition)
        {
            UpdateLockonIKUpperBodyWeight();
            UpdateLockonIKLookAtHelperPosition(_lookAtPosition);
            
            PlayerIKGoal _1h_Lh_DefaultGoal = _currentWeaponIKProfile._1h_Lh_DefaultGoal;
            UpdateLhHelperWeight(_1h_Lh_DefaultGoal._goalWeight);
            UpdateLhHelperLocation(_1h_Lh_DefaultGoal);

            PlayerIKGoal _1h_Rh_DefaultGoal = _currentWeaponIKProfile._1h_Rh_DefaultGoal;
            UpdateRhHelperWeight(_1h_Rh_DefaultGoal._goalWeight);
            UpdateRhHelperLocation(_1h_Rh_DefaultGoal);
        }

        public void UpdateLockonIdleIK_2H(Vector3 _lookAtPosition)
        {
            UpdateLockonIKUpperBodyWeight();
            UpdateLockonIKLookAtHelperPosition(_lookAtPosition);
            
            PlayerIKGoal _2h_Lh_DefaultGoal = _currentWeaponIKProfile._2h_Lh_DefaultGoal;
            UpdateLhHelperWeight(_2h_Lh_DefaultGoal._goalWeight);
            UpdateLhHelperLocation(_2h_Lh_DefaultGoal);

            PlayerIKGoal _2h_Rh_DefaultGoal = _currentWeaponIKProfile._2h_Rh_DefaultGoal;
            UpdateRhHelperWeight(_2h_Rh_DefaultGoal._goalWeight);
            UpdateRhHelperLocation(_2h_Rh_DefaultGoal);
        }

        public void UpdateLockonDefenseWalkingIK_1H(Vector3 _lookAtPosition)
        {
            UpdateLockonIKUpperBodyWeight();
            UpdateLockonIKLookAtHelperPosition(_lookAtPosition);
            
            PlayerIKGoal _oppose1DefenseWalkingGoal = _currentOppose1DefenseProfile._oppose1DefenseWalkingGoal;
            UpdateLhHelperWeight(_oppose1DefenseWalkingGoal._goalWeight);
            UpdateLhHelperLocation(_oppose1DefenseWalkingGoal);

            PlayerIKGoal _1h_Rh_WalkingGoal = _currentWeaponIKProfile._1h_Rh_WalkingGoal;
            UpdateRhHelperWeight(_1h_Rh_WalkingGoal._goalWeight);
            UpdateRhHelperLocation(_1h_Rh_WalkingGoal);
        }

        public void UpdateLockonDefenseWalkingIK_2H(Vector3 _lookAtPosition)
        {
            UpdateLockonIKUpperBodyWeight();
            UpdateLockonIKLookAtHelperPosition(_lookAtPosition);
            
            PlayerIKGoal _light2Defense_Lh_WalkingGoal = _currentWeaponIKProfile._light2Defense_Lh_WalkingGoal;
            UpdateLhHelperWeight(_light2Defense_Lh_WalkingGoal._goalWeight);
            UpdateLhHelperLocation(_light2Defense_Lh_WalkingGoal);

            PlayerIKGoal _light2Defense_Rh_WalkingGoal = _currentWeaponIKProfile._light2Defense_Rh_WalkingGoal;
            UpdateRhHelperWeight(_light2Defense_Rh_WalkingGoal._goalWeight);
            UpdateRhHelperLocation(_light2Defense_Rh_WalkingGoal);
        }

        public void UpdateLockonDefenseIdleIK_1H(Vector3 _lookAtPosition)
        {
            UpdateLockonIKUpperBodyWeight();
            UpdateLockonIKLookAtHelperPosition(_lookAtPosition);

            PlayerIKGoal _oppose1DefenseDefaultGoal = _currentOppose1DefenseProfile._oppose1DefenseDefaultGoal;
            UpdateLhHelperWeight(_oppose1DefenseDefaultGoal._goalWeight);
            UpdateLhHelperLocation(_oppose1DefenseDefaultGoal);

            PlayerIKGoal _1h_Rh_DefaultGoal = _currentWeaponIKProfile._1h_Rh_DefaultGoal;
            UpdateRhHelperWeight(_1h_Rh_DefaultGoal._goalWeight);
            UpdateRhHelperLocation(_1h_Rh_DefaultGoal);
        }

        public void UpdateLockonDefenseIdleIK_2H(Vector3 _lookAtPosition)
        {
            UpdateLockonIKUpperBodyWeight();
            UpdateLockonIKLookAtHelperPosition(_lookAtPosition);
            
            PlayerIKGoal _light2Defense_Lh_DefaultGoal = _currentWeaponIKProfile._light2Defense_Lh_DefaultGoal;
            UpdateLhHelperWeight(_light2Defense_Lh_DefaultGoal._goalWeight);
            UpdateLhHelperLocation(_light2Defense_Lh_DefaultGoal);

            PlayerIKGoal _light2Defense_Rh_DefaultGoal = _currentWeaponIKProfile._light2Defense_Rh_DefaultGoal;
            UpdateRhHelperWeight(_light2Defense_Rh_DefaultGoal._goalWeight);
            UpdateRhHelperLocation(_light2Defense_Rh_DefaultGoal);
        }
        #endregion

        #region Handle Lockon Running Goal IK.
        public void HandleLockonRunningIKGoal_1H()
        {
            Handle_Lh_TargetIKGoalWeight(_currentWeaponIKProfile._1h_Lh_RunningGoal);
            Handle_Rh_TargetIKGoalWeight(_currentWeaponIKProfile._1h_Rh_RunningGoal);

            Tween_Lh_HelperTargetGoal(_currentWeaponIKProfile._1h_Lh_RunningGoal);
            Tween_Rh_HelperTargetGoal(_currentWeaponIKProfile._1h_Rh_RunningGoal);
        }

        public void HandleLockonRunningIKGoal_2H()
        {
            Handle_Lh_TargetIKGoalWeight(_currentWeaponIKProfile._2h_Lh_RunningGoal);
            Handle_Rh_TargetIKGoalWeight(_currentWeaponIKProfile._2h_Rh_RunningGoal);

            Tween_Lh_HelperTargetGoal(_currentWeaponIKProfile._2h_Lh_RunningGoal);
            Tween_Rh_HelperTargetGoal(_currentWeaponIKProfile._2h_Rh_RunningGoal);
        }
        #endregion

        #region Handle Lockon Defense Running Goal IK. 
        /// Handle IK When player is locking on to enemy, could be stopped or walking but just started running.
        public void Lockon_Oppose1DefenseRunningHandleIK()
        {
            Handle_Lh_TargetIKGoalWeight(_currentOppose1DefenseProfile._oppose1DefenseRunningGoal);
            Tween_Lh_HelperTargetGoal(_currentOppose1DefenseProfile._oppose1DefenseRunningGoal);
            
            Handle_Rh_TargetIKGoalWeight(_currentWeaponIKProfile._1h_Rh_WalkingGoal);
            Tween_Rh_HelperTargetGoal(_currentWeaponIKProfile._1h_Rh_WalkingGoal);
        }

        public void Lockon_Light2DefenseRunningHandleIK()
        {
            Handle_Lh_TargetIKGoalWeight(_currentWeaponIKProfile._light2Defense_Lh_RunningGoal);
            Handle_Rh_TargetIKGoalWeight(_currentWeaponIKProfile._light2Defense_Rh_RunningGoal);

            Tween_Lh_HelperTargetGoal(_currentWeaponIKProfile._light2Defense_Lh_RunningGoal);
            Tween_Rh_HelperTargetGoal(_currentWeaponIKProfile._light2Defense_Rh_RunningGoal);
        }
        #endregion

        #region Handle Sprinting.
        public void HandleOnSprintingIK()
        {
            RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, _states.vector3Zero);
        }
        #endregion

        #region Handle Consumable LookAt IK.
        public void HandleLookAtIKWhenUsingConsumable(ConsumableItem _consumItem)
        {
            if (_consumItem._useLookAtIKJob)
            {
                _states._isCurrentNeglectUseLookAtIK = true;

                if (_consumItem._isLookAtIKHeadOnly)
                {
                    RegisterNewHandleIKJob(IKUsageType.HeadOnly, _consumItem._lookAtWeight, _states.vector3Zero);
                }
                else
                {
                    RegisterNewHandleIKJob(IKUsageType.UpperBody, _consumItem._lookAtWeight, _states.vector3Zero);
                }
            }
        }
        #endregion

        #region Handle Death IK.
        public void HandleOnDeathIK()
        {
            /// LookAt IK.
            SetUseLookAtIKToFalse();

            /// IK Goal.
            IKJobDeactivateLeftHandIK();
            IKJobDeactivateRightHandIK();

            /// Pause LocoIK State Tick.
            _states._isPausingLocoIKStateTick = true;
        }
        #endregion

        #region Update FreeForm IK.

        #region LookAt.
        public void UpdateFreeFormIKHeadOnlyIKWeight_Max()
        {
            SetUseHeadOnlyIKToTrue();

            if (_currentHeadIKWeight != 1)
            {
                _currentHeadIKWeight = Mathf.Lerp(_currentHeadIKWeight, 1, _LookAtIKWeightLerpSpeed * _states._delta);

                if (FastApproximately(_currentHeadIKWeight, 1))
                    _currentHeadIKWeight = 1;
            }
        }

        public void UpdateFreeFormIKUpperBodyIKWeight_Max()
        {
            SetUseUpperBodyIKToTrue();

            if (_currentUpperBodyIKWeight != 1)
            {
                _currentUpperBodyIKWeight = Mathf.Lerp(_currentUpperBodyIKWeight, 1, _LookAtIKWeightLerpSpeed * _states._delta);

                if (FastApproximately(_currentUpperBodyIKWeight, 1))
                    _currentUpperBodyIKWeight = 1;
            }
        }

        public void UpdateFreeFormIKHeadOnlyIKWeight(float _weight)
        {
            SetUseHeadOnlyIKToTrue();

            if (_currentHeadIKWeight != _weight)
            {
                _currentHeadIKWeight = Mathf.Lerp(_currentHeadIKWeight, _weight, _LookAtIKWeightLerpSpeed * _states._delta);

                if (FastApproximately(_currentHeadIKWeight, _weight))
                    _currentHeadIKWeight = _weight;
            }
        }

        public void UpdateFreeFormIKUpperBodyIKWeight(float _weight)
        {
            SetUseUpperBodyIKToTrue();
            
            if (_currentUpperBodyIKWeight != _weight)
            {
                _currentUpperBodyIKWeight = Mathf.Lerp(_currentUpperBodyIKWeight, _weight, _LookAtIKWeightLerpSpeed * _states._delta);

                if (FastApproximately(_currentUpperBodyIKWeight, _weight))
                    _currentUpperBodyIKWeight = _weight;
            }
        }

        public void UpdateFreeFormIKLookAtHelperLocalPosition(Vector3 _targetPos)
        {
            lookAtHelper.localPosition = Vector3.SmoothDamp(lookAtHelper.localPosition, _targetPos, ref _LookAtHelperDampVelRef, _LookAtHelperDampSpeed);
        }

        public void UpdateFreeFormIKLookAtHelperPosition(Vector3 _targetPos)
        {
            lookAtHelper.position = Vector3.SmoothDamp(lookAtHelper.position, _targetPos, ref _LookAtHelperDampVelRef, _lockonLookAtHelperDampSpeed);
        }
        #endregion

        #region IK Goal.
        public void UpdateLhHelperWeight(float _weight)
        {
            if (_currentLeftHandIKMoveWeight != _weight)
            {
                _currentLeftHandIKMoveWeight = Mathf.Lerp(_currentLeftHandIKMoveWeight, _weight, _HandIKWeightLerpSpeed * _states._delta);
                _currentLeftHandIKRotateWeight = Mathf.Lerp(_currentLeftHandIKRotateWeight, _weight, _HandIKWeightLerpSpeed * _states._delta);

                if (FastApproximately(_currentLeftHandIKMoveWeight, _weight))
                {
                    _currentLeftHandIKMoveWeight = _weight;
                    _currentLeftHandIKRotateWeight = _weight;
                }
            }
        }

        public void UpdateRhHelperWeight(float _weight)
        {
            if (_currentRightHandIKMoveWeight != _weight)
            {
                _currentRightHandIKMoveWeight = Mathf.Lerp(_currentRightHandIKMoveWeight, _weight, _HandIKWeightLerpSpeed * _states._delta);
                _currentRightHandIKRotateWeight = Mathf.Lerp(_currentRightHandIKRotateWeight, _weight, _HandIKWeightLerpSpeed * _states._delta);
                
                if (FastApproximately(_currentRightHandIKMoveWeight, _weight))
                {
                    _currentRightHandIKMoveWeight = _weight;
                    _currentRightHandIKRotateWeight = _weight;
                }
            }
        }
        
        public void UpdateLhHelperLocation(PlayerIKGoal _playerIKGoal)
        {
            _lhHelper.localPosition = Vector3.SmoothDamp(_lhHelper.localPosition, _playerIKGoal._goalLocalPosition, ref _LhHelperMoveDampVelRef, _HandHelpersMoveDampSpeed);
            _lhHelper.localRotation = Quaternion.Slerp(_lhHelper.localRotation, Quaternion.Euler(_playerIKGoal._goalLocalEulers), _HandHelpersRotateSlerpSpeed * _states._delta);
        }

        public void UpdateRhHelperLocation(PlayerIKGoal _playerIKGoal)
        {
            _rhHelper.localPosition = Vector3.SmoothDamp(_rhHelper.localPosition, _playerIKGoal._goalLocalPosition, ref _RhHelperMoveDampVelRef, _HandHelpersMoveDampSpeed);
            _rhHelper.localRotation = Quaternion.Slerp(_rhHelper.localRotation, Quaternion.Euler(_playerIKGoal._goalLocalEulers), _HandHelpersRotateSlerpSpeed * _states._delta);
        }
        #endregion

        #endregion

        #region Update Lockon Look At IK.
        public void UpdateLockonIKUpperBodyWeight()
        {
            SetUseUpperBodyIKToTrue();
            _currentUpperBodyIKWeight = Mathf.Lerp(_currentUpperBodyIKWeight, 1, _LookAtIKWeightLerpSpeed * _states._delta);
        }

        public void UpdateLockonIKLookAtHelperPosition(Vector3 _targetPos)
        {
            lookAtHelper.position = Vector3.Lerp(lookAtHelper.position, _targetPos, _LookAtHelperLockonMoveSpeed * _states._delta);
        }
        #endregion

        #region INeglect Transition.
        public void DeactivateIKWhenINeglect()
        {
            SetUseLookAtIKToFalse();
            IKJobDeactivateLeftHandIK();
            IKJobDeactivateRightHandIK();
        }
        #endregion

        #region Renew LookAt Positions. Default
        /// This is Variation of 'TweenLookAtHelperTargetPosition', this is created because of it is able to be cancelled when surround target is near player.
        public void TweenLookAtHelperTargetPosition(Vector3 _targetPos)
        {
            CancelCurrentLookAtHelperTweenJob();
            
            _isLookAtHelperAwayFromIdlePos = true;
            _lookAtHelperTweenId = LeanTween.moveLocal(lookAtHelper.gameObject, _targetPos, _lookAtHelperMoveTime).setEase(_lookAtHelperEaseType).id;
        }

        public void TweenLookAtHelperCurrentIdlePosition()
        {
            CancelCurrentLookAtHelperTweenJob();

            _lookAtHelperTweenId = LeanTween.moveLocal(lookAtHelper.gameObject, _currentIdleLookAtHelperPos, _lookAtHelperMoveTime).setEase(_lookAtHelperEaseType).setOnComplete(OnCompleteTweenDefaultPosition).id;
        }

        public void CancelCurrentLookAtHelperTweenJob()
        {
            if (LeanTween.isTweening(_lookAtHelperTweenId))
            {
                LeanTween.cancel(_lookAtHelperTweenId);
            }
        }

        void OnCompleteTweenDefaultPosition()
        {
            _isLookAtHelperAwayFromIdlePos = false;
        }
        #endregion

        #region Register RH Weapon IK.
        public void InitRegisterRhWeaponIK()
        {
            SetCurrent_RH_WeaponIKProfile();

            _isCurrentIdleHeadOnly = _currentWeaponIKProfile._1hDefaultHeadOnly;
            _currentIdleLookAtHelperPos = _currentWeaponIKProfile._1hDefaultLookAtPos;

            HandleIKWeightWithIsHeadOnly(_isCurrentIdleHeadOnly);
            TweenLookAtHelperCurrentIdlePosition();

            Tween_Lh_HelperTargetGoal(_currentWeaponIKProfile._1h_Lh_DefaultGoal);
            Tween_Rh_HelperTargetGoal(_currentWeaponIKProfile._1h_Rh_DefaultGoal);
        }

        public void RegisterRhWeaponIK()
        {
            SetCurrent_RH_WeaponIKProfile();
            SetCurrent_Oppose1_Defense_IKProfile();

            _isCurrentIdleHeadOnly = _currentWeaponIKProfile._1hDefaultHeadOnly;
            _currentIdleLookAtHelperPos = _currentWeaponIKProfile._1hDefaultLookAtPos;

            HandleIKWeightWithIsHeadOnly(_isCurrentIdleHeadOnly);
            TweenLookAtHelperCurrentIdlePosition();

            Tween_Lh_HelperTargetGoal(_currentWeaponIKProfile._1h_Lh_DefaultGoal);
            Tween_Rh_HelperTargetGoal(_currentWeaponIKProfile._1h_Rh_DefaultGoal);
        }
        #endregion

        #region Tween IK Goals.
        void Tween_Lh_HelperTargetGoal(PlayerIKGoal _playerIKGoal)
        {
            if (LeanTween.isTweening(_leftHandHelperMoveTweenId))
            {
                LeanTween.cancel(_leftHandHelperMoveTweenId);
                LeanTween.cancel(_leftHandHelperRotateTweenId);
            }
            
            _leftHandHelperMoveTweenId = LeanTween.moveLocal(_lhHelper.gameObject, _playerIKGoal._goalLocalPosition, _handHelpersMoveTime).setEase(_handHelpersEaseType).id;
            _leftHandHelperRotateTweenId = LeanTween.rotateLocal(_lhHelper.gameObject, _playerIKGoal._goalLocalEulers, _handHelpersMoveTime).setEase(_handHelpersEaseType).id;
        }
        
        void Tween_Rh_HelperTargetGoal(PlayerIKGoal _playerIKGoal)
        {
            if (LeanTween.isTweening(_rightHandHelperMoveTweenId))
            {
                LeanTween.cancel(_rightHandHelperMoveTweenId);
                LeanTween.cancel(_rightHandHelperRotateTweenId);
            }

            _rightHandHelperMoveTweenId = LeanTween.moveLocal(_rhHelper.gameObject, _playerIKGoal._goalLocalPosition, _handHelpersMoveTime).setEase(_handHelpersEaseType).id;
            _rightHandHelperRotateTweenId = LeanTween.rotateLocal(_rhHelper.gameObject, _playerIKGoal._goalLocalEulers, _handHelpersMoveTime).setEase(_handHelpersEaseType).id;
        }
        #endregion

        #region Modifiy / Maximize / Minimize LookAt IK's Weight.

        #region Maximize.
        void MaximizeHeadIKWeight()
        {
            if (LeanTween.isTweening(_headWeightTweenId))
                LeanTween.cancel(_headWeightTweenId);

            _headWeightTweenId = LeanTween.value(gameObject, _currentHeadIKWeight, 1, _headIKActivateTime).setEase(_headIKEaseType).setOnUpdate((value) => _currentHeadIKWeight = value).setOnComplete(OnCompleteMaximizeHeadIKWeight).id;
        }

        void OnCompleteMaximizeHeadIKWeight()
        {
            _currentHeadIKWeight = 1;
        }

        void MaximizeUpperBodyIKWeight()
        {
            if (LeanTween.isTweening(_bodyWeightTweenId))
                LeanTween.cancel(_bodyWeightTweenId);

            _bodyWeightTweenId = LeanTween.value(gameObject, _currentUpperBodyIKWeight, 1, _bodyIKActivateTime).setEase(_bodyIKEaseType).setOnUpdate((value) => _currentUpperBodyIKWeight = value).setOnComplete(OnCompleteMaximizeUpperBodyIKWeight).id;
        }

        void OnCompleteMaximizeUpperBodyIKWeight()
        {
            _currentUpperBodyIKWeight = 1;
        }
        #endregion

        #region Minimize.
        void MinimizeHeadIKWeight()
        {
            if (LeanTween.isTweening(_headWeightTweenId))
                LeanTween.cancel(_headWeightTweenId);

            _headWeightTweenId = LeanTween.value(gameObject, _currentHeadIKWeight, 0, _headIKDeactivateTime).setEase(_headIKEaseType).setOnUpdate((value) => _currentHeadIKWeight = value).setOnComplete(OnCompleteMinimizeHeadIKWeight).id;
        }

        void OnCompleteMinimizeHeadIKWeight()
        {
            _currentHeadIKWeight = 0;
        }

        void MinimizeUpperBodyIKWeight()
        {
            if (LeanTween.isTweening(_bodyWeightTweenId))
                LeanTween.cancel(_bodyWeightTweenId);

            _bodyWeightTweenId = LeanTween.value(gameObject, _currentUpperBodyIKWeight, 0, _bodyIKDeactivateTime).setEase(_bodyIKEaseType).setOnUpdate((value) => _currentUpperBodyIKWeight = value).setOnComplete(OnCompleteMinimizeUpperBodyIKWeight).id;
        }

        void OnCompleteMinimizeUpperBodyIKWeight()
        {
            _currentUpperBodyIKWeight = 0;
        }
        #endregion

        #region Modifly.
        void ModiflyHeadIKWeight()
        {
            if (LeanTween.isTweening(_headWeightTweenId))
                LeanTween.cancel(_headWeightTweenId);

            _headWeightTweenId = LeanTween.value(gameObject, _currentHeadIKWeight, _nextDesiredLookAtWeight, _headIKActivateTime).setEase(_headIKEaseType).setOnUpdate((value) => _currentHeadIKWeight = value).setOnComplete(OnCompleteModiflyHeadIK).id;
        }

        void OnCompleteModiflyHeadIK()
        {
            _currentHeadIKWeight = _nextDesiredLookAtWeight;
        }

        void ModiflyUpperBodyIKWeight()
        {
            if (LeanTween.isTweening(_bodyWeightTweenId))
                LeanTween.cancel(_bodyWeightTweenId);

            _bodyWeightTweenId = LeanTween.value(gameObject, _currentUpperBodyIKWeight, _nextDesiredLookAtWeight, _bodyIKActivateTime).setEase(_bodyIKEaseType).setOnUpdate((value) => _currentUpperBodyIKWeight = value).setOnComplete(OnCompleteModiflyUpperBodyIK).id;
        }

        void OnCompleteModiflyUpperBodyIK()
        {
            _currentUpperBodyIKWeight = _nextDesiredLookAtWeight;
        }
        #endregion

        void HandleIKWeightWithIsHeadOnly(bool _isHeadOnly)
        {
            if (_isHeadOnly)
            {
                if (isUsingHeadOnlyIK)
                {
                    if (_currentHeadIKWeight != 1)
                    {
                        _nextDesiredLookAtWeight = 1;
                        ModiflyHeadIKWeight();
                    }
                }
                else
                {
                    SetUseHeadOnlyIKToTrue();
                    MaximizeHeadIKWeight();
                }
            }
            else
            {
                if (isUsingUpperBodyIK)
                {
                    if (_currentUpperBodyIKWeight != 1)
                    {
                        _nextDesiredLookAtWeight = 1;
                        ModiflyUpperBodyIKWeight();
                    }
                }
                else
                {
                    SetUseUpperBodyIKToTrue();
                    MaximizeUpperBodyIKWeight();
                }
            }
        }
        #endregion

        #region Handle / Modifly / IK Job Activate Deactivate Hand IK's Weight.

        #region Handle.
        void Handle_Lh_TargetIKGoalWeight(PlayerIKGoal _goal)
        {
            _nextDesireLeftHandMoveWeight = _goal._goalWeight;
            if (_currentLeftHandIKMoveWeight != _nextDesireLeftHandMoveWeight)
            {
                ModiflyLeftHandIKWeight();
            }
        }

        void Handle_Rh_TargetIKGoalWeight(PlayerIKGoal _goal)
        {
            _nextDesireRightHandMoveWeight = _goal._goalWeight;
            if (_currentRightHandIKMoveWeight != _nextDesireRightHandMoveWeight)
            {
                ModiflyRightHandIKWeight();
            }
        }
        #endregion

        #region Modifly.
        void ModiflyLeftHandIKWeight()
        {
            if (LeanTween.isTweening(_leftHandWeightMoveTweenId))
            {
                LeanTween.cancel(_leftHandWeightMoveTweenId);
                LeanTween.cancel(_leftHandWeightRotateTweenId);
            }

            _leftHandWeightMoveTweenId = LeanTween.value(gameObject, _currentLeftHandIKMoveWeight, _nextDesireLeftHandMoveWeight, _handIKActivateTime).setEase(_handIKEaseType).setOnUpdate((value) => _currentLeftHandIKMoveWeight = value).setOnComplete(OnCompleteModiflyLeftHandIKWeight).id;
            _leftHandWeightRotateTweenId = LeanTween.value(gameObject, _currentLeftHandIKRotateWeight, _nextDesireLeftHandMoveWeight, _handIKRotateActivateTime).setEase(_handIKEaseType).setOnUpdate((value) => _currentLeftHandIKRotateWeight = value).id;
        }

        void OnCompleteModiflyLeftHandIKWeight()
        {
            _currentLeftHandIKMoveWeight = _nextDesireLeftHandMoveWeight;
            _currentLeftHandIKRotateWeight = _nextDesireLeftHandMoveWeight;
        }
        
        void ModiflyRightHandIKWeight()
        {
            if (LeanTween.isTweening(_rightHandWeightMoveTweenId))
            {
                LeanTween.cancel(_rightHandWeightMoveTweenId);
                LeanTween.cancel(_rightHandWeightRotateTweenId);
            }

            _rightHandWeightMoveTweenId = LeanTween.value(gameObject, _currentRightHandIKMoveWeight, _nextDesireRightHandMoveWeight, _handIKActivateTime).setEase(_handIKEaseType).setOnUpdate((value) => _currentRightHandIKMoveWeight = value).setOnComplete(OnCompleteModiflyRightHandIKWeight).id;
            _rightHandWeightRotateTweenId = LeanTween.value(gameObject, _currentRightHandIKRotateWeight, _nextDesireRightHandMoveWeight, _handIKRotateActivateTime).setEase(_handIKEaseType).setOnUpdate((value) => _currentRightHandIKRotateWeight = value).id;
        }

        void OnCompleteModiflyRightHandIKWeight()
        {
            _currentRightHandIKMoveWeight = _nextDesireRightHandMoveWeight;
            _currentRightHandIKRotateWeight = _nextDesireRightHandMoveWeight;
        }
        #endregion

        #region IK Job Deactivate / Activate.
        void IKJobDeactivateLeftHandIK()
        {
            if (_currentLeftHandIKMoveWeight != 0)
            {
                _nextDesireLeftHandMoveWeight = 0;
                MinimizeLeftHandIKWeight();
            }

            void MinimizeLeftHandIKWeight()
            {
                if (LeanTween.isTweening(_leftHandWeightMoveTweenId))
                {
                    LeanTween.cancel(_leftHandWeightMoveTweenId);
                    LeanTween.cancel(_leftHandWeightMoveTweenId);
                }

                _leftHandWeightMoveTweenId = LeanTween.value(gameObject, _currentLeftHandIKMoveWeight, 0, _IKJobMinimizeTime).setEase(_handIKEaseType).setOnUpdate((value) => _currentLeftHandIKMoveWeight = value).setOnComplete(OnCompleteMinimizeLeftHandIKWeight).id;
                _leftHandWeightRotateTweenId = LeanTween.value(gameObject, _currentLeftHandIKRotateWeight, 0, _IKJobMinimizeTime).setEase(_handIKEaseType).setOnUpdate((value) => _currentLeftHandIKRotateWeight = value).id;
            }

            void OnCompleteMinimizeLeftHandIKWeight()
            {
                _currentLeftHandIKMoveWeight = 0;
                _currentLeftHandIKRotateWeight = 0;
            }
        }

        void IKJobDeactivateRightHandIK()
        {
            if (_currentRightHandIKMoveWeight != 0)
            {
                _nextDesireRightHandMoveWeight = 0;
                MinimizeRightHandIKWeight();
            }

            void MinimizeRightHandIKWeight()
            {
                if (LeanTween.isTweening(_rightHandWeightMoveTweenId))
                {
                    LeanTween.cancel(_rightHandWeightMoveTweenId);
                    LeanTween.cancel(_rightHandWeightRotateTweenId);
                }

                _rightHandWeightMoveTweenId = LeanTween.value(gameObject, _currentRightHandIKMoveWeight, 0, _IKJobMinimizeTime).setEase(_handIKEaseType).setOnUpdate((value) => _currentRightHandIKMoveWeight = value).setOnComplete(OnCompleteMinimizeRightHandIKWeight).id;
                _rightHandWeightRotateTweenId = LeanTween.value(gameObject, _currentRightHandIKRotateWeight, 0, _IKJobMinimizeTime).setEase(_handIKEaseType).setOnUpdate((value) => _currentRightHandIKRotateWeight = value).id;
            }

            void OnCompleteMinimizeRightHandIKWeight()
            {
                _currentRightHandIKMoveWeight = 0;
                _currentRightHandIKRotateWeight = 0;
            }
        }
        #endregion

        #endregion

        #region Set Status.
        void SetUseHeadOnlyIKToTrue()
        {
            if (!isUsingHeadOnlyIK)
            {
                isUsingHeadOnlyIK = true;
                isUsingUpperBodyIK = false;
                isNotUsingLookAtIK = false;

                /// if Body IK is activated
                if (_currentUpperBodyIKWeight > 0)
                    MinimizeUpperBodyIKWeight();
            }
        }

        void SetUseUpperBodyIKToTrue()
        {
            if (!isUsingUpperBodyIK)
            {
                isUsingHeadOnlyIK = false;
                isUsingUpperBodyIK = true;
                isNotUsingLookAtIK = false;

                /// if Head IK is activated
                if (_currentHeadIKWeight > 0)
                    MinimizeHeadIKWeight();
            }
        }

        void SetUseLookAtIKToFalse()
        {
            isUsingHeadOnlyIK = false;
            isUsingUpperBodyIK = false;
            isNotUsingLookAtIK = true;

            /// if Head IK is activated
            if (_currentHeadIKWeight > 0)
                MinimizeHeadIKWeight();

            /// if Body IK is activated
            if (_currentUpperBodyIKWeight > 0)
                MinimizeUpperBodyIKWeight();
        }
        #endregion

        #region Player Weapon IK Profile.
        public void SetCurrent_RH_WeaponIKProfile()
        {
            switch (_inventory._rightHandWeapon_referedItem.weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    _currentWeaponIKProfile = _axeWeaponIKProfile;
                    break;
                case P_Weapon_WeaponTypeEnum.Fist:
                    _currentWeaponIKProfile = _fistWeaponIKProfile;
                    break;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    break;
                case P_Weapon_WeaponTypeEnum.Shield:
                    _currentWeaponIKProfile = _shieldWeaponIKProfile;
                    break;
            }

            _states._surd_IK_UpperBody_MaxAngle_1H = _currentWeaponIKProfile._surroundIKUpperBodyMaxAngle_1H;
        }

        public void SetCurrent_TH_WeaponIKProfile()
        {
            switch (_inventory._twoHandingWeapon_referedItem.weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    _currentWeaponIKProfile = _axeWeaponIKProfile;
                    break;
                case P_Weapon_WeaponTypeEnum.Fist:
                    _currentWeaponIKProfile = _fistWeaponIKProfile;
                    break;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    break;
                case P_Weapon_WeaponTypeEnum.Shield:
                    _currentWeaponIKProfile = _shieldWeaponIKProfile;
                    break;
            }
        }

        public void SetCurrent_Oppose1_Defense_IKProfile()
        {
            switch (_inventory._leftHandWeapon_referedItem.weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    _currentOppose1DefenseProfile = _currentWeaponIKProfile._axe_oppose1_defense_profile;
                    break;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    break;
                case P_Weapon_WeaponTypeEnum.Shield:
                    _currentOppose1DefenseProfile = _currentWeaponIKProfile._shield_oppose1_defense_profile;
                    break;
            }
        }
        #endregion

        #region Init.
        public void Init(StateManager _states)
        {
            InitManagerRefs(_states);
            
            InitHelpers();
            
            InitHandleIKJobStatus();

            InitSetCurrentLocomotionIKState();
        }

        void InitManagerRefs(StateManager _states)
        {
            this._states = _states;
            _states._playerIKHandler = this;

            anim = _states.anim;
        }
        
        void InitHelpers()
        {
            Transform _IKHelpersHub = transform.GetChild(1);
            lookAtHelper = _IKHelpersHub.GetChild(0);
            _lhHelper = _IKHelpersHub.GetChild(1);
            _rhHelper = _IKHelpersHub.GetChild(2);
        }

        void InitHandleIKJobStatus()
        {
            currentHandleIKJobs.Clear();
        }

        void InitSetCurrentLocomotionIKState()
        {
            _states._currentLocoIKState = _freeForm_1H_LocoIKState;
        }
        #endregion

        #region Post Init StateAction Setup
        public void PostInitStateActionSetup()
        {
            SetCurrent_RH_WeaponIKProfile();

            /// LookAt Pos.
            _isCurrentIdleHeadOnly = _currentWeaponIKProfile._1hDefaultHeadOnly;
            _currentIdleLookAtHelperPos = _currentWeaponIKProfile._1hDefaultLookAtPos;

            lookAtHelper.localPosition = _currentIdleLookAtHelperPos;

            /// IK Goal.
            _currentWeaponIKProfile._1h_Lh_DefaultGoal.OverwriteHelperTransform(_lhHelper);
            _currentWeaponIKProfile._1h_Rh_DefaultGoal.OverwriteHelperTransform(_rhHelper);
        }
        #endregion
        
        #region Handle IK Job.
        public void RegisterNewHandleIKJob(IKUsageType _IKUsageType, float _desiredLookAtWeight, Vector3 _lookAtDesirePosition)
        {
            currentHandleIKJobs.Add(new HandleIKJob(_IKUsageType, _desiredLookAtWeight, _lookAtDesirePosition));
            _isHandleIKJobsEmpty = false;
        }

        void ExecuteNewHandleIKJob()
        {
            switch (currentHandleIKJobs[0]._IKUsageType)
            {
                case IKUsageType.NotUseIK:
                    HandleNotUseIKJob();
                    break;

                case IKUsageType.HeadOnly:
                    HandleHeadOnlyIKJob();

                    if (currentHandleIKJobs[0]._lookAtDesirePosition != _states.vector3Zero)
                        TweenLookAtHelperTargetPosition(currentHandleIKJobs[0]._lookAtDesirePosition);

                    break;

                case IKUsageType.UpperBody:
                    HandleUpperBodyIKJob();

                    if (currentHandleIKJobs[0]._lookAtDesirePosition != _states.vector3Zero)
                        TweenLookAtHelperTargetPosition(currentHandleIKJobs[0]._lookAtDesirePosition);

                    break;

                case IKUsageType.OnlyLeftArm:
                    HandleLeftArmOnlyIKJob();
                    break;
            }

            currentHandleIKJobs.Remove(currentHandleIKJobs[0]);
            anim.SetBool(_states.p_IsHandleIKJobFinished_hash, false);
        }

        public void HandleNotUseIKJob()
        {
            /// LookAt IK.
            if (!isNotUsingLookAtIK)
                SetUseLookAtIKToFalse();

            /// IK Goal.
            IKJobDeactivateLeftHandIK();
            IKJobDeactivateRightHandIK();

            /// Pause LocoIK State Tick.
            _states._isPausingLocoIKStateTick = true;
        }

        public void HandleHeadOnlyIKJob()
        {
            /// LookAt IK.
            if (isUsingHeadOnlyIK)
            {
                if (_currentHeadIKWeight != currentHandleIKJobs[0]._desiredLookAtWeight)
                {
                    _nextDesiredLookAtWeight = currentHandleIKJobs[0]._desiredLookAtWeight;
                    ModiflyHeadIKWeight();
                }
            }
            else
            {
                SetUseHeadOnlyIKToTrue();

                _nextDesiredLookAtWeight = currentHandleIKJobs[0]._desiredLookAtWeight;
                ModiflyHeadIKWeight();
            }

            /// IK Goal.
            IKJobDeactivateLeftHandIK();
            IKJobDeactivateRightHandIK();

            /// Pause LocoIK State Tick.
            _states._isPausingLocoIKStateTick = true;
        }

        public void HandleUpperBodyIKJob()
        {
            /// LookAt IK.
            if (isUsingUpperBodyIK)
            {
                if (_currentUpperBodyIKWeight != currentHandleIKJobs[0]._desiredLookAtWeight)
                {
                    _nextDesiredLookAtWeight = currentHandleIKJobs[0]._desiredLookAtWeight;
                    ModiflyUpperBodyIKWeight();
                }
            }
            else
            {
                SetUseUpperBodyIKToTrue();

                _nextDesiredLookAtWeight = currentHandleIKJobs[0]._desiredLookAtWeight;
                ModiflyUpperBodyIKWeight();
            }

            /// IK Goal.
            IKJobDeactivateLeftHandIK();
            IKJobDeactivateRightHandIK();

            /// Pause LocoIK State Tick.
            _states._isPausingLocoIKStateTick = true;
        }

        public void HandleLeftArmOnlyIKJob()
        {
            /// LookAt IK.
            if (!isNotUsingLookAtIK)
                SetUseLookAtIKToFalse();

            /// IK Goal.
            IKJobDeactivateRightHandIK();

            /// Pause LocoIK State Tick.
            _states._isPausingLocoIKStateTick = true;
        }

        public void OnHandleIKJobFinished()
        {
            _states.ResumeLocoIKStateTick();
        }

        public void SetIsHandleIKJobFinishedToTrue()
        {
            anim.SetBool(_states.p_IsHandleIKJobFinished_hash, true);
        }
        #endregion

        #region Fast Approximately Methods.
        public bool FastApproximately(float a, float b)
        {
            return ((a < b) ? (b - a) : (a - b)) <= 0.005f;
        }

        public bool V3Equal(Vector3 a, Vector3 b)
        {
            return Vector3.SqrMagnitude(a - b) < 0.000001;
        }
        #endregion
    }
}