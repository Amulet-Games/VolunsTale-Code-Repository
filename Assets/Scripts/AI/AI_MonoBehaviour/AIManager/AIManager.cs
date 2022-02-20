using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SA
{
    [RequireComponent(typeof(AIStateManager))]
    public abstract class AIManager : MonoBehaviour
    {
        #region Enemy Type 
        public EnemyTypeEnum enemyTypeEnum;
        [ReadOnlyInspector] public int volunReturnAmount;
        #endregion

        #region Weapons
        public AISheathTransform firstSheathTransform;                                      // Parent bone: Spine_01

        [ReadOnlyInspector] public EnemyRuntimeWeapon firstWeapon;
        [ReadOnlyInspector] public EnemyRuntimeWeapon currentWeapon;
        [ReadOnlyInspector] public ThrowableEnemyRuntimeWeaponBase currentThrowableWeapon;
        [ReadOnlyInspector] public ThrowableEnemyRuntimeWeaponPool firstThrowableWeaponPool;

        [ReadOnlyInspector] public bool isWeaponOnHand;
        [ReadOnlyInspector] public bool isReacquireThrowableNeeded;
        #endregion

        #region Decisions Variables
        public float aggro_Thershold = 13;
        public float aggro_ClosestThershold = 5;
        public float exitAggro_Thershold = 25;
        public float aggro_Angle = 45;
        public int defaultCrossFadeLayer = 1;
        [ReadOnlyInspector] public int currentCrossFadeLayer;
        [ReadOnlyInspector] public int _frameCount;
        [ReadOnlyInspector] public float _delta;
        [ReadOnlyInspector] public float _aggroTransitTimer;
        [ReadOnlyInspector] public float _idleTransitTimer;
        #endregion

        #region Angle / Direction / Distance
        [Tooltip("The degrees of Angle that is calculated from AIState Transform to current target in every update frame.")]
        [ReadOnlyInspector]
        public float angleToTarget;
        [Tooltip("The value of Distance that is calculated from AIState Transform to current target in every update frame.")]
        [ReadOnlyInspector]
        public float distanceToTarget;
        [Tooltip("The Vector3 of Direction that is calculated from AIState Transform to current target in every update frame.")]
        [ReadOnlyInspector]
        public Vector3 dirToTarget;

        [ReadOnlyInspector]
        public Vector3 targetPos;
        [ReadOnlyInspector]
        public Transform mTransform;
        #endregion

        #region Overall Health
        public float totalEnemyHealth;
        [ReadOnlyInspector] public double currentEnemyHealth;
        #endregion

        #region Hit Source Refs.
        /// Hit Source Refs.
        [ReadOnlyInspector] public Player_AttackRefs _hitSourceAttackRefs;

        /// -> Result.
        [ReadOnlyInspector] public double _previousHitDamage;
        [ReadOnlyInspector] public Transform _hitSourceColliderTransform;

        /// -> Status.
        [ReadOnlyInspector] public bool _isSkippingOnHitAnim;
        [ReadOnlyInspector] public bool _isHitByChargeAttack;

        /// Knocked Down Status.
        [SerializeField] float _onHitKnockdownGetupWaitRate;
        [SerializeField] float _executionGetupWaitRate;
        [ReadOnlyInspector] public bool isKnockedDown;
        [ReadOnlyInspector] public float _currentGetupWaitRate;
        [ReadOnlyInspector, SerializeField] float _getupWaitTimer;

        /// Execution Source Refs.
        [ReadOnlyInspector] public float _previousExecutionDamage;
        [ReadOnlyInspector] public Player_ExecutionProfile _p_executionProfile;

        /// Invincible Status.
        [SerializeField] float _invincibleResetRate = 0.25f;
        [ReadOnlyInspector] public bool _isInvincible;
        [ReadOnlyInspector, SerializeField] float _invincibleResetTimer;
        #endregion

        #region AI Attck Refs.
        /// AI Attack Elemental Type.
        public AIElementalTypeEnum currentElementalType;
        [ReadOnlyInspector] public AI_AttackRefs currentAttackRefs;
        #endregion

        #region Root Motions.
        public float runningPredictAddonValue = 4;
        [ReadOnlyInspector] public float currentPlayerPredictOffset;
        [ReadOnlyInspector] public bool ignoreAttackRootMotionCalculate;
        public float attackMaxVelocityDistance;

        [ReadOnlyInspector] public float currentAttackVelocity;
        public float currentRollVelocity;
        public float currentFallbackVelocity;
        public float currentKnockdownVelocity;
        public float currentExecutionVelocity;
        
        [ReadOnlyInspector] public bool applyTurnRootMotion;
        [ReadOnlyInspector] public bool applyAttackArtifMotion;
        #endregion
        
        #region Turn With Agent.
        // ANIM TURNING
        [Tooltip("Default value is 90, if AI's \"angleToTarget\" exceeded this value and in idle, AI should turn with Root turning animation.")]
        public float animRootRotateThershold = 90;
        public float rootTurningSpeed = 4.5f;

        [Tooltip("Default value is 50, if AI's \"angleToTarget\" exceeded this value and in idle, AI should turn with Inplace turning animation.")]
        public float animInplaceRotateThershold = 50;
        public float inplaceTurningSpeed = 10f;

        // MOVE TOWARD TURNING
        public float maneuverAngularSpeed = 400;
        public float maneuverLookAtPlayerThershold = 45f;
        public float maneuverHeadIKThershold = 25f;

        // IK TURNING
        [Tooltip("Default value is 30, if AI's \"angleToTarget\" exceeded this value and in idle, AI should turn with Head and Body IK with tiny bit of slerping.")]
        public float upperBodyIKRotateThershold = 30;
        public float maxUpperBodyIKTurningSpeed = 2.25f;
        public float minUpperBodyIKTurningSpeed = 0.5f;
        public float maxUpperBodyIKTurningSpeedDis = 1.5f;
        public float minUpperBodyIKTurningSpeedDis = 8f;
        [ReadOnlyInspector] public float currentUpperBodyIKTurningSpeed;
        #endregion

        #region Anim With Agent.
        // Locomotion Value.
        // Move Toward
        public float locoAnimSwitchDistance;
        public float closeDistanecLocoAnimValue;
        public float farDistanceLocoAnimValue;
        
        // Patrol
        public float patrolLocoAnimValue = 0.5f;
        #endregion

        #region Move With Agent.
        // Stop Distance.
        public float agentStopDistance;

        // Speed config.
        public float agentAccelSpeed;
        public float agentMoveSpeed;
        public float closeSpeedBuffer;

        // Speed Status.
        [ReadOnlyInspector] public float _currentAgentAccelSpeed;
        [ReadOnlyInspector] public float _currentAgentMoveSpeed;
        [ReadOnlyInspector] public float _currentAgentVelocity;

        // Move Toward Predict.
        public float predictMoveTowardAmount_h = 7;
        [ReadOnlyInspector] public Vector3 predictedMoveTowardDestn;
        #endregion

        #region LockOn Move Around Stats
        public float lockOnPosUpdateRate;
        public AI_LockonMoveProfile currentAILockonMoveProfile;
        [ReadOnlyInspector] public float lockOnPosUpdateTimer;
        [ReadOnlyInspector] public bool updateLockOnPos;

        [ReadOnlyInspector, SerializeField] float relatPosAngle;
        [ReadOnlyInspector] public LockOnLocomotionTypeEnum currentLockOnLocomotionType;
        [ReadOnlyInspector] public Vector3 targetLockOnPos;
        #endregion
        
        #region Attack Interval Stats
        [Tooltip("The maximum amount of time enemy will need to wait before another attack.")]
        public float maxAttackIntervalRate = 0;
        
        [Tooltip("The minimum amount of time enemy will need to wait before another attack.")]
        public float minAttackIntervalRate = 0;
        
        [Tooltip("True when enemy used attack action, after \"finalizedAttackIntervalRate\" of time passed this will switch back to false.")]
        [ReadOnlyInspector]
        public bool enemyAttacked;

        [Tooltip("The finalized amount of time that enemy will need to wait to peform attack again.")]
        [ReadOnlyInspector]
        public float finalizedAttackIntervalRate = 0;

        [ReadOnlyInspector]
        public float attackIntervalTimer = 0;
        #endregion

        #region Parryable.
        [Tooltip("Is enemy in parryable state currently.")]
        [ReadOnlyInspector]
        public bool _isParryable;

        [Tooltip("Is enemy open for parry execute currently.")]
        [ReadOnlyInspector]
        public bool _isInParryExecuteWindow;

        [ReadOnlyInspector]
        public bool _isParryExecutingEnemy;

        [Tooltip("A counter that will start when player parried this enemy.")]
        [ReadOnlyInspector]
        public float _parryExecuteWaitTimer;

        [Tooltip("A counter that will start after enemy quit parry execute wait.")]
        [ReadOnlyInspector]
        public float _parryExecuteFacedPlayerWaitTimer;
        #endregion
        
        #region Bools
        public bool debugActionHolder;

        /// Init.
        [ReadOnlyInspector] public bool isWeaponUnSheathAnimExecuted;
        [ReadOnlyInspector] public bool isWeaponSheathAnimExecuted;
        [ReadOnlyInspector] public bool isDead;
        [ReadOnlyInspector] public bool isDeadFromSave;

        /// Movement.
        [ReadOnlyInspector] public bool isMovingToward;
        [ReadOnlyInspector] public bool isMovingTowardPlayer;
        [ReadOnlyInspector] public bool isLockOnMoveAround;
        [ReadOnlyInspector] public bool isMovementChanged;

        /// Quit Neglect.
        [ReadOnlyInspector] public bool isMultiStageAttackAvailable;

        /// Turning.
        [ReadOnlyInspector] public bool isTrackingPlayer;
        [ReadOnlyInspector] public bool isPausingTurnWithAgent;
        [ReadOnlyInspector] public bool useInplaceTurningSpeed;
        #endregion

        #region Action
        [ReadOnlyInspector] public AIPassiveAction currentPassiveAction;
        [ReadOnlyInspector] public AIAction currentAction;
        [ReadOnlyInspector] public AI_MultiStageAttack currentMultiStageAttack;
        #endregion

        #region Action Holder
        public AI_ActionHolder firstWeaponActionHolder;
        [ReadOnlyInspector] public AI_ActionHolder currentActionHolder;
        [ReadOnlyInspector] public bool skippingScoreCalculation;
        #endregion

        #region Area Damage Particle FX.
        [ReadOnlyInspector] public AI_DamageParticle _currentDamageParticle;
        #endregion
        
        #region BloodFx Stickable Transforms.
        public Transform _BfxStickableTrans;
        [ReadOnlyInspector] public AI_Poolable_BFX_Handler _cur_PoolableBfxHandler;
        #endregion

        #region Refs.
        [ReadOnlyInspector] public AIStateManager aIStates;
        [ReadOnlyInspector] public AISessionManager aISessionManager;
        [ReadOnlyInspector] public StateManager playerStates;
        [ReadOnlyInspector] public HashManager hashManager;
        [ReadOnlyInspector] public Animator anim;
        [ReadOnlyInspector] public EnemyAnimatorHook a_hook;
        [ReadOnlyInspector] public EnemyIKHandler iKHandler;
        [ReadOnlyInspector] public NavMeshAgent agent;
        #endregion

        #region Drag and Drops.
        public EnemyProfile profile;
        #endregion

        #region Non Serialized.

        #region Anim State Hash

        #region Transition Parameters.
        [HideInInspector] public int e_IsFacedPlayer_hash;
        [HideInInspector] public int e_IsArmed_hash;
        [HideInInspector] public int e_IsLockOnMoveAround_hash;
        [HideInInspector] public int e_IsKnockedDown_hash;
        [HideInInspector] public int e_IsInParryWindow_hash;
        #endregion

        #region General.
        [HideInInspector] public int horizontal_hash;
        [HideInInspector] public int vertical_hash;
        #endregion

        #region On Hit.
        [HideInInspector] public int e_armed_hit_small_r_hash;
        [HideInInspector] public int e_armed_hit_small_l_hash;
        [HideInInspector] public int e_armed_hit_small_f_hash;

        [HideInInspector] public int e_armed_hit_big_r_hash;
        [HideInInspector] public int e_armed_hit_big_l_hash;
        [HideInInspector] public int e_armed_hit_big_f_hash;

        [HideInInspector] public int e_armed_knockDown_hash;
        [HideInInspector] public int e_unarmed_knockDown_hash;

        [HideInInspector] public int e_unarmed_hit_small_r_hash;
        [HideInInspector] public int e_unarmed_hit_small_l_hash;
        [HideInInspector] public int e_unarmed_hit_small_f_hash;

        [HideInInspector] public int e_unarmed_hit_big_r_hash;
        [HideInInspector] public int e_unarmed_hit_big_l_hash;
        [HideInInspector] public int e_unarmed_hit_big_f_hash;

        [HideInInspector] public int e_armed_death_hash;
        [HideInInspector] public int e_unarmed_death_hash;

        /// This will change from runtime.
        [HideInInspector] public int _executionReceiveHash;
        #endregion

        #region Turning.
        [HideInInspector] public int e_unarmed_turn_left_90_hash;
        [HideInInspector] public int e_unarmed_turn_right_90_hash;
        [HideInInspector] public int e_unarmed_turn_left_inplace_hash;
        [HideInInspector] public int e_unarmed_turn_right_inplace_hash;

        [HideInInspector] public int e_armed_turn_left_90_hash;
        [HideInInspector] public int e_armed_turn_right_90_hash;
        [HideInInspector] public int e_armed_turn_left_inplace_hash;
        [HideInInspector] public int e_armed_turn_right_inplace_hash;
        #endregion

        #region Sheath.
        [HideInInspector] public int e_unsheath_First_hash;
        [HideInInspector] public int e_sheath_First_hash;
        #endregion
        
        #endregion

        #region Vector3s.
        [NonSerialized] public Vector3 vector3Zero = new Vector3(0, 0, 0);
        [NonSerialized] public Vector3 vector3One = new Vector3(1, 1, 1);
        [NonSerialized] public Vector3 vector3Up = new Vector3(0, 1, 0);
        #endregion

        #endregion

        #region Custom Inspector
        [HideInInspector] public bool showEnemyTypeEnum;
        [HideInInspector] public bool showEnemyWeapons;
        [HideInInspector] public bool showDecisionsVariables;
        [HideInInspector] public bool showAngleDirectionDistance;
        [HideInInspector] public bool showHealth;
        [HideInInspector] public bool showGetHitRefs;
        [HideInInspector] public bool showAIAttackRefs;
        [HideInInspector] public bool showRootMotionVelocity;
        [HideInInspector] public bool showIKTurning;
        [HideInInspector] public bool showAnimTurningTab;
        [HideInInspector] public bool showAnimWithAgentTab;
        [HideInInspector] public bool showMoveWithAgentTab;
        [HideInInspector] public bool showLockOnMoveAroundStats;
        [HideInInspector] public bool showAttackIntervalStats;
        [HideInInspector] public bool showParryableStats;
        [HideInInspector] public bool showBooleans;
        [HideInInspector] public bool showActions;
        [HideInInspector] public bool showActionHolders;
        [HideInInspector] public bool showAreaDamageParticleFx;
        [HideInInspector] public bool showBfxStickableTransforms;
        [HideInInspector] public bool showReferences;
        [HideInInspector] public bool showDragAndDropReferences;

        /// Used in Sub Class.
        [HideInInspector] public bool showIndicators;
        #endregion

        #region Indicators.
        [ReadOnlyInspector] public bool canPlayIndicator;
        #endregion

        /// Methods.

        #region Setup.
        public void Setup()
        {
            aISessionManager = aIStates._aiSessionManager;
            anim = aIStates.anim;
            agent = aIStates.agent;
            mTransform = aIStates.mTransform;

            playerStates = aISessionManager._playerState;
            volunReturnAmount = aISessionManager._aiGeneralData.GetVolunDropAmount(enemyTypeEnum);

            currentCrossFadeLayer = defaultCrossFadeLayer;
            currentEnemyHealth = totalEnemyHealth;
            distanceToTarget = 20;

            SetupAgentSpeedStatus();
        }

        void SetupAgentSpeedStatus()
        {
            _currentAgentAccelSpeed = agentAccelSpeed;
            _currentAgentMoveSpeed = agentMoveSpeed;
        }

        public void SetupAnimatorHook()
        {
            a_hook = anim.GetComponent<EnemyAnimatorHook>();
            a_hook.Init(this);
        }

        public void SetupBossAnimatorHook()
        {
            a_hook = anim.gameObject.AddComponent<EnemyAnimatorHook>();
            a_hook.Init(this);
        }

        public void SetupAnimHash()
        {
            #region Transition Parameters.
            e_IsFacedPlayer_hash = hashManager.e_IsFacedPlayer_hash;
            e_IsArmed_hash = hashManager.e_IsArmed_hash;
            e_IsLockOnMoveAround_hash = hashManager.e_IsLockOnMoveAround_hash;
            e_IsKnockedDown_hash = hashManager.e_IsKnockedDown_hash;
            e_IsInParryWindow_hash = hashManager.e_IsInParryWindow_hash;
            #endregion

            #region General.
            vertical_hash = hashManager.vertical_hash;
            horizontal_hash = hashManager.horizontal_hash;
            #endregion

            #region On Hit.
            e_armed_hit_small_r_hash = hashManager.e_armed_hit_small_r_hash;
            e_armed_hit_small_l_hash = hashManager.e_armed_hit_small_l_hash;
            e_armed_hit_small_f_hash = hashManager.e_armed_hit_small_f_hash;

            e_armed_hit_big_r_hash = hashManager.e_armed_hit_big_r_hash;
            e_armed_hit_big_l_hash = hashManager.e_armed_hit_big_l_hash;
            e_armed_hit_big_f_hash = hashManager.e_armed_hit_big_f_hash;

            e_armed_knockDown_hash = hashManager.e_armed_knockDown_hash;
            e_armed_death_hash = hashManager.e_armed_death_hash;

            e_unarmed_hit_small_r_hash = hashManager.e_unarmed_hit_small_r_hash;
            e_unarmed_hit_small_l_hash = hashManager.e_unarmed_hit_small_l_hash;
            e_unarmed_hit_small_f_hash = hashManager.e_unarmed_hit_small_f_hash;

            e_unarmed_hit_big_r_hash = hashManager.e_unarmed_hit_big_r_hash;
            e_unarmed_hit_big_l_hash = hashManager.e_unarmed_hit_big_l_hash;
            e_unarmed_hit_big_f_hash = hashManager.e_unarmed_hit_big_f_hash;

            e_unarmed_knockDown_hash = hashManager.e_unarmed_knockDown_hash;
            e_unarmed_death_hash = hashManager.e_unarmed_death_hash;
            #endregion

            #region Turning.
            e_unarmed_turn_left_90_hash = hashManager.e_unarmed_turn_left_90_hash;
            e_unarmed_turn_right_90_hash = hashManager.e_unarmed_turn_right_90_hash;
            e_unarmed_turn_left_inplace_hash = hashManager.e_unarmed_turn_left_inplace_hash;
            e_unarmed_turn_right_inplace_hash = hashManager.e_unarmed_turn_right_inplace_hash;

            e_armed_turn_left_90_hash = hashManager.e_armed_turn_left_90_hash;
            e_armed_turn_right_90_hash = hashManager.e_armed_turn_right_90_hash;
            e_armed_turn_left_inplace_hash = hashManager.e_armed_turn_left_inplace_hash;
            e_armed_turn_right_inplace_hash = hashManager.e_armed_turn_right_inplace_hash;
            #endregion

            #region Sheath.
            e_unsheath_First_hash = hashManager.e_unsheath_First_hash;
            e_sheath_First_hash = hashManager.e_sheath_First_hash;
            #endregion
        }

        public virtual void SetupAIWeapon()
        {
            EnemyWeapon firstEnemyWeapon = GameManager.singleton._resourcesManager.GetEnemyWeapon(profile.firstWeaponId);
            firstWeapon = firstEnemyWeapon.SetupRuntimeWeapon(this);
        }

        public abstract void SetupAIMods();
        #endregion

        #region AI Actions Ticks.
        public abstract void Tick();

        protected virtual void UpdateAggroStateResets()
        {
            Base_UpdateAggroStateResets();
            isLockOnMoveAround = false;
        }

        protected void Base_UpdateAggroStateResets()
        {
            // Movement.
            isMovingToward = false;
            isMovingTowardPlayer = false;

            // Deltas;
            UpdateModsDeltas();
        }

        public virtual void CheckReacquireThrowableWeapon()
        {
            if (isReacquireThrowableNeeded)
            {
                Set_ReacquireFirstThrowable_PassiveAction();
                isReacquireThrowableNeeded = false;
            }
        }

        public void AttackIntervalTimeCount()
        {
            if (enemyAttacked)
            {
                attackIntervalTimer += _delta;
                if (attackIntervalTimer > finalizedAttackIntervalRate)
                {
                    attackIntervalTimer = 0;
                    enemyAttacked = false;
                }
            }
        }
        
        public void GetNextAction()
        {
            if (!aIStates.isGrounded)
                return;

            if (currentPassiveAction != null)
            {
                currentPassiveAction.Execute(this);
                currentPassiveAction = null;
                return;
            }

            if (!skippingScoreCalculation)
            {
                currentAction = currentActionHolder.FindTopScoreAction(this);
            }

            if (currentAction != null)
            {
                currentAction.Execute(this);
            }
        }
        
        public void UpdateAggroStateIK()
        {
            // IK.
            if (isPausingTurnWithAgent || _isInvincible || angleToTarget > animInplaceRotateThershold)
            {
                iKHandler.isUsingIK = false;
            }
            else
            {
                iKHandler.isUsingIK = true;
            }

            iKHandler.isManuverIK = false;
        }

        public virtual void SetEnemyAnim()
        {
            anim.SetBool(e_IsLockOnMoveAround_hash, isLockOnMoveAround);
        }

        protected abstract void UpdateModsDeltas();
        #endregion

        #region isInteracting Ticks.
        public virtual void IsInteracting_Tick()
        {
            UpdateModsDeltas();
            AttackIntervalTimeCount();
            KnockedDownGetupWaitTimeCount();
        }

        public virtual void MonitorMultiStageAttacks()
        {
            if (isMultiStageAttackAvailable && currentMultiStageAttack != null)
            {
                isMultiStageAttackAvailable = false;
                currentMultiStageAttack.Execute(this);
            }
        }
        #endregion

        #region Thershold Checks.
        public bool CheckTargetInRange(float rangeThershold)
        {
            if (distanceToTarget <= rangeThershold)
                return true;

            return false;
        }

        public bool CheckTargetInAngle(DirectionOptionsTypeEnum directionOptions, AngleOptionsTypeEnum angleOptions)
        {
            int optionAngle = GetAngleOptions(angleOptions);
            bool retVal = false;

            switch (directionOptions)
            {
                case DirectionOptionsTypeEnum.front:

                    if (angleToTarget <= (0 + optionAngle))
                        retVal = true;

                    break;
                case DirectionOptionsTypeEnum.right:

                    if (Vector3.Dot(mTransform.right, dirToTarget) < 0)
                        angleToTarget *= -1;

                    if (angleToTarget <= (90 + optionAngle) && angleToTarget >= (90 - optionAngle))
                        retVal = true;

                    break;
                case DirectionOptionsTypeEnum.left:

                    if (Vector3.Dot(mTransform.right, dirToTarget) < 0)
                        angleToTarget *= -1;

                    if (angleToTarget <= (-90 + optionAngle) && angleToTarget >= (-90 - optionAngle))
                        retVal = true;

                    break;
                case DirectionOptionsTypeEnum.back:

                    if (angleToTarget >= (0 + optionAngle))
                        retVal = true;

                    break;
            }

            return retVal;
        }

        public bool CheckIsPlayerFacingEnemy()
        {
            float _dot = Vector3.Dot(iKHandler.aIHeadTrans.forward, Vector3.Normalize(dirToTarget));
            //Debug.Log("_dot = " + _dot);
            if (_dot > 0.4f)
            {
                return true;
            }

            return false;
        }

        public bool CheckTargetIsBehindWall()
        {
            if (_frameCount % 10 == 0)
            {
                RaycastHit hit;
                Vector3 rayStartPos = iKHandler.aIHeadTrans.position;
                Vector3 raycastDir = playerStates.mTransform.position - rayStartPos;

                //Debug.DrawRay(rayStartPos, raycastDir, Color.black);
                if (Physics.Raycast(rayStartPos, raycastDir, out hit, aggro_Thershold, aIStates._layerManager._raycastCheckBehindWallMask))
                {
                    //Debug.Log(hit.transform.gameObject);
                    if (hit.transform.gameObject == playerStates.gameObject)
                    {
                        return false;
                    }
                    else
                    {
                        //Debug.Log("Target is behind wall");
                        return true;
                    }
                }
            }
            
            return true;
        }
        
        int GetAngleOptions(AngleOptionsTypeEnum angleOptions)
        {
            int retVal = 0;

            switch (angleOptions)
            {
                case AngleOptionsTypeEnum.normal_30:
                    retVal = 30;
                    break;
                case AngleOptionsTypeEnum.wide_45:
                    retVal = 45;
                    break;
                case AngleOptionsTypeEnum.ultra_60:
                    retVal = 60;
                    break;
                case AngleOptionsTypeEnum.ultraWide_120:
                    retVal = 120;
                    break;
            }

            return retVal;
        }
        #endregion

        #region Weapons.
        /// Sheathing.
        public virtual void PlaySheathAnimation()
        {
            PlayAnimation(e_sheath_First_hash, false);
        }

        public void SheathCurrentWeaponInAnim()
        {
            if (currentWeapon)
            {
                SheathCurrentWeaponToPosition();
            }
        }

        public virtual void SheathCurrentWeaponToPosition()
        {
            currentWeapon.transform.parent = anim.GetBoneTransform(HumanBodyBones.Spine);
            currentWeapon.transform.localPosition = firstSheathTransform.pos;
            currentWeapon.transform.localEulerAngles = firstSheathTransform.eulers;
            currentWeapon.transform.localScale = firstSheathTransform.scale;

            currentWeapon = null;
            isWeaponOnHand = false;
        }

        public virtual void SheathCurrentSidearmToPosition()
        {
        }

        public virtual void SetupWeaponToPosition(Transform _runtimeWeaponTransform)
        {
            _runtimeWeaponTransform.parent = anim.GetBoneTransform(HumanBodyBones.Spine);
            _runtimeWeaponTransform.localPosition = firstSheathTransform.pos;
            _runtimeWeaponTransform.localEulerAngles = firstSheathTransform.eulers;
            _runtimeWeaponTransform.localScale = firstSheathTransform.scale;
        }

        /// Throwable Weapon.
        public virtual void ClearThrowableWeaponRefs()
        {
            firstWeapon = null;
            isReacquireThrowableNeeded = true;
        }
        
        /// Second Weapon.
        public virtual void ParentSecondWeaponUnderHand()
        {
        }

        public virtual void ParentSecondSideArmUnderHand()
        {
        }

        public virtual void SetCurrentSecondWeaponBeforeAggro()
        {
        }

        public virtual void SetCurrentSecondWeaponAfterAggro()
        {
        }
        
        /// Set Current Weapon.
        public virtual void SetCurrentFirstWeaponBeforeAggro()
        {
            currentWeapon = firstWeapon;
            currentActionHolder = firstWeaponActionHolder;
            
            anim.SetBool(e_IsArmed_hash, true);
        }

        public virtual void SetCurrentFirstWeaponAfterAggro()
        {
            currentWeapon = firstWeapon;
            currentActionHolder = firstWeaponActionHolder;
            
            PlayAnimationCrossFade(e_unsheath_First_hash, 0.2f, true);
            anim.SetBool(e_IsArmed_hash, true);
        }

        /// Boss Setup Set Current ActionHolder.
        public void _1st_SetupBossFirstWeapon(NonThrowableEnemyRuntimeWeapon _bossRuntimeWeapon)
        {
            _bossRuntimeWeapon.gameObject.SetActive(true);
            _bossRuntimeWeapon._ai = this;

            firstWeapon = _bossRuntimeWeapon;
            currentWeapon = _bossRuntimeWeapon;
            isWeaponOnHand = true;

            currentActionHolder = firstWeaponActionHolder;
        }
        #endregion

        #region Play Animations.
        public void PlayAnimationCrossFade(int targetAnimHash, float smoothTime, bool applyTurnRootMotion)
        {
            anim.CrossFade(targetAnimHash, smoothTime, currentCrossFadeLayer);
            anim.SetBool(aIStates.e_IsInteracting_hash, true);

            this.applyTurnRootMotion = applyTurnRootMotion;
        }

        public void PlayAnimation(int targetAnimHash, bool applyTurnRootMotion)
        {
            anim.Play(targetAnimHash, currentCrossFadeLayer);
            anim.SetBool(aIStates.e_IsInteracting_hash, true);

            this.applyTurnRootMotion = applyTurnRootMotion;
        }

        #region Base No Neglect.
        public void PlayAnimationCrossFade_NoNeglect(int targetAnimHash, float smoothTime, bool applyTurnRootMotion)
        {
            anim.CrossFade(targetAnimHash, smoothTime, currentCrossFadeLayer);
            this.applyTurnRootMotion = applyTurnRootMotion;
        }

        public void PlayAnimation_NoNeglect(int targetAnimHash, bool applyTurnRootMotion)
        {
            anim.Play(targetAnimHash, currentCrossFadeLayer);
            this.applyTurnRootMotion = applyTurnRootMotion;
        }

        public void PlayAnimationCrossFade_NoNeglect_NoSpecificLayer(int targetAnimHash, float smoothTime, bool applyTurnRootMotion)
        {
            anim.CrossFade(targetAnimHash, smoothTime);
            this.applyTurnRootMotion = applyTurnRootMotion;
        }

        public void PlayAnimation_NoNeglect_NoSpecificLayer(int targetAnimHash, bool applyTurnRootMotion)
        {
            anim.Play(targetAnimHash);
            this.applyTurnRootMotion = applyTurnRootMotion;
        }

        public void PlayAnimation_NoNeglect_SpecificLayer(int layerIndex, int targetAnimHash, bool applyTurnRootMotion)
        {
            anim.Play(targetAnimHash, layerIndex);
            this.applyTurnRootMotion = applyTurnRootMotion;
        }

        public void PlayAnimation_NoSpecificLayer(int targetAnimHash, bool applyTurnRootMotion)
        {
            anim.Play(targetAnimHash);
            anim.SetBool(aIStates.e_IsInteracting_hash, true);

            this.applyTurnRootMotion = applyTurnRootMotion;
        }
        #endregion

        #region Attack On Interval.
        public virtual void PlayDefaultAttackAnim(BaseAttackOnInterval.AttackAnimStateEnum _targetAnimHash)
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayerTargetAnim();
            SetEnemyAttackedBoolToTrue();

            void PlayerTargetAnim()
            {
                PlayAnimation(GetAttackAnimHash(), true);

                int GetAttackAnimHash()
                {
                    switch (_targetAnimHash)
                    {
                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_1:
                            return hashManager.e_attack_1_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_2:
                            return hashManager.e_attack_2_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_3:
                            return hashManager.e_attack_3_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_4:
                            return hashManager.e_attack_4_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_5:
                            return hashManager.e_attack_5_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_6:
                            return hashManager.e_attack_6_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_7:
                            return hashManager.e_attack_7_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_8:
                            return hashManager.e_attack_8_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_9:
                            return hashManager.e_attack_9_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_1_a:
                            return hashManager.e_combo_1_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_2_a:
                            return hashManager.e_combo_2_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_3_a:
                            return hashManager.e_combo_3_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_4_a:
                            return hashManager.e_combo_4_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_5_a:
                            return hashManager.e_combo_5_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_6_a:
                            return hashManager.e_combo_6_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_7_a:
                            return hashManager.e_combo_7_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_8_a:
                            return hashManager.e_combo_8_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_9_a:
                            return hashManager.e_combo_9_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_10_a:
                            return hashManager.e_combo_10_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_11_a:
                            return hashManager.e_combo_11_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_12_a:
                            return hashManager.e_combo_12_a_hash;

                        default:
                            return 0;
                    }
                }
            }
        }

        public virtual void CrossFadeDefaultAttackAnim(BaseAttackOnInterval.AttackAnimStateEnum _targetAnimHash, float _crossFadeValue)
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayerTargetAnim();
            SetEnemyAttackedBoolToTrue();

            void PlayerTargetAnim()
            {
                PlayAnimationCrossFade(GetAttackAnimHash(), _crossFadeValue, true);

                int GetAttackAnimHash()
                {
                    switch (_targetAnimHash)
                    {
                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_1:
                            return hashManager.e_attack_1_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_2:
                            return hashManager.e_attack_2_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_3:
                            return hashManager.e_attack_3_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_4:
                            return hashManager.e_attack_4_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_5:
                            return hashManager.e_attack_5_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_6:
                            return hashManager.e_attack_6_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_7:
                            return hashManager.e_attack_7_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_8:
                            return hashManager.e_attack_8_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_9:
                            return hashManager.e_attack_9_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_1_a:
                            return hashManager.e_combo_1_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_2_a:
                            return hashManager.e_combo_2_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_3_a:
                            return hashManager.e_combo_3_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_4_a:
                            return hashManager.e_combo_4_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_5_a:
                            return hashManager.e_combo_5_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_6_a:
                            return hashManager.e_combo_6_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_7_a:
                            return hashManager.e_combo_7_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_8_a:
                            return hashManager.e_combo_8_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_9_a:
                            return hashManager.e_combo_9_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_10_a:
                            return hashManager.e_combo_10_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_11_a:
                            return hashManager.e_combo_11_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_12_a:
                            return hashManager.e_combo_12_a_hash;

                        default:
                            return 0;
                    }
                }
            }
        }

        #region Variants.
        public void PlayThrowableAttackAnim(BaseAttackOnInterval.ThrowAttackAnimStateEnum _targetAnimHash, bool _useCrossFade)
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayerTargetAnim();
            SetEnemyAttackedBoolToTrue();

            void PlayerTargetAnim()
            {
                if (_useCrossFade)
                {
                    PlayAnimationCrossFade(GetThrowableAttackAnimHash(), 0.2f, true);
                }
                else
                {
                    PlayAnimation(GetThrowableAttackAnimHash(), true);
                }

                int GetThrowableAttackAnimHash()
                {
                    switch (_targetAnimHash)
                    {
                        case BaseAttackOnInterval.ThrowAttackAnimStateEnum.e_throw_attack_1:
                            return hashManager.e_throw_attack_1_hash;
                        case BaseAttackOnInterval.ThrowAttackAnimStateEnum.e_throw_attack_2:
                            return hashManager.e_throw_attack_2_hash;
                        case BaseAttackOnInterval.ThrowAttackAnimStateEnum.e_throw_attack_3:
                            return hashManager.e_throw_attack_3_hash;
                        default:
                            return 0;
                    }
                }
            }
        }

        public void PlayDpAttackAnim(int _targetDpAttackAnim, AI_DamageParticle _targetAreaDamageParticle)
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayAnimationCrossFade(_targetDpAttackAnim, 0.2f, true);
            SetCurrentDamageParticle(_targetAreaDamageParticle);
            SetEnemyAttackedBoolToTrue();
        }

        public void PlayReturnalProjectileThrowAttackAnim(bool useCrossFade)
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayerTargetAnim();
            SetEnemyAttackedBoolToTrue();

            void PlayerTargetAnim()
            {
                if (useCrossFade)
                {
                    PlayAnimationCrossFade(GetThrowReturnalProjectileHash(), 0.2f, true);
                }
                else
                {
                    PlayAnimation(GetThrowReturnalProjectileHash(), true);
                }
            }
        }

        public void Play_PW_AttackAnim(BaseAttackOnInterval.AttackAnimStateEnum _targetAnimHash)
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayerTargetAnim();
            SetEnemyAttackedBoolToTrue();
            DepletePowerWeaponDuability();

            void PlayerTargetAnim()
            {
                PlayAnimation(GetAttackAnimHash(), true);

                int GetAttackAnimHash()
                {
                    switch (_targetAnimHash)
                    {
                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_1:
                            return hashManager.e_attack_1_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_2:
                            return hashManager.e_attack_2_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_3:
                            return hashManager.e_attack_3_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_4:
                            return hashManager.e_attack_4_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_5:
                            return hashManager.e_attack_5_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_6:
                            return hashManager.e_attack_6_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_7:
                            return hashManager.e_attack_7_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_8:
                            return hashManager.e_attack_8_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_9:
                            return hashManager.e_attack_9_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_1_a:
                            return hashManager.e_combo_1_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_2_a:
                            return hashManager.e_combo_2_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_3_a:
                            return hashManager.e_combo_3_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_4_a:
                            return hashManager.e_combo_4_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_5_a:
                            return hashManager.e_combo_5_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_6_a:
                            return hashManager.e_combo_6_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_7_a:
                            return hashManager.e_combo_7_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_8_a:
                            return hashManager.e_combo_8_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_9_a:
                            return hashManager.e_combo_9_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_10_a:
                            return hashManager.e_combo_10_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_11_a:
                            return hashManager.e_combo_11_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_12_a:
                            return hashManager.e_combo_12_a_hash;

                        default:
                            return 0;
                    }
                }
            }
        }

        public void CrossFade_PW_AttackAnim(BaseAttackOnInterval.AttackAnimStateEnum _targetAnimHash, float _crossFadeValue)
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayerTargetAnim();
            SetEnemyAttackedBoolToTrue();
            DepletePowerWeaponDuability();

            void PlayerTargetAnim()
            {
                PlayAnimationCrossFade(GetAttackAnimHash(), _crossFadeValue, true);

                int GetAttackAnimHash()
                {
                    switch (_targetAnimHash)
                    {
                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_1:
                            return hashManager.e_attack_1_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_2:
                            return hashManager.e_attack_2_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_3:
                            return hashManager.e_attack_3_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_4:
                            return hashManager.e_attack_4_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_5:
                            return hashManager.e_attack_5_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_6:
                            return hashManager.e_attack_6_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_7:
                            return hashManager.e_attack_7_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_8:
                            return hashManager.e_attack_8_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_attack_9:
                            return hashManager.e_attack_9_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_1_a:
                            return hashManager.e_combo_1_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_2_a:
                            return hashManager.e_combo_2_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_3_a:
                            return hashManager.e_combo_3_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_4_a:
                            return hashManager.e_combo_4_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_5_a:
                            return hashManager.e_combo_5_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_6_a:
                            return hashManager.e_combo_6_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_7_a:
                            return hashManager.e_combo_7_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_8_a:
                            return hashManager.e_combo_8_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_9_a:
                            return hashManager.e_combo_9_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_10_a:
                            return hashManager.e_combo_10_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_11_a:
                            return hashManager.e_combo_11_a_hash;

                        case BaseAttackOnInterval.AttackAnimStateEnum.e_combo_12_a:
                            return hashManager.e_combo_12_a_hash;

                        default:
                            return 0;
                    }
                }
            }
        }

        public void PlayExecutionOpeningAnim()
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayAnimationCrossFade(hashManager.e_execution_opening_full_hash, 0.1f, true);
            SetEnemyAttackedBoolToTrue();
        }
        #endregion

        #endregion

        #region MSA Attacks.
        public void PlayRollMultiStageAttackAnim(AI_MultiStageAttack.RollAttackAnimStateEnum _targetAnimHash, bool _useCrossFade)
        {
            isTrackingPlayer = true;
            currentMultiStageAttack = null;

            PlayTargetAnim();
            SetEnemyAttackedBoolToTrue();

            void PlayTargetAnim()
            {
                if (_useCrossFade)
                {
                    PlayAnimationCrossFade(GetRollAttackAnimHash(), 0.1f, true);
                }
                else
                {
                    PlayAnimation(GetRollAttackAnimHash(), true);
                }

                int GetRollAttackAnimHash()
                {
                    switch (_targetAnimHash)
                    {
                        case AI_MultiStageAttack.RollAttackAnimStateEnum.e_roll_attack_1:
                            return hashManager.e_roll_attack_1_hash;
                        case AI_MultiStageAttack.RollAttackAnimStateEnum.e_roll_attack_2:
                            return hashManager.e_roll_attack_2_hash;
                        case AI_MultiStageAttack.RollAttackAnimStateEnum.e_roll_attack_3:
                            return hashManager.e_roll_attack_3_hash;
                        default:
                            return 0;
                    }
                }
            }
        }

        public void PlayAIComboMultiStageAttackAnim(AI_MultiStageAttack.ComboAnimStateEnum _targetAnimHash)
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayAnimation(GetAIComboAnimHash(), true);

            int GetAIComboAnimHash()
            {
                switch (_targetAnimHash)
                {
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_1_b:
                        return hashManager.e_combo_1_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_1_c:
                        return hashManager.e_combo_1_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_1_d:
                        return hashManager.e_combo_1_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_2_b:
                        return hashManager.e_combo_2_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_2_c:
                        return hashManager.e_combo_2_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_2_d:
                        return hashManager.e_combo_2_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_3_b:
                        return hashManager.e_combo_3_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_3_c:
                        return hashManager.e_combo_3_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_3_d:
                        return hashManager.e_combo_3_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_4_b:
                        return hashManager.e_combo_4_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_4_c:
                        return hashManager.e_combo_4_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_4_d:
                        return hashManager.e_combo_4_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_5_b:
                        return hashManager.e_combo_5_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_5_c:
                        return hashManager.e_combo_5_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_5_d:
                        return hashManager.e_combo_5_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_6_b:
                        return hashManager.e_combo_6_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_6_c:
                        return hashManager.e_combo_6_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_6_d:
                        return hashManager.e_combo_6_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_7_b:
                        return hashManager.e_combo_7_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_7_c:
                        return hashManager.e_combo_7_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_7_d:
                        return hashManager.e_combo_7_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_8_b:
                        return hashManager.e_combo_8_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_8_c:
                        return hashManager.e_combo_8_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_8_d:
                        return hashManager.e_combo_8_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_9_b:
                        return hashManager.e_combo_9_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_9_c:
                        return hashManager.e_combo_9_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_9_d:
                        return hashManager.e_combo_9_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_10_b:
                        return hashManager.e_combo_10_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_10_c:
                        return hashManager.e_combo_10_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_10_d:
                        return hashManager.e_combo_10_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_11_b:
                        return hashManager.e_combo_11_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_11_c:
                        return hashManager.e_combo_11_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_11_d:
                        return hashManager.e_combo_11_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_12_b:
                        return hashManager.e_combo_12_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_12_c:
                        return hashManager.e_combo_12_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_12_d:
                        return hashManager.e_combo_12_d_hash;

                    default:
                        return 0;
                }
            }
        }

        public void CrossFadeAIComboMultiStageAttackAnim(AI_MultiStageAttack.ComboAnimStateEnum _targetAnimHash, float _crossFadeValue)
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayAnimationCrossFade(GetAIComboAnimHash(), _crossFadeValue, true);

            int GetAIComboAnimHash()
            {
                switch (_targetAnimHash)
                {
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_1_b:
                        return hashManager.e_combo_1_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_1_c:
                        return hashManager.e_combo_1_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_1_d:
                        return hashManager.e_combo_1_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_2_b:
                        return hashManager.e_combo_2_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_2_c:
                        return hashManager.e_combo_2_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_2_d:
                        return hashManager.e_combo_2_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_3_b:
                        return hashManager.e_combo_3_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_3_c:
                        return hashManager.e_combo_3_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_3_d:
                        return hashManager.e_combo_3_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_4_b:
                        return hashManager.e_combo_4_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_4_c:
                        return hashManager.e_combo_4_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_4_d:
                        return hashManager.e_combo_4_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_5_b:
                        return hashManager.e_combo_5_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_5_c:
                        return hashManager.e_combo_5_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_5_d:
                        return hashManager.e_combo_5_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_6_b:
                        return hashManager.e_combo_6_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_6_c:
                        return hashManager.e_combo_6_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_6_d:
                        return hashManager.e_combo_6_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_7_b:
                        return hashManager.e_combo_7_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_7_c:
                        return hashManager.e_combo_7_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_7_d:
                        return hashManager.e_combo_7_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_8_b:
                        return hashManager.e_combo_8_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_8_c:
                        return hashManager.e_combo_8_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_8_d:
                        return hashManager.e_combo_8_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_9_b:
                        return hashManager.e_combo_9_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_9_c:
                        return hashManager.e_combo_9_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_9_d:
                        return hashManager.e_combo_9_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_10_b:
                        return hashManager.e_combo_10_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_10_c:
                        return hashManager.e_combo_10_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_10_d:
                        return hashManager.e_combo_10_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_11_b:
                        return hashManager.e_combo_11_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_11_c:
                        return hashManager.e_combo_11_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_11_d:
                        return hashManager.e_combo_11_d_hash;

                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_12_b:
                        return hashManager.e_combo_12_b_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_12_c:
                        return hashManager.e_combo_12_c_hash;
                    case AI_MultiStageAttack.ComboAnimStateEnum.e_combo_12_d:
                        return hashManager.e_combo_12_d_hash;

                    default:
                        return 0;
                }
            }
        }

        public void PlayParryAttackMultiStagetAttackAnim(AI_MultiStageAttack.ParryAttackStateEnum _targetAnimHash, bool _useCrossFade)
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;
            currentMultiStageAttack = null;

            PlayTargetAnim();
            SetEnemyAttackedBoolToTrue();
            SetIsWaitingToParryBool(false);
            
            void PlayTargetAnim()
            {
                if (_useCrossFade)
                {
                    PlayAnimationCrossFade(GetParryAttackAnimHash(), 0.1f, true);
                }
                else
                {
                    PlayAnimation(GetParryAttackAnimHash(), true);
                }

                int GetParryAttackAnimHash()
                {
                    switch (_targetAnimHash)
                    {
                        case AI_MultiStageAttack.ParryAttackStateEnum.e_parry_attack_1:
                            return hashManager.e_parry_attack_1_hash;
                        case AI_MultiStageAttack.ParryAttackStateEnum.e_RS_parry_attack_1:
                            return hashManager.e_RS_parry_attack_1_hash;
                        case AI_MultiStageAttack.ParryAttackStateEnum.e_LS_parry_attack_1:
                            return hashManager.e_LS_parry_attack_1_hash;
                        default:
                            return 0;
                    }
                }
            }
        }
        
        public void Play_MSA_ExecutionOpeningAnim()
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayAnimationCrossFade(hashManager.e_execution_opening_full_hash, 0.1f, true);
        }
        #endregion

        #region Roll On Interval.
        public void Play2DRollAnimation(Vector2 _randomDirection)
        {
            isTrackingPlayer = true;
            
            anim.SetFloat(vertical_hash, _randomDirection.y);
            anim.SetFloat(horizontal_hash, _randomDirection.x);

            PlayAnimationCrossFade(hashManager.e_roll_tree_hash, 0.2f, true);
        }

        public void PlayTwoStanceRollAnimation()
        {
            isTrackingPlayer = true;

            anim.SetFloat(vertical_hash, 0);
            anim.SetFloat(horizontal_hash, 0);

            if (GetIsRightStanceBool())
                PlayAnimationCrossFade(hashManager.e_RS_roll_1_hash, 0.2f, true);
            else
                PlayAnimationCrossFade(hashManager.e_LS_roll_1_hash, 0.2f, true);
        }

        public void Play1DRollAnimation(RollOnInterval.RollAnimStateEnum targetRollAnim)
        {
            isTrackingPlayer = true;

            anim.SetFloat(vertical_hash, 0);
            anim.SetFloat(horizontal_hash, 0);

            switch (targetRollAnim)
            {
                case RollOnInterval.RollAnimStateEnum.e_roll_1:
                    PlayAnimationCrossFade(hashManager.e_roll_1_hash, 0.2f, true);
                    break;
                case RollOnInterval.RollAnimStateEnum.e_roll_2:
                    PlayAnimationCrossFade(hashManager.e_roll_2_hash, 0.2f, true);
                    break;
                case RollOnInterval.RollAnimStateEnum.e_roll_3:
                    PlayAnimationCrossFade(hashManager.e_roll_3_hash, 0.2f, true);
                    break;
            }
        }
        #endregion

        #region Execution Presents.
        public void PlayExecutionPresentAnim(int _executionPresentHash)
        {
            PlayAnimationCrossFade(_executionPresentHash, 0.1f, false);
        }
        #endregion

        #region Kinematic Motion Jump / Attack.

        public virtual void Play_KMJ_1stHalfAnim()
        {
        }

        public virtual void Play_KMJ_2ndHalfAnim()
        {
        }

        public virtual void Play_KMJ_LandAnim()
        {
        }

        #region KMA.
        public void Play_KMA_Anim(int _KMA_Hash)
        {
            anim.CrossFade(_KMA_Hash, 0.1f, currentCrossFadeLayer);
            anim.SetBool(aIStates.e_IsInteracting_hash, true);

            _isSkippingOnHitAnim = true;
            aIStates.Set_AnimMoveRmType_ToNull();
        }

        public void Play_KMA_AttackAnim(AI_KinematicMotionAttackProfile _profile)
        {
            isTrackingPlayer = true;
            /// applyTurnRootMotion = true; is set in event.

            currentPlayerPredictOffset = _profile._KMA_attackTurningPredict;

            anim.Play(_profile.KMA_2ndHalf_AnimState.animStateHash, currentCrossFadeLayer);
        }
        #endregion

        #endregion

        #region Ready.
        public void PlayRollAttackReadyAnimTwoDimension(RollAttackReady.RollAttackReadyAnimStateEnum _targetAnimHash, Vector2 _dashDirection)
        {
            anim.SetFloat(vertical_hash, _dashDirection.y);
            anim.SetFloat(horizontal_hash, _dashDirection.x);

            _isSkippingOnHitAnim = true;

            PlayTargetAnim();

            void PlayTargetAnim()
            {
                PlayAnimationCrossFade(GetRollAttackReadyAnimHash(), 0.2f, true);

                int GetRollAttackReadyAnimHash()
                {
                    switch (_targetAnimHash)
                    {
                        case RollAttackReady.RollAttackReadyAnimStateEnum.e_roll_attack_1_ready:
                            return hashManager.e_roll_attack_1_ready_hash;

                        case RollAttackReady.RollAttackReadyAnimStateEnum.e_roll_attack_2_ready:
                            return hashManager.e_roll_attack_2_ready_hash;

                        case RollAttackReady.RollAttackReadyAnimStateEnum.e_roll_attack_3_ready:
                            return hashManager.e_roll_attack_3_ready_hash;

                        case RollAttackReady.RollAttackReadyAnimStateEnum.e_roll_attack_1_ready_roll_tree:
                            return hashManager.e_roll_attack_1_ready_roll_tree_hash;

                        case RollAttackReady.RollAttackReadyAnimStateEnum.e_roll_attack_2_ready_roll_tree:
                            return hashManager.e_roll_attack_2_ready_roll_tree_hash;

                        case RollAttackReady.RollAttackReadyAnimStateEnum.e_roll_attack_3_ready_roll_tree:
                            return hashManager.e_roll_attack_3_ready_roll_tree_hash;

                        default:
                            return 0;
                    }
                }
            }
        }

        public virtual void PlayRollAttackReadyAnim(RollAttackReady.RollAttackReadyAnimStateEnum _targetAnimHash)
        {
            anim.SetFloat(vertical_hash, 0);
            anim.SetFloat(horizontal_hash, 0);

            _isSkippingOnHitAnim = true;

            PlayTargetAnim();

            void PlayTargetAnim()
            {
                PlayAnimationCrossFade(GetRollAttackReadyAnimHash(), 0.2f, true);

                int GetRollAttackReadyAnimHash()
                {
                    switch (_targetAnimHash)
                    {
                        case RollAttackReady.RollAttackReadyAnimStateEnum.e_roll_attack_1_ready:
                            return hashManager.e_roll_attack_1_ready_hash;

                        case RollAttackReady.RollAttackReadyAnimStateEnum.e_roll_attack_2_ready:
                            return hashManager.e_roll_attack_2_ready_hash;

                        case RollAttackReady.RollAttackReadyAnimStateEnum.e_roll_attack_3_ready:
                            return hashManager.e_roll_attack_3_ready_hash;

                        case RollAttackReady.RollAttackReadyAnimStateEnum.e_roll_attack_1_ready_roll_tree:
                            return hashManager.e_roll_attack_1_ready_roll_tree_hash;

                        case RollAttackReady.RollAttackReadyAnimStateEnum.e_roll_attack_2_ready_roll_tree:
                            return hashManager.e_roll_attack_2_ready_roll_tree_hash;

                        case RollAttackReady.RollAttackReadyAnimStateEnum.e_roll_attack_3_ready_roll_tree:
                            return hashManager.e_roll_attack_3_ready_roll_tree_hash;

                        default:
                            return 0;
                    }
                }
            }
        }

        public void PlayParryAttackReadyAnim(ParryAttackReady.ParryReadyAnimStateEnum _targetAnimHash, bool useCrossFade)
        {
            if (useCrossFade)
            {
                PlayAnimationCrossFade(GetParryReadyAnimHash(), 0.2f, true);
            }
            else
            {
                PlayAnimation(GetParryReadyAnimHash(), true);
            }

            int GetParryReadyAnimHash()
            {
                switch (_targetAnimHash)
                {
                    case ParryAttackReady.ParryReadyAnimStateEnum.e_parry_attack_1_ready:
                        return hashManager.e_parry_attack_1_ready_hash;
                    case ParryAttackReady.ParryReadyAnimStateEnum.e_RS_parry_attack_1_ready:
                        return hashManager.e_RS_parry_attack_1_ready_hash;
                    case ParryAttackReady.ParryReadyAnimStateEnum.e_LS_parry_attack_1_ready:
                        return hashManager.e_LS_parry_attack_1_ready_hash;
                    default:
                        return 0;
                }
            }
        }

        public void PlayAimingPlayerReadyAnim(AimingPlayerReady.StartAimingAnimStateEnum _targetAnimHash, bool useCrossFade)
        {
            if (useCrossFade)
            {
                PlayAnimationCrossFade(GetAimingPlayerReadyAnimHash(), 0.2f, true);
            }
            else
            {
                PlayAnimation(GetAimingPlayerReadyAnimHash(), true);
            }

            int GetAimingPlayerReadyAnimHash()
            {
                switch (_targetAnimHash)
                {
                    case AimingPlayerReady.StartAimingAnimStateEnum.e_aim_start:
                        return hashManager.e_aim_start_hash;
                    case AimingPlayerReady.StartAimingAnimStateEnum.e_aim_loop:
                        return hashManager.e_aim_loop_hash;
                    default:
                        return 0;
                }
            }
        }
        #endregion
        
        #region Fall Back.
        public void PlayFallBackAnim(int _AnimHash)
        {
            On_Fallback_SetRootMotions();
            isPausingTurnWithAgent = true;

            PlayAnimation(_AnimHash, true);
        }
        #endregion

        #region Knock Down.
        public void PlaySpecialArmedKnockDownAnim(int _AnimHash)
        {
            On_Knockdown_SetRootMotions();
            isPausingTurnWithAgent = true;

            isKnockedDown = true;
            anim.SetBool(e_IsKnockedDown_hash, true);
            _currentGetupWaitRate = _onHitKnockdownGetupWaitRate;

            PlayAnimation(_AnimHash, false);
        }

        protected void Play_Defualt_ArmedKnockDownAnim()
        {
            On_Knockdown_SetRootMotions();
            isPausingTurnWithAgent = true;

            isKnockedDown = true;
            anim.SetBool(e_IsKnockedDown_hash, true);
            _currentGetupWaitRate = _onHitKnockdownGetupWaitRate;

            PlayAnimation(e_armed_knockDown_hash, false);
        }

        protected void Play_Defualt_UnarmedKnockDownAnim()
        {
            On_Knockdown_SetRootMotions();
            isPausingTurnWithAgent = true;

            isKnockedDown = true;
            anim.SetBool(e_IsKnockedDown_hash, true);
            _currentGetupWaitRate = _onHitKnockdownGetupWaitRate;

            PlayAnimation(e_unarmed_knockDown_hash, false);
        }
        #endregion

        #region Death.
        public void NoSpecificLayer_PlayDeathAnim(int _AnimHash)
        {
            CancelAllRootMotions();
            isPausingTurnWithAgent = true;

            PlayAnimation_NoSpecificLayer(_AnimHash, false);
            anim.SetBool(hashManager.e_IsDead_hash, true);
        }

        protected void Default_PlayArmedDeathAnim()
        {
            CancelAllRootMotions();
            isPausingTurnWithAgent = true;

            PlayAnimation(e_armed_death_hash, false);
            anim.SetBool(hashManager.e_IsDead_hash, true);
        }
        
        protected void Default_PlayUnarmedDeathAnim()
        {
            CancelAllRootMotions();
            isPausingTurnWithAgent = true;

            PlayAnimation(e_unarmed_death_hash, false);
            anim.SetBool(hashManager.e_IsDead_hash, true);
        }

        protected void PowerWeapon_PlayArmedDeathAnim()
        {
            CancelAllRootMotions();
            isPausingTurnWithAgent = true;

            anim.Play(e_armed_death_hash);

            anim.SetBool(aIStates.e_IsInteracting_hash, true);
            anim.SetBool(hashManager.e_IsDead_hash, true);
        }

        protected void PowerWeapon_PlayUnarmedDeathAnim()
        {
            CancelAllRootMotions();
            isPausingTurnWithAgent = true;

            anim.Play(e_unarmed_death_hash, 3);

            anim.SetBool(aIStates.e_IsInteracting_hash, true);
            anim.SetBool(hashManager.e_IsDead_hash, true);
        }

        protected void PowerWeapon_PlayPowerWeaponDeathAnim()
        {
            CancelAllRootMotions();
            isPausingTurnWithAgent = true;

            anim.Play(hashManager.e_pw_death_hash);

            anim.SetBool(aIStates.e_IsInteracting_hash, true);
            anim.SetBool(hashManager.e_IsDead_hash, true);
        }
        #endregion

        #region Others.
        public void PlayAimAttackAnim(ThrowAimingProjectile.AimAttackAnimStateEnum _targetAnimHash, bool _useCrossFade)
        {
            if (_useCrossFade)
            {
                PlayAnimationCrossFade(GetAimAttackAnimHash(), 0.2f, true);
            }
            else
            {
                PlayAnimation(GetAimAttackAnimHash(), true);
            }

            int GetAimAttackAnimHash()
            {
                switch (_targetAnimHash)
                {
                    case ThrowAimingProjectile.AimAttackAnimStateEnum.e_aim_attack_1:
                        return hashManager.e_aim_attack_1_hash;
                    case ThrowAimingProjectile.AimAttackAnimStateEnum.e_aim_attack_2:
                        return hashManager.e_aim_attack_2_hash;
                    default:
                        return 0;
                }
            }
        }

        public void PlayTauntPlayerAnim(TauntPlayer.TauntAnimStateEnum _targetAnimHash, bool _useCrossFade)
        {
            isTrackingPlayer = true;
            if (_useCrossFade)
            {
                PlayAnimationCrossFade(GetTauntAnimHash(), 0.2f, true);
            }
            else
            {
                PlayAnimation(GetTauntAnimHash(), true);
            }

            int GetTauntAnimHash()
            {
                switch (_targetAnimHash)
                {
                    case TauntPlayer.TauntAnimStateEnum.e_taunt_1:
                        return hashManager.e_taunt_1_hash;
                    case TauntPlayer.TauntAnimStateEnum.e_taunt_2:
                        return hashManager.e_taunt_2_hash;
                    case TauntPlayer.TauntAnimStateEnum.e_taunt_3:
                        return hashManager.e_taunt_3_hash;
                    default:
                        return 0;
                }
            }
        }

        public void PlayParryReceivedAnim()
        {
            /// Unless the enemy is left handed, parry recevied anim should be played on right start.
            PlayAnimationCrossFade_NoNeglect(hashManager.e_parry_received_start_r_hash, 0.1f, false);
        }

        public void PlayEgilRevengeAttackAnim()
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayAnimationCrossFade(hashManager.egil_injured_revenge_attack_1_hash, 0.1f, false);
            SetEnemyAttackedBoolToTrue();
        }
        #endregion

        #endregion

        #region Update Dir.Angle.Dis.
        public void UpdateDirAngleDisTarget()
        {
            UpdateCurrentTargetPos();

            // DIRECTION
            Vector3 _dirToTarget = targetPos - mTransform.position;
            _dirToTarget.y = 0;
            if (_dirToTarget == vector3Zero)
                _dirToTarget = mTransform.forward;

            dirToTarget = _dirToTarget;

            // DISTANCE
            distanceToTarget = Vector3.Magnitude(_dirToTarget);

            // ANGLE
            angleToTarget = Vector3.Angle(mTransform.forward, dirToTarget);
        }

        protected virtual void UpdateCurrentTargetPos()
        {
            targetPos = playerStates.mTransform.position;
        }

        public void UpdateDirAngleDisPlayer()
        {
            targetPos = playerStates.mTransform.position;

            // DIRECTION
            Vector3 _dirToTarget = targetPos - mTransform.position;
            _dirToTarget.y = 0;
            if (_dirToTarget == vector3Zero)
                _dirToTarget = mTransform.forward;

            dirToTarget = _dirToTarget;

            // DISTANCE
            distanceToTarget = Vector3.Magnitude(dirToTarget);

            // ANGLE
            if (distanceToTarget < aggro_Thershold)
            {
                angleToTarget = Vector3.Angle(mTransform.forward, dirToTarget);
            }
        }
        #endregion
        
        #region Turn With Agent - Before Aggro.
        public void FacePlayerWithAgent()
        {
            // if the current angle to target exceeded animation turning thershold...
            if (angleToTarget >= animInplaceRotateThershold && !isWeaponUnSheathAnimExecuted && !isWeaponSheathAnimExecuted)
            {
                if (angleToTarget >= animRootRotateThershold)
                {
                    FacePlayerWithRootAnimation();
                }
                else
                {
                    useInplaceTurningSpeed = true;
                    FacePlayerWithInplaceAnimation();
                }
            }
        }

        void FacePlayerWithRootAnimation()
        {
            if (IsTargetOnLeftSide())
            {
                PlayAnimation_NoNeglect(e_unarmed_turn_left_90_hash, true);
            }
            else
            {
                PlayAnimation_NoNeglect(e_unarmed_turn_right_90_hash, true);
            }
        }

        void FacePlayerWithInplaceAnimation()
        {
            if (IsTargetOnLeftSide())
            {
                PlayAnimation_NoNeglect(e_unarmed_turn_left_inplace_hash, true);
            }
            else
            {
                PlayAnimation_NoNeglect(e_unarmed_turn_right_inplace_hash, true);
            }
        }

        protected bool IsTargetOnLeftSide()
        {
            if (Vector3.Dot(mTransform.right, dirToTarget) < 0)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Turn With Agent - After Aggro.

        #region Tick.
        public virtual void TurnWithAgent()
        {
            if (isPausingTurnWithAgent)
                return;

            CalculatingCurrentUpperBodyIKTurningSpeed();

            if (isMovingToward)
            {
                TurningWhileManeuvering();
            }
            else if (isLockOnMoveAround)
            {
                TurningWhileLockonMoveAround();
            }
            else
            {
                TurningWhileIdleWithAnim();
            }
        }

        ///* Calculate Current UpperBody IK Turning Speed.
        protected void CalculatingCurrentUpperBodyIKTurningSpeed()
        {
            if (distanceToTarget < maxUpperBodyIKTurningSpeedDis)
            {
                currentUpperBodyIKTurningSpeed = maxUpperBodyIKTurningSpeed;
            }
            else if (distanceToTarget > minUpperBodyIKTurningSpeedDis)
            {
                currentUpperBodyIKTurningSpeed = minUpperBodyIKTurningSpeed;
            }
            else
            {
                currentUpperBodyIKTurningSpeed = (distanceToTarget / minUpperBodyIKTurningSpeedDis * (minUpperBodyIKTurningSpeed - maxUpperBodyIKTurningSpeed) + maxUpperBodyIKTurningSpeed);
            }
        }

        #region Move Toward.
        protected void TurningWhileManeuvering()
        {
            if (agent.pathPending || _currentAgentVelocity <= 0.15f)
                return;

            // IK.
            iKHandler.isManuverIK = true;

            // Turning.
            Vector3 _curVelocityDir = agent.velocity;
            _curVelocityDir.y = 0;

            float _angleToPlayer = Vector3.Angle(dirToTarget, _curVelocityDir);
            if (_angleToPlayer < maneuverLookAtPlayerThershold/*|| distanceToTarget <= applyMoveTowardPredictDistance*/)
            {
                if (_angleToPlayer < maneuverHeadIKThershold)
                {
                    iKHandler.isEnemyForwardIK = false;
                }

                ManeuveringLookAtPlayer();
            }
            else
            {
                iKHandler.isEnemyForwardIK = true;
                ManeuveringLookAtRoute(_curVelocityDir);
            }
        }

        void ManeuveringLookAtPlayer()
        {
            float angle = Vector3.SignedAngle(mTransform.forward, dirToTarget, vector3Up);
            float rot = maneuverAngularSpeed * _delta;
            rot = Mathf.Min(Mathf.Abs(angle), rot);
            mTransform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
        }

        void ManeuveringLookAtRoute(Vector3 _curVelocityDir)
        {
            float angle = Vector3.SignedAngle(mTransform.forward, _curVelocityDir, vector3Up);
            float rot = maneuverAngularSpeed * _delta;
            rot = Mathf.Min(Mathf.Abs(angle), rot);
            mTransform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
        }
        #endregion

        #region Lockon Move.
        protected void TurningWhileLockonMoveAround()
        {
            if (angleToTarget > animInplaceRotateThershold)
            {
                AggroTurnWithInplaceAnimation();
            }
        }

        protected void AggroTurnWithInplaceAnimation()
        {
            useInplaceTurningSpeed = true;
            GetInplaceTurningAnimation();
        }

        protected virtual void GetInplaceTurningAnimation()
        {
            if (IsTargetOnLeftSide())
            {
                if (isWeaponOnHand)
                {
                    PlayAnimationCrossFade(e_armed_turn_left_inplace_hash, 0.2f, true);
                }
                else
                {
                    PlayAnimationCrossFade(e_unarmed_turn_left_inplace_hash, 0.2f, true);
                }
            }
            else
            {
                if (isWeaponOnHand)
                {
                    PlayAnimationCrossFade(e_armed_turn_right_inplace_hash, 0.2f, true);
                }
                else
                {
                    PlayAnimationCrossFade(e_unarmed_turn_right_inplace_hash, 0.2f, true);
                }
            }
        }
        #endregion

        #region Idle.
        protected virtual void TurningWhileIdleWithAnim()
        {
            // if the current angle to target exceeded animation turning thershold...
            if (angleToTarget >= animInplaceRotateThershold)
            {
                if (angleToTarget >= animRootRotateThershold)
                {
                    AggroTurnWithRootAnimation();
                }
                else
                {
                    // useInplaceTurningSpeed should be on in order to use different slerp speed on TurnRootMotion AIAction.
                    AggroTurnWithInplaceAnimation();
                }
            }
        }

        protected virtual void AggroTurnWithRootAnimation()
        {
            if (IsTargetOnLeftSide())
            {
                if (isWeaponOnHand)
                {
                    PlayAnimationCrossFade(e_armed_turn_left_90_hash, 0.2f, true);
                }
                else
                {
                    PlayAnimationCrossFade(e_unarmed_turn_left_90_hash, 0.2f, true);
                }
            }
            else
            {
                if (isWeaponOnHand)
                {
                    PlayAnimationCrossFade(e_armed_turn_right_90_hash, 0.2f, true);
                }
                else
                {
                    PlayAnimationCrossFade(e_unarmed_turn_right_90_hash, 0.2f, true);
                }
            }
        }
        #endregion

        #endregion

        #region Fixed Tick.
        public virtual void TurningWhileIdleWithIK()
        {
            if (isPausingTurnWithAgent || isMovingToward)
                return;

            if (angleToTarget < animInplaceRotateThershold)
            {
                iKHandler.isEnemyForwardIK = false;

                if (angleToTarget >= upperBodyIKRotateThershold)
                {
                    iKHandler.TurnOnBodyRigIK();
                    LerpToFaceTarget();
                }
                else
                {
                    iKHandler.TurnOffBodyRigIK();
                }
            }
        }

        protected void LerpToFaceTarget()
        {
            mTransform.rotation = Quaternion.Lerp(mTransform.rotation, Quaternion.LookRotation(dirToTarget), _delta * currentUpperBodyIKTurningSpeed);
            //float angle = Vector3.SignedAngle(mTransform.forward, aiManager.dirToTarget, aiManager.vector3Up);
            //float rot = upperBodyIKTurningSpeed * _fixedDelta;
            //rot = Mathf.Min(Mathf.Abs(angle), rot);
            //mTransform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
        }
        #endregion

        public void OffParryExecuteLookAtPlayer()
        {
            // IK.
            iKHandler.isManuverIK = false;

            // Turning.
            float angle = Vector3.SignedAngle(mTransform.forward, dirToTarget, vector3Up);
            float rot = maneuverAngularSpeed * _delta;
            rot = Mathf.Min(Mathf.Abs(angle), rot);
            mTransform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
        }
        #endregion

        #region Turn With Agent - KMA.

        #region Tick.
        public void TurnWithAgent_KMA()
        {
            if (isPausingTurnWithAgent)
            {
                iKHandler.isUsingIK = false;
                return;
            }
            else if (angleToTarget > animInplaceRotateThershold)
            {
                iKHandler.isUsingIK = false;
            }
            else
            {
                iKHandler.isUsingIK = true;
            }

            TurningWhileIdleWithAnim_KMA();

            void TurningWhileIdleWithAnim_KMA()
            {
                // if the current angle to target exceeded animation turning thershold...
                if (angleToTarget >= animInplaceRotateThershold)
                {
                    // useInplaceTurningSpeed should be on in order to use different slerp speed on TurnRootMotion AIAction.
                    AggroTurnWithInplaceAnimation();
                }
            }
        }
        #endregion

        #region Fixed Tick.
        public void TurningWhileIdleWithIK_KMA()
        {
            if (isPausingTurnWithAgent || isMovingToward)
                return;

            float angleToCurrentTarget = angleToTarget;
            if (angleToCurrentTarget < animInplaceRotateThershold)
            {
                iKHandler.isEnemyForwardIK = false;
                if (angleToCurrentTarget < upperBodyIKRotateThershold)
                {
                    iKHandler.TurnOffBodyRigIK();
                }

                mTransform.rotation = Quaternion.LookRotation(dirToTarget);
            }
        }
        #endregion

        #endregion

        #region Anim With Agent.
        public abstract void AnimWithAgent();

        #region Move Toward.
        /* Enemy's locomotion will changed base on their close aggro distance,
         * which allows enemy to have a dynamic locomotion. 
         * e.g. Stop -> running -> walking -> running. */
        protected void UpdateLocomotionDualDis()
        {
            float agentVel = Mathf.Clamp01(_currentAgentVelocity / agent.speed);

            if (distanceToTarget <= locoAnimSwitchDistance)
            {
                anim.SetFloat(vertical_hash, agentVel * closeDistanecLocoAnimValue, 0.1f, _delta);
            }
            else
            {
                anim.SetFloat(vertical_hash, agentVel * farDistanceLocoAnimValue, 0.1f, _delta);
            }

            anim.SetFloat(horizontal_hash, 0, 0.1f, _delta);
        }

        /* Enemy's close aggro distance will not affect locomotion animation,
         * which means enemy's locomotion will only change linearly. 
         * e.g. Stop -> walking -> running */
        protected void UpdateLocomotionSingleDis()
        {
            float agentVel = Mathf.Clamp01(_currentAgentVelocity / agent.speed);

            anim.SetFloat(vertical_hash, agentVel * closeDistanecLocoAnimValue, 0.1f, _delta);
            anim.SetFloat(horizontal_hash, 0, 0.1f, _delta);
        }
        #endregion

        #region Lockon Move.
        protected void UpdateLockOnLocomotion()
        {
            // Speed up agent velocity by muliplying it 1.5f each frame.
            //_currentAgentVelocity = _currentAgentVelocity < 0.1 ? lockonLocoBaseValue : _currentAgentVelocity;
            float agentVel = Mathf.Clamp01(_currentAgentVelocity / (agent.speed * 0.5f));
            switch (currentLockOnLocomotionType)
            {
                case LockOnLocomotionTypeEnum.forward:
                    anim.SetFloat(horizontal_hash, agentVel * 0, 0.1f, _delta);
                    anim.SetFloat(vertical_hash, agentVel * 0.5f, 0.1f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.forward_left:
                    anim.SetFloat(horizontal_hash, agentVel * -0.5f, 0.1f, _delta);
                    anim.SetFloat(vertical_hash, agentVel * 0.5f, 0.1f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.forward_right:
                    anim.SetFloat(horizontal_hash, agentVel * 0.5f, 0.1f, _delta);
                    anim.SetFloat(vertical_hash, agentVel * 0.5f, 0.1f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.left:
                    anim.SetFloat(horizontal_hash, agentVel * -0.5f, 0.1f, _delta);
                    anim.SetFloat(vertical_hash, 0, 0.1f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.right:
                    anim.SetFloat(horizontal_hash, agentVel * 0.5f, 0.1f, _delta);
                    anim.SetFloat(vertical_hash, 0, 0.1f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.backward_left:
                    anim.SetFloat(horizontal_hash, agentVel * -0.5f, 0.1f, _delta);
                    anim.SetFloat(vertical_hash, agentVel * -0.5f, 0.1f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.backward_right:
                    anim.SetFloat(horizontal_hash, agentVel * 0.5f, 0.1f, _delta);
                    anim.SetFloat(vertical_hash, agentVel * -0.5f, 0.1f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.backward:
                    anim.SetFloat(horizontal_hash, 0, 0.1f, _delta);
                    anim.SetFloat(vertical_hash, agentVel * -0.5f, 0.1f, _delta);
                    break;
            }
        }
        #endregion

        #region Fix Dir Move.
        protected void UpdateFixDirectionLocomotion()
        {
            anim.SetFloat(horizontal_hash, 0, 0.1f, _delta);
            anim.SetFloat(vertical_hash, 1f, 0.1f, _delta);
        }
        #endregion

        #region Mod.
        public void ZeroOutLocomotionValue()
        {
            anim.SetFloat(horizontal_hash, 0);
            anim.SetFloat(vertical_hash, 0);
        }
        #endregion

        #region Patrol.
        public void AnimWithPatrol()
        {
            if (!agent.isStopped)
            {
                UpdatePatrolLocomotion();
            }

            // Stop enemy locomotion if he is too close to destination.
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                StopLocomotion();
            }
        }

        void UpdatePatrolLocomotion()
        {
            anim.SetFloat(vertical_hash, patrolLocoAnimValue, 0.1f, _delta);
            anim.SetFloat(horizontal_hash, 0, 0.1f, _delta);
        }
        #endregion

        protected void StopLocomotion()
        {
            anim.SetFloat(horizontal_hash, 0, 0.1f, _delta);
            anim.SetFloat(vertical_hash, 0, 0.1f, _delta);
        }
        #endregion

        #region Move With Agent.
        public abstract void MoveWithAgent();
        
        #region Move Toward.
        protected void UpdateAgentSpeedMoveToward()
        {
            float t = distanceToTarget / (aggro_Thershold * 0.45f);
            t = t > 1 ? 1 : t;

            if (distanceToTarget < (agentStopDistance + 1.75f))
            {
                agent.acceleration = (_currentAgentAccelSpeed * t) + closeSpeedBuffer;
                agent.speed = (_currentAgentMoveSpeed * t) + closeSpeedBuffer;
            }
            else
            {
                agent.acceleration = _currentAgentAccelSpeed * t;
                agent.speed = _currentAgentMoveSpeed * t;
            }
        }

        protected void UpdateMoveTowardAgent()
        {
            //Debug.Log("agent.remainingDistance " + agent.remainingDistance);

            agent.isStopped = false;
            agent.stoppingDistance = agentStopDistance;

            SetMoveTowardDestination();
            UpdateMoveTowardPosition();

            isMovementChanged = true;
        }

        protected void SetMoveTowardDestination()
        {
            if (playerStates._isAttackCharging)
            {
                SetDesinationToPlayer_NoPredict();
            }
            else
            {
                SetDestinationToPlayer_Predict();
            }

            void SetDesinationToPlayer_NoPredict()
            {
                agent.SetDestination(targetPos);
            }

            void SetDestinationToPlayer_Predict()
            {
                playerStates.ReturnPredictedMoveTowardDestn(this);
                //Debug.DrawLine(_playerStates.mTransform.position, targetPos + _predictedMoveTowardDestn, Color.black);

                agent.SetDestination(targetPos + predictedMoveTowardDestn);
            }
        }

        protected void UpdateMoveTowardPosition()
        {
            float proportionalDistance = agent.acceleration / _currentAgentAccelSpeed;
            mTransform.position = Vector3.Lerp(mTransform.position, agent.nextPosition, proportionalDistance);
        }
        #endregion

        #region Lock On Move.
        protected void UpdateAgentSpeedLockonMove()
        {
            agent.acceleration = _currentAgentAccelSpeed * 0.4f;
            agent.speed = _currentAgentMoveSpeed * 0.6f;
        }

        protected void UpdateLockOnAgent()
        {
            agent.isStopped = false;
            agent.stoppingDistance = 0f;

            UpdateLockonMovePosition();

            isMovementChanged = true;
        }

        protected void UpdateLockonMovePosition()
        {
            mTransform.position = agent.nextPosition;
        }
        #endregion

        public void SetTargetPosToPlayer()
        {
            targetPos = playerStates.mTransform.position;
        }

        public void MoveTowardPlayer()
        {
            isLockOnMoveAround = false;
            isMovingToward = true;
            isMovingTowardPlayer = true;
        }
        #endregion

        #region Lock On Move Around (Type).
        public void ResetUpdateLockOnPosBool()
        {
            if (!updateLockOnPos)
            {
                lockOnPosUpdateTimer += _delta;
                if (lockOnPosUpdateTimer >= lockOnPosUpdateRate)
                {
                    RefreshLockonStatus();
                }
            }
        }

        public void RefreshLockonStatus()
        {
            updateLockOnPos = true;
            lockOnPosUpdateTimer = 0;
        }

        public void UpdateLockonPosition()
        {
            if (updateLockOnPos)
            {
                updateLockOnPos = false;
                Vector3 newLockOnPosition = vector3Zero;

                if (currentAILockonMoveProfile.isMoveAroundPlayer)
                {
                    Vector3 targetDir = StaticHelper.GetDirFromAngle(playerStates.mTransform, currentAILockonMoveProfile.GetRandomDegreeFromType(), false);
                    newLockOnPosition = playerStates.mTransform.position + targetDir * currentAILockonMoveProfile.GetRandomDistanceFromRange();
                }
                else
                {
                    Vector3 targetDir = StaticHelper.GetDirFromAngle(mTransform, currentAILockonMoveProfile.GetRandomDegreeFromType(), false);
                    newLockOnPosition = mTransform.position + targetDir * currentAILockonMoveProfile.GetRandomDistanceFromRange();
                }

                targetLockOnPos = newLockOnPosition;
            }
        }

        public void SetNewLockonPositionToAgent()
        {
            UpdateLockonPosition();

            agent.SetDestination(targetLockOnPos);

            isLockOnMoveAround = true;
            isMovingToward = false;
        }

        public void SetNewLockonPositionImmediately()
        {
            updateLockOnPos = false;
            lockOnPosUpdateTimer = 0;

            updateLockOnPos = false;
            Vector3 newLockOnPosition = vector3Zero;

            if (currentAILockonMoveProfile.isMoveAroundPlayer)
            {
                Vector3 targetDir = StaticHelper.GetDirFromAngle(playerStates.mTransform, currentAILockonMoveProfile.GetRandomDegreeFromType(), false);
                newLockOnPosition = playerStates.mTransform.position + targetDir * currentAILockonMoveProfile.GetRandomDistanceFromRange();
            }
            else
            {
                Vector3 targetDir = StaticHelper.GetDirFromAngle(mTransform, currentAILockonMoveProfile.GetRandomDegreeFromType(), false);
                newLockOnPosition = mTransform.position + targetDir * currentAILockonMoveProfile.GetRandomDistanceFromRange();
            }

            targetLockOnPos = newLockOnPosition;

            SetNewLockonPositionToAgent();
        }

        public void MonitorLockOnLocomotionType()
        {
            if (isLockOnMoveAround)
            {
                Transform aiHeadTrans = iKHandler.aIHeadTrans;
                Vector3 relatPosDir = targetLockOnPos - aiHeadTrans.position;
                relatPosDir.y = 0;

                Vector3 forward = aiHeadTrans.forward;
                forward.y = 0;

                relatPosAngle = Vector3.Angle(forward, relatPosDir);
                relatPosAngle = Vector3.Dot(aiHeadTrans.right, relatPosDir) < 0 ? -relatPosAngle : relatPosAngle;
                currentLockOnLocomotionType = GetTargetLockOnLocomotionType();
            }
        }

        LockOnLocomotionTypeEnum GetTargetLockOnLocomotionType()
        {
            LockOnLocomotionTypeEnum retVal = LockOnLocomotionTypeEnum.forward;

            if (relatPosAngle < -30)
            {
                // DO SENCOND CHECK
                if (relatPosAngle <= -120)
                {
                    if (relatPosAngle < -150)
                    {
                        retVal = LockOnLocomotionTypeEnum.backward;
                    }
                    else
                    {
                        retVal = LockOnLocomotionTypeEnum.backward_left;
                    }
                }
                else
                {
                    // DO THIRD CHECK
                    if (relatPosAngle <= -60)
                    {
                        retVal = LockOnLocomotionTypeEnum.left;
                    }
                    else
                    {
                        retVal = LockOnLocomotionTypeEnum.forward_left;
                    }
                }
            }
            else
            {
                if (relatPosAngle >= 120)
                {
                    if (relatPosAngle > 150)
                    {
                        retVal = LockOnLocomotionTypeEnum.backward;
                    }
                    else
                    {
                        retVal = LockOnLocomotionTypeEnum.backward_right;
                    }
                }
                else
                {
                    if (relatPosAngle > 60)
                    {
                        retVal = LockOnLocomotionTypeEnum.right;
                    }
                    else
                    {
                        if (relatPosAngle >= 30)
                        {
                            retVal = LockOnLocomotionTypeEnum.forward_right;
                        }
                        else
                        {
                            retVal = LockOnLocomotionTypeEnum.forward;
                        }
                    }
                }
            }

            return retVal;
        }
        #endregion

        #region Root Motions.
        /// Tick.
        public virtual void ApplyTurnRootMotion()
        {
            if (applyTurnRootMotion)
            {
                Quaternion _tarRot;
                CalculateTurnRootMotion();
                //Debug.DrawRay(mTransform.position, _turnRootMotionDir, Color.green);

                if (useInplaceTurningSpeed)
                {
                    mTransform.rotation = Quaternion.Slerp(mTransform.rotation, _tarRot, _delta * inplaceTurningSpeed);
                }
                else
                {
                    mTransform.rotation = Quaternion.Slerp(mTransform.rotation, _tarRot, _delta * rootTurningSpeed);
                }

                void CalculateTurnRootMotion()
                {
                    Vector3 tarDir;

                    if (isTrackingPlayer)
                    {
                        float targetPredictOffset = playerStates._isRunning ? currentPlayerPredictOffset + runningPredictAddonValue : currentPlayerPredictOffset;
                        tarDir = StaticHelper.GetNewRotatedVector3(dirToTarget, -targetPredictOffset * playerStates.horizontal);
                        //Debug.DrawRay(_mTransform.position, tarDir, Color.black);
                    }
                    else
                    {
                        tarDir = dirToTarget;
                    }

                    tarDir.y = 0;

                    _tarRot = Quaternion.LookRotation(tarDir);
                }
            }
        }

        /// Fixed Tick.
        public virtual void AI_HandleRootMotions_FixedUpdate()
        {
            aIStates.ApplyFallingRootMotions();
        }
        
        protected void On_Fallback_SetRootMotions()
        {
            // Anim Move State.
            aIStates.Set_AnimMoveRmType_ToFallback();
        }

        protected void On_Knockdown_SetRootMotions()
        {
            // Turning
            applyTurnRootMotion = false;

            // Fixed
            applyAttackArtifMotion = false;

            // Anim Move State.
            aIStates.Set_AnimMoveRmType_ToKnockDown();
        }

        public void CancelAllRootMotions()
        {
            // Turning
            applyTurnRootMotion = false;

            // Fixed
            applyAttackArtifMotion = false;

            // Anim Move State.
            aIStates.Set_AnimMoveRmType_ToNull();
        }
        #endregion

        #region On Enter Aggro Faced Player States.
        /// Is Found Player Transition doesn't have a method.
        
        public void FindPlayerInPatrol()
        {
            if (aIStates._aiGroupManagable.isForbiddenToFoundPlayer)
                return;

            /// If player is within aggro range.
            if (distanceToTarget < aggro_Thershold)
            {
                /// check if player is within the closet limit of aggro.
                if (distanceToTarget <= aggro_ClosestThershold)
                {
                    if (CheckTargetIsBehindWall())
                        return;

                    OnEnterAggroFacedPlayerState();
                }
                /// if player is not within closet limit, check if angle within aggro angle.
                else if (angleToTarget <= aggro_Angle)
                {
                    if (CheckTargetIsBehindWall())
                        return;

                    OnEnterAggroFacedPlayerState();
                    #region Raycast Message.
                    //if (Physics.Raycast(rayStartPos, dirToTarget, out hit, aggro_Thershold, aIStates.nonPlayerLayers))
                    //{
                    //    if (debugMessageObjectName != hit.transform.gameObject.name)
                    //    {
                    //        debugMessageObjectName = hit.transform.gameObject.name;
                    //        Debug.Log(aIStates.GetErrorMessage(gameObject.name, " Monitor Aggro Error, Ray hit ", hit.transform.gameObject.name, hit.transform.parent.gameObject.name));
                    //    }
                    //}
                    #endregion
                }
            }
        }

        public void OnEnterAggroFacedPlayerState()
        {
            aIStates.OnEnterAggroFacedPlayerReset();
            _aggroTransitTimer = 0;

            // Reset vertical, horizontal para.
            anim.SetFloat(vertical_hash, 0);
            anim.SetFloat(horizontal_hash, 0);

            // Reset Is Faced Player.
            anim.SetBool(e_IsFacedPlayer_hash, false);

            // LockOn Move Around Stats
            RefreshLockonStatus();

            AIModsOnEnterAggroFacedPlayer();
        }
        #endregion

        #region On Enter Aggro States / Transition.
        public bool IsEnterAggroTransition(float _aggroTransitRate, float _aggroTransitAngle)
        {
            if (GetHeadAngleToTarget() <= _aggroTransitAngle)
            {
                OnAggroEnemyUnSheathWeapon();

                _aggroTransitTimer += _delta;
                if (_aggroTransitTimer >= _aggroTransitRate || distanceToTarget <= aggro_ClosestThershold)
                {
                    if (anim.GetBool(e_IsFacedPlayer_hash))
                    {
                        aIStates.OnAggroStateResets();
                        OnAggroStateResets();
                        return true;
                    }
                }
            }

            return false;
        }

        float GetHeadAngleToTarget()
        {
            Vector3 headForwardVector = iKHandler.aIHeadTrans.forward;
            headForwardVector.y = 0;

            Vector3 headDirToTarget = targetPos - iKHandler.aIHeadTrans.position;
            headDirToTarget.y = 0;
            if (headDirToTarget == vector3Zero)
                headDirToTarget = headForwardVector;

            return Vector3.Angle(headForwardVector, headDirToTarget);
        }

        protected virtual void OnAggroEnemyUnSheathWeapon()
        {
            if (!isWeaponUnSheathAnimExecuted)
            {
                SetCurrentFirstWeaponBeforeAggro();
                PlayAnimation_NoNeglect(e_unsheath_First_hash, true);
                isWeaponUnSheathAnimExecuted = true;
            }
        }

        public abstract void AIModsOnEnterAggroFacedPlayer();
        #endregion

        #region On Exit Aggro Faced Player States / Transition.
        public virtual bool IsExitAggroTransition_ByDistance()
        {
            if (distanceToTarget > exitAggro_Thershold)
            {
                OnExitAggroFacedPlayerReset();
                return true;
            }

            return false;
        }

        public virtual bool IsExitAggroTransition_OnPlayerDead()
        {
            if (playerStates.isDead)
            {
                OnExitAggroFacedPlayerReset();
                return true;
            }

            return false;
        }

        public abstract void OnExitAggroFacedPlayerReset();

        protected void Base_OnExitAggroFacedPlayerResets()
        {
            // Current RootMotion Velocity
            ignoreAttackRootMotionCalculate = false;
            currentAttackVelocity = 0;

            // Anim Move RM
            aIStates.Set_AnimMoveRmType_ToNull();

            // Attack Interval Stats
            enemyAttacked = false;
            _isSkippingOnHitAnim = false;
            attackIntervalTimer = 0;

            RandomizeAttackInterval_SpecificRange();

            // Booleans
            isMovingToward = false;
            isMovingTowardPlayer = false;
            isLockOnMoveAround = false;
            
            isTrackingPlayer = false;
            isPausingTurnWithAgent = false;

            applyTurnRootMotion = false;

            // Action
            currentAction = null;
            currentPassiveAction = null;
            currentMultiStageAttack = null;

            // Action Holder
            skippingScoreCalculation = false;
            currentActionHolder = null;

            // Area Damage Particle FX
            _currentDamageParticle = null;

            // Reset Vertical, Horizontal Para.
            anim.SetFloat(vertical_hash, 0);
            anim.SetFloat(horizontal_hash, 0);

            // Reset Is Anim Para.
            anim.SetBool(aIStates.e_IsInteracting_hash, false);
            anim.SetBool(e_IsLockOnMoveAround_hash, false);
            anim.SetBool(e_IsFacedPlayer_hash, false);

            _idleTransitTimer = 0;
        }
        #endregion

        #region Off Exit Aggro Faced Player States / Transition.
        public bool ByDistance_IsExitAggroFacedPlayerTransition(float _idleTransitRate)
        {
            _idleTransitTimer += _delta;
            if (_idleTransitTimer >= _idleTransitRate)
            {
                OffExitAggroFacePlayerSheathWeapon();

                if (anim.GetBool(e_IsFacedPlayer_hash))
                {
                    OffExitAggroFacedPlayerReset();
                    aIStates.OffExitAggroFacedPlayerReset();
                    return true;
                }
            }

            return false;
        }

        public bool PlayerDied_IsExitAggroFacedPlayerTransition(float _idleTransitRate)
        {
            _idleTransitTimer += _delta;
            if (_idleTransitTimer >= _idleTransitRate)
            {
                OffExitAggroFacePlayerSheathWeapon();

                if (anim.GetBool(e_IsFacedPlayer_hash))
                {
                    OffExitAggroFacedPlayerReset();
                    aIStates.PlayerDied_OffExitAggroFacePlayerReset();
                    return true;
                }
            }

            return false;
        }

        protected virtual void OffExitAggroFacePlayerSheathWeapon()
        {
            if (!isWeaponSheathAnimExecuted)
            {
                PlaySheathAnimation();

                anim.SetBool(e_IsArmed_hash, false);
                isWeaponSheathAnimExecuted = true;
            }
        }

        void OffExitAggroFacedPlayerReset()
        {
            /// * This reset needs to follow behind Interactable Stats reset.
            currentCrossFadeLayer = defaultCrossFadeLayer;
            isWeaponUnSheathAnimExecuted = false;
            isWeaponSheathAnimExecuted = false;
        }
        #endregion
        
        #region Force Exit Aggro To Patrol.
        public void ForceExitAggroToPatrol()
        {
            OnExitAggroFacedPlayerReset();
            aIStates.currentState = GameManager.singleton.enemyExitAggroFacedPlayerState;
        }
        #endregion

        #region On Aggro State Reset.
        public void OnAggroStateResets()
        {
            // Attack Interval Stats.
            _isSkippingOnHitAnim = false;

            // Quit Neglect.
            isMultiStageAttackAvailable = false;

            // Attack Root Motion.
            applyAttackArtifMotion = false;
            currentAttackVelocity = 0;
            ignoreAttackRootMotionCalculate = false;

            // Anim Move RM
            aIStates.Set_AnimMoveRmType_ToNull();

            // Turn(Rotation) Root Motion.
            applyTurnRootMotion = false;
            useInplaceTurningSpeed = false;
            isPausingTurnWithAgent = false;
            isTrackingPlayer = false;
            currentPlayerPredictOffset = 0;
        }
        #endregion

        #region On Parry Execute States.
        public bool IsEnterParryExecuteTransition()
        {
            if (_isInParryExecuteWindow)
            {
                OnParryExecuteStateReset();
                return true;
            }

            return false;
        }

        void OnParryExecuteStateReset()
        {
            // Parry Stats.
            _parryExecuteWaitTimer = 0;

            // Attack Interval Stats.
            _isSkippingOnHitAnim = false;

            // Quit Neglect.
            isMultiStageAttackAvailable = false;

            // Anim Move RM
            aIStates.Set_AnimMoveRmType_ToNull();

            // Attack Root Motion.
            applyAttackArtifMotion = false;
            currentAttackVelocity = 0;
            ignoreAttackRootMotionCalculate = false;

            // Weapon.
            OnParryExecuteStateResetColliderStatus();

            // Reset Vertical, Horizontal Para.
            anim.SetFloat(vertical_hash, 0);
            anim.SetFloat(horizontal_hash, 0);

            // Reset Bool Para.
            anim.SetBool(aIStates.e_IsInteracting_hash, false);
            anim.SetBool(e_IsInParryWindow_hash, true);
            
            PlayParryReceivedAnim();
        }

        protected virtual void OnParryExecuteStateResetColliderStatus()
        {
            currentWeapon.SetColliderStatusToFalse();
        }
        #endregion

        #region Parry Execution.
        public void PermitParryExecution(float _parryExecutePermitRange)
        {
            if (!_isParryExecutingEnemy && _isInParryExecuteWindow)
            {
                if (CheckTargetInRange(_parryExecutePermitRange))
                {
                    if (CheckIsPlayerFacingEnemy())
                    {
                        /// Show UI here...
                        playerStates.ShowExecutionCard();

                        if (playerStates.p_execution_select)
                        {
                            _isParryExecutingEnemy = true;
                            playerStates.PerformParryExecution(this);
                        }
                    }
                    else
                    {
                        playerStates.HideExecutionCard_MoveOut();
                    }
                }
                else
                {
                    playerStates.HideExecutionCard_MoveOut();
                }
            }
        }

        public void OnReceivedParryExecution()
        {
            _isInvincible = true;

            /// Set Position.
            Vector3 _targetPos = (playerStates.mTransform.forward * _p_executionProfile._executionMoveDistance) + playerStates.mTransform.position;
            _targetPos.y = mTransform.position.y + _p_executionProfile._executionHeightBuffer;

            LeanTween.move(gameObject, _targetPos, 0.15f).setEase(LeanTweenType.easeOutQuad);

            /// Set Rotation.
            mTransform.rotation = Quaternion.LookRotation(dirToTarget);

            /// Set Root Motion.
            currentExecutionVelocity = _p_executionProfile._executionRootMotion;

            /// Play Animation.
            anim.SetBool(e_IsInParryWindow_hash, false);
            PlayAnimationCrossFade(_executionReceiveHash, 0.1f, true);

            /// Set Is Knocked Down, "isKnockDown" bool is Set in Anim Event.
            anim.SetBool(e_IsKnockedDown_hash, true);
            _currentGetupWaitRate = _executionGetupWaitRate;
        }

        public void OnExecutionFinished()
        {
            if (isDead)
            {
                OnEnemyDeath_ExecutionFinished();
                aIStates.OnEnemyDeath_Executed();
            }
            else
            {
                /// Start Knock Down Getup Count.
                isKnockedDown = true;
            }

            _isParryExecutingEnemy = false;
            currentExecutionVelocity = 0;
        }
        #endregion

        #region Time Out Exit Parry Execute States.
        public bool IsTimeOutExitParryExecuteTransition(float _parryExecuteWaitRate)
        {
            _parryExecuteWaitTimer += _delta;
            if (_parryExecuteWaitTimer >= _parryExecuteWaitRate || _isInvincible)
            {
                
                TimeOutExitParryExecuteStateReset();
                return true;
            }

            return false;
        }
        
        void TimeOutExitParryExecuteStateReset()
        {
            /// Hide Execution Card.
            playerStates.HideExecutionCard_MoveOut();

            anim.SetBool(e_IsInParryWindow_hash, false);
            _isInParryExecuteWindow = false;
            _isParryable = false;

            aIStates.ReEnableAgent();
        }
        #endregion

        #region Knock Down Exit Parry Execute States.
        public bool IsKnockDownExitParryExecutionTransition()
        {
            if (!_isParryExecutingEnemy && !isKnockedDown)
            {
                aIStates.OnAggroStateResets();
                OnAggroStateResets();
                return true;
            }

            return false;
        }
        #endregion

        #region Dead Exit Parry Execute States.
        public bool IsDeadExitParryExecuteTransition()
        {
            if (!_isParryExecutingEnemy && isDead)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Is Enter Wait For Parry Execution End Transition.
        public bool IsEnterWaitForParryExecutionEndTransition()
        {
            if (_isParryExecutingEnemy)
            {
                WaitForParryExecutionEndTransitionReset();
                return true;
            }

            return false;
        }

        void WaitForParryExecutionEndTransitionReset()
        {
            _isInParryExecuteWindow = false;
            _isParryable = false;
        }
        #endregion

        #region Off Parry Execute Faced Player State.
        public bool IsExitParryExecuteFacedPlayerTransition(float _facePlayerWaitRate)
        {
            _parryExecuteFacedPlayerWaitTimer += _delta;
            if (_parryExecuteFacedPlayerWaitTimer >= _facePlayerWaitRate)
            {
                _parryExecuteFacedPlayerWaitTimer = 0;
                OffParryExecuteFacedPlayerResetTurnRootMotionStatus();
                return true;
            }

            return false;
        }

        void OffParryExecuteFacedPlayerResetTurnRootMotionStatus()
        {
            // Turn(Rotation) Root Motion.
            isPausingTurnWithAgent = false;
            isTrackingPlayer = false;
            currentPlayerPredictOffset = 0;
        }
        #endregion

        #region On Hit.
        public void OnHit(DamageCollider _damageCollider)
        {
            GetCurrentGetHitReferences();
            _isInvincible = true;

            ChangeAIStatsWhenHit();
            ChangePlayerStatsWhenHit();
            AIDisplayManagerWhenHit();
            
            OnHitAIMods();
            SpawnOnHitEffect();
            PlayOnHitAnimation();

            ResetGetHitReferences();
            CheckIsEnemyDeadAndKnockedDown();

            void GetCurrentGetHitReferences()
            {
                _previousHitDamage = _damageCollider._runtimeWeapon.ReturnWeaponTotalAttackPower(this);
                _hitSourceColliderTransform = _damageCollider.transform;

                _hitSourceAttackRefs = playerStates._currentAttackAction._playerAttackRefs;
                _isHitByChargeAttack = _hitSourceAttackRefs._attackActionType == Player_AttackRefs.AttackActionTypeEnum.Charged ? true : false;
            }

            void AIDisplayManagerWhenHit()
            {
                aIStates.aiDisplayManager.OnHit();
            }

            void ChangePlayerStatsWhenHit()
            {
                playerStates.ChangePlayerStatsWhenHit();
            }

            void ResetGetHitReferences()
            {
                _hitSourceAttackRefs = null;
            }

            void CheckIsEnemyDeadAndKnockedDown()
            {
                if (isDead && !isKnockedDown)
                {
                    aIStates.OnDeathSwitchLayer();
                    PlayOnDeathAnimation();
                }
            }
        }

        protected void ChangeAIStatsWhenHit()
        {
            DepleteHealthFromDamage();

            if (currentEnemyHealth <= 0)
            {
                if (!isDead)
                {
                    OnEnemyDeath_BeforeAnimPlay();
                    isDead = true;
                }
            }

            if (!isWeaponOnHand)
            {
                OnEnterAggroFacedPlayerState();
            }
        }

        protected virtual void DepleteHealthFromDamage()
        {
            DepleteHealth_Regular();
        }
        
        protected virtual void OnHitAIMods()
        {
        }

        protected virtual void SpawnOnHitEffect()
        {
            Spawn_Regular_BloodFx();
        }

        protected abstract void PlayOnHitAnimation();

        public abstract void HandleArmedGetHitAnimation();

        public abstract void HandleUnarmedGetHitAnimation();
        
        public abstract void PlayOnDeathAnimation();

        #region On Knocked Down.
        /// Is Interacting Tick.
        void KnockedDownGetupWaitTimeCount()
        {
            if (isKnockedDown)
            {
                _getupWaitTimer += _delta;
                if (_getupWaitTimer >= _currentGetupWaitRate)
                {
                    _getupWaitTimer = 0;

                    applyTurnRootMotion = true;
                    isTrackingPlayer = true;
                    currentPlayerPredictOffset = 27;

                    isKnockedDown = false;
                    anim.SetBool(e_IsKnockedDown_hash, false);
                }
            }
        }
        #endregion

        #region CAN CALLED FROM MOD.
        public void DepleteHealth_Regular()
        {
            currentEnemyHealth -= _previousHitDamage;
        }
        #endregion

        #endregion
        
        #region On Execution Hit.

        #region First.
        public void OnParryExecutionFirstHit()
        {
            RefreshPreviousDamageValue_1st_ExecuteHit();

            DepleteHealthFromDamage();
            aIStates.aiDisplayManager.OnHit();

            Spawn_Execution_1stHit_BloodFx();
                
            void RefreshPreviousDamageValue_1st_ExecuteHit()
            {
                if (_p_executionProfile._isExecutionDividedInThreeSection)
                {
                    _previousHitDamage = _previousExecutionDamage * 0.3f;
                }
                else
                {
                    _previousHitDamage = _previousExecutionDamage * 0.4f;
                }
            }
        }
        #endregion

        #region Second.
        public void OnParryExecutionSecondHit()
        {
            if (_p_executionProfile._isExecutionDividedInThreeSection)
            {
                OnSecondHit_ThreeHitVer();
            }
            else
            {
                OnSecondHit_TwoHitVer();
            }

            void OnSecondHit_ThreeHitVer()
            {
                _previousHitDamage = _previousExecutionDamage * 0.3f;

                DepleteHealthFromDamage();
                aIStates.aiDisplayManager.OnHitAgain();

                Spawn_Execution_2ndHit_BloodFx_ThreeHitVer();
            }

            void OnSecondHit_TwoHitVer()
            {
                _previousHitDamage = _previousExecutionDamage * 0.6f;

                DepleteHealthFromDamage();
                aIStates.aiDisplayManager.OnHitAgain();

                Spawn_Execution_2ndHit_BloodFx_TwoHitVer();

                CheckIsEnemyDeadInExecution();
                OnExecutionAIMods();
                ResetGetExecutedReferences();
            }
        }
        #endregion

        #region Third.
        public void OnParryExecutionThirdHit()
        {
            RefreshPreviousDamageValue_3rd_ExecuteHit();

            DepleteHealthFromDamage();
            aIStates.aiDisplayManager.OnHitAgain();

            Spawn_Execution_3rdHit_BloodFx();

            CheckIsEnemyDeadInExecution();
            OnExecutionAIMods();
            ResetGetExecutedReferences();

            void RefreshPreviousDamageValue_3rd_ExecuteHit()
            {
                _previousHitDamage = _previousExecutionDamage * 0.4f;
            }
        }
        #endregion

        protected void CheckIsEnemyDeadInExecution()
        {
            if (currentEnemyHealth <= 0)
            {
                if (!isDead)
                {
                    isDead = true;
                }
            }
        }

        protected virtual void OnExecutionAIMods()
        {
        }

        protected void ResetGetExecutedReferences()
        {
            _previousHitDamage = 0;
        }

        public void ExecutionGetupWaitTimeCount()
        {
            if (isKnockedDown)
            {
                _getupWaitTimer += _delta;
                if (_getupWaitTimer >= _currentGetupWaitRate)
                {
                    _getupWaitTimer = 0;

                    isKnockedDown = false;
                    anim.SetBool(e_IsKnockedDown_hash, false);
                }
            }
        }
        #endregion

        #region On Enemy Death.
        /// On Executed.
        protected void OnEnemyDeath_ExecutionFinished()
        {
            OnDeathCancelLockon();
            
            void OnDeathCancelLockon()
            {
                /// Reset Player Lockon States if this is lockon target.
                if (aIStates._isLockonState)
                {
                    playerStates.SetIsLockingOnStatusToFalse();
                }
            }
        }
        
        /// Before Anim Play.
        protected void OnEnemyDeath_BeforeAnimPlay()
        {
            OnDeathTurnOffDamageCollider();
            OnDeathPauseAIAction();
            OnDeathCancelLockon();

            void OnDeathPauseAIAction()
            {
                skippingScoreCalculation = true;
                currentMultiStageAttack = null;
                currentAction = null;
            }

            void OnDeathCancelLockon()
            {
                /// Reset Player Lockon States if this is lockon target.
                if (aIStates._isLockonState)
                {
                    playerStates.SetIsLockingOnStatusToFalse();
                }
            }
        }

        protected virtual void OnDeathTurnOffDamageCollider()
        {
            if (currentWeapon != null)
            {
                currentWeapon.SetColliderStatusToFalse();
            }
        }

        /// After Anim Play
        public void OnEnemyDeath_AfterAnimPlay()
        {
            #region Current RootMotion Velocity
            ignoreAttackRootMotionCalculate = false;
            currentAttackVelocity = 0;
            #endregion
            
            #region Attack Interval Stats
            enemyAttacked = false;
            _isSkippingOnHitAnim = false;
            attackIntervalTimer = 0;

            RandomizeAttackInterval_SpecificRange();
            #endregion

            #region Bools.
            isTrackingPlayer = false;
            isPausingTurnWithAgent = false;
            #endregion

            #region Action
            currentAction = null;
            currentPassiveAction = null;
            currentMultiStageAttack = null;
            #endregion
            
            #region Area Damage Particle FX
            _currentDamageParticle = null;
            #endregion

            #region Anim Para.
            anim.SetBool(e_IsLockOnMoveAround_hash, false);
            anim.SetBool(e_IsFacedPlayer_hash, false);
            anim.SetBool(aIStates.e_IsInteracting_hash, false);
            anim.SetFloat(vertical_hash, 0);
            anim.SetFloat(horizontal_hash, 0);
            #endregion
            
            #region Player.
            /// Give Player Volun points.
            if (!isDeadFromSave)
            {
                playerStates.statsHandler.RefreshVolunsWhenAIKilled(volunReturnAmount);

                /// Change Player Amulet's Emission.
                playerStates._savableInventory.RefreshAmuletEmissionWhenAIKilled();
            }
            #endregion

            #region Boss.
            OnBossDeath_SetStatus();
            #endregion

            #region Anim Hook.
            a_hook.enabled = false;
            #endregion

            #region Damage Collider.
            /// Turn off again just to be sure.
            if (!isDeadFromSave)
            {
                OnDeathTurnOffDamageCollider();
            }
            #endregion

            enabled = false;
        }

        public virtual void OnBossDeath_SetStatus()
        {
        }
        #endregion

        #region On Enemy Revive.
        public void OnEnemyRevive()
        {
            #region Weapon.
            if (currentWeapon)
            {
                SheathCurrentWeaponToPosition();
                SheathCurrentSidearmToPosition();
            }
            #endregion

            #region Anim Para.
            anim.SetBool(e_IsArmed_hash, false);
            anim.SetBool(hashManager.e_IsDead_hash, false);
            #endregion

            #region Bools.
            isDead = false;
            isDeadFromSave = false;
            #endregion

            #region Decision Variable.
            currentCrossFadeLayer = defaultCrossFadeLayer;
            isWeaponUnSheathAnimExecuted = false;
            isWeaponSheathAnimExecuted = false;
            #endregion

            #region Knock Down.
            if (isKnockedDown)
            {
                _getupWaitTimer = 0;
                isKnockedDown = false;
                anim.SetBool(e_IsKnockedDown_hash, false);
            }
            #endregion
            
            #region Anim Hook.
            a_hook.enabled = true;
            #endregion

            enabled = true;
        }
        #endregion

        #region On Checkpoint Refresh Aggro Enemy.
        public void OnCheckpointRefresh_AggroEnemy()
        {
            Base_OnCheckpointRefresh_AggroEnemy();

            #region Weapons.
            if (currentWeapon)
            {
                SheathCurrentWeaponToPosition();
                SheathCurrentSidearmToPosition();
            }
            #endregion

            #region Anim Para.
            anim.SetBool(e_IsArmed_hash, false);
            #endregion

            #region Decision Variable.
            /// * This reset needs to follow behind Interactable Stats reset.
            currentCrossFadeLayer = defaultCrossFadeLayer;
            isWeaponUnSheathAnimExecuted = false;
            isWeaponSheathAnimExecuted = false;
            #endregion
        }

        void Base_OnCheckpointRefresh_AggroEnemy()
        {
            // Current RootMotion Velocity
            ignoreAttackRootMotionCalculate = false;
            currentAttackVelocity = 0;

            // Anim Move RM
            aIStates.Set_AnimMoveRmType_ToNull();

            // Attack Interval Stats
            enemyAttacked = false;
            _isSkippingOnHitAnim = false;
            attackIntervalTimer = 0;

            RandomizeAttackInterval_SpecificRange();

            // Booleans
            isMovingToward = false;
            isMovingTowardPlayer = false;
            isLockOnMoveAround = false;

            isTrackingPlayer = false;
            isPausingTurnWithAgent = false;

            applyTurnRootMotion = false;
            
            // Action
            currentAction = null;
            currentPassiveAction = null;
            currentMultiStageAttack = null;
            
            // Area Damage Particle FX
            _currentDamageParticle = null;
            
            // Reset Is Anim Para.
            anim.SetBool(aIStates.e_IsInteracting_hash, false);
            anim.SetBool(e_IsLockOnMoveAround_hash, false);
            anim.SetBool(e_IsFacedPlayer_hash, false);

            _idleTransitTimer = 0;
        }
        
        public void OnCheckpointRefresh_General()
        {
            // Health
            currentEnemyHealth = totalEnemyHealth;

            // Action Holder
            skippingScoreCalculation = false;
            currentActionHolder = null;


            OnCheckpointRefresh_Mods();
        }

        protected virtual void OnCheckpointRefresh_Mods()
        {
        }
        #endregion

        #region AI State Actions FixedTicks.
        public float CalculateAttackRootMotion()
        {
            if (ignoreAttackRootMotionCalculate)
            {
                return currentAttackVelocity;
            }
            else
            {
                return currentAttackVelocity * Mathf.Clamp01(distanceToTarget / attackMaxVelocityDistance);
            }
        }

        public float CalculateAttackArtificalMotion()
        {
            return currentAttackVelocity * Mathf.Clamp01(distanceToTarget / attackMaxVelocityDistance);
        }
        #endregion

        #region Reset IsInvincible.
        public void ResetIsInvincibleStatus()
        {
            if (!_isInvincible)
                return;

            _invincibleResetTimer += _delta;
            if (_invincibleResetTimer > _invincibleResetRate)
            {
                _invincibleResetTimer = 0;
                _isInvincible = false;
            }
        }
        #endregion

        #region Enemy Attacked.
        public void SetEnemyAttackedBoolToTrue()
        {
            enemyAttacked = true;

            RandomizeAttackInterval_SpecificRange();
        }
        #endregion
        
        #region Perlious Attack Mod.
        public virtual void PerliousAttackMod_SetNewPhaseData(PerliousAttackMod_EP_Data _perliousAttackMod_EP_Data)
        {
        }


        #endregion

        #region Stamina Usage Mod.
        public virtual void RefillEnemyStamina()
        {
        }

        public virtual void DepleteEnemyStamina(float staminaUsage)
        {
        }
        #endregion

        #region Roll Interval Mod.
        public virtual void RollIntervalMod_SetNewPhaseData(RollIntervalMod_EP_Data _rollInterval_ep_data)
        {
        }
        #endregion

        #region Parry Player Mod.
        public virtual void OnWaitingToParry()
        {
        }

        public virtual void OffWaitingToParry_Parryable()
        {
        }

        public virtual void OffWaitingToParry_TimesOut()
        {
        }

        public virtual void OffWaitingToParry_HitByChargeAttack()
        {
        }
        #endregion

        #region Enemy Hit Counting Mod.
        public virtual void ResetHitCountingStates()
        {
        }
        #endregion

        #region Player Spam Blocking Mod.
        public virtual void ResetSpammedBlockingStatus()
        {
        }
        #endregion

        #region Player Spam Attack Mod.
        public virtual void ResetSpammedAttackingStatus()
        {
        }
        #endregion

        #region Move In Fix Direction Mod.
        public virtual void OnFixDirectionMove()
        {
        }

        public virtual void OffFixDirectionMove()
        {
        }

        public void HitByChargeAttack_OffFixDirectionMove(int e_mod_IsMovingInFixDirection_hash)
        {
            Set_skippingScoreCalculation_Wait();
            isPausingTurnWithAgent = true;

            currentAction = null;
            aIStates.ClearAgentPath();
            agent.isStopped = true;

            void Set_skippingScoreCalculation_Wait()
            {
                LeanTween.value(0, 1, 1f).setOnComplete(OnCompleteWait);

                void OnCompleteWait()
                {
                    skippingScoreCalculation = false;
                    isPausingTurnWithAgent = false;

                    anim.SetBool(e_mod_IsMovingInFixDirection_hash, false);
                }
            }
        }
        #endregion

        #region Enemy Blocking Mod.
        public virtual void OnIsEnemyBlockingMoveAround()
        {
        }

        public virtual void OnBlockingChangeStatus()
        {
        }

        public virtual void OffBlockingReverseStatus()
        {
        }
        
        public virtual bool GetIsWithinBlockingAngle()
        {
            return false;
        }

        public virtual void OnHitBlockingBreak()
        {
        }

        public virtual void PauseEnemyBlockingWhenAttack()
        {
        }

        public virtual void ResumeEnemyBlockingAfterAttack()
        {
        }
        #endregion 

        #region Throw Return Projectile Mod.
        public virtual void SetHasThrownProjectileStatusToTrue()
        {
        }

        public virtual int GetThrowReturnalProjectileHash()
        {
            return 0;
        }

        public virtual void ReturnProjectileWhenHitObstacles()
        {
        }

        public virtual void ParentReturnProjectileToHand()
        {
        }
        #endregion

        #region Area Damage Particle Mod.
        public virtual void HandleDpAttack(DamageParticleAttackMod.DpAttackAnimStateEnum _targetAnimHash)
        {
        }
        #endregion

        #region Public Damage Collider Mods.

        #region L_Leg_DamageColliderMod.
        public virtual void Enable_L_Leg_DamageCollider()
        {
        }

        public virtual void Disable_L_Leg_DamageCollider()
        {
        }
        #endregion

        #region R_Leg_DamageColliderMod.
        public virtual void Enable_R_Leg_DamageCollider()
        {
        }

        public virtual void Disable_R_Leg_DamageCollider()
        {
        }
        #endregion

        #region L_Shoulder_DamageColliderMod.
        public virtual void Enable_L_Shoulder_DamageCollider()
        {
        }

        public virtual void Disable_L_Shoulder_DamageCollider()
        {
        }
        #endregion

        #region FullBody_DamageColliderMod.
        /// R Arm. 
        public virtual void FullBody_DC_Mod_Enable_R_Arm_DC()
        {
        }

        public virtual void FullBody_DC_Mod_Disable_R_Arm_DC()
        {
        }

        /// L Arm. 
        public virtual void FullBody_DC_Mod_Enable_L_Arm_DC()
        {
        }

        public virtual void FullBody_DC_Mod_Disable_L_Arm_DC()
        {
        }

        /// R Leg.
        public virtual void FullBody_DC_Mod_Enable_R_Leg_DC()
        {
        }

        public virtual void FullBody_DC_Mod_Disable_R_Leg_DC()
        {
        }

        /// L Leg.
        public virtual void FullBody_DC_Mod_Enable_L_Leg_DC()
        {
        }

        public virtual void FullBody_DC_Mod_Disable_L_Leg_DC()
        {
        }
        #endregion

        #endregion

        #region Aiming Player Mod.
        public virtual void OnAiming()
        {
        }

        public virtual void OffAiming()
        {
        }

        public virtual void CorrectWeaponTransformWhenAiming()
        {
        }

        public virtual void ReverseWeaponTransformQuitAiming()
        {
        }
        #endregion

        #region Enemy Interactable Mod.

        #region Interactables.
        public virtual void ExecuteInteractable()
        {
        }
        #endregion

        #region AI Actions.
        public virtual void ExecutePowerWeaponInteractable(PowerWeapon_Interactable _powerWeaponInteractable)
        {
        }
        #endregion

        #region Anim Events.
        
        #region Get Power Weapon.
        public virtual void SetIsInGettingInterAnimToTrue()
        {
        }

        public virtual void GetThrowablePowerWeapon()
        {
        }

        public virtual void SetSwitchTargetToInteractableToFalse()
        {
        }
        #endregion

        #region Break Power Weapon.
        public virtual void BreakPowerWeapon()
        {
        }

        public virtual void BreakPowerWeaponByChargeAttack()
        {
        }

        public void PlayPowerWeaponBrokenReaction()
        {
            isTrackingPlayer = false;
            PlayAnimationCrossFade(hashManager.e_javelin_brokenBounceBack_hash, 0.1f, true);
        }

        public virtual void ClearPowerWeaponRefsAfterThrown()
        {
        }
        #endregion

        #region PW Attacks.
        public virtual void SetPowerWeaponMSA_Available()
        {
        }

        public virtual void PowerWeaponDamageColliderStatus(int value)
        {
        }
        #endregion

        #endregion
        
        #region Resets After Broke / Thrown.
        public void ReturnFromThrowablePowerWeapon()
        {
            ResetWeaponRefsAfterPowerWeapon();
            ResetAnimRefsAfterPowerWeapon();
            ChangeToUseFirstWeaponActionHolder();
        }

        void ResetWeaponRefsAfterPowerWeapon()
        {
            currentWeapon = null;
            currentThrowableWeapon = null;
        }

        void ResetAnimRefsAfterPowerWeapon()
        {
            currentCrossFadeLayer = defaultCrossFadeLayer;
            anim.SetBool(hashManager.e_javelin_isEquiped_hash, false);
        }

        void ChangeToUseFirstWeaponActionHolder()
        {
            currentActionHolder = firstWeaponActionHolder;
        }
        #endregion

        #region AI PW Attack Action.
        public virtual void DepletePowerWeaponDuability()
        {
        }
        #endregion
        
        #region Get Status.
        public virtual bool GetIsCurrentPowerWeaponBroke()
        {
            return false;
        }

        public virtual bool GetIsSwitchTargetToInteractable()
        {
            return false;
        }
        #endregion

        #region Set Status.
        public virtual void SetIsCurrentPowerWeaponBrokeToTrue()
        {
        }
        #endregion

        #endregion

        #region Egil Stamina Mod.
        public virtual void EgilStaminaMod_SetNewPhaseData_2ndPhase(EgilStaminaMod_2ndPhase_EP_Data _egilStaminaMod_2P_EP_Data)
        {
        }

        public virtual void EgilStaminaMod_SetNewPhaseData_3rdPhase(EgilStaminaMod_3rdPhase_EP_Data _egilStaminaMod_3P_EP_Data)
        {
        }

        public virtual void InjuredStateTick()
        {
        }

        public virtual void EgilInjuryRecovered()
        {
        }

        public virtual void EgilEnterInjuredState()
        {
        }

        public virtual void InjuredRecovered_OnAggroStateReset()
        {
        }
        #endregion

        #region Egil Execution Mod.
        public virtual void SetIsExecutePresentAttackToFalse()
        {
        }

        public virtual bool GetIsExecutePresentAttack()
        {
            return false;
        }

        public virtual bool GetIsExecutionWait()
        {
            return false;
        }
        
        public virtual void TryCatchPlayerToExecute()
        {
        }

        public virtual void MSA_TryCatchPlayerToExecute()
        {
        }

        public virtual void OnSucessfulCaughtPlayer()
        {
        }

        public virtual void SetExecutionDamageColldierToTrue()
        {
        }

        public virtual void SetExecutionDamageColldierToFalse()
        {
        }
        #endregion

        #region Knock Down Player Mod.
        public virtual bool GetIsKnockDownPlayerWait()
        {
            return false;
        }

        public virtual void SetIsKnockedDownPlayerToTrue()
        {
        }
        #endregion

        #region Egil Kinematic Motion Attack Mod.
        public virtual void Execute_KMJ(_KMA_ActionData _KMA_profile)
        {
        }

        public virtual void PhaseChange_Execute_KMJ(bool _is_KMA_PerliousAttack, bool _isUsedAsMSACombo)
        {
        }

        public virtual void KMJ_ApplyRootMotion_InEvent()
        {
        }

        public virtual void KMA_ApplyRootMotion_InEvent()
        {
        }

        public virtual void KMA_ResetTopDownRotation()
        {
        }
        
        public virtual void Egil_KMA_Mod_SetNewPhaseData(Egil_KMA_Mod_EP_Data _egil_KMA_Mod_EP_Data)
        {

        }

        public virtual bool GetCanExitKMAState()
        {
            return false;
        }

        /// Damage Collider.
        public virtual void Set_KMA_EnemyDamageColliderStatusToTrue()
        {
        }

        public virtual void Set_KMA_EnemyDamageColliderStatusToFalse()
        {
        }

        public virtual void MonitorOverlapBoxDamageCollider()
        {
        }
        #endregion

        #region Enemy Enumerable Phase Mod.

        #region Enemy Change To...Phase.
        public virtual void EnemyChangeTo2ndPhase()
        {
        }

        public virtual void EnemyChangeTo3rdPhase()
        {
        }

        public virtual void EnemyChangeTo4thPhase()
        {
        }
        #endregion

        #region On...Phase Change End.
        public virtual void On2ndPhaseChangeEnd()
        {
        }

        public virtual void On3rdPhaseChangeEnd()
        {
        }

        public virtual void On4thPhaseChangeEnd()
        {
        }
        #endregion

        #region Get Is In...Phase Bool.
        public virtual bool GetIsIn2ndPhaseBool()
        {
            return false;
        }

        public virtual bool GetIsIn3rdPhaseBool()
        {
            return false;
        }

        public virtual bool GetIsIn4thPhaseBool()
        {
            return false;
        }
        #endregion

        #region On Phase Change Set Anim Para.
        public void ChangeTo2ndPhaseSetPara()
        {
            anim.SetBool(hashManager.e_mod_isIn2ndPhase_hash, true);
        }

        public void ChangeTo3rdPhaseSetPara()
        {
            anim.SetBool(hashManager.e_mod_isIn2ndPhase_hash, false);
            anim.SetBool(hashManager.e_mod_isIn3rdPhase_hash, true);
        }

        public void ChangeTo4thPhaseSetPara()
        {
            anim.SetBool(hashManager.e_mod_isIn3rdPhase_hash, false);
            anim.SetBool(hashManager.e_mod_isIn4thPhase_hash, true);
        }
        #endregion

        #region On...Phase Change Reset Status.
        public virtual void OnPhaseChangedResetStatus()
        {
        }
        #endregion

        #region On...Phase Change Execute Passive Action.
        public virtual void On2ndPhaseChangeExecutePassiveAction()
        {
        }

        public virtual void On3rdPhaseChangeExecutePassiveAction()
        {
        }
        #endregion

        #region Egil 3rd Phase Change - Anim Events.
        public void Egil3rdPhaseChange_PlayChangePhase_Chain3_Anim()
        {
            anim.CrossFade(hashManager.egil_ChangePhase2_Chain_3_hash, 0.05f);
            anim.CrossFade(hashManager.egil_3P_empty_override_hash, 0.065f);
        }
        #endregion

        public virtual void EnemyEnumerablePhaseGoesAggroReset_ResetAnimPara()
        {
        }

        public virtual void Egil3rdPhaseChangeEnd_ParentWeaponOnHand()
        {
        }
        #endregion

        #region AI Passive Action.
        
        #region First Throwable.
        public void ReacquireFirstThrowableWithAnim()
        {
            ReacquireFirstThrowable();
            PlayAnimationCrossFade(e_unsheath_First_hash, 0.2f, true);
        }

        public void ReacquireFirstThrowable()
        {
            ThrowableEnemyRuntimeWeapon newThrowable = firstThrowableWeaponPool.Get();

            firstWeapon = newThrowable;
            currentWeapon = newThrowable;
            currentThrowableWeapon = newThrowable;

            newThrowable._ai = this;

            if (newThrowable.isThrowableInited)
            {
                newThrowable.ReSetupThrowableRuntimeWeapon();
            }
            else
            {
                newThrowable.SetupThrowableRuntimeWeapon(firstThrowableWeaponPool);
            }
        }
        #endregion

        #region Second Throwable.
        public virtual void ReacquireSecondThrowableWithAnim()
        {
        }

        public virtual void ReacquireSecondThrowable()
        {
        }
        #endregion

        #region Dual Weapon Mod.
        public void SwitchWeaponReadyPassiveAction()
        {
            currentAction = null;
            isPausingTurnWithAgent = true;

            enemyAttacked = false;
            attackIntervalTimer = 0;

            _isSkippingOnHitAnim = true;

            if (currentWeapon != null)
                PlaySheathAnimation();
        }

        public virtual void SwitchTo_FW_SetStatus()
        {
        }

        public virtual void SwitchTo_SW_SetStatus()
        {
        }
        #endregion

        #region Power Weapon.
        public void EquipPowerWeapon()
        {
            PlayAnimationCrossFade(hashManager.e_javelin_pickup_hash, 0.2f, true);
        }

        public void Equip_FW_AfterUsedPowerWeapon()
        {
            SetCurrentFirstWeaponBeforeAggro();
            PlayAnimation_NoNeglect(e_unsheath_First_hash, true);
        }
        #endregion

        #region Set Current Passive Action.
        public void Set_ReacquireFirstThrowable_PassiveAction()
        {
            currentPassiveAction = aISessionManager.reacquireFirstThrowable;
        }

        public void Set_ReacquireSecondThrowable_PassiveAction()
        {
            currentPassiveAction = aISessionManager.reacquireSecondThrowable;
        }

        public void Set_SwitchWeaponReady_PassiveAction()
        {
            currentPassiveAction = aISessionManager.switchWeaponReady;
        }

        public void Set_SwitchToFirstWeapon_PassiveAction()
        {
            currentPassiveAction = aISessionManager.switchToFirstWeapon;
        }

        public void Set_SwitchToSecondWeapon_PassiveAction()
        {
            currentPassiveAction = aISessionManager.switchToSecondWeapon;
        }

        public void Set_EquipFirstWeaponAfterPW_PassiveAction()
        {
            currentPassiveAction = aISessionManager.equipFWAfterUsedPowerWeapon;
        }

        public void Set_EquipPowerWeapon_PassiveAction()
        {
            currentPassiveAction = aISessionManager.equipPowerWeaponInteractable;
        }
        #endregion

        #endregion

        #region Get Child Bools.
        public virtual bool GetIsEnemyTiredBool()
        {
            return false;
        }
        
        public virtual bool GetIsMovingInFixDirectionBool()
        {
            return false;
        }

        public virtual bool GetIsFixDirectionInCooldownBool()
        {
            return false;
        }

        public virtual bool GetEnemyRolledBool()
        {
            return false;
        }

        public virtual bool GetIsHitCountEventTriggeredBool()
        {
            return false;
        }
        
        public virtual bool GetIsRightStanceBool()
        {
            return true;
        }

        public virtual bool GetCheckCombatStanceBool()
        {
            return false;
        }

        public virtual bool GetTauntedPlayerBool()
        {
            return false;
        }

        public virtual bool GetIsWaitingToParryBool()
        {
            return false;
        }

        public virtual bool GetTriedParryPlayerBool()
        {
            return false;
        }

        public virtual bool GetUsedPerilousAttackBool()
        {
            return false;
        }

        public virtual bool GetIsEnemyBlockingBool()
        {
            return false;
        }

        public virtual bool GetEnemyBlockedBool()
        {
            return false;
        }

        public virtual bool GetHasSpammedBlockingBool()
        {
            return false;
        }

        public virtual bool GetHasSpammedAttackingBool()
        {
            return false;
        }

        public virtual bool GetIsUsingSecondWeaponBool()
        {
            return false;
        }

        public virtual bool GetHasThrownProjectileStatus()
        {
            return false;
        }

        public virtual bool GetIsThrowProjectile()
        {
            return false;
        }

        public virtual bool GetIsEnemyEvolvable()
        {
            return false;
        }

        public virtual bool GetUsedDpAttack()
        {
            return false;
        }
        #endregion

        #region Set Child Bools.
        public virtual void SetEnemyRolledBoolToTrue()
        {
        }

        public virtual void SetTauntedPlayerToTrue()
        {
        }

        public virtual void SetApplyFixDirMoveRootMotionToTrue()
        {
        }

        public virtual void SetIsMovingFixDirectionToTrue()
        {
        }

        public virtual void SetIsRightStanceBool(bool isRightStance)
        {
        }

        public virtual void SetIsWaitingToParryBool(bool isWaitingToParry)
        {
        }

        public virtual void SetUsedPerilousAttackToTrue()
        {
        }
        
        public virtual void SetIsEvolveStartedStatusToTrue()
        {
        }
        #endregion

        #region AI Damage Particle.
        public void Get_Set_CurrentDamageParticle(int id)
        {
            AI_DamageParticle _dp = aISessionManager.GetSinglesAreaDP_ById(id);

            _currentDamageParticle = _dp;
            _dp.ai = this;
        }

        public void SetCurrentDamageParticle(AI_DamageParticle _dp)
        {
            _currentDamageParticle = _dp;
            _dp.ai = this;
        }
        #endregion

        #region AI Indicator.
        public void SetCanPlayIndicatorToTrue()
        {
            canPlayIndicator = true;
        }

        public void SetCanPlayIndicatorToFalse()
        {
            canPlayIndicator = false;
        }

        public virtual void Play_RH_ParryIndicator()
        {
        }

        public virtual void Play_LH_ParryIndicator()
        {
        }

        public virtual void Play_PerliousAttackIndicator()
        {
        }
        #endregion

        #region AI BFX Handler.
        public void Spawn_Regular_BloodFx()
        {
            Get_Spawn_TargetBfxHandler();
            
            StartBloodFXs();

            void Get_Spawn_TargetBfxHandler()
            {
                switch (_hitSourceAttackRefs._attackPhysicalType)
                {
                    case Player_AttackRefs.AttackPhysicalTypeEnum.Strike:

                        switch (_hitSourceAttackRefs._attackActionType)
                        {
                            case Player_AttackRefs.AttackActionTypeEnum.Normal:
                                // Medium Strike.
                                _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_M_Strike_Bfx_ID());
                                break;

                            case Player_AttackRefs.AttackActionTypeEnum.Hold:

                                if (playerStates._hasHoldAtkReachedMaximum)
                                {
                                    // Large Strike.
                                    _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_L_Strike_Bfx_ID());
                                }
                                else
                                {
                                    // Medium Strike.
                                    _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_M_Strike_Bfx_ID());
                                }
                                break;

                            case Player_AttackRefs.AttackActionTypeEnum.Charged:
                                // Large Strike.
                                _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_L_Strike_Bfx_ID());
                                break;
                        }

                        SpawnStrikeBfxHandler();
                        break;

                    case Player_AttackRefs.AttackPhysicalTypeEnum.Slash:

                        switch (_hitSourceAttackRefs._attackActionType)
                        {
                            case Player_AttackRefs.AttackActionTypeEnum.Normal:
                                // Medium Slash.
                                _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_M_Slash_Bfx_ID());
                                break;

                            case Player_AttackRefs.AttackActionTypeEnum.Hold:

                                if (playerStates._hasHoldAtkReachedMaximum)
                                {
                                    // Large Slash.
                                    _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_L_Slash_Bfx_ID());
                                }
                                else
                                {
                                    // Medium Slash.
                                    _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_M_Slash_Bfx_ID());
                                }
                                break;

                            case Player_AttackRefs.AttackActionTypeEnum.Charged:
                                // Large Slash.
                                _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_L_Slash_Bfx_ID());
                                break;
                        }

                        SpawnSlashBfxHandler();
                        break;

                    case Player_AttackRefs.AttackPhysicalTypeEnum.Thrust:

                        switch (_hitSourceAttackRefs._attackActionType)
                        {
                            case Player_AttackRefs.AttackActionTypeEnum.Normal:
                                // Medium Thrust.
                                _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_M_Thrust_Bfx_ID());
                                break;

                            case Player_AttackRefs.AttackActionTypeEnum.Hold:

                                if (playerStates._hasHoldAtkReachedMaximum)
                                {
                                    // Large Thrust.
                                    _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_L_Thrust_Bfx_ID());
                                }
                                else
                                {
                                    // Medium Thrust.
                                    _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_M_Thrust_Bfx_ID());
                                }
                                break;

                            case Player_AttackRefs.AttackActionTypeEnum.Charged:
                                // Large Thrust.
                                _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_L_Thrust_Bfx_ID());
                                break;
                        }

                        SpawnThrustBfxHandler();
                        break;
                }
            }
        }

        public void Spawn_Execution_1stHit_BloodFx()
        {
            Get_Spawn_TargetBfxHandler();

            StartBloodFXs();

            void Get_Spawn_TargetBfxHandler()
            {
                switch (_p_executionProfile._1st_hit_phys_type)
                {
                    case Player_ExecutionProfile.ExecutionPhysicalTypeEnum.Strike:

                        // Medium Strike.
                        _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_M_Strike_Bfx_ID());
                        SpawnStrikeBfxHandler();
                        break;

                    case Player_ExecutionProfile.ExecutionPhysicalTypeEnum.Slash:

                        // Medium Slash.
                        _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_M_Slash_Bfx_ID());
                        SpawnSlashBfxHandler();
                        break;

                    case Player_ExecutionProfile.ExecutionPhysicalTypeEnum.Thrust:

                        // Medium Thrust.
                        _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_M_Thrust_Bfx_ID());
                        SpawnThrustBfxHandler();
                        break;
                }
            }
        }

        public void Spawn_Execution_2ndHit_BloodFx_TwoHitVer()
        {
            Get_Spawn_TargetBfxHandler();

            StartBloodFXs();

            void Get_Spawn_TargetBfxHandler()
            {
                // Large Strike.
                switch (_p_executionProfile._2nd_hit_phys_type)
                {
                    case Player_ExecutionProfile.ExecutionPhysicalTypeEnum.Strike:

                        _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_L_Strike_Bfx_ID());
                        SpawnStrikeBfxHandler();
                        break;

                    case Player_ExecutionProfile.ExecutionPhysicalTypeEnum.Slash:

                        _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_L_Slash_Bfx_ID());
                        SpawnSlashBfxHandler();
                        break;

                    case Player_ExecutionProfile.ExecutionPhysicalTypeEnum.Thrust:

                        _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_L_Thrust_Bfx_ID());
                        SpawnThrustBfxHandler();
                        break;
                }
            }
        }

        public void Spawn_Execution_2ndHit_BloodFx_ThreeHitVer()
        {
            Get_Spawn_TargetBfxHandler();

            StartBloodFXs();

            void Get_Spawn_TargetBfxHandler()
            {
                // Medium Strike.
                switch (_p_executionProfile._2nd_hit_phys_type)
                {
                    case Player_ExecutionProfile.ExecutionPhysicalTypeEnum.Strike:

                        _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_M_Strike_Bfx_ID());
                        SpawnStrikeBfxHandler();
                        break;

                    case Player_ExecutionProfile.ExecutionPhysicalTypeEnum.Slash:

                        _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_M_Slash_Bfx_ID());
                        SpawnSlashBfxHandler();
                        break;

                    case Player_ExecutionProfile.ExecutionPhysicalTypeEnum.Thrust:

                        _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_M_Thrust_Bfx_ID());
                        SpawnThrustBfxHandler();
                        break;
                }
            }
        }

        public void Spawn_Execution_3rdHit_BloodFx()
        {
            Get_Spawn_TargetBfxHandler();

            StartBloodFXs();

            void Get_Spawn_TargetBfxHandler()
            {
                switch (_p_executionProfile._3rd_hit_phys_type)
                {
                    case Player_ExecutionProfile.ExecutionPhysicalTypeEnum.Strike:

                        // Large Strike.
                        _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_L_Strike_Bfx_ID());
                        SpawnStrikeBfxHandler();
                        break;

                    case Player_ExecutionProfile.ExecutionPhysicalTypeEnum.Slash:

                        // Large Slash.
                        _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_L_Slash_Bfx_ID());
                        SpawnSlashBfxHandler();
                        break;

                    case Player_ExecutionProfile.ExecutionPhysicalTypeEnum.Thrust:

                        // Large Thrust.
                        _cur_PoolableBfxHandler = aISessionManager.GetPoolableAIBfxHandler_ById(playerStates.Get_Random_L_Thrust_Bfx_ID());
                        SpawnThrustBfxHandler();
                        break;
                }
            }
        }
        
        void SpawnStrikeBfxHandler()
        {
            Transform _newBfxHandlerTrans = _cur_PoolableBfxHandler.transform;

            //Rotation
            _newBfxHandlerTrans.localRotation = Quaternion.LookRotation(_hitSourceColliderTransform.position - _BfxStickableTrans.position);
            _newBfxHandlerTrans.Rotate(0, Random.Range(-46, 46), Random.Range(-46, 46), Space.Self);

            //Position
            _newBfxHandlerTrans.parent = _BfxStickableTrans;
            _newBfxHandlerTrans.localPosition = vector3Zero;
        }

        void SpawnSlashBfxHandler()
        {
            Transform _newBfxHandlerTrans = _cur_PoolableBfxHandler.transform;

            //Rotation
            _newBfxHandlerTrans.eulerAngles = _hitSourceColliderTransform.eulerAngles;
            _newBfxHandlerTrans.Rotate(Random.Range(-46, 46), Random.Range(-46, 46), 0, Space.Self);

            //Position
            _newBfxHandlerTrans.parent = _BfxStickableTrans;
            _newBfxHandlerTrans.localPosition = vector3Zero;
        }

        void SpawnThrustBfxHandler()
        {
            Transform _newBfxHandlerTrans = _cur_PoolableBfxHandler.transform;

            //Rotation
            _newBfxHandlerTrans.localRotation = Quaternion.LookRotation(_hitSourceColliderTransform.position - _BfxStickableTrans.position);

            //Position
            _newBfxHandlerTrans.parent = _BfxStickableTrans;
            _newBfxHandlerTrans.localPosition = vector3Zero;
        }

        void StartBloodFXs()
        {
            _cur_PoolableBfxHandler.Start_AI_Bfx();
        }
        #endregion

        #region AI Group.
        public void DeactivateEnemyAnimInstant()
        {
            if (isDead || aIStates.isAggros)
                return;

            anim.SetFloat(vertical_hash, 0);
            anim.enabled = false;
        }

        public void DeactivateEnemyAnimDelay()
        {
            if (isDead || aIStates.isAggros)
                return;

            anim.SetFloat(vertical_hash, 0);
            StartCoroutine(DisableAnimatorAfterSecond());
        }

        IEnumerator DisableAnimatorAfterSecond()
        {
            //yield return new WaitForSeconds(1f);
            yield return new WaitForEndOfFrame();
            anim.enabled = false;
        }
        #endregion

        #region Savable Enemy States.
        public void KillSavedStateEnemy()
        {
            isDead = true;
            isDeadFromSave = true;

            if (gameObject.activeSelf)
            {
                /// If the Dead Enemy is currently in the same group with player.
                PlayOnDeathAnimation();
            }
            else
            {
                /// Otherwise kill them without animation.
                aIStates.OnEnemyDeath_SavedState();
            }
        }
        #endregion

        /// RANDOMIZE.

        void RandomizeWithAddonValue(float addOnAmount, float standardAmount, ref float finalizedAmount)
        {
            bool isAdding = false;
            bool netual = false;

            float randomResult = Random.Range(1, 4);
            if (randomResult == 1)
            {
                isAdding = true;
            }
            else if (randomResult == 2)
            {
                netual = true;
            }

            float randomizedAmount = Random.Range(0, addOnAmount + 0.1f);
            if (isAdding)
            {
                finalizedAmount = standardAmount + randomizedAmount;
            }
            else
            {
                if (netual)
                {
                    finalizedAmount = standardAmount;
                }
                else
                {
                    finalizedAmount = standardAmount - randomizedAmount;
                }

            }
        }

        void RandomizeAttackInterval_SpecificRange()
        {
            finalizedAttackIntervalRate = Random.Range(minAttackIntervalRate, maxAttackIntervalRate);
        }
    }
    
    public enum EnemyTypeEnum
    {
        Egil,
        Bomber,
        Warrior,
        Marksman,
        Swordsman,
        Shieldman,
        Lancer,
        Thief,
        Assassin
    }

    public enum EnemyAttackPowerWeaknessTypeEnum
    {
        Magical,
        Fire,
        Lightning,
        Dark,
        Physical
    }

    public enum AIElementalTypeEnum
    {
        Physical,
        Magical,
        Fire,
        Lightning,
        Dark
    }

    public enum LockOnLocomotionTypeEnum
    {
        forward,
        forward_left,
        forward_right,
        left,
        right,
        backward_left,
        backward_right,
        backward
    }

    public enum DirectionOptionsTypeEnum
    {
        front,
        right,
        left,
        back,
        whole360        /// This options will ignore 'Angle Options' Thershold.
    }

    public enum AngleOptionsTypeEnum
    {
        normal_30,
        wide_45,
        ultra_60,
        ultraWide_120,
        ignored
    }

    /*
     * public void ParentEnemyFirstWeaponUnderHand(EnemyWeapon weapon, HumanBodyBones targetBone, bool isLeft = false)
        {
            Transform bone = aiStates.anim.GetBoneTransform(targetBone);
            weapon.runtimeWeapon.modelInstance.transform.parent = bone;
            if (isLeft)
            {
                weapon.runtimeWeapon.modelInstance.transform.localPosition = weapon.leftHandPosition.pos;
                weapon.runtimeWeapon.modelInstance.transform.localEulerAngles = weapon.leftHandPosition.eulers;
            }
            else
            {
                weapon.runtimeWeapon.modelInstance.transform.localPosition = aiStates.vector3Zero;
                weapon.runtimeWeapon.modelInstance.transform.localEulerAngles = aiStates.vector3Zero;
            }

            weapon.runtimeWeapon.modelInstance.transform.localScale = aiStates.vector3One;
        }
     */

    /* RB:
     * ResetRbForFalling();
     * MonitorEnemyRigidbodyAndCollider();
     * aiStates.InitRigidbody();
     * aiStates.ResetRbForLanding();
     * aiStates.ApplyFallbackRootMotion();
     * aiStates.ApplyRollRootMotion();
     * aiStates.ApplyAttackRootMotion();
     * aiStates.
     */

    #region Move Toward.
    //public virtual void SetMoveTowardDestination()
    //{
    //    if (isMovingTowardPlayer && distanceToTarget <= aIStates.applyMoveTowardPredictDistance)
    //    {
    //        Vector3 _targetDir = _playerStates.moveDirection * aIStates.moveTowardPredictAmount_h;
    //        _targetDir.z = (_playerStates.moveDirection * aIStates.moveTowardPredictAmount_v).z;

    //        aIStates.agent.SetDestination(targetPos + _targetDir);
    //    }
    //    else
    //    {
    //        aIStates.agent.SetDestination(targetPos);
    //    }
    //}
    #endregion
}