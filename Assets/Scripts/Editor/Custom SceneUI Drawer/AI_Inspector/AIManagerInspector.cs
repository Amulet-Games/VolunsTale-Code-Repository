using UnityEditor;
using UnityEngine;
using System.Text;

namespace SA
{
    public class AIManagerInspector : Editor
    {
        #region OnSceneGUI
        //float padding = 0.25f;
        //float labelHeight = 2.5f;
        //float numDifferences = 0.15f;

        //StringBuilder aiObjectIdString;
        //StringBuilder aiDirToTargetString;
        //StringBuilder aiDisToTargetString;
        //StringBuilder aiAngleToTargetString;

        //string tempName;
        //Vector3 tempDirToTarget;
        //float tempDistanceToTarget;
        //float tempAngleToTarget;

        //public void OnSceneGUI_Init()
        //{
        //    aiObjectIdString = new StringBuilder();
        //    aiDirToTargetString = new StringBuilder();
        //    aiDisToTargetString = new StringBuilder();
        //    aiAngleToTargetString = new StringBuilder();
        //}

        //public void OnSceneGUI_Tick(AIManager ai)
        //{
        //    if (ai.aIStates == null)
        //        return;

        //    /// Make a new GUIStyle
        //    GUIStyle idfontStyle = new GUIStyle();
        //    ModifyGUIStyleFont(idfontStyle, Color.black, 16);

        //    GUIStyle dirfontStyle = new GUIStyle();
        //    ModifyGUIStyleFont(dirfontStyle, Color.green, 16);

        //    GUIStyle disfontStyle = new GUIStyle();
        //    ModifyGUIStyleFont(disfontStyle, Color.blue, 16);

        //    GUIStyle angfontStyle = new GUIStyle();
        //    ModifyGUIStyleFont(angfontStyle, Color.magenta, 16);

        //    /// Get Transform from aiManager
        //    Transform aiTrans = ai.gameObject.transform;

        //    /// DRAWING TEXT
        //    UpdateAIStatsText(ai);

        //    Handles.Label(aiTrans.position + new Vector3(0.3f, labelHeight, -0.4f), aiObjectIdString.ToString(), idfontStyle);
        //    Handles.Label(aiTrans.position + new Vector3(0.3f, labelHeight - padding, -0.4f), aiDirToTargetString.ToString(), dirfontStyle);
        //    Handles.Label(aiTrans.position + new Vector3(0.3f, labelHeight - padding * 2, -0.4f), aiDisToTargetString.ToString(), disfontStyle);
        //    Handles.Label(aiTrans.position + new Vector3(0.3f, labelHeight - padding * 3, -0.4f), aiAngleToTargetString.ToString(), angfontStyle);

            
        //    /// DRAWING CIRCLE WIRE
        //    Handles.color = Color.red;
        //    Handles.DrawWireArc(aiTrans.position, ai.aIStates.vector3up, aiTrans.forward, 360, ai.close_aggro_Thershold);

        //    /// DRAWING VIEW LINE
        //    Vector3 viewAngleA = StaticHelper.GetDirFromAngle(aiTrans, ai.aggro_Angle, false);
        //    Vector3 viewAngleB = StaticHelper.GetDirFromAngle(aiTrans, -ai.aggro_Angle, false);

        //    Handles.DrawLine(aiTrans.position, aiTrans.position + viewAngleA * ai.close_aggro_Thershold);
        //    Handles.DrawLine(aiTrans.position, aiTrans.position + viewAngleB * ai.close_aggro_Thershold);
        //}

        //void UpdateAIStatsText(AIManager ai)
        //{
        //    if (tempName != ai.name)
        //    {
        //        aiObjectIdString.Clear();
        //        aiObjectIdString.Append("ID    : ");
        //        aiObjectIdString.Append(ai.name.ToString());
        //        tempName = ai.name;
        //    }

        //    if (Mathf.Abs(ai.dirToTarget.x - tempDirToTarget.x) > numDifferences || Mathf.Abs(ai.dirToTarget.y - tempDirToTarget.y) > numDifferences || Mathf.Abs(ai.dirToTarget.z - tempDirToTarget.z) > numDifferences)
        //    {
        //        aiDirToTargetString.Clear();
        //        aiDirToTargetString.Append("Dir   : ");
        //        aiDirToTargetString.Append(ai.dirToTarget.ToString());
        //        tempDirToTarget = ai.dirToTarget;
        //    }

        //    if (Mathf.Abs(tempDistanceToTarget - ai.distanceToTarget) > numDifferences)
        //    {
        //        aiDisToTargetString.Clear();
        //        aiDisToTargetString.Append("Dis   : ");
        //        aiDisToTargetString.Append(ai.distanceToTarget.ToString());
        //        tempDistanceToTarget = ai.distanceToTarget;
        //    }

        //    if (Mathf.Abs(tempAngleToTarget - ai.angleToTarget) > numDifferences)
        //    {
        //        aiAngleToTargetString.Clear();
        //        aiAngleToTargetString.Append("Angle : ");
        //        aiAngleToTargetString.Append(ai.angleToTarget.ToString());
        //        tempAngleToTarget = ai.angleToTarget;
        //    }
        //}

        //void ModifyGUIStyleFont(GUIStyle guiStyle, Color color, int size)
        //{
        //    guiStyle.fontStyle = FontStyle.Bold;
        //    guiStyle.fontSize = size;
        //    guiStyle.normal.textColor = color;
        //}
        #endregion

        #region OnInspectorGUI
        public GUIStyle modHeaderGUIStyle;
        public GUIStyle modSubHeaderGUIStyle;
        public GUIStyle classFoldoutHeaderGUIStyle;
        public GUIStyle foldoutHeaderGUIStyle;
        public GUIStyle subFoldoutHeaderGUIStyle;

        bool openEnemyTypeEnum;
        bool openEnemyWeapons;
        bool openDecisionsVariables;
        bool openAngleDirectionDistance;
        bool openHealth;
        bool openGetHitRefs;
        bool openAIAttackRefs;
        bool openRootMotionVelocity;
        bool openIKTurning;
        bool openAnimTurningTab;
        bool openAnimWithAgentTab;
        bool openMoveWithAgentTab;
        bool openLockOnMoveAroundStats;
        bool openAttackIntervalStats;
        bool openParryableStats;
        bool openBooleans;
        bool openActions;
        bool openActionHolders;
        bool openAreaDamageParticleFx;
        bool openBfxStickableTransforms;
        bool openReferences;
        bool openDragAndDropReferences;
        protected bool openIndicators;

        // Enemy Type
        SerializedProperty enemyTypeEnum;
        SerializedProperty volunReturnAmount;

        // Weapons
        SerializedProperty firstSheathTransform;

        SerializedProperty firstWeapon;
        SerializedProperty currentWeapon;
        SerializedProperty currentThrowableWeapon;
        SerializedProperty firstThrowableWeaponPool;

        SerializedProperty isWeaponOnHand;
        SerializedProperty isReacquireThrowableNeeded;

        // Decisions Variables
        SerializedProperty aggro_Thershold;
        SerializedProperty aggro_ClosestThershold;
        SerializedProperty exitAggro_Thershold;
        SerializedProperty aggro_Angle;
        SerializedProperty currentCrossFadeLayer;
        SerializedProperty defaultCrossFadeLayer;
        SerializedProperty _frameCount;
        SerializedProperty _delta;
        SerializedProperty _aggroTransitTimer;
        SerializedProperty _idleTransitTimer;

        // Angle, Direction, Distance
        SerializedProperty angleToTarget;
        SerializedProperty distanceToTarget;
        SerializedProperty dirToTarget;
        SerializedProperty targetPos;
        SerializedProperty mTransform;

        // Overall Health
        SerializedProperty totalEnemyHealth;
        SerializedProperty currentEnemyHealth;

        // Hit Source Refs.
        SerializedProperty _hitSourceAttackRefs;
        SerializedProperty _previousHitDamage;
        SerializedProperty _hitSourceColliderTransform;
        SerializedProperty _isInvincible;
        SerializedProperty _isSkippingOnHitAnim;
        SerializedProperty _isHitByChargeAttack;

        // Knocked Down Status.
        SerializedProperty _onHitKnockdownGetupWaitRate;
        SerializedProperty _executionGetupWaitRate;
        SerializedProperty isKnockedDown;
        SerializedProperty _currentGetupWaitRate;
        SerializedProperty _getupWaitTimer;

        // Get Executed Status.
        SerializedProperty _previousExecutionDamage;
        SerializedProperty _p_executionProfile;

        // Invincible Status.
        SerializedProperty _invincibleResetRate;
        SerializedProperty _invincibleResetTimer;

        // AI Attack Refs.
        SerializedProperty currentElementalType;
        SerializedProperty currentAttackRefs;

        // Root Motion Velocity
        SerializedProperty runningPredictAddonValue;
        SerializedProperty currentPlayerPredictOffset;
        SerializedProperty ignoreAttackRootMotionCalculate;
        SerializedProperty attackMaxVelocityDistance;

        SerializedProperty currentAttackVelocity;
        SerializedProperty currentRollVelocity;
        SerializedProperty currentFallbackVelocity;
        SerializedProperty currentKnockdownVelocity;

        SerializedProperty applyTurnRootMotion;
        SerializedProperty applyAttackArtifMotion;
        
        // ANIM TURNING
        SerializedProperty animRootRotateThershold;
        SerializedProperty rootTurningSpeed;
        SerializedProperty animInplaceRotateThershold;
        SerializedProperty inplaceTurningSpeed;

        // MOVE TOWARD TURNING
        SerializedProperty maneuverAngularSpeed;
        SerializedProperty maneuverLookAtPlayerThershold;
        SerializedProperty maneuverHeadIKThershold;

        // IK TURNING
        SerializedProperty upperBodyIKRotateThershold;
        SerializedProperty maxUpperBodyIKTurningSpeed;
        SerializedProperty minUpperBodyIKTurningSpeed;
        SerializedProperty maxUpperBodyIKTurningSpeedDis;
        SerializedProperty minUpperBodyIKTurningSpeedDis;
        SerializedProperty currentUpperBodyIKTurningSpeed;

        // Move Toward
        SerializedProperty locoAnimSwitchDistance;
        SerializedProperty closeDistanecLocoAnimValue;
        SerializedProperty farDistanceLocoAnimValue;
        
        // Patrol
        SerializedProperty patrolLocoAnimValue;

        // Stop Distance
        SerializedProperty agentStopDistance;

        // Speed Config
        SerializedProperty agentAccelSpeed;
        SerializedProperty agentMoveSpeed;
        SerializedProperty closeSpeedBuffer;

        // Speed Status
        SerializedProperty _currentAgentAccelSpeed;
        SerializedProperty _currentAgentMoveSpeed;
        SerializedProperty _currentAgentVelocity;

        // Move Toward Predict
        SerializedProperty predictMoveTowardAmount_h;
        SerializedProperty predictedMoveTowardDestn;

        // LockOn Move Around Stats
        SerializedProperty lockOnPosUpdateRate;
        SerializedProperty lockOnPosUpdateTimer;
        SerializedProperty updateLockOnPos;
        SerializedProperty currentAILockonMoveProfile;
        SerializedProperty relatPosAngle;
        SerializedProperty currentLockOnLocomotionType;
        SerializedProperty targetLockOnPos;

        // Attack Interval Stats
        SerializedProperty maxAttackIntervalRate;
        SerializedProperty minAttackIntervalRate;
        SerializedProperty enemyAttacked;
        SerializedProperty finalizedAttackIntervalRate;
        SerializedProperty attackIntervalTimer;

        // Parryable Stats
        SerializedProperty _isParryable;
        SerializedProperty _isInParryExecuteWindow;
        SerializedProperty _isParryExecutingEnemy;
        SerializedProperty _parryExecuteWaitTimer;

        // Booleans
        SerializedProperty debugActionHolder;

        // Init.
        SerializedProperty isWeaponUnSheathAnimExecuted;
        SerializedProperty isWeaponSheathAnimExecuted;
        SerializedProperty isDead;

        // Movement
        SerializedProperty isMovingToward;
        SerializedProperty isMovingTowardPlayer;
        SerializedProperty isLockOnMoveAround;

        // Quit Neglect.
        SerializedProperty isMultiStageAttackAvailable;

        // Turning.
        SerializedProperty isTrackingPlayer;
        SerializedProperty isPausingTurnWithAgent;
        SerializedProperty useInplaceTurningSpeed;

        // Action
        SerializedProperty currentPassiveAction;
        SerializedProperty currentAction;
        SerializedProperty currentMultiStageAttack;

        // Action Holder
        SerializedProperty firstWeaponActionHolder;
        SerializedProperty currentActionHolder;
        SerializedProperty skippingScoreCalculation;

        // Area Damage Particle FX
        SerializedProperty _currentDamageParticle;

        // Bfx Stickable Transforms
        SerializedProperty _BfxStickableTrans;
        SerializedProperty _cur_PoolableBfxHandler;

        // Ref
        SerializedProperty aiStates;
        SerializedProperty playerStates;
        SerializedProperty hashManager;
        SerializedProperty anim;
        SerializedProperty a_hook;
        SerializedProperty iKHandler;
        SerializedProperty agent;

        // Drag and Drop Ref
        SerializedProperty profile;

        // Show Foldout
        SerializedProperty showEnemyTypeEnum;
        SerializedProperty showEnemyWeapons;
        SerializedProperty showDecisionsVariables;
        SerializedProperty showAngleDirectionDistance;
        SerializedProperty showHealth;
        SerializedProperty showGetHitRefs;
        SerializedProperty showAIAttackRefs;
        SerializedProperty showRootMotionVelocity;
        SerializedProperty showIKTurning;
        SerializedProperty showAnimTurningTab;
        SerializedProperty showAnimWithAgentTab;
        SerializedProperty showMoveWithAgentTab;
        SerializedProperty showLockOnMoveAroundStats;
        SerializedProperty showAttackIntervalStats;
        SerializedProperty showParryableStats;
        SerializedProperty showBooleans;
        SerializedProperty showActions;
        SerializedProperty showActionHolders;
        SerializedProperty showAreaDamageParticleFx;
        SerializedProperty showBfxStickableTransforms;
        SerializedProperty showReferences;
        SerializedProperty showDragAndDropReferences;

        /// Used in Sub Class.
        protected SerializedProperty showIndicators;

