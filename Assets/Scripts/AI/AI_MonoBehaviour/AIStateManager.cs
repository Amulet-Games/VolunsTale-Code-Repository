using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Text;
using System;

namespace SA
{
    public class AIStateManager : MonoBehaviour
    {
        #region Serialization.
        [Header("Serialization.")]
        public string savableId;
        #endregion
        
        #region Turn Root Motion.
        [Header("Patrol Turning.")]
        public float applyMoveTowardPredictDistance = 3.5f;
        public float patrolTurnSpeed = 15;
        #endregion

        #region Anim Move Root Motion.
        [Header("Anim Move RM Type.")]
        [ReadOnlyInspector] public AI_AnimMoveRM_Type _animMoveRmType;
        #endregion

        #region Invincible Config.
        [Header("Invincible Config.")]
        #endregion

        #region Late Ticks.
        [Header("Late Ticks.")]
        public StateActions[] lateTickActions;
        [ReadOnlyInspector, SerializeField] int _lateTickActionsLength;
        #endregion

        #region Enemy Health Bar.
        [Header("Enemy Health Bar.")]
        public AIDisplayManager aiDisplayManager;
        #endregion
        
        #region AI Patrol Route.
        [Header("AI Patrol Config.")]
        public int patrolRouteId;
        public bool reverseRouteAtEndPoint;             /* If patrol point is less than 3, it will set to false in patrol  */ 
        public float returnToPatrolWaitRate = 3;

        [Space(12)]
        public float patrolAccelSpeed;
        public float patrolMoveSpeed;
        public float patrolStopDistance = 0.25f;

        [Header("AI Patrol Status.")]
        [ReadOnlyInspector] public int _currentPointId;
        [ReadOnlyInspector] public float _currentPatrolWaitTimer;
        [ReadOnlyInspector] public bool _isStopPatrolTurning;
        [ReadOnlyInspector] public bool _isReversingRoute;
        [ReadOnlyInspector] public bool _hasPatrolRoute;
        [ReadOnlyInspector] public bool _hasArrivedToPoint;
        [ReadOnlyInspector] public AI_PatrolPoint currentPatrolPoint;
        [ReadOnlyInspector] public AI_PatrolRoute currentPatrolRoute;
        #endregion

        #region AI Group, Session.
        [Header("AI Group, Session.")]
        [ReadOnlyInspector] public bool isAggros;
        [ReadOnlyInspector] public AI_Group _belongedAIGroup;
        [ReadOnlyInspector] public AIGroupManagable _aiGroupManagable;
        [ReadOnlyInspector] public AISessionManager _aiSessionManager;
        #endregion

        #region Refs.
        [Header("Refs")]
        [ReadOnlyInspector] public State currentState;
        [ReadOnlyInspector] public AIManager aiManager;
        [ReadOnlyInspector] public Animator anim;
        [ReadOnlyInspector] public Rigidbody e_rb;
        [ReadOnlyInspector] public Collider e_hitBox_Collider;
        [ReadOnlyInspector] public CapsuleCollider e_root_Collider;
        [ReadOnlyInspector] public NavMeshAgent agent;
        [ReadOnlyInspector] public EnemyIKHandler iKHandler;
        [ReadOnlyInspector] public LayerManager _layerManager;
        [ReadOnlyInspector] public HashManager _hashManager;
        #endregion

        #region Status.
        [Header("Status.")]
        [ReadOnlyInspector] public int _frameCount;
        [ReadOnlyInspector] public float _delta;
        [ReadOnlyInspector] public float _fixedDelta;
        [ReadOnlyInspector] public Transform mTransform;

        [Space(5)]
        [ReadOnlyInspector] public bool _hasFoundPlayer;
        [ReadOnlyInspector] public bool _isLockonState;
        [ReadOnlyInspector] public bool _isMovementChanged;
        [ReadOnlyInspector] public bool _hasSubToCheckpointRefreshEvent;

        [Space(5)]
        [ReadOnlyInspector] public bool applyControllerCameraYMovement;
        [ReadOnlyInspector] public bool applyControllerCameraZoom;
        #endregion

        #region Is Grounded.
        [Header("Is Grounded.")]
        public float fallVelocity;
        [ReadOnlyInspector] public bool isGrounded;
        [ReadOnlyInspector] public bool ignoreGroundCheck;
        #endregion

        #region Ragdolls.
        [Header("Ragdolls")]
        [ReadOnlyInspector] public List<Rigidbody> ragdollRigids = new List<Rigidbody>();
        [ReadOnlyInspector] public List<Collider> ragdollColliders = new List<Collider>();
        [ReadOnlyInspector] public int m_ragdollCount;
        #endregion

        #region Non Serialized.

        #region Vector3s.
        [NonSerialized] public Vector3 vector3Zero = new Vector3(0, 0, 0);
        #endregion

        #region Anim State Hash.
        [HideInInspector] public int e_IsInteracting_hash;
        [HideInInspector] public int e_IsGrounded_hash;
        #endregion

        #endregion

        #region Deletable.
        private StringBuilder strBuilder;
        #endregion

        public void Init()
        {
            InitAIStateManager();
            
            InitBelongedAIGroup();

            InitLayerManager();
            
            InitRigidbody();

            InitAnimMoveRmType();

            InitHitBoxCollider();
            
            InitRagdoll();

            InitAnimator();

            InitAIStateAnimHash();

            InitAgent();

            InitRootCollider();

            InitAddAIStatesToSavable();
        }

