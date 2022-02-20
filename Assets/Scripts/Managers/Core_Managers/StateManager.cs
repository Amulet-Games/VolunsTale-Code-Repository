using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using MTAssets.SkinnedMeshCombiner;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace SA
{
    ///* MUST BE DEACTIVATED IN PREFAB.
    public class StateManager : MonoBehaviour
    {
        #region Inventory
        [Header("Inventory")]
        public SavableInventory _savableInventory;
        #endregion

        #region Stats Attribute Handler
        [Header("Stats Handler.")]
        public StatsAttributeHandler statsHandler;
        #endregion

        #region Invincible Status
        [Header("Invincible Status")]
        public float invincibleRate = 0.75f;
        [ReadOnlyInspector] public float invincibleTimer;
        [ReadOnlyInspector] public bool isInvincible;
        [ReadOnlyInspector] public bool isCantBeDamaged;
        [ReadOnlyInspector] public bool isDead;
        #endregion

        #region Decision Variables
        [Header("Decision Variables")]
        [ReadOnlyInspector] public State currentState;
        [ReadOnlyInspector] public float _delta;
        [ReadOnlyInspector] public float _fixedDelta;
        #endregion

        #region Axis.
        [Header("Axis.")]
        [ReadOnlyInspector]
        public float horizontal;
        [ReadOnlyInspector]
        public float vertical;
        #endregion

        #region Trigger, Bumpers.
        [Header("Triggers, Bumpers")]
        [ReadOnlyInspector]
        public bool rb;
        [ReadOnlyInspector]
        public bool lb;

        [Header("Triggers, Bumpers Hold verison.")]
        [ReadOnlyInspector]
        public bool lb_hold;
        [ReadOnlyInspector]
        public bool rt_hold;
        [ReadOnlyInspector]
        public bool lt_hold;
        [ReadOnlyInspector]
        public bool p_left_trigger;
        #endregion

        #region Inputs.
        [Header("Inputs.")]
        [ReadOnlyInspector]
        public bool p_walk_input;
        [ReadOnlyInspector]
        public bool p_run_input;
        [ReadOnlyInspector]
        public bool p_run_up_input;
        [ReadOnlyInspector]
        public bool p_run_down_input;
        [ReadOnlyInspector]
        public bool p_sprinting_input;
        [ReadOnlyInspector]
        public bool p_useConsum_input;
        [ReadOnlyInspector]
        public bool p_twoHanding_input;
        [ReadOnlyInspector]
        public bool p_lockon_input;
        [ReadOnlyInspector]
        public bool p_esacpe_input;

        [Space(10)]
        [ReadOnlyInspector]
        public bool p_switchLeftWeapon_input;
        [ReadOnlyInspector]
        public bool p_switchRightWeapon_input;
        [ReadOnlyInspector]
        public bool p_switchConsum_input;

        [Space(10)]
        [ReadOnlyInspector]
        public bool p_interaction_select;
        [ReadOnlyInspector]
        public bool p_interaction_switch;

        [Space(10)]
        [ReadOnlyInspector]
        public bool p_execution_select;
        
        [Header("Get Button Amount")]
        [ReadOnlyInspector]
        public float runInputAmount;
        [ReadOnlyInspector]
        public float spriteInputAmount;
        [ReadOnlyInspector]
        public float twoHandingInputAmount;
        [ReadOnlyInspector]
        public float _currentQuitNeglectInputAmount;
        #endregion

        #region Input Thershold.
        [Header("Input Thershold.")]
        public float twoHandLhWeaponInputThershold;
        public float _currentQuitNeglectInputThershold;
        public float _QuitFighterModeInputThershold;

        [Header("Locomotion Floats.")]
        public float runThershold = 0.25f;
        public float jumpInputWaitRate = 0.25f;
        #endregion

        #region Switch Weapon Wait.
        [Header("Switch Weapon Wait.")]
        public float _2HandRhSwitch_WithSheath_WaitRate = 0.8f;
        public float _2HandRhSwitch_NoSheath_WaitRate = 0.25f;
        public float _2HandLhSwitch_WithSheath_WaitRate = 1.4f;
        public float _2HandLhSwitch_NoSheath_WaitRate = 0.7f;
        public float switchRhCurrentWaitRate = 1.15f;
        public float switchLhCurrentWaitRate = 1.15f;
        public float setupEmptyWeaponSlotWaitRate = 1f;
        public float setupTakenWeaponSlotWaitRate = 2.2f;
        public float removed_TH_WeaponWaitRate = 1f;
        public float removedWeaponWaitRate = 0.6f;
        [ReadOnlyInspector] public float _currentWeaponSwitchWaitRate;
        [ReadOnlyInspector] public float _weaponSwitchWaitTimer;
        
        [Header("Manual Set Hold Attack Speed Multi")]
        [ReadOnlyInspector] public float _manualSetHoldAttackSpeedMultiValue;
        #endregion

        #region Foot Step Effect.
        [Header("Foot Particles / Sounds Effect.")]
        public float _footEffectsReplayDistance;
        [ReadOnlyInspector] public bool _isFootEffectReplayable;
        [ReadOnlyInspector] public Vector3 _lastFootEffectPlayedPosition;
        #endregion

        #region Bools.
        [Header("Triggers / Bumpers Holding Bool")]
        [ReadOnlyInspector] public bool _isHoldingLB;
        [ReadOnlyInspector] public bool _isHoldingRT;
        [ReadOnlyInspector] public bool _isHoldingLT;

        [Header("Action Bools.")]
        [ReadOnlyInspector] public bool _isAttacking;
        [ReadOnlyInspector] public bool _isAttackCharging;
        [ReadOnlyInspector] public bool _isTwoHandFistAttacking;
        [ReadOnlyInspector] public bool _hasHoldAtkReachedMaximum;
        [ReadOnlyInspector] public bool _isLhHoldLoopEffect;
        [ReadOnlyInspector] public bool _hasChargeEnchanted;
        [ReadOnlyInspector] public bool _isParrying;

        [Header("Quit Animation Bools.")]
        [ReadOnlyInspector] public bool isComboAvailable;
        [ReadOnlyInspector] public bool canQuitNeglectState;
        [ReadOnlyInspector] public bool isNeglectingInput;
        [ReadOnlyInspector] public bool _isActionRequireUpdateInNeglectState;

        [Header("Roll, Run, Jump")]
        [ReadOnlyInspector] public bool _isWaitForStaminaRecover;
        [ReadOnlyInspector] public bool _isRolling;
        [ReadOnlyInspector] public bool _isRunning;
        [ReadOnlyInspector] public bool _isSprinting;
        [ReadOnlyInspector] public bool _isSprintingChangeVelocity;
        [ReadOnlyInspector] public bool _isSpritingEndAnimPlayed;
        [ReadOnlyInspector] public bool _isJumping;
        [ReadOnlyInspector] public bool _isWaitForJumpInput;
        [ReadOnlyInspector] public bool _startJumpInputWaitCountDown;
        [ReadOnlyInspector] public bool _hasReleaseRunButton;

        [Header("Status.")]
        [ReadOnlyInspector] public bool isLockingOn;
        [ReadOnlyInspector] public bool _isTwoHanding;
        [ReadOnlyInspector] public bool _isInTwoHandFist;
        [ReadOnlyInspector] public bool _UseUnarmedLocoInConsumable;

        [Header("Main Hud.")]
        [ReadOnlyInspector] public bool _isInMainHud;
        [ReadOnlyInspector] public bool _isWaitForRhCurrentSwitch;
        [ReadOnlyInspector] public bool _isWaitForLhCurrentSwitch;
        [ReadOnlyInspector] public bool _isWaitForWeaponSwitch;

        [Header("Trail Fx.")]
        [ReadOnlyInspector] public bool _isUsedTrailFx;

        [Header("Charm Perks.")]
        [ReadOnlyInspector] public bool isIncreaseBackstabDmg;
        [ReadOnlyInspector] public bool isExtendParryWindow;
        [ReadOnlyInspector] public bool isGivenExtraEustusFlask;
        [ReadOnlyInspector] public bool isReduceDmgTaken;
        [ReadOnlyInspector] public bool isIncreaseDmgWhenCombo;

        [Header("Fighter Mode Perks.")]
        [ReadOnlyInspector] public bool isReduceNextAttackCost;
        #endregion

        #region Locomotion.
        [Header("Locomotion Status.")]
        [ReadOnlyInspector] public Transform mTransform;
        [ReadOnlyInspector] public float jumpInputWaitTimer;
        [ReadOnlyInspector] public float moveAmount;
        [ReadOnlyInspector] public Vector3 moveDirection;
        #endregion

        #region Turning Speed.
        [Header("Turn Speed Before Neglect")]
        public float normalTurnSpeedBeforeNeglect = 10;
        public float lockonTurnSpeedBeforeNeglect = 8;

        [Header("Turn Speed After Neglect")]
        public float normalTurnSpeedAfterNeglect = 5;
        public float lockonTurnSpeedAfterNeglect = 12;
        #endregion

        #region Root Motion.
        [Header("Root Motion.")]
        /// Turning.
        [ReadOnlyInspector] public bool applyTurningWithMoveDir;
        [ReadOnlyInspector] public bool applyTurningWithInverseMoveDir;
        [ReadOnlyInspector] public bool applyTurningWithLockonDir;

        /// Move.
        [ReadOnlyInspector] public bool canMoveWhileNeglect;

        /// Attack.
        [ReadOnlyInspector] public bool ignoreAttackRootCalculate;
        [ReadOnlyInspector] public bool applyAttackRootMotion;
        [ReadOnlyInspector] public float attackRootMotion;
        public float attackMaxVelocityDis = 3;

        /// Roll / Evade.
        [ReadOnlyInspector] public Vector3 rollRootMotion;
        [ReadOnlyInspector] public float rollSpeedCurveCounter;
        public AnimationCurve _rollCurve;

        /// Jump.
        [ReadOnlyInspector] public float _fallingVelocity;

        /// Hit Impact.
        [ReadOnlyInspector] public bool applyHitImpactMotion;
        [ReadOnlyInspector] public float hitImpactSpeedCurveCounter;
        public float hitImpactSpeed = 200;
        public float blockBrokenImpactSpeed = 250;
        public AnimationCurve _hitImpactCurve;

        /// Sprint End.
        [ReadOnlyInspector] public bool applySprintEndMotion;
        public float sprintEndMotionSpeed;
        #endregion

        #region Front step Height.
        [Header("Front step Height Config.")]
        public float frontRayOffset = 0.5f;
        public float frontRayLength = 1.2f;
        public float frontRayHeight = 0.7f;
        public float frontRayRaiseAmount = 0.4f;
        public float frontRayDropAmount = 0.4f;

        [Header("Front step Height Status.")]
        [ReadOnlyInspector, SerializeField] float frontStepHeight;
        #endregion

        #region IsGrounded.
        [Header("IsGrounded.")]
        public float sphereCastRadius = 0.3f;
        public float sphereCastDistance = 10;
        public float isGroundedCheckRate = 0.1f;
        public float jumpSkipGroundCheckRate = 1;
        public float offGroundDistance = 0.15f;
        public float startFallingDistance = 1f;
        
        [ReadOnlyInspector, SerializeField] float OnGroundHeight;
        [ReadOnlyInspector, SerializeField] float OffGroundPoint;
        [ReadOnlyInspector, SerializeField] float skipGroundCheckTimer = 0;
        [ReadOnlyInspector, SerializeField] float isGroundedCheckTimer = 0;

        [ReadOnlyInspector, SerializeField] bool isGrounded;
        [ReadOnlyInspector, SerializeField] bool skipGroundCheck;
        [ReadOnlyInspector, SerializeField] bool knockedDownSkipGroundCheck;
        [ReadOnlyInspector, SerializeField] bool isOffGroundFalling;
        [ReadOnlyInspector] public bool canQuitOffGroundEarly;
        #endregion

        #region Fall Damage.
        [Header("Fall Damage.")]
        [SerializeField] float maxFallDamageHeight = 5f;
        [SerializeField] float fallingToDeathHeight = 7f;
        [SerializeField] float fallingSafeHeight = 1f;
        [SerializeField] float maxFallDamage;
        #endregion

        #region Blocking.
        [Header("Blocking Config.")]
        [SerializeField] float validateBlockingThershold = 45;

        [ReadOnlyInspector] public bool _isBlocking;
        [ReadOnlyInspector, SerializeField] bool _isBlockingValidate;
        [ReadOnlyInspector] public bool _hasBlockingBroken;
        #endregion

        #region Lockon Target.
        [Header("Lockon Config.")]
        public LockonToEnemyModeEnum _preferLockonMode;
        public float validateTargetRate = 1.5f;
        public float lockOnValidDistance = 17;
        [ReadOnlyInspector] public float validateTargetTimer = 0;

        [Space(10)]
        public float _cancelLockonWaitRate;
        [ReadOnlyInspector] public float _cancelLockonWaitTimer;
        [ReadOnlyInspector] public bool _isStartCancelLockonWait;

        [Header("Lockon AI States.")]
        [ReadOnlyInspector] public AIStateManager _lockonState;
        [ReadOnlyInspector] public Vector3 _dirToLockonStates;
        [ReadOnlyInspector] public float _angleToLockonStates;

        [Header("Locking on Bone Transform.")]
        [ReadOnlyInspector] public Transform _lockonBodyBoneTransform;
        [NonSerialized] public Collider[] lockableHitColliders = new Collider[3];
        #endregion

        #region Hit Source Refs.
        [Header("Hit Source Refs.")]
        [ReadOnlyInspector] public AIManager _hitSourceAI;
        [ReadOnlyInspector] public AI_AreaDamageParticle_Base _hitSourceAOEParticle;
        [ReadOnlyInspector] public AI_AttackRefs _hitSourceAttackRefs;
        [ReadOnlyInspector] public Collider _hitSourceCollider;

        [Header("Execution Source Refs.")]
        [ReadOnlyInspector] public AI_BaseExecution_Profile _currentExecutionProfile;
        [ReadOnlyInspector, SerializeField] float _executionReceivedYPos;

        [Header("On Hit Result.")]
        [ReadOnlyInspector] public Vector3 _hitPoint;
        [ReadOnlyInspector] public float _hitSourceAngle;
        [ReadOnlyInspector] public float _hitSourceAngle_noDirCheck;
        [ReadOnlyInspector] public float _previousGetHitDamage;
        [ReadOnlyInspector] public bool isOnHit;

        [Header("Damage Taken Result.")]
        [ReadOnlyInspector] public DamageTakenDirectionTypeEnum _damageTakenDirectionType;
        [ReadOnlyInspector] public DamageTakenPhysicalTypeEnum _damageTakenPhysicalType;
        #endregion

        #region Executing Target.
        [ReadOnlyInspector] public AIManager _currentExecutingTarget;
        [ReadOnlyInspector] public bool _isExecutionCardShown;
        #endregion

        #region Late Tick Actions.
        [Header("Late Tick Actions.")]
        [SerializeField] StateActions[] lateTicks;
        #endregion

        #region INeglect.
        [Header("INeglect Configs.")]
        [SerializeField] RollAction rollAction = null;
        [SerializeField] JumpAction jumpAction = null;

        [Header("INeglect Status.")]
        [ReadOnlyInspector] public INeglectInputAction _currentNeglectInputAction;

        [Header("Attack Action Status.")]
        [ReadOnlyInspector] public WeaponAttackAction _currentAttackAction;
        #endregion

        #region LocomotionIK State.
        [Header("LocomotionIK Status.")]
        [ReadOnlyInspector] public BaseLocoIKState _currentLocoIKState;
        [ReadOnlyInspector] public bool _isCurrentNeglectUseLookAtIK;
        [ReadOnlyInspector] public bool _isPausingLocoIKStateTick;
        #endregion

        #region LookAt IK.
        [Header("Surrounding LookAt IK Thersholds.")]
        public float _surroundLookAtIKRange = 6.5f;
        public float _surroundLookAtAngleThershold_1H = 90f;
        public float _surroundLookAtAngleThershold_2H = 50f;
        public float _surd_IK_UpperBody_MaxAngleDis = 36f;
        public float _surd_IK_HeadOnly_Angle = 30;
        public float _surd_IK_UpperBody_MinAngle_1H = 65;
        [ReadOnlyInspector] public float _surd_IK_UpperBody_MaxAngle_1H;
        public float _surd_IK_UpperBody_MinAngle_2H = 20;
        public float _surd_IK_UpperBody_MaxAngle_2H = 45;
        public float _surd_IK_Weight_MinRange = 12.25f;
        [ReadOnlyInspector] public float _surroundIKUpperBodyAngle;

        [Header("Surrounding Found Groups.")]
        [ReadOnlyInspector, SerializeField] Collider[] _surroundFoundCols = new Collider[3];

        [Header("Surrounding LookAt Target.")]
        [ReadOnlyInspector] public int _cur_surround_hitAmounts;
        [ReadOnlyInspector] public Transform _cur_surround_lookAtTarget;
        [ReadOnlyInspector] public Vector3 _dirToSurroundTarget;
        [ReadOnlyInspector] public float _sqrtDisToSurroundTarget;
        [ReadOnlyInspector] public float _angleToSurroundTarget;

        [Header("Lockon LookAt Target.")]
        public float _lockonLookAtMaxAngleThershold;
        #endregion
        
        #region Public Damage Collider.
        public DamageCollider r_fist_dmgCollider;
        public DamageCollider l_fist_dmgCollider;
        public DamageCollider r_lower_leg_dmgCollider;
        public DamageCollider l_lower_leg_dmgCollider;
        #endregion

        #region Getup Counter.
        [Header("Getup Counter.")]
        public float _getupWaitRate;
        public float _knockback_death_WaitRate = 0.5f;
        public float _knockback_Velocity_backward = 5f;
        public float _knockback_Velocity_upward = 5f;
        [ReadOnlyInspector] public float _getupTimer;
        [ReadOnlyInspector] public bool _startGetupCounting;
        [ReadOnlyInspector] public bool _isExecutionKnockedBack;
        #endregion

        #region Skinned Mesh Status.
        [Header("Skin Meshes Status.")]
        [ReadOnlyInspector] public bool isMeshCombined;
        [ReadOnlyInspector] public bool isMeshCombinedNeeded;
        [ReadOnlyInspector] public float reCombineWaitTimer;
        public float reCombineWaitRate;
        #endregion

        #region Dissolve Material.
        [Header("Player Dissolve Session.")]
        public float _dissolveFullOpaqueValue = 1.4f;
        public float _dissolveFullTransparentValue = -0.75f;
        public float _deathDissolveSpeed = 2.75f;
        public float _deathDissolveFullTransparentValue = -1;
        public float _reviveDissolveSpeed = 2.75f;
        public float _armorPreviewDissolveSpeed = 1f;
        public Material _playerBodyDissolveMat;
        public Material _playerBackCapeDissolveMat;
        [ColorUsage(true, true)] public Color _defaultVolunColor;
        [ColorUsage(true, true)] public Color _armorAbsorbHDRColor;
        [ColorUsage(true, true)] public Color _powerupAbsorbHDRColor;
        [HideInInspector] public int _dissolveCutoffPropertyId;
        [HideInInspector] public int _dissolveEmmisionColorPropertyId;
        #endregion

        #region Blur Screen Effect.
        [Header("Blur Screen Effect.")]
        public FloatParameter _WF_DOF_StartPara;
        public FloatParameter _WF_DOF_MaxRadiusPara;
        public float _originalDoFStartValue = 45;
        public float _originalDoFMaxRadius = 0.5f;
        public float _targetDoFStartValue = 0;
        public float _targetDoFMaxRadius = 0.85f;
        public float _DoFStartParaChangeSpeed = 0.5f;
        public float _OnDeath_DoFMaxRadiusChangeSpeed = 7f;
        public float _OnRevive_DoFMaxRadiusChangeSpeed = 0.55f;
        #endregion

        #region Profile.
        [Header("Profile.")]
        public StarterClassProfile _currentProfile;
        #endregion

        #region Refs.
        [Header("Refs.")]
        [ReadOnlyInspector] public InputManager _inp;
        [ReadOnlyInspector] public CameraHandler _camHandler;
        [ReadOnlyInspector] public Animator anim;
        [ReadOnlyInspector] public Rigidbody p_rb;
        [ReadOnlyInspector] public Collider p_collider;
        [ReadOnlyInspector] public AnimatorHook a_hook;
        [ReadOnlyInspector] public FootStepHook f_hook;
        [ReadOnlyInspector] public PlayerIKHandler _playerIKHandler;
        [ReadOnlyInspector] public LayerManager _layerManager;
        [ReadOnlyInspector] public NavMeshAgent _agent;
        [ReadOnlyInspector] public MainHudManager _mainHudManager;
        [ReadOnlyInspector] public GameManager _gameManager;
        [ReadOnlyInspector] public LoadingScreenHandler _loadingScreenHandler;
        [ReadOnlyInspector] public AIGroupManagable _aiGroupManagable;
        #endregion

        #region Interactables.
        [Header("Interactables.")]
        public int maxInteractablesAmount = 2;
        [ReadOnlyInspector] public bool _isPausingSearchInteractables;
        [ReadOnlyInspector] public List<PlayerInteractable> foundInteractables;
        [ReadOnlyInspector] public Collider[] interactHitColliders;
        [ReadOnlyInspector] public PlayerInteractable _currentInteractable;
        [ReadOnlyInspector] public int _foundInteractablesAmount;
        [ReadOnlyInspector] public int _int_Index;
        #endregion

        #region NavMesh Agent.
        public float _agentMoveAccel;
        public float _agentMoveSpeed;
        public float _agentManeuverTurningSpeed;
        public float _agentManeuverLocoAnimVal = 0.6f;
        public float _executeAgentInteractionDisSqr;
        [ReadOnlyInspector] public Transform _currentAgentDestination;
        [ReadOnlyInspector] public Vector3 _dirToAgentDestination;
        [ReadOnlyInspector] public float _disToAgentDestinationSqr;
        [ReadOnlyInspector] public bool _isControlByAgent;
        [ReadOnlyInspector] public bool _isAllowAgentInteraction;
        [ReadOnlyInspector] public bool _isPauseControlByAgent;
        #endregion

        #region Comment Handler.
        [Header("Comment Handler.")]
        public CharacterCommentHandler _commentHandler;
        #endregion

        #region Spawn Point.
        [Header("Spawn Points.")]
        [ReadOnlyInspector] public PlayerSpawnPoint _currentSpawnPoint;
        [ReadOnlyInspector] public RestInteractable _currentWalkTowardRestInteractable;
        [ReadOnlyInspector] public Transform _currentRestPoint_linkedIgniteInterTrans;
        #endregion

        #region Hold Attack WA Profiles.
        [Header("Hold Attack WA Profiles.")]
        [ReadOnlyInspector] public WeaponItem _currentHoldAttackWeapon;
        [ReadOnlyInspector] public BaseWeaponActionEffect _holdATK_Loop_effect;
        #endregion
        
        #region Serialization.
        [Header("Serialization.")]
        [ReadOnlyInspector] public SavableManager _savableManager;
        [ReadOnlyInspector] public MainSaveFile _current_main_saveFile;
        [ReadOnlyInspector] public SubSaveFile _current_sub_saveFile;
        #endregion

        #region Charge Attack.
        [Header("Charge Attack Action.")]
        [ReadOnlyInspector] public float _inputChargeAmount;
        [ReadOnlyInspector] public bool _isReadyForChargeRelease;
        #endregion

        #region BloodFx Data.
        [Header("Stickable Trans")]
        public float upperBodyApproxHeight = 0.825f;
        public Transform[] u_F_stickableTrans;
        public Transform[] u_B_stickableTrans;
        public Transform[] u_R_stickableTrans;
        public Transform[] u_L_stickableTrans;
        public Transform[] d_F_stickableTrans;
        public Transform[] d_B_stickableTrans;
        public Transform d_L_FeetStickableTrans;

        [Header("Random Bfx IDs")]
        public int[] _random_Bfx_IDs;

        [Header("BFX Status.")]
        [ReadOnlyInspector] public Transform closestStickableTrans = null;
        [ReadOnlyInspector] public bool isStickyUpdatersEmpty;
        [ReadOnlyInspector] public bool isBfxHandlersEmpty;
        [ReadOnlyInspector] public bool isUseBfxHandlerYPosBuffer;

        List<BFX_StickyUpdater> inProcess_stickyUpdaters = new List<BFX_StickyUpdater>();
        List<BFX_Handler> inProcess_bfxhandlers = new List<BFX_Handler>();

        public StateActions HandleBloodFx;
        #endregion

        #region AI BloodFx Data.
        [Header("L. AI BloodFx IDs.")]
        public int[] _strike_L_AI_Bfx_Id;
        public int[] _slash_L_AI_Bfx_Id;
        public int[] _thrust_L_AI_Bfx_Id;

        [Header("M. AI BloodFx IDs.")]
        public int[] _strike_M_AI_Bfx_Id;
        public int[] _slash_M_AI_Bfx_Id;
        public int[] _thrust_M_AI_Bfx_Id;
        #endregion

        #region Avatar Handler.
        [Header("Avatar Handler.")]
        [ReadOnlyInspector] public AvatarHandler _avatarHandler;
        #endregion

        #region HideInInspector.

        #region Vector3s.
        [HideInInspector] public readonly Vector3 vector3Zero = new Vector3(0, 0, 0);
        [HideInInspector] public readonly Vector3 vector3One = new Vector3(1, 1, 1);
        [HideInInspector] public readonly Vector3 vector3Up = new Vector3(0, 1, 0);
        
        Vector3 moveWithPlayerRayOrigin;
        Vector3 previousMoveVelocity;
        #endregion
        
        #region Skin Mesh Renderers Refs.
        [HideInInspector] public Transform skinRendererHubTransform = null;

        [HideInInspector] public SkinnedMeshRenderer[] headPiecesRenderer = new SkinnedMeshRenderer[3];
        [HideInInspector] public SkinnedMeshRenderer[] chestPiecesRenderer = new SkinnedMeshRenderer[2];
        [HideInInspector] public SkinnedMeshRenderer[] handPiecesRenderer = new SkinnedMeshRenderer[10];
        [HideInInspector] public SkinnedMeshRenderer[] legPiecesRenderer = new SkinnedMeshRenderer[4];
        [HideInInspector] public SkinnedMeshRenderer backPiecesRenderer;
        
        [HideInInspector] public SkinnedMeshCombiner skinnedMeshCombiner;
        public delegate void GetModularSkinRenderersDelegate();
        public event GetModularSkinRenderersDelegate GetModularSkinRenderersEvent;
        #endregion

        #region Body Bone Transforms.
        [HideInInspector] public Transform rightHandTransform = null;
        [HideInInspector] public Transform rightIndexIntermediateTransform = null;
        [HideInInspector] public Transform leftHandTransform = null;
        [HideInInspector] public Transform leftIndexIntermediateTransform = null;
        [HideInInspector] public Transform spineTransform = null;
        [HideInInspector] public Transform hipTransform = null;
        [HideInInspector] public Transform headTransform = null;
        #endregion

        #region Anim Hash.

        #region Public.
        [HideInInspector] public int vertical_hash;
        [HideInInspector] public int horizontal_hash;

        [HideInInspector] public int vertical_whole_hash;
        [HideInInspector] public int horizontal_whole_hash;
        #endregion

        #region Transition Parameter.
        [HideInInspector] public int p_IsNeglecting_hash;
        [HideInInspector] public int p_IsRunning_hash;
        [HideInInspector] public int p_IsSprinting_hash;
        [HideInInspector] public int p_IsGrounded_hash;
        [HideInInspector] public int p_IsBlocking_hash;
        [HideInInspector] public int p_IsAnimationJobFinished_hash;
        [HideInInspector] public int p_IsHandleIKJobFinished_hash;
        [HideInInspector] public int p_IsTwoHanding_hash;
        [HideInInspector] public int p_IsGetupReady_hash;
        [HideInInspector] public int p_IsBonfireEnd_hash;
        [HideInInspector] public int p_IsLevelupBegin_hash;
        [HideInInspector] public int p_IsTriggerFullyHeld_hash;
        #endregion

        #region Anim State Speed Multi.
        [HideInInspector] public int p_HoldAttackSpeedMulti_hash;
        #endregion

        #region Chest Right Hand Override Layer.
        /// Fist Two Hand Sheath / UnSheath
        [HideInInspector] public int p_fist_th_sheath_hash;
        [HideInInspector] public int p_fist_th_unSheath_hash;

        /// Item.
        [HideInInspector] public int p_item_throw_horizontal_hash;
        [HideInInspector] public int p_item_throw_high_hash;
        [HideInInspector] public int p_item_throw_mid_hash;
        [HideInInspector] public int p_item_throw_low_hash;
        [HideInInspector] public int p_item_vessel_empty_hash;

        /// Interactions.
        [HideInInspector] public int p_int_bonfire_ignite_hash;
        #endregion
        
        #region Upper Body Override Layer.
        [HideInInspector] public int p_switchToRh_hash;
        [HideInInspector] public int p_switchToLh_hash;
        [HideInInspector] public int p_passToRh_hash;
        [HideInInspector] public int p_passToLh_hash;
        #endregion

        #region Full Body Left Hand Override Layer.
        /// Empty.
        [HideInInspector] public int p_empty_fullbody_lh_overide_hash;

        /// Interaction.
        [HideInInspector] public int p_int_pickup_up_hash;
        [HideInInspector] public int p_int_pickup_mid_hash;
        [HideInInspector] public int p_int_pickup_down_hash;
        #endregion

        #region Full Body Right Hand Override Layer.
        /// Empty.
        [HideInInspector] public int p_empty_fullbody_rh_overide_hash;

        /// Interaction.
        [HideInInspector] public int p_int_takeChest_hash;
        #endregion

        #region Base Override Layer.

        #region Empty Full Body Override.
        [HideInInspector] public int p_empty_fullBody_override_hash;
        #endregion
        
        #region Roll.
        [HideInInspector] public int p_backstep_hash;
        [HideInInspector] public int p_fist_evade_tree_hash;
        #endregion

        #region Jump.
        [HideInInspector] public int p_unarmed_jump_start_hash;
        [HideInInspector] public int p_unarmed_fall_start_hash;
        [HideInInspector] public int p_unarmed_land_hash;
        [HideInInspector] public int p_armed_jump_start_hash;
        [HideInInspector] public int p_armed_fall_start_hash;
        [HideInInspector] public int p_armed_land_hash;
        #endregion

        #region Parry Received.
        [HideInInspector] public int p_parry_received_hash;
        #endregion

        #region Parry Present.
        [HideInInspector] public int p_shield_heavy2_parry_fast_hash;
        [HideInInspector] public int p_shield_heavy2_parry_slow_hash;
        #endregion

        #region Sprinting.
        [HideInInspector] public int p_sprint_end_hash;
        #endregion
        
        #region Interaction.
        [HideInInspector] public int p_int_bonfire_start_hash;

        [HideInInspector] public int p_int_cantOpen_hash;
        [HideInInspector] public int p_int_openDoor_hash;
        [HideInInspector] public int p_int_openChest_hash;

        [HideInInspector] public int p_int_levelup_end_hash;
        #endregion

        #endregion

        #endregion

        #region String Builder.
        public StringBuilder _strBuilder;
        #endregion

        #region List Lengths.
        int lateTickActionsLength;
        int inProcessStickyUpdatersAmount;
        int inProcessBfxHandlersAmount;
        int randomBfxAmount;
        #endregion

        #region Setup Gear Action.
        [HideInInspector] public StateActions _setupPlayerGearActions;
        #endregion

        public event Action CheckpointRefreshEvent;
        #endregion

        //[Header("Test.")]
        //public bool _test;

        public void Init()
        {
            mTransform = transform;

            InitGetStrBuilder();

            InitLayerManager();
            
            InitCollider();

            InitRigidbody();

            InitNavAgent();

            InitSavableManager();

            InitAvatarHandler();

            InitAnimatorHook();

            InitSavableInventory();
        }

        public void Setup()
        {
            CreateOrLoadSaveFiles();

            SetupStatsHandler();

            Setup_GetGameManager_SetCurrentState();

            SetupMainHudManagerReference();

            SetupAnimHash();
            
            SetupAnimParas();

            SetupParentTransforms();
            
            SetupSkinnedMeshCombiner();

            SetupModularSkinnedMeshRenderers();

            SetupWeaponActionEffectList();

            SetupPlayDissolveMaterial();

            SetupPublicDamageCollider();

            SetupIsGrounded();

            SetupInteractables();

            SetupWalkAndRunInput();

            SetupGetPostProcessBlurParas();

            SetupCharacterCommentHandler();

            SetupGetLoadingScreenHandler();

            SetupFootStepHook();

            Setup_BFX_Sticky_Handlers();

            GetLateTickActionsLength();
        }

        public void Tick()
        {
            HandleBloodFxTick();

            currentState.Tick(this);

            statsHandler.Tick();
            //BreakEditor();
        }

        public void FixedTick()
        {
            currentState.FixedTick(this);
        }

        public void LateTick()
        {
            for (int i = 0; i < lateTickActionsLength; i++)
            {
                lateTicks[i].Execute(this);
            }
        }
        
        #region Awake.
        void InitGetStrBuilder()
        {
            _strBuilder = _inp.states_strBuilder;
        }

        void InitLayerManager()
        {
            _layerManager = LayerManager.singleton;
            gameObject.layer = _layerManager.playerLayer;
        }
        
        void InitCollider()
        {
            p_collider = GetComponent<Collider>();
        }

        void InitRigidbody()
        {
            p_rb = GetComponent<Rigidbody>();
            p_rb.mass = 500;
            p_rb.drag = 4;
            p_rb.angularDrag = 999;
            p_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        void InitNavAgent()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.enabled = false;
            _agent.stoppingDistance = 0;
            _agent.updatePosition = false;
            _agent.updateRotation = false;
            _isControlByAgent = false;
        }

        void InitSavableManager()
        {
            _savableManager = SavableManager.singleton;
            _savableManager._states = this;
        }

        void InitAvatarHandler()
        {
            _avatarHandler = AvatarHandler.singleton;
            _avatarHandler._states = this;
        }

        void InitAnimatorHook()
        {
            a_hook = GetComponentInChildren<AnimatorHook>();
            a_hook.Init(this);
        }

        void InitSavableInventory()
        {
            _savableInventory._states = this;
            _savableInventory.Init();
        }
        #endregion

        #region Start.
        void CreateOrLoadSaveFiles()
        {
            if (_savableManager.isContinueGame)
            {
                _current_main_saveFile = _savableManager._prev_MainSavedFile;
                _current_sub_saveFile = _savableManager._prev_SubSaveFile;

                _setupPlayerGearActions = _savableManager.loadPlayerGearsFromSaveAction;
                CheckCompleteSaveFile();
            }
            else if (_savableManager.isLoadGame)
            {
                _savableManager.If_PrevSaveNotCurrent_SwitchIsUsedStatus();

                _current_main_saveFile = _savableManager._spec_MainSavedFile;
                _current_sub_saveFile = _savableManager._spec_SubSaveFile;

                _setupPlayerGearActions = _savableManager.loadPlayerGearsFromSaveAction;
                CheckCompleteSaveFile();
            }
            else
            {
                _savableManager.Set_PrevSave_IsUsedToFalse();

                _setupPlayerGearActions = _savableManager.createStartupGearAction;
                RequestSerialization_SaveFromNewSaveFile();
            }
        }

        void SetupStatsHandler()
        {
            statsHandler._states = this;
            statsHandler.Setup();
        }

        void Setup_GetGameManager_SetCurrentState()
        {
            _gameManager = GameManager.singleton;
            currentState = _gameManager.playerIdleState;
        }

        void SetupMainHudManagerReference()
        {
            _mainHudManager = _inp._mainHudManager;
            _mainHudManager.SetupManagerReferences(this);
            
            statsHandler._mainHudManager = _mainHudManager;
            _savableInventory._mainHudManager = _mainHudManager;
        }

        void SetupAnimHash()
        {
            HashManager hashManager = HashManager.singleton;

            #region Public.
            vertical_hash = hashManager.vertical_hash;
            horizontal_hash = hashManager.horizontal_hash;

            vertical_whole_hash = hashManager.vertical_whole_hash;
            horizontal_whole_hash = hashManager.horizontal_whole_hash;
            #endregion

            #region Transition Parameter.
            p_IsNeglecting_hash = hashManager.p_IsNeglecting_hash;
            p_IsRunning_hash = hashManager.p_IsRunning_hash;
            p_IsSprinting_hash = hashManager.p_IsSprinting_hash;
            p_IsGrounded_hash = hashManager.p_IsGrounded_hash;
            p_IsBlocking_hash = hashManager.p_IsBlocking_hash;
            p_IsTwoHanding_hash = hashManager.p_IsTwoHanding_hash;
            p_IsGetupReady_hash = hashManager.p_IsGetupReady_hash;
            p_IsBonfireEnd_hash = hashManager.p_IsBonfireEnd_hash;
            p_IsLevelupBegin_hash = hashManager.p_IsLevelupBegin_hash;
            p_IsTriggerFullyHeld_hash = hashManager.p_IsTriggerFullyHeld_hash;
            p_IsAnimationJobFinished_hash = hashManager.p_IsAnimationJobFinished_hash;
            p_IsHandleIKJobFinished_hash = hashManager.p_IsHandleIKJobFinished_hash;
            #endregion

            #region Anim State Multi.
            p_HoldAttackSpeedMulti_hash = hashManager.p_HoldAttackSpeedMulti_hash;
            #endregion

            #region Chest Right Hand Override Layer.
            p_fist_th_sheath_hash = hashManager.p_fist_th_sheath_hash;
            p_fist_th_unSheath_hash = hashManager.p_fist_th_unSheath_hash;

            p_item_throw_horizontal_hash = hashManager.p_item_throw_horizontal_hash;
            p_item_throw_high_hash = hashManager.p_item_throw_high_hash;
            p_item_throw_mid_hash = hashManager.p_item_throw_mid_hash;
            p_item_throw_low_hash = hashManager.p_item_throw_low_hash;
            p_int_bonfire_ignite_hash = hashManager.p_int_bonfire_ignite_hash;
            #endregion
            
            #region Upper Body Override Layer.
            p_switchToRh_hash = hashManager.p_switchToRh_hash;
            p_switchToLh_hash = hashManager.p_switchToLh_hash;
            p_passToRh_hash = hashManager.p_passToRh_hash;
            p_passToLh_hash = hashManager.p_passToLh_hash;

            p_item_vessel_empty_hash = hashManager.p_item_vessel_empty_hash;
            #endregion

            #region Full Body Left Hand Override Layer.
            /// Empty.
            p_empty_fullbody_lh_overide_hash = hashManager.p_empty_fullbody_lh_overide_hash;

            /// Interaction.
            p_int_pickup_up_hash = hashManager.p_int_pickup_up_hash;
            p_int_pickup_mid_hash = hashManager.p_int_pickup_mid_hash;
            p_int_pickup_down_hash = hashManager.p_int_pickup_down_hash;
            #endregion

            #region Full Body Right Hand Override Layer.
            /// Empty.
            p_empty_fullbody_rh_overide_hash = hashManager.p_empty_fullbody_rh_overide_hash;

            /// Interaction.
            p_int_takeChest_hash = hashManager.p_int_takeChest_hash;
            #endregion

            #region Full Body Override Layer.
            /// Empty.
            p_empty_fullBody_override_hash = hashManager.p_empty_fullBody_override_hash;
            
            /// Roll.
            p_backstep_hash = hashManager.p_backstep_hash;
            p_fist_evade_tree_hash = hashManager.p_fist_evade_tree_hash;

            /// Jump.
            p_unarmed_jump_start_hash = hashManager.p_unarmed_jump_start_hash;
            p_unarmed_fall_start_hash = hashManager.p_unarmed_fall_start_hash;
            p_unarmed_land_hash = hashManager.p_unarmed_land_hash;
            p_armed_jump_start_hash = hashManager.p_armed_jump_start_hash;
            p_armed_fall_start_hash = hashManager.p_armed_fall_start_hash;
            p_armed_land_hash = hashManager.p_armed_land_hash;

            /// Sprinting.
            p_sprint_end_hash = hashManager.p_sprint_end_hash;

            /// Parry Received.
            p_parry_received_hash = hashManager.p_parry_received_hash;
            
            /// Interaction.
            p_int_bonfire_start_hash = hashManager.p_int_bonfire_start_hash;

            p_int_cantOpen_hash = hashManager.p_int_cantOpen_hash;
            p_int_openDoor_hash = hashManager.p_int_openDoor_hash;
            p_int_openChest_hash = hashManager.p_int_openChest_hash;

            p_int_levelup_end_hash = hashManager.p_int_levelup_end_hash;
            #endregion
        }

        void SetupAnimParas()
        {
            anim.SetFloat(p_HoldAttackSpeedMulti_hash, 1);
            anim.SetBool(p_IsAnimationJobFinished_hash, true);
            anim.SetBool(p_IsHandleIKJobFinished_hash, true);
        }

        void SetupParentTransforms()
        {
            skinRendererHubTransform = anim.transform.GetChild(0);
            rightHandTransform = anim.GetBoneTransform(HumanBodyBones.RightHand);
            rightIndexIntermediateTransform = anim.GetBoneTransform(HumanBodyBones.RightIndexIntermediate);
            leftHandTransform = anim.GetBoneTransform(HumanBodyBones.LeftHand);
            leftIndexIntermediateTransform = anim.GetBoneTransform(HumanBodyBones.LeftIndexIntermediate);
            spineTransform = anim.GetBoneTransform(HumanBodyBones.Spine);
            hipTransform = anim.GetBoneTransform(HumanBodyBones.Hips);
            headTransform = anim.GetBoneTransform(HumanBodyBones.Head);
        }

        void SetupSkinnedMeshCombiner()
        {
            skinnedMeshCombiner = anim.GetComponent<SkinnedMeshCombiner>();

            skinnedMeshCombiner.rootBoneToUse = SkinnedMeshCombiner.RootBoneToUse.Manual;
            skinnedMeshCombiner.manualRootBoneToUse = skinRendererHubTransform;
            isMeshCombinedNeeded = true;
        }

        void SetupModularSkinnedMeshRenderers()
        {
            GetModularSkinRenderersEvent.Invoke();
        }

        void SetupWeaponActionEffectList()
        {
            _gameManager.SetupWeaponActionEffectList(this);
        }

        void SetupPlayDissolveMaterial()
        {
            _dissolveCutoffPropertyId = Shader.PropertyToID("_CutoffLength");
            _dissolveEmmisionColorPropertyId = Shader.PropertyToID("_EdgeColor");
            _playerBodyDissolveMat.SetFloat(_dissolveCutoffPropertyId, _dissolveFullOpaqueValue);
            _playerBackCapeDissolveMat.SetFloat(_dissolveCutoffPropertyId, 2);
        }

        void SetupPublicDamageCollider()
        {
            r_fist_dmgCollider.Init();
            l_fist_dmgCollider.Init();

            r_lower_leg_dmgCollider.Init();
            l_lower_leg_dmgCollider.Init();
        }

        void SetupIsGrounded()
        {
            isGrounded = true;
            anim.SetBool(p_IsGrounded_hash, true);
        }

        void SetupInteractables()
        {
            foundInteractables = new List<PlayerInteractable>();
            interactHitColliders = new Collider[maxInteractablesAmount];
            _foundInteractablesAmount = 0;
            _int_Index = 0;
        }

        void SetupWalkAndRunInput()
        {
            _hasReleaseRunButton = true;
        }

        void SetupGetPostProcessBlurParas()
        {
            VolumeProfile _WF_PPV = PostProcessManager.singleton._WF_PPV.profile;

            _WF_PPV.TryGet(out DepthOfField _depthOfField);

            _WF_DOF_StartPara = _depthOfField.gaussianStart;
            _WF_DOF_MaxRadiusPara = _depthOfField.gaussianMaxRadius;
        }

        void SetupCharacterCommentHandler()
        {
            _commentHandler.SetupByStates(this);
        }

        void SetupGetLoadingScreenHandler()
        {
            _loadingScreenHandler = LoadingScreenHandler.singleton;
        }

        void SetupFootStepHook()
        {
            f_hook = GetComponent<FootStepHook>();
            a_hook.f_hook = f_hook;
        }
        
        void Setup_BFX_Sticky_Handlers()
        {
            _gameManager.Setup_BFX_Sticky_Handlers_Dictionary();
            randomBfxAmount = _random_Bfx_IDs.Length;
        }

        void GetLateTickActionsLength()
        {
            lateTickActionsLength = lateTicks.Length;
        }
        #endregion
        
        #region Update Locomotion Stats.
        public void HandleLocomotionStats()
        {
            // MOVE AMOUNT
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            // MOVE DIRECTION
            Transform camHandlerTrans = _camHandler._mTransform;
            moveDirection = camHandlerTrans.forward * vertical;
            moveDirection += camHandlerTrans.right * horizontal;
            moveDirection = Vector3.Normalize(moveDirection);
            moveDirection.y = 0;
        }
        #endregion

        #region Weapon Switch Wait.
        /// UPDATE.
        public void MonitorWeaponSwitch()
        {
            if (_isWaitForWeaponSwitch)
            {
                _weaponSwitchWaitTimer += _delta;
                if (_weaponSwitchWaitTimer > _currentWeaponSwitchWaitRate)
                {
                    _weaponSwitchWaitTimer = 0;
                    _isWaitForWeaponSwitch = false;
                }
            }
        }
        
        /// ON SETUP WEAPON IN MENU WAIT.
        public void OnSetupEmptyWeaponWait()
        {
            _isWaitForWeaponSwitch = true;
            _currentWeaponSwitchWaitRate = setupEmptyWeaponSlotWaitRate;
        }

        public void OnSetupTakenWeaponWait()
        {
            _isWaitForWeaponSwitch = true;
            _currentWeaponSwitchWaitRate = setupTakenWeaponSlotWaitRate;
        }

        /// REMOVE WEAPON IN MENU WAIT.
        public void OnRemoveWeaponWait()
        {
            _isWaitForWeaponSwitch = true;
            _currentWeaponSwitchWaitRate = removedWeaponWaitRate;
        }

        public void OnRemove_TH_WeaponWait()
        {
            _isWaitForWeaponSwitch = true;
            _currentWeaponSwitchWaitRate = removed_TH_WeaponWaitRate;
        }

        /// SWITCH WEAPON IN QSLOT.
        public void OnQSlotSwitchLhWeaponWait()
        {
            _isWaitForWeaponSwitch = true;
            _currentWeaponSwitchWaitRate = switchLhCurrentWaitRate;
        }

        public void OnQSlotSwitchRhWeaponWait()
        {
            _isWaitForWeaponSwitch = true;
            _currentWeaponSwitchWaitRate = switchRhCurrentWaitRate;
        }

        /// TWO HANDING SWITCH WEAPON WAIT.
        public void OnTwoHandingWeaponWait(bool _isLhTwoHandingSwitch, bool _isSheathingNeeded, bool _isFistWeapon)
        {
            _isWaitForWeaponSwitch = true;

            if (_isLhTwoHandingSwitch)
            {
                if (_isSheathingNeeded)
                {
                    _currentWeaponSwitchWaitRate = _2HandLhSwitch_WithSheath_WaitRate;

                }
                else
                {
                    _currentWeaponSwitchWaitRate = _2HandLhSwitch_NoSheath_WaitRate;
                }

                
            }
            else
            {
                if (_isSheathingNeeded)
                {
                    _currentWeaponSwitchWaitRate = _2HandRhSwitch_WithSheath_WaitRate;
                }
                else
                {
                    _currentWeaponSwitchWaitRate = _2HandRhSwitch_NoSheath_WaitRate;
                }
            }

            if (_isFistWeapon)
                _currentWeaponSwitchWaitRate += 0.15f;
        }
        #endregion

        #region Update Inputs (Include Switch QSlot).
        public void UpdateTickInputs_Main()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            p_walk_input = Input.GetButton("p_walk");

            rb = Input.GetButtonDown("RB");
            lb = Input.GetButtonDown("LB");
            
            p_left_trigger = Input.GetButton("p_left_trigger");

            p_switchLeftWeapon_input = Input.GetButtonDown("p_switchLeftWeapon");
            p_switchRightWeapon_input = Input.GetButtonDown("p_switchRightWeapon");
            p_switchConsum_input = Input.GetButtonDown("p_switchConsum");
            
            p_useConsum_input = Input.GetButtonDown("p_useConsum");

            if (!_isSprinting)
                p_lockon_input = Input.GetButtonDown("p_lockon");

            p_interaction_select = Input.GetButtonDown("p_interaction_select");
            p_execution_select = Input.GetButtonDown("p_execution_select");
            p_esacpe_input = Input.GetButtonDown("escape");
            //p_interaction_switch = Input.GetButtonDown("p_interaction_switch");

            /// Button Holding Amount.
            Update_Run_ButtonHoldAmount();
            Update_TwoHanding_ButtonHoldAmount();
            Update_Sprinting_ButtonHoldAmount();
        }

        public void UpdateInputs_Menu()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            Update_Run_ButtonHoldAmount();
        }

        void Update_Run_ButtonHoldAmount()
        {
            p_run_input = Input.GetButton("p_run");
            p_run_up_input = Input.GetButtonUp("p_run");
            p_run_down_input = Input.GetButtonDown("p_run");

            if (p_run_input)
            {
                runInputAmount += _delta;
            }
            else if (runInputAmount != 0)
            {
                if (runInputAmount > runThershold)
                {
                    runInputAmount = 0;
                }
                else
                {
                    runInputAmount -= _delta;
                    runInputAmount = runInputAmount < 0 ? 0 : runInputAmount;
                }
            }

            if (p_run_up_input)
            {
                _hasReleaseRunButton = true;
            }
        }

        void Update_TwoHanding_ButtonHoldAmount()
        {
            p_twoHanding_input = Input.GetButton("p_twoHanding");
            if (p_twoHanding_input)
            {
                twoHandingInputAmount += _delta;
            }
            else if (twoHandingInputAmount != 0)
            {
                if (twoHandingInputAmount > twoHandLhWeaponInputThershold)
                {
                    twoHandingInputAmount = 0;
                }
                else
                {
                    twoHandingInputAmount -= _delta;
                    twoHandingInputAmount = twoHandingInputAmount < 0 ? 0 : twoHandingInputAmount;
                }
            }
        }

        void Update_Sprinting_ButtonHoldAmount()
        {
            p_sprinting_input = Input.GetButton("p_sprinting");
            if (_isRunning && p_sprinting_input)
            {
                spriteInputAmount += _delta;
            }
            else
            {
                spriteInputAmount = 0;
            }
        }

        public void HandleQSlotsInputs()
        {
            if (p_switchLeftWeapon_input)
            {
                _mainHudManager.ShowLeftWeaponHighLighter();
                if (!_isWaitForWeaponSwitch && !isNeglectingInput && !_isControlByAgent && !_isTwoHanding)
                {
                    OnQSlotSwitchLhWeaponWait();

                    Disable_LH_CurrentBlocking();

                    _savableInventory.SwitchLeftWeapon();
                }
            }
            else if (p_switchRightWeapon_input)
            {
                _mainHudManager.ShowRightWeaponHighLighter();
                if (!_isWaitForWeaponSwitch && !isNeglectingInput && !_isControlByAgent && !_isTwoHanding)
                {
                    OnQSlotSwitchRhWeaponWait();

                    _savableInventory.SwitchRightWeapon();
                }
            }
            else if (p_switchConsum_input)
            {
                _mainHudManager.ShowConsumableHighLighter();
                _savableInventory.SwitchConsumable();
            }

            void Disable_LH_CurrentBlocking()
            {
                if (_isBlocking)
                    SetIsBlockingStatus(false);
            }
        }
        
        void PausePlayerInput_CampStart()
        {
            _inp.PausePlayerInput();

            horizontal = 0;
            vertical = 0;

            anim.SetFloat(horizontal_hash, 0);
            anim.SetFloat(vertical_hash, 0);

            p_rb.isKinematic = true;
            p_rb.velocity = vector3Zero;
        }

        void ResetPlayerInput_OnDeath()
        {
            _inp.PausePlayerInput();

            horizontal = 0;
            vertical = 0;

            runInputAmount = 0;

            anim.SetFloat(horizontal_hash, 0);
            anim.SetFloat(vertical_hash, 0);

            rb = false;
            lb = false;
            p_left_trigger = false;

            lb_hold = false;
            rt_hold = false;
            lt_hold = false;

            p_run_input = false;
            p_run_up_input = false;
            p_sprinting_input = false;
        }

        void ResetPlayerInput_OnAvatarCapture()
        {
            _inp.PausePlayerInput();

            horizontal = 0;
            vertical = 0;

            runInputAmount = 0;

            anim.SetFloat(horizontal_hash, 0);
            anim.SetFloat(vertical_hash, 0);

            rb = false;
            lb = false;
            p_left_trigger = false;

            lb_hold = false;
            rt_hold = false;
            lt_hold = false;

            p_run_input = false;
            p_run_up_input = false;
            p_sprinting_input = false;
        }
        #endregion

        #region On Reset Bools.
        public void OnIdleStateResetBools()
        {
            //Debug.Log("OnIdleStateResetBools");

            #region Weapon Action bools
            _isAttacking = false;
            _isParrying = false;
            _hasChargeEnchanted = false;
            SetIsTriggerFullyHeldToFalse();
            #endregion

            #region Quit Neglect bools.
            isComboAvailable = false;
            canQuitNeglectState = false;
            canMoveWhileNeglect = false;
            _currentQuitNeglectInputAmount = 0;
            isNeglectingInput = false;
            #endregion

            #region Consumable Unarmed Locomotion.
            /// If Player can move while using consumable, set locomotion back to current TH / RH Weapon.
            if (_UseUnarmedLocoInConsumable)
            {
                SetUseUnarmedLocoInConsumableToFalse();
            }
            #endregion

            #region Locomotions bools.
            _isRolling = false;

            if (canQuitOffGroundEarly)
            {
                OnLand();
                canQuitOffGroundEarly = false;
            }
            #endregion

            #region RootMotion status.
            applyTurningWithMoveDir = false;
            applyTurningWithLockonDir = false;
            ignoreAttackRootCalculate = false;
            attackRootMotion = 0;
            applyAttackRootMotion = false;
            rollRootMotion = vector3Zero;
            rollSpeedCurveCounter = 0;
            applyHitImpactMotion = false;
            hitImpactSpeedCurveCounter = 0;
            applySprintEndMotion = false;
            #endregion

            #region Blocking status.
            _hasBlockingBroken = false;
            #endregion
            
            #region Stats Handler.
            statsHandler.isPausingStaminaRecover = false;
            #endregion

            #region Interactables.
            _isPausingSearchInteractables = false;
            #endregion

            #region Invincibility.
            isCantBeDamaged = false;  /// Only here for Parry Execution
            #endregion
            
            #region IK.
            if (!isDead)
                ResumeLocoIKStateTick();

            _isCurrentNeglectUseLookAtIK = false;
            #endregion

            #region Accepting Comment.
            ResetCommentInterruptStatus();
            #endregion

            anim.SetFloat(p_HoldAttackSpeedMulti_hash, 1);
        }

        public void OnExecuteNeglectActionResetBools()
        {
            #region Is Blocking.
            if (_isBlocking)
                SetIsBlockingStatus(false);
            #endregion

            #region IK.
            if (!_isCurrentNeglectUseLookAtIK)
            {
                PauseLocoIKStateTick();
            }
            #endregion
        }

        public void RegainControlFromAgentResetBools()
        {
            _isPausingSearchInteractables = false;
            _isPauseControlByAgent = false;
            p_rb.isKinematic = false;

            _inp.SetIsNeglectSelectionMenuToFalse();
        }

        public void OffAgentInteractionResetBools()
        {
            _isPausingSearchInteractables = false;
            _isPauseControlByAgent = false;

            _inp.SetIsNeglectSelectionMenuToFalse();
        }

        public void OffFighterModeResetBools()
        {
            #region Weapon Action bools
            _isAttacking = false;
            _isTwoHandFistAttacking = false;

            if (isReduceNextAttackCost)
            {
                isReduceNextAttackCost = false;
                _mainHudManager.UnRegisterFighterModeIcon();
            }
            #endregion

            #region Quit Neglect bools.
            isComboAvailable = false;
            canQuitNeglectState = false;
            isNeglectingInput = false;
            _currentQuitNeglectInputAmount = 0;
            isNeglectingInput = false;
            #endregion

            #region Locomotions bools.
            _isRolling = false;
            #endregion

            #region RootMotion status.
            applyTurningWithMoveDir = false;
            applyTurningWithLockonDir = false;
            ignoreAttackRootCalculate = false;
            attackRootMotion = 0;
            applyAttackRootMotion = false;
            rollRootMotion = vector3Zero;
            rollSpeedCurveCounter = 0;
            applyHitImpactMotion = false;
            hitImpactSpeedCurveCounter = 0;
            #endregion
            
            #region Stats Handler.
            statsHandler.isPausingStaminaRecover = false;
            #endregion

            #region IK.
            if (!isDead)
                ResumeLocoIKStateTick();

            _isCurrentNeglectUseLookAtIK = false;
            #endregion

            #region Accepting Comment.
            ResetCommentInterruptStatus();
            #endregion
        }

        public void OnMenuResetInputsStatus()
        {
            runInputAmount = 0;
            twoHandingInputAmount = 0;

            if (_isBlocking)
                SetIsBlockingStatus(false);
        }

        public void OnConsumableResetInputsStatus()
        {
            moveAmount = 0;
            moveDirection = vector3Zero;
        }
        
        public void OnDeathResetBools()
        {
            _inp.OnDeathChangeInput();

            #region Locomotion.
            moveAmount = 0;
            moveDirection = vector3Zero;
            anim.SetFloat(vertical_hash, 0);
            anim.SetFloat(vertical_hash, 0);
            #endregion

            #region Quit Neglect bools.
            isComboAvailable = false;
            canQuitNeglectState = false;
            canMoveWhileNeglect = false;
            _isActionRequireUpdateInNeglectState = false;
            #endregion

            #region Is Blocking.
            if (_isBlocking)
                SetIsBlockingStatus(false);
            #endregion

            #region Locked On.
            if (isLockingOn)
            {
                SetIsLockingOnStatusToFalse();
            }
            #endregion

            #region Rb / Collider.
            p_rb.isKinematic = true;
            p_rb.velocity = vector3Zero;
            p_rb.freezeRotation = true;
            p_collider.isTrigger = false;
            #endregion

            #region Is Invincible.
            gameObject.layer = _layerManager.defaultLayer;
            #endregion

            #region Damage Collider.
            _savableInventory.OnDeathTurnOffDamageCollider(_isTwoHanding);
            #endregion

            #region Weapon Action Loop Effect.
            Stop_HoldATK_Loop_Effect();
            #endregion

            #region Comment Handler.
            _commentHandler.PauseAcceptingComment();
            #endregion
        }
        #endregion

        #region Update Reset Bools.
        public void UpdateIdleStateResetBools()
        {
            #region Weapon Action bools.
            if (_isBlocking && !lb_hold)
            {
                SetIsBlockingStatus(false);
            }
            #endregion

            #region Locomotions bools.
            SetIsWaitForJumpInputToFalse();

            if (_isRunning)
            {
                if (moveAmount == 0 || runInputAmount <= 0.1f || _isWaitForStaminaRecover)
                {
                    SetIsRunningStatusToFalse_NoRunInput();
                }
            }

            if (_isSprinting)
            {
                if (moveAmount < 0.9f || !p_sprinting_input || _isWaitForStaminaRecover)
                {
                    if (!_isSpritingEndAnimPlayed)
                    {
                        SetIsSprintingStatusToFalse();
                    }
                }
            }
            #endregion

            #region RootMotion status.
            _fallingVelocity = 0;
            #endregion
        }
        #endregion

        #region Sprinting.
        public void HandleSprinting()
        {
            if (!_isSprinting)
            {
                if (spriteInputAmount > 0.4f)
                {
                    SetIsRunningStatusToFalse_OnSprint();
                    SetIsSprintingStatusToTrue();
                }
            }
            else
            {
                statsHandler.DecrementPlayerStaminaWhenSprinting();
            }
        }
        #endregion

        #region Two Handing.
        public void HandleTwoHanding()
        {
            if (_isTwoHanding)
            {
                if (twoHandingInputAmount > 0 && !p_twoHanding_input)
                {
                    twoHandingInputAmount = 0;
                    _isTwoHanding = false;

                    if (_isBlocking)
                        SetIsBlockingStatus(false);

                    _savableInventory.SetIsTwoHandingStatusToFalseByType();
                    _currentLocoIKState.OffTwoHanding(this);
                }
            }
            else
            {
                if (twoHandingInputAmount > 0 && !p_twoHanding_input)
                {
                    twoHandingInputAmount = 0;
                    SetIsTwoHandingStatusToTrue(false);
                }
                else if (twoHandingInputAmount > twoHandLhWeaponInputThershold)
                {
                    SetIsTwoHandingStatusToTrue(true);
                }
            }
        }

        public void SetIsTwoHandingStatusToTrue(bool _isLeft)
        {
            /// IKState Tick has to be paused in here.
            PauseLocoIKStateTick();

            if (_isBlocking)
                SetIsBlockingStatus(false);

            _isTwoHanding = true;
            if (_isLeft)
            {
                _savableInventory.TwoHandingLhWeapon();
            }
            else
            {
                _savableInventory.TwoHandingRhWeapon();
            }
            
            _currentLocoIKState.OnTwoHanding(this);
        }

        public void OnTwoHandingRhWeapon(WeaponItem _rightHandWeapon_referedItem, bool _isFistWeapon)
        {
            if (_isFistWeapon)
            {
                RegisterNew_Fist_UnSheath2H_AnimJob();
            }
            else
            {
                a_hook.RegisterNewAnimationJob(_rightHandWeapon_referedItem.GetThLocomotionHashByType(), false);
            }
            
            anim.SetBool(p_IsTwoHanding_hash, true);
        }
        
        public void OffTwoHandingRhWeapon(WeaponItem _rightHandWeapon_referedItem)
        {
            CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
            anim.SetBool(p_IsTwoHanding_hash, false);
        }

        public void OffTwoHandingFistBeforeUnSheathLhWeapon()
        {
            RegisterNew_Fist_Sheath2H_AnimJob();
        }

        public void OnTwoHandingLhWeapon(WeaponItem _leftHandWeapon_referedItem, bool _isFistWeapon)
        {
            if (_isFistWeapon)
            {
                RegisterNew_Fist_UnSheath2H_AnimJob();
            }
            else
            {
                a_hook.RegisterNewAnimationJob(p_switchToRh_hash, false);
                a_hook.RegisterNewAnimationJob(_leftHandWeapon_referedItem.GetThLocomotionHashByType(), false);

                _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);
            }
            
            anim.SetBool(p_IsTwoHanding_hash, true);
        }

        public void OffTwoHandingLhWeapon(WeaponItem _leftHandWeapon_referedItem, bool _isFistWeapon)
        {
            if (_isFistWeapon)
            {
                RegisterNew_Fist_Sheath2H_AnimJob();
            }
            else
            {
                a_hook.RegisterNewAnimationJob(p_switchToLh_hash, false);
                _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);
            }
            
            anim.SetBool(p_IsTwoHanding_hash, false);
        }

        public void OffTwoHandingWhenSetupSlot()
        {
            _isTwoHanding = false;
            anim.SetBool(p_IsTwoHanding_hash, false);
            _currentLocoIKState.OffTwoHanding(this);
        }

        public void OffTwoHandingFistInMenuBeforeSetupSlot()
        {
            RegisterNew_Fist_Sheath2H_AnimJob();
            OffTwoHandingWhenSetupSlot();
        }
        
        public void OffTwoHandingFistOnRevive()
        {
            _isInTwoHandFist = false;
        }
        #endregion

        #region Roll / Jump / Run Inputs.
        bool HandleRollAndJumpAction()
        {
            if (runInputAmount > 0 && !p_run_input && _hasReleaseRunButton)
            {
                if (_isWaitForJumpInput)
                {
                    _currentNeglectInputAction = jumpAction;
                    return true;
                }
                else
                {
                    _currentNeglectInputAction = rollAction;
                    return true;
                }
            }

            //if (_test)
            //{
            //    if (!_isRunning)
            //    {
            //        SetIsRunningStatusToTrue();
            //    }
            //}
            //else
            //{
            //    if (_isRunning)
            //    {
            //        SetIsRunningStatusToFalse(false);
            //    }
            //}

            if (moveAmount == 1 && runInputAmount > runThershold && _hasReleaseRunButton)
            {
                if (!_isRunning)
                {
                    SetIsRunningStatusToTrue();
                }

                statsHandler.DecrementPlayerStaminaWhenRunning();
            }

            return false;
        }

        public void RollAction()
        {
            _isRolling = true;

            isCantBeDamaged = true;
            invincibleTimer = 0;
            
            statsHandler.DecrementPlayerStaminaWhenRolling();

            if (moveAmount > 0)
            {
                if (isLockingOn)
                {
                    rollRootMotion = moveDirection;
                    Set2DRollLocomotionAnimValue();
                }
                else
                {
                    mTransform.rotation = Quaternion.LookRotation(moveDirection);
                    rollRootMotion = mTransform.forward;
                    
                    Set1DRollLocomotionAnimValue();
                }

                PlayRollAnimation();
            }
            else
            {
                rollRootMotion = -mTransform.forward;
                CrossFadeAnimWithMoveDir(p_backstep_hash, false, true);
            }
        }

        public void JumpAction()
        {
            _isJumping = true;
            canMoveWhileNeglect = true;

            skipGroundCheck = true;
            
            _isWaitForJumpInput = false;
            jumpInputWaitTimer = 0;
            
            statsHandler.DecrementPlayerStaminaWhenJumping();

            if (_savableInventory._isBothUnarmed)
            {
                CrossFadeAnimWithMoveDir(p_unarmed_jump_start_hash, true, true);
            }
            else
            {
                CrossFadeAnimWithMoveDir(p_armed_jump_start_hash, true, true);
            }

            isGrounded = false;
            anim.SetBool(p_IsGrounded_hash, false);
        }

        void PerformEvadeAction()
        {
            _isRolling = true;

            isCantBeDamaged = true;
            invincibleTimer = 0;

            statsHandler.isPausingStaminaRecover = true;
            statsHandler.DecrementPlayerStaminaWhenEvading();

            if (moveAmount > 0)
            {
                if (isLockingOn)
                {
                    rollRootMotion = moveDirection;
                    Set2DRollLocomotionAnimValue();
                }
                else
                {
                    mTransform.rotation = Quaternion.LookRotation(moveDirection);
                    rollRootMotion = mTransform.forward;

                    SetFwdEvadeLocomAnimValue();
                }
            }
            else
            {
                rollRootMotion = -mTransform.forward;
                SetBwdEvadeLocoAnimValue();
            }

            PlayEvadeAnimation();
        }
        #endregion
        
        #region Calculate Front Step Height.
        public void CalculateFrontHeight()
        {
            //Debug.DrawRay(moveWithPlayerRayOrigin, -vector3Up * frontRayLength, Color.red);
            if (moveAmount > 0.1f)
            {
                CalculateFrontStepHeight();
            }
            else
            {
                frontStepHeight = 0;
            }
        }

        public void CalculateFrontHeightWhileNeglect()
        {
            if (canMoveWhileNeglect && isGrounded)
            {
                CalculateFrontStepHeight();
            }
            else
            {
                frontStepHeight = 0;
            }
        }

        void CalculateFrontStepHeight()
        {
            frontStepHeight = 0;
            moveWithPlayerRayOrigin = mTransform.position + (mTransform.forward * frontRayOffset);
            moveWithPlayerRayOrigin.y += frontRayHeight;
            
            if (Physics.Raycast(moveWithPlayerRayOrigin, -vector3Up, out RaycastHit hit, frontRayLength, _layerManager._playerStepCheckMask))
            {
                frontStepHeight = hit.point.y - mTransform.position.y;
            }
        }
        #endregion

        #region Non Neglect Action Inputs.
        public void HandleNonNeglectActionInputs()
        {
            if (!_isWaitForStaminaRecover)
            {
                HandleSprinting();
            }

            if (!_isWaitForWeaponSwitch)
            {
                HandleTwoHanding();
            }
        }
        #endregion

        #region Neglect Action Inputs.
        public void HandleNeglectActionInputs()
        {
            if (!_isWaitForWeaponSwitch && !_isSprinting)
            {
                if (!_isWaitForStaminaRecover)
                {
                    if (_savableInventory.HandleWeaponActions(_isTwoHanding))
                        return;

                    if (HandleRollAndJumpAction())
                        return;
                }

                _savableInventory.HandleConsumableAction();
            }
        }

        public void HandleRollWhenControlByAgent()
        {
            if (!_isWaitForStaminaRecover)
            {
                if (HandleRollAndJumpAction())
                    return;
            }
        }
        #endregion

        #region Execute Neglect Action.
        public void ExecuteNeglectAction()
        {
            if (_currentNeglectInputAction != null)
            {
                _currentNeglectInputAction.Execute(this);

                OnExecuteNeglectActionResetBools();

                if (!_isActionRequireUpdateInNeglectState)
                    _currentNeglectInputAction = null;
            }
        }
        #endregion

        #region Action Require Update In Neglect State.
        public void UpdateActionInNeglectState()
        {
            if (_isActionRequireUpdateInNeglectState)
            {
                _savableInventory.HandleWeaponInputs(_isTwoHanding);
                _currentNeglectInputAction.Execute(this);
            }
        }
        #endregion

        #region Move With Player.
        public void MoveWithPlayerIdle()
        {
            Vector3 targetVelocity = vector3Zero;

            if (isGrounded)
            {
                if (moveAmount > 0.1f)
                {
                    targetVelocity = mTransform.forward * (moveAmount * statsHandler._walk_speed * _fixedDelta);

                    // if player is locking on to enemy
                    if (isLockingOn)
                    {
                        targetVelocity = moveDirection * (moveAmount * statsHandler._walk_speed * _fixedDelta);
                    }

                    // if player is running
                    if (_isRunning)
                    {
                        targetVelocity = moveDirection * (moveAmount * statsHandler._run_speed * _fixedDelta);
                    }

                    // if player is sprinting and can change it's velocity
                    if (_isSprintingChangeVelocity)
                    {
                        targetVelocity = moveDirection * (moveAmount * statsHandler._run_speed * 1.65f * _fixedDelta);
                    }

                    // check the road before player is higher or lower
                    if (Mathf.Abs(frontStepHeight) > 0.02f)
                    {
                        // if the the road is higher, raise the player a bit.
                        if (frontStepHeight > 0)
                        {
                            targetVelocity.y = frontStepHeight * frontRayRaiseAmount;
                            //Debug.Log("+targetVelocity.y = " + targetVelocity.y);

                        }
                        // otherwise lower the player a bit.
                        else
                        {
                            targetVelocity.y = frontStepHeight * frontRayDropAmount;
                        }
                    }

                    //p_rb.isKinematic = false;
                    p_rb.MovePosition(mTransform.position + targetVelocity);
                }
            }
            else
            {
                targetVelocity = moveDirection * (moveAmount * statsHandler._walk_speed * _fixedDelta);
                targetVelocity += transform.up * (statsHandler.b_fall_speed * _fixedDelta * -1);

                //p_rb.isKinematic = false;
                p_rb.MovePosition(mTransform.position + targetVelocity);
            }
        }

        public void MoveWithPlayerWhileNeglect()
        {
            if (canMoveWhileNeglect)
            {
                Vector3 targetVelocity = vector3Zero;

                if (isGrounded)
                {
                    if (moveAmount > 0.1f)
                    {
                        targetVelocity = moveDirection * (moveAmount * statsHandler.b_consumable_walk_speed * _fixedDelta);

                        // check the road before player is higher or lower
                        if (Mathf.Abs(frontStepHeight) > 0.02f)
                        {
                            // if the the road is higher, raise the player a bit.
                            if (frontStepHeight > 0)
                            {
                                targetVelocity.y = frontStepHeight * frontRayRaiseAmount;

                            }
                            // otherwise lower the player a bit.
                            else
                            {
                                targetVelocity.y = frontStepHeight * frontRayDropAmount;
                            }
                        }
                        
                        //p_rb.isKinematic = false;
                        p_rb.MovePosition(mTransform.position + targetVelocity);
                    }
                }
                // if player is falling or just jump.
                else
                {
                    if (!_isJumping)
                    {
                        targetVelocity = moveDirection * (moveAmount * statsHandler._walk_speed * _fixedDelta);
                        targetVelocity += transform.up * (statsHandler.b_fall_speed * _fixedDelta * -1);
                    }

                    //p_rb.isKinematic = false;
                    p_rb.MovePosition(mTransform.position + targetVelocity);
                }
            }
        }

        public void MoveWithPlayerInFighterMode()
        {
            if (canMoveWhileNeglect)
            {
                Vector3 targetVelocity = vector3Zero;

                // if player is falling or just jump.
                if (!isGrounded)
                {
                    if (!_isJumping)
                    {
                        targetVelocity = moveDirection * (moveAmount * statsHandler._walk_speed * _fixedDelta);
                        targetVelocity += transform.up * (statsHandler.b_fall_speed * _fixedDelta * -1);
                    }

                    //p_rb.isKinematic = false;
                    p_rb.MovePosition(mTransform.position + targetVelocity);
                }
            }
        }
        #endregion

        #region Turn With Player.
        public void TurnWithPlayerBeforeNeglect()
        {
            if (!isLockingOn)
            {
                TurnWithMoveDir_BeforeNeglect();
            }
            else
            {
                TurnWithLockonDir_BeforeNeglect();
            }
        }

        public void TurnWithPlayerWhileNeglect()
        {
            if (applyTurningWithMoveDir)
            {
                TurnWithMoveDir_AfterNeglect();
            }
            else if (applyTurningWithLockonDir)
            {
                TurnWithLockonDir_AfterNeglect();
            }
            else if (applyTurningWithInverseMoveDir)
            {
                TurnWithInverseMoveDir_AfterNeglect();
            }
        }

        void TurnWithMoveDir_BeforeNeglect()
        {
            Quaternion targetRotation;

            if (moveAmount != 0)
            {
                targetRotation = Quaternion.Slerp(mTransform.rotation, Quaternion.LookRotation(moveDirection), moveAmount * normalTurnSpeedBeforeNeglect * _fixedDelta);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(mTransform.forward);
            }

            mTransform.rotation = targetRotation;
        }

        void TurnWithMoveDir_AfterNeglect()
        {
            Quaternion targetRotation;

            if (moveAmount != 0)
            {
                targetRotation = Quaternion.Slerp(mTransform.rotation, Quaternion.LookRotation(moveDirection), moveAmount * normalTurnSpeedAfterNeglect * _fixedDelta);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(mTransform.forward);
            }

            mTransform.rotation = targetRotation;
        }

        void TurnWithInverseMoveDir_AfterNeglect()
        {
            // MOVE DIRECTION
            Transform camHandlerTrans = _camHandler._mTransform;
            Vector3 _inverseMoveDir = camHandlerTrans.forward * vertical;
            _inverseMoveDir += -camHandlerTrans.right * horizontal;     //moveDirection = Vector3.Normalize(moveDirection);
            _inverseMoveDir.y = 0;

            mTransform.rotation = Quaternion.Slerp(mTransform.rotation, Quaternion.LookRotation(_inverseMoveDir), moveAmount * normalTurnSpeedAfterNeglect * _fixedDelta);
        }

        void TurnWithLockonDir_BeforeNeglect()
        {
            mTransform.rotation = Quaternion.Slerp(mTransform.rotation, Quaternion.LookRotation(_dirToLockonStates), lockonTurnSpeedBeforeNeglect * _fixedDelta);
        }

        void TurnWithLockonDir_AfterNeglect()
        {
            mTransform.rotation = Quaternion.Slerp(mTransform.rotation, Quaternion.LookRotation(_dirToLockonStates), lockonTurnSpeedAfterNeglect * _fixedDelta);
        }

        /// Anim Event.
        public void SetCurrentTurningTypeStatusToFalse()
        {
            if (applyTurningWithMoveDir)
            {
                applyTurningWithMoveDir = false;
            }
            else
            {
                applyTurningWithLockonDir = false;
            }
        }
        #endregion

        #region Anim With Player.
        public void SetPlayerAnim()
        {
            if (isLockingOn)
            {
                anim.SetFloat(vertical_hash, vertical, 0.1f, _delta);
                anim.SetFloat(horizontal_hash, horizontal, 0.1f, _delta);
            }
            else
            {
                anim.SetFloat(vertical_hash, moveAmount, 0.1f, _delta);
                anim.SetFloat(horizontal_hash, 0);
            }
        }

        public void OnAttackRemoveAnimLocomotionValue()
        {
            LeanTween.value(anim.GetFloat(vertical_hash), 0, 0.35f).setOnUpdate((value) => anim.SetFloat(vertical_hash, value));
            LeanTween.value(anim.GetFloat(horizontal_hash), 0, 0.35f).setOnUpdate((value) => anim.SetFloat(horizontal_hash, value));
        }

        #region Hold Attack.
        public void SetHoldAttackSpeedMultiAnimFloat(float value)
        {
            anim.SetFloat(p_HoldAttackSpeedMulti_hash, value);
        }

        public void RegisterManualHoldAttackSpeed(float value)
        {
            anim.SetFloat(p_HoldAttackSpeedMulti_hash, 1);
            _manualSetHoldAttackSpeedMultiValue = value;
        }

        public void SetHoldAttackSpeedFromAnimEvent()
        {
            /// If Player is still Holding Attack Button. Hold Attack is still going on.
            if (_isActionRequireUpdateInNeglectState)
                anim.SetFloat(p_HoldAttackSpeedMulti_hash, _manualSetHoldAttackSpeedMultiValue);
        }

        public void WeaponActionResetVerticalHorizontalPara()
        {
            anim.SetFloat(vertical_hash, 0);
            anim.SetFloat(horizontal_hash, 0);
        }
        #endregion

        #region Can Move While Neglect Anim Para.
        public void HandleNeglectStateMoveAnim()
        {
            if (canMoveWhileNeglect)
            {
                anim.SetBool(p_IsRunning_hash, false);

                anim.SetFloat(vertical_hash, moveAmount, 0.1f, _delta);

                if (isLockingOn)
                {
                    anim.SetFloat(horizontal_hash, moveAmount, 0.1f, _delta);
                }
                else
                {
                    anim.SetFloat(horizontal_hash, 0, 0.1f, _delta);
                }
            }
            else
            {
                if (_savableInventory._isUsingConsumable)
                {
                    anim.SetFloat(vertical_hash, 0, 0.2f, _delta);
                    anim.SetFloat(horizontal_hash, 0, 0.2f, _delta);
                }
            }
        }
        #endregion

        #region Roll Evade Vertical / Horizontal Whole Anim Para.
        public void Set1DRollLocomotionAnimValue()
        {
            anim.SetFloat(horizontal_whole_hash, 0);
            anim.SetFloat(vertical_whole_hash, 1);
        }

        public void Set2DRollLocomotionAnimValue()
        {
            if (horizontal != 0)
            {
                if (horizontal > 0)
                {
                    anim.SetFloat(horizontal_whole_hash, 1);
                }
                else
                {
                    anim.SetFloat(horizontal_whole_hash, -1);
                }
            }
            else
            {
                anim.SetFloat(horizontal_whole_hash, 0);
            }

            if (vertical != 0)
            {
                if (vertical > 0)
                {
                    anim.SetFloat(vertical_whole_hash, 1);
                }
                else
                {
                    anim.SetFloat(vertical_whole_hash, -1);
                }
            }
            else
            {
                anim.SetFloat(vertical_whole_hash, 0);
            }
        }

        #region Evade
        public void SetFwdEvadeLocomAnimValue()
        {
            anim.SetFloat(horizontal_whole_hash, 0);
            anim.SetFloat(vertical_whole_hash, 1);
        }

        public void SetBwdEvadeLocoAnimValue()
        {
            anim.SetFloat(horizontal_whole_hash, 0);
            anim.SetFloat(vertical_whole_hash, -1);
        }
        #endregion

        #endregion

        #endregion
        
        #region LocomotionIK State.
        public void LocoIKStateTick()
        {
            if (!_isPausingLocoIKStateTick)
                _currentLocoIKState.Tick(this);
        }

        public void PauseLocoIKStateTick()
        {
            _playerIKHandler.DeactivateIKWhenINeglect();
            _isPausingLocoIKStateTick = true;
        }

        public void ResumeLocoIKStateTick()
        {
            /// If Running is Still true, means that this is called from 'TwoHandingRhWeapon' or 'BackFromRhTwoHanding'.
            if (_isRunning)
            {
                _currentLocoIKState.OnRunningIK(this);
            }
            else
            {
                _isPausingLocoIKStateTick = false;
            }
        }
        
        #region Surrounding LookAt IK.
        void CalculateDisDirAngleToSurroundTarget()
        {
            if (Time.frameCount % 5 == 0)
            {
                /// Direction.
                _dirToSurroundTarget = _cur_surround_lookAtTarget.position - mTransform.position;
                _dirToSurroundTarget.y = 0;
                //Debug.DrawRay(mTransform.position, _dirToSurroundTarget);

                /// Distance.
                _sqrtDisToSurroundTarget = Vector3.SqrMagnitude(_dirToSurroundTarget);

                /// Angle
                Vector3 _fwd = mTransform.forward;
                _fwd.y = 0;

                _angleToSurroundTarget = Vector3.Angle(_dirToSurroundTarget, _fwd);
            }
        }

        //void UpdateLookAtSurroundingObj()
        //{
        //    Vector3 _targetPos = _cur_surround_lookAtTarget.position + vector3Up * CalculateTagetLookAtHeight(_cur_surround_lookAtTarget.position.y);

        //    if (_angleToSurroundTarget < _surroundIKHeadOnlyAngle)
        //    {
        //        _playerIKHandler.UpdateFreeForm_CustomizedLookAtIK(true, CalculateTagetSurroundLookAtWeight(), _targetPos);
        //    }
        //    else
        //    {
        //        _playerIKHandler.UpdateFreeForm_CustomizedLookAtIK(false, CalculateTagetSurroundLookAtWeight(), _targetPos);
        //    }

        //    float CalculateTagetLookAtHeight(float _lookAtTargetHeight)
        //    {
        //        if (_lookAtTargetHeight >= 1.2f)
        //        {
        //            return _lookAtTargetHeight > 1.4 ? 1.4f : _lookAtTargetHeight;
        //        }
        //        else
        //        {
        //            return _lookAtTargetHeight < 1 ? 0.9f : _lookAtTargetHeight;
        //        }
        //    }
        //}

        void UpdateLookAtSurroundingEnemy()
        {
            _playerIKHandler.UpdateFreeForm_CustomizedLookAtIK(false, CalculateTagetSurroundLookAtWeight(), _cur_surround_lookAtTarget.position + (vector3Up * 1.45f));

            float CalculateTagetSurroundLookAtWeight()
            {
                return Mathf.Lerp(1, 0.65f, _sqrtDisToSurroundTarget / _surd_IK_Weight_MinRange);
            }
        }
        
        #region 1H.
        void TryGetSurroundTargetFrom_SingleGoal_1H()
        {
            bool _isGoalInSight = false;

            CheckIsSurroundGoalInSight();

            SetSurroundTargetIfValid();

            void CheckIsSurroundGoalInSight()
            {
                Vector3 _dirToFoundSurroundLookAtGoal = _surroundFoundCols[0].transform.position - headTransform.position;
                
                Vector3 _headFwdDir = headTransform.forward;
                _headFwdDir.y = _dirToFoundSurroundLookAtGoal.y;

                if (Vector3.Angle(_dirToFoundSurroundLookAtGoal, _headFwdDir) <= _surroundLookAtAngleThershold_1H)
                {
                    if (!Physics.Raycast(headTransform.position, _dirToFoundSurroundLookAtGoal, Vector3.Magnitude(_dirToFoundSurroundLookAtGoal), _layerManager._lockonObstaclesMask))
                    {
                        _isGoalInSight = true;
                    }
                }
            }

            void SetSurroundTargetIfValid()
            {
                if (_isGoalInSight)
                {
                    _cur_surround_lookAtTarget = _surroundFoundCols[0].transform;
                    SetSurroundingLookAtTarget_1H();
                }
                else
                {
                    FreeFormLookForwardIKBehaviour_1H();
                }
            }
        }
        
        void TryGetSurroundTargetFrom_MultiGoals_1H()
        {
            Vector3 _dirToFoundSurroundLookAtGoal;
            Vector3 _headFwdDir;

            float _shortestDisToFoundTarget = 1000f;
            Transform _shortestFoundSurroundGoal = null;

            for (int i = 0; i < _cur_surround_hitAmounts; i++)
            {
                #region Get Directions From Player and Goals.
                _dirToFoundSurroundLookAtGoal = _surroundFoundCols[i].transform.position - headTransform.position;

                _headFwdDir = headTransform.forward;
                _headFwdDir.y = _dirToFoundSurroundLookAtGoal.y;
                #endregion

                if (Vector3.Angle(_dirToFoundSurroundLookAtGoal, _headFwdDir) <= _surroundLookAtAngleThershold_1H)
                {
                    #region Get Sqrt Distance.
                    float temp_dis = Vector3.Magnitude(_dirToFoundSurroundLookAtGoal);
                    #endregion

                    if (!Physics.Raycast(headTransform.position, _dirToFoundSurroundLookAtGoal, temp_dis, _layerManager._lockonObstaclesMask))
                    {
                        if (temp_dis < _shortestDisToFoundTarget)
                        {
                            _shortestDisToFoundTarget = temp_dis;
                            _shortestFoundSurroundGoal = _surroundFoundCols[i].transform;
                        }
                    }
                }
            }

            if (_shortestFoundSurroundGoal)
            {
                _cur_surround_lookAtTarget = _shortestFoundSurroundGoal;
                SetSurroundingLookAtTarget_1H();
            }
            else
            {
                FreeFormLookForwardIKBehaviour_1H();
            }
        }
        
        void SetSurroundingLookAtTarget_1H()
        {
            CalculateDisDirAngleToSurroundTarget();
            Calculate_1H_SurroundIKUpperBodyAngleThershold();

            if (_angleToSurroundTarget < _surroundIKUpperBodyAngle)
            {
                UpdateLookAtSurroundingEnemy();
                UpdateIKGoalsSurrounding_1H();
            }
            else
            {
                FreeFormLookForwardIKBehaviour_1H();
            }
            
            void Calculate_1H_SurroundIKUpperBodyAngleThershold()
            {
                _surroundIKUpperBodyAngle = Mathf.Lerp(_surd_IK_UpperBody_MaxAngle_1H, _surd_IK_UpperBody_MinAngle_1H, _sqrtDisToSurroundTarget / _surd_IK_UpperBody_MaxAngleDis);
            }
        }

        void UpdateIKGoalsSurrounding_1H()
        {
            if (p_walk_input)
            {
                _playerIKHandler.Update_1H_Walking_IKGoals_FreeFormSurrounding();
            }
            else
            {
                _playerIKHandler.Update_1H_Idle_IKGoals_FreeFormSurrounding();
            }
        }
        #endregion

        #region 2H.
        void TryGetSurroundTargetFrom_SingleGoal_2H()
        {
            bool _isGoalInSight = false;

            CheckIsSurroundGoalInSight();

            SetSurroundTargetIfValid();

            void CheckIsSurroundGoalInSight()
            {
                Vector3 _dirToFoundSurroundLookAtGoal = _surroundFoundCols[0].transform.position - headTransform.position;

                Vector3 _headFwdDir = headTransform.forward;
                _headFwdDir.y = _dirToFoundSurroundLookAtGoal.y;

                if (Vector3.Angle(_dirToFoundSurroundLookAtGoal, _headFwdDir) <= _surroundLookAtAngleThershold_2H)
                {
                    if (!Physics.Raycast(headTransform.position, _dirToFoundSurroundLookAtGoal, Vector3.Magnitude(_dirToFoundSurroundLookAtGoal), _layerManager._lockonObstaclesMask))
                    {
                        _isGoalInSight = true;
                    }
                }
            }

            void SetSurroundTargetIfValid()
            {
                if (_isGoalInSight)
                {
                    _cur_surround_lookAtTarget = _surroundFoundCols[0].transform;
                    SetSurroundingLookAtTarget_2H();
                }
                else
                {
                    FreeFormLookForwardIKBehaviour_2H();
                }
            }
        }

        void TryGetSurroundTargetFrom_MultiGoals_2H()
        {
            Vector3 _dirToFoundSurroundLookAtGoal;
            Vector3 _headFwdDir;

            float _shortestDisToFoundTarget = 1000f;
            Transform _shortestFoundSurroundGoal = null;

            for (int i = 0; i < _cur_surround_hitAmounts; i++)
            {
                #region Get Directions From Player and Goals.
                _dirToFoundSurroundLookAtGoal = _surroundFoundCols[i].transform.position - headTransform.position;

                _headFwdDir = headTransform.forward;
                _headFwdDir.y = _dirToFoundSurroundLookAtGoal.y;
                #endregion

                if (Vector3.Angle(_dirToFoundSurroundLookAtGoal, _headFwdDir) <= _surroundLookAtAngleThershold_2H)
                {
                    #region Get Sqrt Distance.
                    float temp_dis = Vector3.Magnitude(_dirToFoundSurroundLookAtGoal);
                    #endregion

                    if (!Physics.Raycast(headTransform.position, _dirToFoundSurroundLookAtGoal, temp_dis, _layerManager._lockonObstaclesMask))
                    {
                        if (temp_dis < _shortestDisToFoundTarget)
                        {
                            _shortestDisToFoundTarget = temp_dis;
                            _shortestFoundSurroundGoal = _surroundFoundCols[i].transform;
                        }
                    }
                }
            }

            if (_shortestFoundSurroundGoal)
            {
                _cur_surround_lookAtTarget = _shortestFoundSurroundGoal;
                SetSurroundingLookAtTarget_2H();
            }
            else
            {
                FreeFormLookForwardIKBehaviour_2H();
            }
        }
        
        void SetSurroundingLookAtTarget_2H()
        {
            CalculateDisDirAngleToSurroundTarget();
            Calculate_2H_SurroundIKUpperBodyAngleThershold();

            if (_angleToSurroundTarget < _surroundIKUpperBodyAngle)
            {
                UpdateLookAtSurroundingEnemy();
                UpdateIKGoalsSurrounding_2H();
            }
            else
            {
                FreeFormLookForwardIKBehaviour_2H();
            }

            void Calculate_2H_SurroundIKUpperBodyAngleThershold()
            {
                _surroundIKUpperBodyAngle = Mathf.Lerp(_surd_IK_UpperBody_MaxAngle_2H, _surd_IK_UpperBody_MinAngle_2H, _sqrtDisToSurroundTarget / _surd_IK_UpperBody_MaxAngleDis);
            }
        }

        void UpdateIKGoalsSurrounding_2H()
        {
            if (p_walk_input)
            {
                _playerIKHandler.Update_2H_Walking_IKGoals_FreeFormSurrounding();
            }
            else
            {
                _playerIKHandler.Update_2H_Idle_IKGoals_FreeFormSurrounding();
            }
        }
        #endregion

        #endregion

        #region Freeform 1H LocomotionIK State.
        public void FreeFormLocoIKState_1H_Tick()
        {
            _cur_surround_hitAmounts = Physics.OverlapSphereNonAlloc(mTransform.position, _surroundLookAtIKRange, _surroundFoundCols, _layerManager._surroundLookAtIKTargetMask);
            if (_cur_surround_hitAmounts > 0)
            {
                FreeFormLookSurroundingIKBehaviour_1H();
            }
            else
            {
                FreeFormLookForwardIKBehaviour_1H();
            }
        }

        void FreeFormLookSurroundingIKBehaviour_1H()
        {
            if (_cur_surround_hitAmounts == 1)
            {
                TryGetSurroundTargetFrom_SingleGoal_1H();
            }
            else
            {
                TryGetSurroundTargetFrom_MultiGoals_1H();
            }
        }
        
        void FreeFormLookForwardIKBehaviour_1H()
        {
            if (p_walk_input/*moveAmount > 0.02f*/)
            {
                _playerIKHandler.UpdateFreeFormWalkingIK_1H();
            }
            else
            {
                _playerIKHandler.UpdateFreeFormIdle_1H();
            }
        }

        public void FreeFormLocoIKState_1H_OnRunningIK()
        {
            _isPausingLocoIKStateTick = true;
            _playerIKHandler.HandleFreeFormRunningIK_1H();
        }

        public void FreeFormLocoIKState_1H_OffRunningIK()
        {
            ResumeLocoIKStateTick();
        }

        public void FreeFormLocoIKState_1H_OnTwoHanding()
        {
            _currentLocoIKState = _playerIKHandler._freeForm_2H_LocoIKState;
            _playerIKHandler.SetCurrent_TH_WeaponIKProfile();
        }
        
        public void FreeFormLocoIKState_1H_OnDefense()
        {
            _currentLocoIKState = _playerIKHandler._freeFormDefense_1H_LocoIKState;

            if (_isRunning)
                _currentLocoIKState.OnRunningIK(this);
        }
        
        public void FreeFormLocoIKState_1H_OnLockon()
        {
            _currentLocoIKState = _playerIKHandler._lockon_1H_LocoIKState;
        }
        #endregion

        #region Freeform 2H LocomotionIK State.
        public void FreeFormLocoIKState_2H_Tick()
        {
            _cur_surround_hitAmounts = Physics.OverlapSphereNonAlloc(mTransform.position, _surroundLookAtIKRange, _surroundFoundCols, _layerManager._surroundLookAtIKTargetMask);
            if (_cur_surround_hitAmounts > 0)
            {
                FreeFormLookSurroundingIKBehaviour_2H();
            }
            else
            {
                FreeFormLookForwardIKBehaviour_2H();
            }
        }

        void FreeFormLookSurroundingIKBehaviour_2H()
        {
            if (_cur_surround_hitAmounts == 1)
            {
                TryGetSurroundTargetFrom_SingleGoal_2H();
            }
            else
            {
                TryGetSurroundTargetFrom_MultiGoals_2H();
            }
        }

        void FreeFormLookForwardIKBehaviour_2H()
        {
            if (p_walk_input)
            {
                _playerIKHandler.UpdateFreeFormWalkingIK_2H();
            }
            else
            {
                _playerIKHandler.UpdateFreeFormIdle_2H();
            }
        }

        public void FreeFormLocoIKState_2H_OnRunningIK()
        {
            _isPausingLocoIKStateTick = true;
            _playerIKHandler.HandleFreeFormRunningIK_2H();
        }

        public void FreeFormLocoIKState_2H_OffRunningIK()
        {
            ResumeLocoIKStateTick();
        }

        public void FreeFormLocoIKState_2H_OffTwoHanding()
        {
            _currentLocoIKState = _playerIKHandler._freeForm_1H_LocoIKState;
            _playerIKHandler.SetCurrent_RH_WeaponIKProfile();
        }

        public void FreeFormLocoIKState_2H_OnDefense()
        {
            _currentLocoIKState = _playerIKHandler._freeFormDefense_2H_LocoIKState;

            if (_isRunning)
                _currentLocoIKState.OnRunningIK(this);
        }

        public void FreeFormLocoIKState_2H_OnLockon()
        {
            _currentLocoIKState = _playerIKHandler._lockon_2H_LocoIKState;
        }
        #endregion

        #region Freeform 1H Defense LocomotionIK State.
        public void FreeFormDefenseLocoIKState_1H_Tick()
        {
            FreeFormDefenseForwardIKBehaviour_1H();
        }
        
        public void FreeFormDefenseLocoIKState_1H_OnRunningIK()
        {
            _isPausingLocoIKStateTick = true;
            _playerIKHandler.FreeForm_Oppose1DefenseRunningHandleIK();
        }

        public void FreeFormDefenseLocoIKState_1H_OffRunningIK()
        {
            ResumeLocoIKStateTick();
        }

        public void FreeFormDefenseLocoIKState_1H_OnTwoHanding()
        {
            _currentLocoIKState = _playerIKHandler._freeFormDefense_2H_LocoIKState;
            _playerIKHandler.SetCurrent_TH_WeaponIKProfile();
        }
        
        public void FreeFormDefenseLocoIKState_1H_OffDefense()
        {
            _currentLocoIKState = _playerIKHandler._freeForm_1H_LocoIKState;

            if (_isRunning)
                _currentLocoIKState.OnRunningIK(this);
        }

        public void FreeFormDefenseLocoIKState_1H_OnLockon()
        {
            _currentLocoIKState = _playerIKHandler._lockonDefense_1H_LocoIKState;
        }

        void FreeFormDefenseForwardIKBehaviour_1H()
        {
            if (p_walk_input/*moveAmount > 0.02f*/)
            {
                _playerIKHandler.UpdateFreeFormDefenseWalkingIK_1H();
            }
            else
            {
                _playerIKHandler.UpdateFreeFormDefenseIdle_1H();
            }
        }
        #endregion

        #region Freeform 2H Defense LocomotionIK State.
        public void FreeFormDefenseLocoIKState_2H_Tick()
        {
            FreeFormDefenseForwardIKBehaviour_2H();
        }
        
        public void FreeFormDefenseLocoIKState_2H_OnRunningIK()
        {
            _isPausingLocoIKStateTick = true;
            _playerIKHandler.FreeForm_Light2DefenseRunningHandleIK();
        }

        public void FreeFormDefenseLocoIKState_2H_OffRunningIK()
        {
            ResumeLocoIKStateTick();
        }
        
        public void FreeFormDefenseLocoIKState_2H_OffTwoHanding()
        {
            _currentLocoIKState = _playerIKHandler._freeFormDefense_1H_LocoIKState;
            _playerIKHandler.SetCurrent_RH_WeaponIKProfile();
        }

        public void FreeFormDefenseLocoIKState_2H_OffDefense()
        {
            _currentLocoIKState = _playerIKHandler._freeForm_2H_LocoIKState;

            if (_isRunning)
                _currentLocoIKState.OnRunningIK(this);
        }

        public void FreeFormDefenseLocoIKState_2H_OnLockon()
        {
            _currentLocoIKState = _playerIKHandler._lockonDefense_2H_LocoIKState;
        }

        void FreeFormDefenseForwardIKBehaviour_2H()
        {
            if (p_walk_input)
            {
                _playerIKHandler.UpdateFreeFormDefenseWalkingIK_2H();
            }
            else
            {
                _playerIKHandler.UpdateFreeFormDefenseIdle_2H();
            }
        }
        #endregion

        #region Lockon 1H LocomotionIK State.
        public void LockonLocoIKState_1H_Tick()
        {
            //Debug.DrawRay(mTransform.position, _dirToLockonStates);

            if (_angleToLockonStates < _lockonLookAtMaxAngleThershold)
            {
                LockonLookAtTarget_1H_IKBehaviour();
            }
            else
            {
                OutOfRange_1H_IKBehaviour();
            }
        }
        
        public void LockonLocoIKState_1H_OnRunningIK()
        {
            _isPausingLocoIKStateTick = true;
            _playerIKHandler.HandleLockonRunningIKGoal_1H();
        }

        public void LockonLocoIKState_1H_OffRunningIK()
        {
            ResumeLocoIKStateTick();
        }

        public void LockonLocoIKState_1H_OnTwoHanding()
        {
            _currentLocoIKState = _playerIKHandler._lockon_2H_LocoIKState;
            _playerIKHandler.SetCurrent_TH_WeaponIKProfile();
        }
        
        public void LockonLocoIKState_1H_OnDefense()
        {
            _currentLocoIKState = _playerIKHandler._lockonDefense_1H_LocoIKState;

            if (_isRunning)
                _currentLocoIKState.OnRunningIK(this);
        }

        public void LockonLocoIKState_1H_OffLockon()
        {
            _currentLocoIKState = _playerIKHandler._freeForm_1H_LocoIKState;
        }
        
        void LockonLookAtTarget_1H_IKBehaviour()
        {
            if (p_walk_input)
            {
                _playerIKHandler.UpdateLockonWalkingIK_1H(UpdateLookAtHelperPosToLockonTarget());
            }
            else
            {
                _playerIKHandler.UpdateLockonIdleIK_1H(UpdateLookAtHelperPosToLockonTarget());
            }
        }

        Vector3 UpdateLookAtHelperPosToLockonTarget()
        {
            float _lookAtTargetHeight = moveAmount > 0.1f ? 1.6f : 1.4f;
            return _dirToLockonStates + mTransform.position + vector3Up * _lookAtTargetHeight;
        }
        
        void OutOfRange_1H_IKBehaviour()
        {
            if (p_walk_input)
            {
                _playerIKHandler.UpdateFreeFormWalkingIK_1H();
            }
            else
            {
                _playerIKHandler.UpdateFreeFormIdle_1H();
            }
        }
        #endregion

        #region Lockon 2H LocomotionIK State.
        public void LockonLocoIKState_2H_Tick()
        {
            //Debug.DrawRay(mTransform.position, _dirToLockonStates);

            if (_angleToLockonStates < _lockonLookAtMaxAngleThershold)
            {
                LockonLookAtTarget_2H_IKBehaviour();
            }
            else
            {
                OutOfRange_2H_IKBehaviour();
            }
        }

        public void LockonLocoIKState_2H_OnRunningIK()
        {
            _isPausingLocoIKStateTick = true;
            _playerIKHandler.HandleLockonRunningIKGoal_2H();
        }

        public void LockonLocoIKState_2H_OffRunningIK()
        {
            ResumeLocoIKStateTick();
        }

        public void LockonLocoIKState_2H_OffTwoHanding()
        {
            _currentLocoIKState = _playerIKHandler._lockon_1H_LocoIKState;
            _playerIKHandler.SetCurrent_RH_WeaponIKProfile();
        }

        public void LockonLocoIKState_2H_OnDefense()
        {
            _currentLocoIKState = _playerIKHandler._lockonDefense_2H_LocoIKState;

            if (_isRunning)
                _currentLocoIKState.OnRunningIK(this);
        }

        public void LockonLocoIKState_2H_OffLockon()
        {
            _currentLocoIKState = _playerIKHandler._freeForm_2H_LocoIKState;
        }

        void LockonLookAtTarget_2H_IKBehaviour()
        {
            if (p_walk_input)
            {
                _playerIKHandler.UpdateLockonWalkingIK_2H(UpdateLookAtHelperPosToLockonTarget());
            }
            else
            {
                _playerIKHandler.UpdateLockonIdleIK_2H(UpdateLookAtHelperPosToLockonTarget());
            }
        }

        void OutOfRange_2H_IKBehaviour()
        {
            if (p_walk_input)
            {
                _playerIKHandler.UpdateFreeFormWalkingIK_2H();
            }
            else
            {
                _playerIKHandler.UpdateFreeFormIdle_2H();
            }
        }
        #endregion

        #region Lockon Defense 1H LocomotionIK State.
        public void LockonDefenseIKState_1H_Tick()
        {
            if (_angleToLockonStates < _lockonLookAtMaxAngleThershold)
            {
                LockonDefenseLookAtTarget_1H_IKBehaviour();
            }
            else
            {
                OutOfRangeDefense_1H_IKBehaviour();
            }
        }
        
        public void LockonDefenseIKState_1H_OnRunningIK()
        {
            _isPausingLocoIKStateTick = true;
            _playerIKHandler.Lockon_Oppose1DefenseRunningHandleIK();
        }

        public void LockonDefenseIKState_1H_OffRunningIK()
        {
            ResumeLocoIKStateTick();
        }

        public void LockonDefenseIKState_1H_OnTwoHanding()
        {
            _currentLocoIKState = _playerIKHandler._lockonDefense_2H_LocoIKState;
            _playerIKHandler.SetCurrent_TH_WeaponIKProfile();
        }
        
        public void LockonDefenseIKState_1H_OffDefense()
        {
            _currentLocoIKState = _playerIKHandler._lockon_1H_LocoIKState;

            if (_isRunning)
                _currentLocoIKState.OnRunningIK(this);
        }

        public void LockonDefenseIKState_1H_OffLockon()
        {
            _currentLocoIKState = _playerIKHandler._freeFormDefense_1H_LocoIKState;
        }

        void LockonDefenseLookAtTarget_1H_IKBehaviour()
        {
            if (p_walk_input)
            {
                _playerIKHandler.UpdateLockonDefenseWalkingIK_1H(UpdateLookAtHelperPosToLockonTarget());
            }
            else
            {
                _playerIKHandler.UpdateLockonDefenseIdleIK_1H(UpdateLookAtHelperPosToLockonTarget());
            }
        }

        void OutOfRangeDefense_1H_IKBehaviour()
        {
            if (p_walk_input)
            {
                _playerIKHandler.UpdateFreeFormDefenseWalkingIK_1H();
            }
            else
            {
                _playerIKHandler.UpdateFreeFormDefenseIdle_1H();
            }
        }
        #endregion

        #region Lockon Defense 2H LocomotionIK State.
        public void LockonDefenseIKState_2H_Tick()
        {
            if (_angleToLockonStates < _lockonLookAtMaxAngleThershold)
            {
                LockonDefenseLookAtTarget_2H_IKBehaviour();
            }
            else
            {
                OutOfRangeDefense_2H_IKBehaviour();
            }
        }

        public void LockonDefenseIKState_2H_OnRunningIK()
        {
            _isPausingLocoIKStateTick = true;
            _playerIKHandler.Lockon_Light2DefenseRunningHandleIK();
        }

        public void LockonDefenseIKState_2H_OffRunningIK()
        {
            ResumeLocoIKStateTick();
        }

        public void LockonDefenseIKState_2H_OffTwoHanding()
        {
            _currentLocoIKState = _playerIKHandler._lockonDefense_1H_LocoIKState;
            _playerIKHandler.SetCurrent_RH_WeaponIKProfile();
        }

        public void LockonDefenseIKState_2H_OffDefense()
        {
            _currentLocoIKState = _playerIKHandler._lockon_2H_LocoIKState;

            if (_isRunning)
                _currentLocoIKState.OnRunningIK(this);
        }

        public void LockonDefenseIKState_2H_OffLockon()
        {
            _currentLocoIKState = _playerIKHandler._freeFormDefense_2H_LocoIKState;
        }

        void LockonDefenseLookAtTarget_2H_IKBehaviour()
        {
            if (p_walk_input)
            {
                _playerIKHandler.UpdateLockonDefenseWalkingIK_2H(UpdateLookAtHelperPosToLockonTarget());
            }
            else
            {
                _playerIKHandler.UpdateLockonDefenseIdleIK_2H(UpdateLookAtHelperPosToLockonTarget());
            }
        }

        void OutOfRangeDefense_2H_IKBehaviour()
        {
            if (p_walk_input)
            {
                _playerIKHandler.UpdateFreeFormDefenseWalkingIK_2H();
            }
            else
            {
                _playerIKHandler.UpdateFreeFormDefenseIdle_2H();
            }
        }
        #endregion

        #endregion

        #region Reset Invincible
        public void HandlePlayerInvincibility()
        {
            if (isCantBeDamaged)
            {
                isInvincible = true;
            }
            else
            {
                if (isInvincible && !isDead)
                {
                    invincibleTimer += _delta;
                    if (invincibleTimer >= invincibleRate)
                    {
                        invincibleTimer = 0;
                        isInvincible = false;
                    }
                }
            }
        }
        #endregion

        #region Root Motions.
        public void ApplyRootMotions()
        {
            if (_isRolling)
            {
                CalculateRollRootMotion();
            }
            else if (_isJumping)
            {
                CalculateJumpRootMotion();
            }

            if (applyHitImpactMotion)
            {
                CalculateHitImpactRootMotion();
            }
        }

        public void ApplyFighterModeRootMotions()
        {
            if (_isRolling)
            {
                CalculateEvadeRootMotion();
            }

            if (applyHitImpactMotion)
            {
                CalculateHitImpactRootMotion();
            }
        }

        public void ApplyRootMotions_OnAnimMove()
        {
            /// Parry root motion comes from here.
            if (applyAttackRootMotion)
            {
                CalculateAttackRootMotion();
            }
            else if (applySprintEndMotion)
            {
                CalculateSprintEndRootMotion();
            }
        }
        
        void CalculateJumpRootMotion()
        {
            //p_rb.isKinematic = false;

            Vector3 tarVel = moveDirection * statsHandler._jump_speed;
            tarVel.y = statsHandler._jump_height;

            _fallingVelocity += statsHandler.b_jump_gravity;
            tarVel.y -= _fallingVelocity * _fixedDelta;
            p_rb.velocity = tarVel;
        }

        /// Curve Evaluate.
        void CalculateRollRootMotion()
        {
            rollSpeedCurveCounter += _fixedDelta;

            Vector3 tarVel = rollRootMotion * (statsHandler._roll_speed * _rollCurve.Evaluate(rollSpeedCurveCounter));
            p_rb.velocity = tarVel;
        }

        void CalculateEvadeRootMotion()
        {
            rollSpeedCurveCounter += _fixedDelta;

            Vector3 tarVel = rollRootMotion * (statsHandler.b_evade_speed * _rollCurve.Evaluate(rollSpeedCurveCounter));
            p_rb.velocity = tarVel;
        }

        void CalculateHitImpactRootMotion()
        {
            //p_rb.isKinematic = false;

            hitImpactSpeedCurveCounter += _fixedDelta;

            Vector3 impactDir = vector3Zero;
            switch (_damageTakenDirectionType)
            {
                case DamageTakenDirectionTypeEnum.HitFromLeft:
                    impactDir = mTransform.right;
                    break;
                case DamageTakenDirectionTypeEnum.HitFromRight:
                    impactDir = -mTransform.right;
                    break;
                case DamageTakenDirectionTypeEnum.HitFromFront:
                    impactDir = -mTransform.forward;
                    break;
                case DamageTakenDirectionTypeEnum.HitFromBack:
                    impactDir = mTransform.forward;
                    break;
            }

            impactDir.y = 0;
            impactDir *= _hasBlockingBroken ? blockBrokenImpactSpeed : hitImpactSpeed * (_fixedDelta * _hitImpactCurve.Evaluate(hitImpactSpeedCurveCounter));
            p_rb.velocity = impactDir;
        }

        /// Delta Position.
        void CalculateAttackRootMotion()
        {
            Vector3 tarVel = anim.deltaPosition / _delta;
            tarVel *= CalculateAttackVelocity();
            p_rb.velocity = tarVel;

            float CalculateAttackVelocity()
            {
                float retVal = attackRootMotion;

                if (isLockingOn)
                {
                    if (!ignoreAttackRootCalculate)
                    {
                        float dis = Vector3.Distance(_lockonBodyBoneTransform.position, mTransform.position);
                        retVal = attackRootMotion * Mathf.Clamp01(dis / attackMaxVelocityDis);
                    }
                }

                return retVal;
            }
        }

        void CalculateSprintEndRootMotion()
        {
            // ROOT MOTION FOR ATTACK
            p_rb.velocity = anim.deltaPosition / _delta * sprintEndMotionSpeed;
        }

        // Anim Events.
        public void SetUseAttackRootMotionToFalse()
        {
            if (isComboAvailable)
                applyAttackRootMotion = false;
        }
        #endregion

        #region Quit Neglects.
        public void SetCanQuitNeglectStateToTrue()
        {
            isComboAvailable = true;
            canQuitNeglectState = true;
        }

        public void MonitorCanQuitNeglectStateEarly()
        {
            UpdateQuitNeglectStateInputAmount();

            // Quit Neglect for combo or from attack
            MonitorComboAttackAction();

            UpdateQuitOffGroundInputAmount();
        }

        public void ExecuteComboBlockAction()
        {
            isComboAvailable = false;
            canQuitNeglectState = false;
            _currentQuitNeglectInputAmount = 0;

            anim.SetBool(p_IsNeglecting_hash, false);

            lb_hold = true;

            SetIsBlockingStatus(true);
        }

        void UpdateQuitNeglectStateInputAmount()
        {
            if (canQuitNeglectState)
            {
                _currentQuitNeglectInputAmount += moveAmount;
                if (_currentQuitNeglectInputAmount >= _currentQuitNeglectInputThershold || p_run_down_input)
                {
                    isComboAvailable = false;
                    canQuitNeglectState = false;
                    _currentQuitNeglectInputAmount = 0;

                    anim.SetBool(p_IsNeglecting_hash, false);

                    /// Trail Fx.
                    if (_isUsedTrailFx)
                        a_hook.currentTrailFxHandler.Play_NullState_TrailFx();
                }
            }
        }
        
        void MonitorComboAttackAction()
        {
            if (isComboAvailable && !_isWaitForStaminaRecover)
            {
                if (_currentAttackAction)
                {
                    WeaponAction _comboAttackAction = _currentAttackAction.comboBranches.GetNextAttackAction(this);
                    if (_comboAttackAction)
                    {
                        isComboAvailable = false;
                        canQuitNeglectState = false;
                        _currentQuitNeglectInputAmount = 0;

                        /// Trail Fx.
                        if (_isUsedTrailFx)
                            a_hook.currentTrailFxHandler.Play_NullState_TrailFx();

                        _comboAttackAction.Execute(this);

                        /// For Hold / Charge Attack Combo.
                        if (_isActionRequireUpdateInNeglectState)
                            _currentNeglectInputAction = _comboAttackAction;
                    }
                }
            }
        }
        
        void UpdateQuitOffGroundInputAmount()
        {
            if (canQuitOffGroundEarly)
            {
                _currentQuitNeglectInputAmount += moveAmount;
                if (_currentQuitNeglectInputAmount >= 5)
                {
                    OnLand();
                    canQuitOffGroundEarly = false;
                    anim.SetBool(p_IsNeglecting_hash, false);
                }
            }
        }

        #region Fighter Mode.
        public void MonitorCanQuitFighterModeEarly()
        {
            UpdateQuitFighterModeInputAmount();
            
            MonitorComboAttackAction();
            
            MonitorFighterModeEvadeAction();
        }

        void UpdateQuitFighterModeInputAmount()
        {
            if (canQuitNeglectState)
            {
                _currentQuitNeglectInputAmount += moveAmount;
                if (_currentQuitNeglectInputAmount >= _QuitFighterModeInputThershold)
                {
                    isComboAvailable = false;
                    canQuitNeglectState = false;
                    _currentQuitNeglectInputAmount = 0;

                    anim.SetBool(p_IsNeglecting_hash, false);
                }
            }
        }

        void MonitorFighterModeEvadeAction()
        {
            if (p_run_down_input)
            {
                if (canQuitNeglectState && !_isWaitForStaminaRecover)
                {
                    isComboAvailable = false;
                    canQuitNeglectState = false;
                    _currentQuitNeglectInputAmount = 0;

                    /// Reset IsAttacking.
                    _isAttacking = false;
                    applyAttackRootMotion = false;
                    _isTwoHandFistAttacking = false;

                    /// Reset Turning.
                    applyTurningWithMoveDir = false;

                    /// Reset Roll Curve Counter.
                    rollSpeedCurveCounter = 0;

                    PerformEvadeAction();
                }
            }
        }
        #endregion
        
        #endregion

        #region Play Animations.

        #region Move Dir Turning.
        public void CrossFadeAnimWithMoveDir(int targetAnimHash, bool applyTurningWithMoveDir, bool isNeglecting)
        {
            this.applyTurningWithMoveDir = applyTurningWithMoveDir;
            isNeglectingInput = isNeglecting;

            anim.CrossFade(targetAnimHash, 0.1f);
            anim.SetBool(p_IsNeglecting_hash, isNeglecting);
        }

        public void PlayAnimWithMoveDir(int targetAnimHash, bool applyTurningWithMoveDir, bool isNeglecting)
        {
            this.applyTurningWithMoveDir = applyTurningWithMoveDir;
            isNeglectingInput = isNeglecting;

            anim.Play(targetAnimHash);
            anim.SetBool(p_IsNeglecting_hash, isNeglecting);
        }
        #endregion

        #region Lockon Dir Turning.
        public void CrossFadeAnimWithLockonDir(int targetAnimHash, bool isNeglecting)
        {
            applyTurningWithLockonDir = true;
            isNeglectingInput = isNeglecting;

            anim.CrossFade(targetAnimHash, 0.1f);
            anim.SetBool(p_IsNeglecting_hash, isNeglecting);
        }

        public void PlayAnimWithLockonDir(int targetAnimHash, bool isNeglecting)
        {
            applyTurningWithLockonDir = true;
            isNeglectingInput = isNeglecting;

            anim.Play(targetAnimHash);
            anim.SetBool(p_IsNeglecting_hash, isNeglecting);
        }
        #endregion

        #region Inverse Move Dir Turning.
        public void CrossFadeAnimWithInverseMoveDir(int targetAnimHash, bool isNeglecting)
        {
            applyTurningWithInverseMoveDir = true;
            isNeglectingInput = isNeglecting;

            anim.CrossFade(targetAnimHash, 0.1f);
            anim.SetBool(p_IsNeglecting_hash, isNeglecting);
        }

        public void PlayAnimWithInverseMoveDir(int targetAnimHash, bool isNeglecting)
        {
            applyTurningWithInverseMoveDir = true;
            isNeglectingInput = isNeglecting;

            anim.Play(targetAnimHash);
            anim.SetBool(p_IsNeglecting_hash, isNeglecting);
        }
        #endregion

        #region Items.
        public void PlayEmptyVesselAnimation()
        {
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);

            if (isLockingOn)
            {
                CrossFadeAnimWithLockonDir(p_item_vessel_empty_hash, true);
            }
            else
            {
                CrossFadeAnimWithMoveDir(p_item_vessel_empty_hash, false, true);
            }

            SetUseUnarmedLocoInConsumableToTrue();
        }

        public void PlayConsumableAnimation()
        {
            if (isLockingOn)
            {
                CrossFadeAnimWithLockonDir(_savableInventory._consumable_referedItem.consumableUsedAnim.animStateHash, true);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._consumable_referedItem.consumableUsedAnim.animStateHash, canMoveWhileNeglect, true);
            }

            SetUseUnarmedLocoInConsumableToTrue();
        }
        #endregion

        #region Roll.
        public void PlayRollAnimation()
        {
            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon.GetRollsTreeHashByType(), false, true);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._rightHandWeapon.GetRollsTreeHashByType(), false, true);
            }
        }

        public void PlayEvadeAnimation()
        {
            CrossFadeAnimWithMoveDir(p_fist_evade_tree_hash, false, true);
        }
        #endregion

        #region Blocking.
        public void PlayBlockingAnimation()
        {
            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.GetLight2BlockingHashByType(), false, false);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._leftHandWeapon_referedItem.GetOppose1BlockingHashByType(), false, false);
            }
        }

        public void PlayBlockingReactAnimation()
        {
            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.GetLight2BlockingReactHashByType(), false, false);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._leftHandWeapon_referedItem.GetOppose1BlockingReactHashByType(), false, false);
            }
        }

        public void PlayBlockingBreakAnimation()
        {
            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.GetLight2BlockingBreakHashByType(), false, true);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._leftHandWeapon_referedItem.GetOppose1BlockingBreakHashByType(), false, true);
            }
        }
        #endregion

        #region Hit.

        #region Small.
        public void PlayHitSmallFrontAnimation()
        {
            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.Get_TwoHanded_Small_Hit_F_HashByType(), false, false);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._rightHandWeapon_referedItem.Get_OneHanded_Small_Hit_F_HashByType(), false, false);
            }
        }

        public void PlayHitSmallBackAnimation()
        {
            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.Get_TwoHanded_Small_Hit_B_HashByType(), false, false);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._rightHandWeapon_referedItem.Get_OneHanded_Small_Hit_B_HashByType(), false, false);
            }
        }

        public void PlayHitSmallLeftAnimation()
        {
            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.Get_TwoHanded_Small_Hit_L_HashByType(), false, false);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._rightHandWeapon_referedItem.Get_OneHanded_Small_Hit_L_HashByType(), false, false);
            }
        }

        public void PlayHitSmallRightAnimation()
        {
            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.Get_TwoHanded_Small_Hit_R_HashByType(), false, false);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._rightHandWeapon_referedItem.Get_OneHanded_Small_Hit_R_HashByType(), false, false);
            }
        }
        #endregion

        #region Big.
        public void PlayHitBigFrontAnimation()
        {
            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.Get_TwoHanded_Big_Hit_F_HashByType(), false, true);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._rightHandWeapon_referedItem.Get_OneHanded_Big_Hit_F_HashByType(), false, true);
            }
        }

        public void PlayHitBigBackAnimation()
        {
            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.Get_TwoHanded_Big_Hit_B_HashByType(), false, true);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._rightHandWeapon_referedItem.Get_OneHanded_Big_Hit_B_HashByType(), false, true);
            }
        }

        public void PlayHitBigLeftAnimation()
        {
            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.Get_TwoHanded_Big_Hit_L_HashByType(), false, true);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._rightHandWeapon_referedItem.Get_OneHanded_Big_Hit_L_HashByType(), false, true);
            }
        }

        public void PlayHitBigRightAnimation()
        {
            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.Get_TwoHanded_Big_Hit_R_HashByType(), false, true);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._rightHandWeapon_referedItem.Get_OneHanded_Big_Hit_R_HashByType(), false, true);
            }
        }
        #endregion

        #region Knockback.
        public void PlayKnockbackAnimation()
        {
            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.Get_TwoHanded_Knockback_HashByType(), false, true);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._rightHandWeapon_referedItem.Get_OneHanded_Knockback_HashByType(), false, true);
            }
        }
        #endregion

        #endregion

        #region Death.
        public void PlayDeathAnimation()
        {
            anim.Play(p_empty_fullbody_lh_overide_hash);
            anim.Play(p_empty_fullbody_rh_overide_hash);

            if (_isTwoHanding)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.Get_TwoHanded_Death_HashByType(), false, true);
            }
            else
            {
                CrossFadeAnimWithMoveDir(_savableInventory._rightHandWeapon_referedItem.Get_OneHanded_Death_HashByType(), false, true);
            }
        }
        #endregion

        #region Revive.
        public void PlayReviveAnimation()
        {
            CrossFadeAnimWithMoveDir(_savableInventory._rightHandWeapon_referedItem.GetReviveHashByType(), false, true);
        }
        #endregion

        #region Execution Received.
        public void PlayExecutionReceivedAnim()
        {
            isNeglectingInput = true;

            anim.Play(_currentExecutionProfile._receiveAnimState.animStateHash);
            anim.SetBool(p_IsNeglecting_hash, true);

            PauseLocoIKStateTick();
        }
        #endregion

        #region Getup.
        public void PlayKnockedDownGetupAnim(int _animHash)
        {
            isNeglectingInput = true;

            anim.CrossFade(_animHash, 0.2f);
            anim.SetBool(p_IsNeglecting_hash, true);

            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);
        }
        #endregion

        #region Two Hand Fist Sheath / UnSheath.
        public void RegisterNew_Fist_UnSheath2H_AnimJob()
        {
            a_hook.RegisterNewAnimationJob(p_fist_th_unSheath_hash, false);
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.UpperBody, 1, vector3Zero);
            _isInTwoHandFist = true;
        }

        public void RegisterNew_Fist_Sheath2H_AnimJob()
        {
            a_hook.RegisterNewAnimationJob(p_fist_th_sheath_hash, false);
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.UpperBody, 1, vector3Zero);
            _isInTwoHandFist = false;
        }
        #endregion

        #region Interactions.
        public void IgniteBonfireInteractable()
        {
            CrossFadeAnimWithMoveDir(HashManager.singleton.p_int_bonfire_ignite_hash, false, true);
            _isPausingSearchInteractables = true;
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);
        }

        public void Set_IsLevelUpBegin_AnimParaToTrue()
        {
            anim.SetBool(p_IsLevelupBegin_hash, true);
        }

        public void PlayLevelUpEndAnim()
        {
            CrossFadeAnimWithMoveDir(p_int_levelup_end_hash, false, false);
            anim.SetBool(p_IsLevelupBegin_hash, false);
        }

        void Set_IsInBonfireEnd_AnimParaToFalse()
        {
            anim.SetBool(p_IsBonfireEnd_hash, false);
        }

        void Set_IsInBonfireEnd_AnimParaToTrue()
        {
            anim.SetBool(p_IsBonfireEnd_hash, true);
        }

        /// Chest.
        public void TakeChestInteractable(PickupCommentaryTypeEnum _commentType)
        {
            CrossFadeAnimWithMoveDir(p_int_takeChest_hash, false, true);
            _isPausingSearchInteractables = true;
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);

            _commentHandler._currentPickupCommentType = _commentType;
        }

        public void OpenChestInteractable()
        {
            CrossFadeAnimWithMoveDir(p_int_openChest_hash, false, true);
            _isPausingSearchInteractables = true;
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);
        }

        /// Door.
        public void CantOpenDoorInteractable()
        {
            CrossFadeAnimWithMoveDir(p_int_cantOpen_hash, false, true);
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);
        }

        public void OpenDoorInteractable()
        {
            CrossFadeAnimWithMoveDir(p_int_openDoor_hash, false, true);
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);
        }

        /// Pickups.
        public void PickupInteractable(PlayerPickupIntactablePosType _pickupPos)
        {
            switch (_pickupPos)
            {
                case PlayerPickupIntactablePosType.High:
                    CrossFadeAnimWithMoveDir(p_int_pickup_up_hash, false, true);
                    break;
                case PlayerPickupIntactablePosType.Mid:
                    CrossFadeAnimWithMoveDir(p_int_pickup_mid_hash, false, true);
                    break;
                case PlayerPickupIntactablePosType.Down:
                    CrossFadeAnimWithMoveDir(p_int_pickup_down_hash, false, true);
                    break;
            }

            _isPausingSearchInteractables = true;
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);
        }
        #endregion

        #region Weapon Actions.
        public void PlayMoveDirAttackAnimation(int animStateHash)
        {
            if (!_isTwoHanding)
                OnAttackRemoveAnimLocomotionValue();

            CrossFadeAnimWithMoveDir(animStateHash, true, true);
        }

        public void PlayLockonDirAttackAnimation(int animStateHash)
        {
            if (!_isTwoHanding)
                OnAttackRemoveAnimLocomotionValue();

            CrossFadeAnimWithLockonDir(animStateHash, true);
        }

        public void PlayInverseMoveDirAttackAnimation(int animStateHash)
        {
            if (!_isTwoHanding)
                OnAttackRemoveAnimLocomotionValue();

            CrossFadeAnimWithInverseMoveDir(animStateHash, true);
        }
        #endregion

        #region Parry.
        //public void PlayParryAnimation(bool _useMoveDir)
        //{
        //    if (_isTwoHanding)
        //    {
        //        CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.Get_Heavy2_ParryAnimState().animStateHash, _useMoveDir, true);
        //    }
        //    else
        //    {
        //        CrossFadeAnimWithMoveDir(_savableInventory._leftHandWeapon_referedItem.Get_Oppose2_ParryAnimState().animStateHash, _useMoveDir, true);
        //    }
        //}
        #endregion

        #endregion

        #region Set Status.

        #region Sprinting / Running / Jump.

        #region Sprinting.
        public void SetIsSprintingStatusToTrue()
        {
            _isSprinting = true;

            _savableInventory.BeginSprinting();

            _playerIKHandler.HandleOnSprintingIK();

            if (isLockingOn)
                SetIsLockingOnStatusToFalse();

            _commentHandler.PauseAcceptingComment();
        }
        
        public void BeginSpritingChangeVelocity()
        {
            _isRunning = false;
            _isSprintingChangeVelocity = true;
        }

        public void SetIsSprintingStatusToFalse()
        {
            _isSpritingEndAnimPlayed = true;
            _savableInventory.EndSprinting();

            if (_aiGroupManagable._isAggroEmpty)
                _commentHandler.ResumeAcceptingComment();
        }

        public void SprintingEndSetIsSprintingFalse()
        {
            _isSprinting = false;
            _isSpritingEndAnimPlayed = false;
            _isSprintingChangeVelocity = false;

            anim.SetBool(p_IsRunning_hash, false);
        }
        #endregion

        #region Running.
        public void SetIsRunningStatusToTrue()
        {
            _isRunning = true;
            anim.SetBool(p_IsRunning_hash, true);

            _isWaitForJumpInput = true;
            jumpInputWaitTimer = jumpInputWaitRate;

            statsHandler.isPausingStaminaRecover = true;

            _currentLocoIKState.OnRunningIK(this);
        }

        #region Set Is Running Status To False.
        /// This is called on Update. It set is running to false because player isn't pressing run input anymore.
        public void SetIsRunningStatusToFalse_NoRunInput()
        {
            _isRunning = false;
            anim.SetBool(p_IsRunning_hash, false);

            _startJumpInputWaitCountDown = true;

            statsHandler.isPausingStaminaRecover = false;
            
            _currentLocoIKState.OffRunningIK(this);
        }

        /// This is called on Spriting. When player choose to sprint when running.
        public void SetIsRunningStatusToFalse_OnSprint()
        {
            //* Note: No _isRunning = false in here, it will be set later when check leftHandWeaponIsNullOrNot.
            //* Note: No 'anim.SetBool(p_IsRunning_hash, false)' in here.

            _isWaitForJumpInput = false;
            
            _currentLocoIKState.OffRunningIK(this);
        }

        /// This is called on Attack Actions. Check is player running currently when attack action executed.
        public void OnAttackActionCheckIsRunningStatus()
        {
            if (p_run_input)
            {
                _hasReleaseRunButton = false;
                SetIsRunningStatusToFalse_OnAttacks();
            }
        }

        void SetIsRunningStatusToFalse_OnAttacks()
        {
            if (_isRunning)
            {
                _isRunning = false;
                anim.SetBool(p_IsRunning_hash, false);

                _isWaitForJumpInput = false;

                _currentLocoIKState.OffRunningIK(this);
            }
        }
        #endregion

        #endregion

        void SetIsWaitForJumpInputToFalse()
        {
            if (_startJumpInputWaitCountDown)
            {
                jumpInputWaitTimer -= _delta;
                if (jumpInputWaitTimer <= 0)
                {
                    _isWaitForJumpInput = false;
                    _startJumpInputWaitCountDown = false;
                }
            }
        }
        #endregion

        #region Stamina Recover
        public void SetIsWaitStaminaRecoverStatus(bool isWaitStaminaRecover)
        {
            if (isWaitStaminaRecover)
            {
                if (!_isWaitForStaminaRecover)
                {
                    _isWaitForStaminaRecover = true;
                    _mainHudManager.OnStaminaSliderWaitForRecover();
                }
            }
            else
            {
                if (_isWaitForStaminaRecover)
                {
                    _isWaitForStaminaRecover = false;
                    _mainHudManager.OffStaminaSliderWaitForRecover();
                }
            }
        }
        #endregion

        #region Locking on.
        public void SetIsLockingOnStatusToTrue(AIStateManager aiStates)
        {
            SetStatus();

            StartValideTargetCount();

            SwitchTurningTypeInConsumable();

            _currentLocoIKState.OnLockon(this);

            _camHandler.SwitchToLockonRotateManagable();

            _commentHandler.PauseAcceptingComment();

            void SetStatus()
            {
                _lockonState = aiStates;
                _lockonState.SetIsLockonStateStatus(true);
                _lockonBodyBoneTransform = _lockonState.anim.GetBoneTransform(HumanBodyBones.Chest);
                isLockingOn = true;
            }

            void StartValideTargetCount()
            {
                validateTargetTimer = 0;
                SetIsStartCancelLockonWaitToFalse();
            }

            void SwitchTurningTypeInConsumable()
            {
                if (_savableInventory._isUsingConsumable)
                {
                    if (applyTurningWithMoveDir)
                    {
                        applyTurningWithMoveDir = false;
                        applyTurningWithLockonDir = true;
                    }
                }
            }
        }

        public void SetIsLockingOnStatusToFalse()
        {
            SetStatus();

            SwitchTurningTypeInConsumable();

            _currentLocoIKState.OffLockon(this);

            _camHandler.SwitchToFreemodeRotateManagable();

            ResumeAcceptingComment();

            void SetStatus()
            {
                _lockonState.SetIsLockonStateStatus(false);
                _lockonState = null;
                _lockonBodyBoneTransform = null;
                isLockingOn = false;
            }
            
            void SwitchTurningTypeInConsumable()
            {
                if (_savableInventory._isUsingConsumable)
                {
                    if (applyTurningWithLockonDir)
                    {
                        applyTurningWithLockonDir = false;
                        applyTurningWithMoveDir = true;
                    }
                }
            }

            void ResumeAcceptingComment()
            {
                if (_aiGroupManagable._isAggroEmpty && !isDead)
                    _commentHandler.ResumeAcceptingComment();
            }
        }
        
        void SetIsStartCancelLockonWaitToFalse()
        {
            _isStartCancelLockonWait = false;
            _cancelLockonWaitTimer = 0;
        }
        #endregion

        #region Blocking.
        public void SetIsBlockingStatus(bool _isBlocking)
        {
            if (_isBlocking)
            {
                if (!this._isBlocking)
                {
                    _isHoldingLB = true;
                    this._isBlocking = true;

                    anim.SetBool(p_IsBlocking_hash, true);
                    PlayBlockingAnimation();
                    
                    _currentLocoIKState.OnDefense(this);
                    StartBlockingTrailFx();

                    void StartBlockingTrailFx()
                    {
                        if (_isTwoHanding)
                        {
                            _savableInventory._twoHandingWeapon._trailFxHandler.Play_Block_TrailFx();
                        }
                        else
                        {
                            _savableInventory._leftHandWeapon._trailFxHandler.Play_Block_TrailFx();
                        }
                    }
                }
            }
            else
            {
                _isHoldingLB = false;
                this._isBlocking = false;
                anim.SetBool(p_IsBlocking_hash, false);

                _currentLocoIKState.OffDefense(this);

                StopBlockingTrailFx();

                void StopBlockingTrailFx()
                {
                    a_hook.currentTrailFxHandler.Play_NullState_TrailFx();
                }
            }
        }
        #endregion

        #region Control By Agent / Agent Interaction.
        void SetIsControlByAgentToFalse()
        {
            _isControlByAgent = false;
            StopAgentMoving();
        }

        void SetIsControlByAgentToTrue()
        {
            _isControlByAgent = true;
            p_rb.isKinematic = true;
        }

        void SetIsAllowAgentInteractionToTrue()
        {
            _isAllowAgentInteraction = true;
        }

        public void SetIsAllowAgentInteractionToFalse()
        {
            _isAllowAgentInteraction = false;
        }
        #endregion

        #region Consumable Unarmed Locomotion.
        public void SetUseUnarmedLocoInConsumableToTrue()
        {
            _UseUnarmedLocoInConsumable = true;
            anim.CrossFade(_savableInventory.runtimeUnarmed._referedWeaponItem.GetRhLocomotionHashByType(), 0.2f);
        }

        public void SetUseUnarmedLocoInConsumableToFalse()
        {
            _UseUnarmedLocoInConsumable = false;

            if (_isTwoHanding)
            {
                anim.CrossFade(_savableInventory._twoHandingWeapon_referedItem.GetThLocomotionHashByType(), 0.2f);
            }
            else
            {
                anim.CrossFade(_savableInventory._rightHandWeapon_referedItem.GetRhLocomotionHashByType(), 0.2f);
            }
        }
        #endregion

        #region Trigger Fully Held.
        public void SetIsTriggerFullyHeldToTrue()
        {
            _hasHoldAtkReachedMaximum = true;
            anim.SetBool(p_IsTriggerFullyHeld_hash, true);
        }

        public void SetIsTriggerFullyHeldToFalse()
        {
            _hasHoldAtkReachedMaximum = false;
            anim.SetBool(p_IsTriggerFullyHeld_hash, false);
        }
        #endregion

        #region Execution Card Shown.
        public void ShowExecutionCard()
        {
            if (!_isExecutionCardShown)
            {
                _mainHudManager.ShowExecutionCard();
                _isExecutionCardShown = true;
            }
        }

        public void HideExecutionCard_MoveOut()
        {
            if (_isExecutionCardShown)
            {
                _mainHudManager.HideExecutionCard_MoveOut();
                _isExecutionCardShown = false;
            }
        }

        void HideExecutionCard_FadeOut()
        {
            if (_isExecutionCardShown)
            {
                _mainHudManager.HideExecutionCard_FadeOut();
                _isExecutionCardShown = false;
            }
        }
        #endregion

        #endregion

        #region Is Grounded.
        public void CheckIsGrounded()
        {
            if (skipGroundCheck)
            {
                if (knockedDownSkipGroundCheck)
                    return;

                skipGroundCheckTimer += _delta;
                if (skipGroundCheckTimer > jumpSkipGroundCheckRate)
                {
                    skipGroundCheckTimer = 0f;
                    skipGroundCheck = false;
                }
            }
            else
            {
                isGroundedCheckTimer += _delta;
                if (isGroundedCheckTimer > isGroundedCheckRate)
                {
                    isGroundedCheckTimer = 0;
                    GetOnGroundHeight();
                    MonitorIsGroundedStatus();
                }
            }
        }

        void MonitorIsGroundedStatus()
        {
            if (OnGroundHeight >= offGroundDistance)
            {
                if (OnGroundHeight >= startFallingDistance)
                {
                    OffGroundFalling();
                }
                else
                {
                    OffGround();
                }
            }
            else
            {
                OnGround();
            }
        }

        void OnGround()
        {
            if (!isGrounded)
            {
                Land();
            }
        }

        void OffGround()
        {
            if (isGrounded)
            {
                //Debug.Log("OffGround.");
                isGrounded = false;
                anim.SetBool(p_IsGrounded_hash, false);
                //p_rb.isKinematic = false;

                canMoveWhileNeglect = true;
                applyTurningWithMoveDir = true;
            }
        }

        void OffGroundFalling()
        {
            if (!isOffGroundFalling)
            {
                //Debug.Log("OffGroundFalling.");
                isGrounded = false;
                isOffGroundFalling = true;
                anim.SetBool(p_IsGrounded_hash, false);

                canMoveWhileNeglect = true;
                OffGroundPoint = OnGroundHeight;

                if (_savableInventory._isBothUnarmed)
                {
                    CrossFadeAnimWithMoveDir(p_unarmed_fall_start_hash, true, true);
                }
                else
                {
                    //Debug.Log("OffGroundFalling");
                    CrossFadeAnimWithMoveDir(p_armed_fall_start_hash, true, true);
                }
            }
        }
        
        void Land()
        {
            // Play IsGrounded Set to true will play fall start anim.
            anim.SetBool(p_IsGrounded_hash, true);
            canMoveWhileNeglect = false;
            TakeDamageFromFall();

            if (!isOffGroundFalling)
                isGrounded = true;

            isOffGroundFalling = false;
        }

        void OnLand()
        {
            if (_isJumping)
            {
                _isJumping = false;
            }
            
            isGrounded = true;
        }

        void GetOnGroundHeight()
        {
            Vector3 origin = mTransform.position;
            origin.y += .7f;

            //Debug.DrawRay(origin, -vector3Up * sphereCastDistance);
            RaycastHit hit;
            if (Physics.SphereCast(origin, sphereCastRadius, -vector3Up, out hit, sphereCastDistance, _layerManager._playerGroundCheckMask))
            {
                //Debug.Log("mTransform.position.y = " + mTransform.position.y);
                //Debug.Log("hit.point.y = " + hit.point.y);
                _hitPoint = hit.point;
                OnGroundHeight = mTransform.position.y - hit.point.y;
            }
        }

        public void GetOffGroundPointWhenJumping()
        {
            GetOnGroundHeight();
            OffGroundPoint = OnGroundHeight;
        }

        void TakeDamageFromFall()
        {
            if (OffGroundPoint >= fallingSafeHeight)
            {
                if (OffGroundPoint >= fallingToDeathHeight)
                {
                    isDead = true;
                }
                else
                {
                    SetDamageTakenEnumByFalling();

                    CalculateFallingDamage();

                    SpawnFallOffBloodFx();

                    _mainHudManager.ShowDamagedScreen_RegisterDamagePreviwer();
                }
            }

            OffGroundPoint = 0;

            if (isDead)
            {
                KillPlayer();
            }
        }

        void CalculateFallingDamage()
        {
            float t = OffGroundPoint / maxFallDamageHeight;
            _previousGetHitDamage = maxFallDamage * t;

            statsHandler.DecrementPlayerHealth();
        }
        #endregion

        #region Locking On Validate Check.
        public void ValidateCheckLockonTarget()
        {
            if (isLockingOn)
            {
                if (_isStartCancelLockonWait)
                {
                    _cancelLockonWaitTimer += _delta;
                    if (_cancelLockonWaitTimer > _cancelLockonWaitRate)
                    {
                        /// Check again for wall.
                        if (WallsInBetweenTarget(_lockonState))
                        {
                            SetIsLockingOnStatusToFalse();
                        }

                        SetIsStartCancelLockonWaitToFalse();
                    }
                }
                else
                {
                    validateTargetTimer += _delta;
                    if (validateTargetTimer > validateTargetRate)
                    {
                        validateTargetTimer = 0;
                        ValidateCurrentLockonStatesByDistance();
                    }
                }
            }

            void ValidateCurrentLockonStatesByDistance()
            {
                if (Vector3.SqrMagnitude(_dirToLockonStates) < lockOnValidDistance)
                {
                    if (WallsInBetweenTarget(_lockonState))
                    {
                        _isStartCancelLockonWait = true;
                    }
                }
                else
                {
                    _isStartCancelLockonWait = true;
                }
            }
        }

        public bool WallsInBetweenTarget(AIStateManager _potentialLockonAI)
        {
            Vector3 origin = mTransform.position;
            origin.y += .7f;

            Vector3 _raycastEnd = _potentialLockonAI.mTransform.position;
            _raycastEnd.y += .7f;
            
            Vector3 _raycastDir = _raycastEnd - origin;

            if (Physics.Raycast(origin, _raycastDir, Vector3.Magnitude(_raycastDir), _layerManager._lockonObstaclesMask))
            {
                return true;
            }

            return false;
        }
        
        public bool WallsInBetweenLockableCollider_Multiple_OutDistance(int _index, out float _raycastDistance)
        {
            Vector3 origin = mTransform.position;
            origin.y += .7f;

            Vector3 _raycastEnd = lockableHitColliders[_index].transform.position;
            _raycastEnd.y += .7f;

            Vector3 _raycastDir = _raycastEnd - origin;
            _raycastDistance = Vector3.Magnitude(_raycastDir);

            if (Physics.Raycast(origin, _raycastDir, _raycastDistance, _layerManager._lockonObstaclesMask))
            {
                return true;
            }

            return false;
        }

        public bool WallsInBetweenLockableCollider_Multiple(int _index)
        {
            Vector3 origin = mTransform.position;
            origin.y += .7f;

            Vector3 _raycastEnd = lockableHitColliders[_index].transform.position;
            _raycastEnd.y += .7f;
            
            Vector3 _raycastDir = _raycastEnd - origin;

            if (Physics.Raycast(origin, _raycastDir, Vector3.Magnitude(_raycastDir), _layerManager._lockonObstaclesMask))
            {
                return true;
            }

            return false;
        }

        public bool WallsInBetweenLockableCollider_Single()
        {
            Vector3 origin = mTransform.position;
            origin.y += .7f;

            Vector3 _raycastEnd = lockableHitColliders[0].transform.position;
            _raycastEnd.y += .7f;

            Vector3 _raycastDir = _raycastEnd - origin;

            if (Physics.Raycast(origin, _raycastDir, Vector3.Magnitude(_raycastDir), _layerManager._lockonObstaclesMask))
            {
                return true;
            }

            return false;
        }

        public int GetTargetWithinLockableSphere()
        {
            return Physics.OverlapSphereNonAlloc(mTransform.position, lockOnValidDistance, lockableHitColliders, _layerManager._lockonTargetMask);
        }
        #endregion

        #region Set Public Damage Colliders.

        #region Elbow.
        public void SetRightElbowDmgColliderToTrue(RuntimeWeapon _runtimeWeapon)
        {
            r_fist_dmgCollider._runtimeWeapon = _runtimeWeapon;
            r_fist_dmgCollider.SetColliderStatusToTrue();
        }

        public void SetRightElbowDmgColliderToFalse()
        {
            r_fist_dmgCollider._collider.enabled = false;
        }

        public void SetLeftElbowDmgColliderToTrue(RuntimeWeapon _runtimeWeapon)
        {
            l_fist_dmgCollider._runtimeWeapon = _runtimeWeapon;
            l_fist_dmgCollider.SetColliderStatusToTrue();
        }

        public void SetLeftElbowDmgColliderToFalse()
        {
            l_fist_dmgCollider._collider.enabled = false;
        }
        #endregion

        #region Lower Legs.
        public void SetRightLegDmgColliderToTrue(RuntimeWeapon _runtimeWeapon)
        {
            r_lower_leg_dmgCollider._runtimeWeapon = _runtimeWeapon;
            r_lower_leg_dmgCollider.SetColliderStatusToTrue();
        }

        public void SetRightLegDmgColliderToFalse()
        {
            r_lower_leg_dmgCollider._collider.enabled = false;
        }

        public void SetLeftLegDmgColliderToTrue(RuntimeWeapon _runtimeWeapon)
        {
            l_lower_leg_dmgCollider._runtimeWeapon = _runtimeWeapon;
            l_lower_leg_dmgCollider.SetColliderStatusToTrue();
        }

        public void SetLeftLegDmgColliderToFalse()
        {
            l_lower_leg_dmgCollider._collider.enabled = false;
        }
        #endregion

        public void OnDeathTurnOffPublieColliders()
        {
            r_fist_dmgCollider._collider.enabled = false;
            l_fist_dmgCollider._collider.enabled = false;

            r_lower_leg_dmgCollider._collider.enabled = false;
            l_lower_leg_dmgCollider._collider.enabled = false;
        }

        #endregion

        #region On Hit.
        public void OnDamageColliderHit(BaseEnemyDamageCollider _damageCollider)
        {
            isInvincible = true;
            isOnHit = true;
            
            DamageCollider_GetCurrentHitRefs();
            SetDamageTakenEnumByPhysicalAttackType();

            DamageCollider_GetFinalEnemyDamage();
            DamageCollider_CalculateHitAngle();

            GetHitSourceDirectionType();
            FindClosestStickableTransform();

            ValidatePlayerBlocking();
            if (_isBlockingValidate)
            {
                DamageBlockingPlayer();
            }
            else
            {
                DamageNoBlockingPlayer();
            }

            RegisterDamagePreviewer();

            DamageCollider_ResetGetHitRefs();

            void DamageCollider_GetCurrentHitRefs()
            {
                _hitSourceCollider = _damageCollider._collider;

                _hitSourceAI = _damageCollider._ai;
                _hitSourceAttackRefs = _hitSourceAI.currentAttackRefs;
            }

            void DamageCollider_GetFinalEnemyDamage()
            {
                _previousGetHitDamage = _hitSourceAttackRefs._attackBaseDamage;
                _previousGetHitDamage -= GetDamageReductionFromElemental();
                _previousGetHitDamage -= GetDamageReductionFromPhysicalType();
            }

            void DamageCollider_CalculateHitAngle()
            {
                Vector3 dirToHitSource = vector3Zero;

                switch (_hitSourceAttackRefs._attackType)
                {
                    case AI_AttackRefs.AIAttackTypeEnum.Melee:
                        /// Direction from AIManager position to player position.
                        dirToHitSource = _hitSourceAI.mTransform.position - mTransform.position;
                        break;

                    case AI_AttackRefs.AIAttackTypeEnum.Projectile:
                        /// Direction from Projectile's position to player position.
                        dirToHitSource = _hitSourceCollider.transform.position - mTransform.position;
                        break;
                }

                dirToHitSource.y = 0;
                _hitSourceAngle = Vector3.Angle(dirToHitSource, mTransform.forward);
                _hitSourceAngle_noDirCheck = _hitSourceAngle;
                _hitSourceAngle = Vector3.Dot(dirToHitSource, mTransform.right) > 0 ? _hitSourceAngle : -_hitSourceAngle;
                //Debug.Log("_getHitAngle = " + _hitSourceAngle);
            }
            
            void DamageCollider_ResetGetHitRefs()
            {
                _isBlockingValidate = false;
                isOnHit = false;
            }
        }

        public void OnOverlapSphereHit(AI_AreaDamageParticle_Base _areaDamageParticle)
        {
            isInvincible = true;
            isOnHit = true;
            isUseBfxHandlerYPosBuffer = true;

            Vector3 dirToHitSource;

            OverlapSphere_GetCurrentHitRefs();
            SetDamageTakenEnumByAOE();

            CalculateDirToHitSource();
            OverlapSphere_GetFinalEnemyDamage();
            OverlapSphere_CalculateHitAngle();

            GetHitSourceDirectionType();
            FindClosestStickableTransform();

            ValidatePlayerBlocking();
            if (_isBlockingValidate)
            {
                DamageBlockingPlayer();
            }
            else
            {
                DamageNoBlockingPlayer();
            }

            RegisterDamagePreviewer();

            OverlapSphere_ResetCurrentGetHitRefs();

            void OverlapSphere_GetCurrentHitRefs()
            {
                _hitSourceAOEParticle = _areaDamageParticle;

                _hitSourceAI = _areaDamageParticle.ai;
                _hitSourceAttackRefs = _hitSourceAI.currentAttackRefs;
            }

            void CalculateDirToHitSource()
            {
                dirToHitSource = _hitSourceAOEParticle.transform.position - mTransform.position;
                dirToHitSource.y = 0;
            }

            void OverlapSphere_GetFinalEnemyDamage()
            {
                _previousGetHitDamage = _hitSourceAttackRefs._attackBaseDamage;
                _previousGetHitDamage -= GetDamageReductionFromElemental();

                float _SqrDisToHitSource = Vector3.SqrMagnitude(dirToHitSource);
                if (_SqrDisToHitSource > 6.25f)
                {
                    _previousGetHitDamage = _previousGetHitDamage * 0.7f;
                }
            }

            void OverlapSphere_CalculateHitAngle()
            {
                _hitSourceAngle = Vector3.Angle(dirToHitSource, mTransform.forward);
                _hitSourceAngle_noDirCheck = _hitSourceAngle;
                _hitSourceAngle = Vector3.Dot(dirToHitSource, mTransform.right) > 0 ? _hitSourceAngle : -_hitSourceAngle;
            }
            
            void OverlapSphere_ResetCurrentGetHitRefs()
            {
                isUseBfxHandlerYPosBuffer = false;
                _isBlockingValidate = false;
                isOnHit = false;
            }
        }

        void GetHitSourceDirectionType()
        {
            /// Hit is from player perspective right side.
            if (_hitSourceAngle > 0)
            {
                if (_hitSourceAngle > 135)
                {
                    _damageTakenDirectionType = DamageTakenDirectionTypeEnum.HitFromBack;
                }
                else if (_hitSourceAngle > 45)
                {
                    _damageTakenDirectionType = DamageTakenDirectionTypeEnum.HitFromRight;
                }
                else
                {
                    _damageTakenDirectionType = DamageTakenDirectionTypeEnum.HitFromFront;
                }
            }
            /// Hit is from player perspective left side.
            else
            {
                if (_hitSourceAngle < -135)
                {
                    _damageTakenDirectionType = DamageTakenDirectionTypeEnum.HitFromBack;
                }
                else if (_hitSourceAngle < -45)
                {
                    _damageTakenDirectionType = DamageTakenDirectionTypeEnum.HitFromLeft;
                }
                else
                {
                    _damageTakenDirectionType = DamageTakenDirectionTypeEnum.HitFromFront;
                }
            }
        }

        void FindClosestStickableTransform()
        {
            float closestDis = 10000f;
            Transform[] _stickableArray = null;

            /// If Hit Point is on Upper Side.
            if (_hitPoint.y > mTransform.position.y + upperBodyApproxHeight)
            {
                switch (_damageTakenDirectionType)
                {
                    case DamageTakenDirectionTypeEnum.HitFromLeft:
                        _stickableArray = u_L_stickableTrans;
                        break;
                    case DamageTakenDirectionTypeEnum.HitFromRight:
                        _stickableArray = u_R_stickableTrans;
                        break;
                    case DamageTakenDirectionTypeEnum.HitFromFront:
                        _stickableArray = u_F_stickableTrans;
                        break;
                    case DamageTakenDirectionTypeEnum.HitFromBack:
                        _stickableArray = u_B_stickableTrans;
                        break;
                }
            }
            else
            {
                if (_damageTakenDirectionType == DamageTakenDirectionTypeEnum.HitFromBack)
                {
                    _stickableArray = d_B_stickableTrans;
                }
                else
                {
                    _stickableArray = d_F_stickableTrans;
                }
            }
            
            for (int i = 0; i < _stickableArray.Length; i++)
            {
                float dist = Vector3.SqrMagnitude(_stickableArray[i].position - _hitPoint);
                if (dist < closestDis)
                {
                    closestDis = dist;
                    closestStickableTrans = _stickableArray[i];
                }
            }
        }

        void ValidatePlayerBlocking()
        {
            if (!_isBlocking)
            {
                _isBlockingValidate = false;
            }
            else
            {
                
                if (_hitSourceAngle_noDirCheck <= validateBlockingThershold)
                {
                    _isBlockingValidate = true;
                }
                else
                {
                    _isBlockingValidate = false;
                }
            }
        }

        #region Get Damage Reductions.
        float GetDamageReductionFromElemental()
        {
            /// Play can get 50% damage reduction from enemy attacks in highest reduction stats.
            switch (_hitSourceAI.currentElementalType)
            {
                case AIElementalTypeEnum.Physical:
                    return _hitSourceAttackRefs._attackBaseDamage * 0.5f * (statsHandler.b_physical_reduction / 100);
                case AIElementalTypeEnum.Magical:
                    return _hitSourceAttackRefs._attackBaseDamage * 0.5f * (statsHandler.b_magic_reduction / 100);
                case AIElementalTypeEnum.Fire:
                    return _hitSourceAttackRefs._attackBaseDamage * 0.5f * (statsHandler.b_fire_reduction / 100);
                case AIElementalTypeEnum.Lightning:
                    return _hitSourceAttackRefs._attackBaseDamage * 0.5f * (statsHandler.b_lightning_reduction / 100);
                case AIElementalTypeEnum.Dark:
                    return _hitSourceAttackRefs._attackBaseDamage * 0.5f * (statsHandler.b_dark_reduction / 100);
                default:
                    return 0;
            }
        }

        float GetDamageReductionFromPhysicalType()
        {
            /// Play can get 30% damage reduction from enemy attacks in highest reduction stats.
            switch (_damageTakenPhysicalType)
            {
                case DamageTakenPhysicalTypeEnum.Strike:
                    return _hitSourceAttackRefs._attackBaseDamage * 0.3f * (statsHandler.b_strike_reduction / 100);
                case DamageTakenPhysicalTypeEnum.Slash:
                    return _hitSourceAttackRefs._attackBaseDamage * 0.3f * (statsHandler.b_slash_reduction / 100);
                case DamageTakenPhysicalTypeEnum.Thrust:
                    return _hitSourceAttackRefs._attackBaseDamage * 0.3f * (statsHandler.b_thrust_reduction / 100);
                default:
                    return 0;
            }
        }
        #endregion

        #region Damage Blocking Player.
        void DamageBlockingPlayer()
        {
            DepleteHealthWithBlocking();
            CancelBlockingIfBlockBroken();
            SpawnBlockingOnHitEffect();

            if (!isDead)
            {
                PlayBlockingOnHitAnimation();
            }
            else
            {
                KillPlayer();
            }
        }

        void DepleteHealthWithBlocking()
        {
            WeaponItem _cur_BlockingWeapon = _isTwoHanding ? _savableInventory._twoHandingWeapon_referedItem : _savableInventory._leftHandWeapon_referedItem;

            /// Deplete stamina has to come before deplete health, beacuse deplete health method will override previousHitDamage.
            statsHandler.DecrementPlayerStaminaWhenBlocking(CalculateStaminaUsageFromBlockingWeapon());

            CalculateGuardAbsorpFromBlockingWeapon();
            statsHandler.DecrementPlayerHealth();

            float CalculateStaminaUsageFromBlockingWeapon()
            {
                return _previousGetHitDamage * (1 - _cur_BlockingWeapon.stability * 0.01f);
            }

            void CalculateGuardAbsorpFromBlockingWeapon()
            {
                _previousGetHitDamage = _previousGetHitDamage * (1 - _cur_BlockingWeapon._guardAbsorpValue * 0.01f);
            }
        }

        void CancelBlockingIfBlockBroken()
        {
            if (_hasBlockingBroken)
                SetIsBlockingStatus(false);
        }

        void SpawnBlockingOnHitEffect()
        {
            if (_hasBlockingBroken)
            {
                SpawnTargetBloodFx();
            }
            else
            {
                Spawn_Block_ImpactEffect();
            }
        }

        void Spawn_Block_ImpactEffect()
        {
            WorldImpactEffect _effect = _gameManager._blockImpactEffect;

            _effect.mTransform.parent = null;

            if (_isTwoHanding)
            {
                _effect.mTransform.position = _savableInventory._twoHandingWeapon.transform.position;
            }
            else
            {
                _effect.mTransform.position = _savableInventory._leftHandWeapon.transform.position;
            }
            
            _effect.mTransform.eulerAngles = mTransform.eulerAngles;
            _effect.BlockingHit_SpawnEffect();
        }

        void PlayBlockingOnHitAnimation()
        {
            if (_hasBlockingBroken)
            {
                _damageTakenDirectionType = DamageTakenDirectionTypeEnum.HitFromFront;
                applyHitImpactMotion = true;

                _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);
                PlayBlockingBreakAnimation();
            }
            else
            {
                PlayBlockingReactAnimation();
            }
        }
        #endregion

        #region Damage No Blocking Player.
        void DamageNoBlockingPlayer()
        {
            DepleteHealthWithoutBlock();
            SpawnOnHitEffect();
            _mainHudManager.ShowDamagedScreen();

            if (_isAttackCharging)
            {
                OnHit_SetIsAttackChargingToFalse();
                ChargingAttack_PlayerOnHitAnimation();
            }
            else
            {
                if (!isDead)
                {
                    Regular_PlayOnHitAnimation();
                }
                else
                {
                    KillPlayer();
                }
            }
        }

        void DepleteHealthWithoutBlock()
        {
            statsHandler.DecrementPlayerHealth();
        }

        void SpawnOnHitEffect()
        {
            SpawnTargetBloodFx();
        }

        void Regular_PlayOnHitAnimation()
        {
            if (isNeglectingInput)
                return;

            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);
            switch (_hitSourceAttackRefs._attackImpactType)
            {
                case AI_AttackRefs.AIAttackImpactTypeEnum.Normal:

                    #region Play Small Hit Effect.
                    switch (_damageTakenDirectionType)
                    {
                        case DamageTakenDirectionTypeEnum.HitFromLeft:
                            PlayHitSmallRightAnimation();
                            break;
                        case DamageTakenDirectionTypeEnum.HitFromRight:
                            PlayHitSmallLeftAnimation();
                            break;
                        case DamageTakenDirectionTypeEnum.HitFromFront:
                            PlayHitSmallFrontAnimation();
                            break;
                        case DamageTakenDirectionTypeEnum.HitFromBack:
                            PlayHitSmallBackAnimation();
                            break;
                    }
                    #endregion
                    break;

                case AI_AttackRefs.AIAttackImpactTypeEnum.Big:

                    #region Play Big Hit Effect.
                    applyHitImpactMotion = true;
                    switch (_damageTakenDirectionType)
                    {
                        case DamageTakenDirectionTypeEnum.HitFromLeft:
                            PlayHitBigRightAnimation();
                            break;
                        case DamageTakenDirectionTypeEnum.HitFromRight:
                            PlayHitBigLeftAnimation();
                            break;
                        case DamageTakenDirectionTypeEnum.HitFromFront:
                            PlayHitBigFrontAnimation();
                            break;
                        case DamageTakenDirectionTypeEnum.HitFromBack:
                            PlayHitBigBackAnimation();
                            break;
                    }
                    #endregion
                    break;

                case AI_AttackRefs.AIAttackImpactTypeEnum.Knockback:

                    #region Play Knock Back Effect.
                    switch (_damageTakenDirectionType)
                    {
                        case DamageTakenDirectionTypeEnum.HitFromLeft:
                        case DamageTakenDirectionTypeEnum.HitFromRight:
                        case DamageTakenDirectionTypeEnum.HitFromFront:
                            OnNormalHitKnockBackSetStatus();
                            PlayKnockbackAnimation();
                            ApplyKnockBackRootMotion();
                            break;

                        case DamageTakenDirectionTypeEnum.HitFromBack:
                            applyHitImpactMotion = true;
                            PlayHitBigBackAnimation();
                            break;
                    }
                    #endregion
                    break;
            }
        }

        void ChargingAttack_PlayerOnHitAnimation()
        {
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);
            switch (_hitSourceAttackRefs._attackImpactType)
            {
                case AI_AttackRefs.AIAttackImpactTypeEnum.Normal:
                case AI_AttackRefs.AIAttackImpactTypeEnum.Big:

                    #region Play Big Hit Effect.
                    applyHitImpactMotion = true;
                    switch (_damageTakenDirectionType)
                    {
                        case DamageTakenDirectionTypeEnum.HitFromLeft:
                            PlayHitBigRightAnimation();
                            break;
                        case DamageTakenDirectionTypeEnum.HitFromRight:
                            PlayHitBigLeftAnimation();
                            break;
                        case DamageTakenDirectionTypeEnum.HitFromFront:
                            PlayHitBigFrontAnimation();
                            break;
                        case DamageTakenDirectionTypeEnum.HitFromBack:
                            PlayHitBigBackAnimation();
                            break;
                    }
                    #endregion
                    break;

                case AI_AttackRefs.AIAttackImpactTypeEnum.Knockback:

                    #region Play Knock Back Effect.
                    switch (_damageTakenDirectionType)
                    {
                        case DamageTakenDirectionTypeEnum.HitFromLeft:
                        case DamageTakenDirectionTypeEnum.HitFromRight:
                        case DamageTakenDirectionTypeEnum.HitFromFront:
                            OnNormalHitKnockBackSetStatus();
                            PlayKnockbackAnimation();
                            ApplyKnockBackRootMotion();
                            break;

                        case DamageTakenDirectionTypeEnum.HitFromBack:
                            applyHitImpactMotion = true;
                            PlayHitBigBackAnimation();
                            break;
                    }
                    #endregion
                    break;
            }
        }

        void OnNormalHitKnockBackSetStatus()
        {
            OnKnockBackSetStatus();
            _hitSourceAI.SetIsKnockedDownPlayerToTrue();
            _isExecutionKnockedBack = false;
            anim.SetBool(p_IsGetupReady_hash, false);
        }
        #endregion

        #region On Execution Hit.

        public void OnExecutionHit(AI_BaseExecution_Profile currentExecutionProfile)
        {
            _currentExecutionProfile = currentExecutionProfile;

            Execution_GetCurrentHitRefs();

            OnExecutionHitSetStatus();
            SetDamageTakenEnumByExecution();

            SetStickableTransformByExecution();
            DamageCaughtExecutionPlayer();

            TweenLookTowardParentPoint();
            TweenMoveTowardParentPoint();

            _mainHudManager.ShowDamagedScreen_RegisterDamagePreviwer();


            void Execution_GetCurrentHitRefs()
            {
                _hitSourceAI = _currentExecutionProfile._executionerAI;
                _hitSourceAttackRefs = _hitSourceAI.currentAttackRefs;
                _executionReceivedYPos = transform.position.y;
            }

            void OnExecutionHitSetStatus()
            {
                OnKnockBackSetStatus();
                _hitSourceAI.SetIsKnockedDownPlayerToTrue();
                _isExecutionKnockedBack = true;
                p_rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            void SetStickableTransformByExecution()
            {
                float closestDis = 10000f;
                
                for (int i = 0; i < u_F_stickableTrans.Length; i++)
                {
                    float dist = Vector3.SqrMagnitude(u_F_stickableTrans[i].position - _hitPoint);
                    if (dist < closestDis)
                    {
                        closestDis = dist;
                        closestStickableTrans = u_F_stickableTrans[i];
                    }
                }
            }

            void DamageCaughtExecutionPlayer()
            {
                DepleteHealthFromExecution_1stHit();
                SpawnOnHitEffect();

                PlayExecutionReceivedAnim();
                anim.applyRootMotion = false;

                void DepleteHealthFromExecution_1stHit()
                {
                    _previousGetHitDamage = _currentExecutionProfile._1st_executionDamage;
                    statsHandler.DecrementPlayerHealth();
                }
            }

            void TweenLookTowardParentPoint()
            {
                LeanTween.rotateLocal(gameObject, _currentExecutionProfile._playerLocalEulers, 0.1f);
            }

            void TweenMoveTowardParentPoint()
            {
                gameObject.transform.parent = _currentExecutionProfile._executionParentPoint;
                LeanTween.moveLocal(gameObject, vector3Zero, 0.1f);
            }
        }

        #region Egil Anim Hooks.
        public void KnockBackFromExecution()
        {
            _currentExecutionProfile.KnockBackFromExecution(p_rb);
        }
        
        public void DepleteHealthFromExecution_2ndHit()
        {
            _previousGetHitDamage = _currentExecutionProfile._2nd_executionDamage;
            statsHandler.DecrementPlayerHealth();

            SpawnOnHitEffect();

            _mainHudManager.ShowDamagedScreen_RegisterDamagePreviwer();
        }
        #endregion

        #endregion

        #region KnockBack Anim Hooks.
        public void OnKnockBackResetRotation()
        {
            Vector3 _targetDir = transform.eulerAngles;
            _targetDir.x = 0;
            _targetDir.z = 0;
            LeanTween.rotate(gameObject, _targetDir, 0.15f).setEaseOutSine();
        }

        public void KnockBackStartGetupCounter()
        {
            if (isDead)
            {
                _inp.OnDeathHideMainHud();
            }

            _startGetupCounting = true;
        }
        #endregion

        #region Kill Player When isDead.
        void KillPlayer()
        {
            OnDeathResetBools();
            PlayDeathAnimation();
            OnDeathPauseIK();
        }

        void KillPlayer_KnockedDown()
        {
            OnDeathResetBools();
            OnDeathPauseIK();
        }
        
        void OnDeathPauseIK()
        {
            _playerIKHandler.HandleOnDeathIK();
        }
        #endregion

        #endregion

        #region Spells.
        public void ThrowSpellProjectile()
        {
            GameObject spellProjectile = _savableInventory._spell.spellProjectile;

            if (spellProjectile == null)
                return;

            GameObject go = Instantiate(spellProjectile) as GameObject;
            go.transform.position = mTransform.position + (vector3Up * 1.5f) + mTransform.forward;
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.velocity = mTransform.forward * 5;
        }

        public void CreateSpellParticle()
        {
        }
        #endregion

        #region Skinned Mesh Combiner.
        public void CombineSkinnedMesh()
        {
            if (isMeshCombinedNeeded)
            {
                reCombineWaitTimer += _delta;
                if (reCombineWaitTimer >= reCombineWaitRate)
                {
                    reCombineWaitTimer = 0;
                    skinnedMeshCombiner.CombineMeshes();
                    isMeshCombined = true;
                    isMeshCombinedNeeded = false;
                }
            }
        }

        public void UnCombineSkinnedMesh()
        {
            if (isMeshCombined)
            {
                skinnedMeshCombiner.UndoCombineMeshes(false, false);
                isMeshCombined = false;
                isMeshCombinedNeeded = true;
            }

            reCombineWaitTimer = 0;
        }

        /// Merge Event.
        public void OnSkinnedMeshCombined()
        {
            skinnedMeshCombiner.resultMergeGameObject.layer = _layerManager.playerSkinnedMeshLayer;
        }
        #endregion

        #region Monitor Interaction.
        public void AddToFoundInteractables(int i)
        {
            foundInteractables.Add(interactHitColliders[i].GetComponent<PlayerInteractable>());
            _foundInteractablesAmount++;
        }

        public void ClearFoundInteractables()
        {
            foundInteractables.Clear();
            _foundInteractablesAmount = 0;
        }

        public void HandleFoundInteractables()
        {
            if (_foundInteractablesAmount > 0)
            {
                //MonitorInputSwitchInteractables();
                SetCurrentInteractable();
                MonitorInputSelectInteractable();
            }
        }

        public void OnInteractingClearFoundInteractables()
        {
            ClearFoundInteractables();
            SetCurrentInteractableToNull();
        }

        void MonitorInputSwitchInteractables()
        {
            if (_foundInteractablesAmount > 1)
            {
                if (p_interaction_switch)
                {
                    _int_Index++;
                    _int_Index = (_int_Index == _foundInteractablesAmount) ? 0 : _int_Index;
                }
            }
        }

        void MonitorInputSelectInteractable()
        {
            if (p_interaction_select)
            {
                if (_currentInteractable != null)
                {
                    _currentInteractable.OnInteract();
                }
            }
        }

        #region Set Current Interactable.
        void SetCurrentInteractable()
        {
            _currentInteractable = foundInteractables[_int_Index];
            _currentInteractable.SetInteractionContent();
        }
        
        public void SetCurrentInteractableToNull()
        {
            _int_Index = 0;
            _currentInteractable = null;
        }

        public void SetPotentialInteractableToNull()
        {
            if (_currentInteractable)
            {
                _int_Index = 0;
                _currentInteractable = null;
                _mainHudManager.HideInteractionCard_MoveOut();
            }
        }
        #endregion
        
        #endregion

        #region Calculate Dir To Destination.
        public void CalculateDirDisToDestination()
        {
            _dirToAgentDestination = _currentAgentDestination.position - mTransform.position;
            _dirToAgentDestination.y = 0;
            _disToAgentDestinationSqr = _dirToAgentDestination.sqrMagnitude;
            //Debug.Log("_disToAgentDestinationSqr = " + _disToAgentDestinationSqr);
        }
        #endregion

        #region Turn With Agent.
        public void TurnWithAgent()
        {
            if (_isPauseControlByAgent)
                return;

            if (_disToAgentDestinationSqr <= _executeAgentInteractionDisSqr)
            {
                TurnWhenStopping();
            }
            else
            {
                TurnWhenManeuvering();
            }
        }

        void TurnWhenStopping()
        {
            Vector3 _lookAtDir = _currentAgentDestination.forward;
            _lookAtDir.y = 0;

            //Debug.Log("TurnWhenStopping");
            
            float angle = Vector3.SignedAngle(mTransform.forward, _lookAtDir, vector3Up);
            float rot = _agentManeuverTurningSpeed * _delta;
            rot = Mathf.Min(Mathf.Abs(angle), rot);
            mTransform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
        }

        void TurnWhenManeuvering()
        {
            float _dot = Vector3.Dot(mTransform.forward, _currentAgentDestination.forward);
            //Debug.Log("_dot = " + _dot);
            if (_dot > 0.35f)
            {
                ManeuveringLookAtDestination();
            }
            else
            {
                ManeuveringLookAtRoute();
            }
        }

        void ManeuveringLookAtRoute()
        {
            Vector3 _curVelocityDir = _agent.velocity;
            _curVelocityDir.y = 0;

            float angle = Vector3.SignedAngle(mTransform.forward, _curVelocityDir, vector3Up);
            float rot = _agentManeuverTurningSpeed * _delta;
            rot = Mathf.Min(Mathf.Abs(angle), rot);
            mTransform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
        }

        void ManeuveringLookAtDestination()
        {
            float angle = Vector3.SignedAngle(mTransform.forward, _dirToAgentDestination, vector3Up);
            float rot = _agentManeuverTurningSpeed * _delta;
            rot = Mathf.Min(Mathf.Abs(angle), rot);
            mTransform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
        }
        #endregion

        #region Anim With Agent.
        public void AnimWithAgent()
        {
            if (_isPauseControlByAgent)
                return;

            if (_disToAgentDestinationSqr <= _executeAgentInteractionDisSqr)
            {
                StopAgentMoveLocomotion();
            }
            else
            {
                UpdateAgentMoveLocomotion();
            }
        }

        void UpdateAgentMoveLocomotion()
        {
            float agentVel = Mathf.Clamp01(_agent.velocity.magnitude / _agent.speed);

            anim.SetFloat(vertical_hash, agentVel * _agentManeuverLocoAnimVal, 0.1f, _delta);
            anim.SetFloat(horizontal_hash, 0, 0.1f, _delta);
        }

        void StopAgentMoveLocomotion()
        {
            anim.SetFloat(vertical_hash, 0, 0.1f, _delta);
            anim.SetFloat(horizontal_hash, 0, 0.1f, _delta);
        }
        #endregion

        #region Move With Agent.
        public void SetNextAgentDestination(Transform _agentDestination)
        {
            _currentAgentDestination = _agentDestination;
            SetIsControlByAgentToTrue();

            _agent.enabled = true;
            _agent.isStopped = false;
            _agent.SetDestination(_currentAgentDestination.position);
        }

        public void StopAgentMoving()
        {
            _agent.ResetPath();
            _agent.path.ClearCorners();
            _agent.isStopped = true;
            _agent.enabled = false;
        }

        public void MoveWithAgent()
        {
            if (_isPauseControlByAgent)
                return;

            UpdateAgentSpeed();
            UpdateAgentMovePosition();
        }

        void UpdateAgentSpeed()
        {
            float t = anim.GetFloat(vertical_hash) / _agentManeuverLocoAnimVal;
            _agent.acceleration = _agentMoveAccel * t;
            _agent.speed = _agentMoveSpeed * t;
        }

        void UpdateAgentMovePosition()
        {
            float proportionalDistance = _agent.acceleration / _agentMoveAccel;
            mTransform.position = Vector3.Lerp(mTransform.position, _agent.nextPosition, proportionalDistance);
        }
        #endregion

        #region Regain Control From Agent.
        public bool RegainControlBackToIdleTransition()
        {
            if (!_isControlByAgent)
            {
                if (!_isAllowAgentInteraction && _currentNeglectInputAction == null)
                {
                    RegainControlFromAgentResetBools();
                    return true;
                }
            }

            return false;
        }

        public void UpdateControlByAgentStateBools()
        {
            /// If player wants to exit ControlByAgent state with move amount or roll, or get attacked.
            if (moveAmount >= 1 || _currentNeglectInputAction != null || isOnHit)
            {
                #region Get Attacked By Enemy.
                if (isOnHit)
                {
                    if (_isPauseControlByAgent)
                    {
                        _isPauseControlByAgent = false;
                        _savableInventory.CheckIfBothCurrentWeaponIsHidden();
                    }
                }
                #endregion
                
                SetIsAllowAgentInteractionToFalse();
                SetIsControlByAgentToFalse();
                SetCurrentInteractableToNull();

                _inp.SetIsInMainHudStatus(true);
            }
            /// If player is close enough to agent destination, play animation and pause agent.
            else if (!_isPauseControlByAgent && IsAbleToStartAgentInteraction())
            {
                _isPauseControlByAgent = true;

                // Anims.
                CrossFadeAnimWithMoveDir(p_int_bonfire_start_hash, false, false);
                Set_IsInBonfireEnd_AnimParaToFalse();

                // Pause Tick & Inputs.
                PauseLocoIKStateTick();
                PausePlayerInput_CampStart();
                _camHandler.PausePlayerInput_CampStart();

                // Spawn Point.
                RefreshPlayerCurrentSpawnPoint();
            }

            bool IsAbleToStartAgentInteraction()
            {
                // Check Distance.
                if (_disToAgentDestinationSqr <= _executeAgentInteractionDisSqr)
                {
                    // Check Angle.
                    if (Vector3.Dot(_currentAgentDestination.forward, mTransform.forward) >= 0.99f)
                    {
                        // Check Anim.
                        if (anim.GetFloat(vertical_hash) <= 0.1f)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            void RefreshPlayerCurrentSpawnPoint()
            {
                _currentSpawnPoint = _currentWalkTowardRestInteractable._linkedSpawnPoint;
            }
        }
        #endregion

        #region Checkpoint Agent Interaction.
        public void OnInteractWithRestInteractable()
        {
            /// Set Rest Point Info.
            SetNextAgentDestination(_currentWalkTowardRestInteractable.bonfireSeatTrans);
            _currentRestPoint_linkedIgniteInterTrans = _currentWalkTowardRestInteractable.igniteInteractable.transform;

            /// Interaction Status.
            _isPausingSearchInteractables = true;
            ClearFoundInteractables();
            _mainHudManager.HideInteractionCard_FadeOut();

            /// Input Manager.
            _inp.SetIsNeglectSelectionMenuToTrue();
        }

        public void OnCheckpointAgentInteraction()
        {
            if (_isAllowAgentInteraction)
                return;

            SetIsAllowAgentInteractionToTrue();
            SetIsControlByAgentToFalse();

            SpawnBonfireRestImpactEffect();
            SetCurrentInteractableToNull();
            
            CheckpointRefreshEvent.Invoke();

            _commentHandler.OnCheckpointAgentInteraction();

            void SpawnBonfireRestImpactEffect()
            {
                _gameManager._bonfireRestImpactEffect.SpawnInParent(_currentRestPoint_linkedIgniteInterTrans);
            }
        }

        public void OffCheckpointAgentInteraction()
        {
            Set_IsInBonfireEnd_AnimParaToTrue();

            RequestSerialization_SaveFromCurrentSaveFile();

            _commentHandler.ResetCommentTextToOriginalPosition();
        }
        #endregion

        #region Snap To Positions.
        public void SnapToBonfireSeatPosition()
        {
            Vector3 _snapPos = _currentAgentDestination.position;
            _snapPos.y = mTransform.position.y;
            LeanTween.move(gameObject, _snapPos, 1).setEaseLinear();

            float _angle = Vector3.SignedAngle(mTransform.forward, _currentRestPoint_linkedIgniteInterTrans.position - mTransform.position, vector3Up);
            LeanTween.rotateAroundLocal(gameObject, vector3Up, _angle, 0.5f).setEaseLinear();
        }

        public void SnapToLevelupPosition()
        {
            Vector3 _snapPos = (_currentAgentDestination.position + (_currentAgentDestination.forward * 0.5f));
            _snapPos.y = mTransform.position.y;
            LeanTween.move(gameObject, _snapPos, 1.3f).setEase(LeanTweenType.easeOutQuart);
        }

        public void SnapToExitBonfirePosition()
        {
            Vector3 _snapPos = (_currentAgentDestination.position + (_currentAgentDestination.forward * 0.5f));
            _snapPos.y = mTransform.position.y;
            LeanTween.move(gameObject, _snapPos, 1).setEase(LeanTweenType.easeOutCirc).setOnComplete(OnCompleteMoveToSnapPosition);

            void OnCompleteMoveToSnapPosition()
            {
                p_rb.isKinematic = false;
                ResumeLocoIKStateTick();
                SetIsAllowAgentInteractionToFalse();
            }
        }
        #endregion

        #region Monitor Foot Steps.
        public void HandlePlayerLocoFootStep()
        {
            if (moveAmount == 1)
            {
                SetIsFootEffectReplayableStatus(true);
            }
            else
            {
                SetIsFootEffectReplayableStatus(false);
            }
        }

        public void HandlePlayerAgentFootStep()
        {
            if (Vector3.SqrMagnitude(mTransform.position - _lastFootEffectPlayedPosition) > _footEffectsReplayDistance)
            {
                PlayFootStepEffect();
            }
        }
        
        void SetIsFootEffectReplayableStatus(bool _isFootEffectReplayable)
        {
            if (_isFootEffectReplayable)
            {
                if (!this._isFootEffectReplayable)
                {
                    this._isFootEffectReplayable = true;
                    PlayFootStepEffect();
                }
                else
                {
                    if (Vector3.SqrMagnitude(mTransform.position - _lastFootEffectPlayedPosition) > _footEffectsReplayDistance)
                    {
                        PlayFootStepEffect();
                    }
                }
            }
            else
            {
                if (this._isFootEffectReplayable)
                {
                    this._isFootEffectReplayable = false;
                    PlayFootStepEffect();
                }
            }
        }

        void PlayFootStepEffect()
        {
            f_hook.ActivateFootStepParticle();
            _lastFootEffectPlayedPosition = mTransform.position;
        }
        #endregion

        #region Parry Execution.
        public void PerformParryExecution(AIManager _ai)
        {
            /// Hide Execution Card.
            HideExecutionCard_FadeOut();

            /// Set States.
            isCantBeDamaged = true;
            _currentExecutingTarget = _ai;
            
            OnPresentParryExecution();

            void OnPresentParryExecution()
            {
                /// Look At Enemy.
                mTransform.rotation = Quaternion.LookRotation(_currentExecutingTarget.mTransform.position - mTransform.position);

                WeaponItem _executionWeaponItem = _isTwoHanding ? _savableInventory._twoHandingWeapon_referedItem : _savableInventory._rightHandWeapon_referedItem;

                /// Play Animation.
                CrossFadeAnimWithMoveDir(_executionWeaponItem.GetParryExecutePresentHashByType(), false, true);
                _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, vector3Zero);

                /// Auto Alert Enemy.
                if (_executionWeaponItem._alertEnemyFromBeginning)
                {
                    AlertEnemyReceiveParryExecution();
                }

                //Debug.Break();
            }
        }

        public void AlertEnemyReceiveParryExecution()
        {
            StoreExecutionInfoToEnemy();
            _currentExecutingTarget.OnReceivedParryExecution();
        }
        
        void StoreExecutionInfoToEnemy()
        {
            RuntimeWeapon _executionWeapon;

            if (_isTwoHanding)
            {
                _executionWeapon = _savableInventory._twoHandingWeapon;
            }
            else
            {
                _executionWeapon = _savableInventory._rightHandWeapon;
            }

            RegisterExecutionInfo();

            void RegisterExecutionInfo()
            {
                _currentExecutingTarget._p_executionProfile = _executionWeapon._referedWeaponItem._executionProfile;
                _currentExecutingTarget._executionReceiveHash = _executionWeapon._referedWeaponItem.GetParryExecuteReceivedHashByType();

                GetHitSourceColliderTransform();
                _currentExecutingTarget._previousExecutionDamage = _executionWeapon.ReturnFinalExecutionPower();

            }

            void GetHitSourceColliderTransform()
            {
                if (_executionWeapon.IsUnarmed())
                {
                    /// This is hard coded for unarm execution present animation.
                    _currentExecutingTarget._hitSourceColliderTransform = r_lower_leg_dmgCollider.transform;
                }
                else
                {
                    _currentExecutingTarget._hitSourceColliderTransform = _executionWeapon.weaponHook.dmgCollider.transform;
                }
            }
        }

        /// Anim Events.
        public void ParentExecuteTargetToWeapon()
        {
            _currentExecutingTarget.mTransform.parent = _savableInventory._rightHandWeapon.transform;
        }

        public void UnParentExecuteTargetFromWeapon()
        {
            Transform _targetTransform = _currentExecutingTarget.mTransform;
            _targetTransform.parent = null;

            Vector3 _targetEuler = _targetTransform.eulerAngles;
            _targetEuler.x = 0;
            _targetEuler.z = 0;
            
            _targetTransform.eulerAngles = _targetEuler;
        }

        public void Switch_HitSourceColliderTransform_To_L_Lower_Leg()
        {
            _currentExecutingTarget._hitSourceColliderTransform = l_lower_leg_dmgCollider.transform;
        }
        #endregion

        #region Knocked Back / Getup.
        void OnKnockBackSetStatus()
        {
            /// Invincible Set Status.
            isInvincible = true;
            isCantBeDamaged = true;
            isOnHit = true;

            /// On Ground Set Status.
            skipGroundCheck = true;
            knockedDownSkipGroundCheck = true;

            /// Weapon Action Loop Effect.
            Stop_HoldATK_Loop_Effect();
        }

        void ApplyKnockBackRootMotion()
        {
            p_rb.mass = 1;
            p_rb.drag = 0;
            p_rb.angularDrag = 0.05f;
            p_rb.useGravity = true;
            p_rb.constraints = RigidbodyConstraints.FreezeRotation;
            p_rb.AddForce((_knockback_Velocity_upward * p_rb.transform.up) + (_knockback_Velocity_backward * -p_rb.transform.forward), ForceMode.Impulse);
        }

        public void HandleGetupTimeCounter()
        {
            if (_startGetupCounting)
            {
                if (isDead)
                {
                    KnockBack_Dead_CountDown();
                }
                else
                {
                    NormalGetUpCountDown();
                }

                void NormalGetUpCountDown()
                {
                    _getupTimer += _delta;
                    if (_getupTimer >= _getupWaitRate)
                    {
                        _getupTimer = 0;
                        _startGetupCounting = false;
                        GetupAfterKnockDown();
                    }
                }

                void KnockBack_Dead_CountDown()
                {
                    _getupTimer += _delta;
                    if (_getupTimer >= _knockback_death_WaitRate)
                    {
                        _getupTimer = 0;
                        _startGetupCounting = false;

                        KillPlayer_KnockedDown();
                        BlurInScreenOnDeath();
                        DissolveOutPlayerOnDeath();
                    }
                }
            }
        }

        void GetupAfterKnockDown()
        {
            if (_isExecutionKnockedBack)
            {
                GetupByPlayingGetupAnim();
            }
            else
            {
                GetupBySetAnimBool();
            }

            void GetupBySetAnimBool()
            {
                anim.SetBool(p_IsGetupReady_hash, true);
            }

            void GetupByPlayingGetupAnim()
            {
                int _animHash = 0;

                if (_isTwoHanding)
                {
                    if (_currentExecutionProfile._isGetupFromFaceUp)
                    {
                        _animHash = _savableInventory._rightHandWeapon_referedItem.Get_TwoHanded_FaceUp_GetupHashByType();
                    }
                    else
                    {
                        _animHash = _savableInventory._rightHandWeapon_referedItem.Get_TwoHanded_FaceDown_GetupHashByType();
                    }
                }
                else
                {
                    if (_currentExecutionProfile._isGetupFromFaceUp)
                    {
                        _animHash = _savableInventory._rightHandWeapon_referedItem.Get_OneHanded_FaceUp_GetupHashByType();
                    }
                    else
                    {
                        _animHash = _savableInventory._rightHandWeapon_referedItem.Get_OneHanded_FaceDown_GetupHashByType();
                    }
                }

                PlayKnockedDownGetupAnim(_animHash);
            }
        }

        public void OnGetupResetStatus()
        {
            p_rb.mass = 500;
            p_rb.drag = 4;
            p_rb.angularDrag = 999f;
            p_rb.useGravity = false;
            p_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            isCantBeDamaged = false;

            skipGroundCheck = false;
            knockedDownSkipGroundCheck = false;

            if (_isExecutionKnockedBack)
            {
                _currentExecutionProfile = null;
                _isExecutionKnockedBack = false;
            }
        }
        #endregion

        #region Transitions.

        #region Enter / Exit Fighter Mod.
        public bool IsExitFighterModeTransition()
        {
            if (!anim.GetBool(p_IsNeglecting_hash))
            {
                OffFighterModeResetBools();
                return true;
            }

            return false;
        }
        #endregion

        #region Enter / Exit Wait For Animation To End.
        public bool IsEnterWaitForAnimationEndTransition()
        {
            if (anim.GetBool(p_IsNeglecting_hash) && !_isTwoHandFistAttacking)
            {
                return true;
            }

            return false;
        }

        public bool IsExitWaitForAnimationEndTransition()
        {
            if (!anim.GetBool(p_IsNeglecting_hash))
            {
                OnIdleStateResetBools();
                return true;
            }

            return false;
        }
        #endregion
        
        #endregion

        #region Fighter Mode.
        public void SetIsTwoHandFistAttacking()
        {
            if (_isInTwoHandFist)
                _isTwoHandFistAttacking = true;
            else
                _isTwoHandFistAttacking = false;
        }

        public void ChangePlayerStatsWhenHit()
        {
            if (_isTwoHandFistAttacking)
            {
                isReduceNextAttackCost = true;
                _mainHudManager.RegisterFighterModeIcon();
            }
        }
        #endregion

        #region Getup.
        public bool IsExitWaitForKnockedDownEndTransition()
        {
            if (!anim.GetBool(p_IsNeglecting_hash))
            {
                OnIdleStateResetBools();
                OnGetupResetStatus();
                return true;
            }

            return false;
        }
        #endregion

        #region Level up (Confirm Levelup Message / Effect).
        public void OnConfirmLevelup()
        {
            statsHandler.OnLevelingConfirm_OverwriteBaseAttributes();
            _savableInventory.runtimeAmulet.OnLevelupChangeEmission();
            Spawn_LevelupEffect();
        }

        public void Spawn_LevelupEffect()
        {
            _gameManager._levelupImpactEffect.SpawnInParent(mTransform);
        }
        #endregion

        #region On Death.
        public void DissolveOutPlayerOnDeath()
        {
            _aiGroupManagable.isForbiddenToFoundPlayer = true;

            OnDeathResetInputs();
            OnDeathPauseComment();
            OnDeathDissolvePlayer();
            OnDeathHideTrailFx();

            void OnDeathResetInputs()
            {
                ResetPlayerInput_OnDeath();
                _camHandler.PausePlayerInput_OnDeath();
            }

            void OnDeathPauseComment()
            {
                _commentHandler.PauseAcceptingComment();
            }

            void OnDeathDissolvePlayer()
            {
                DissolveOutPlayer_Immediately();
                DissolveOutEquipments_AfterWait();

                void DissolveOutPlayer_Immediately()
                {
                    LeanTween.value(_dissolveFullOpaqueValue, _deathDissolveFullTransparentValue, _deathDissolveSpeed).setOnUpdate
                    (
                        (value) => _playerBodyDissolveMat.SetFloat(_dissolveCutoffPropertyId, value)
                    )
                    .setOnComplete(OnDeathShowOpeningMask);

                    /// Opening Mask.
                    void OnDeathShowOpeningMask()
                    {
                        _loadingScreenHandler.QuickShowOpeningMask();

                        _camHandler.ResetCurrentAngles_OnDeath();
                        _camHandler.SwitchToPauseCamCollisionManagable();

                        LeanTween.value(0, 1, 0.25f).setOnComplete(OnDeathStartRevivePlayer);
                    }
                }

                void DissolveOutEquipments_AfterWait()
                {
                    LeanTween.value(0, 1, 1).setOnComplete(_savableInventory.OnDeathDissolveOutEquipments);
                }
            }

            void OnDeathHideTrailFx()
            {
                if (_isUsedTrailFx)
                    a_hook.currentTrailFxHandler.Play_NullState_TrailFx();
            }
        }
        
        /// Savable Inventory, On Death Dessolve Powerup. 
        public void DissolveOutPowerup()
        {
            _playerBackCapeDissolveMat.SetColor(_dissolveEmmisionColorPropertyId, _defaultVolunColor);
            _playerBackCapeDissolveMat.EnableKeyword("_LOCALAXISCUTOFFTYPE_LOCAL_Z");

            PowerupItem _powerUpItem = _savableInventory.powerupSlot._referedPowerupItem;
            LeanTween.value(_powerUpItem._onDeath_cutOffFullOpaqueValue, _powerUpItem._onDeath_cutOffTransparentValue, 1f).setOnUpdate
                (
                    (value) => _playerBackCapeDissolveMat.SetFloat(_dissolveCutoffPropertyId, value)
                );
        }

        /// Loading Screen Operations.
        public void OnDeathStartRevivePlayer()
        {
            _loadingScreenHandler.OnReviveFadeOutOpeningMask();
            _loadingScreenHandler.ShowLoadingScreen(3, "Prepare Level.", OnRevivePlayer_UnloadUnuseAssets);
        }
        #endregion

        #region On Revive.
        void OnRevivePlayer_UnloadUnuseAssets()
        {
            _loadingScreenHandler.SetLoadingSliderValueImmidiate(0.3f);
            
            /// Refresh Enemy AI. Replenish Vessels and General Stats.
            CheckpointRefreshEvent.Invoke();

            LeanTween.value(0, 1, 0.5f).setOnComplete(OnRevivePlayer_PrepareLevel);
        }

        void OnRevivePlayer_PrepareLevel()
        {
            _loadingScreenHandler.SetLoadingSliderValueImmidiate(0.7f);
            _loadingScreenHandler.RefreshLoadingScreen("Unload Unused Assets.");

            /// Unload Unuse Assets.
            Resources.UnloadUnusedAssets();

            LeanTween.value(0, 1, 1f).setOnComplete(OnRevivePlayer_PreparePlayer);
        }

        void OnRevivePlayer_PreparePlayer()
        {
            _loadingScreenHandler.SetLoadingSliderValueImmidiate(0.9f);
            _loadingScreenHandler.RefreshLoadingScreen("Prepare Player.");

            /// Off TwoHanding.
            if (_isTwoHanding)
            {
                _isTwoHanding = false;
                _savableInventory.OnReviveOffTwoHanding();
            }

            /// Off Lockon.
            if (isLockingOn)
            {
                SetIsLockingOnStatusToFalse();
            }

            /// Reset Locomotion IK State.
            _currentLocoIKState = _playerIKHandler._freeForm_1H_LocoIKState;
            _isPausingLocoIKStateTick = false;

            /// INeglectActions.
            OnRevivePlayerClearNeglectAction();

            /// Respawn player to spawn point. Move Camera to Init Transforms.
            ReturnToCurrentSpawnLocation();
            _camHandler.MoveComponentsToInitTransform();

            /// Dissolve In Player
            DissolveInPlayerOnRevive();

            LeanTween.value(0, 1, 0.5f).setOnComplete(OnRevivePlayer_Restart);
        }

        void OnRevivePlayerClearNeglectAction()
        {
            _currentNeglectInputAction = null;
            isNeglectingInput = false;
        }

        void OnRevivePlayer_Restart()
        {
            _loadingScreenHandler.HideLoadingScreen(true);

            /// Gameobject Layer.
            gameObject.layer = _layerManager.playerLayer;

            /// Rb.
            p_rb.isKinematic = false;
            p_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            /// Play Revive Animation.
            PlayReviveAnimation();
            
            /// Post Process DoF.
            LeanTween.value(0, 1, 0.75f).setOnComplete(BlurOutScreenOnRevive);

            LeanTween.value(0, 1, 0.75f).setOnComplete(OnRevivePlayer_ResumePlayerControls);
        }

        void OnRevivePlayer_ResumePlayerControls()
        {
            isDead = false;
            currentState = _gameManager.playerIdleState;

            _aiGroupManagable.isForbiddenToFoundPlayer = false;

            _commentHandler.ResumeAcceptingComment();

            _inp.SetIsInMainHudStatus(true);
            _camHandler.SwitchToFreeCamCollisionManagable();
        }

        public void DissolveInPlayerOnRevive()
        {
            DissolveInPlayer();

            LeanTween.value(0, 1, 1).setOnComplete(_savableInventory.OnReviveDissolveInEquipments);
            LeanTween.value(0, 8, 1).setOnComplete(_commentHandler.ResumeAcceptingComment);

            void DissolveInPlayer()
            {
                LeanTween.value(_deathDissolveFullTransparentValue, _dissolveFullOpaqueValue, _reviveDissolveSpeed).setOnUpdate
                (
                    (value) => _playerBodyDissolveMat.SetFloat(_dissolveCutoffPropertyId, value)
                );
            }
        }

        /// Savable Inventory, On Death Dessolve Powerup. 
        public void DissolveInPowerup()
        {
            _playerBackCapeDissolveMat.EnableKeyword("_LOCALAXISCUTOFFTYPE_LOCAL_Y");

            PowerupItem _powerUpItem = _savableInventory.powerupSlot._referedPowerupItem;
            LeanTween.value(_powerUpItem._cutOffTransparentValue, _powerUpItem._cutOffFullOpaqueValue, 1f).setOnUpdate
                (
                    (value) => _playerBackCapeDissolveMat.SetFloat(_dissolveCutoffPropertyId, value)
                );
        }
        #endregion

        #region Spawn Point.
        public void ReturnToCurrentSpawnLocation()
        {
            mTransform.position = _currentSpawnPoint.transform.position;
            mTransform.rotation = _currentSpawnPoint.transform.rotation;
        }
        #endregion

        #region Post Process Depth Of Field (Blur Screen).
        public void BlurInScreenOnDeath()
        {
            LeanTween.value(_WF_DOF_MaxRadiusPara.value, _targetDoFMaxRadius, _OnDeath_DoFMaxRadiusChangeSpeed).setOnUpdate((value) => _WF_DOF_MaxRadiusPara.value = value);
            LeanTween.value(_WF_DOF_StartPara.value, _targetDoFStartValue, _DoFStartParaChangeSpeed).setOnUpdate((value) => _WF_DOF_StartPara.value = value);
        }

        void BlurOutScreenOnRevive()
        {
            LeanTween.value(_WF_DOF_MaxRadiusPara.value, _originalDoFMaxRadius, _OnRevive_DoFMaxRadiusChangeSpeed).setOnUpdate((value) => _WF_DOF_MaxRadiusPara.value = value);
            LeanTween.value(_WF_DOF_StartPara.value, _originalDoFStartValue, _DoFStartParaChangeSpeed).setOnUpdate((value) => _WF_DOF_StartPara.value = value);
        }
        #endregion

        #region Armor Chest Interactable.
        public void ArmorTakeChestInteractable()
        {
            skinnedMeshCombiner.UndoCombineMeshes(false, false);
            TakeChestInteractable(PickupCommentaryTypeEnum.Armors);
        }

        /// On Armor Preview.
        public void OnArmorPreviewDissolve()
        {
            _playerBodyDissolveMat.SetColor(_dissolveEmmisionColorPropertyId, _armorAbsorbHDRColor);

            LeanTween.value(_dissolveFullOpaqueValue, _dissolveFullTransparentValue, _armorPreviewDissolveSpeed).setOnUpdate
                (
                    (value) => _playerBodyDissolveMat.SetFloat(_dissolveCutoffPropertyId, value)
                )
                .setOnComplete(OnCompleteDissolvePlayer);
            
            void OnCompleteDissolvePlayer()
            {
                isMeshCombinedNeeded = true;
                _savableInventory.ReversePreviewArmors();
            }
        }

        public void ReverseArmorPreviewDissolve()
        {
            _playerBodyDissolveMat.SetColor(_dissolveEmmisionColorPropertyId, _defaultVolunColor);
            LeanTween.value(_dissolveFullTransparentValue, _dissolveFullOpaqueValue, _armorPreviewDissolveSpeed).setOnUpdate
            (
                (value) => _playerBodyDissolveMat.SetFloat(_dissolveCutoffPropertyId, value)
            );
        }
        #endregion

        #region Powerup Chest Interactable.
        public void PowerupTakeChestInteractable()
        {
            skinnedMeshCombiner.UndoCombineMeshes(false, false);
            TakeChestInteractable(PickupCommentaryTypeEnum.Powerups);
        }

        /// On Powerup Preview.
        public void OnPowerupPreivewComplete()
        {
            OnPreviewCompleteDissolvePowerup();

            void OnPreviewCompleteDissolvePowerup()
            {
                _playerBackCapeDissolveMat.SetColor(_dissolveEmmisionColorPropertyId, _powerupAbsorbHDRColor);
                _playerBackCapeDissolveMat.EnableKeyword("_LOCALAXISCUTOFFTYPE_LOCAL_Y");

                PowerupItem _powerUpItem = _savableInventory._pickedUpReadyDissolvePowerup._referedPowerupItem;
                LeanTween.value(_powerUpItem._cutOffFullOpaqueValue, _powerUpItem._cutOffTransparentValue, 0.7f).setOnUpdate
                    (
                        (value) => _playerBackCapeDissolveMat.SetFloat(_dissolveCutoffPropertyId, value)
                    )
                    .setOnComplete(OnCompleteDissolvePowerup);

                void OnCompleteDissolvePowerup()
                {
                    _savableInventory.OnCompleteDissolvePreviewPowerup();
                }
            }
        }
        
        public void OnCompleteDissolvePowerup_WithoutPowerupEquip()
        {
            isMeshCombinedNeeded = true;
            backPiecesRenderer.enabled = false;
            _playerBackCapeDissolveMat.SetFloat(_dissolveCutoffPropertyId, 2);
        }
        
        public void ReversePowerupPreviewDissolve()
        {
            backPiecesRenderer.sharedMesh = _savableInventory.powerupSlot.powerupMesh;

            _playerBackCapeDissolveMat.SetColor(_dissolveEmmisionColorPropertyId, _defaultVolunColor);

            PowerupItem _powerUpItem = _savableInventory.powerupSlot._referedPowerupItem;
            LeanTween.value(_powerUpItem._cutOffTransparentValue, _powerUpItem._cutOffFullOpaqueValue, 0.7f).setOnUpdate
                (
                    (value) => _playerBackCapeDissolveMat.SetFloat(_dissolveCutoffPropertyId, value)
                );

            isMeshCombinedNeeded = true;
        }
        #endregion

        #region Ring Chest Leave Comments.
        public void TakeRingChestItemLeaveComment()
        {
            _commentHandler._currentPickupCommentType = PickupCommentaryTypeEnum.Rings;
            _commentHandler.RegisterPickupItemCommentMoment();
        }
        #endregion

        #region WA Slash Effects - Hold Attack / Charge Attack.
        public void SetCurrent_1H_HoldAttackWeapon(bool _isLeft)
        {
            if (_isLeft)
            {
                _isHoldingLT = true;
                _currentHoldAttackWeapon = _savableInventory._leftHandWeapon_referedItem;
                _isLhHoldLoopEffect = true;
            }
            else
            {
                _isHoldingRT = true;
                _currentHoldAttackWeapon = _savableInventory._rightHandWeapon_referedItem;
                _isLhHoldLoopEffect = false;
            }
        }

        public void SetCurrent_2H_HoldAttackWeapon()
        {
            if (_savableInventory._twoHandingWeapon.isCurrentLhWeapon)
                _isHoldingLT = true;
            else
                _isHoldingLT = false;

            _currentHoldAttackWeapon = _savableInventory._twoHandingWeapon_referedItem;
            _isLhHoldLoopEffect = false;
        }

        public void Play_HoldATK_Loop_Effect()
        {
            WA_Effect_Profile _profile = _currentHoldAttackWeapon.hold_loop_effect_profile;
            
            _holdATK_Loop_effect = _gameManager.Get_WA_Effect(_profile._id);
            _holdATK_Loop_effect.PlayEffect(_profile);
        }

        public void Stop_HoldATK_Loop_Effect()
        {
            if (_holdATK_Loop_effect != null)
            {
                _holdATK_Loop_effect.StopEffect();
                _holdATK_Loop_effect = null;
            }
        }

        public void Play_HoldATK_Comp_Effect()
        {
            if (_hasHoldAtkReachedMaximum)
            {
                Play_FullComplete_Effect();
            }
            else
            {
                Play_HalfComplete_Effect();
            }

            void Play_HalfComplete_Effect()
            {
                Stop_HoldATK_Loop_Effect();
                _gameManager.Play_WA_Effect(_currentHoldAttackWeapon.Get_Hold_HalfComp_Effect());
            }

            void Play_FullComplete_Effect()
            {
                Stop_HoldATK_Loop_Effect();
                _gameManager.Play_WA_Effect(_currentHoldAttackWeapon.Get_Hold_FullComp_Effect());
            }
        }

        public void Play_ChargeATK_Loop_Effect()
        {
            WA_Effect_Profile _profile = _currentHoldAttackWeapon.hold_loop_effect_profile;

            _holdATK_Loop_effect = _gameManager.Get_WA_Effect(_profile._id);
            _holdATK_Loop_effect.PlayEffect(_profile);
        }

        public void Play_Charge_Enchant_Effect()
        {
            _gameManager.Play_WA_Effect(_currentHoldAttackWeapon.Get_ChargeEnchant_Effect());
            _savableInventory._twoHandingWeapon._trailFxHandler.Play_ChargeEnchant_TrailFx();
        }

        public void Play_Charge_Attack_Effect()
        {
            Stop_HoldATK_Loop_Effect();

            _gameManager.Play_WA_Effect(_currentHoldAttackWeapon.Get_ChargeAttack_Effect());
            _savableInventory._twoHandingWeapon._trailFxHandler.Play_ChargeAttack_TrailFx();
        }
        #endregion

        #region Set Damage Taken Type Enum.
        void RegisterDamagePreviewer()
        {
            _mainHudManager.RegisterDamagePreviewer();
        }

        void SetDamageTakenEnumByPhysicalAttackType()
        {
            switch (_hitSourceAttackRefs._attackPhysicalType)
            {
                case AI_AttackRefs.AIAttackPhysicalTypeEnum.Strike:
                    _damageTakenPhysicalType = DamageTakenPhysicalTypeEnum.Strike;
                    break;
                case AI_AttackRefs.AIAttackPhysicalTypeEnum.Slash:
                    _damageTakenPhysicalType = DamageTakenPhysicalTypeEnum.Slash;
                    break;
                case AI_AttackRefs.AIAttackPhysicalTypeEnum.Thrust:
                    _damageTakenPhysicalType = DamageTakenPhysicalTypeEnum.Thrust;
                    break;
            }
        }

        void SetDamageTakenEnumByExecution()
        {
            _damageTakenPhysicalType = DamageTakenPhysicalTypeEnum.Execution;
        }

        void SetDamageTakenEnumByAOE()
        {
            _damageTakenPhysicalType = DamageTakenPhysicalTypeEnum.AOE;
        }

        void SetDamageTakenEnumByFalling()
        {
            _damageTakenPhysicalType = DamageTakenPhysicalTypeEnum.Falling;
        }
        #endregion

        #region Comment Handler.
        public void OnAttackActionInterruptComment()
        {
            _commentHandler.PauseAcceptingComment_AsInterrupt();
        }

        public void ResetCommentInterruptStatus()
        {
            if (_commentHandler._isPauseAcceptingComment)
            {
                if (_aiGroupManagable._isAggroEmpty && !isLockingOn)
                {
                    _commentHandler.ResumeAcceptingComment();
                }
            }
        }

        public void InterruptedComment_TriggerCommentTrigger()
        {
            p_collider.enabled = false;
            p_collider.enabled = true;
        }
        #endregion

        #region Charging Attack Action.
        public void UpdateChargeAttackInputAmount()
        {
            _inputChargeAmount += _delta;
        }

        public void EnchantChargeAttack()
        {
            if (!_hasChargeEnchanted)
            {
                CrossFadeAnimWithMoveDir(_savableInventory._twoHandingWeapon_referedItem.Get_Heavy1_Charge_EnchantHashByType(), false, true);
                _hasChargeEnchanted = true;
            }
        }

        public void SetIsReadyForChargeReleaseToTrue()
        {
            Play_Charge_Enchant_Effect();
            _isReadyForChargeRelease = true;
        }

        public void Base_SetIsAttackChargingToTrue()
        {
            /// Set is Holding RT to true.
            _isHoldingRT = true;
            _isAttackCharging = true;

            /// Prepare for Effects.
            _isLhHoldLoopEffect = false;
            _currentHoldAttackWeapon = _savableInventory._twoHandingWeapon_referedItem;

            OnAttackActionInterruptComment();
        }

        public void Base_SetIsAttackChargingToFalse()
        {
            /// Set is Holding RT to false.
            _isHoldingRT = false;

            _inputChargeAmount = 0;
            _isAttackCharging = false;
            _isReadyForChargeRelease = false;

            _isActionRequireUpdateInNeglectState = false;
            _currentNeglectInputAction = null;

            ResetCommentInterruptStatus();
        }

        void OnHit_SetIsAttackChargingToFalse()
        {
            SetisAttackChargingToFalse();
            CancelAllWeaponChargeEffects();
            PlayEmptyFullbodyOverrideAnim();

            void SetisAttackChargingToFalse()
            {
                Base_SetIsAttackChargingToFalse();
                isNeglectingInput = false;
            }

            void CancelAllWeaponChargeEffects()
            {
                Stop_HoldATK_Loop_Effect();

                if (_isUsedTrailFx)
                    a_hook.currentTrailFxHandler.Play_NullState_TrailFx();
            }

            void PlayEmptyFullbodyOverrideAnim()
            {
                anim.Play(p_empty_fullBody_override_hash);
            }
        }
        #endregion

        #region BFX Handlers.
        void SpawnTargetBloodFx()
        {
            BFX_StickyUpdater newSticky;
            BFX_Handler newBfxHandler;
            
            Vector3 hitNormal;
            
            Get_Init_Sticky();
            Get_Init_Handler();
            
            StartBloodFXs();
            
            void Get_Init_Sticky()
            {
                newSticky = _gameManager._stickyPool.Get();

                Transform stickyBloodTransfrom = newSticky.transform;

                //Scale
                stickyBloodTransfrom.localScale = vector3One * Random.Range(0.75f, 1.2f);

                //Position
                stickyBloodTransfrom.parent = closestStickableTrans;
                stickyBloodTransfrom.localPosition = vector3Zero;

                //Rotation
                hitNormal = _hitPoint - closestStickableTrans.position;
                stickyBloodTransfrom.LookAt(hitNormal, Vector3.right);
                stickyBloodTransfrom.LookAt(hitNormal, Vector3.forward);

                AddStickyUpdaterToInProcessList();

                void AddStickyUpdaterToInProcessList()
                {
                    inProcess_stickyUpdaters.Add(newSticky);
                    inProcessStickyUpdatersAmount++;
                    isStickyUpdatersEmpty = false;
                }
            }

            void Get_Init_Handler()
            {
                if (_hitSourceAttackRefs.isRandomizeBfxId)
                {
                    newBfxHandler = _gameManager.GetTargetBfxHandler(_random_Bfx_IDs[Random.Range(0, randomBfxAmount)]);
                }
                else
                {
                    newBfxHandler = _gameManager.GetTargetBfxHandler(_hitSourceAttackRefs.bfxId);
                }

                Transform newBfxHandlerTrans = newBfxHandler.transform;
                newBfxHandlerTrans.parent = null;

                if (!isUseBfxHandlerYPosBuffer)
                {
                    newBfxHandlerTrans.position = _hitPoint;
                }
                else
                {
                    Vector3 _newSpawnPos = _hitPoint;
                    _newSpawnPos.y += 1;
                    newBfxHandlerTrans.position = _newSpawnPos;
                }

                newBfxHandlerTrans.rotation = Quaternion.Euler(0, Mathf.Atan2(hitNormal.z, hitNormal.x) * Mathf.Rad2Deg + 270, 0);

                AddBfxHandlerToInProcessList();

                void AddBfxHandlerToInProcessList()
                {
                    inProcess_bfxhandlers.Add(newBfxHandler);
                    inProcessBfxHandlersAmount++;
                    isBfxHandlersEmpty = false;
                }
            }
            
            void StartBloodFXs()
            {
                newSticky.OnStickyStart();
                newBfxHandler.On_BFX_Start();
            }
        }

        void SpawnFallOffBloodFx()
        {
            BFX_StickyUpdater newSticky;
            BFX_Handler newBfxHandler;

            Vector3 hitNormal;

            Get_Init_Sticky();
            Get_Init_Handler();

            StartBloodFXs();

            void Get_Init_Sticky()
            {
                newSticky = _gameManager._stickyPool.Get();

                Transform stickyBloodTransfrom = newSticky.transform;

                //Scale
                stickyBloodTransfrom.localScale = vector3One * Random.Range(0.75f, 1.2f);

                //Position
                stickyBloodTransfrom.parent = d_L_FeetStickableTrans;
                stickyBloodTransfrom.localPosition = vector3Zero;

                //Rotation
                hitNormal = _hitPoint - d_L_FeetStickableTrans.position;
                Vector3 _targetToLookAt = d_L_FeetStickableTrans.position + hitNormal;
                stickyBloodTransfrom.LookAt(_targetToLookAt, Vector3.right);
                stickyBloodTransfrom.LookAt(_targetToLookAt, Vector3.forward);

                AddStickyUpdaterToInProcessList();

                void AddStickyUpdaterToInProcessList()
                {
                    inProcess_stickyUpdaters.Add(newSticky);
                    inProcessStickyUpdatersAmount++;
                    isStickyUpdatersEmpty = false;
                }
            }

            void Get_Init_Handler()
            {
                newBfxHandler = _gameManager.GetFallOffBfxHandler();

                Transform newBfxHandlerTrans = newBfxHandler.transform;
                newBfxHandlerTrans.parent = null;
                
                newBfxHandlerTrans.position = _hitPoint;
                newBfxHandlerTrans.rotation = Quaternion.Euler(0, Mathf.Atan2(hitNormal.z, hitNormal.x) * Mathf.Rad2Deg + 270, 0);

                AddBfxHandlerToInProcessList();

                void AddBfxHandlerToInProcessList()
                {
                    inProcess_bfxhandlers.Add(newBfxHandler);
                    inProcessBfxHandlersAmount++;
                    isBfxHandlersEmpty = false;
                }
            }

            void StartBloodFXs()
            {
                newSticky.OnStickyStart();
                newBfxHandler.On_BFX_Start();
            }
        }

        public void RemoveStickyUpdaterFromInProcessList(BFX_StickyUpdater _stickerToRemove)
        {
            inProcess_stickyUpdaters.Remove(_stickerToRemove);
            inProcessStickyUpdatersAmount--;
            
            if (inProcessStickyUpdatersAmount < 1)
                isStickyUpdatersEmpty = true;
        }

        public void RemoveBfxHandlerFromInProcessList(BFX_Handler handlerToRemove)
        {
            inProcess_bfxhandlers.Remove(handlerToRemove);
            inProcessBfxHandlersAmount--;

            if (inProcessBfxHandlersAmount < 1)
                isBfxHandlersEmpty = true;
        }
        
        public void HandleBloodFxTick()
        {
            if (!isStickyUpdatersEmpty)
            {
                for (int i = 0; i < inProcessStickyUpdatersAmount; i++)
                {
                    inProcess_stickyUpdaters[i].Tick();
                }
            }

            if (!isBfxHandlersEmpty)
            {
                for (int i = 0; i < inProcessBfxHandlersAmount; i++)
                {
                    inProcess_bfxhandlers[i].Tick();
                }
            }
        }
        #endregion

        #region Get Random AI BloodFx IDs.

        // Larges.
        public int Get_Random_L_Strike_Bfx_ID()
        {
            return _strike_L_AI_Bfx_Id[Random.Range(0, _strike_L_AI_Bfx_Id.Length)];
        }

        public int Get_Random_L_Slash_Bfx_ID()
        {
            return _slash_L_AI_Bfx_Id[Random.Range(0, _slash_L_AI_Bfx_Id.Length)];
        }

        public int Get_Random_L_Thrust_Bfx_ID()
        {
            return _thrust_L_AI_Bfx_Id[Random.Range(0, _thrust_L_AI_Bfx_Id.Length)];
        }

        // Mediums
        public int Get_Random_M_Strike_Bfx_ID()
        {
            return _strike_M_AI_Bfx_Id[Random.Range(0, _strike_M_AI_Bfx_Id.Length)];
        }

        public int Get_Random_M_Slash_Bfx_ID()
        {
            return _slash_M_AI_Bfx_Id[Random.Range(0, _slash_M_AI_Bfx_Id.Length)];
        }

        public int Get_Random_M_Thrust_Bfx_ID()
        {
            return _thrust_M_AI_Bfx_Id[Random.Range(0, _thrust_M_AI_Bfx_Id.Length)];
        }
        #endregion

        #region AI Set Destination To Player.
        public void ReturnPredictedMoveTowardDestn(AIManager _ai)
        {
            _ai.predictedMoveTowardDestn = mTransform.right * (horizontal * _ai.predictMoveTowardAmount_h);
            _ai.predictedMoveTowardDestn += -mTransform.forward;
        }
        #endregion

        #region Serialization.

        #region SAVE (NEW).
        public SavablePlayerState SaveStateToSave_Player()
        {
            SavablePlayerState _savablePlayerState = new SavablePlayerState();
            _savablePlayerState.savablePosition = new SavableVector3(mTransform.position);
            _savablePlayerState.savableEulers = new SavableVector3(mTransform.eulerAngles);
            _savablePlayerState.savableSpawnPointId = _currentSpawnPoint.spawnId;
            return _savablePlayerState;
        }

        public SavableProfileState SaveStateToSave_Profile()
        {
            SavableProfileState _savableProfileState = new SavableProfileState();
            _savableProfileState.savableProfName = statsHandler.characterName;
            _savableProfileState.savableProfDate = DateTime.UtcNow.ToLocalTime().ToString("M/d/yy   HH:mm");
            _savableProfileState.savableProfLevel = statsHandler.playerLevel;
            _savableProfileState.savableProfVolun = statsHandler.voluns;
            return _savableProfileState;
        }

        public void CreateAvatarImageToSave()
        {
            ResetPlayerInput_OnAvatarCapture();
            _avatarHandler.RequestAvatarCapture();
        }
        #endregion

        #region SAVE (EXIST).
        public void OverwriteStateToSave_Player()
        {
            SavablePlayerState _overwriteState = _current_main_saveFile.savedPlayerState;

            _overwriteState.savablePosition = new SavableVector3(mTransform.position);
            _overwriteState.savableEulers = new SavableVector3(mTransform.eulerAngles);
            _overwriteState.savableSpawnPointId = _currentSpawnPoint.spawnId;
        }

        public void OverwriteStateToSave_Profile()
        {
            SavableProfileState _overwriteState = _current_sub_saveFile._savedProfileState;

            _overwriteState.savableProfName = statsHandler.characterName;
            _overwriteState.savableProfDate = DateTime.UtcNow.ToLocalTime().ToString("M/d/yy   HH:mm");
            _overwriteState.savableProfLevel = statsHandler.playerLevel;
            _overwriteState.savableProfVolun = statsHandler.voluns;
        }
        #endregion

        #region LOAD.
        void LoadStateFromSave(SavablePlayerState _savablePlayerState)
        {
            transform.position = _savablePlayerState.savablePosition.GetValues();
            transform.eulerAngles = _savablePlayerState.savableEulers.GetValues();
            _currentSpawnPoint = LevelManager.singleton.Get_WF_SpawnPointFromDict(_savablePlayerState.savableSpawnPointId);
        }
        #endregion

        #region CHECK SAVE FILE.
        void CheckCompleteSaveFile()
        {
            #region PLAYER
            if (_current_main_saveFile.savedPlayerState != null)
            {
                LoadStateFromSave(_current_main_saveFile.savedPlayerState);
                statsHandler.LoadStatsFromSave(_current_main_saveFile.savedStatusState);
            }
            #endregion

            #region INVENTORY
            _savableInventory.LoadStateFromSave(_current_main_saveFile);
            #endregion

            #region INTERACTION
            Dictionary<string, SavableInteractionState> savedInteractionsDict = new Dictionary<string, SavableInteractionState>();

            List<SavableInteractionState> _savedInteractionStates = _current_main_saveFile.savedInteractionStates;
            int savedInteractionStatesCount = _savedInteractionStates.Count;
            for (int i = 0; i < savedInteractionStatesCount; i++)
            {
                savedInteractionsDict.Add(_savedInteractionStates[i].interactionId, _savedInteractionStates[i]);
            }
            
            List<PlayerInteractable> savedInteractions = _savableManager._playerInteractables;
            int savedInteractionsCount = savedInteractions.Count;

            for (int i = 0; i < savedInteractionsCount; i++)
            {
                savedInteractionsDict.TryGetValue(savedInteractions[i].interactionId, out SavableInteractionState _savableInterState);
                if (_savableInterState != null)
                {
                    savedInteractions[i].LoadInteractionStateFromSave(_savableInterState);
                }
            }
            #endregion
        }
        #endregion

        #region REQUEST SERIALIZATION.
        void RequestSerialization_SaveFromNewSaveFile()
        {
            _savableManager.Serialize_NewFile();
        }

        void RequestSerialization_SaveFromCurrentSaveFile()
        {
            _savableManager.Serialize_ExistFile();
        }
        #endregion

        #endregion

        #region Debug Break.
        void BreakEditor()
        {
            if (Input.GetButton("DebugBreak"))
            {
                Debug.Break();
            }
        }
        #endregion

        #region OnDrawGizoms Sample.
        /*
        bool isUpdating;
        public float sphereDownwardDistance = 1.2f;
        RaycastHit[] hitColliders = new RaycastHit[4];

        private void OnDrawGizmos()
        {
            if (!isUpdating)
                return;

            float r = 0.1f;
            Vector3 origin = mTransform.position;
            origin.y += .7f;

            LayerMask availableGroundCheckMask = ~(1 << LayerMaskManager.singleton.unwalkableLayer | 1 << LayerMaskManager.singleton.playerLayer);
            if (Physics.SphereCastNonAlloc(origin, r, -vector3Up, hitColliders, sphereDownwardDistance, availableGroundCheckMask) > 0)
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].collider != null)
                    {
                        //Debug.Log(hitColliders[i].collider.gameObject);
                    }
                }
            }

            Gizmos.color = Color.red;
            Gizmos.DrawRay(mTransform.position, -vector3Up * sphereDownwardDistance);
            Gizmos.DrawWireSphere(mTransform.position + (-vector3Up * sphereDownwardDistance), r);
        }
        */

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(mTransform.position, _surroundLookAtIKRange);
        //}
        #endregion
    }

    public enum LockonToEnemyModeEnum
    {
        Prioritize_Distance,
        Prioritize_Angle,
        MixWithDistance,
        MixWithAngle
    }

    public enum DamageTakenDirectionTypeEnum
    {
        HitFromLeft,
        HitFromRight,
        HitFromFront,
        HitFromBack
    }

    public enum DamageTakenPhysicalTypeEnum
    {
        Strike,
        Slash,
        Thrust,
        AOE,
        Execution,
        Falling,
        Init
    }
}

#region Two Handing (Comment Out).
//if (twoHandingInputAmount > 0 && !p_twoHanding_input)
//{
//    twoHandingInputAmount = 0;
//    if (_isTwoHanding)
//    {
//        _savableInventory.SetIsTwoHandingStatusToFalseByType();
//    }
//    else
//    {
//        SetIsTwoHandingStatusToTrue(false);
//    }

//}

//if (twoHandingInputAmount > twoHandLhWeaponInputThershold && !_isTwoHanding)
//{
//    SetIsTwoHandingStatusToTrue(true);
//}
#endregion