        public void OnInspectorGUI_Init()
        {
            enemyTypeEnum = serializedObject.FindProperty("enemyTypeEnum");
            volunReturnAmount = serializedObject.FindProperty("volunReturnAmount");

            firstSheathTransform = serializedObject.FindProperty("firstSheathTransform");
            firstWeapon = serializedObject.FindProperty("firstWeapon");
            currentWeapon = serializedObject.FindProperty("currentWeapon");
            currentThrowableWeapon = serializedObject.FindProperty("currentThrowableWeapon");
            firstThrowableWeaponPool = serializedObject.FindProperty("firstThrowableWeaponPool");
            isWeaponOnHand = serializedObject.FindProperty("isWeaponOnHand");
            isReacquireThrowableNeeded = serializedObject.FindProperty("isReacquireThrowableNeeded");

            aggro_Thershold = serializedObject.FindProperty("aggro_Thershold");
            aggro_ClosestThershold = serializedObject.FindProperty("aggro_ClosestThershold");
            exitAggro_Thershold = serializedObject.FindProperty("exitAggro_Thershold");
            aggro_Angle = serializedObject.FindProperty("aggro_Angle");
            currentCrossFadeLayer = serializedObject.FindProperty("currentCrossFadeLayer");
            defaultCrossFadeLayer = serializedObject.FindProperty("defaultCrossFadeLayer");
            _frameCount = serializedObject.FindProperty("_frameCount");
            _delta = serializedObject.FindProperty("_delta");
            _aggroTransitTimer = serializedObject.FindProperty("_aggroTransitTimer");
            _idleTransitTimer = serializedObject.FindProperty("_idleTransitTimer");


            angleToTarget = serializedObject.FindProperty("angleToTarget");
            distanceToTarget = serializedObject.FindProperty("distanceToTarget");
            dirToTarget = serializedObject.FindProperty("dirToTarget");
            targetPos = serializedObject.FindProperty("targetPos");
            mTransform = serializedObject.FindProperty("mTransform");

            totalEnemyHealth = serializedObject.FindProperty("totalEnemyHealth");
            currentEnemyHealth = serializedObject.FindProperty("currentEnemyHealth");


            _hitSourceAttackRefs = serializedObject.FindProperty("_hitSourceAttackRefs");
            _previousHitDamage = serializedObject.FindProperty("_previousHitDamage");
            _hitSourceColliderTransform = serializedObject.FindProperty("_hitSourceColliderTransform");
            _isSkippingOnHitAnim = serializedObject.FindProperty("_isSkippingOnHitAnim");
            _isHitByChargeAttack = serializedObject.FindProperty("_isHitByChargeAttack");

            _onHitKnockdownGetupWaitRate = serializedObject.FindProperty("_onHitKnockdownGetupWaitRate");
            _executionGetupWaitRate = serializedObject.FindProperty("_executionGetupWaitRate");
            isKnockedDown = serializedObject.FindProperty("isKnockedDown");
            _currentGetupWaitRate = serializedObject.FindProperty("_currentGetupWaitRate");
            _getupWaitTimer = serializedObject.FindProperty("_getupWaitTimer");

            _previousExecutionDamage = serializedObject.FindProperty("_previousExecutionDamage");
            _p_executionProfile = serializedObject.FindProperty("_p_executionProfile");

            _invincibleResetRate = serializedObject.FindProperty("_invincibleResetRate");
            _isInvincible = serializedObject.FindProperty("_isInvincible");
            _invincibleResetTimer = serializedObject.FindProperty("_invincibleResetTimer");


            currentElementalType = serializedObject.FindProperty("currentElementalType");
            currentAttackRefs = serializedObject.FindProperty("currentAttackRefs");

            runningPredictAddonValue = serializedObject.FindProperty("runningPredictAddonValue");
            currentPlayerPredictOffset = serializedObject.FindProperty("currentPlayerPredictOffset");
            ignoreAttackRootMotionCalculate = serializedObject.FindProperty("ignoreAttackRootMotionCalculate");
            attackMaxVelocityDistance = serializedObject.FindProperty("attackMaxVelocityDistance");
            currentAttackVelocity = serializedObject.FindProperty("currentAttackVelocity");
            currentRollVelocity = serializedObject.FindProperty("currentRollVelocity");
            currentFallbackVelocity = serializedObject.FindProperty("currentFallbackVelocity");
            currentKnockdownVelocity = serializedObject.FindProperty("currentKnockdownVelocity");
            
            // Roots (Tick).
            applyTurnRootMotion = serializedObject.FindProperty("applyTurnRootMotion");

            // Roots (FixedTick).
            applyAttackArtifMotion = serializedObject.FindProperty("applyAttackArtifMotion");
            
            // ANIM TURNING
            animRootRotateThershold = serializedObject.FindProperty("animRootRotateThershold");
            rootTurningSpeed = serializedObject.FindProperty("rootTurningSpeed");

            animInplaceRotateThershold = serializedObject.FindProperty("animInplaceRotateThershold");
            inplaceTurningSpeed = serializedObject.FindProperty("inplaceTurningSpeed");

            // MOVE TOWARD TURNING
            maneuverAngularSpeed = serializedObject.FindProperty("maneuverAngularSpeed");
            maneuverLookAtPlayerThershold = serializedObject.FindProperty("maneuverLookAtPlayerThershold");
            maneuverHeadIKThershold = serializedObject.FindProperty("maneuverHeadIKThershold");
            
            // IK TURNING
            upperBodyIKRotateThershold = serializedObject.FindProperty("upperBodyIKRotateThershold");
            maxUpperBodyIKTurningSpeed = serializedObject.FindProperty("maxUpperBodyIKTurningSpeed");
            minUpperBodyIKTurningSpeed = serializedObject.FindProperty("minUpperBodyIKTurningSpeed");
            maxUpperBodyIKTurningSpeedDis = serializedObject.FindProperty("maxUpperBodyIKTurningSpeedDis");
            minUpperBodyIKTurningSpeedDis = serializedObject.FindProperty("minUpperBodyIKTurningSpeedDis");
            currentUpperBodyIKTurningSpeed = serializedObject.FindProperty("currentUpperBodyIKTurningSpeed");


            // Move Toward.
            locoAnimSwitchDistance = serializedObject.FindProperty("locoAnimSwitchDistance");
            closeDistanecLocoAnimValue = serializedObject.FindProperty("closeDistanecLocoAnimValue");
            farDistanceLocoAnimValue = serializedObject.FindProperty("farDistanceLocoAnimValue");
            
            // Patrol.
            patrolLocoAnimValue = serializedObject.FindProperty("patrolLocoAnimValue");


            // Stop Distance.
            agentStopDistance = serializedObject.FindProperty("agentStopDistance");

            // Speed config.
            agentAccelSpeed = serializedObject.FindProperty("agentAccelSpeed");
            agentMoveSpeed = serializedObject.FindProperty("agentMoveSpeed");
            closeSpeedBuffer = serializedObject.FindProperty("closeSpeedBuffer");

            // Speed Status.
            _currentAgentAccelSpeed = serializedObject.FindProperty("_currentAgentAccelSpeed");
            _currentAgentMoveSpeed = serializedObject.FindProperty("_currentAgentMoveSpeed");
            _currentAgentVelocity = serializedObject.FindProperty("_currentAgentVelocity");

            // Move Toward Predict.
            predictMoveTowardAmount_h = serializedObject.FindProperty("predictMoveTowardAmount_h");
            predictedMoveTowardDestn = serializedObject.FindProperty("predictedMoveTowardDestn");


            // Lockon Move Around.
            lockOnPosUpdateRate = serializedObject.FindProperty("lockOnPosUpdateRate");
            lockOnPosUpdateTimer = serializedObject.FindProperty("lockOnPosUpdateTimer");
            updateLockOnPos = serializedObject.FindProperty("updateLockOnPos");
            currentAILockonMoveProfile = serializedObject.FindProperty("currentAILockonMoveProfile");
            relatPosAngle = serializedObject.FindProperty("relatPosAngle");
            currentLockOnLocomotionType = serializedObject.FindProperty("currentLockOnLocomotionType");
            targetLockOnPos = serializedObject.FindProperty("targetLockOnPos");

            // Attack Interval.
            maxAttackIntervalRate = serializedObject.FindProperty("maxAttackIntervalRate");
            minAttackIntervalRate = serializedObject.FindProperty("minAttackIntervalRate");
            enemyAttacked = serializedObject.FindProperty("enemyAttacked");
            finalizedAttackIntervalRate = serializedObject.FindProperty("finalizedAttackIntervalRate");
            attackIntervalTimer = serializedObject.FindProperty("attackIntervalTimer");

            // Parryable.
            _isParryable = serializedObject.FindProperty("_isParryable");
            _isInParryExecuteWindow = serializedObject.FindProperty("_isInParryExecuteWindow");
            _isParryExecutingEnemy = serializedObject.FindProperty("_isParryExecutingEnemy");
            _parryExecuteWaitTimer = serializedObject.FindProperty("_parryExecuteWaitTimer");

            // Debug
            debugActionHolder = serializedObject.FindProperty("debugActionHolder");

            // Init.
            isWeaponUnSheathAnimExecuted = serializedObject.FindProperty("isWeaponUnSheathAnimExecuted");
            isWeaponSheathAnimExecuted = serializedObject.FindProperty("isWeaponSheathAnimExecuted");
            isDead = serializedObject.FindProperty("isDead");

            // Movement.
            isMovingToward = serializedObject.FindProperty("isMovingToward");
            isMovingTowardPlayer = serializedObject.FindProperty("isMovingTowardPlayer");
            isLockOnMoveAround = serializedObject.FindProperty("isLockOnMoveAround");

            // Quit Neglect.
            isMultiStageAttackAvailable = serializedObject.FindProperty("isMultiStageAttackAvailable");

            // Turning.
            isTrackingPlayer = serializedObject.FindProperty("isTrackingPlayer");
            isPausingTurnWithAgent = serializedObject.FindProperty("isPausingTurnWithAgent");
            useInplaceTurningSpeed = serializedObject.FindProperty("useInplaceTurningSpeed");

            // Action
            currentPassiveAction = serializedObject.FindProperty("currentPassiveAction");
            currentAction = serializedObject.FindProperty("currentAction");
            currentMultiStageAttack = serializedObject.FindProperty("currentMultiStageAttack");


            // Action Holder.
            firstWeaponActionHolder = serializedObject.FindProperty("firstWeaponActionHolder");
            currentActionHolder = serializedObject.FindProperty("currentActionHolder");
            skippingScoreCalculation = serializedObject.FindProperty("skippingScoreCalculation");


            // Area Damage Particle Fx.
            _currentDamageParticle = serializedObject.FindProperty("_currentDamageParticle");

            // Bfx Stickabable Transforms.
            _BfxStickableTrans = serializedObject.FindProperty("_BfxStickableTrans");
            _cur_PoolableBfxHandler = serializedObject.FindProperty("_cur_PoolableBfxHandler");

            // Refs
            aiStates = serializedObject.FindProperty("aIStates");
            playerStates = serializedObject.FindProperty("playerStates");
            hashManager = serializedObject.FindProperty("hashManager");
            anim = serializedObject.FindProperty("anim");
            a_hook = serializedObject.FindProperty("a_hook");
            iKHandler = serializedObject.FindProperty("iKHandler");
            agent = serializedObject.FindProperty("agent");


            // Drag and Drop Refs.
            profile = serializedObject.FindProperty("profile");


            // Custom Inspector.
            showEnemyTypeEnum = serializedObject.FindProperty("showEnemyTypeEnum");
            openEnemyTypeEnum = showEnemyTypeEnum.boolValue;
            showEnemyWeapons = serializedObject.FindProperty("showEnemyWeapons");
            openEnemyWeapons = showEnemyWeapons.boolValue;
            showDecisionsVariables = serializedObject.FindProperty("showDecisionsVariables");
            openDecisionsVariables = showDecisionsVariables.boolValue;
            showAngleDirectionDistance = serializedObject.FindProperty("showAngleDirectionDistance");
            openAngleDirectionDistance = showAngleDirectionDistance.boolValue;
            showHealth = serializedObject.FindProperty("showHealth");
            openHealth = showHealth.boolValue;
            showGetHitRefs = serializedObject.FindProperty("showGetHitRefs");
            openGetHitRefs = showGetHitRefs.boolValue;
            showAIAttackRefs = serializedObject.FindProperty("showAIAttackRefs");
            openAIAttackRefs = showAIAttackRefs.boolValue;
            showRootMotionVelocity = serializedObject.FindProperty("showRootMotionVelocity");
            openRootMotionVelocity = showRootMotionVelocity.boolValue;
            showIKTurning = serializedObject.FindProperty("showIKTurning");
            openIKTurning = showIKTurning.boolValue;
            showAnimTurningTab = serializedObject.FindProperty("showAnimTurningTab");
            openAnimTurningTab = showAnimTurningTab.boolValue;
            showAnimWithAgentTab = serializedObject.FindProperty("showAnimWithAgentTab");
            openAnimWithAgentTab = showAnimWithAgentTab.boolValue;
            showMoveWithAgentTab = serializedObject.FindProperty("showMoveWithAgentTab");
            openMoveWithAgentTab = showMoveWithAgentTab.boolValue;
            showLockOnMoveAroundStats = serializedObject.FindProperty("showLockOnMoveAroundStats");
            openLockOnMoveAroundStats = showLockOnMoveAroundStats.boolValue;
            showAttackIntervalStats = serializedObject.FindProperty("showAttackIntervalStats");
            openAttackIntervalStats = showAttackIntervalStats.boolValue;
            showParryableStats = serializedObject.FindProperty("showParryableStats");
            openParryableStats = showParryableStats.boolValue;
            showBooleans = serializedObject.FindProperty("showBooleans");
            openBooleans = showBooleans.boolValue;
            showActions = serializedObject.FindProperty("showActions");
            openActions = showActions.boolValue;
            showActionHolders = serializedObject.FindProperty("showActionHolders");
            openActionHolders = showActionHolders.boolValue;
            showAreaDamageParticleFx = serializedObject.FindProperty("showAreaDamageParticleFx");
            openAreaDamageParticleFx = showAreaDamageParticleFx.boolValue;
            showBfxStickableTransforms = serializedObject.FindProperty("showBfxStickableTransforms");
            openBfxStickableTransforms = showBfxStickableTransforms.boolValue;
            showReferences = serializedObject.FindProperty("showReferences");
            openReferences = showReferences.boolValue;
            showDragAndDropReferences = serializedObject.FindProperty("showDragAndDropReferences");
            openDragAndDropReferences = showDragAndDropReferences.boolValue;
        }

        public void OnInspectorGUI_Tick()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();