        public void Setup()
        {
            SetupAIManager();

            SetupIKHandler();

            SetupAnimatorHook();

            SetupAIDisplayManager();
            
            SetupAIPatrolRoute();
            
            SetupAIMods();
            
            SetupStringBuilder();

            GetLateTickActionsLength();

            aiManager.SetupAIWeapon();
        }
        
        public void Tick()
        {
            UpdateDeltas();
            currentState.AITick(this);
        }

        public void FixedTick()
        {
            UpdateFixedDeltas();
            currentState.AIFixedTick(this);
        }

        public void LateTick()
        {
            for (int i = 0; i < _lateTickActionsLength; i++)
            {
                lateTickActions[i].AIExecute(this);
            }
        }
        
        public string GetErrorMessage(string str1 = null, string str2 = null, string str3 = null, string str4 = null)
        {
            strBuilder.Clear();
            strBuilder.Append(str1).Append(str2).Append(str3).Append(str4);
            return strBuilder.ToString();
        }

        #region Deltas.
        void UpdateDeltas()
        {
            _frameCount = _aiSessionManager._frameCount;
            aiManager._frameCount = _frameCount;

            _delta = _aiSessionManager._delta;
            aiManager._delta = _delta;
        }

        void UpdateFixedDeltas()
        {
            _fixedDelta = _aiSessionManager._fixedDelta;
        }
        #endregion

        #region Init.
        private void InitAIStateManager()
        {
            currentState = GameManager.singleton.enemyPatrolState;
            aiManager = GetComponent<AIManager>();
            mTransform = transform;
        }

        private void InitBelongedAIGroup()
        {
            isAggros = false;

            _aiGroupManagable = _belongedAIGroup._aiGroupManagable;
            _aiSessionManager = _aiGroupManagable._aiSessionManager;
        }

        private void InitLayerManager()
        {
            _layerManager = LayerManager.singleton;
            gameObject.layer = _layerManager.enemyLayer;
        }
        
        private void InitRigidbody()
        {
            e_rb = GetComponent<Rigidbody>();
            e_rb.angularDrag = 999;
            e_rb.drag = 4;
            e_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            isGrounded = true;
        }

        private void InitAnimMoveRmType()
        {
            _animMoveRmType = AI_AnimMoveRM_Type.Null;
        }

        private void InitHitBoxCollider()
        {
            e_hitBox_Collider = GetComponent<Collider>();
            e_hitBox_Collider.isTrigger = true;
        }
        
        private void InitAnimator()
        {
            anim = transform.GetChild(0).GetComponent<Animator>();
            anim.enabled = true;
        }

        private void InitAIStateAnimHash()
        {
            HashManager _hashManager = HashManager.singleton;
            e_IsInteracting_hash = _hashManager.e_IsInteracting_hash;
            e_IsGrounded_hash = _hashManager.e_IsGrounded_hash;

            aiManager.hashManager = _hashManager;
            aiManager.SetupAnimHash();
        }

        private void InitAgent()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updatePosition = false;
        }

        private void InitRootCollider()
        {
            e_root_Collider = anim.GetComponent<CapsuleCollider>();
            e_root_Collider.isTrigger = false;
        }

        private void InitAddAIStatesToSavable()
        {
            SavableManager.singleton._aIStates.Add(this);
        }
        #endregion

        #region Setup.
        private void SetupAIManager()
        {
            aiManager.aIStates = this;
            aiManager.Setup();
        }

        private void SetupIKHandler()
        {
            iKHandler = anim.GetComponent<EnemyIKHandler>();
            iKHandler.Init(this);
        }
        
        private void SetupAnimatorHook()
        {
            aiManager.SetupAnimatorHook();
        }

        private void SetupAIDisplayManager()
        {
            aiDisplayManager.Setup(aiManager);
        }
        
        public void SetupAIPatrolRoute()
        {
            GetTargetPatrolRoute();
            CheckIsAIHasPatrolRoute();
            ChangeAgentStats_Patrol();
            WrapAgentToInitPoint();
        }
        
        private void SetupAIMods()
        {
            aiManager.SetupAIMods();
        }
        
        private void SetupStringBuilder()
        {
            strBuilder = new StringBuilder();
        }

        private void GetLateTickActionsLength()
        {
            _lateTickActionsLength = lateTickActions.Length;
        }
        #endregion

        #region On / Off State Reset.
        public void OnEnterAggroFacedPlayerReset()
        {
            // IK.
            iKHandler.isEnemyForwardIK = false;

            // Status.
            _hasFoundPlayer = true;

            // Agent.
            _hasArrivedToPoint = false;

            // Checkpoint Action.
            SubscribeCheckpointRefreshEvent();

            if (!isAggros)
            {
                AddToAggros();
            }
        }

        public void OffExitAggroFacedPlayerReset()
        {
            iKHandler.isUsingIK = true;
            iKHandler.isEnemyForwardIK = true;

            _hasFoundPlayer = false;

            RemoveFromAggros();
            _aiGroupManagable.AddToRemnants(this);

            // AI Patrol Point.
            OffFacedPlayerResetPatrolStatus();
        }
        
