using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class EgilAIManager : AIManager
    {
        #region Change To 2P Anim Velocity.
        public float changeToVelocity = 1.1f;
        public float _2P_fallbackVelocity = 4;

        /// Custom Inspector.
        public bool showChangeTo2PAnimVelocity;
        #endregion

        [Header("IEgilStamina")]
        public EgilStaminaMod egilStaminaMod;

        [Header("IEnemyRollInterval")]
        public RollIntervalMod rollIntervalMod;

        [Header("IEnemyPerilousAttack")]
        public EgilExecutionMod egilExecutionMod;

        [Header("IEnemyPerilousAttack")]
        public PerilousAttackMod perilousAttackMod;

        [Header("IKnockDownPlayerMod")]
        public KnockDownPlayerMod knockDownPlayerMod;

        [Header("IEgil_KMA_Mod")]
        public EgilKinematicMotionAttackMod egil_KMA_Mod;

        [Header("IEnemyCentralPhaseMod")]
        public EnemyCentralPhaseMod enemyCentralPhaseMod;

        [Header("IEnemy2ndPhase_EP_Mod")]
        public EnemyEnumerablePhaseMod enemy2ndPhaseEpMod;

        [Header("IEnemy3rdPhase_EP_Mod")]
        public EnemyEnumerablePhaseMod enemy3rdPhaseEpMod;
        
        [Header("IR_Leg_DamageCollider")]
        public R_Leg_DamageColliderMod r_leg_damageColliderMod;

        [Header("IL_Shoulder_DamageCollider")]
        public L_Shoulder_DamageColliderMod l_shoulder_damageColliderMod;

        [Header("ILimitEnemyTurning")]
        public LimitEnemyTurningMod limitEnemyTurning;

        #region Setup.
        public override void SetupAIMods()
        {
            egilStaminaMod.StaminaModInit(this);
            egilExecutionMod.EgilExecutionModInit(this);
            egil_KMA_Mod.Egil_KMA_ModInit(this);
            enemyCentralPhaseMod.EnemyCentralPhaseModInit(this);
            enemy2ndPhaseEpMod.EnemyEnumerablePhaseModInit(this);
            enemy3rdPhaseEpMod.EnemyEnumerablePhaseModInit(this);
            r_leg_damageColliderMod.R_Leg_DamageColliderModInit(this);
            l_shoulder_damageColliderMod.L_Shoulder_DamageColliderModInit(this);
        }

        public override void AIModsOnEnterAggroFacedPlayer()
        {
            egilStaminaMod.StaminaUsageGoesAggroReset();
            rollIntervalMod.RollIntervalGoesAggroReset();
            egilExecutionMod.EgilExecutionGoesAggroReset();
            perilousAttackMod.PerilousAttackGoesAggroReset();
            egil_KMA_Mod.Egil_KMA_GoesAggroReset();
            enemyCentralPhaseMod.EnemyEnumerablePhaseGoesAggroReset();
            enemy2ndPhaseEpMod.EnemyEnumerablePhaseGoesAggroReset();
            enemy3rdPhaseEpMod.EnemyEnumerablePhaseGoesAggroReset();
            limitEnemyTurning.LimitEnemyTurningModGoesAggroReset();

            EnemyEnumerablePhaseGoesAggroReset_ResetAnimPara();

            SubscribleCheckpointRefreshEvent();
        }
        #endregion

        #region AI Ticks.
        public override void Tick()
        {
            UpdateAggroStateResets();

            ResetUpdateLockOnPosBool();
            
            AttackIntervalTimeCount();

            egilStaminaMod.MonitorEgilStamina();

            rollIntervalMod.RollIntervalTimeCount();

            egilExecutionMod.EgilExecutionTimeCount();

            perilousAttackMod.PerilousAttackTimeCount();

            knockDownPlayerMod.KnockedDownPlayerTimeCount();

            limitEnemyTurning.TrackIdleAnimTurning();

            GetNextAction();

            MonitorLockOnLocomotionType();

            UpdateAggroStateIK();

            SetEnemyAnim();
        }

        protected override void UpdateModsDeltas()
        {
            egilStaminaMod._delta = _delta;
            rollIntervalMod._delta = _delta;
            egilExecutionMod._delta = _delta;
            perilousAttackMod._delta = _delta;
            knockDownPlayerMod._delta = _delta;
            egil_KMA_Mod._delta = aIStates._delta;
            limitEnemyTurning._delta = _delta;
        }

        public override void SetEnemyAnim()
        {
            base.SetEnemyAnim();
        }
        #endregion

        #region isInteracting Ticks.
        public override void IsInteracting_Tick()
        {
            base.IsInteracting_Tick();

            egilStaminaMod.MonitorEgilStamina();

            rollIntervalMod.RollIntervalTimeCount();

            egilExecutionMod.EgilExecutionTimeCount();

            perilousAttackMod.PerilousAttackTimeCount();

            knockDownPlayerMod.KnockedDownPlayerTimeCount();

            egil_KMA_Mod._i_KMA_MotionMonitorHandler.KMA_WaitTick(egil_KMA_Mod);

            limitEnemyTurning.TrackIdleAnimTurning();
        }

        public override void MonitorMultiStageAttacks()
        {
            if (isMultiStageAttackAvailable && currentMultiStageAttack != null && !playerStates.isDead)
            {
                isMultiStageAttackAvailable = false;
                currentMultiStageAttack.Execute(this);
            }
        }
        #endregion

        #region On Hit.
        protected override void OnHitAIMods()
        {
            enemy2ndPhaseEpMod.OnHitCheckIsPhaseChangeReady();
            enemy3rdPhaseEpMod.OnHitCheckIsPhaseChangeReady();
        }

        protected override void PlayOnHitAnimation()
        {
            if (_isSkippingOnHitAnim || enemyCentralPhaseMod._isChangingPhase)
                return;

            if (enemy3rdPhaseEpMod._isInNewPhase)
            {
                if (!egilStaminaMod.isEgilTired)
                {
                    HandleEgil_2_3_PhaseGetHitAnimation();
                }
            }
            else if (enemy2ndPhaseEpMod._isInNewPhase)
            {
                HandleEgil_2_3_PhaseGetHitAnimation();
            }
            else
            {
                HandleArmedGetHitAnimation();
            }
        }

        public override void HandleArmedGetHitAnimation()
        {
            switch (_hitSourceAttackRefs._attackActionType)
            {
                case Player_AttackRefs.AttackActionTypeEnum.Normal:

                    #region Play Small Hit Animation.
                    switch (_hitSourceAttackRefs._attackDirectionType)
                    {
                        case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                            PlayAnimation_NoNeglect_SpecificLayer(1, e_armed_hit_small_r_hash, false);
                            break;
                        case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                            PlayAnimation_NoNeglect_SpecificLayer(1, e_armed_hit_small_l_hash, false);
                            break;
                        case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                            PlayAnimation_NoNeglect_SpecificLayer(1, e_armed_hit_small_f_hash, false);
                            break;
                    }
                    #endregion

                    break;

                case Player_AttackRefs.AttackActionTypeEnum.Hold:
                case Player_AttackRefs.AttackActionTypeEnum.Charged:

                    #region Play Big Hit Animation.
                    PlayFallBackAnim(e_armed_hit_big_f_hash);
                    FallbackLookTowardPlayer();
                    #endregion

                    break;
            }
        }

        public override void HandleUnarmedGetHitAnimation()
        {
        }

        public override void PlayOnDeathAnimation()
        {
            egilStaminaMod.HideInjuredLoopingFxImmediately();
            FallbackLookTowardPlayer();
            Default_PlayArmedDeathAnim();
        }

        void HandleEgil_2_3_PhaseGetHitAnimation()
        {
            switch (_hitSourceAttackRefs._attackActionType)
            {
                case Player_AttackRefs.AttackActionTypeEnum.Normal:

                    #region Play Small Hit Animation.
                    switch (_hitSourceAttackRefs._attackDirectionType)
                    {
                        case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                            PlayAnimation_NoNeglect(e_armed_hit_small_r_hash, false);
                            break;
                        case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                            PlayAnimation_NoNeglect(e_armed_hit_small_l_hash, false);
                            break;
                        case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                            PlayAnimation_NoNeglect(e_armed_hit_small_f_hash, false);
                            break;
                    }
                    #endregion

                    break;

                case Player_AttackRefs.AttackActionTypeEnum.Hold:
                case Player_AttackRefs.AttackActionTypeEnum.Charged:

                    #region Play Big Hit Animation.
                    PlayFallBackAnim(e_armed_hit_big_f_hash);
                    FallbackLookTowardPlayer();
                    #endregion

                    break;
            }
        }

        void FallbackLookTowardPlayer()
        {
            float _angle = Vector3.SignedAngle(mTransform.forward, dirToTarget, vector3Up);
            LeanTween.rotateAroundLocal(gameObject, vector3Up, _angle, 0.25f).setEaseOutQuart();
        }
        #endregion

        #region Turn With Agent.
        public override void TurnWithAgent()
        {
            if (isPausingTurnWithAgent)
                return;

            CalculatingCurrentUpperBodyIKTurningSpeed();

            if (isMovingToward)
            {
                TurningWhileManeuvering();
            }
            else if (!limitEnemyTurning._isNeglectingIdleAnimTurning)
            {
                TurnWithAnim_Limited();
            }
        }
        
        void TurnWithAnim_Limited()
        {
            if (isLockOnMoveAround)
            {
                if (angleToTarget > animInplaceRotateThershold)
                {
                    AggroTurnWithInplaceAnimation();
                    limitEnemyTurning.SetIsCountingAnimTurningToTrue();
                }
            }
            else
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

                    limitEnemyTurning.SetIsCountingAnimTurningToTrue();
                }
            }
        }
        
        protected override void AggroTurnWithRootAnimation()
        {
            if (IsTargetOnLeftSide())
            {
                PlayAnimationCrossFade(hashManager.e_armed_turn_left_90_hash, 0.2f, true);
            }
            else
            {
                PlayAnimationCrossFade(hashManager.e_armed_turn_right_90_hash, 0.2f, true);
            }
        }

        protected override void GetInplaceTurningAnimation()
        {
            if (IsTargetOnLeftSide())
            {
                //PlayAnimationWithCrossFade(e_armed_turn_left_inplace_hash, 0.2f, true);
                PlayAnimation(hashManager.e_armed_turn_left_inplace_hash, true);
            }
            else
            {
                //PlayAnimationWithCrossFade(e_armed_turn_right_inplace_hash, 0.2f, true);
                PlayAnimation(hashManager.e_armed_turn_right_inplace_hash, true);
            }
        }
        #endregion

        #region Anim With Agent.
        public override void AnimWithAgent()
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                StopLocomotion();
            }
            else
            {
                if (isMovingToward)
                {
                    UpdateLocomotionDualDis();
                }
                else if (isLockOnMoveAround)
                {
                    UpdateLockOnLocomotion();
                }
            }
        }
        #endregion

        #region Move With Agent.
        public override void MoveWithAgent()
        {
            if (isMovingToward)
            {
                UpdateAgentSpeedMoveToward();
                UpdateMoveTowardAgent();
            }
            else if (isLockOnMoveAround)
            {
                UpdateAgentSpeedLockonMove();
                UpdateLockOnAgent();
            }

            MonitorIsMovementChanged();

            void MonitorIsMovementChanged()
            {
                if (!isMovementChanged)
                {
                    if (_frameCount % 3 == 0)
                    {
                        aIStates.ClearAgentPath();
                    }
                }

                isMovementChanged = false;
            }
        }
        #endregion

        #region Egil Stamina Mod.
        public override void DepleteEnemyStamina(float staminaUsage)
        {
            egilStaminaMod.DepleteEgilStamina(staminaUsage);
        }

        public override bool GetIsEnemyTiredBool()
        {
            return egilStaminaMod.isEgilTired;
        }
        
        public override void EgilStaminaMod_SetNewPhaseData_2ndPhase(EgilStaminaMod_2ndPhase_EP_Data _egilStaminaMod_2P_EP_Data)
        {
            egilStaminaMod.SetNewPhaseData_2ndPhase(_egilStaminaMod_2P_EP_Data);
        }

        public override void EgilStaminaMod_SetNewPhaseData_3rdPhase(EgilStaminaMod_3rdPhase_EP_Data _egilStaminaMod_3P_EP_Data)
        {
            egilStaminaMod.SetNewPhaseData_3rdPhase(_egilStaminaMod_3P_EP_Data);
        }

        /// Injured State Tick.

        // Tick.
        public override void InjuredStateTick()
        {
            UpdateModsDeltas();
            
            egilStaminaMod.MonitorEgilInjuredRecovery();
        }

        // After Injured Recovered.
        public override void EgilInjuryRecovered()
        {
            OnRecoveredHandleFx();
            OnRecoveredStartRevengeAttackCounter();

            void OnRecoveredHandleFx()
            {
                StopInjuredLoopingFx();
                Play3rdPhaseAuraFx();

                void StopInjuredLoopingFx()
                {
                    egilStaminaMod.egilInjuredLoopingFx.Stop();
                }

                void Play3rdPhaseAuraFx()
                {
                    aISessionManager._egilPhaseChangeAuraFx.Play();
                }
            }

            void OnRecoveredStartRevengeAttackCounter()
            {
                LeanTween.value(0, 1, 0.7f).setOnComplete(egilStaminaMod.OnInjuredRecoveredCounterComplete);
            }
        }

        // AI Action.

        public override void EgilEnterInjuredState()
        {
            OnEnterInjuredStateReset();
            OnEnterInjuredHandleFx();
            ChangeToInjuredState();
            PlayInjuredIdleAnim();

            void OnEnterInjuredStateReset()
            {
                isPausingTurnWithAgent = true;
                iKHandler.isUsingIK = false;
            }
            
            void OnEnterInjuredHandleFx()
            {
                Stop3rdPhaseAuraFx();
                LeanTween.value(1, 0, 0.85f).setOnComplete(PlayInjuredLoopingFx);

                void Stop3rdPhaseAuraFx()
                {
                    aISessionManager._egilPhaseChangeAuraFx.Stop();
                }

                void PlayInjuredLoopingFx()
                {
                    Transform _loopFxTransform = egilStaminaMod.egilInjuredLoopingFx.transform;
                    _loopFxTransform.parent = mTransform;

                    Vector3 _targetPos = vector3Zero;
                    _targetPos.y = 0.13f;
                    _loopFxTransform.localPosition = _targetPos;

                    _loopFxTransform.gameObject.SetActive(true);
                    egilStaminaMod.egilInjuredLoopingFx.Play();
                }
            }

            void ChangeToInjuredState()
            {
                aIStates.currentState = egilStaminaMod.egilInjuredState;
            }

            void PlayInjuredIdleAnim()
            {
                anim.CrossFade(hashManager.egil_injured_hash, 0.05f);
            }
        }

        public override void InjuredRecovered_OnAggroStateReset()
        {
            anim.SetBool(egilStaminaMod.e_mod_IsTired_hash, false);
            OnAggroStateResets();
        }
        #endregion

        #region Roll Interval Mod.
        public override bool GetEnemyRolledBool()
        {
            return rollIntervalMod.enemyRolled;
        }

        public override void SetEnemyRolledBoolToTrue()
        {
            rollIntervalMod.SetEnemyRolledBoolToTrue();
        }
        
        public override void RollIntervalMod_SetNewPhaseData(RollIntervalMod_EP_Data _rollInterval_ep_data)
        {
            rollIntervalMod.SetNewPhaseData(_rollInterval_ep_data);
        }
        #endregion

        #region Egil Execution Mod.
        public override void SetIsExecutePresentAttackToFalse()
        {
            egilExecutionMod.OffExecutePresentAttack();
        }

        public override bool GetIsExecutePresentAttack()
        {
            return egilExecutionMod._isExecutePresentAttack;
        }

        public override bool GetIsExecutionWait()
        {
            return egilExecutionMod._isExecuteWait;
        }
        
        public override void TryCatchPlayerToExecute()
        {
            egilExecutionMod.TryCatchPlayerToExecute();
        }

        public override void MSA_TryCatchPlayerToExecute()
        {
            egilExecutionMod.MSA_TryCatchPlayerToExecute();
        }

        public override void OnSucessfulCaughtPlayer()
        {
            egilExecutionMod.OnSucessfulCaughtPlayer();
        }

        public override void SetExecutionDamageColldierToTrue()
        {
            egilExecutionMod.OnExecutePresentAttack();
            currentWeapon.SetColliderStatusToTrue();
        }

        public override void SetExecutionDamageColldierToFalse()
        {
            egilExecutionMod.OffExecutePresentAttack();
            currentWeapon.SetColliderStatusToFalse();
        }
        #endregion

        #region Perilous Attack Mod.
        public override void PerliousAttackMod_SetNewPhaseData(PerliousAttackMod_EP_Data _perliousAttackMod_EP_Data)
        {
            perilousAttackMod.SetNewPhaseData(_perliousAttackMod_EP_Data);
        }

        public override bool GetUsedPerilousAttackBool()
        {
            return perilousAttackMod.usedPerilousAttack;
        }

        public override void SetUsedPerilousAttackToTrue()
        {
            perilousAttackMod.SetUsedPerilousAttackToTrue();
        }
        #endregion

        #region Knock Down Player Mod.
        public override bool GetIsKnockDownPlayerWait()
        {
            return knockDownPlayerMod._isKnockDownPlayerWait;
        }

        public override void SetIsKnockedDownPlayerToTrue()
        {
            knockDownPlayerMod.SetIsKnockedDownPlayerToTrue();
        }
        #endregion

        #region Egil Kinematic Motion Attack Mod.
        public override void Execute_KMJ(_KMA_ActionData _KMA_profile)
        {
            egil_KMA_Mod.Execute_KMJ_ForAttack(_KMA_profile);
        }

        public override void PhaseChange_Execute_KMJ(bool _is_KMA_PerliousAttack, bool _isUsedAsMSACombo)
        {
            egil_KMA_Mod.Execute_KMJ_Attack_ForPhaseChange();
        }

        public override void KMJ_ApplyRootMotion_InEvent()
        {
            egil_KMA_Mod.KMJ_ComputeMotionVelocity_InEvent();
        }

        public override void KMA_ApplyRootMotion_InEvent()
        {
            egil_KMA_Mod.KMA_ComputeMotionVelocity();
        }

        public override void KMA_ResetTopDownRotation()
        {
            egil_KMA_Mod.KMA_ResetTopDownRotation();
        }
        
        public override void Egil_KMA_Mod_SetNewPhaseData(Egil_KMA_Mod_EP_Data _egil_KMA_Mod_EP_Data)
        {
            egil_KMA_Mod.SetNewPhaseData(_egil_KMA_Mod_EP_Data);
        }

        public override bool GetCanExitKMAState()
        {
            return egil_KMA_Mod._canExit_KMA_State;
        }

        /// Extra Damage Collider Detection.
        public override void Set_KMA_EnemyDamageColliderStatusToTrue()
        {
            currentWeapon.SetColliderStatusToTrue();
            egil_KMA_Mod._startOverlapBoxHitDetect = true;
        }

        public override void Set_KMA_EnemyDamageColliderStatusToFalse()
        {
            currentWeapon.SetColliderStatusToFalse();
            egil_KMA_Mod.On_KMA_AttackFinish();
        }

        public override void MonitorOverlapBoxDamageCollider()
        {
            egil_KMA_Mod.TickOverlapBoxCollider();
        }
        
        #region Play Animations.
        public override void Play_KMJ_1stHalfAnim()
        {
            if (GetIsIn2ndPhaseBool())
            {
                PlayAnimationCrossFade_NoNeglect(hashManager.egil_P2_KMJ_1stHalf_hash, 0.1f, false);
            }
            else
            {
                PlayAnimationCrossFade_NoNeglect(hashManager.egil_P3_KMJ_1stHalf_hash, 0.1f, false);
            }
        }

        public override void Play_KMJ_2ndHalfAnim()
        {
            if (GetIsIn2ndPhaseBool())
            {
                PlayAnimation_NoNeglect(hashManager.egil_P2_KMJ_2ndHalf_hash, false);
            }
            else
            {
                PlayAnimation_NoNeglect(hashManager.egil_P3_KMJ_2ndHalf_hash, false);
            }
        }

        public override void Play_KMJ_LandAnim()
        {
            if (GetIsIn2ndPhaseBool())
            {
                PlayAnimation(hashManager.egil_P2_KMJ_land_hash, false);
            }
            else
            {
                PlayAnimation(hashManager.egil_P3_KMJ_land_hash, false);
            }

            anim.SetFloat(vertical_hash, 0);
            anim.SetFloat(horizontal_hash, 0);
        }
        #endregion

        #endregion

        #region Enemy Enumerable Phase Mod.

        #region Enemy Change To...Phase.
        public override void EnemyChangeTo2ndPhase()
        {
            currentAction = null;
            currentMultiStageAttack = null;
            skippingScoreCalculation = true;

            isTrackingPlayer = false;
            currentFallbackVelocity = changeToVelocity;

            aIStates.Set_AnimMoveRmType_ToFallback();
            aIStates.e_rb.velocity = vector3Zero;

            enemyCentralPhaseMod.SwitchToPhase2();
            enemyCentralPhaseMod.PrePhaseChangedRestrainTurning();
            anim.Play(hashManager.egil_1P_empty_override_hash);

            iKHandler.isUsingIK = true;
            isPausingTurnWithAgent = false;

            PlayAnimation(enemy2ndPhaseEpMod._phaseChangeAnim.animStateHash, true);

            FallbackLookTowardPlayer();
        }

        public override void EnemyChangeTo3rdPhase()
        {
            currentAction = null;
            currentMultiStageAttack = null;
            skippingScoreCalculation = true;

            isTrackingPlayer = false;
            aIStates.e_rb.velocity = vector3Zero;

            aIStates.Set_AnimMoveRmType_ToNull();

            enemyCentralPhaseMod.SwitchToPhase3();
            anim.Play(hashManager.egil_2P_empty_override_hash);

            iKHandler.isUsingIK = false;
            isPausingTurnWithAgent = true;

            anim.SetFloat(vertical_hash, 0);
            anim.SetFloat(horizontal_hash, 0);
            PlayAnimationCrossFade(enemy3rdPhaseEpMod._phaseChangeAnim.animStateHash, 0.1f, false);
        }
        #endregion

        #region On...Phase Change End.
        public override void On2ndPhaseChangeEnd()
        {
            aIStates.Set_AnimMoveRmType_ToNull();
            enemyCentralPhaseMod._isChangingPhase = false;
            currentFallbackVelocity = _2P_fallbackVelocity;

            iKHandler.isUsingIK = false;
            isPausingTurnWithAgent = true;

            egil_KMA_Mod.Execute_KMJ_ForPhaseChange();
        }

        public override void On3rdPhaseChangeEnd()
        {
            skippingScoreCalculation = false;
            LevelAreaFxManager.singleton.PlayEgil3rdPhaseSnowFx();
        }
        #endregion

        #region Get Is In...Phase Bool.
        public override bool GetIsIn2ndPhaseBool()
        {
            return enemyCentralPhaseMod._currentEnemyPhaseIndex == 2;
        }

        public override bool GetIsIn3rdPhaseBool()
        {
            return enemyCentralPhaseMod._currentEnemyPhaseIndex == 3;
        }
        #endregion

        #region On...Phase Change Execute Passive Action.
        public override void On3rdPhaseChangeExecutePassiveAction()
        {
            enemy3rdPhaseEpMod.Execute_EP_AI_PassiveAction_1();
        }
        #endregion
        
        public override void EnemyEnumerablePhaseGoesAggroReset_ResetAnimPara()
        {
            anim.SetBool(hashManager.e_mod_isIn2ndPhase_hash, false);
            anim.SetBool(hashManager.e_mod_isIn3rdPhase_hash, false);
        }

        public override void Egil3rdPhaseChangeEnd_ParentWeaponOnHand()
        {
            enemyCentralPhaseMod._isChangingPhase = false;
            firstWeapon.ParentEnemyWeaponUnderHand();
        }

        /// Execute In 1st PhaseChange KMJ Landed.
        public override void OnPhaseChangedResetStatus()
        {
            enemyCentralPhaseMod.PostPhaseChangedRestoreTuring();
        }
        #endregion

        #region R Leg Damage Collider Mod.
        public override void Enable_R_Leg_DamageCollider()
        {
            r_leg_damageColliderMod.Enable_R_Leg_DamageCollider();
        }

        public override void Disable_R_Leg_DamageCollider()
        {
            r_leg_damageColliderMod.Disable_R_Leg_DamageCollider();
        }
        #endregion

        #region L Shoulder Damage Collider Mod.
        public override void Enable_L_Shoulder_DamageCollider()
        {
            l_shoulder_damageColliderMod.Enable_L_Shoulder_DamageCollider();
        }

        public override void Disable_L_Shoulder_DamageCollider()
        {
            l_shoulder_damageColliderMod.Disable_L_Shoulder_DamageCollider();
        }
        #endregion

        #region On Boss Killed Player.
        public override bool IsExitAggroTransition_OnPlayerDead()
        {
            if (playerStates.isDead)
            {
                OnBossKilledPlayer();
                return true;
            }

            return false;
        }

        void OnBossKilledPlayer()
        {
            OnPlayKilledHideDisplayManager();
            OnPlayKilledResetBools();
            OnPlayerKilledResetAnimPara();
            OnExitAggroFacedPlayerReset();
        }

        void OnPlayKilledHideDisplayManager()
        {
            aIStates.aiDisplayManager.OnPlayerDeath();
        }

        void OnPlayKilledResetBools()
        {
            // Agent.
            agent.enabled = false;
            
            // Booleans
            isMovingToward = false;
            isMovingTowardPlayer = false;
            isLockOnMoveAround = false;
            
            isTrackingPlayer = false;
            isPausingTurnWithAgent = false;

            // Action
            currentAction = null;
            currentPassiveAction = null;
            currentMultiStageAttack = null;

            // Attack Interval.
            enemyAttacked = false;
            attackIntervalTimer = 0;

            _idleTransitTimer = 0;

            // AISessionManager.
            aISessionManager.OnBossFightEnded_SetManagablesStatus();
        }
        
        void OnPlayerKilledResetAnimPara()
        {
            anim.SetFloat(vertical_hash, 0);
            anim.SetFloat(horizontal_hash, 0);

            anim.SetBool(aIStates.e_IsInteracting_hash, false);
            anim.SetBool(e_IsLockOnMoveAround_hash, false);
            anim.SetBool(e_IsFacedPlayer_hash, false);
        }
        #endregion

        #region On Boss Checkpoint Refresh.
        void SubscribleCheckpointRefreshEvent()
        {
            playerStates.CheckpointRefreshEvent += OnBossCheckpointRefresh;
        }

        void UnSubscribleCheckpointRefreshEvent()
        {
            playerStates.CheckpointRefreshEvent -= OnBossCheckpointRefresh;
        }

        void OnBossCheckpointRefresh()
        {
            CheckpointRefresh_BossStates();

            CheckpointRefresh_BossAIManager();

            CheckpointRefresh_ManagableStatus();

            CheckpointRefresh_RemoveAnimator();

            CheckpointRefresh_PlayIntroPoses();

            UnSubscribleCheckpointRefreshEvent();
        }

        void CheckpointRefresh_BossStates()
        {
            aIStates.Boss_CheckpointRefresh();
        }

        void CheckpointRefresh_BossAIManager()
        {
            distanceToTarget = 20;

            currentEnemyHealth = totalEnemyHealth;
            currentCrossFadeLayer = defaultCrossFadeLayer;
            currentActionHolder = firstWeaponActionHolder;

            aIStates.aiDisplayManager.SetHealthBarValueToFullImmediately();

            enemyCentralPhaseMod.CheckpointRefresh_ReversePhaseChanges();
        }

        void CheckpointRefresh_ManagableStatus()
        {
            aISessionManager.OnBossCheckpointRefresh();
        }

        void CheckpointRefresh_RemoveAnimator()
        {
            Destroy(a_hook);
        }

        void CheckpointRefresh_PlayIntroPoses()
        {
            /// Play Intro Poses
            anim.Play(hashManager.egil_intro_pose_1_hash);
        }

        #endregion

        #region On Enemy Death Turn Off Damage Collider.
        protected override void OnDeathTurnOffDamageCollider()
        {
            base.OnDeathTurnOffDamageCollider();

            r_leg_damageColliderMod.R_Leg_DamageColliderOnDeathReset();
            l_shoulder_damageColliderMod.L_Shoulder_DamageColliderOnDeathReset();
        }
        #endregion

        #region On Boss Death Reset Status.
        public override void OnBossDeath_SetStatus()
        {
            OnExitAggroFacedPlayerReset();
        }
        #endregion
        
        public override void OnExitAggroFacedPlayerReset()
        {
            ModsExitAggroResets();
        }

        void ModsExitAggroResets()
        {
            egilStaminaMod.StaminaUsageExitAggroReset();

            rollIntervalMod.RollIntervalExitAggroReset();

            egilExecutionMod.EgilExecutionExitAggroReset();

            perilousAttackMod.PerilousAttackExitAggroReset();

            knockDownPlayerMod.KnockedDownPlayerExitAggroReset();

            egil_KMA_Mod.Egil_KMA_ExitAggroReset();

            r_leg_damageColliderMod.R_Leg_DamageColliderOnDeathReset();

            l_shoulder_damageColliderMod.L_Shoulder_DamageColliderOnDeathReset();
        }

        #region Indicators.
        public override void Play_PerliousAttackIndicator()
        {
            perilousAttackMod.perilousATKIndicator.SetActive(true);
        }
        #endregion
    }
}