            // Enemy Type
            openEnemyTypeEnum = EditorGUILayout.BeginFoldoutHeaderGroup(openEnemyTypeEnum, "Enemy Type");
            if (openEnemyTypeEnum)
            {
                showEnemyTypeEnum.boolValue = true;
                EditorGUILayout.PropertyField(enemyTypeEnum);
                EditorGUILayout.PropertyField(volunReturnAmount);

            }
            else
            {
                showEnemyTypeEnum.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            
            // Weapons
            openEnemyWeapons = EditorGUILayout.BeginFoldoutHeaderGroup(openEnemyWeapons, "Enemy Weapons");
            if (openEnemyWeapons)
            {
                showEnemyWeapons.boolValue = true;

                EditorGUILayout.PropertyField(firstSheathTransform);

                EditorGUILayout.Space(7);
                EditorGUILayout.PropertyField(firstWeapon);
                EditorGUILayout.PropertyField(currentWeapon);
                EditorGUILayout.PropertyField(currentThrowableWeapon);
                EditorGUILayout.PropertyField(firstThrowableWeaponPool);

                EditorGUILayout.Space(7);
                EditorGUILayout.PropertyField(isWeaponOnHand);
                EditorGUILayout.PropertyField(isReacquireThrowableNeeded);
            }
            else
            {
                showEnemyWeapons.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Decisions Variables
            openDecisionsVariables = EditorGUILayout.BeginFoldoutHeaderGroup(openDecisionsVariables, "Decisions Variables");
            if (openDecisionsVariables)
            {
                showDecisionsVariables.boolValue = true;
                DrawModHeader("Aggro Stats.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(aggro_Thershold);
                EditorGUILayout.PropertyField(aggro_ClosestThershold);
                EditorGUILayout.PropertyField(exitAggro_Thershold);
                EditorGUILayout.PropertyField(aggro_Angle);

                DrawModHeader("Deltas.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_frameCount);
                EditorGUILayout.PropertyField(_delta);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Transition Timer.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_aggroTransitTimer);
                EditorGUILayout.PropertyField(_idleTransitTimer);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("CrossFade Layer.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(currentCrossFadeLayer);
                EditorGUILayout.PropertyField(defaultCrossFadeLayer);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showDecisionsVariables.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Angle / Direction / Distance
            openAngleDirectionDistance = EditorGUILayout.BeginFoldoutHeaderGroup(openAngleDirectionDistance, "Angle / Direction / Distance");
            if (openAngleDirectionDistance)
            {
                showAngleDirectionDistance.boolValue = true;
                EditorGUILayout.PropertyField(angleToTarget);
                EditorGUILayout.PropertyField(distanceToTarget);
                EditorGUILayout.PropertyField(dirToTarget);

                EditorGUILayout.Space(7);
                EditorGUILayout.PropertyField(targetPos);

                EditorGUILayout.Space(7);
                EditorGUILayout.PropertyField(mTransform);
            }
            else
            {
                showAngleDirectionDistance.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Overall Health
            openHealth = EditorGUILayout.BeginFoldoutHeaderGroup(openHealth, "Overall Health");
            if (openHealth)
            {
                showHealth.boolValue = true;
                EditorGUILayout.PropertyField(totalEnemyHealth);
                EditorGUILayout.PropertyField(currentEnemyHealth);
            }
            else
            {
                showHealth.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Get Hit Refs.
            openGetHitRefs = EditorGUILayout.BeginFoldoutHeaderGroup(openGetHitRefs, "Hit Source Refs");
            if (openGetHitRefs)
            {
                showGetHitRefs.boolValue = true;
                DrawModSubHeader("Ref.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_hitSourceAttackRefs);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("Result.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_previousHitDamage);
                EditorGUILayout.PropertyField(_hitSourceColliderTransform);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("Status.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_isSkippingOnHitAnim);
                EditorGUILayout.PropertyField(_isHitByChargeAttack);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Knocked Down Refs.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_onHitKnockdownGetupWaitRate);
                EditorGUILayout.PropertyField(_executionGetupWaitRate);
                EditorGUILayout.PropertyField(isKnockedDown);
                EditorGUILayout.PropertyField(_currentGetupWaitRate);
                EditorGUILayout.PropertyField(_getupWaitTimer);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Get Executed Refs.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_previousExecutionDamage);
                EditorGUILayout.PropertyField(_p_executionProfile);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Invincible Refs.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_invincibleResetRate);
                EditorGUILayout.PropertyField(_isInvincible);
                EditorGUILayout.PropertyField(_invincibleResetTimer);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showGetHitRefs.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // AI Attack Refs
            openAIAttackRefs = EditorGUILayout.BeginFoldoutHeaderGroup(openAIAttackRefs, "AI Attack Refs");
            if (openAIAttackRefs)
            {
                showAIAttackRefs.boolValue = true;

                DrawModHeader("Current Element.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(currentElementalType);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Current Attack Refs.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(currentAttackRefs);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showAIAttackRefs.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Current RootMotion Velocity
            openRootMotionVelocity = EditorGUILayout.BeginFoldoutHeaderGroup(openRootMotionVelocity, "Root Motion Velocity");
            if (openRootMotionVelocity)
            {
                showRootMotionVelocity.boolValue = true;

                DrawModHeader("Predict Offsets.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(runningPredictAddonValue);
                EditorGUILayout.PropertyField(currentPlayerPredictOffset);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Attack Root Status.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(ignoreAttackRootMotionCalculate);
                EditorGUILayout.PropertyField(attackMaxVelocityDistance);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Root Velocity.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(currentAttackVelocity);
                EditorGUILayout.PropertyField(currentRollVelocity);
                EditorGUILayout.PropertyField(currentFallbackVelocity);
                EditorGUILayout.PropertyField(currentKnockdownVelocity);
                EditorGUI.indentLevel -= 1;
                
                DrawModHeader("Apply.", modHeaderGUIStyle);
                DrawModSubHeader("Tick.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(applyTurnRootMotion);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("Fixed.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(applyAttackArtifMotion);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showRootMotionVelocity.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            
            // IK Turning.
            openIKTurning = EditorGUILayout.BeginFoldoutHeaderGroup(openIKTurning, "Turn With Agent");
            if (openIKTurning)
            {
                showIKTurning.boolValue = true;

                #region Turning With Anim.
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                openAnimTurningTab = EditorGUILayout.Foldout(openAnimTurningTab, "Turning With Anim", subFoldoutHeaderGUIStyle);
                if (openAnimTurningTab)
                {
                    showAnimTurningTab.boolValue = true;
                    EditorGUI.indentLevel += 1;

                    DrawModHeader("Anim Turning", modHeaderGUIStyle);
                    DrawModSubHeader("Root", modSubHeaderGUIStyle);
                    EditorGUILayout.PropertyField(animRootRotateThershold);
                    EditorGUILayout.PropertyField(rootTurningSpeed);
                    EditorGUI.indentLevel -= 1;

                    DrawModSubHeader("Inplace", modSubHeaderGUIStyle);
                    EditorGUILayout.PropertyField(animInplaceRotateThershold);
                    EditorGUILayout.PropertyField(inplaceTurningSpeed);
                    EditorGUI.indentLevel -= 1;
                    EditorGUI.indentLevel -= 1;
                }
                else
                {
                    showAnimTurningTab.boolValue = false;
                }
                EditorGUI.indentLevel -= 1;
                #endregion

                DrawModHeader("Upper Body IK Turning", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(maneuverAngularSpeed);
                EditorGUILayout.PropertyField(maneuverLookAtPlayerThershold);
                EditorGUILayout.PropertyField(maneuverHeadIKThershold);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Upper Body IK Turning", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(upperBodyIKRotateThershold);
                EditorGUILayout.PropertyField(maxUpperBodyIKTurningSpeed);
                EditorGUILayout.PropertyField(minUpperBodyIKTurningSpeed);
                EditorGUILayout.PropertyField(maxUpperBodyIKTurningSpeedDis);
                EditorGUILayout.PropertyField(minUpperBodyIKTurningSpeedDis);
                EditorGUILayout.PropertyField(currentUpperBodyIKTurningSpeed);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showIKTurning.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Anim With Agent.
            openAnimWithAgentTab = EditorGUILayout.BeginFoldoutHeaderGroup(openAnimWithAgentTab, "Anim With Agent");
            if (openAnimWithAgentTab)
            {
                showAnimWithAgentTab.boolValue = true;

                DrawModHeader("Loco Value", modHeaderGUIStyle);
                DrawModSubHeader("Move Toward", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(locoAnimSwitchDistance);
                EditorGUILayout.PropertyField(closeDistanecLocoAnimValue);
                EditorGUILayout.PropertyField(farDistanceLocoAnimValue);
                EditorGUI.indentLevel -= 1;
                
                DrawModSubHeader("Patrol", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(patrolLocoAnimValue);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showAnimWithAgentTab.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Move With Agent.
            openMoveWithAgentTab = EditorGUILayout.BeginFoldoutHeaderGroup(openMoveWithAgentTab, "Move With Agent");
            if (openMoveWithAgentTab)
            {
                showMoveWithAgentTab.boolValue = true;

                DrawModHeader("Stop Distance", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(agentStopDistance);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Speed", modHeaderGUIStyle);
                DrawModSubHeader("Config", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(agentAccelSpeed);
                EditorGUILayout.PropertyField(agentMoveSpeed);
                EditorGUILayout.PropertyField(closeSpeedBuffer);
                EditorGUI.indentLevel -= 1;
                
                DrawModSubHeader("Status", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_currentAgentAccelSpeed);
                EditorGUILayout.PropertyField(_currentAgentMoveSpeed);
                EditorGUILayout.PropertyField(_currentAgentVelocity);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Predict", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(predictMoveTowardAmount_h);
                EditorGUILayout.PropertyField(predictedMoveTowardDestn);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showMoveWithAgentTab.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // LockOn Move Around.
            openLockOnMoveAroundStats = EditorGUILayout.BeginFoldoutHeaderGroup(openLockOnMoveAroundStats, "LockOn Move Around");
            if (openLockOnMoveAroundStats)
            {
                showLockOnMoveAroundStats.boolValue = true;
                EditorGUILayout.PropertyField(lockOnPosUpdateRate);
                EditorGUILayout.PropertyField(currentAILockonMoveProfile);
                EditorGUILayout.PropertyField(lockOnPosUpdateTimer);
                EditorGUILayout.PropertyField(updateLockOnPos);

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(relatPosAngle);
                EditorGUILayout.PropertyField(currentLockOnLocomotionType);
                EditorGUILayout.PropertyField(targetLockOnPos);
            }
            else
            {
                showLockOnMoveAroundStats.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Attack Interval Stats
            openAttackIntervalStats = EditorGUILayout.BeginFoldoutHeaderGroup(openAttackIntervalStats, "Attack Interval Stats");
            if (openAttackIntervalStats)
            {
                showAttackIntervalStats.boolValue = true;
                EditorGUILayout.PropertyField(maxAttackIntervalRate);
                EditorGUILayout.PropertyField(minAttackIntervalRate);
                EditorGUILayout.PropertyField(enemyAttacked);
                EditorGUILayout.PropertyField(finalizedAttackIntervalRate);
                EditorGUILayout.PropertyField(attackIntervalTimer);
            }
            else
            {
                showAttackIntervalStats.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Parryable Stats
            openParryableStats = EditorGUILayout.BeginFoldoutHeaderGroup(openParryableStats, "Parryable Stats");
            if (openParryableStats)
            {
                showParryableStats.boolValue = true;
                EditorGUILayout.PropertyField(_isParryable);
                EditorGUILayout.PropertyField(_isInParryExecuteWindow);
                EditorGUILayout.PropertyField(_isParryExecutingEnemy);
                EditorGUILayout.PropertyField(_parryExecuteWaitTimer);
            }
            else
            {
                showParryableStats.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            
            // Booleans
            openBooleans = EditorGUILayout.BeginFoldoutHeaderGroup(openBooleans, "Booleans");
            if (openBooleans)
            {
                showBooleans.boolValue = true;
                EditorGUILayout.PropertyField(debugActionHolder);

                DrawModHeader("Init.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(isWeaponUnSheathAnimExecuted);
                EditorGUILayout.PropertyField(isWeaponSheathAnimExecuted);
                EditorGUILayout.PropertyField(isDead);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Moving.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(isMovingToward);
                EditorGUILayout.PropertyField(isMovingTowardPlayer);
                EditorGUILayout.PropertyField(isLockOnMoveAround);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Quit Neglect.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(isMultiStageAttackAvailable);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Turning.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(isTrackingPlayer);
                EditorGUILayout.PropertyField(isPausingTurnWithAgent);
                EditorGUILayout.PropertyField(useInplaceTurningSpeed);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showBooleans.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Action
            openActions = EditorGUILayout.BeginFoldoutHeaderGroup(openActions, "Current Action & Combo & Damage");
            if (openActions)
            {
                showActions.boolValue = true;
                EditorGUILayout.PropertyField(currentPassiveAction);
                EditorGUILayout.PropertyField(currentAction);
                EditorGUILayout.PropertyField(currentMultiStageAttack);
            }
            else
            {
                showActions.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Action Holders
            openActionHolders = EditorGUILayout.BeginFoldoutHeaderGroup(openActionHolders, "First Weapon Action Holder");
            if (openActionHolders)
            {
                showActionHolders.boolValue = true;
                EditorGUILayout.PropertyField(firstWeaponActionHolder);
                EditorGUILayout.PropertyField(currentActionHolder);
                EditorGUILayout.PropertyField(skippingScoreCalculation);
            }
            else
            {
                showActionHolders.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Area Damage Effect Fx.
            openAreaDamageParticleFx = EditorGUILayout.BeginFoldoutHeaderGroup(openAreaDamageParticleFx, "Current Area Damage Particle Fx");
            if (openAreaDamageParticleFx)
            {
                showAreaDamageParticleFx.boolValue = true;
                EditorGUILayout.PropertyField(_currentDamageParticle);
            }
            else
            {
                showAreaDamageParticleFx.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Bfx Stickable Transform.
            openBfxStickableTransforms = EditorGUILayout.BeginFoldoutHeaderGroup(openBfxStickableTransforms, "BloodFx Stickable Transforms");
            if (openBfxStickableTransforms)
            {
                showBfxStickableTransforms.boolValue = true;

                DrawModHeader("Current Stickable.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_BfxStickableTrans);
                EditorGUILayout.PropertyField(_cur_PoolableBfxHandler);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showBfxStickableTransforms.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Refs
            openReferences = EditorGUILayout.BeginFoldoutHeaderGroup(openReferences, "Refs");
            if (openReferences)
            {
                showReferences.boolValue = true;
                EditorGUILayout.PropertyField(aiStates);
                EditorGUILayout.PropertyField(playerStates);
                EditorGUILayout.PropertyField(hashManager);
                EditorGUILayout.PropertyField(anim);
                EditorGUILayout.PropertyField(a_hook);
                EditorGUILayout.PropertyField(iKHandler);
                EditorGUILayout.PropertyField(agent);
            }
            else
            {
                showReferences.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // Drag and Drop Refs
            openDragAndDropReferences = EditorGUILayout.BeginFoldoutHeaderGroup(openDragAndDropReferences, "Drag and Drop Refs");
            if (openDragAndDropReferences)
            {
                showDragAndDropReferences.boolValue = true;
                EditorGUILayout.PropertyField(profile);
            }
            else
            {
                showDragAndDropReferences.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            DrawUILine2();

            EditorGUILayout.EndVertical();
        }

        public void InitModHeaderGUIStyle()
        {
            if (modHeaderGUIStyle == null)
            {
                modHeaderGUIStyle = new GUIStyle();
                modHeaderGUIStyle.fontStyle = FontStyle.Bold;
                modHeaderGUIStyle.fontSize = 12;
                Color32 headerColor = new Color32(235, 89, 110, 255);
                modHeaderGUIStyle.normal.textColor = headerColor;
            }
        }

        public void InitModSubHeaderGUIStyle()
        {
            if (modSubHeaderGUIStyle == null)
            {
                modSubHeaderGUIStyle = new GUIStyle();
                modSubHeaderGUIStyle.fontStyle = FontStyle.Bold;
                modSubHeaderGUIStyle.fontSize = 12;
                Color32 subHeaderColor = new Color32(245, 172, 183, 255);
                modSubHeaderGUIStyle.normal.textColor = subHeaderColor;
            }
        }

        public void InitClassFoldoutHeaderGUIStyle()
        {
            if (classFoldoutHeaderGUIStyle == null)
            {
                classFoldoutHeaderGUIStyle = new GUIStyle(EditorStyles.foldoutHeader);
                Color32 foldoutHeaderColor = new Color32(186, 115, 211, 255); // 176,224,230 //210, 105, 30
                classFoldoutHeaderGUIStyle.onHover.textColor = foldoutHeaderColor;
                classFoldoutHeaderGUIStyle.hover.textColor = foldoutHeaderColor;
                classFoldoutHeaderGUIStyle.normal.textColor = foldoutHeaderColor;
                classFoldoutHeaderGUIStyle.onNormal.textColor = foldoutHeaderColor;
                classFoldoutHeaderGUIStyle.onFocused.textColor = foldoutHeaderColor;
                classFoldoutHeaderGUIStyle.focused.textColor = foldoutHeaderColor;
            }
        }

        public void InitFoldoutHeaderGUIStyle()
        {
            if (foldoutHeaderGUIStyle == null)
            {
                foldoutHeaderGUIStyle = new GUIStyle(EditorStyles.foldoutHeader);
                Color32 foldoutHeaderColor = new Color32(255, 199, 135, 255);
                foldoutHeaderGUIStyle.onHover.textColor = foldoutHeaderColor;
                foldoutHeaderGUIStyle.hover.textColor = foldoutHeaderColor;
                foldoutHeaderGUIStyle.normal.textColor = foldoutHeaderColor;
                foldoutHeaderGUIStyle.onNormal.textColor = foldoutHeaderColor;
                foldoutHeaderGUIStyle.onFocused.textColor = foldoutHeaderColor;
                foldoutHeaderGUIStyle.focused.textColor = foldoutHeaderColor;
            }
        }

        public void InitSubFoldoutHeaderGUIStyle()
        {
            if (subFoldoutHeaderGUIStyle == null)
            {
                subFoldoutHeaderGUIStyle = new GUIStyle(EditorStyles.foldoutHeader);
                Color32 foldoutHeaderColor = new Color32(255, 128, 58, 255);
                subFoldoutHeaderGUIStyle.onHover.textColor = foldoutHeaderColor;
                subFoldoutHeaderGUIStyle.hover.textColor = foldoutHeaderColor;
                subFoldoutHeaderGUIStyle.normal.textColor = foldoutHeaderColor;
                subFoldoutHeaderGUIStyle.onNormal.textColor = foldoutHeaderColor;
                subFoldoutHeaderGUIStyle.onFocused.textColor = foldoutHeaderColor;
                subFoldoutHeaderGUIStyle.focused.textColor = foldoutHeaderColor;
            }
        }
        #endregion

        #region Stamina Usage Mods

        bool openStaminaUsageMod;
        SerializedProperty showStaminaUsageMod;

        SerializedProperty staminaReturnSpeed;
        SerializedProperty stdStaminaAmount;
        SerializedProperty staminaRandomizeAmount;
        SerializedProperty isEnemyTired;
        SerializedProperty currentEnemyStamina;
        SerializedProperty finalizedStaminaAmount;

        public void StaminaUsageMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty staminaUsageMod = specializeClassSerializedObject.FindProperty("staminaUsageMod");
            staminaReturnSpeed = staminaUsageMod.FindPropertyRelative("staminaReturnSpeed");
            stdStaminaAmount = staminaUsageMod.FindPropertyRelative("stdStaminaAmount");
            staminaRandomizeAmount = staminaUsageMod.FindPropertyRelative("staminaRandomizeAmount");
            isEnemyTired = staminaUsageMod.FindPropertyRelative("isEnemyTired");
            currentEnemyStamina = staminaUsageMod.FindPropertyRelative("currentEnemyStamina");
            finalizedStaminaAmount = staminaUsageMod.FindPropertyRelative("finalizedStaminaAmount");

            showStaminaUsageMod = staminaUsageMod.FindPropertyRelative("showStaminaUsageMod");
            openStaminaUsageMod = showStaminaUsageMod.boolValue;
        }

        public void StaminaUsageMod_Tick()
        {
            EditorGUILayout.BeginVertical();

            openStaminaUsageMod = EditorGUILayout.BeginFoldoutHeaderGroup(openStaminaUsageMod, "Stamina Usage Mod", foldoutHeaderGUIStyle);
            if (openStaminaUsageMod)
            {
                showStaminaUsageMod.boolValue = true;

                DrawModHeader("Configuration", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(staminaReturnSpeed);
                EditorGUILayout.PropertyField(stdStaminaAmount);
                EditorGUILayout.PropertyField(staminaRandomizeAmount);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(isEnemyTired);
                EditorGUILayout.PropertyField(currentEnemyStamina);
                EditorGUILayout.PropertyField(finalizedStaminaAmount);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showStaminaUsageMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();
        }

        #endregion

        #region Roll Interval Mods

        bool openRollIntervalMod;
        SerializedProperty showRollIntervalMod;

        SerializedProperty stdRollIntervalRate;
        SerializedProperty isRollIntervalRateRandomized;
        SerializedProperty rollIntervalRandomizeAmount;
        SerializedProperty checkEnemyRolledInit;
        SerializedProperty enemyRolled;
        SerializedProperty finalizedRollIntervalTime;

        public void RollIntervalMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty rollIntervalMod = specializeClassSerializedObject.FindProperty("rollIntervalMod");
            stdRollIntervalRate = rollIntervalMod.FindPropertyRelative("stdRollIntervalRate");
            isRollIntervalRateRandomized = rollIntervalMod.FindPropertyRelative("isRandomized");
            rollIntervalRandomizeAmount = rollIntervalMod.FindPropertyRelative("rollIntervalRandomizeAmount");
            checkEnemyRolledInit = rollIntervalMod.FindPropertyRelative("checkEnemyRolledInit");
            enemyRolled = rollIntervalMod.FindPropertyRelative("enemyRolled");
            finalizedRollIntervalTime = rollIntervalMod.FindPropertyRelative("finalizedRollIntervalTime");
            showRollIntervalMod = rollIntervalMod.FindPropertyRelative("showRollIntervalMod");
            openRollIntervalMod = showRollIntervalMod.boolValue;
        }

        public void RollIntervalMod_Tick()
        {
            openRollIntervalMod = EditorGUILayout.BeginFoldoutHeaderGroup(openRollIntervalMod, "Roll Interval Mod", foldoutHeaderGUIStyle);
            if (openRollIntervalMod)
            {
                showRollIntervalMod.boolValue = true;

                DrawModHeader("Configuration", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(stdRollIntervalRate);
                EditorGUILayout.PropertyField(isRollIntervalRateRandomized);
                EditorGUILayout.PropertyField(rollIntervalRandomizeAmount);
                EditorGUILayout.PropertyField(checkEnemyRolledInit);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(enemyRolled);
                EditorGUILayout.PropertyField(finalizedRollIntervalTime);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showRollIntervalMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Fix Direction Move Mods

        bool openMoveInFixDirectionMod;
        SerializedProperty showFixDirectionMoveMod;

        SerializedProperty stdFixDirectionMoveRate;
        SerializedProperty fixDirectionMoveRateRandomizeAmount;
        SerializedProperty stdFixDirectionWaitRate;
        SerializedProperty fixDirectionWaitRateRandomizeAmount;

        SerializedProperty targetMoveDirection;
        SerializedProperty calculatedFixDirection;
        SerializedProperty finalizedFixDirectionMoveTime;
        SerializedProperty finalizedFixDirectionWaitTime;
        SerializedProperty applyFixDirMoveRootMotion;
        SerializedProperty isMovingInFixDirection;
        SerializedProperty isFixDirectionInCooldown;
        SerializedProperty isHitByChargeAttackWhenMoving;

        public void MoveInFixDirectionMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty moveInFixDirectionMod = specializeClassSerializedObject.FindProperty("fixDirectionMoveMod");
            targetMoveDirection = moveInFixDirectionMod.FindPropertyRelative("targetMoveDirection");
            stdFixDirectionMoveRate = moveInFixDirectionMod.FindPropertyRelative("stdFixDirectionMoveRate");
            fixDirectionMoveRateRandomizeAmount = moveInFixDirectionMod.FindPropertyRelative("fixDirectionMoveRateRandomizeAmount");
            stdFixDirectionWaitRate = moveInFixDirectionMod.FindPropertyRelative("stdFixDirectionWaitRate");
            fixDirectionWaitRateRandomizeAmount = moveInFixDirectionMod.FindPropertyRelative("fixDirectionWaitRateRandomizeAmount");

            calculatedFixDirection = moveInFixDirectionMod.FindPropertyRelative("calculatedFixDirection");
            finalizedFixDirectionMoveTime = moveInFixDirectionMod.FindPropertyRelative("finalizedFixDirectionMoveTime");
            finalizedFixDirectionWaitTime = moveInFixDirectionMod.FindPropertyRelative("finalizedFixDirectionWaitTime");
            applyFixDirMoveRootMotion = moveInFixDirectionMod.FindPropertyRelative("applyFixDirMoveRootMotion");
            isMovingInFixDirection = moveInFixDirectionMod.FindPropertyRelative("isMovingInFixDirection");
            isFixDirectionInCooldown = moveInFixDirectionMod.FindPropertyRelative("isFixDirectionInCooldown");
            isHitByChargeAttackWhenMoving = moveInFixDirectionMod.FindPropertyRelative("isHitByChargeAttackWhenMoving");

            showFixDirectionMoveMod = moveInFixDirectionMod.FindPropertyRelative("showFixDirectionMoveMod");
            openMoveInFixDirectionMod = showFixDirectionMoveMod.boolValue;
        }

        public void MoveInFixDirectionMod_Tick()
        {
            openMoveInFixDirectionMod = EditorGUILayout.BeginFoldoutHeaderGroup(openMoveInFixDirectionMod, "Fix Direction Move Mod", foldoutHeaderGUIStyle);
            if (openMoveInFixDirectionMod)
            {
                showFixDirectionMoveMod.boolValue = true;

                DrawModHeader("Config", modHeaderGUIStyle);

                DrawModSubHeader("Target Dir.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(targetMoveDirection);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("Move Rate.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(stdFixDirectionMoveRate);
                EditorGUILayout.PropertyField(fixDirectionMoveRateRandomizeAmount);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("Cooldown Rate.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(stdFixDirectionWaitRate);
                EditorGUILayout.PropertyField(fixDirectionWaitRateRandomizeAmount);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Status", modHeaderGUIStyle);
                DrawModSubHeader("Direction.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(calculatedFixDirection);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("finalized Rates.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(finalizedFixDirectionMoveTime);
                EditorGUILayout.PropertyField(finalizedFixDirectionWaitTime);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("Status.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(applyFixDirMoveRootMotion);
                EditorGUILayout.PropertyField(isMovingInFixDirection);
                EditorGUILayout.PropertyField(isFixDirectionInCooldown);
                EditorGUILayout.PropertyField(isHitByChargeAttackWhenMoving);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showFixDirectionMoveMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }

        #endregion

        #region Enemy Blocking Mods

        bool openEnemyBlockingMod;
        SerializedProperty showEnemyBlockingMod;

        SerializedProperty isBlockingWithoutAction;
        SerializedProperty checkEnemyBlockedInit;
        SerializedProperty stdBlockingStaminaAmount;
        SerializedProperty blockingStaminaRandomizeAmount;
        SerializedProperty maxBlockingWaitRate;
        SerializedProperty minBlockingWaitRate;
        SerializedProperty initialBlockingWaitRate;
        SerializedProperty maxUpperBodyIKBlockingTurnSpeed;
        SerializedProperty blockingAttackIntervalRate;
        SerializedProperty blockingAttackIntervalRandomizeAmount;
        SerializedProperty damageReductionWhenBlocking;
        SerializedProperty blockingDuabilityValue;
        SerializedProperty currentBlockingDuabilityValue;
        SerializedProperty isEnemyBlocking;
        SerializedProperty isInAttackBlocking;
        SerializedProperty enemyBlocked;
        SerializedProperty currentBlockingStamina;
        SerializedProperty finalizedBlockingStaminaAmount;
        SerializedProperty finalizedBlockingWaitTime;

        public void EnemyBlockingMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty enemyBlockingMod = specializeClassSerializedObject.FindProperty("enemyBlockingMod");
            isBlockingWithoutAction = enemyBlockingMod.FindPropertyRelative("isBlockingWithoutAction");
            checkEnemyBlockedInit = enemyBlockingMod.FindPropertyRelative("checkEnemyBlockedInit");
            stdBlockingStaminaAmount = enemyBlockingMod.FindPropertyRelative("stdBlockingStaminaAmount");
            blockingStaminaRandomizeAmount = enemyBlockingMod.FindPropertyRelative("blockingStaminaRandomizeAmount");
            maxBlockingWaitRate = enemyBlockingMod.FindPropertyRelative("maxBlockingWaitRate");
            minBlockingWaitRate = enemyBlockingMod.FindPropertyRelative("minBlockingWaitRate");
            initialBlockingWaitRate = enemyBlockingMod.FindPropertyRelative("initialBlockingWaitRate");
            maxUpperBodyIKBlockingTurnSpeed = enemyBlockingMod.FindPropertyRelative("maxUpperBodyIKBlockingTurnSpeed");
            blockingAttackIntervalRate = enemyBlockingMod.FindPropertyRelative("blockingAttackIntervalRate");
            blockingAttackIntervalRandomizeAmount = enemyBlockingMod.FindPropertyRelative("blockingAttackIntervalRandomizeAmount");
            damageReductionWhenBlocking = enemyBlockingMod.FindPropertyRelative("damageReductionWhenBlocking");
            blockingDuabilityValue = enemyBlockingMod.FindPropertyRelative("blockingDuabilityValue");
            currentBlockingDuabilityValue = enemyBlockingMod.FindPropertyRelative("currentBlockingDuabilityValue");
            isEnemyBlocking = enemyBlockingMod.FindPropertyRelative("isEnemyBlocking");
            isInAttackBlocking = enemyBlockingMod.FindPropertyRelative("isInAttackBlocking");
            enemyBlocked = enemyBlockingMod.FindPropertyRelative("enemyBlocked");
            currentBlockingStamina = enemyBlockingMod.FindPropertyRelative("currentBlockingStamina");
            finalizedBlockingStaminaAmount = enemyBlockingMod.FindPropertyRelative("finalizedBlockingStaminaAmount");
            finalizedBlockingWaitTime = enemyBlockingMod.FindPropertyRelative("finalizedBlockingWaitTime");

            showEnemyBlockingMod = enemyBlockingMod.FindPropertyRelative("showEnemyBlockingMod");
            openEnemyBlockingMod = showEnemyBlockingMod.boolValue;
        }

        public void EnemyBlockingMod_Tick()
        {
            openEnemyBlockingMod = EditorGUILayout.BeginFoldoutHeaderGroup(openEnemyBlockingMod, "Enemy Blocking Mod", foldoutHeaderGUIStyle);
            if (openEnemyBlockingMod)
            {
                showEnemyBlockingMod.boolValue = true;
                
                DrawModHeader("Blocking Stamina.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(isBlockingWithoutAction);
                EditorGUILayout.PropertyField(checkEnemyBlockedInit);
                EditorGUILayout.PropertyField(stdBlockingStaminaAmount);
                EditorGUILayout.PropertyField(blockingStaminaRandomizeAmount);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Wait Rate.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(maxBlockingWaitRate);
                EditorGUILayout.PropertyField(minBlockingWaitRate);
                EditorGUILayout.PropertyField(initialBlockingWaitRate);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Turn Speed.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(maxUpperBodyIKBlockingTurnSpeed);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Attack Rate.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(blockingAttackIntervalRate);
                EditorGUILayout.PropertyField(blockingAttackIntervalRandomizeAmount);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("On Hit", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(damageReductionWhenBlocking);
                EditorGUILayout.PropertyField(blockingDuabilityValue);
                EditorGUILayout.PropertyField(currentBlockingDuabilityValue);
                EditorGUI.indentLevel -= 1;
                
                DrawModHeader("Mod Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(isEnemyBlocking);
                EditorGUILayout.PropertyField(isInAttackBlocking);
                EditorGUILayout.PropertyField(enemyBlocked);
                EditorGUILayout.PropertyField(currentBlockingStamina);
                EditorGUILayout.PropertyField(finalizedBlockingStaminaAmount);
                EditorGUILayout.PropertyField(finalizedBlockingWaitTime);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showEnemyBlockingMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }

        #endregion

        #region Hit Counting Mods

        bool openHitCountingMod;
        SerializedProperty showHitCountingMod;

        SerializedProperty cancelHitRate;
        SerializedProperty countHitTimer;
        SerializedProperty hitCount;
        SerializedProperty isHitCountEventTriggered;

        public void HitCountingMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty hitCountingMod = specializeClassSerializedObject.FindProperty("hitCountingMod");
            cancelHitRate = hitCountingMod.FindPropertyRelative("cancelHitRate");
            countHitTimer = hitCountingMod.FindPropertyRelative("countHitTimer");
            hitCount = hitCountingMod.FindPropertyRelative("hitCount");
            isHitCountEventTriggered = hitCountingMod.FindPropertyRelative("isHitCountEventTriggered");
            showHitCountingMod = hitCountingMod.FindPropertyRelative("showHitCountingMod");
            openHitCountingMod = showHitCountingMod.boolValue;
        }

        public void HitCountingMod_Tick()
        {
            EditorGUILayout.BeginVertical();

            openHitCountingMod = EditorGUILayout.BeginFoldoutHeaderGroup(openHitCountingMod, "Hit Counting Mod", foldoutHeaderGUIStyle);
            if (openHitCountingMod)
            {
                showHitCountingMod.boolValue = true;

                DrawModHeader("Configuration", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(cancelHitRate);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(countHitTimer);
                EditorGUILayout.PropertyField(hitCount);
                EditorGUILayout.PropertyField(isHitCountEventTriggered);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showHitCountingMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();
        }

        #endregion
        
        #region Aiming Player Mods

        bool openAimingPlayerMod;
        SerializedProperty showAimingPlayerMod;

        SerializedProperty throwAimingProjectile;
        SerializedProperty aimingHandPosition;
        SerializedProperty aimingRate;
        SerializedProperty aimingQuitDistance;
        SerializedProperty aimingTimer;
        SerializedProperty isAiming;

        public void AimingPlayerMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty aimingPlayerMod = specializeClassSerializedObject.FindProperty("aimingPlayerMod");
            throwAimingProjectile = aimingPlayerMod.FindPropertyRelative("throwAimingProjectile");
            aimingHandPosition = aimingPlayerMod.FindPropertyRelative("aimingHandPosition");
            aimingRate = aimingPlayerMod.FindPropertyRelative("aimingRate");
            aimingQuitDistance = aimingPlayerMod.FindPropertyRelative("aimingQuitDistance");
            aimingTimer = aimingPlayerMod.FindPropertyRelative("aimingTimer");
            isAiming = aimingPlayerMod.FindPropertyRelative("isAiming");

            showAimingPlayerMod = aimingPlayerMod.FindPropertyRelative("showAimingPlayerMod");
            openAimingPlayerMod = showAimingPlayerMod.boolValue;
        }

        public void AimingPlayerMod_Tick()
        {
            openAimingPlayerMod = EditorGUILayout.BeginFoldoutHeaderGroup(openAimingPlayerMod, "Aiming Player Mod", foldoutHeaderGUIStyle);
            if (openAimingPlayerMod)
            {
                showAimingPlayerMod.boolValue = true;
                
                DrawModHeader("Configuration", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(throwAimingProjectile);
                EditorGUILayout.PropertyField(aimingHandPosition);
                EditorGUILayout.PropertyField(aimingRate);
                EditorGUILayout.PropertyField(aimingQuitDistance);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(aimingTimer);
                EditorGUILayout.PropertyField(isAiming);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showAimingPlayerMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }

        #endregion

        #region Enemy Taunt Mods

        bool openEnemyTauntMod;
        SerializedProperty showEnemyTauntMod;

        SerializedProperty stdTauntRate;
        SerializedProperty tauntRateRandomizeAmount;
        SerializedProperty checkTauntedPlayerInit;
        SerializedProperty tauntedPlayer;
        SerializedProperty finalizedTauntTime;

        public void EnemyTauntMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty enemyTauntMod = specializeClassSerializedObject.FindProperty("enemyTauntMod");
            stdTauntRate = enemyTauntMod.FindPropertyRelative("stdTauntRate");
            tauntRateRandomizeAmount = enemyTauntMod.FindPropertyRelative("tauntRateRandomizeAmount");
            checkTauntedPlayerInit = enemyTauntMod.FindPropertyRelative("checkTauntedPlayerInit");
            tauntedPlayer = enemyTauntMod.FindPropertyRelative("tauntedPlayer");
            finalizedTauntTime = enemyTauntMod.FindPropertyRelative("finalizedTauntTime");
            showEnemyTauntMod = enemyTauntMod.FindPropertyRelative("showEnemyTauntMod");
            openEnemyTauntMod = showEnemyTauntMod.boolValue;
        }

        public void EnemyTauntMod_Tick()
        {
            openEnemyTauntMod = EditorGUILayout.BeginFoldoutHeaderGroup(openEnemyTauntMod, "Enemy Taunt Mod", foldoutHeaderGUIStyle);
            if (openEnemyTauntMod)
            {
                showEnemyTauntMod.boolValue = true;

                DrawModHeader("Configuration", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(stdTauntRate);
                EditorGUILayout.PropertyField(tauntRateRandomizeAmount);
                EditorGUILayout.PropertyField(checkTauntedPlayerInit);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(tauntedPlayer);
                EditorGUILayout.PropertyField(finalizedTauntTime);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showEnemyTauntMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }

        #endregion

        #region Two Stance Combat Mods

        bool openTwoStanceCombatMod;
        SerializedProperty showTwoStanceCombatMod;

        SerializedProperty _changeStanceMaxRate;
        SerializedProperty _changeStanceMinRate;
        SerializedProperty _changeStanceTimer;
        SerializedProperty _finalizedChangeStanceRate;

        SerializedProperty isRightStance;

        public void TwoStanceCombatMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty twoStanceCombatMod = specializeClassSerializedObject.FindProperty("twoStanceCombatMod");

            _changeStanceMaxRate = twoStanceCombatMod.FindPropertyRelative("_changeStanceMaxRate");
            _changeStanceMinRate = twoStanceCombatMod.FindPropertyRelative("_changeStanceMinRate");
            _changeStanceTimer = twoStanceCombatMod.FindPropertyRelative("_changeStanceTimer");
            _finalizedChangeStanceRate = twoStanceCombatMod.FindPropertyRelative("_finalizedChangeStanceRate");

            isRightStance = twoStanceCombatMod.FindPropertyRelative("isRightStance");

            showTwoStanceCombatMod = twoStanceCombatMod.FindPropertyRelative("showTwoStanceCombatMod");
            openTwoStanceCombatMod = showTwoStanceCombatMod.boolValue;
        }

        public void TwoStanceCombatMod_Tick()
        {
            openTwoStanceCombatMod = EditorGUILayout.BeginFoldoutHeaderGroup(openTwoStanceCombatMod, "Two Stance Combat Mod", foldoutHeaderGUIStyle);
            if (openTwoStanceCombatMod)
            {
                showTwoStanceCombatMod.boolValue = true;

                DrawModHeader("Change Stance Rate.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_changeStanceMaxRate);
                EditorGUILayout.PropertyField(_changeStanceMinRate);
                EditorGUILayout.PropertyField(_changeStanceTimer);
                EditorGUILayout.PropertyField(_finalizedChangeStanceRate);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(isRightStance);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showTwoStanceCombatMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }

        #endregion

        #region Parry Player Mods

        bool openParryPlayerMod;
        SerializedProperty showParryPlayerMod;

        SerializedProperty maxParryCooldownRate;
        SerializedProperty minParryCooldownRate;

        SerializedProperty maxParryWaitTime;
        SerializedProperty minParryWaitTime;

        SerializedProperty maxUpperBodyIKParryTurnSpeed;
        //SerializedProperty parryMetalImpactSmallPool;

        SerializedProperty rSParryAttack1ReadyAction;
        SerializedProperty lSParryAttack1ReadyAction;

        SerializedProperty finalizedParryCooldownRate;
        SerializedProperty finalizedParryWaitTime;
        SerializedProperty isInParryCooldown;
        SerializedProperty isParryWaiting;
        SerializedProperty isPlayParryFxNeeded;
        SerializedProperty isPlayerAttackBaited;

        public void ParryPlayerMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty parryPlayerMod = specializeClassSerializedObject.FindProperty("parryPlayerMod");

            maxParryCooldownRate = parryPlayerMod.FindPropertyRelative("maxParryCooldownRate");
            minParryCooldownRate = parryPlayerMod.FindPropertyRelative("minParryCooldownRate");

            maxParryWaitTime = parryPlayerMod.FindPropertyRelative("maxParryWaitTime");
            minParryWaitTime = parryPlayerMod.FindPropertyRelative("minParryWaitTime");
            //parryMetalImpactSmallPool = parryPlayerMod.FindPropertyRelative("metalImpactSmallPool");

            maxUpperBodyIKParryTurnSpeed = parryPlayerMod.FindPropertyRelative("maxUpperBodyIKParryTurnSpeed");

            rSParryAttack1ReadyAction = parryPlayerMod.FindPropertyRelative("rSParryAttack1ReadyAction");
            lSParryAttack1ReadyAction = parryPlayerMod.FindPropertyRelative("lSParryAttack1ReadyAction");

            finalizedParryCooldownRate = parryPlayerMod.FindPropertyRelative("finalizedParryCooldownRate");
            finalizedParryWaitTime = parryPlayerMod.FindPropertyRelative("finalizedParryWaitTime");
            isInParryCooldown = parryPlayerMod.FindPropertyRelative("isInParryCooldown");
            isParryWaiting = parryPlayerMod.FindPropertyRelative("isParryWaiting");
            isPlayParryFxNeeded = parryPlayerMod.FindPropertyRelative("isPlayParryFxNeeded");
            isPlayerAttackBaited = parryPlayerMod.FindPropertyRelative("isPlayerAttackBaited");

            showParryPlayerMod = parryPlayerMod.FindPropertyRelative("showParryPlayerMod");
            openParryPlayerMod = showParryPlayerMod.boolValue;
        }

        public void ParryPlayerMod_Tick()
        {
            openParryPlayerMod = EditorGUILayout.BeginFoldoutHeaderGroup(openParryPlayerMod, "Parry Player Mod", foldoutHeaderGUIStyle);
            if (openParryPlayerMod)
            {
                showParryPlayerMod.boolValue = true;

                DrawModHeader("CoolDown Rate", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(maxParryCooldownRate);
                EditorGUILayout.PropertyField(minParryCooldownRate);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Parry Wait Time", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(maxParryWaitTime);
                EditorGUILayout.PropertyField(minParryWaitTime);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Turn Speed", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(maxUpperBodyIKParryTurnSpeed);
                EditorGUI.indentLevel -= 1;
                //DrawModHeader("VFX Particle", modHeaderGUIStyle);
                //EditorGUILayout.PropertyField(parryMetalImpactSmallPool);
                //EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Actions", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(rSParryAttack1ReadyAction);
                EditorGUILayout.PropertyField(lSParryAttack1ReadyAction);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(finalizedParryCooldownRate);
                EditorGUILayout.PropertyField(finalizedParryWaitTime);
                EditorGUILayout.PropertyField(isInParryCooldown);
                EditorGUILayout.PropertyField(isParryWaiting);
                EditorGUILayout.PropertyField(isPlayParryFxNeeded);
                EditorGUILayout.PropertyField(isPlayerAttackBaited);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showParryPlayerMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }

        #endregion

        #region Perilous Attack Mods

        bool openPerilousAttackMod;
        SerializedProperty showPerilousAttackMod;

        SerializedProperty stdPerilousAttackRate;
        SerializedProperty perilousAttackIsRandomized;
        SerializedProperty perilousAttackRandomizeAmount;
        SerializedProperty usedPerilousAttack;
        SerializedProperty finalizedPerilousAttackTime;
        SerializedProperty perilousATKIndicator;

        public void PerilousAttackMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty perilousAttackMod = specializeClassSerializedObject.FindProperty("perilousAttackMod");

            stdPerilousAttackRate = perilousAttackMod.FindPropertyRelative("stdPerilousAttackRate");
            perilousAttackIsRandomized = perilousAttackMod.FindPropertyRelative("isRandomized");
            perilousAttackRandomizeAmount = perilousAttackMod.FindPropertyRelative("perilousAttackRandomizeAmount");
            usedPerilousAttack = perilousAttackMod.FindPropertyRelative("usedPerilousAttack");
            finalizedPerilousAttackTime = perilousAttackMod.FindPropertyRelative("finalizedPerilousAttackTime");
            perilousATKIndicator = perilousAttackMod.FindPropertyRelative("perilousATKIndicator");

            showPerilousAttackMod = perilousAttackMod.FindPropertyRelative("showPerilousAttackMod");
            openPerilousAttackMod = showPerilousAttackMod.boolValue;
        }

        public void PerilousAttackMod_Tick()
        {
            openPerilousAttackMod = EditorGUILayout.BeginFoldoutHeaderGroup(openPerilousAttackMod, "Perilous Attack Mod", foldoutHeaderGUIStyle);
            if (openPerilousAttackMod)
            {
                showPerilousAttackMod.boolValue = true;

                DrawModHeader("Configuration", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(stdPerilousAttackRate);
                EditorGUILayout.PropertyField(perilousAttackIsRandomized);
                EditorGUILayout.PropertyField(perilousAttackRandomizeAmount);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Indicator", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(perilousATKIndicator);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(usedPerilousAttack);
                EditorGUILayout.PropertyField(finalizedPerilousAttackTime);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showPerilousAttackMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Player Spam Blocking Mods

        bool openPlayerSpamBlockingMod;
        SerializedProperty showPlayerSpamBlockingMod;

        // Blocking
        SerializedProperty markAsSpammedBlockingRate;
        SerializedProperty maxBlockingCounterAmount;
        SerializedProperty blockingCounterDelepteRate;
        SerializedProperty playerBlockingCounter;
        SerializedProperty hasSpammedBlocking;
        
        public void PlayerSpamBlockingMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty playerSpamBlockingMod = specializeClassSerializedObject.FindProperty("playerSpamBlockingMod");

            markAsSpammedBlockingRate = playerSpamBlockingMod.FindPropertyRelative("markAsSpammedBlockingRate");
            maxBlockingCounterAmount = playerSpamBlockingMod.FindPropertyRelative("maxBlockingCounterAmount");
            blockingCounterDelepteRate = playerSpamBlockingMod.FindPropertyRelative("blockingCounterDelepteRate");
            playerBlockingCounter = playerSpamBlockingMod.FindPropertyRelative("playerBlockingCounter");
            hasSpammedBlocking = playerSpamBlockingMod.FindPropertyRelative("hasSpammedBlocking");
            
            showPlayerSpamBlockingMod = playerSpamBlockingMod.FindPropertyRelative("showPlayerSpamBlockingMod");
            openPlayerSpamBlockingMod = showPlayerSpamBlockingMod.boolValue;
        }

        public void PlayerSpamBlockingMod_Tick()
        {
            openPlayerSpamBlockingMod = EditorGUILayout.BeginFoldoutHeaderGroup(openPlayerSpamBlockingMod, "Player Spam Blocking Mod", foldoutHeaderGUIStyle);
            if (openPlayerSpamBlockingMod)
            {
                showPlayerSpamBlockingMod.boolValue = true;
                
                DrawModHeader("Config", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(markAsSpammedBlockingRate);
                EditorGUILayout.PropertyField(maxBlockingCounterAmount);
                EditorGUILayout.PropertyField(blockingCounterDelepteRate);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(playerBlockingCounter);
                EditorGUILayout.PropertyField(hasSpammedBlocking);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showPlayerSpamBlockingMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }

        #endregion
        
        #region Player Spam Attack Mod.

        bool openPlayerSpamAttackMod;
        SerializedProperty showPlayerSpamAttackMod;
        
        // Attacking
        SerializedProperty markAsSpammedAttackingRate;
        SerializedProperty maxAttackingCounterAmount;
        SerializedProperty attackingCounterDelepteRate;
        SerializedProperty playerAttackingCounter;
        SerializedProperty hasSpammedAttacking;

        public void PlayerSpamAttackMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty playerSpamAttackMod = specializeClassSerializedObject.FindProperty("playerSpamAttackMod");
            
            markAsSpammedAttackingRate = playerSpamAttackMod.FindPropertyRelative("markAsSpammedAttackingRate");
            maxAttackingCounterAmount = playerSpamAttackMod.FindPropertyRelative("maxAttackingCounterAmount");
            attackingCounterDelepteRate = playerSpamAttackMod.FindPropertyRelative("attackingCounterDelepteRate");
            playerAttackingCounter = playerSpamAttackMod.FindPropertyRelative("playerAttackingCounter");
            hasSpammedAttacking = playerSpamAttackMod.FindPropertyRelative("hasSpammedAttacking");

            showPlayerSpamAttackMod = playerSpamAttackMod.FindPropertyRelative("showPlayerSpamAttackMod");
            openPlayerSpamAttackMod = showPlayerSpamAttackMod.boolValue;
        }

        public void PlayerSpamAttackMod_Tick()
        {
            openPlayerSpamAttackMod = EditorGUILayout.BeginFoldoutHeaderGroup(openPlayerSpamAttackMod, "Player Spam Attack Mod", foldoutHeaderGUIStyle);
            if (openPlayerSpamAttackMod)
            {
                showPlayerSpamAttackMod.boolValue = true;

                DrawModHeader("Config", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(markAsSpammedAttackingRate);
                EditorGUILayout.PropertyField(maxAttackingCounterAmount);
                EditorGUILayout.PropertyField(attackingCounterDelepteRate);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(playerAttackingCounter);
                EditorGUILayout.PropertyField(hasSpammedAttacking);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showPlayerSpamAttackMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }

        #endregion

        #region Dual Weapon Mods

        bool openDualWeaponMod;
        SerializedProperty showDualWeaponMod;

        // Configuration
        SerializedProperty switchDistance;
        SerializedProperty switchDistanceBuffer;

        // Second Weapon Info
        SerializedProperty secondWeaponId;
        SerializedProperty secondSheathTransform;
        SerializedProperty secondWeaponActionHolder;
        
        // Mod Status
        SerializedProperty secondWeapon;
        SerializedProperty secondThrowableWeaponPool;
        SerializedProperty isUsingSecondWeapon;
        SerializedProperty isSwitchWeaponNeeded;
        SerializedProperty isFirstWeaponToReacquire;

        public void DualWeaponMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty dualWeaponMod = specializeClassSerializedObject.FindProperty("dualWeaponMod");

            switchDistance = dualWeaponMod.FindPropertyRelative("switchDistance");
            switchDistanceBuffer = dualWeaponMod.FindPropertyRelative("switchDistanceBuffer");

            secondWeaponId = dualWeaponMod.FindPropertyRelative("secondWeaponId");
            secondSheathTransform = dualWeaponMod.FindPropertyRelative("secondSheathTransform");
            secondWeaponActionHolder = dualWeaponMod.FindPropertyRelative("secondWeaponActionHolder");
            
            secondWeapon = dualWeaponMod.FindPropertyRelative("secondWeapon");
            secondThrowableWeaponPool = dualWeaponMod.FindPropertyRelative("secondThrowableWeaponPool");
            isUsingSecondWeapon = dualWeaponMod.FindPropertyRelative("isUsingSecondWeapon");
            isSwitchWeaponNeeded = dualWeaponMod.FindPropertyRelative("isSwitchWeaponNeeded");
            isFirstWeaponToReacquire = dualWeaponMod.FindPropertyRelative("isFirstWeaponToReacquire");

            showDualWeaponMod = dualWeaponMod.FindPropertyRelative("showDualWeaponMod");
            openDualWeaponMod = showDualWeaponMod.boolValue;
        }

        public void DualWeaponMod_Tick()
        {
            openDualWeaponMod = EditorGUILayout.BeginFoldoutHeaderGroup(openDualWeaponMod, "Dual Weapon Mod", foldoutHeaderGUIStyle);
            if (openDualWeaponMod)
            {
                showDualWeaponMod.boolValue = true;

                DrawModHeader("Configuration", modHeaderGUIStyle);
                DrawModSubHeader("Switch distance Configuration", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(switchDistance);
                EditorGUILayout.PropertyField(switchDistanceBuffer);

                DrawModSubHeader("Second Weapon Configuration", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(secondWeaponId);
                EditorGUILayout.PropertyField(secondSheathTransform);
                EditorGUILayout.PropertyField(secondWeaponActionHolder);
                
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(secondWeapon);
                EditorGUILayout.PropertyField(secondThrowableWeaponPool);
                EditorGUILayout.PropertyField(isUsingSecondWeapon);
                EditorGUILayout.PropertyField(isSwitchWeaponNeeded);
                EditorGUILayout.PropertyField(isFirstWeaponToReacquire);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showDualWeaponMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }

        #endregion

        #region Throw Returnal Projectile Mod.
        bool openThrowReturnalProjectileMod;
        SerializedProperty showThrowReturnalProjectileMod;

        // Throw Wait Config.
        SerializedProperty stdThrowWaitRate;
        SerializedProperty throwWaitRateRandomizeAmount;
        SerializedProperty checkHasThrownProjectileInit;

        // Projectile Config.
        SerializedProperty targetThrowingDistance;
        SerializedProperty targetThrowingHeight;
        SerializedProperty projectileRotateDir;
        SerializedProperty projectileThrownSpeed;
        SerializedProperty projectileRotateSpeed;
        SerializedProperty projectileReturnWaitRate;
        SerializedProperty reachedDestinationThershold;

        // Throw Wait Status.
        SerializedProperty finalizedThrowWaitTime;

        // Projectile Bool Status.
        SerializedProperty hasThrownProjectile;
        SerializedProperty isThrowingProjectile;
        SerializedProperty isDestinationReached;
        SerializedProperty isProjectileReturning;

        // Projectile Float Status.
        SerializedProperty projectileReturnWaitTimer;
        SerializedProperty distanceToDestination;
        SerializedProperty timeStartedLerping;
        SerializedProperty timeSinceLerpingStarted;

        // Projectile Vector3 Status.
        SerializedProperty startingThrowPosition;
        SerializedProperty targetThrowDestination;

        public void ThrowReturnalProjectileMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty throwReturnalProjectileMod = specializeClassSerializedObject.FindProperty("throwReturnalProjectileMod");

            stdThrowWaitRate = throwReturnalProjectileMod.FindPropertyRelative("stdThrowWaitRate");
            throwWaitRateRandomizeAmount = throwReturnalProjectileMod.FindPropertyRelative("throwWaitRateRandomizeAmount");
            checkHasThrownProjectileInit = throwReturnalProjectileMod.FindPropertyRelative("checkHasThrownProjectileInit");

            targetThrowingDistance = throwReturnalProjectileMod.FindPropertyRelative("targetThrowingDistance");
            targetThrowingHeight = throwReturnalProjectileMod.FindPropertyRelative("targetThrowingHeight");
            projectileRotateDir = throwReturnalProjectileMod.FindPropertyRelative("projectileRotateDir");
            projectileThrownSpeed = throwReturnalProjectileMod.FindPropertyRelative("projectileThrownSpeed");
            projectileRotateSpeed = throwReturnalProjectileMod.FindPropertyRelative("projectileRotateSpeed");
            projectileReturnWaitRate = throwReturnalProjectileMod.FindPropertyRelative("projectileReturnWaitRate");
            reachedDestinationThershold = throwReturnalProjectileMod.FindPropertyRelative("reachedDestinationThershold");

            finalizedThrowWaitTime = throwReturnalProjectileMod.FindPropertyRelative("finalizedThrowWaitTime");

            hasThrownProjectile = throwReturnalProjectileMod.FindPropertyRelative("hasThrownProjectile");
            isThrowingProjectile = throwReturnalProjectileMod.FindPropertyRelative("isThrowingProjectile");
            isDestinationReached = throwReturnalProjectileMod.FindPropertyRelative("isDestinationReached");
            isProjectileReturning = throwReturnalProjectileMod.FindPropertyRelative("isProjectileReturning");

            projectileReturnWaitTimer = throwReturnalProjectileMod.FindPropertyRelative("projectileReturnWaitTimer");
            distanceToDestination = throwReturnalProjectileMod.FindPropertyRelative("distanceToDestination");
            timeStartedLerping = throwReturnalProjectileMod.FindPropertyRelative("timeStartedLerping");
            timeSinceLerpingStarted = throwReturnalProjectileMod.FindPropertyRelative("timeSinceLerpingStarted");

            startingThrowPosition = throwReturnalProjectileMod.FindPropertyRelative("startingThrowPosition");
            targetThrowDestination = throwReturnalProjectileMod.FindPropertyRelative("targetThrowDestination");

            showThrowReturnalProjectileMod = throwReturnalProjectileMod.FindPropertyRelative("showThrowReturnalProjectileMod");
            openThrowReturnalProjectileMod = showThrowReturnalProjectileMod.boolValue;
        }

        public void ThrowReturnalProjectileMod_Tick()
        {
            openThrowReturnalProjectileMod = EditorGUILayout.BeginFoldoutHeaderGroup(openThrowReturnalProjectileMod, "Throw Returnal Projectile Mod", foldoutHeaderGUIStyle);
            if (openThrowReturnalProjectileMod)
            {
                showThrowReturnalProjectileMod.boolValue = true;

                DrawModHeader("Config.", modHeaderGUIStyle);
                DrawModSubHeader("Throw Wait.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(stdThrowWaitRate);
                EditorGUILayout.PropertyField(throwWaitRateRandomizeAmount);
                EditorGUILayout.PropertyField(checkHasThrownProjectileInit);

                DrawModSubHeader("Projectile.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(targetThrowingDistance);
                EditorGUILayout.PropertyField(targetThrowingHeight);
                EditorGUILayout.PropertyField(projectileRotateDir);
                EditorGUILayout.PropertyField(projectileThrownSpeed);
                EditorGUILayout.PropertyField(projectileRotateSpeed);
                EditorGUILayout.PropertyField(projectileReturnWaitRate);
                EditorGUILayout.PropertyField(reachedDestinationThershold);

                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Status.", modHeaderGUIStyle);
                DrawModSubHeader("Throw Wait.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(finalizedThrowWaitTime);

                DrawModSubHeader("Projectile Bools.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(hasThrownProjectile);
                EditorGUILayout.PropertyField(isThrowingProjectile);
                EditorGUILayout.PropertyField(isDestinationReached);
                EditorGUILayout.PropertyField(isProjectileReturning);

                DrawModSubHeader("Projectile Floats.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(projectileReturnWaitTimer);
                EditorGUILayout.PropertyField(distanceToDestination);
                EditorGUILayout.PropertyField(timeStartedLerping);
                EditorGUILayout.PropertyField(timeSinceLerpingStarted);

                DrawModSubHeader("Projectile Vector3.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(startingThrowPosition);
                EditorGUILayout.PropertyField(targetThrowDestination);

                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showThrowReturnalProjectileMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Enemy Evolve Mod.
        bool openEnemyEvolveMod;
        SerializedProperty showEnemyEvolveMod;

        // Config.
        SerializedProperty _evolveHealthRatioThershold;
        SerializedProperty _evolveChargeRate;

        // Status.
        SerializedProperty _evolveChargeTimer;
        SerializedProperty _isEvolvable;
        SerializedProperty _isEvolveStarted;
        SerializedProperty _hasEvolved;

        // Drag And Drop.
        SerializedProperty _evolvedActionHolder;
        SerializedProperty _evolvedAppearanceEffects;
        SerializedProperty _evolveChargeEffect;
        SerializedProperty _evolveRelease_area_dp_Id;
        SerializedProperty _evolveReleaseAttackRefs;

        public void EnemyEvolveMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty enemyEvolveMod = specializeClassSerializedObject.FindProperty("enemyEvolveMod");

            // Config.
            _evolveHealthRatioThershold = enemyEvolveMod.FindPropertyRelative("evolveHealthRatioThershold");
            _evolveChargeRate = enemyEvolveMod.FindPropertyRelative("evolveChargeRate");

            // Status.
            _evolveChargeTimer = enemyEvolveMod.FindPropertyRelative("evolveChargeTimer");
            _isEvolvable = enemyEvolveMod.FindPropertyRelative("isEvolvable");
            _isEvolveStarted = enemyEvolveMod.FindPropertyRelative("isEvolveStarted");
            _hasEvolved = enemyEvolveMod.FindPropertyRelative("hasEvolved");

            // Drag And Drop.
            _evolvedActionHolder = enemyEvolveMod.FindPropertyRelative("evolvedActionHolder");
            _evolvedAppearanceEffects = enemyEvolveMod.FindPropertyRelative("evolvedAppearanceEffects");
            _evolveChargeEffect = enemyEvolveMod.FindPropertyRelative("evolveChargeEffect");
            _evolveRelease_area_dp_Id = enemyEvolveMod.FindPropertyRelative("evolveRelease_area_dp_Id");
            _evolveReleaseAttackRefs = enemyEvolveMod.FindPropertyRelative("evolveReleaseAttackRefs");

            showEnemyEvolveMod = enemyEvolveMod.FindPropertyRelative("showEnemyEvolveMod");
            openEnemyEvolveMod = showEnemyEvolveMod.boolValue;
        }

        public void EnemyEvolveMod_Tick()
        {
            openEnemyEvolveMod = EditorGUILayout.BeginFoldoutHeaderGroup(openEnemyEvolveMod, "Enemy Evolve Mod", foldoutHeaderGUIStyle);
            if (openEnemyEvolveMod)
            {
                showEnemyEvolveMod.boolValue = true;

                DrawModHeader("Config.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_evolveHealthRatioThershold);
                EditorGUILayout.PropertyField(_evolveChargeRate);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Status.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_evolveChargeTimer);
                EditorGUILayout.PropertyField(_isEvolvable);
                EditorGUILayout.PropertyField(_isEvolveStarted);
                EditorGUILayout.PropertyField(_hasEvolved);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Action Holder.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_evolvedActionHolder);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Effects FX.", modHeaderGUIStyle);
                DrawModSubHeader("Appearances", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_evolvedAppearanceEffects);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("Charges.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_evolveChargeEffect);
                EditorGUILayout.PropertyField(_evolveRelease_area_dp_Id);
                EditorGUILayout.PropertyField(_evolveReleaseAttackRefs);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showEnemyEvolveMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Damage Particle Attack Mod.
        bool openDpAttackMod;
        SerializedProperty showDpAttackMod;

        SerializedProperty stdDpAttackWaitRate;
        SerializedProperty damageParticleAttackIsRandomized;
        SerializedProperty dPAttackRandomizeAmount;

        SerializedProperty usedDpAttack;
        SerializedProperty finalizedDpAttackWaitTime;

        SerializedProperty _dp_area_attack_1_singleton_Id;
        SerializedProperty _dp_area_attack_2_singleton_Id;
        SerializedProperty _dp_area_attack_3_singleton_Id;
        SerializedProperty _dp_proj_attack_1_singleton_Id;
        SerializedProperty _dp_proj_attack_2_singleton_Id;
        SerializedProperty _dp_proj_attack_3_singleton_Id;

        public void DamageParticleAttackMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty damageParticleAttackMod = specializeClassSerializedObject.FindProperty("damageParticleAttackMod");

            stdDpAttackWaitRate = damageParticleAttackMod.FindPropertyRelative("stdDpAttackWaitRate");
            damageParticleAttackIsRandomized = damageParticleAttackMod.FindPropertyRelative("isRandomized");
            dPAttackRandomizeAmount = damageParticleAttackMod.FindPropertyRelative("dPAttackRandomizeAmount");

            usedDpAttack = damageParticleAttackMod.FindPropertyRelative("usedDpAttack");
            finalizedDpAttackWaitTime = damageParticleAttackMod.FindPropertyRelative("finalizedDpAttackWaitTime");

            _dp_area_attack_1_singleton_Id = damageParticleAttackMod.FindPropertyRelative("_dp_area_attack_1_singleton_Id");
            _dp_area_attack_2_singleton_Id = damageParticleAttackMod.FindPropertyRelative("_dp_area_attack_2_singleton_Id");
            _dp_area_attack_3_singleton_Id = damageParticleAttackMod.FindPropertyRelative("_dp_area_attack_3_singleton_Id");
            _dp_proj_attack_1_singleton_Id = damageParticleAttackMod.FindPropertyRelative("_dp_proj_attack_1_singleton_Id");
            _dp_proj_attack_2_singleton_Id = damageParticleAttackMod.FindPropertyRelative("_dp_proj_attack_2_singleton_Id");
            _dp_proj_attack_3_singleton_Id = damageParticleAttackMod.FindPropertyRelative("_dp_proj_attack_3_singleton_Id");

            showDpAttackMod = damageParticleAttackMod.FindPropertyRelative("showDpAttackMod");
            openDpAttackMod = showDpAttackMod.boolValue;
        }

        public void DamageParticleAttackMod_Tick()
        {
            openDpAttackMod = EditorGUILayout.BeginFoldoutHeaderGroup(openDpAttackMod, "Damage Particle Attack Mod", foldoutHeaderGUIStyle);
            if (openDpAttackMod)
            {
                showDpAttackMod.boolValue = true;

                DrawModHeader("Config.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(stdDpAttackWaitRate);
                EditorGUILayout.PropertyField(damageParticleAttackIsRandomized);
                EditorGUILayout.PropertyField(dPAttackRandomizeAmount);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Status.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(usedDpAttack);
                EditorGUILayout.PropertyField(finalizedDpAttackWaitTime);
                EditorGUI.indentLevel -= 1;
                
                DrawModHeader("Area Damage Effects.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_dp_area_attack_1_singleton_Id);
                EditorGUILayout.PropertyField(_dp_area_attack_2_singleton_Id);
                EditorGUILayout.PropertyField(_dp_area_attack_3_singleton_Id);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Projectile Damage Effects.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_dp_proj_attack_1_singleton_Id);
                EditorGUILayout.PropertyField(_dp_proj_attack_2_singleton_Id);
                EditorGUILayout.PropertyField(_dp_proj_attack_3_singleton_Id);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showDpAttackMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Left Leg Damage Collider Mod.
        bool open_L_Leg_DC_Mod;
        SerializedProperty show_L_Leg_DC_Mod;

        SerializedProperty _l_leg_damageCollider;

        public void LeftLegDamageColliderMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty l_leg_damageColliderMod = specializeClassSerializedObject.FindProperty("l_leg_damageColliderMod");

            _l_leg_damageCollider = l_leg_damageColliderMod.FindPropertyRelative("_l_leg_damageCollider");

            show_L_Leg_DC_Mod = l_leg_damageColliderMod.FindPropertyRelative("show_L_Leg_DC_Mod");
            open_L_Leg_DC_Mod = show_L_Leg_DC_Mod.boolValue;
        }

        public void LeftLegDamageColliderMod_Tick()
        {
            open_L_Leg_DC_Mod = EditorGUILayout.BeginFoldoutHeaderGroup(open_L_Leg_DC_Mod, "Left Leg Damage Collider Mod", foldoutHeaderGUIStyle);
            if (open_L_Leg_DC_Mod)
            {
                show_L_Leg_DC_Mod.boolValue = true;

                DrawModHeader("Config.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_l_leg_damageCollider);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                show_L_Leg_DC_Mod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Right Leg Damage Collider Mod.
        bool open_R_Leg_DC_Mod;
        SerializedProperty show_R_Leg_DC_Mod;

        SerializedProperty _r_leg_damageCollider;

        public void RightLegDamageColliderMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty r_leg_damageColliderMod = specializeClassSerializedObject.FindProperty("r_leg_damageColliderMod");

            _r_leg_damageCollider = r_leg_damageColliderMod.FindPropertyRelative("_r_leg_damageCollider");

            show_R_Leg_DC_Mod = r_leg_damageColliderMod.FindPropertyRelative("show_R_Leg_DC_Mod");
            open_R_Leg_DC_Mod = show_R_Leg_DC_Mod.boolValue;
        }

        public void RightLegDamageColliderMod_Tick()
        {
            open_R_Leg_DC_Mod = EditorGUILayout.BeginFoldoutHeaderGroup(open_R_Leg_DC_Mod, "Right Leg Damage Collider Mod", foldoutHeaderGUIStyle);
            if (open_R_Leg_DC_Mod)
            {
                show_R_Leg_DC_Mod.boolValue = true;

                DrawModHeader("Config.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_r_leg_damageCollider);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                show_R_Leg_DC_Mod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Left Shoulder Damage Collider Mod.
        bool open_L_Shoulder_DC_Mod;
        SerializedProperty show_L_Shoulder_DC_Mod;

        SerializedProperty _l_shoulder_damageCollider;

        public void LeftShoulderDamageColliderMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty l_shoulder_damageColliderMod = specializeClassSerializedObject.FindProperty("l_shoulder_damageColliderMod");

            _l_shoulder_damageCollider = l_shoulder_damageColliderMod.FindPropertyRelative("_l_shoulder_damageCollider");

            show_L_Shoulder_DC_Mod = l_shoulder_damageColliderMod.FindPropertyRelative("show_L_Shoulder_DC_Mod");
            open_L_Shoulder_DC_Mod = show_L_Shoulder_DC_Mod.boolValue;
        }

        public void LeftShoulderDamageColliderMod_Tick()
        {
            open_L_Shoulder_DC_Mod = EditorGUILayout.BeginFoldoutHeaderGroup(open_L_Shoulder_DC_Mod, "Left Shoulder Damage Collider Mod", foldoutHeaderGUIStyle);
            if (open_L_Shoulder_DC_Mod)
            {
                show_L_Shoulder_DC_Mod.boolValue = true;

                DrawModHeader("Config.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_l_shoulder_damageCollider);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                show_L_Shoulder_DC_Mod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Full Body Damage Collider Mod.
        bool open_FullBody_DC_Mod;
        SerializedProperty show_FullBody_DC_Mod;

        SerializedProperty _fullBody_r_arm_damageCollider;
        SerializedProperty _fullBody_l_arm_damageCollider;
        SerializedProperty _fullBody_r_leg_damageCollider;
        SerializedProperty _fullBody_l_leg_damageCollider;

        SerializedProperty _fullBody_prev_damageCollider;

        public void FullBodyDamageColliderMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty fullBody_damageColliderMod = specializeClassSerializedObject.FindProperty("fullBodyDamageColliderMod");

            _fullBody_r_arm_damageCollider = fullBody_damageColliderMod.FindPropertyRelative("_r_arm_damageCollider");
            _fullBody_l_arm_damageCollider = fullBody_damageColliderMod.FindPropertyRelative("_l_arm_damageCollider");
            _fullBody_r_leg_damageCollider = fullBody_damageColliderMod.FindPropertyRelative("_r_leg_damageCollider");
            _fullBody_l_leg_damageCollider = fullBody_damageColliderMod.FindPropertyRelative("_l_leg_damageCollider");

            _fullBody_prev_damageCollider = fullBody_damageColliderMod.FindPropertyRelative("_prev_DamageCollider");

            show_FullBody_DC_Mod = fullBody_damageColliderMod.FindPropertyRelative("show_FullBody_DC_Mod");
            open_FullBody_DC_Mod = show_FullBody_DC_Mod.boolValue;
        }

        public void FullBodyDamageColliderMod_Tick()
        {
            open_FullBody_DC_Mod = EditorGUILayout.BeginFoldoutHeaderGroup(open_FullBody_DC_Mod, "FullBody Damage Collider Mod", foldoutHeaderGUIStyle);
            if (open_FullBody_DC_Mod)
            {
                show_FullBody_DC_Mod.boolValue = true;

                DrawModHeader("Config.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_fullBody_r_arm_damageCollider);
                EditorGUILayout.PropertyField(_fullBody_l_arm_damageCollider);
                EditorGUILayout.PropertyField(_fullBody_r_leg_damageCollider);
                EditorGUILayout.PropertyField(_fullBody_l_leg_damageCollider);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Refs.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_fullBody_prev_damageCollider);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                show_FullBody_DC_Mod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Enemy Interactable Mod.
        bool openEnemyInteractableMod;
        SerializedProperty showEnemyInteractableMod;

        // Interactable Config.
        SerializedProperty stdSearchWaitRate;
        SerializedProperty interactableSearchRandomizeAmount;
        SerializedProperty interactableSearchRange;
        SerializedProperty interactableExecuteDistance;
        
        // Interactable Status.
        SerializedProperty switchTargetToInteractable;
        SerializedProperty isPickingUpPowerWeapon;
        SerializedProperty desired_interactable;
        SerializedProperty canSearchInteractable;
        SerializedProperty finalizedSearchWaitRate;

        // Power Weapon Status.
        SerializedProperty currentPowerWeaponInteractable;
        SerializedProperty isCurrentPowerWeaponBroke;

        // Searched Results.
        SerializedProperty searched_interactables;

        public void EnemyInteractableMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty enemyInteractableMod = specializeClassSerializedObject.FindProperty("enemyInteractableMod");

            // Interactable Config.
            stdSearchWaitRate = enemyInteractableMod.FindPropertyRelative("stdSearchWaitRate");
            interactableSearchRandomizeAmount = enemyInteractableMod.FindPropertyRelative("interactableSearchRandomizeAmount");
            interactableSearchRange = enemyInteractableMod.FindPropertyRelative("interactableSearchRange");
            interactableExecuteDistance = enemyInteractableMod.FindPropertyRelative("interactableExecuteDistance");
            
            // Interactable Status.
            switchTargetToInteractable = enemyInteractableMod.FindPropertyRelative("switchTargetToInteractable");
            isPickingUpPowerWeapon = enemyInteractableMod.FindPropertyRelative("isPickingUpPowerWeapon");
            desired_interactable = enemyInteractableMod.FindPropertyRelative("desired_interactable");
            canSearchInteractable = enemyInteractableMod.FindPropertyRelative("canSearchInteractable");
            finalizedSearchWaitRate = enemyInteractableMod.FindPropertyRelative("finalizedSearchWaitRate");

            // Power Weapon Status.
            currentPowerWeaponInteractable = enemyInteractableMod.FindPropertyRelative("currentPowerWeaponInteractable");
            isCurrentPowerWeaponBroke = enemyInteractableMod.FindPropertyRelative("isCurrentPowerWeaponBroke");

            // Searched Results.
            searched_interactables = enemyInteractableMod.FindPropertyRelative("searched_interactables");

            showEnemyInteractableMod = enemyInteractableMod.FindPropertyRelative("showEnemyInteractableMod");
            openEnemyInteractableMod = showEnemyInteractableMod.boolValue;
        }

        public void EnemyInteractableMod_Tick()
        {
            openEnemyInteractableMod = EditorGUILayout.BeginFoldoutHeaderGroup(openEnemyInteractableMod, "Enemy Interactable Mod", foldoutHeaderGUIStyle);
            if (openEnemyInteractableMod)
            {
                showEnemyInteractableMod.boolValue = true;

                DrawModHeader("Config.", modHeaderGUIStyle);
                DrawModSubHeader("Search Customizes.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(stdSearchWaitRate);
                EditorGUILayout.PropertyField(interactableSearchRandomizeAmount);
                EditorGUILayout.PropertyField(interactableSearchRange);
                EditorGUILayout.PropertyField(interactableExecuteDistance);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
                
                DrawModHeader("Status.", modHeaderGUIStyle);
                DrawModSubHeader("Found Interactable.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(switchTargetToInteractable);
                EditorGUILayout.PropertyField(isPickingUpPowerWeapon);
                EditorGUILayout.PropertyField(desired_interactable);
                EditorGUILayout.PropertyField(canSearchInteractable);
                EditorGUILayout.PropertyField(finalizedSearchWaitRate);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("Power Weapon.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(currentPowerWeaponInteractable);
                EditorGUILayout.PropertyField(isCurrentPowerWeaponBroke);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Searched Results.", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(searched_interactables);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showEnemyInteractableMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Egil Stamina Mods

        #region Custom Inspector
        bool openEgilStaminaMod;
        bool openEgilIsTiredSection;
        bool openIsEgilInjuredSection;
        bool openEgilStaminaStatusSection;
        SerializedProperty showEgilStaminaMod;
        SerializedProperty showEgilIsTiredSection;
        SerializedProperty showIsEgilInjuredSection;
        SerializedProperty showEgilStaminStatusSection;
        #endregion

        #region Is Tired Section.
        SerializedProperty egilStaminaReturnMaxSpeed;
        SerializedProperty egilStaminaReturnMinSpeed;
        SerializedProperty maxEgilActionAmounts;
        SerializedProperty leastEgilActionAmounts;
        SerializedProperty egilStaminaCostPerAction;
        #endregion

        #region Is Injured Section.
        SerializedProperty injuredStaminaRecoverSpeed;
        SerializedProperty egilInjuredLoopingFx;
        SerializedProperty egilInjuredState;
        SerializedProperty recoveredAIPassiveAction;
        #endregion

        #region Mod Status Section.
        SerializedProperty isEgilTired;
        SerializedProperty currentEgilStamina;
        SerializedProperty finalizedEgilStaminaAmount;
        SerializedProperty finalizedEgilStaminaRecoverSpeed;
        #endregion

        public void EgilStaminaMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty egilStaminaMod = specializeClassSerializedObject.FindProperty("egilStaminaMod");

            #region Is Tired Section.
            egilStaminaReturnMaxSpeed = egilStaminaMod.FindPropertyRelative("staminaReturnMaxSpeed");
            egilStaminaReturnMinSpeed = egilStaminaMod.FindPropertyRelative("staminaReturnMinSpeed");
            maxEgilActionAmounts = egilStaminaMod.FindPropertyRelative("maxActionAmounts");
            leastEgilActionAmounts = egilStaminaMod.FindPropertyRelative("leastActionAmounts");
            egilStaminaCostPerAction = egilStaminaMod.FindPropertyRelative("staminaCostPerAction");
            #endregion

            #region Is Injured Section.
            injuredStaminaRecoverSpeed = egilStaminaMod.FindPropertyRelative("injuredStaminaRecoverSpeed");
            egilInjuredLoopingFx = egilStaminaMod.FindPropertyRelative("egilInjuredLoopingFx");
            egilInjuredState = egilStaminaMod.FindPropertyRelative("egilInjuredState");
            recoveredAIPassiveAction = egilStaminaMod.FindPropertyRelative("recoveredAIPassiveAction");
            #endregion

            #region Egil Stamina Status Section.
            isEgilTired = egilStaminaMod.FindPropertyRelative("isEgilTired");
            currentEgilStamina = egilStaminaMod.FindPropertyRelative("currentEgilStamina");
            finalizedEgilStaminaAmount = egilStaminaMod.FindPropertyRelative("finalizedStaminaAmount");
            finalizedEgilStaminaRecoverSpeed = egilStaminaMod.FindPropertyRelative("finalizedStaminaRecoverSpeed");
            #endregion

            #region Custom Inspector
            showEgilStaminaMod = egilStaminaMod.FindPropertyRelative("showEgilStaminaMod");
            openEgilStaminaMod = showEgilStaminaMod.boolValue;
            showEgilIsTiredSection = egilStaminaMod.FindPropertyRelative("showIsTiredSection");
            openEgilIsTiredSection = showEgilIsTiredSection.boolValue;
            showIsEgilInjuredSection = egilStaminaMod.FindPropertyRelative("showIsInjuredSection");
            openIsEgilInjuredSection = showIsEgilInjuredSection.boolValue;
            showEgilStaminStatusSection = egilStaminaMod.FindPropertyRelative("showModStatusSection");
            openEgilStaminaStatusSection = showEgilStaminStatusSection.boolValue;
            #endregion
        }

        public void EgilStaminaMod_Tick()
        {
            openEgilStaminaMod = EditorGUILayout.BeginFoldoutHeaderGroup(openEgilStaminaMod, "Egil Stamina Mod", foldoutHeaderGUIStyle);
            if (openEgilStaminaMod)
            {
                showEgilStaminaMod.boolValue = true;

                #region Is Tired Section.
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                openEgilIsTiredSection = EditorGUILayout.Foldout(openEgilIsTiredSection, "Is Tired Config", subFoldoutHeaderGUIStyle);
                if (openEgilIsTiredSection)
                {
                    showEgilIsTiredSection.boolValue = true;
                    EditorGUI.indentLevel += 1;

                    DrawModHeader("Tired Config", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(egilStaminaReturnMaxSpeed);
                    EditorGUILayout.PropertyField(egilStaminaReturnMinSpeed);
                    EditorGUILayout.PropertyField(maxEgilActionAmounts);
                    EditorGUILayout.PropertyField(leastEgilActionAmounts);
                    EditorGUILayout.PropertyField(egilStaminaCostPerAction);
                    EditorGUI.indentLevel -= 1;
                    EditorGUI.indentLevel -= 1;
                }
                else
                {
                    showEgilIsTiredSection.boolValue = false;
                }
                EditorGUI.indentLevel -= 1;
                #endregion

                #region Is Injured Section.
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                openIsEgilInjuredSection = EditorGUILayout.Foldout(openIsEgilInjuredSection, "Is Injured Config", subFoldoutHeaderGUIStyle);
                if (openIsEgilInjuredSection)
                {
                    showIsEgilInjuredSection.boolValue = true;
                    EditorGUI.indentLevel += 1;

                    DrawModHeader("Injured Config", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(injuredStaminaRecoverSpeed);
                    EditorGUILayout.PropertyField(egilInjuredLoopingFx);
                    EditorGUILayout.PropertyField(egilInjuredState);
                    EditorGUILayout.PropertyField(recoveredAIPassiveAction);
                    EditorGUI.indentLevel -= 1;
                    EditorGUI.indentLevel -= 1;
                }
                else
                {
                    showIsEgilInjuredSection.boolValue = false;
                }
                EditorGUI.indentLevel -= 1;
                #endregion

                #region Egil Stamina Status Section.
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                openEgilStaminaStatusSection = EditorGUILayout.Foldout(openEgilStaminaStatusSection, "Status Section", subFoldoutHeaderGUIStyle);
                if (openEgilStaminaStatusSection)
                {
                    showEgilStaminStatusSection.boolValue = true;
                    EditorGUI.indentLevel += 1;
                    
                    DrawModHeader("Status", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(isEgilTired);
                    EditorGUILayout.PropertyField(currentEgilStamina);
                    EditorGUILayout.PropertyField(finalizedEgilStaminaAmount);
                    EditorGUILayout.PropertyField(finalizedEgilStaminaRecoverSpeed);
                    EditorGUI.indentLevel -= 1;
                    EditorGUI.indentLevel -= 1;
                }
                else
                {
                    showEgilStaminStatusSection.boolValue = false;
                }
                EditorGUI.indentLevel -= 1;
                #endregion
            }
            else
            {
                showEgilStaminaMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Egil Execution Mod.
        bool openEgilExecutionMod;
        SerializedProperty showEgilExecutionMod;

        SerializedProperty executionWaitMaxTime;
        SerializedProperty executionWaitMinTime;
        SerializedProperty _execution1_ParentPoint;
        SerializedProperty _execution2_ParentPoint;
        SerializedProperty _executionProfile_1;
        SerializedProperty _executionProfile_2;
        SerializedProperty _isExecutePresentAttack;
        SerializedProperty _isExecuteWait;
        SerializedProperty _finalizedExecutionWaitTime;

        public void EgilExecutionMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty egilExecutionMod = specializeClassSerializedObject.FindProperty("egilExecutionMod");
            executionWaitMaxTime = egilExecutionMod.FindPropertyRelative("executionWaitMaxTime");
            executionWaitMinTime = egilExecutionMod.FindPropertyRelative("executionWaitMinTime");
            _execution1_ParentPoint = egilExecutionMod.FindPropertyRelative("_execution1_ParentPoint");
            _execution2_ParentPoint = egilExecutionMod.FindPropertyRelative("_execution2_ParentPoint");
            _executionProfile_1 = egilExecutionMod.FindPropertyRelative("_executionProfile_1");
            _executionProfile_2 = egilExecutionMod.FindPropertyRelative("_executionProfile_2");
            _isExecutePresentAttack = egilExecutionMod.FindPropertyRelative("_isExecutePresentAttack");
            _isExecuteWait = egilExecutionMod.FindPropertyRelative("_isExecuteWait");
            _finalizedExecutionWaitTime = egilExecutionMod.FindPropertyRelative("_finalizedExecutionWaitTime");

            showEgilExecutionMod = egilExecutionMod.FindPropertyRelative("showEgilExecutionMod");
            openEgilExecutionMod = showEgilExecutionMod.boolValue;
        }

        public void EgilExecutionMod_Tick()
        {
            openEgilExecutionMod = EditorGUILayout.BeginFoldoutHeaderGroup(openEgilExecutionMod, "Egil Execution Mod", foldoutHeaderGUIStyle);
            if (openEgilExecutionMod)
            {
                showEgilExecutionMod.boolValue = true;

                DrawModHeader("Configuration", modHeaderGUIStyle);
                DrawModSubHeader("Execution Wait", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(executionWaitMaxTime);
                EditorGUILayout.PropertyField(executionWaitMinTime);
                EditorGUI.indentLevel -= 1;
                
                DrawModSubHeader("Parent Points", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_execution1_ParentPoint);
                EditorGUILayout.PropertyField(_execution2_ParentPoint);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("Execution Profiles", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_executionProfile_1);
                EditorGUILayout.PropertyField(_executionProfile_2);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mod Status", modHeaderGUIStyle);
                DrawModSubHeader("Bools.", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_isExecutePresentAttack);
                EditorGUILayout.PropertyField(_isExecuteWait);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("Finalized Times", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_finalizedExecutionWaitTime);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showEgilExecutionMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Knock Down Player Mod.
        bool openKnockDownPlayerMod;
        SerializedProperty showKnockDownPlayerMod;

        SerializedProperty knockDownWaitRate;

        SerializedProperty _knockDownWaitTimer;
        SerializedProperty _isKnockDownPlayerWait;

        public void KnockDownPlayerMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty knockDownPlayerMod = specializeClassSerializedObject.FindProperty("knockDownPlayerMod");
            
            knockDownWaitRate = knockDownPlayerMod.FindPropertyRelative("knockDownWaitRate");

            _knockDownWaitTimer = knockDownPlayerMod.FindPropertyRelative("_knockDownWaitTimer");
            _isKnockDownPlayerWait = knockDownPlayerMod.FindPropertyRelative("_isKnockDownPlayerWait");

            showKnockDownPlayerMod = knockDownPlayerMod.FindPropertyRelative("showKnockDownPlayerMod");
            openKnockDownPlayerMod = showKnockDownPlayerMod.boolValue;
        }

        public void KnockDownPlayerMod_Tick()
        {
            openKnockDownPlayerMod = EditorGUILayout.BeginFoldoutHeaderGroup(openKnockDownPlayerMod, "Knock Down Player Mod", foldoutHeaderGUIStyle);
            if (openKnockDownPlayerMod)
            {
                showKnockDownPlayerMod.boolValue = true;

                DrawModHeader("Configuration", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(knockDownWaitRate);
                EditorGUI.indentLevel -= 1;
                
                DrawModHeader("Mod Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_knockDownWaitTimer);
                EditorGUILayout.PropertyField(_isKnockDownPlayerWait);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showKnockDownPlayerMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Egil Kinematic Motion Attack Mod.

        #region Custom Inspector
        bool openEgil_KMA_Mod;
        bool openI_KMA_MotionMonitorHandler;
        bool open_PivotStatus_section;
        bool open_KMJ_section;
        bool open_Switch_section;
        bool open_KMA_Wait_section;
        bool open_KMA_section;
        bool open_TrackPlayer_section;
        bool open_KM_Result_section;
        bool open_KM_Record_section;

        SerializedProperty showEgil_KMA_Mod;
        SerializedProperty showI_KMA_MotionMonitorHandler;
        SerializedProperty show_PivotStatus_section;
        SerializedProperty show_KMJ_section;
        SerializedProperty show_Switch_section;
        SerializedProperty show_KMA_Wait_section;
        SerializedProperty show_KMA_section;
        SerializedProperty show_TrackPlayer_section;
        SerializedProperty show_KM_Result_section;
        SerializedProperty show_KM_Record_section;
        #endregion

        #region I KMA MotionMonitorHandler section.
        SerializedProperty _firstPhaseChangeMMH;
        SerializedProperty _secondPhaseChangeMMH;
        SerializedProperty _default_KMA_MMH;
        #endregion

        #region Pivot Status section.
        SerializedProperty _cur_KMJ_Zone_Type;
        SerializedProperty _pivotToPlayerAngle;
        SerializedProperty _pivotToPlayerSqrDis;
        #endregion

        #region KMJ Section.
        SerializedProperty _KMJ_Pivot_Transform;
        SerializedProperty _F_KMJ_Goal;
        SerializedProperty _R_KMJ_Goal;
        SerializedProperty _L_KMJ_Goal;
        SerializedProperty _B_KMJ_Goal;

        SerializedProperty _lowerGoalMaxHeightOffset;
        SerializedProperty _higherGoalMaxHeightOffset;
        SerializedProperty _KMJGravity;

        SerializedProperty v1JumpPlay2ndHalfOffset;
        SerializedProperty v2JumpPlay2ndHalfOffset;
        #endregion

        #region Switch Section.
        SerializedProperty _switchPoint_1;
        SerializedProperty _switchPoint_2;
        SerializedProperty _switchPoint_3;
        SerializedProperty _switchPoint_4;
        
        SerializedProperty switchZoneSqrDistance;
        SerializedProperty switchZoneAngle;
        
        SerializedProperty _isStartMonitoringSwitch;
        SerializedProperty _isSwitchNeeded;
        #endregion

        #region KMA Wait Section.
        SerializedProperty _KMA_WaitState;
        SerializedProperty _KMA_NormalWaitRate;
        SerializedProperty _KMA_NoWait_WaitRate;
        SerializedProperty _KMA_waitTimer;
        SerializedProperty _is_KMA_WaitNeeded;
        SerializedProperty _is_KMA_WaitPaused;
        #endregion

        #region KMA Section.
        SerializedProperty _KMA_Goal_Transform;
        SerializedProperty KMA_Profile_1;
        SerializedProperty KMA_Profile_2;
        SerializedProperty KMA_Profile_3;
        SerializedProperty KMA_Profile_4;
        SerializedProperty _cur_KMA_Profile;

        SerializedProperty _KMA_staminaUsage;
        SerializedProperty _KMA_attackPredictRange;
        SerializedProperty _is_KMA_PerliousAttack;

        SerializedProperty maxFallHeight;
        SerializedProperty fallGravity;
        #endregion

        #region Track Player Section.
        SerializedProperty _trackRate;
        SerializedProperty _applyMovementOffsetThershold;
        SerializedProperty _trackTimer;
        SerializedProperty _sqrDisFromLastTracked;
        SerializedProperty _lastTrackPlayerPos;
        SerializedProperty _hasTrackedForOnce;
        #endregion

        #region KM Result Section.
        SerializedProperty totalTimeTaken;
        SerializedProperty zenithTimeTaken;
        SerializedProperty inAttackRangeApproxTime;
        SerializedProperty motionTakenTimer;
        
        SerializedProperty _isJumpMotion;
        SerializedProperty _isMotionStarted;
        SerializedProperty _is_KMA_WaitTick_Started;
        SerializedProperty _canExit_KMA_State;
        SerializedProperty _KMJ_isZenithReached;
        SerializedProperty _KMA_isAttackRangeReached;
        #endregion

        #region KM Record Section.
        SerializedProperty _currentGravity;
        SerializedProperty _currentMaxHeight;
        SerializedProperty _current_KMJ_Goal;
        #endregion

        #region Test.
        SerializedProperty _startOverlapBoxHitDetect;
        SerializedProperty _bossWeaponCollider;
        #endregion

        public void Egil_KMA_Mod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty egil_KMA_Mod = specializeClassSerializedObject.FindProperty("egil_KMA_Mod");

            #region I KMA MotionMonitorHandler section.
            _firstPhaseChangeMMH = egil_KMA_Mod.FindPropertyRelative("_firstPhaseChangeMMH");
            _secondPhaseChangeMMH = egil_KMA_Mod.FindPropertyRelative("_secondPhaseChangeMMH");
            _default_KMA_MMH = egil_KMA_Mod.FindPropertyRelative("_default_KMA_MMH");
            #endregion

            #region Pivot Status section.
            _cur_KMJ_Zone_Type = egil_KMA_Mod.FindPropertyRelative("_cur_KMJ_Zone_Type");
            _pivotToPlayerAngle = egil_KMA_Mod.FindPropertyRelative("_pivotToPlayerAngle");
            _pivotToPlayerSqrDis = egil_KMA_Mod.FindPropertyRelative("_pivotToPlayerSqrDis");
            #endregion

            #region KMJ Section.
            _KMJ_Pivot_Transform = egil_KMA_Mod.FindPropertyRelative("_KMJ_Pivot_Transform");
            _F_KMJ_Goal = egil_KMA_Mod.FindPropertyRelative("_F_KMJ_Goal");
            _R_KMJ_Goal = egil_KMA_Mod.FindPropertyRelative("_R_KMJ_Goal");
            _L_KMJ_Goal = egil_KMA_Mod.FindPropertyRelative("_L_KMJ_Goal");
            _B_KMJ_Goal = egil_KMA_Mod.FindPropertyRelative("_B_KMJ_Goal");

            _lowerGoalMaxHeightOffset = egil_KMA_Mod.FindPropertyRelative("_lowerGoalMaxHeightOffset");
            _higherGoalMaxHeightOffset = egil_KMA_Mod.FindPropertyRelative("_higherGoalMaxHeightOffset");
            _KMJGravity = egil_KMA_Mod.FindPropertyRelative("_KMJGravity");

            v1JumpPlay2ndHalfOffset = egil_KMA_Mod.FindPropertyRelative("v1JumpPlay2ndHalfOffset");
            v2JumpPlay2ndHalfOffset = egil_KMA_Mod.FindPropertyRelative("v2JumpPlay2ndHalfOffset");
            #endregion

            #region Switch Section.
            _switchPoint_1 = egil_KMA_Mod.FindPropertyRelative("_switchPoint_1");
            _switchPoint_2 = egil_KMA_Mod.FindPropertyRelative("_switchPoint_2");
            _switchPoint_3 = egil_KMA_Mod.FindPropertyRelative("_switchPoint_3");
            _switchPoint_4 = egil_KMA_Mod.FindPropertyRelative("_switchPoint_4");

            switchZoneSqrDistance = egil_KMA_Mod.FindPropertyRelative("switchZoneSqrDistance");
            switchZoneAngle = egil_KMA_Mod.FindPropertyRelative("switchZoneAngle");
            
            _isStartMonitoringSwitch = egil_KMA_Mod.FindPropertyRelative("_isStartMonitoringSwitch");
            _isSwitchNeeded = egil_KMA_Mod.FindPropertyRelative("_isSwitchNeeded");
            #endregion

            #region Wait Section.
            _KMA_WaitState = egil_KMA_Mod.FindPropertyRelative("_KMA_WaitState");
            _KMA_NormalWaitRate = egil_KMA_Mod.FindPropertyRelative("_KMA_NormalWaitRate");
            _KMA_NoWait_WaitRate = egil_KMA_Mod.FindPropertyRelative("_KMA_NoWait_WaitRate");
            _KMA_waitTimer = egil_KMA_Mod.FindPropertyRelative("_KMA_waitTimer");
            _is_KMA_WaitNeeded = egil_KMA_Mod.FindPropertyRelative("_is_KMA_WaitNeeded");
            _is_KMA_WaitPaused = egil_KMA_Mod.FindPropertyRelative("_is_KMA_WaitPaused");
            #endregion

            #region KMA Section.
            _KMA_Goal_Transform = egil_KMA_Mod.FindPropertyRelative("_KMA_Goal_Transform");
            KMA_Profile_1 = egil_KMA_Mod.FindPropertyRelative("KMA_Profile_1");
            KMA_Profile_2 = egil_KMA_Mod.FindPropertyRelative("KMA_Profile_2");
            KMA_Profile_3 = egil_KMA_Mod.FindPropertyRelative("KMA_Profile_3");
            KMA_Profile_4 = egil_KMA_Mod.FindPropertyRelative("KMA_Profile_4");
            _cur_KMA_Profile = egil_KMA_Mod.FindPropertyRelative("_cur_KMA_Profile");

            _KMA_staminaUsage = egil_KMA_Mod.FindPropertyRelative("_KMA_staminaUsage");
            _KMA_attackPredictRange = egil_KMA_Mod.FindPropertyRelative("_KMA_attackPredictRange");
            _is_KMA_PerliousAttack = egil_KMA_Mod.FindPropertyRelative("_is_KMA_PerliousAttack");
            
            maxFallHeight = egil_KMA_Mod.FindPropertyRelative("maxFallHeight");
            fallGravity = egil_KMA_Mod.FindPropertyRelative("fallGravity");
            #endregion

            #region Track Player Section.
            _trackRate = egil_KMA_Mod.FindPropertyRelative("_trackRate");
            _applyMovementOffsetThershold = egil_KMA_Mod.FindPropertyRelative("_applyMovementOffsetThershold");
            _trackTimer = egil_KMA_Mod.FindPropertyRelative("_trackTimer");
            _sqrDisFromLastTracked = egil_KMA_Mod.FindPropertyRelative("_sqrDisFromLastTracked");
            _lastTrackPlayerPos = egil_KMA_Mod.FindPropertyRelative("_lastTrackPlayerPos");
            _hasTrackedForOnce = egil_KMA_Mod.FindPropertyRelative("_hasTrackedForOnce");
            #endregion

            #region KMA Result Section.
            totalTimeTaken = egil_KMA_Mod.FindPropertyRelative("totalTimeTaken");
            zenithTimeTaken = egil_KMA_Mod.FindPropertyRelative("zenithTimeTaken");
            inAttackRangeApproxTime = egil_KMA_Mod.FindPropertyRelative("inAttackRangeApproxTime");
            motionTakenTimer = egil_KMA_Mod.FindPropertyRelative("motionTakenTimer");
            
            _isJumpMotion = egil_KMA_Mod.FindPropertyRelative("_isJumpMotion");
            _isMotionStarted = egil_KMA_Mod.FindPropertyRelative("_isMotionStarted");
            _is_KMA_WaitTick_Started = egil_KMA_Mod.FindPropertyRelative("_is_KMA_WaitTick_Started");
            _canExit_KMA_State = egil_KMA_Mod.FindPropertyRelative("_canExit_KMA_State");
            _KMJ_isZenithReached = egil_KMA_Mod.FindPropertyRelative("_KMJ_isZenithReached");
            _KMA_isAttackRangeReached = egil_KMA_Mod.FindPropertyRelative("_KMA_isAttackRangeReached");
            #endregion

            #region KM Record Section.
            _currentGravity = egil_KMA_Mod.FindPropertyRelative("_currentGravity");
            _currentMaxHeight = egil_KMA_Mod.FindPropertyRelative("_currentMaxHeight");
            _current_KMJ_Goal = egil_KMA_Mod.FindPropertyRelative("_current_KMJ_Goal");
            #endregion

            _startOverlapBoxHitDetect = egil_KMA_Mod.FindPropertyRelative("_startOverlapBoxHitDetect");
            _bossWeaponCollider = egil_KMA_Mod.FindPropertyRelative("_bossWeaponCollider");

            #region Custom Inspector.
            showEgil_KMA_Mod = egil_KMA_Mod.FindPropertyRelative("showEgil_KMA_Mod");
            openEgil_KMA_Mod = showEgil_KMA_Mod.boolValue;

            showI_KMA_MotionMonitorHandler = egil_KMA_Mod.FindPropertyRelative("showI_KMA_MotionMonitorHandler");
            openI_KMA_MotionMonitorHandler = showI_KMA_MotionMonitorHandler.boolValue;

            show_PivotStatus_section = egil_KMA_Mod.FindPropertyRelative("show_PivotStatus_section");
            open_PivotStatus_section = show_PivotStatus_section.boolValue;

            show_KMJ_section = egil_KMA_Mod.FindPropertyRelative("show_KMJ_section");
            open_KMJ_section = show_KMJ_section.boolValue;

            show_Switch_section = egil_KMA_Mod.FindPropertyRelative("show_Switch_section");
            open_Switch_section = show_Switch_section.boolValue;

            show_KMA_Wait_section = egil_KMA_Mod.FindPropertyRelative("show_KMA_Wait_section");
            open_KMA_Wait_section = show_KMA_Wait_section.boolValue;

            show_KMA_section = egil_KMA_Mod.FindPropertyRelative("show_KMA_section");
            open_KMA_section = show_KMA_section.boolValue;

            show_TrackPlayer_section = egil_KMA_Mod.FindPropertyRelative("show_TrackPlayer_section");
            open_TrackPlayer_section = show_TrackPlayer_section.boolValue;

            show_KM_Result_section = egil_KMA_Mod.FindPropertyRelative("show_KM_Result_section");
            open_KM_Result_section = show_KM_Result_section.boolValue;

            show_KM_Record_section = egil_KMA_Mod.FindPropertyRelative("show_KM_Record_section");
            open_KM_Record_section = show_KM_Record_section.boolValue;
            #endregion
        }

        public void Egil_KMA_Mod_Tick()
        {
            openEgil_KMA_Mod = EditorGUILayout.BeginFoldoutHeaderGroup(openEgil_KMA_Mod, "Egil KMA Mod", foldoutHeaderGUIStyle);
            if (openEgil_KMA_Mod)
            {
                showEgil_KMA_Mod.boolValue = true;

                #region I KMA MotionMonitorHandler section.
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                openI_KMA_MotionMonitorHandler = EditorGUILayout.Foldout(openI_KMA_MotionMonitorHandler, "I KMA MMHs", subFoldoutHeaderGUIStyle);
                if (openI_KMA_MotionMonitorHandler)
                {
                    showI_KMA_MotionMonitorHandler.boolValue = true;
                    EditorGUI.indentLevel += 1;

                    DrawModHeader("MMHs", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_firstPhaseChangeMMH);
                    EditorGUILayout.PropertyField(_secondPhaseChangeMMH);
                    EditorGUILayout.PropertyField(_default_KMA_MMH);
                    EditorGUI.indentLevel -= 1;
                    EditorGUI.indentLevel -= 1;
                }
                else
                {
                    showI_KMA_MotionMonitorHandler.boolValue = false;
                }
                EditorGUI.indentLevel -= 1;
                #endregion

                #region Pivot Status section.
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                open_PivotStatus_section = EditorGUILayout.Foldout(open_PivotStatus_section, "Pivot Status", subFoldoutHeaderGUIStyle);
                if (open_PivotStatus_section)
                {
                    show_PivotStatus_section.boolValue = true;
                    EditorGUI.indentLevel += 1;

                    DrawModHeader("KMJ Status", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_cur_KMJ_Zone_Type);
                    EditorGUILayout.PropertyField(_pivotToPlayerAngle);
                    EditorGUILayout.PropertyField(_pivotToPlayerSqrDis);
                    EditorGUI.indentLevel -= 1;
                    EditorGUI.indentLevel -= 1;
                }
                else
                {
                    show_PivotStatus_section.boolValue = false;
                }
                EditorGUI.indentLevel -= 1;
                #endregion

                #region KMJ Section.
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                open_KMJ_section = EditorGUILayout.Foldout(open_KMJ_section, "KMJ Section", subFoldoutHeaderGUIStyle);
                if (open_KMJ_section)
                {
                    show_KMJ_section.boolValue = true;
                    EditorGUI.indentLevel += 1;

                    DrawModHeader("Goals", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_KMJ_Pivot_Transform);
                    EditorGUILayout.PropertyField(_F_KMJ_Goal);
                    EditorGUILayout.PropertyField(_R_KMJ_Goal);
                    EditorGUILayout.PropertyField(_L_KMJ_Goal);
                    EditorGUILayout.PropertyField(_B_KMJ_Goal);
                    EditorGUI.indentLevel -= 1;

                    DrawModHeader("Jump Motion Value", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_lowerGoalMaxHeightOffset);
                    EditorGUILayout.PropertyField(_higherGoalMaxHeightOffset);
                    EditorGUILayout.PropertyField(_KMJGravity);
                    EditorGUI.indentLevel -= 1;

                    DrawModHeader("Offsets", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(v1JumpPlay2ndHalfOffset);
                    EditorGUILayout.PropertyField(v2JumpPlay2ndHalfOffset);
                    EditorGUI.indentLevel -= 1;
                    EditorGUI.indentLevel -= 1;
                }
                else
                {
                    show_KMJ_section.boolValue = false;
                }
                EditorGUI.indentLevel -= 1;
                #endregion

                #region Switch Section.
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                open_Switch_section = EditorGUILayout.Foldout(open_Switch_section, "Switch Section", subFoldoutHeaderGUIStyle);
                if (open_Switch_section)
                {
                    show_Switch_section.boolValue = true;
                    EditorGUI.indentLevel += 1;

                    DrawModHeader("Switch Points", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_switchPoint_1);
                    EditorGUILayout.PropertyField(_switchPoint_2);
                    EditorGUILayout.PropertyField(_switchPoint_3);
                    EditorGUILayout.PropertyField(_switchPoint_4);
                    EditorGUI.indentLevel -= 1;

                    DrawModHeader("Thersholds", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(switchZoneSqrDistance);
                    EditorGUILayout.PropertyField(switchZoneAngle);
                    EditorGUI.indentLevel -= 1;
                    
                    DrawModHeader("Switch Status", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_isStartMonitoringSwitch);
                    EditorGUILayout.PropertyField(_isSwitchNeeded);
                    EditorGUI.indentLevel -= 1;
                    EditorGUI.indentLevel -= 1;
                }
                else
                {
                    show_Switch_section.boolValue = false;
                }
                EditorGUI.indentLevel -= 1;
                #endregion

                #region Wait Section.
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                open_KMA_Wait_section = EditorGUILayout.Foldout(open_KMA_Wait_section, "Wait Section", subFoldoutHeaderGUIStyle);
                if (open_KMA_Wait_section)
                {
                    show_KMA_Wait_section.boolValue = true;
                    EditorGUI.indentLevel += 1;

                    DrawModHeader("Wait State", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_KMA_WaitState);
                    EditorGUI.indentLevel -= 1;

                    DrawModHeader("Wait Info", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_KMA_NormalWaitRate);
                    EditorGUILayout.PropertyField(_KMA_NoWait_WaitRate);
                    EditorGUILayout.PropertyField(_KMA_waitTimer);
                    EditorGUI.indentLevel -= 1;

                    DrawModHeader("Status", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_is_KMA_WaitNeeded);
                    EditorGUILayout.PropertyField(_is_KMA_WaitPaused);
                    EditorGUI.indentLevel -= 1;
                    EditorGUI.indentLevel -= 1;
                }
                else
                {
                    show_KMA_Wait_section.boolValue = false;
                }
                EditorGUI.indentLevel -= 1;
                #endregion

                #region KMA Section.
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                open_KMA_section = EditorGUILayout.Foldout(open_KMA_section, "KMA Section", subFoldoutHeaderGUIStyle);
                if (open_KMA_section)
                {
                    show_KMA_section.boolValue = true;
                    EditorGUI.indentLevel += 1;

                    DrawModHeader("Profiles", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_KMA_Goal_Transform);
                    EditorGUILayout.PropertyField(KMA_Profile_1);
                    EditorGUILayout.PropertyField(KMA_Profile_2);
                    EditorGUILayout.PropertyField(KMA_Profile_3);
                    EditorGUILayout.PropertyField(KMA_Profile_4);
                    EditorGUILayout.PropertyField(_cur_KMA_Profile);
                    EditorGUI.indentLevel -= 1;

                    DrawModHeader("Action Info", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_KMA_staminaUsage);
                    EditorGUILayout.PropertyField(_KMA_attackPredictRange);
                    EditorGUILayout.PropertyField(_is_KMA_PerliousAttack);
                    EditorGUI.indentLevel -= 1;
                    
                    DrawModHeader("KMA Motion Values", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(maxFallHeight);
                    EditorGUILayout.PropertyField(fallGravity);
                    EditorGUI.indentLevel -= 1;
                    EditorGUI.indentLevel -= 1;
                }
                else
                {
                    show_KMA_section.boolValue = false;
                }
                EditorGUI.indentLevel -= 1;
                #endregion

                #region Track Player Section.
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                open_TrackPlayer_section = EditorGUILayout.Foldout(open_TrackPlayer_section, "Track Player Section", subFoldoutHeaderGUIStyle);
                if (open_TrackPlayer_section)
                {
                    show_TrackPlayer_section.boolValue = true;
                    EditorGUI.indentLevel += 1;

                    DrawModHeader("Last Player Sqr Dis.", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_trackRate);
                    EditorGUILayout.PropertyField(_applyMovementOffsetThershold);
                    EditorGUILayout.PropertyField(_trackTimer);
                    EditorGUILayout.PropertyField(_sqrDisFromLastTracked);
                    EditorGUILayout.PropertyField(_lastTrackPlayerPos);
                    EditorGUILayout.PropertyField(_hasTrackedForOnce);
                    EditorGUI.indentLevel -= 1;
                    EditorGUI.indentLevel -= 1;
                }
                else
                {
                    show_TrackPlayer_section.boolValue = false;
                }
                EditorGUI.indentLevel -= 1;
                #endregion

                #region KM Result Section.
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                open_KM_Result_section = EditorGUILayout.Foldout(open_KM_Result_section, "KM Result Section", subFoldoutHeaderGUIStyle);
                if (open_KM_Result_section)
                {
                    show_KM_Result_section.boolValue = true;
                    EditorGUI.indentLevel += 1;

                    DrawModHeader("Time Taken Results.", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(totalTimeTaken);
                    EditorGUILayout.PropertyField(zenithTimeTaken);
                    EditorGUILayout.PropertyField(inAttackRangeApproxTime);
                    EditorGUILayout.PropertyField(motionTakenTimer);
                    EditorGUI.indentLevel -= 1;
                    
                    DrawModHeader("Timestamp Reached.", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_isJumpMotion);
                    EditorGUILayout.PropertyField(_isMotionStarted);
                    EditorGUILayout.PropertyField(_is_KMA_WaitTick_Started);
                    EditorGUILayout.PropertyField(_canExit_KMA_State);
                    EditorGUILayout.PropertyField(_KMJ_isZenithReached);
                    EditorGUILayout.PropertyField(_KMA_isAttackRangeReached);
                    EditorGUI.indentLevel -= 1;
                    EditorGUI.indentLevel -= 1;
                }
                else
                {
                    show_KM_Result_section.boolValue = false;
                }
                EditorGUI.indentLevel -= 1;
                #endregion

                #region KM Record Section.
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                open_KM_Record_section = EditorGUILayout.Foldout(open_KM_Record_section, "KM Record Section", subFoldoutHeaderGUIStyle);
                if (open_KM_Record_section)
                {
                    show_KM_Record_section.boolValue = true;
                    EditorGUI.indentLevel += 1;

                    DrawModHeader("Record.", modHeaderGUIStyle);
                    EditorGUILayout.PropertyField(_currentGravity);
                    EditorGUILayout.PropertyField(_currentMaxHeight);
                    EditorGUILayout.PropertyField(_current_KMJ_Goal);
                    EditorGUI.indentLevel -= 1;

                    EditorGUILayout.Space(10);
                    EditorGUILayout.PropertyField(_startOverlapBoxHitDetect);
                    EditorGUILayout.PropertyField(_bossWeaponCollider);

                    
                    EditorGUI.indentLevel -= 1;
                }
                else
                {
                    show_KM_Record_section.boolValue = false;
                }
                EditorGUI.indentLevel -= 1;
                #endregion
            }
            else
            {
                showEgil_KMA_Mod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Enemy Central Phase Mod.
        bool openEnemyCentralPhaseMod;
        SerializedProperty showEnemyCentralPhaseMod;

        SerializedProperty _enemy_1P_originalDatas;

        SerializedProperty _mods_1P_originalDatas;

        SerializedProperty _isChangingPhase;
        SerializedProperty _currentEnemyPhaseIndex;

        public void EnemyCentralPhaseMod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty enemyCentralPhaseMod = specializeClassSerializedObject.FindProperty("enemyCentralPhaseMod");

            _enemy_1P_originalDatas = enemyCentralPhaseMod.FindPropertyRelative("_enemy_1P_originalDatas");
            
            _mods_1P_originalDatas = enemyCentralPhaseMod.FindPropertyRelative("_mods_1P_originalDatas");

            _isChangingPhase = enemyCentralPhaseMod.FindPropertyRelative("_isChangingPhase");
            _currentEnemyPhaseIndex = enemyCentralPhaseMod.FindPropertyRelative("_currentEnemyPhaseIndex");

            showEnemyCentralPhaseMod = enemyCentralPhaseMod.FindPropertyRelative("showEnemyCentralPhaseMod");
            openEnemyCentralPhaseMod = showEnemyCentralPhaseMod.boolValue;
        }

        public void EnemyCentralPhaseMod_Tick()
        {
            openEnemyCentralPhaseMod = EditorGUILayout.BeginFoldoutHeaderGroup(openEnemyCentralPhaseMod, "Enemy Central Phase Mod", foldoutHeaderGUIStyle);
            if (openEnemyCentralPhaseMod)
            {
                showEnemyCentralPhaseMod.boolValue = true;
                
                DrawModHeader("Enemy 1P Original Datas", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_enemy_1P_originalDatas);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Mods 1P Original Datas", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_mods_1P_originalDatas);
                EditorGUI.indentLevel -= 1;
                
                DrawModHeader("Central Phase Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_isChangingPhase);
                EditorGUILayout.PropertyField(_currentEnemyPhaseIndex);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showEnemyCentralPhaseMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Enemy 2nd Phase EP Mod.
        bool open2ndPhaseEpMod;
        SerializedProperty show2ndPhaseEpMod;

        SerializedProperty _2ndPhaseChangeRatio;
        SerializedProperty _2ndPhaseChangeAnim;
        SerializedProperty _2ndPhaseActionHolder;
        SerializedProperty _2ndPhaseCrossFadeLayer;
        SerializedProperty _2ndPhaseIndex;

        SerializedProperty _2ndPhaseAIStatus_EP_Datas;

        SerializedProperty _2ndPhaseMods_EP_Datas;

        SerializedProperty _2ndPhase_PassiveAction_1;

        SerializedProperty _2ndPhase_isInNewPhase;

        public void Enemy2ndPhase_EP_Mod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty enemy2ndPhaseEpMod = specializeClassSerializedObject.FindProperty("enemy2ndPhaseEpMod");

            _2ndPhaseChangeRatio = enemy2ndPhaseEpMod.FindPropertyRelative("_phaseChangeRatio");
            _2ndPhaseChangeAnim = enemy2ndPhaseEpMod.FindPropertyRelative("_phaseChangeAnim");
            _2ndPhaseActionHolder = enemy2ndPhaseEpMod.FindPropertyRelative("_nextPhaseActionHolder");
            _2ndPhaseCrossFadeLayer = enemy2ndPhaseEpMod.FindPropertyRelative("_nextPhaseCrossFadeLayer");
            _2ndPhaseIndex = enemy2ndPhaseEpMod.FindPropertyRelative("_nextPhaseIndex");

            _2ndPhaseAIStatus_EP_Datas = enemy2ndPhaseEpMod.FindPropertyRelative("_aiStatus_EP_Datas");

            _2ndPhaseMods_EP_Datas = enemy2ndPhaseEpMod.FindPropertyRelative("_mods_EP_Datas");

            _2ndPhase_PassiveAction_1 = enemy2ndPhaseEpMod.FindPropertyRelative("_EP_PassiveAction_1");

            _2ndPhase_isInNewPhase = enemy2ndPhaseEpMod.FindPropertyRelative("_isInNewPhase");

            show2ndPhaseEpMod = enemy2ndPhaseEpMod.FindPropertyRelative("showEnemyEnumerablePhaseMod");
            open2ndPhaseEpMod = show2ndPhaseEpMod.boolValue;
        }

        public void Enemy2ndPhase_EP_Mod_Tick()
        {
            open2ndPhaseEpMod = EditorGUILayout.BeginFoldoutHeaderGroup(open2ndPhaseEpMod, "Enemy 2nd Phase Mod", foldoutHeaderGUIStyle);
            if (open2ndPhaseEpMod)
            {
                show2ndPhaseEpMod.boolValue = true;

                DrawModHeader("Configuration", modHeaderGUIStyle);
                DrawModSubHeader("Base Config", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_2ndPhaseChangeRatio);
                EditorGUILayout.PropertyField(_2ndPhaseChangeAnim);
                EditorGUILayout.PropertyField(_2ndPhaseActionHolder);
                EditorGUILayout.PropertyField(_2ndPhaseCrossFadeLayer);
                EditorGUILayout.PropertyField(_2ndPhaseIndex);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("AI EP Datas", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_2ndPhaseAIStatus_EP_Datas);
                EditorGUI.indentLevel -= 1;
                DrawModSubHeader("Mods EP Datas", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_2ndPhaseMods_EP_Datas);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("EP AI PassiveAction", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_2ndPhase_PassiveAction_1);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_2ndPhase_isInNewPhase);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                show2ndPhaseEpMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Enemy 3rd Phase EP Mod.
        bool open3rdPhaseEpMod;
        SerializedProperty show3rdPhaseEpMod;

        SerializedProperty _3rdPhaseChangeRatio;
        SerializedProperty _3rdPhaseChangeAnim;
        SerializedProperty _3rdPhaseActionHolder;
        SerializedProperty _3rdPhaseCrossFadeLayer;
        SerializedProperty _3rdPhaseIndex;

        SerializedProperty _3rdPhaseAIStatus_EP_Datas;

        SerializedProperty _3rdPhaseMods_EP_Datas;

        SerializedProperty _3rdPhase_PassiveAction_1;

        SerializedProperty _3rdPhase_isInNewPhase;

        public void Enemy3rdPhase_EP_Mod_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty enemy3rdPhaseEpMod = specializeClassSerializedObject.FindProperty("enemy3rdPhaseEpMod");

            _3rdPhaseChangeRatio = enemy3rdPhaseEpMod.FindPropertyRelative("_phaseChangeRatio");
            _3rdPhaseChangeAnim = enemy3rdPhaseEpMod.FindPropertyRelative("_phaseChangeAnim");
            _3rdPhaseActionHolder = enemy3rdPhaseEpMod.FindPropertyRelative("_nextPhaseActionHolder");
            _3rdPhaseCrossFadeLayer = enemy3rdPhaseEpMod.FindPropertyRelative("_nextPhaseCrossFadeLayer");
            _3rdPhaseIndex = enemy3rdPhaseEpMod.FindPropertyRelative("_nextPhaseIndex");

            _3rdPhaseAIStatus_EP_Datas = enemy3rdPhaseEpMod.FindPropertyRelative("_aiStatus_EP_Datas");

            _3rdPhaseMods_EP_Datas = enemy3rdPhaseEpMod.FindPropertyRelative("_mods_EP_Datas");

            _3rdPhase_PassiveAction_1 = enemy3rdPhaseEpMod.FindPropertyRelative("_EP_PassiveAction_1");

            _3rdPhase_isInNewPhase = enemy3rdPhaseEpMod.FindPropertyRelative("_isInNewPhase");

            show3rdPhaseEpMod = enemy3rdPhaseEpMod.FindPropertyRelative("showEnemyEnumerablePhaseMod");
            open3rdPhaseEpMod = show3rdPhaseEpMod.boolValue;
        }

        public void Enemy3rdPhase_EP_Mod_Tick()
        {
            open3rdPhaseEpMod = EditorGUILayout.BeginFoldoutHeaderGroup(open3rdPhaseEpMod, "Enemy 3rd Phase Mod", foldoutHeaderGUIStyle);
            if (open3rdPhaseEpMod)
            {
                show3rdPhaseEpMod.boolValue = true;

                DrawModHeader("Configuration", modHeaderGUIStyle);
                DrawModSubHeader("Base Config", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_3rdPhaseChangeRatio);
                EditorGUILayout.PropertyField(_3rdPhaseChangeAnim);
                EditorGUILayout.PropertyField(_3rdPhaseActionHolder);
                EditorGUILayout.PropertyField(_3rdPhaseCrossFadeLayer);
                EditorGUILayout.PropertyField(_3rdPhaseIndex);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("AI EP Datas", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_3rdPhaseAIStatus_EP_Datas);
                EditorGUI.indentLevel -= 1;
                
                DrawModSubHeader("Mods EP Datas", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_3rdPhaseMods_EP_Datas);
                EditorGUI.indentLevel -= 1;

                DrawModSubHeader("EP AI PassiveAction", modSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_3rdPhase_PassiveAction_1);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Status", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_3rdPhase_isInNewPhase);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                show3rdPhaseEpMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        #region Limit Enemy Turning Mod.
        bool openLimitEnemyTurningMod;
        SerializedProperty showLimitEnemyTurningMod;

        SerializedProperty _neglectIdleAnimTurningThershold;
        SerializedProperty _currentIdleAnimTurningCount;
        SerializedProperty _countingIdleAnimTurnRate;
        SerializedProperty _countingIdleAnimTurnTimer;
        SerializedProperty _isCountingIdleAnimTurn;
        SerializedProperty _neglectingIdleAnimTurningRate;
        SerializedProperty _isNeglectingIdleAnimTurning;

        public void LimitEnemyTurning_Init(SerializedObject specializeClassSerializedObject)
        {
            SerializedProperty limitEnemyTurning = specializeClassSerializedObject.FindProperty("limitEnemyTurning");

            _neglectIdleAnimTurningThershold = limitEnemyTurning.FindPropertyRelative("_neglectIdleAnimTurningThershold");
            _currentIdleAnimTurningCount = limitEnemyTurning.FindPropertyRelative("_currentIdleAnimTurningCount");

            _countingIdleAnimTurnRate = limitEnemyTurning.FindPropertyRelative("_countingIdleAnimTurnRate");
            _countingIdleAnimTurnTimer = limitEnemyTurning.FindPropertyRelative("_countingIdleAnimTurnTimer");
            _isCountingIdleAnimTurn = limitEnemyTurning.FindPropertyRelative("_isCountingIdleAnimTurn");

            _neglectingIdleAnimTurningRate = limitEnemyTurning.FindPropertyRelative("_neglectingIdleAnimTurningRate");
            _isNeglectingIdleAnimTurning = limitEnemyTurning.FindPropertyRelative("_isNeglectingIdleAnimTurning");

            showLimitEnemyTurningMod = limitEnemyTurning.FindPropertyRelative("showLimitEnemyTurningMod");
            openLimitEnemyTurningMod = showLimitEnemyTurningMod.boolValue;
        }

        public void LimitEnemyTurning_Tick()
        {
            openLimitEnemyTurningMod = EditorGUILayout.BeginFoldoutHeaderGroup(openLimitEnemyTurningMod, "Limit Enemy Turning Mod", foldoutHeaderGUIStyle);
            if (openLimitEnemyTurningMod)
            {
                showLimitEnemyTurningMod.boolValue = true;

                DrawModHeader("Anim Turning Count", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_neglectIdleAnimTurningThershold);
                EditorGUILayout.PropertyField(_currentIdleAnimTurningCount);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Counting Rate", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_countingIdleAnimTurnRate);
                EditorGUILayout.PropertyField(_countingIdleAnimTurnTimer);
                EditorGUILayout.PropertyField(_isCountingIdleAnimTurn);
                EditorGUI.indentLevel -= 1;

                DrawModHeader("Neglect Rate", modHeaderGUIStyle);
                EditorGUILayout.PropertyField(_neglectingIdleAnimTurningRate);
                EditorGUILayout.PropertyField(_isNeglectingIdleAnimTurning);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showLimitEnemyTurningMod.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        #endregion

        protected void DrawUILine1(int thickness = 1, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            Color32 lineColor = new Color32(0, 255, 111, 255);
            EditorGUI.DrawRect(r, lineColor);
        }

        protected void DrawUILine2(int thickness = 1, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            Color32 lineColor = new Color32(210, 105, 30, 255);
            EditorGUI.DrawRect(r, lineColor);
        }

        void DrawModHeader(string headerName, GUIStyle headerFontStyle)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField(headerName, headerFontStyle);
            EditorGUI.indentLevel += 1;
        }

        void DrawModSubHeader(string subHeaderName, GUIStyle subHeaderFontStyle)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField(subHeaderName, subHeaderFontStyle);
            EditorGUI.indentLevel += 1;
        }
    }
}