        public void PlayerDied_OffExitAggroFacePlayerReset()
        {
            iKHandler.OnPlayerDeathExitAggroTurnOffIK();

            RemoveFromAggros();
            _hasFoundPlayer = false;

            // AI Patrol Point.
            OffFacedPlayerResetPatrolStatus();
        }

        public void OnAggroStateResets()
        {
            // Player Camera Movement.
            applyControllerCameraYMovement = false;
            applyControllerCameraZoom = false;

            // Agent.
            ReEnableAgent();
        }
        
        public void OnWaitForAnimationEndResets()
        {
            if (agent.hasPath)
            {
                ClearAgentPath();
            }
        }
        #endregion
        
        #region Is Enter / Exit Wait For Animation End Transition.
        public bool IsEnterWaitForAnimationEndTransition()
        {
            if (anim.GetBool(e_IsInteracting_hash))
            {
                OnWaitForAnimationEndResets();
                return true;
            }

            return false;
        }

        public bool IsExitWaitForAnimationEndTransition()
        {
            if (!anim.GetBool(e_IsInteracting_hash))
            {
                OnAggroStateResets();
                aiManager.OnAggroStateResets();
                return true;
            }

            return false;
        }
        #endregion

        #region Is Enter / Exit KMA WaitForAnimationFinishTransition.
        public bool IsEnter_KMA_WaitForAnimationFinishTransition()
        {
            return anim.GetBool(e_IsInteracting_hash) && !aiManager.GetCanExitKMAState();
        }

        public bool IsExit_KMA_WaitForAnimationFinishTransition()
        {
            return !anim.GetBool(e_IsInteracting_hash);
        }
        #endregion

        #region Is Enter Injured WaitForAnimationFinishTransition.
        public bool IsEnter_Injured_WaitForAnimationFinishTransition()
        {
            return anim.GetBool(e_IsInteracting_hash);
        }

        public bool IsExit_Injured_WaitForAnimationFinishTransition()
        {
            if (!anim.GetBool(e_IsInteracting_hash))
            {
                OnAggroStateResets();
                aiManager.OnAggroStateResets();
                return true;
            }

            return false;
        }

        void OnInjuredRecovered_AggroStateReset()
        {
            ReEnableAgent();

            aiManager.InjuredRecovered_OnAggroStateReset();
        }
        #endregion
        
        #region Agent General.
        public void GetCurrentAgentVelocity()
        {
            aiManager._currentAgentVelocity = agent.velocity.magnitude;
            //Debug.DrawRay(mTransform.position, agent.velocity, Color.black);
            //Debug.Log("agent.velocity = " + _currentAgentVelocity);
            //Debug.Log("agent.desiredVelocity = " + agent.desiredVelocity.magnitude);
        }

        public void ReEnableAgent()
        {
            agent.enabled = false;
            agent.enabled = true;
        }

        public void ClearAgentPath()
        {
            agent.path.ClearCorners();
            agent.ResetPath();
            agent.velocity = vector3Zero;
        }
        #endregion
        
        #region Patrol With Agent
        public void PatrolWithAgent()
        {
            if (!isGrounded)
                return;

            TurnWithPatrol();
            aiManager.AnimWithPatrol();
            MoveWithPatrol();
        }

