using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class EgilKinematicMotionAttackMod : AIMod
    {
        /// Custom Inspector Bools.
        [HideInInspector] public bool showEgil_KMA_Mod;
        [HideInInspector] public bool showI_KMA_MotionMonitorHandler;
        [HideInInspector] public bool show_PivotStatus_section;
        [HideInInspector] public bool show_KMJ_section;
        [HideInInspector] public bool show_Switch_section;
        [HideInInspector] public bool show_KMA_Wait_section;
        [HideInInspector] public bool show_KMA_section;
        [HideInInspector] public bool show_TrackPlayer_section;
        [HideInInspector] public bool show_KM_Result_section;
        [HideInInspector] public bool show_KM_Record_section;

        /// I_KMA_MotionMonitorHandler.
        public I_KMA_MotionMonitorHandler _i_KMA_MotionMonitorHandler;
        public FirstPhaseChangeMMH _firstPhaseChangeMMH;
        public SecondPhaseChangeMMH _secondPhaseChangeMMH;
        public Default_KMA_MMH _default_KMA_MMH;

        /// Pivot Status.
        [ReadOnlyInspector] public KMJ_Zone_TypeEnum _cur_KMJ_Zone_Type;
        [ReadOnlyInspector] public float _pivotToPlayerAngle;
        [ReadOnlyInspector] public float _pivotToPlayerSqrDis;

        /// KMJ Goals.
        [Tooltip("Used To Calculate which KMJ Goal is the best for Egil to Jump On.")]
        public Transform _KMJ_Pivot_Transform;
        public Transform _F_KMJ_Goal;
        public Transform _R_KMJ_Goal;
        public Transform _L_KMJ_Goal;
        public Transform _B_KMJ_Goal;

        /// KMJ Jump Motion Value.
        public float _lowerGoalMaxHeightOffset = 0.5f;
        public float _higherGoalMaxHeightOffset = 1f;
        public float _KMJGravity = -31f;
        public float _KMJ_BodyIKRetargetDistance = 10;

        /// KMJ Offsets.
        public float v1JumpPlay2ndHalfOffset = 0.02f;
        public float v2JumpPlay2ndHalfOffset = 0.06f;
        
        /// Switch Points.
        public Transform _switchPoint_1;
        public Transform _switchPoint_2;
        public Transform _switchPoint_3;
        public Transform _switchPoint_4;

        /// Switch Thersholds.
        public float switchZoneSqrDistance = 20.5f;
        public float switchZoneAngle = 60f;

        /// Switch Status.
        [ReadOnlyInspector] public bool _isInPivotGoals;
        [ReadOnlyInspector] public bool _isStartMonitoringSwitch;
        [ReadOnlyInspector] public bool _isSwitchNeeded;

        /// Wait State.
        public State _KMA_WaitState;

        /// Wait Info.
        public float _KMA_NormalWaitRate = 1.5f;
        public float _KMA_NoWait_WaitRate = 0.15f;
        [ReadOnlyInspector] public float _KMA_waitTimer;

        /// Wait Status.
        [ReadOnlyInspector] public bool _is_KMA_WaitNeeded;
        [ReadOnlyInspector] public bool _is_KMA_WaitPaused;

        /// KMA Profiles.
        public Transform _KMA_Goal_Transform;
        public AI_KinematicMotionAttackProfile KMA_Profile_1;   /// Phase 1.
        public AI_KinematicMotionAttackProfile KMA_Profile_2;   /// Phase 1.
        public AI_KinematicMotionAttackProfile KMA_Profile_3;   /// Phase 2.
        public AI_KinematicMotionAttackProfile KMA_Profile_4;   /// Phase 2.
        [ReadOnlyInspector, SerializeField] AI_KinematicMotionAttackProfile _cur_KMA_Profile;

        /// KMA Action Info.
        public float _KMA_staminaUsage = 60;
        public float _KMA_attackPredictRange = 2;
        [ReadOnlyInspector] public bool _is_KMA_PerliousAttack;
        
        /// KMA Physics.
        public float maxFallHeight = 0.1f;
        public float fallGravity = -73;

        /// KM Result.
        [ReadOnlyInspector] public float totalTimeTaken;
        [ReadOnlyInspector] public float zenithTimeTaken;
        [ReadOnlyInspector] public float inAttackRangeApproxTime;
        [ReadOnlyInspector] public float motionTakenTimer;
        
        /// KM Status.
        [ReadOnlyInspector] public bool _isJumpMotion;
        [ReadOnlyInspector] public bool _isMotionStarted;
        [ReadOnlyInspector] public bool _is_KMA_WaitTick_Started;
        [ReadOnlyInspector] public bool _canExit_KMA_State;
        [ReadOnlyInspector] public bool _KMJ_isZenithReached;
        [ReadOnlyInspector] public bool _KMA_isAttackRangeReached;

        /// Player Distance Tracking.
        public float _trackRate = 0.4f;
        public float _applyMovementOffsetThershold = 10;
        [ReadOnlyInspector] public float _trackTimer;
        [ReadOnlyInspector] public float _sqrDisFromLastTracked;
        [ReadOnlyInspector] public Vector3 _lastTrackPlayerPos;
        [ReadOnlyInspector] public bool _hasTrackedForOnce;

        /// KM Record.
        [ReadOnlyInspector] public float _currentGravity;
        [ReadOnlyInspector] public float _currentMaxHeight;
        [ReadOnlyInspector] public Transform _current_KMJ_Goal;

        [ReadOnlyInspector] public bool _startOverlapBoxHitDetect;
        public BoxCollider _bossWeaponCollider;
        
        #region Non Serialized.

        #region Refs.
        [NonSerialized]
        AIStateManager _aiStates;
        [NonSerialized]
        AIManager _ai;
        [NonSerialized]
        StateManager _states;
        [NonSerialized]
        Transform _playerTransform;
        [NonSerialized]
        Transform _pivotTransform;
        #endregion

        #region Temp Data.
        [NonSerialized]
        Vector3 _originalGravity;
        [NonSerialized]
        float _originalInplaceRotateSpeed;
        [NonSerialized]
        Vector3 _vector3Right = new Vector3(0, 1, 0);
        [NonSerialized]
        public float _delta;
        [NonSerialized]
        public LayerMask _playerMask;
        [NonSerialized, ReadOnlyInspector]
        Collider[] hitColliders;
        [NonSerialized]
        Vector3 _bossWeaponColliderHalfSize;
        #endregion

        #endregion

        #region INIT.
        public void Egil_KMA_ModInit(AIManager _ai)
        {
            this._ai = _ai;

            InitRefs();
            InitGetOriginalStatus();
            InitOverlapBoxCollider();

            void InitRefs()
            {
                _aiStates = _ai.aIStates;
                _states = _ai.playerStates;
                _playerTransform = _states.mTransform;
                _pivotTransform = _KMJ_Pivot_Transform.transform;
            }

            void InitGetOriginalStatus()
            {
                _originalGravity = Physics.gravity;
                _originalInplaceRotateSpeed = _ai.inplaceTurningSpeed;
            }

            void InitOverlapBoxCollider()
            {
                _playerMask = 1 << 8;
                hitColliders = new Collider[1];
                _bossWeaponColliderHalfSize = _bossWeaponCollider.size / 2;
            }
        }

        public void Egil_KMA_GoesAggroReset()
        {
            SetIsSwitchNeededToFalse();

            /// KMA Wait Info.
            _KMA_waitTimer = 0;

            /// KMA Wait Status.
            _is_KMA_WaitNeeded = false;
            _is_KMA_WaitPaused = true;

            /// KM Status.
            _isMotionStarted = false;
            _isStartMonitoringSwitch = false;
            _canExit_KMA_State = true;
            _KMJ_isZenithReached = false;
            _KMA_isAttackRangeReached = false;

            /// I KMA MotionMonitorHandler.
            _i_KMA_MotionMonitorHandler = _firstPhaseChangeMMH;

            /// For Test.
            //_i_KMA_MotionMonitorHandler = _default_KMA_MMH;
        }

        public void Egil_KMA_ExitAggroReset()
        {
            if (_is_KMA_WaitTick_Started)
            {
                _is_KMA_WaitTick_Started = false;
                
                _aiStates.e_rb.velocity = _ai.vector3Zero;
                KMJ_SetIsMotionStartToFalse();

                _startOverlapBoxHitDetect = false;
                _bossWeaponCollider.enabled = false;
            }
        }
        #endregion

        #region TICK.

        #region First Phase Change Tick.
        public void FirstPhaseChange_KMA_WaitTick()
        {
            FirstPhaseChangeMMH_MonitorMotionDeltaTime();
        }

        public void FirstPhaseChangeMMH_MonitorMotionDeltaTime()
        {
            if (_isMotionStarted)
            {
                motionTakenTimer += _delta;

                if (!_KMJ_isZenithReached)
                {
                    if (motionTakenTimer >= zenithTimeTaken)
                    {
                        On_KMJ_ZenithPoint();
                    }
                }
                else
                {
                    if (motionTakenTimer >= totalTimeTaken)
                    {
                        motionTakenTimer = 0;
                        On_1stPhaseChange_KMJ_Landed();
                    }
                }
            }
        }
        #endregion

        #region Second Phase Change Tick.
        public void SecondPhaseChange_KMA_WaitTick()
        {
            if (_is_KMA_WaitTick_Started)
            {
                SecondPhaseChange_MonitorMotionDeltaTime();

                if (_isStartMonitoringSwitch)
                {
                    MonitorPivotToPlayerInfo();
                    MonitorPlayerInZoneStatus();
                    ReadyToSwitch_KMJ_Point();
                }

                TrackPlayerDistanceOnInterval();
                Monitor_KMA_WaitCounter();
            }
            
            TickOverlapBoxCollider();
        }

        public void SecondPhaseChange_MonitorMotionDeltaTime()
        {
            if (_isMotionStarted)
            {
                motionTakenTimer += _delta;

                if (_isJumpMotion)
                {
                    if (!_KMJ_isZenithReached)
                    {
                        if (motionTakenTimer >= zenithTimeTaken)
                        {
                            On_KMJ_ZenithPoint();
                        }
                    }
                    else
                    {
                        if (motionTakenTimer >= totalTimeTaken)
                        {
                            motionTakenTimer = 0;
                            On_2ndPhaseChange_KMJ_Landed();
                        }
                    }
                }
                else
                {
                    if (!_KMA_isAttackRangeReached)
                    {
                        if (motionTakenTimer >= inAttackRangeApproxTime)
                        {
                            On_KMA_AttackRange();
                        }
                    }
                    else
                    {
                        if (motionTakenTimer >= totalTimeTaken)
                        {
                            motionTakenTimer = 0;
                            On_2ndPhaseChange_KMA_Landed();
                        }
                    }
                }
            }
        }
        #endregion

        #region Default Tick.
        public void Defualt_KMA_WaitTick()
        {
            if (_is_KMA_WaitTick_Started)
            {
                Default_MonitorMotionDeltaTime();

                if (_isStartMonitoringSwitch)
                {
                    MonitorPivotToPlayerInfo();
                    MonitorPlayerInZoneStatus();
                    ReadyToSwitch_KMJ_Point();
                }

                TrackPlayerDistanceOnInterval();
                Monitor_KMA_WaitCounter();
            }
            
            TickOverlapBoxCollider();
        }
        
        public void Default_MonitorMotionDeltaTime()
        {
            if (_isMotionStarted)
            {
                motionTakenTimer += _delta;

                if (_isJumpMotion)
                {
                    if (!_KMJ_isZenithReached)
                    {
                        if (motionTakenTimer >= zenithTimeTaken)
                        {
                            On_KMJ_ZenithPoint();
                        }
                    }
                    else
                    {
                        if (motionTakenTimer >= totalTimeTaken)
                        {
                            motionTakenTimer = 0;
                            On_KMJ_Land();
                        }
                    }
                }
                else
                {
                    if (!_KMA_isAttackRangeReached)
                    {
                        if (motionTakenTimer >= inAttackRangeApproxTime)
                        {
                            On_KMA_AttackRange();
                        }
                    }
                    else
                    {
                        if (motionTakenTimer >= totalTimeTaken)
                        {
                            motionTakenTimer = 0;
                            On_KMA_Land();
                        }
                    }
                }
            }
        }
        #endregion

        void MonitorPivotToPlayerInfo()
        {
            Vector3 _pivotToPlayerDir = _playerTransform.position - _pivotTransform.position;
            _pivotToPlayerDir.y = _pivotTransform.forward.y;

            UpdatePivotToPlayerAngle();
            UpdatePivotToPlayerDistance();

            void UpdatePivotToPlayerAngle()
            {
                _pivotToPlayerAngle = Vector3.Angle(_pivotToPlayerDir, _pivotTransform.forward);

                if (Vector3.Dot(_pivotToPlayerDir, _pivotTransform.right) < 0)
                {
                    _pivotToPlayerAngle = -_pivotToPlayerAngle;
                }
            }

            void UpdatePivotToPlayerDistance()
            {
                _pivotToPlayerSqrDis = Vector3.SqrMagnitude(_pivotToPlayerDir);
            }
        }

        void MonitorPlayerInZoneStatus()
        {
            if (_isInPivotGoals && _pivotToPlayerSqrDis < switchZoneSqrDistance)
            {
                if (GetIsPlayerInCurrentZoneInAngle())
                {
                    MonitorZoneStatusInAngle();
                }
                else
                {
                    /// Switch To its Own Zone Points.
                    SetGoalBy_FurthestGoalInCurrentZone();
                    SetIsSwitchNeededToTrue();
                }
            }
            else
            {
                MonitorZoneStatusInAngle();
            }
            
            void MonitorZoneStatusInAngle()
            {
                switch (_cur_KMJ_Zone_Type)
                {
                    case KMJ_Zone_TypeEnum.Zone1:
                        MonitorZoneStatus_Zone1();
                        break;
                    case KMJ_Zone_TypeEnum.Zone2:
                        MonitorZoneStatus_Zone2();
                        break;
                    case KMJ_Zone_TypeEnum.Zone3:
                        MonitorZoneStatus_Zone3();
                        break;
                    case KMJ_Zone_TypeEnum.Zone4:
                        MonitorZoneStatus_Zone4();
                        break;
                }

                void MonitorZoneStatus_Zone1()
                {
                    if (_pivotToPlayerAngle < -switchZoneAngle)
                    {   
                        /// Player has entered Zone 4. (Anti-ClockWise)
                        SetGoalBy_AntiClockwiseToNextZone();
                        SetIsSwitchNeededToTrue();
                    }
                    else if (_pivotToPlayerAngle > switchZoneAngle)
                    {
                        /// Player has entered Zone 2. (ClockWise)
                        SetGoalBy_ClockwiseToNextZone();
                        SetIsSwitchNeededToTrue();
                    }
                }

                void MonitorZoneStatus_Zone2()
                {
                    if (_pivotToPlayerAngle < (90 - switchZoneAngle))
                    {
                        /// Player has entered Zone 1. (Anti-ClockWise)
                        SetGoalBy_AntiClockwiseToNextZone();
                        SetIsSwitchNeededToTrue();
                    }
                    else if (_pivotToPlayerAngle > (90 + switchZoneAngle))
                    {
                        /// Player has entered Zone 3. (ClockWise)
                        SetGoalBy_ClockwiseToNextZone();
                        SetIsSwitchNeededToTrue();
                    }
                }

                void MonitorZoneStatus_Zone3()
                {
                    if (_pivotToPlayerAngle > 0)
                    {
                        if (_pivotToPlayerAngle < (90 + switchZoneAngle))
                        {
                            /// Player has entered Zone 2. (Anti-ClockWise)
                            SetGoalBy_AntiClockwiseToNextZone();
                            SetIsSwitchNeededToTrue();
                        }
                    }
                    else
                    {
                        if (_pivotToPlayerAngle > (-180 + switchZoneAngle))
                        {
                            /// Player has entered Zone 4. (ClockWise)
                            SetGoalBy_ClockwiseToNextZone();
                            SetIsSwitchNeededToTrue();
                        }
                    }
                }

                void MonitorZoneStatus_Zone4()
                {
                    if (_pivotToPlayerAngle < (-90 - switchZoneAngle))
                    {
                        /// Player has entered Zone 3. (Anti-ClockWise)
                        SetGoalBy_AntiClockwiseToNextZone();
                        SetIsSwitchNeededToTrue();
                    }
                    else if (_pivotToPlayerAngle > (-90 + switchZoneAngle))
                    {
                        /// Player has entered Zone 1. (ClockWise)
                        SetGoalBy_ClockwiseToNextZone();
                        SetIsSwitchNeededToTrue();
                    }
                }
            }
        }
        
        void ReadyToSwitch_KMJ_Point()
        {
            if (_isSwitchNeeded)
            {
                Switch_KMJ_Point();
            }
        }

        void TrackPlayerDistanceOnInterval()
        {
            _trackTimer += _delta;
            if (_trackTimer >= _trackRate)
            {
                _trackTimer = 0;
                _hasTrackedForOnce = true;
                TrackPlayerDistance();
            }
        }

        void TrackPlayerDistance()
        {
            _sqrDisFromLastTracked = Vector3.SqrMagnitude(_lastTrackPlayerPos - _playerTransform.position);
            _lastTrackPlayerPos = _playerTransform.position;
        }

        void Monitor_KMA_WaitCounter()
        {
            if (!_is_KMA_WaitPaused)
            {
                _KMA_waitTimer += _delta;
                if (_is_KMA_WaitNeeded)
                {
                    if (_KMA_waitTimer >= _KMA_NormalWaitRate)
                    {
                        _is_KMA_WaitNeeded = false;
                        _is_KMA_WaitPaused = true;
                        _KMA_waitTimer = 0;

                        Execute_KMA();
                    }
                }
                else
                {
                    if (_KMA_waitTimer >= _KMA_NoWait_WaitRate)
                    {
                        _is_KMA_WaitPaused = true;
                        _KMA_waitTimer = 0;

                        Execute_KMA();
                    }
                }
            }
        }
        #endregion

        #region KMJ.
        public void Execute_KMJ_ForAttack(_KMA_ActionData _KMA_ActionData)
        {
            _is_KMA_PerliousAttack = _KMA_ActionData.isPerliousAttack;
            _is_KMA_WaitNeeded = _KMA_ActionData._isKMAWaitNeeded;

            _is_KMA_WaitPaused = true;
            _is_KMA_WaitTick_Started = true;

            GetDesired_KMJ_GoalFromPivot();
            On_KMJ_Begin_Get_KMA_Profile();
            On_KMJ_Begin_SetStatus();
            On_KMJ_Begin_LookTowardGoal();
            On_KMJ_Begin_PlayAnim();
            On_KMJ_ChangeState();

            void On_KMJ_Begin_Get_KMA_Profile()
            {
                switch (_KMA_ActionData._KMA_Type)
                {
                    case _KMA_ActionData.KMA_AttackOnIntervalTypeEnum.V1_ByPhase:
                        Get_V1_ProfileWithCurrentPhase();
                        GetMSAComboFromAction();
                        break;
                    case _KMA_ActionData.KMA_AttackOnIntervalTypeEnum.V2_ByPhase:
                        Get_V2_ProfileWithCurrentPhase();
                        GetMSAComboFromAction();
                        break;
                    case _KMA_ActionData.KMA_AttackOnIntervalTypeEnum.RandomByPhase:
                        GetProfileByRandom();
                        GetMSAComboFromRandomProfile();
                        break;
                }

                void Get_V1_ProfileWithCurrentPhase()
                {
                    if (_ai.GetIsIn2ndPhaseBool())
                    {
                        _cur_KMA_Profile = KMA_Profile_3;
                    }
                    else
                    {
                        _cur_KMA_Profile = KMA_Profile_1;
                    }
                }

                void Get_V2_ProfileWithCurrentPhase()
                {
                    if (_ai.GetIsIn2ndPhaseBool())
                    {
                        _cur_KMA_Profile = KMA_Profile_4;
                    }
                    else
                    {
                        _cur_KMA_Profile = KMA_Profile_2;
                    }
                }

                void GetProfileByRandom()
                {
                    int _tempInt = Random.Range(1, 3);

                    if (_ai.GetIsIn2ndPhaseBool())
                    {
                        if (_tempInt == 1)
                        {
                            _cur_KMA_Profile = KMA_Profile_3;
                        }
                        else
                        {
                            _cur_KMA_Profile = KMA_Profile_4;
                        }
                    }
                    else
                    {
                        if (_tempInt == 1)
                        {
                            _cur_KMA_Profile = KMA_Profile_1;
                        }
                        else
                        {
                            _cur_KMA_Profile = KMA_Profile_2;
                        }
                    }
                }

                void GetMSAComboFromAction()
                {
                    if (_KMA_ActionData._isUseCombo)
                    {
                        _ai.currentMultiStageAttack = _KMA_ActionData._AICombo;
                    }
                    else
                    {
                        _ai.currentMultiStageAttack = null;
                    }
                }

                void GetMSAComboFromRandomProfile()
                {
                    if (_KMA_ActionData._isUseCombo)
                    {
                        _ai.currentMultiStageAttack = _cur_KMA_Profile.AICombo;
                    }
                    else
                    {
                        _ai.currentMultiStageAttack = null;
                    }
                }
            }
        }

        #region Phase Change.
        public void Execute_KMJ_Attack_ForPhaseChange()
        {
            _is_KMA_PerliousAttack = true;
            _is_KMA_WaitNeeded = true;
            _is_KMA_WaitPaused = true;

            _is_KMA_WaitTick_Started = true;

            //GetPhaseChangeDesire_KMJ_Goal();
            GetDesired_KMJ_GoalFromPivot();
            On_PhaseChange_KMJ_Begin_Get_KMA_Profile();
            On_KMJ_Begin_SetStatus();
            On_KMJ_Begin_LookTowardGoal();
            On_KMJ_Begin_PlayAnim();
            On_KMJ_ChangeState();

            void On_PhaseChange_KMJ_Begin_Get_KMA_Profile()
            {
                GetProfileByRandom();
                GetMSAComboFromRandomProfile();

                void GetProfileByRandom()
                {
                    int _tempInt = Random.Range(1, 3);

                    if (_tempInt == 1)
                    {
                        _cur_KMA_Profile = KMA_Profile_3;
                    }
                    else
                    {
                        _cur_KMA_Profile = KMA_Profile_4;
                    }
                }
                
                void GetMSAComboFromRandomProfile()
                {
                    _ai.currentMultiStageAttack = _cur_KMA_Profile.AICombo;
                }
            }
        }

        public void Execute_KMJ_ForPhaseChange()
        {
            GetDesired_KMJ_GoalFromClosetSwitchPoints();
            On_KMJ_Begin_SetStatus();
            On_KMJ_Begin_LookTowardGoal();
            On_KMJ_Begin_PlayAnim();
            On_KMJ_ChangeState();
        }
        #endregion

        /// Begin
        void On_KMJ_Begin_SetStatus()
        {
            /// Skipp Attack anim.
            _ai._isSkippingOnHitAnim = true;

            /// Is Grounded.
            _aiStates.isGrounded = true;
            _aiStates.ignoreGroundCheck = true;

            /// Anim Move RM.
            _aiStates.Set_AnimMoveRmType_ToNull();

            /// AI Turning.
            _ai.isPausingTurnWithAgent = true;
            _ai.iKHandler.isUsingIK = false;

            /// Last Track Player Pos.
            _lastTrackPlayerPos = _playerTransform.position;
            _hasTrackedForOnce = false;
            _trackTimer = 0;

            _isJumpMotion = true;
            _KMJ_isZenithReached = false;
            
            _currentGravity = _KMJGravity;
            Calculate_KMJ_GoalMaxHeight();

            _isInPivotGoals = true;
        }
        
        void On_KMJ_Begin_LookTowardGoal()
        {
            float _angle = Vector3.SignedAngle(_aiStates.mTransform.forward, _current_KMJ_Goal.transform.position - _aiStates.mTransform.position, _ai.vector3Up);
            LeanTween.rotateAroundLocal(_aiStates.gameObject, _ai.vector3Up, _angle, 1f).setEaseOutCirc();
        }

        void On_KMJ_Begin_PlayAnim()
        {
            _ai.Play_KMJ_1stHalfAnim();
        }

        void On_KMJ_ChangeState()
        {
            _canExit_KMA_State = false;
            _aiStates.currentState = _KMA_WaitState;
            _aiStates.OnWaitForAnimationEndResets();
        }

        void Calculate_KMJ_GoalMaxHeight()
        {
            if (_current_KMJ_Goal.position.y < _aiStates.mTransform.position.y)
            {
                _currentMaxHeight = _lowerGoalMaxHeightOffset;
            }
            else
            {
                _currentMaxHeight = _current_KMJ_Goal.position.y + _higherGoalMaxHeightOffset;
            }
        }
        
        public void KMJ_ComputeMotionVelocity_InEvent()
        {
            SetIsMotionStartToTrue();
            _aiStates.e_rb.velocity = Compute_KMJ_MotionVelocity();
        }

        /// Zenith
        void On_KMJ_ZenithPoint()
        {
            _KMJ_isZenithReached = true;

            _ai.Play_KMJ_2ndHalfAnim();
        }

        /// Land
        void On_KMJ_Land()
        {
            KMJ_SetIsMotionStartToFalse();
            On_KMJ_Land_PlayAnim();
            On_KMJ_Land_SetStatus();
            On_KMJ_Land_TweenTowardGoal();
        }

        void On_KMJ_Land_PlayAnim()
        {
            _ai.Play_KMJ_LandAnim();
        }

        void On_KMJ_Land_SetStatus()
        {
            if (!_isSwitchNeeded)
            {
                ResetStatusFor_KMJ_Landed();
            }
            else
            {
                ResetStatusFor_KMJ_Switched();
            }

            void ResetStatusFor_KMJ_Landed()
            {
                /// AI Action.
                _ai.skippingScoreCalculation = true;
                _ai.currentAction = null;

                _isStartMonitoringSwitch = true;

                _is_KMA_WaitPaused = false;
            }

            void ResetStatusFor_KMJ_Switched()
            {
                SetIsSwitchNeededToFalse();
            }
        }

        void On_KMJ_Land_TweenTowardGoal()
        {
            LeanTween.move(_aiStates.gameObject, _current_KMJ_Goal.transform.position, 0.25f).setEaseOutSine();
        }
        
        #region 1st Phase Change KMJ Landed.
        public void On_1stPhaseChange_KMJ_Landed()
        {
            KMJ_SetIsMotionStartToFalse();
            On_KMJ_Land_PlayAnim();
            On_KMJ_Land_TweenTowardGoal();
            On_1stPhaseChange_KMJ_Land_OnPhaseChange();
            //On_1stPhaseChange_KMJ_Land_SetCanExit_KMA_State();

            void On_1stPhaseChange_KMJ_Land_OnPhaseChange()
            {
                _ai.OnPhaseChangedResetStatus();
                _ai.iKHandler.TurnOffHeadRigIK();

                LeanTween.value(0, 1, 1.35f).setOnComplete(OnCompletePlayAnim);
                _i_KMA_MotionMonitorHandler = _secondPhaseChangeMMH;

                void OnCompletePlayAnim()
                {
                    _ai.PlayAnimationCrossFade_NoNeglect(_ai.hashManager.egil_Taunt_Chain_1_hash, 0.2f, false);
                }
            }

            //void On_1stPhaseChange_KMJ_Land_SetCanExit_KMA_State()
            //{
            //    _canExit_KMA_State = true;
            //}
        }
        #endregion

        #region 2nd Phase Change KMJ Landed.
        public void On_2ndPhaseChange_KMJ_Landed()
        {
            KMJ_SetIsMotionStartToFalse();
            On_KMJ_Land_PlayAnim();
            ResetStatus_2ndPhaseChange_KMJ_Landed();
            On_KMJ_Land_TweenTowardGoal();

            void ResetStatus_2ndPhaseChange_KMJ_Landed()
            {
                /// AI Action.
                _ai.skippingScoreCalculation = true;
                _ai.currentAction = null;

                _is_KMA_WaitPaused = false;
            }
        }
        #endregion

        #endregion

        #region Switch Point.
        void Switch_KMJ_Point()
        {
            On_Switch_Begin_SetStatus();
            On_KMJ_Begin_LookTowardGoal();
            On_KMJ_Begin_PlayAnim();
        }

        void On_Switch_Begin_SetStatus()
        {
            _isJumpMotion = true;
            _KMJ_isZenithReached = false;

            _currentGravity = _KMJGravity;
            Calculate_KMJ_GoalMaxHeight();

            _isInPivotGoals = false;
        }
        #endregion

        #region KMA.
        void Execute_KMA()
        {
            On_KMA_Begin_ExecuteAIAction();
            On_KMA_Begin_SetStatus();
            GetDesired_KMA_Goal();
            On_KMA_Begin_LookTowardTarget();
            On_KMA_Begin_PlayAnim();
            On_KMA_Begin_ChangeState();
        }

        /// Begin
        void On_KMA_Begin_ExecuteAIAction()
        {
            _ai.currentAttackRefs = _cur_KMA_Profile._aiAttackRefs;

            _ai.DepleteEnemyStamina(_KMA_staminaUsage);

            if (_is_KMA_PerliousAttack)
                _ai.SetUsedPerilousAttackToTrue();
            
            _aiStates.applyControllerCameraYMovement = true;
            _aiStates.applyControllerCameraZoom = true;
        }

        void On_KMA_Begin_SetStatus()
        {
            /// AI Turning.
            _ai.isPausingTurnWithAgent = true;
            _ai.iKHandler.isUsingIK = false;
            _ai.inplaceTurningSpeed = 100;

            _isJumpMotion = false;
            _KMA_isAttackRangeReached = false;

            _isStartMonitoringSwitch = false;

            _currentGravity = fallGravity;
            _currentMaxHeight = maxFallHeight;
        }
        
        void On_KMA_Begin_LookTowardTarget()
        {
            float _angle = Vector3.SignedAngle(_aiStates.mTransform.forward, _KMA_Goal_Transform.position - _aiStates.mTransform.position, _vector3Right);
            LeanTween.rotateAroundLocal(_aiStates.gameObject, _vector3Right, -_angle, 0.2f);
        }

        void On_KMA_Begin_PlayAnim()
        {
            _ai.Play_KMA_Anim(_cur_KMA_Profile.KMA_1stHalf_AnimState.animStateHash);

            if (!_cur_KMA_Profile.isApplyRootMotionInAnim)
            {
                KMA_ComputeMotionVelocity();
            }
        }

        void On_KMA_Begin_ChangeState()
        {
            _canExit_KMA_State = true;
            _aiStates.currentState = GameManager.singleton.bossWaitForAnimState;
        }

        public void KMA_ComputeMotionVelocity()
        {
            SetIsMotionStartToTrue();
            _aiStates.e_rb.velocity = Compute_KMA_MotionVelocity();
            GetInAttackRangeApproxTime();
        }

        /// Attack Range
        void On_KMA_AttackRange()
        {
            _KMA_isAttackRangeReached = true;

            /// Collider.
            _aiStates.e_root_Collider.enabled = true;

            _ai.currentWeapon.rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            _ai.Play_KMA_AttackAnim(_cur_KMA_Profile);
        }
        
        public void On_KMA_AttackFinish()
        {
            _startOverlapBoxHitDetect = false;
            _ai.inplaceTurningSpeed = _originalInplaceRotateSpeed;
            _ai.currentWeapon.rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }

        /// Land
        void On_KMA_Land()
        {
            KMA_SetIsMotionStartToFalse();
            On_KMA_Land_SetStatus();
        }

        void On_KMA_Land_SetStatus()
        {
            /// AI Action.
            _ai.skippingScoreCalculation = false;

            /// Is Grounded.
            _aiStates.ignoreGroundCheck = false;

            _is_KMA_WaitTick_Started = false;
        }

        #region 2nd Phase Change KMA Landed.
        public void On_2ndPhaseChange_KMA_Landed()
        {
            KMA_SetIsMotionStartToFalse();
            On_2ndPhaseChange_KMA_Land_SetStatus();
            
            void On_2ndPhaseChange_KMA_Land_SetStatus()
            {
                On_KMA_Land_SetStatus();
                LevelAreaFxManager.singleton.PlayEgil2ndPhaseSnowFx();
                _i_KMA_MotionMonitorHandler = _default_KMA_MMH;
            }
        }
        #endregion

        /// Anim Event.
        public void KMA_ResetTopDownRotation()
        {
            float _angle = 0;
            if (_aiStates.mTransform.eulerAngles.x > 180)
            {
                _angle = 360 - _aiStates.mTransform.eulerAngles.x;
            }
            else
            {
                _angle = _aiStates.mTransform.eulerAngles.x * -1;
            }

            LeanTween.rotateAroundLocal(_aiStates.gameObject, _vector3Right, _angle, 0.08f);
        }
        #endregion

        #region Get KMJ, KMA Goal.
        void GetPhaseChangeDesire_KMJ_Goal()
        {
            _current_KMJ_Goal = _F_KMJ_Goal;
            _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone1;
        }

        void GetDesired_KMJ_GoalFromPivot()
        {
            Vector3 _dirFromPivotToAI = _aiStates.mTransform.position - _KMJ_Pivot_Transform.position;
            _dirFromPivotToAI.y = 0;

            Vector3 _pivotForwardDir = _KMJ_Pivot_Transform.forward;
            _pivotForwardDir.y = 0;

            float _angle = Vector3.Angle(_pivotForwardDir, _dirFromPivotToAI);
            if (Vector3.Dot(_dirFromPivotToAI, _pivotTransform.right) < 0)
            {
                _angle = -_angle;
            }
            //Debug.Log("_angle = " + _angle);

            if (_angle > 0)
            {
                if (_angle > 90)
                {
                    if (_angle > /*135*/105) ///* normally this should be 135, but I don't want the boss to jump to zone 2 very often, so I here set it 105.
                    {
                        _current_KMJ_Goal = _B_KMJ_Goal;
                        _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone3;
                    }
                    else
                    {
                        _current_KMJ_Goal = _R_KMJ_Goal;
                        _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone2;
                    }
                }
                else
                {
                    if (_angle > /*45*/75) ///* normally this should be 45, but I don't want the boss to jump to zone 2 very often, so I here set it 75.
                    {
                        _current_KMJ_Goal = _R_KMJ_Goal;
                        _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone2;
                    }
                    else
                    {
                        _current_KMJ_Goal = _F_KMJ_Goal;
                        _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone1;
                    }
                }
            }
            else
            {
                if (_angle < -90)
                {
                    if (_angle < -135)
                    {
                        _current_KMJ_Goal = _B_KMJ_Goal;
                        _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone3;
                    }
                    else
                    {
                        _current_KMJ_Goal = _L_KMJ_Goal;
                        _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone4;
                    }
                }
                else
                {
                    if (_angle < -45)
                    {
                        _current_KMJ_Goal = _L_KMJ_Goal;
                        _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone4;
                    }
                    else
                    {
                        _current_KMJ_Goal = _F_KMJ_Goal;
                        _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone1;
                    }
                }
            }
        }

        void GetDesired_KMJ_GoalFromClosetSwitchPoints()
        {
            FindCurrentZone();
            SetGoalBy_ClosetGoalInCurrentZone();

            void FindCurrentZone()
            {
                Vector3 _dirFromPivotToAI = _aiStates.mTransform.position - _KMJ_Pivot_Transform.position;
                _dirFromPivotToAI.y = 0;

                Vector3 _pivotForwardDir = _KMJ_Pivot_Transform.forward;
                _pivotForwardDir.y = 0;

                float _angle = Vector3.Angle(_pivotForwardDir, _dirFromPivotToAI);
                if (Vector3.Dot(_dirFromPivotToAI, _pivotTransform.right) < 0)
                {
                    _angle = -_angle;
                }

                if (_angle > 0)
                {
                    if (_angle > 90)
                    {
                        if (_angle > 135)
                        {
                            _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone3;
                        }
                        else
                        {
                            _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone2;
                        }
                    }
                    else
                    {
                        if (_angle > 45)
                        {
                            _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone2;
                        }
                        else
                        {
                            _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone1;
                        }
                    }
                }
                else
                {
                    if (_angle < -90)
                    {
                        if (_angle < -135)
                        {
                            _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone3;
                        }
                        else
                        {
                            _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone4;
                        }
                    }
                    else
                    {
                        if (_angle < -45)
                        {
                            _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone4;
                        }
                        else
                        {
                            _cur_KMJ_Zone_Type = KMJ_Zone_TypeEnum.Zone1;
                        }
                    }
                }
            }
        }

        void GetDesired_KMA_Goal()
        {
            #region Fwd Direction.
            Vector3 _aiFwdDir = _aiStates.mTransform.forward;
            _aiFwdDir.y = 0;

            Vector3 _playerFwdDir = _playerTransform.forward;
            _playerFwdDir.y = 0;

            Vector3 _playerMoveDir = _states.moveDirection;
            #endregion

            #region Get Diagonal Extra Offset.
            float diagonalExtraOffset = _cur_KMA_Profile._KMA_diagonalExtraOffset;
            if (_playerMoveDir.x < 0.5 || _playerMoveDir.z < 0.5)
            {
                diagonalExtraOffset = 0;
            }
            #endregion

            #region Check Tracking Sqr Distance Result.
            bool _applyMovementOffset = true;
            if (!_hasTrackedForOnce)
            {
                TrackPlayerDistance();
            }

            if (_sqrDisFromLastTracked < _applyMovementOffsetThershold)
            {
                diagonalExtraOffset = 0;
                _applyMovementOffset = false;
            }
            #endregion

            #region Get KMA_ Goal Transform.
            if (Vector3.Dot(_aiFwdDir, _playerFwdDir) < 0)
            {
                /// Facing Opposite, Goal Set in front of player.
                Debug.Log("Facing Opposite, Goal Set in front of player");
                if (_states._isRunning)
                {
                    _KMA_Goal_Transform.position = _playerTransform.forward * (_KMA_attackPredictRange + diagonalExtraOffset);
                    if (_applyMovementOffset)
                    {
                        _KMA_Goal_Transform.position += _playerMoveDir * (_cur_KMA_Profile._KMA_playerRunPosOffset * _states.moveAmount);
                    }
                }
                else
                {
                    _KMA_Goal_Transform.position = _playerTransform.forward * (_KMA_attackPredictRange + diagonalExtraOffset);
                    if (_applyMovementOffset)
                    {
                        _KMA_Goal_Transform.position += _playerMoveDir * (_cur_KMA_Profile._KMA_playerWalkPosOffset * _states.moveAmount);
                    }
                }

                _KMA_Goal_Transform.position += _playerTransform.position;
            }
            else
            {
                /// Facing Same Direction, Goal Set Behind of player.
                Debug.Log("Facing Same Direction, Goal Set Behind of player");
                if (_states._isRunning)
                {
                    _KMA_Goal_Transform.position = (_playerTransform.forward * -1 * (_KMA_attackPredictRange + diagonalExtraOffset)) + _playerTransform.position;
                    if (_applyMovementOffset)
                    {
                        _KMA_Goal_Transform.position += _playerMoveDir * (_cur_KMA_Profile._KMA_playerRunPosOffset * _states.moveAmount);
                    }   
                }
                else
                {
                    _KMA_Goal_Transform.position = (_playerTransform.forward * -1 * (_KMA_attackPredictRange + diagonalExtraOffset)) + _playerTransform.position;
                    if (_applyMovementOffset)
                    {
                        _KMA_Goal_Transform.position += _playerMoveDir * (_cur_KMA_Profile._KMA_playerWalkPosOffset * _states.moveAmount);
                    }
                }
            }
            #endregion
        }
        #endregion

        #region Set Switch Goal.
        bool GetIsPlayerInCurrentZoneInAngle()
        {
            switch (_cur_KMJ_Zone_Type)
            {
                case KMJ_Zone_TypeEnum.Zone1:

                    if (_pivotToPlayerAngle < -switchZoneAngle || _pivotToPlayerAngle > switchZoneAngle)
                    {
                        return true;
                    }
                    return false;

                case KMJ_Zone_TypeEnum.Zone2:

                    if (_pivotToPlayerAngle < (90 - switchZoneAngle) || _pivotToPlayerAngle > (90 + switchZoneAngle))
                    {
                        return true;
                    }
                    return false;

                case KMJ_Zone_TypeEnum.Zone3:

                    if (_pivotToPlayerAngle > 0)
                    {
                        if (_pivotToPlayerAngle < (90 + switchZoneAngle))
                        {
                            return true;
                        }

                        return false;
                    }
                    else
                    {
                        if (_pivotToPlayerAngle > (-180 + switchZoneAngle))
                        {
                            return true;
                        }

                        return false;
                    }
                    
                case KMJ_Zone_TypeEnum.Zone4:

                    if (_pivotToPlayerAngle < (-90 - switchZoneAngle) || _pivotToPlayerAngle > (-90 + switchZoneAngle))
                    {
                        return true;
                    }
                    return false;

                default:
                    return false;
            }
        }

        void SetGoalBy_FurthestGoalInCurrentZone()
        {
            switch (_cur_KMJ_Zone_Type)
            {
                case KMJ_Zone_TypeEnum.Zone1:
                    FindFurthestPoint_Zone1();
                    break;
                case KMJ_Zone_TypeEnum.Zone2:
                    FindFurthestPoint_Zone2();
                    break;
                case KMJ_Zone_TypeEnum.Zone3:
                    FindFurthestPoint_Zone3();
                    break;
                case KMJ_Zone_TypeEnum.Zone4:
                    FindFurthestPoint_Zone4();
                    break;
            }

            void FindFurthestPoint_Zone1()
            {
                Vector3 _dirFromFirstPoint = _playerTransform.position - _switchPoint_1.position;
                _dirFromFirstPoint.y = 0;

                Vector3 dirFromSecondPoint = _playerTransform.position - _switchPoint_4.position;
                dirFromSecondPoint.y = 0;

                if (Vector3.SqrMagnitude(_dirFromFirstPoint) > Vector3.SqrMagnitude(dirFromSecondPoint))
                {
                    _current_KMJ_Goal = _switchPoint_1;
                }
                else
                {
                    _current_KMJ_Goal = _switchPoint_4;
                }
            }

            void FindFurthestPoint_Zone2()
            {
                Vector3 _dirFromFirstPoint = _playerTransform.position - _switchPoint_1.position;
                _dirFromFirstPoint.y = 0;

                Vector3 dirFromSecondPoint = _playerTransform.position - _switchPoint_2.position;
                dirFromSecondPoint.y = 0;

                if (Vector3.SqrMagnitude(_dirFromFirstPoint) > Vector3.SqrMagnitude(dirFromSecondPoint))
                {
                    _current_KMJ_Goal = _switchPoint_1;
                }
                else
                {
                    _current_KMJ_Goal = _switchPoint_2;
                }
            }

            void FindFurthestPoint_Zone3()
            {
                Vector3 _dirFromFirstPoint = _playerTransform.position - _switchPoint_2.position;
                _dirFromFirstPoint.y = 0;

                Vector3 dirFromSecondPoint = _playerTransform.position - _switchPoint_3.position;
                dirFromSecondPoint.y = 0;

                if (Vector3.SqrMagnitude(_dirFromFirstPoint) > Vector3.SqrMagnitude(dirFromSecondPoint))
                {
                    _current_KMJ_Goal = _switchPoint_2;
                }
                else
                {
                    _current_KMJ_Goal = _switchPoint_3;
                }
            }

            void FindFurthestPoint_Zone4()
            {
                Vector3 _dirFromFirstPoint = _playerTransform.position - _switchPoint_3.position;
                _dirFromFirstPoint.y = 0;

                Vector3 dirFromSecondPoint = _playerTransform.position - _switchPoint_4.position;
                dirFromSecondPoint.y = 0;

                if (Vector3.SqrMagnitude(_dirFromFirstPoint) > Vector3.SqrMagnitude(dirFromSecondPoint))
                {
                    _current_KMJ_Goal = _switchPoint_3;
                }
                else
                {
                    _current_KMJ_Goal = _switchPoint_4;
                }
            }
        }

        void SetGoalBy_ClosetGoalInCurrentZone()
        {
            switch (_cur_KMJ_Zone_Type)
            {
                case KMJ_Zone_TypeEnum.Zone1:
                    FindClosetPoint_Zone1();
                    break;
                case KMJ_Zone_TypeEnum.Zone2:
                    FindClosetPoint_Zone2();
                    break;
                case KMJ_Zone_TypeEnum.Zone3:
                    FindClosetPoint_Zone3();
                    break;
                case KMJ_Zone_TypeEnum.Zone4:
                    FindClosetPoint_Zone4();
                    break;
            }

            void FindClosetPoint_Zone1()
            {
                Vector3 _dirFromFirstPoint = _playerTransform.position - _switchPoint_1.position;
                _dirFromFirstPoint.y = 0;

                Vector3 dirFromSecondPoint = _playerTransform.position - _switchPoint_4.position;
                dirFromSecondPoint.y = 0;

                if (Vector3.SqrMagnitude(_dirFromFirstPoint) > Vector3.SqrMagnitude(dirFromSecondPoint))
                {
                    _current_KMJ_Goal = _switchPoint_4;
                }
                else
                {
                    _current_KMJ_Goal = _switchPoint_1;
                }
            }

            void FindClosetPoint_Zone2()
            {
                Vector3 _dirFromFirstPoint = _playerTransform.position - _switchPoint_1.position;
                _dirFromFirstPoint.y = 0;

                Vector3 dirFromSecondPoint = _playerTransform.position - _switchPoint_2.position;
                dirFromSecondPoint.y = 0;

                if (Vector3.SqrMagnitude(_dirFromFirstPoint) > Vector3.SqrMagnitude(dirFromSecondPoint))
                {
                    _current_KMJ_Goal = _switchPoint_2;
                }
                else
                {
                    _current_KMJ_Goal = _switchPoint_1;
                }
            }

            void FindClosetPoint_Zone3()
            {
                Vector3 _dirFromFirstPoint = _playerTransform.position - _switchPoint_2.position;
                _dirFromFirstPoint.y = 0;

                Vector3 dirFromSecondPoint = _playerTransform.position - _switchPoint_3.position;
                dirFromSecondPoint.y = 0;

                if (Vector3.SqrMagnitude(_dirFromFirstPoint) > Vector3.SqrMagnitude(dirFromSecondPoint))
                {
                    _current_KMJ_Goal = _switchPoint_3;
                }
                else
                {
                    _current_KMJ_Goal = _switchPoint_2;
                }
            }

            void FindClosetPoint_Zone4()
            {
                Vector3 _dirFromFirstPoint = _playerTransform.position - _switchPoint_3.position;
                _dirFromFirstPoint.y = 0;

                Vector3 dirFromSecondPoint = _playerTransform.position - _switchPoint_4.position;
                dirFromSecondPoint.y = 0;

                if (Vector3.SqrMagnitude(_dirFromFirstPoint) > Vector3.SqrMagnitude(dirFromSecondPoint))
                {
                    _current_KMJ_Goal = _switchPoint_4;
                }
                else
                {
                    _current_KMJ_Goal = _switchPoint_3;
                }
            }
        }

        /// This happens when play goes from "Zone 1 -> 4" "Zone 4 -> 3" "Zone 3 -> 2" etc.
        /// When Player in Zone 1 and facing the tomb, Player's Left Side is negative degree and Right is Positive.
        void SetGoalBy_AntiClockwiseToNextZone()
        {
            switch (_cur_KMJ_Zone_Type)
            {
                case KMJ_Zone_TypeEnum.Zone1:
                    _current_KMJ_Goal = _switchPoint_4;
                    break;
                case KMJ_Zone_TypeEnum.Zone2:
                    _current_KMJ_Goal = _switchPoint_1;
                    break;
                case KMJ_Zone_TypeEnum.Zone3:
                    _current_KMJ_Goal = _switchPoint_2;
                    break;
                case KMJ_Zone_TypeEnum.Zone4:
                    _current_KMJ_Goal = _switchPoint_3;
                    break;
            }
        }

        /// This happens when play goes from "Zone 4 -> 1" "Zone 1 -> 2" "Zone 2 -> 3" etc.
        void SetGoalBy_ClockwiseToNextZone()
        {
            switch (_cur_KMJ_Zone_Type)
            {
                case KMJ_Zone_TypeEnum.Zone1:
                    _current_KMJ_Goal = _switchPoint_1;
                    break;
                case KMJ_Zone_TypeEnum.Zone2:
                    _current_KMJ_Goal = _switchPoint_2;
                    break;
                case KMJ_Zone_TypeEnum.Zone3:
                    _current_KMJ_Goal = _switchPoint_3;
                    break;
                case KMJ_Zone_TypeEnum.Zone4:
                    _current_KMJ_Goal = _switchPoint_4;
                    break;
            }
        }
        #endregion

        #region Compute Motion Velocity.
        Vector3 Compute_KMJ_MotionVelocity()
        {
            Transform _aiStateTransform = _aiStates.mTransform;

            float deltaY = _current_KMJ_Goal.position.y - _aiStateTransform.position.y;
            Vector3 deltaXZ = new Vector3(_current_KMJ_Goal.position.x - _aiStateTransform.position.x, 0, _current_KMJ_Goal.position.z - _aiStateTransform.position.z);

            float zenithTime = Mathf.Sqrt(-2 * _currentMaxHeight / _currentGravity);
            zenithTimeTaken = zenithTime + GetJump2ndHalfOffsetByType();

            totalTimeTaken = zenithTime + Mathf.Sqrt(2 * -(_currentMaxHeight - deltaY) / _currentGravity);

            Vector3 upVeloctiy = Mathf.Sqrt(-2 * _currentGravity * _currentMaxHeight) * Vector3.up;
            Vector3 sideVelocity = deltaXZ / totalTimeTaken;

            //Debug.Log("result Vel 1 = " + upVeloctiy + sideVelocity);
            return upVeloctiy + sideVelocity;
        }

        Vector3 Compute_KMA_MotionVelocity()
        {
            Transform _aiStateTransform = _aiStates.mTransform;

            float deltaY = _KMA_Goal_Transform.position.y - _aiStateTransform.position.y;
            Vector3 deltaXZ = new Vector3(_KMA_Goal_Transform.position.x - _aiStateTransform.position.x, 0, _KMA_Goal_Transform.position.z - _aiStateTransform.position.z);
            
            totalTimeTaken = Mathf.Sqrt(-2 * _currentMaxHeight / _currentGravity) + Mathf.Sqrt(2 * -(_currentMaxHeight - deltaY) / _currentGravity);

            Vector3 upVeloctiy = Mathf.Sqrt(-2 * _currentGravity * _currentMaxHeight) * Vector3.up;
            Vector3 sideVelocity = deltaXZ / totalTimeTaken;

            //Debug.Log("result Vel 1 = " + upVeloctiy + sideVelocity);
            return upVeloctiy + sideVelocity;
        }
        #endregion

        #region Set Is Motion Start.
        void SetIsMotionStartToTrue()
        {
            _isMotionStarted = true;

            /// Rigidbody.
            _aiStates.e_rb.isKinematic = false;
            _aiStates.e_rb.drag = 0;

            /// Collider.
            _aiStates.e_root_Collider.enabled = false;

            //Time.timeScale = 0.4f;

            /// Gravity.
            Physics.gravity = _currentGravity * Vector3.up;
        }

        void KMJ_SetIsMotionStartToFalse()
        {
            _isMotionStarted = false;

            /// Rigidbody.
            _aiStates.e_rb.isKinematic = true;
            _aiStates.e_rb.drag = 4;
            
            //Time.timeScale = 1f;

            /// Gravity.
            Physics.gravity = _originalGravity;
        }

        void KMA_SetIsMotionStartToFalse()
        {
            _isMotionStarted = false;

            /// Rigidbody.
            _aiStates.e_rb.velocity = _ai.vector3Zero;
            _aiStates.e_rb.drag = 4;

            /// Collider.
            //_aiStates.e_root_Collider.enabled = true;
            
            //Time.timeScale = 1f;

            /// Gravity.
            Physics.gravity = _originalGravity;
        }
        #endregion

        #region Set Is Switch Need.
        void SetIsSwitchNeededToTrue()
        {
            _is_KMA_WaitPaused = true;

            if (_KMA_waitTimer >= _KMA_NormalWaitRate - 0.25f)
                _KMA_waitTimer -= 0.25f;

            _isSwitchNeeded = true;
            _isStartMonitoringSwitch = false;
        }

        void SetIsSwitchNeededToFalse()
        {
            _is_KMA_WaitPaused = false;

            _isSwitchNeeded = false;
        }
        #endregion

        #region Get Motion Offset.
        public float GetJump2ndHalfOffsetByType()
        {
            if (_ai.GetIsIn2ndPhaseBool())
            {
                return v2JumpPlay2ndHalfOffset;
            }
            else
            {
                return v1JumpPlay2ndHalfOffset;
            }
        }

        void GetInAttackRangeApproxTime()
        {
            inAttackRangeApproxTime = totalTimeTaken + _cur_KMA_Profile._2ndHalf_KMA_Offset;
        }
        #endregion

        #region Overlap Box Detection.
        public void TickOverlapBoxCollider()
        {
            if (_startOverlapBoxHitDetect)
            {
                Transform _bossWeaponColliderTransform = _bossWeaponCollider.transform;

                Vector3 targetPos = _bossWeaponColliderTransform.TransformPoint(_bossWeaponCollider.center);

                int hits = Physics.OverlapBoxNonAlloc(targetPos, _bossWeaponColliderHalfSize, hitColliders, _bossWeaponColliderTransform.rotation, _playerMask);
                if (hits > 0)
                {
                    //Debug.Log("Detected Hit To Player");

                    if (_states.isInvincible)
                        return;

                    _bossWeaponCollider.ClosestPoint(_bossWeaponColliderTransform.position);
                    _ai.currentWeapon.On_KMA_OverlapBoxHitPlayer();
                }
            }
        }
        #endregion
        
        /// 2nd PHASE CHANGE.

        public void SetNewPhaseData(Egil_KMA_Mod_EP_Data _egil_KMA_Mod_EP_Data)
        {
            _originalInplaceRotateSpeed = _ai.maxUpperBodyIKTurningSpeed;
            fallGravity = _egil_KMA_Mod_EP_Data._nextPhaseFallGravity;
        }
        
        public enum KMJ_Zone_TypeEnum
        {
            Zone1,
            Zone2,
            Zone3,
            Zone4
        }
    }

    public interface I_KMA_MotionMonitorHandler
    {
        void KMA_WaitTick(EgilKinematicMotionAttackMod _egil_KMA_Mod);
    }
}