        #region Turn With Patrol.
        void TurnWithPatrol()
        {
            if (_isStopPatrolTurning)
                return;

            // Stop enemy locomotion if he is too close to destination.
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                LooksTowardPatrolPoint();
            }
            else if (!agent.isStopped)
            {
                LooksTowardPatrolRoute();
            }
        }

        void LooksTowardPatrolRoute()
        {
            Vector3 _dirToPatrolRoute = agent.steeringTarget - mTransform.position;
            _dirToPatrolRoute.y = 0;

            if (_dirToPatrolRoute != vector3Zero)
                mTransform.localRotation = Quaternion.Lerp(mTransform.localRotation, Quaternion.LookRotation(_dirToPatrolRoute), patrolTurnSpeed * _delta);
        }

        void LooksTowardPatrolPoint()
        {
            Vector3 _dirToPatrolPoint = currentPatrolPoint.transform.forward;
            _dirToPatrolPoint.y = 0;

            if (_dirToPatrolPoint != vector3Zero)
                mTransform.localRotation = Quaternion.Lerp(mTransform.localRotation, Quaternion.LookRotation(_dirToPatrolPoint), patrolTurnSpeed * _delta);
        }
        #endregion
        
        #region Move With Patrol.
        void MoveWithPatrol()
        {
            UpdateAgentSpeedPatrol();

            UpdatePatrolAgent();
        }

        void UpdateAgentSpeedPatrol()
        {
            float t = anim.GetFloat(aiManager.vertical_hash) / aiManager.patrolLocoAnimValue;
            agent.acceleration = patrolAccelSpeed * t;
            agent.speed = patrolMoveSpeed * t;
        }

        void UpdatePatrolAgent()
        {
            if (agent.pathPending)
                return;

            //Debug.Log("agent.remainingDistance = " + agent.remainingDistance);
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                StoppingPatrol();
            }
            else
            {
                RegularPatrol();
            }
        }
        
        void StoppingPatrol()
        {
            SetIsStoppingPatrolingToTrue();

            if (_hasPatrolRoute)
            {
                WaitForSearching();
            }
        }
        
        void RegularPatrol()
        {
            mTransform.position = agent.nextPosition;
        }

        void WaitForSearching()
        {
            //Debug.Log("searching...");
            _currentPatrolWaitTimer -= _delta;
            if (_currentPatrolWaitTimer <= 0)
            {
                SetNewCurrentPatrolPoint();
            }
        }

        void SetNewCurrentPatrolPoint()
        {
            if (reverseRouteAtEndPoint)
            {
                if (_isReversingRoute)
                {
                    _currentPointId--;
                    if (_currentPointId == 0)
                    {
                        _isReversingRoute = false;
                    }
                }
                else
                {
                    _currentPointId++;
                    if (_currentPointId == currentPatrolRoute.patrolPoints.Length - 1)
                    {
                        _isReversingRoute = true;
                    }
                }
            }
            else
            {
                _currentPointId++;
                if (_currentPointId == currentPatrolRoute.patrolPoints.Length)
                {
                    _currentPointId = 0;
                }
            }

            currentPatrolPoint = currentPatrolRoute.patrolPoints[_currentPointId];

            SetIsStoppingPatrolingToFalse();
        }
        
        void SetIsStoppingPatrolingToTrue()
        {
            if (!agent.isStopped)
            {
                agent.isStopped = true;
                agent.velocity = vector3Zero;

                _isStopPatrolTurning = true;
                _hasArrivedToPoint = true;
            }
        }

        void SetIsStoppingPatrolingToFalse()
        {
            _currentPatrolWaitTimer = currentPatrolPoint.onPointSearchWaitRate;

            _isStopPatrolTurning = false;
            _hasArrivedToPoint = false;

            agent.isStopped = false;
            agent.SetDestination(currentPatrolPoint._localPosition);
        }
        #endregion

        #endregion

        #region Anim With Agent - isGrounded Check.
        public void AnimWithAgent()
        {
            if (!isGrounded)
                return;

            aiManager.AnimWithAgent();
        }
        #endregion

        #region Move With Agent - isGrounded Check.
        public void MoveWithAgent()
        {
            if (!isGrounded)
                return;

            aiManager.MoveWithAgent();
        }
        #endregion

        #region Root Motions.
        /// Fixed Ticks.
        public void ApplyAttackArtifiMotion()
        {
            e_rb.velocity = mTransform.forward * (_fixedDelta * aiManager.CalculateAttackArtificalMotion());
        }

        public void ApplyFallingRootMotions()
        {
            if (!isGrounded)
            {
                e_rb.velocity = aiManager.vector3Up * (fallVelocity * _fixedDelta * -1);
            }
        }

        public void ApplyParryExecutionRootMotion()
        {
            e_rb.velocity = aiManager.currentExecutionVelocity * anim.deltaPosition;
        }

        /// On Anim Move.
        public void Apply_AnimMoveRootMotion_ByType()
        {
            switch (_animMoveRmType)
            {
                case AI_AnimMoveRM_Type.Null:
                    break;
                case AI_AnimMoveRM_Type.Attack:
                    ApplyAttackRootMotion();
                    break;
                case AI_AnimMoveRM_Type.Roll:
                    ApplyRollRootMotion();
                    break;
                case AI_AnimMoveRM_Type.Fallback:
                    ApplyFallbackRootMotion();
                    break;
                case AI_AnimMoveRM_Type.KnockDown:
                    ApplyKnockDownRootMotion();
                    break;
            }
        }

        void ApplyAttackRootMotion()
        {
            Vector3 tarVel = anim.deltaPosition / _delta;
            e_rb.velocity = aiManager.CalculateAttackRootMotion() * tarVel;
        }

        void ApplyRollRootMotion()
        {
            Vector3 tarVel = anim.deltaPosition / _delta;
            e_rb.velocity = aiManager.currentRollVelocity * tarVel;
        }

        void ApplyFallbackRootMotion()
        {
            Vector3 tarVel = anim.deltaPosition / _delta;
            e_rb.velocity = aiManager.currentFallbackVelocity * tarVel;
        }

        void ApplyKnockDownRootMotion()
        {
            Vector3 tarVel = anim.deltaPosition / _delta;
            e_rb.velocity = aiManager.currentKnockdownVelocity * tarVel;
        }

        #region Set Type.
        public void Set_AnimMoveRmType_ToNull()
        {
            _animMoveRmType = AI_AnimMoveRM_Type.Null;
        }

        public void Set_AnimMoveRmType_ToAttack()
        {
            _animMoveRmType = AI_AnimMoveRM_Type.Attack;
        }

        public void Set_AnimMoveRmType_ToRoll()
        {
            _animMoveRmType = AI_AnimMoveRM_Type.Roll;
        }

        public void Set_AnimMoveRmType_ToFallback()
        {
            _animMoveRmType = AI_AnimMoveRM_Type.Fallback;
        }

        public void Set_AnimMoveRmType_ToKnockDown()
        {
            _animMoveRmType = AI_AnimMoveRM_Type.KnockDown;
        }
        #endregion

        #endregion

        #region On Enemy Death.
        public void OnEnemyDeath()
        {
            ResetRefsBeforeDeath();
            EnableRagDoll();

            StartCoroutine("DisableAIState");
        }

        public void OnDeathSwitchLayer()
        {
            gameObject.layer = _layerManager.enemyDeadLayer;
        }
        
        void ResetRefsBeforeDeath()
        {
            /// Rigidbody / Collider layer.
            e_rb.isKinematic = true;
            e_hitBox_Collider.enabled = false;
            e_root_Collider.enabled = false;

            /// Animator / IK.
            anim.enabled = false;
            iKHandler.OnEnemyDeath();

            /// AI Manager.
            aiManager.OnEnemyDeath_AfterAnimPlay();

            /// Agent layer.
            OnEnemyDeathAgent();

            /// AI Health Display.
            aiDisplayManager.OnDeath();
        }

        void OnEnemyDeathAgent()
        {
            ReEnableAgent();
            ClearAgentPath();
            agent.isStopped = true;
            agent.enabled = false;
        }

        IEnumerator DisableAIState_Coroutine()
        {
            yield return new WaitForEndOfFrame();
            DisableAIState();
        }

        void DisableAIState()
        {
            // AI Group.
            if (isAggros)
            {
                RemoveFromAggros();
            }
            
            // Bools.
            _hasFoundPlayer = false;
            iKHandler.isEnemyForwardIK = true;

            // Disable State.
            enabled = false;
        }
        #endregion

        #region On Enemy Death - Executed
        public void OnEnemyDeath_Executed()
        {
            OnDeathSwitchLayer();
            ResetRefsBeforeDeath_Executed();
            EnableRagDoll();
            aiDisplayManager.OnDeath();

            StartCoroutine("DisableAIState");
        }
        
        void ResetRefsBeforeDeath_Executed()
        {
            /// Animator layer.
            anim.enabled = false;
            iKHandler.OnEnemyDeath();

            /// AI Manager.
            aiManager.OnEnemyDeath_AfterAnimPlay();

            /// Agent layer.
            OnEnemyDeathAgent();
        }
        #endregion

        #region On Enemy Death - Saved State.
        public void OnEnemyDeath_SavedState()
        {
            OnDeathSwitchLayer();
            ResetRefsBeforeDeath_SavedState();
            OnEnemyDeath_SavedState_Agent();
            EnableRagDoll();

            DisableAIState();

            void OnEnemyDeath_SavedState_Agent()
            {
                agent.enabled = false;
            }
        }

        void ResetRefsBeforeDeath_SavedState()
        {
            /// Rigidbody / Collider layer.
            e_rb.isKinematic = true;
            e_hitBox_Collider.enabled = false;
            e_root_Collider.enabled = false;

            /// Animator / IK.
            anim.enabled = false;
            iKHandler.OnEnemyDeath();

            /// AI Manager.
            aiManager.OnEnemyDeath_AfterAnimPlay();
        }
        #endregion

        #region On Enemy Revive.
        void OnEnemyRevive()
        {
            ResetRefsBeforeRevive();
            DisableRagDoll();

            if (gameObject.activeSelf)
            {
                StartCoroutine("ReEnableAIState");
            }
            else
            {
                OnEnemyReviveAIState();
            }
        }

        void ResetRefsBeforeRevive()
        {
            // AIManager layer.
            e_rb.isKinematic = false;
            e_hitBox_Collider.enabled = true;
            e_root_Collider.enabled = true;

            // Animator layer.
            anim.enabled = true;
            iKHandler.OnEnemyRevive();

            // Agent.
            agent.enabled = true;

            aiManager.OnEnemyRevive();
        }

        IEnumerator ReEnableAIState()
        {
            yield return new WaitForEndOfFrame();
            OnEnemyReviveAIState();
        }

        void OnEnemyReviveAIState()
        {
            _belongedAIGroup.AddToUpdatables(this);

            gameObject.layer = _layerManager.enemyLayer;
            enabled = true;
        }
        #endregion

        #region On Boss Death.
        public void OnBossDeath()
        {
            ResetRefsBeforeDeath();
            OnBossDeath_ResetAIState();
            OnBossDeath_SetManagablesStatus();
        }

        void OnBossDeath_ResetAIState()
        {
            // Disable State.
            enabled = false;
        }

        void OnBossDeath_SetManagablesStatus()
        {
            _aiSessionManager.OnBossDeath_SetManagablesStatus();
        }
        #endregion

        #region Ragdoll.
        private void InitRagdoll()
        {
            // Get all RB Components form different parts of the body and store them in an array.
            Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();
            GameObject rbObject;
            Collider col;

            for (int i = 0; i < rigs.Length; i++)
            {
                // if this is the current rb that is using on the top layer(parent gameObject), continue to the next rb
                if (rigs[i] == e_rb)
                    continue;

                rbObject = rigs[i].gameObject;
                if (_layerManager.IsInLayer(rbObject.layer, _layerManager._enemyRigidbodyExcludeMask))
                    continue;

                rbObject.layer = _layerManager.enemyRagdollLayer;

                // also add the rb to a list and set isKinematic to true so that there won't be any forces apply to rb yet.
                ragdollRigids.Add(rigs[i]);

                rigs[i].useGravity = true;
                rigs[i].isKinematic = true;

                // next we need to collect the collider component in each one of them
                col = rbObject.GetComponent<Collider>();
                // and disable them
                col.enabled = false;
                // lastly add them to a list
                ragdollColliders.Add(col);
                m_ragdollCount++;
            }
        }

        void EnableRagDoll()
        {
            for (int i = 0; i < m_ragdollCount; i++)
            {
                ragdollRigids[i].isKinematic = false;
                ragdollColliders[i].enabled = true;
            }
        }

        void DisableRagDoll()
        {
            for (int i = 0; i < m_ragdollCount; i++)
            {
                ragdollRigids[i].isKinematic = true;
                ragdollColliders[i].enabled = false;
            }
        }
        #endregion

        #region Is Lockon State.
        public void SetIsLockonStateStatus(bool _isLockonState)
        {
            if (_isLockonState)
            {
                this._isLockonState = true;
                aiDisplayManager.OnLockon();
                aiManager.SetCanPlayIndicatorToTrue();
            }
            else
            {
                this._isLockonState = false;
                aiDisplayManager.OffLockon();
                aiManager.SetCanPlayIndicatorToFalse();
            }
        }
        #endregion

        #region Is Grounded.
        public void SetIsGroundStatusToFalseWithAnim()
        {
            if (isGrounded)
            {
                isGrounded = false;
                anim.SetBool(e_IsGrounded_hash, false);
                FacePlayerWhenFalling();
                OnFallingReset();
                _aiGroupManagable.isForbiddenToFoundPlayer = true;
            }
        }

        public void SetIsGroundStatusToFalse()
        {
            isGrounded = false;
            OnFallingReset();
            _aiGroupManagable.isForbiddenToFoundPlayer = true;
        }

        public void SetIsGroundStatusToTrue()
        {
            if (!isGrounded)
            {
                isGrounded = true;
                anim.SetBool(e_IsGrounded_hash, true);
                OnLandingReset();
                _aiGroupManagable.isForbiddenToFoundPlayer = false;
            }
        }

        void FacePlayerWhenFalling()
        {
            mTransform.rotation = Quaternion.LookRotation(aiManager.dirToTarget);
        }

        void OnFallingReset()
        {
            agent.enabled = false;
            aiManager.CancelAllRootMotions();
        }

        void OnLandingReset()
        {
            ReEnableAgent();
        }
        #endregion

        #region AI Patrol Point

        #region Init.
        void GetTargetPatrolRoute()
        {
            currentPatrolRoute = _aiGroupManagable.GetAIPatrolRouteById(patrolRouteId);
            currentPatrolPoint = currentPatrolRoute.patrolPoints[0];
            _currentPointId = 0;
        }

        void CheckIsAIHasPatrolRoute()
        {
            currentPatrolRoute.SetupPatrolRoute();

            int _patrolPointsLength = currentPatrolRoute.patrolPoints.Length;
            _hasPatrolRoute = _patrolPointsLength > 1 ? true : false;
            reverseRouteAtEndPoint = _patrolPointsLength > 2 ? true : false;
        }

        void ChangeAgentStats_Patrol()
        {
            agent.stoppingDistance = patrolStopDistance;
        }

        void WrapAgentToInitPoint()
        {
            _currentPatrolWaitTimer = 1;
            _isStopPatrolTurning = true;
            ReEnableAgent();
        }
        #endregion

        #region Off Faced Player.
        void OffFacedPlayerResetPatrolStatus()
        {
            _isStopPatrolTurning = false;
            _currentPatrolWaitTimer = returnToPatrolWaitRate;

            ClearAgentPath();
            ChangeAgentStats_Patrol();
            ReEnableAgent();
            
            if (_hasPatrolRoute)
                SetCurrentPatrolPointFromNearest();

            agent.SetDestination(currentPatrolPoint._localPosition);
        }

        void SetCurrentPatrolPointFromNearest()
        {
            currentPatrolPoint = currentPatrolRoute.GetClosetPatrolPoint(this);
        }
        #endregion

        #endregion

        #region AI_Group.

        #region Monitor Remnants.
        public void RemoveRemnantAfterDistance()
        {
            if (_hasArrivedToPoint)
            {
                ElligiableToRemoveRemnant();
            }
            else if (aiManager.distanceToTarget > 23f)
            {
                if (CameraHandler.singleton.GetDotProductToEnemy(this) < 0.7f)
                {
                    ElligiableToRemoveRemnant();
                }
            }
            else
            {
                Tick();
            }
        }
        
        void ElligiableToRemoveRemnant()
        {
            if (!_belongedAIGroup._isActiveGroup)
            {
                if (_aiGroupManagable.GetIsGroupAdjcentToActiveGroup(_belongedAIGroup._groupId))
                {
                    DisableRemnantAnim();
                }
                else
                {
                    DeactivateRemnantCompletely();
                }
            }
            else
            {
                KeepRemantAsCurrentGroup();
            }

            _aiGroupManagable.RemoveFromRemnants(this);
        }

        void DeactivateRemnantCompletely()
        {
            WrapAgentToPoint_SnapRotation();
            anim.enabled = false;
            gameObject.SetActive(false);
        }

        void DisableRemnantAnim()
        {
            WrapAgentToPoint_SmoothRotation();
            anim.enabled = false;
        }

        void KeepRemantAsCurrentGroup()
        {
            WrapAgentToPoint_SmoothRotation();
        }

        void WrapAgentToPoint_SnapRotation()
        {
            Vector3 _dirToPatrolPoint = currentPatrolPoint.transform.forward;
            _dirToPatrolPoint.y = 0;

            mTransform.localRotation = Quaternion.LookRotation(_dirToPatrolPoint);
            agent.Warp(currentPatrolPoint._localPosition);
        }

        void WrapAgentToPoint_SmoothRotation()
        {
            float _angle = Vector3.SignedAngle(mTransform.forward, currentPatrolPoint.transform.forward, aiManager.vector3Up);
            LeanTween.rotateAroundLocal(gameObject, aiManager.vector3Up, _angle, 0.65f).setEaseOutQuart();

            if (_angle > 0)
            {
                if (_angle > 20f)
                    aiManager.PlayAnimation_NoNeglect(aiManager.e_unarmed_turn_right_inplace_hash, false);
            }
            else
            {
                if (_angle < -20f)
                    aiManager.PlayAnimation_NoNeglect(aiManager.e_unarmed_turn_left_inplace_hash, false);
            }

            agent.Warp(currentPatrolPoint._localPosition);
        }
        #endregion

        #region Activate / Deactivate Enemy.
        public void ActivateEnemyObj()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateEnemyObj()
        {
            if (isAggros)
                return;

            gameObject.SetActive(false);
        }

        public void ActivateEnemyAnim()
        {
            if (!aiManager.isDead)
                anim.enabled = true;
        }
        #endregion

        #region On Active Group.
        public void OnActiveGroupRefreshEnemy()
        {
            ReEnableAgent();
        }
        #endregion

        #region Add / Remove Aggro
        void RemoveFromAggros()
        {
            if (!aiManager.isDead)
                _belongedAIGroup.AddToUpdatables(this);

            isAggros = false;
            
            _aiGroupManagable.DecrementAggrosCount(this);
        }

        void AddToAggros()
        {
            _belongedAIGroup.RemoveFromUpdatables(this);
            isAggros = true;
            
            _aiGroupManagable.IncrementAggrosCount(this);
        }
        #endregion
        
        #endregion

        #region Checkpoint Event.
        void CheckpointRefreshAction()
        {
            /// If this enemy is dead
            if (aiManager.isDead)
            {
                OnEnemyRevive();
            }

            /// If this enemy was aggro before and wasn't dead
            if (isAggros)
            {
                aiManager.OnCheckpointRefresh_AggroEnemy();
                OnCheckpointRefresh_AggroEnemy();
                RemoveFromAggros();
            }

            aiManager.OnCheckpointRefresh_General();
            aiDisplayManager.OnCheckpointRefresh();
            iKHandler.OnCheckpointRefresh_General();
            OnCheckpointRefresh_AIPatrolStatus();
            OnCheckpointRefresh_AILocalTransform();
            UnSubscribeCheckpointRefreshEvent();
        }

        void OnCheckpointRefresh_AggroEnemy()
        {
            _hasFoundPlayer = false;
        }
        
        void OnCheckpointRefresh_AIPatrolStatus()
        {
            currentState = GameManager.singleton.enemyPatrolState;

            _currentPointId = 0;
            _currentPatrolWaitTimer = 0;
            _isReversingRoute = false;
            currentPatrolPoint = currentPatrolRoute.patrolPoints[0];
        }

        void OnCheckpointRefresh_AILocalTransform()
        {
            agent.Warp(currentPatrolPoint._localPosition);
            mTransform.localEulerAngles = currentPatrolPoint.transform.localEulerAngles;
        }
        
        void SubscribeCheckpointRefreshEvent()
        {
            if (!_hasSubToCheckpointRefreshEvent)
            {
                aiManager.playerStates.CheckpointRefreshEvent += CheckpointRefreshAction;
                _hasSubToCheckpointRefreshEvent = true;
            }
        }

        void UnSubscribeCheckpointRefreshEvent()
        {
            aiManager.playerStates.CheckpointRefreshEvent -= CheckpointRefreshAction;
            _hasSubToCheckpointRefreshEvent = false;
        }
        #endregion

        #region AI Boss Init / Setup / Checkpoint.

        #region Boss Init.
        public void BossInSessionInit(AISessionManager _aiSessionManager)
        {
            this._aiSessionManager = _aiSessionManager;
            _aiGroupManagable = _aiSessionManager._aiGroupManagable;

            InitBossAIStateManager();
            InitLayerManager();
            InitBossRigidBody();
            InitAnimMoveRmType();
            InitHitBoxCollider();
            InitAnimator();
            InitAIStateAnimHash();
            InitBossAgent();
            InitBossRootCollider();
        }

        void InitBossAIStateManager()
        {
            currentState = GameManager.singleton.bossIntroSequenceState;
            aiManager = GetComponent<AIManager>();
            mTransform = transform;
        }

        void InitBossRigidBody()
        {
            e_rb = GetComponent<Rigidbody>();
            e_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            isGrounded = true;

            e_rb.isKinematic = true;
        }

        void InitBossAgent()
        {
            InitAgent();
            agent.enabled = false;
        }

        void InitBossRootCollider()
        {
            InitRootCollider();
            e_root_Collider.isTrigger = true;
        }
        #endregion

        #region Boss Setup 1st time.
        public void _1st_SetupBossAIManager()
        {
            SetupBossAIManager();
            SetupIKHandler();
            SetupAIDisplayManager();
            SetupAIMods();
            ///* NO: SetupStringBuilder();
            GetLateTickActionsLength();

            void SetupBossAIManager()
            {
                SetupAIManager();
                aiManager.RefreshLockonStatus();
            }
        }
        #endregion

        #region Boss Setup 2nd time.
        public void _2nd_SetupBossInSession()
        {
            _2nd_SetupBossAnimatorHook();
            _2nd_SetupBossRigidBody();
            _2nd_SetupBossCollider();
            _2nd_SetupBossTransform();
            StartCoroutine(_2nd_SetupBossAgent());
            _2nd_SetupBossAnimatorState();
            _2nd_SetupBossCurrentState();
            _2nd_SetupBossAIModsGoesAggro();
        }
        
        void _2nd_SetupBossAnimatorHook()
        {
            aiManager.SetupBossAnimatorHook();
        }

        void _2nd_SetupBossRigidBody()
        {
            e_rb.isKinematic = false;
        }

        void _2nd_SetupBossCollider()
        {
            e_root_Collider.isTrigger = false;
        }

        void _2nd_SetupBossTransform()
        {
            Transform _animGameObjectTransform = anim.transform;

            Vector3 _targetPos = _animGameObjectTransform.position;
            _targetPos.y = 0.156f;

            mTransform.position = _targetPos;
            mTransform.eulerAngles = _animGameObjectTransform.eulerAngles;

            _animGameObjectTransform.localPosition = vector3Zero;
            _animGameObjectTransform.localEulerAngles = vector3Zero;
        }
        
        IEnumerator _2nd_SetupBossAgent()
        {
            yield return new WaitForSeconds(0.15f);
            ReEnableAgent();
            _aiSessionManager.SwitchCurrentManagable_To_AIBossManagable();
        }

        void _2nd_SetupBossAnimatorState()
        {
            anim.Play(aiManager.hashManager.e_egil_1P_armed_locomotion_hash);
        }

        void _2nd_SetupBossCurrentState()
        {
            currentState = GameManager.singleton.enemyBossAggroState;
        }

        void _2nd_SetupBossAIModsGoesAggro()
        {
            aiManager.AIModsOnEnterAggroFacedPlayer();
        }
        #endregion

        #region Boss Checkpoint Refresh.
        public void Boss_CheckpointRefresh()
        {
            Boss_CheckpointRefresh_Colliders_RB();
            Boss_CheckpointRefresh_IK();
        }

        void Boss_CheckpointRefresh_Colliders_RB()
        {
            e_rb.isKinematic = true;
            e_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            e_root_Collider.isTrigger = true;
        }

        void Boss_CheckpointRefresh_IK()
        {
            iKHandler.OnCheckpointRefresh_Boss();
        }
        #endregion

        #endregion
        
        #region Savable Enemy States.
        public SavableEnemyState SaveEnemyStateToSave()
        {
            SavableEnemyState _savableEnemyState = new SavableEnemyState();
            _savableEnemyState.isDeadFlag = aiManager.isDead;
            _savableEnemyState.savableId = savableId;
            return _savableEnemyState;
        }

        public void LoadEnemyStateFromSave(SavableEnemyState _savableEnemyState)
        {
            if (_savableEnemyState.isDeadFlag)
            {
                SubscribeCheckpointRefreshEvent();
                aiManager.KillSavedStateEnemy();
            }
            else
            {
                _belongedAIGroup.AddToUpdatables(this);
            }
        }
        #endregion
    }

    public enum AI_AnimMoveRM_Type
    {
        Null,
        Attack,
        Roll,
        Fallback,
        KnockDown
    }
}

//[Header("Parry Logic")]
//public State waitForStunOverState;
//public Condition onParryLogic;              // Condition isUsed inside stateManager, won'currentWeight appear in editor.
//public bool isStunned;

//void StopMoveTowards()
//{
//    agent.isStopped = true;
//    Debug.Log("StopMoveTowards");
//}

//void SlowingMoveTowards()
//{
//    Debug.Log("SlowingMoveTowards");
//    agent.isStopped = true;

//    float proportionalDistance = 1f - (agent.remainingDistance / agent.stoppingDistance);
//    float speed = Mathf.Lerp(slowingSpeed, 0f, proportionalDistance);

//    agent.Move(mTransform.forward * (speed * _delta));
//    _isMovementChanged = true;
//}

//void RegularMoveTowards()
//{
//    agent.isStopped = false;
//    agent.stoppingDistance = agentStopDistance;

//    if (aiManager.isMovingTowardPlayer && aiManager.distanceToTarget <= applyMoveTowardPredictDistance)
//    {
//        Vector3 _currentMoveTowardPredictOffset = aiManager._playerStates.moveDirection * (moveTowardPredictAmount * aiManager._playerStates.moveAmount);
//        agent.SetDestination(aiManager.targetPos + _currentMoveTowardPredictOffset);
//    }
//    else
//    {
//        agent.SetDestination(aiManager.targetPos);
//    }

//    float proportionalDistance = agent.acceleration / agentAccelSpeed;
//    mTransform.position = Vector3.Lerp(mTransform.position, aTransform.position, proportionalDistance);

//    _isMovementChanged = true;
//}