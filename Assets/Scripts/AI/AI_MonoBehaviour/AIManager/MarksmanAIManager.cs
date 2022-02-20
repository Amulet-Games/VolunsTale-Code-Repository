using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MarksmanAIManager : AIManager
    {
        #region Indicators.
        public GameObject rh_ParryIndicator;
        public GameObject lh_ParryIndicator;
        #endregion

        [Header("IEnemyRollInterval")]
        public RollIntervalMod rollIntervalMod;

        [Header("IEnemyAiming")]
        public AimingPlayerMod aimingPlayerMod;

        [Header("IFullBodyDamageCollider")]
        public FullBodyDamageColliderMod fullBodyDamageColliderMod;

        [Header("IEnemyInteractable")]
        public EnemyInteractableMod enemyInteractableMod;

        [Header("ILimitEnemyTurning")]
        public LimitEnemyTurningMod limitEnemyTurning;

        #region Init.
        public override void AIModsOnEnterAggroFacedPlayer()
        {
            rollIntervalMod.RollIntervalGoesAggroReset();
            enemyInteractableMod.EnemyInteractableGoesAggroReset();
            limitEnemyTurning.LimitEnemyTurningModGoesAggroReset();
        }

        public override void SetupAIMods()
        {
            aimingPlayerMod.AimingPlayerModInit(this);
            fullBodyDamageColliderMod.FullBodyDamageColliderModInit(this);
            enemyInteractableMod.EnemyInteractableInit(this);
        }
        #endregion

        #region AI Ticks.
        public override void Tick()
        {
            UpdateAggroStateResets();

            enemyInteractableMod.MonitorInteractables();

            ResetUpdateLockOnPosBool();
            
            CheckReacquireThrowableWeapon();

            AttackIntervalTimeCount();

            limitEnemyTurning.TrackIdleAnimTurning();

            rollIntervalMod.RollIntervalTimeCount();

            aimingPlayerMod.AimingPlayerTimeDistanceCheck();

            GetNextAction();

            MonitorLockOnLocomotionType();

            UpdateAggroStateIK();

            SetEnemyAnim();
        }
        
        protected override void UpdateModsDeltas()
        {
            rollIntervalMod._delta = _delta;
            aimingPlayerMod._delta = _delta;
            enemyInteractableMod._delta = _delta;
            limitEnemyTurning._delta = _delta;
        }
        #endregion
        
        #region isInteracting Ticks.
        public override void IsInteracting_Tick()
        {
            base.IsInteracting_Tick();

            limitEnemyTurning.TrackIdleAnimTurning();

            rollIntervalMod.RollIntervalTimeCount();
        }
        #endregion

        #region On Hit.
        protected override void OnHitAIMods()
        {
            if (_isHitByChargeAttack)
            {
                enemyInteractableMod.OnChargedAttackHit();
            }
        }

        protected override void PlayOnHitAnimation()
        {
            if (isWeaponOnHand)
            {
                HandleArmedGetHitAnimation();
            }
            else
            {
                HandleUnarmedGetHitAnimation();
            }
        }
        
        public override void HandleArmedGetHitAnimation()
        {
            if (_isHitByChargeAttack)
            {
                Play_Defualt_ArmedKnockDownAnim();
            }
            else
            {
                if (!_isSkippingOnHitAnim)
                {
                    PlayArmedGetHitAnim();
                }
            }

            void PlayArmedGetHitAnim()
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

                        if (playerStates._hasHoldAtkReachedMaximum)
                        {
                            #region Play Big Hit Animation.
                            switch (_hitSourceAttackRefs._attackDirectionType)
                            {
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                    PlayFallBackAnim(e_armed_hit_big_r_hash);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                    PlayFallBackAnim(e_armed_hit_big_l_hash);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                    PlayFallBackAnim(e_armed_hit_big_f_hash);
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
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
                        }
                        break;
                }
            }
        }

        public override void HandleUnarmedGetHitAnimation()
        {
            if (_isHitByChargeAttack)
            {
                Play_Defualt_UnarmedKnockDownAnim();
            }
            else
            {
                if (!_isSkippingOnHitAnim)
                {
                    PlayUnarmedGetHitAnim();
                }
            }

            void PlayUnarmedGetHitAnim()
            {
                switch (_hitSourceAttackRefs._attackActionType)
                {
                    case Player_AttackRefs.AttackActionTypeEnum.Normal:

                        #region Play Unarmed Small Hit Aniamtion.
                        switch (_hitSourceAttackRefs._attackDirectionType)
                        {
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                PlayAnimation_NoNeglect(e_unarmed_hit_small_r_hash, false);
                                break;
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                PlayAnimation_NoNeglect(e_unarmed_hit_small_l_hash, false);
                                break;
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                PlayAnimation_NoNeglect(e_unarmed_hit_small_f_hash, false);
                                break;
                        }
                        #endregion

                        break;

                    case Player_AttackRefs.AttackActionTypeEnum.Hold:

                        if (playerStates._hasHoldAtkReachedMaximum)
                        {
                            #region Play Unarmed Big Hit Aniamtion.
                            switch (_hitSourceAttackRefs._attackDirectionType)
                            {
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                    PlayFallBackAnim(e_unarmed_hit_big_r_hash);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                    PlayFallBackAnim(e_unarmed_hit_big_l_hash);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                    PlayFallBackAnim(e_unarmed_hit_big_f_hash);
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Play Unarmed Small Hit Aniamtion.
                            switch (_hitSourceAttackRefs._attackDirectionType)
                            {
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                    PlayAnimation_NoNeglect(e_unarmed_hit_small_r_hash, false);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                    PlayAnimation_NoNeglect(e_unarmed_hit_small_l_hash, false);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                    PlayAnimation_NoNeglect(e_unarmed_hit_small_f_hash, false);
                                    break;
                            }
                            #endregion
                        }
                        break;
                }
            }
        }

        public override void PlayOnDeathAnimation()
        {
            if (isWeaponOnHand)
            {
                if (enemyInteractableMod.currentPowerWeapon)
                {
                    PowerWeapon_PlayPowerWeaponDeathAnim();
                }
                else
                {
                    PowerWeapon_PlayArmedDeathAnim();
                }
            }
            else
            {
                PowerWeapon_PlayUnarmedDeathAnim();
            }
        }
        #endregion

        #region Turn With Agent.
        public override void TurnWithAgent()
        {
            if (isPausingTurnWithAgent)
                return;

            if (aimingPlayerMod.isAiming)
            {
                SetFixedUpperBodyIKTurningSpeedWhenAiming();
            }
            else
            {
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
        }

        void SetFixedUpperBodyIKTurningSpeedWhenAiming()
        {
            currentUpperBodyIKTurningSpeed = 3f;
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
                if (isWeaponOnHand)
                {
                    aimingPlayerMod.RotateWithRootAnimation_TurnLeft();
                }
                else
                {
                    PlayAnimationCrossFade(hashManager.e_unarmed_turn_left_90_hash, 0.2f, true);
                }
            }
            else
            {
                if (isWeaponOnHand)
                {
                    aimingPlayerMod.RotateWithRootAnimation_TurnRight();
                }
                else
                {
                    PlayAnimationCrossFade(hashManager.e_unarmed_turn_right_90_hash, 0.2f, true);
                }
            }
        }

        protected override void GetInplaceTurningAnimation()
        {
            if (IsTargetOnLeftSide())
            {
                if (isWeaponOnHand)
                {
                    aimingPlayerMod.RotateWithInplaceAnimation_TurnLeft();
                }
                else
                {
                    PlayAnimationCrossFade(hashManager.e_unarmed_turn_left_inplace_hash, 0.2f, true);
                }
            }
            else
            {
                if (isWeaponOnHand)
                {
                    aimingPlayerMod.RotateWithInplaceAnimation_TurnRight();
                }
                else
                {
                    PlayAnimationCrossFade(hashManager.e_unarmed_turn_right_inplace_hash, 0.2f, true);
                }
            }
        }

        public override void TurningWhileIdleWithIK()
        {
            if (isPausingTurnWithAgent || isMovingToward)
                return;

            if (angleToTarget < animInplaceRotateThershold)
            {
                iKHandler.isEnemyForwardIK = false;

                if (aimingPlayerMod.isAiming || angleToTarget >= upperBodyIKRotateThershold)
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
                    Marksman_UpdateLockOnLocomotion();
                }
            }
        }

        protected void Marksman_UpdateLockOnLocomotion()
        {
            float agentVel = Mathf.Clamp01(_currentAgentVelocity);
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
                    anim.SetFloat(horizontal_hash, 0, 0.2f, _delta);
                    anim.SetFloat(vertical_hash, agentVel * -0.5f, 0.1f, _delta);
                    break;
            }
        }
        #endregion

        #region Move With Agent.
        public override void MoveWithAgent()
        {
            if (isMovingToward)
            {
                UpdateAgentSpeedMoveToward();
                Update_MultiTarget_MoveTowardableAgent();
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
                        //Debug.Log("ClearAgentPath");
                        aIStates.ClearAgentPath();
                    }
                }

                isMovementChanged = false;
            }
        }
        
        void Update_MultiTarget_MoveTowardableAgent()
        {
            if (isMovingTowardPlayer)
            {
                UpdateMoveTowardAgent();
            }
            else
            {
                SetMoveTowardEnemyInterPos();
            }

            void SetMoveTowardEnemyInterPos()
            {
                //Debug.Log("isMovingToward");
                agent.isStopped = false;
                agent.stoppingDistance = 0;

                agent.SetDestination(targetPos);
                UpdateMoveTowardPosition();

                isMovementChanged = true;
            }
        }
        #endregion

        #region Weapon.
        void SheathCurrentEmptyWeapon()
        {
            /// For Marksman Sheath Fist Weapon Without Parent it to position.
            currentWeapon = null;
            isWeaponOnHand = false;
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
        #endregion

        #region Aiming Player Mod.
        public override void OnAiming()
        {
            aimingPlayerMod.OnAiming();
        }

        public override void OffAiming()
        {
            aimingPlayerMod.OffAiming();
        }

        public override void CorrectWeaponTransformWhenAiming()
        {
            WeaponOpposedHandTransform _aimingTransform = aimingPlayerMod.aimingHandPosition;
            currentThrowableWeapon.transform.localPosition = _aimingTransform.pos;
            currentThrowableWeapon.transform.localEulerAngles = _aimingTransform.eulers;
        }

        public override void ReverseWeaponTransformQuitAiming()
        {
            currentThrowableWeapon.transform.localPosition = vector3Zero;
            currentThrowableWeapon.transform.localEulerAngles = vector3Zero;
        }
        #endregion

        #region FullBody Damage Collider Mod.
        /// R Arm. 
        public override void FullBody_DC_Mod_Enable_R_Arm_DC()
        {
            fullBodyDamageColliderMod.Enable_R_Arm_DamageCollider();
        }

        public override void FullBody_DC_Mod_Disable_R_Arm_DC()
        {
            fullBodyDamageColliderMod.Disable_R_Arm_DamageCollider();
        }

        /// L Arm. 
        public override void FullBody_DC_Mod_Enable_L_Arm_DC()
        {
            fullBodyDamageColliderMod.Enable_L_Arm_DamageCollider();
        }

        public override void FullBody_DC_Mod_Disable_L_Arm_DC()
        {
            fullBodyDamageColliderMod.Disable_L_Arm_DamageCollider();
        }

        /// R Leg. 
        public override void FullBody_DC_Mod_Enable_R_Leg_DC()
        {
            fullBodyDamageColliderMod.Enable_R_Leg_DamageCollider();
        }

        public override void FullBody_DC_Mod_Disable_R_Leg_DC()
        {
            fullBodyDamageColliderMod.Disable_R_Leg_DamageCollider();
        }

        /// L Leg. 
        public override void FullBody_DC_Mod_Enable_L_Leg_DC()
        {
            fullBodyDamageColliderMod.Enable_L_Leg_DamageCollider();
        }

        public override void FullBody_DC_Mod_Disable_L_Leg_DC()
        {
            fullBodyDamageColliderMod.Disable_L_Leg_DamageCollider();
        }
        #endregion

        #region Enemy Interactable Mod.

        #region Update Dir.Angle.Dis.
        protected override void UpdateCurrentTargetPos()
        {
            if (enemyInteractableMod.switchTargetToInteractable)
            {
                // target position will be the interactable
                targetPos = enemyInteractableMod.desired_interactable.transform.position;
            }
            else
            {
                // otherwise target position will be the player
                targetPos = playerStates.mTransform.position;
            }
        }
        #endregion

        #region Interactables.
        public override void ExecuteInteractable()
        {
            enemyInteractableMod.ExecuteInteractable();
        }
        #endregion

        #region AI Actions.
        public override void ExecutePowerWeaponInteractable(PowerWeapon_Interactable _powerWeaponInteractable)
        {
            enemyInteractableMod.currentPowerWeaponInteractable = _powerWeaponInteractable;
            Set_EquipPowerWeapon_PassiveAction();

            if (isWeaponOnHand)
                PlaySheathAnimation();

            currentWeapon = null;
        }
        #endregion

        #region Anim Events.

        #region Get Power Weapon.
        public override void SetIsInGettingInterAnimToTrue()
        {
            enemyInteractableMod.isPickingUpPowerWeapon = true;
        }

        public override void GetThrowablePowerWeapon()
        {
            enemyInteractableMod.GetPowerWeapon();
        }

        public override void SetSwitchTargetToInteractableToFalse()
        {
            enemyInteractableMod.SetSwitchTargetToInteractableToFalse();
        }
        #endregion

        #region PW Attacks.
        public override void BreakPowerWeapon()
        {
            enemyInteractableMod.BreakPowerWeaponWhenIdle();
        }
        
        public override void SetPowerWeaponMSA_Available()
        {
            enemyInteractableMod.SetPowerWeaponMSA_Available();
        }

        public override void PowerWeaponDamageColliderStatus(int v)
        {
            if (!isDead)
            {
                enemyInteractableMod.PowerWeaponDamageColliderStatus(v);
            }
        }
        #endregion

        #region Get Charge Attacked.
        public override void BreakPowerWeaponByChargeAttack()
        {
            enemyInteractableMod.BreakPowerWeaponWhenKnockedDown();
        }
        #endregion

        #region After Thrown.
        public override void ClearPowerWeaponRefsAfterThrown()
        {
            enemyInteractableMod.ClearPowerWeaponRefsAfterThrown();
        }
        #endregion

        #endregion

        #region Others.
        /// AI PW Attack Action.
        public override void DepletePowerWeaponDuability()
        {
            enemyInteractableMod.Deplete_PW_Duability();
        }
        #endregion

        #region Get Status.
        public override bool GetIsCurrentPowerWeaponBroke()
        {
            return enemyInteractableMod.isCurrentPowerWeaponBroke;
        }

        public override bool GetIsSwitchTargetToInteractable()
        {
            return enemyInteractableMod.switchTargetToInteractable;
        }
        #endregion

        #region Set Status.
        public override void SetIsCurrentPowerWeaponBrokeToTrue()
        {
            enemyInteractableMod.isCurrentPowerWeaponBroke = true;
        }
        #endregion

        #region Revive.
        public override void SheathCurrentWeaponToPosition()
        {
            if (enemyInteractableMod.currentPowerWeapon)
            {
                enemyInteractableMod.EnemyInteractable_HandleWeaponSheath();
            }
            else
            {
                SheathCurrentEmptyWeapon();
            }
        }
        #endregion
        
        #endregion

        #region On Enemy Death Turn Off Damage Collider.
        protected override void OnDeathTurnOffDamageCollider()
        {
            fullBodyDamageColliderMod.FullBodyDamageColliderOnDeathReset();
        }
        #endregion

        #region On Parry Execute States Turn Off Damage Collider.
        protected override void OnParryExecuteStateResetColliderStatus()
        {
            if (currentWeapon)
                currentWeapon.SetColliderStatusToFalse();
            else
                fullBodyDamageColliderMod.FullBodyDamageColliderOnDeathReset();
        }
        #endregion

        #region On / Off Exit Aggro Faced Player.
        public override bool IsExitAggroTransition_ByDistance()
        {
            if (enemyInteractableMod.desired_interactable)
            {
                if (Vector3.SqrMagnitude(playerStates.mTransform.position - mTransform.position) > (exitAggro_Thershold * exitAggro_Thershold))
                {
                    OnExitAggroFacedPlayerReset();
                    return true;
                }
            }
            else
            {
                if (distanceToTarget > exitAggro_Thershold)
                {
                    OnExitAggroFacedPlayerReset();
                    return true;
                }
            }

            return false;
        }

        public override void OnExitAggroFacedPlayerReset()
        {
            Base_OnExitAggroFacedPlayerResets();

            rollIntervalMod.RollIntervalExitAggroReset();

            aimingPlayerMod.AimingPlayerExitAggroReset();

            fullBodyDamageColliderMod.FullBodyDamageColliderOnDeathReset();

            enemyInteractableMod.EnemyInteractableExitAggroReset();
        }

        protected override void OffExitAggroFacePlayerSheathWeapon()
        {
            if (!isWeaponSheathAnimExecuted)
            {
                if (enemyInteractableMod.currentPowerWeapon)
                {
                    enemyInteractableMod.ReactToPowerWeaponBroken();
                }
                else
                {
                    SheathCurrentEmptyWeapon();
                    anim.SetBool(e_IsFacedPlayer_hash, true);
                }

                anim.SetBool(e_IsArmed_hash, false);
                isWeaponSheathAnimExecuted = true;
            }
        }
        #endregion

        #region Indicators.
        public override void Play_RH_ParryIndicator()
        {
            if (canPlayIndicator)
                rh_ParryIndicator.SetActive(true);
        }

        public override void Play_LH_ParryIndicator()
        {
            if (canPlayIndicator)
                lh_ParryIndicator.SetActive(true);
        }
        #endregion

        //public override void FindPlayerInPatrol()
        //{
        //    if (aIStates._aiGroupManagable.isForbiddenToFoundPlayer)
        //        return;

        //    /// If player is within aggro range.
        //    if (distanceToTarget < aggro_Thershold)
        //    {
        //        /// check if player is within the closet limit of aggro.
        //        if (distanceToTarget <= aggro_ClosestThershold)
        //        {
        //            if (CheckTargetIsBehindWall())
        //                return;

        //            OnEnterAggroFacedPlayerState();
        //        }
        //        /// if player is not within closet limit, check if angle within aggro angle.
        //        else if (angleToTarget <= aggro_Angle)
        //        {
        //            if (CheckTargetIsBehindWall())
        //                return;

        //            OnEnterAggroFacedPlayerState();
        //        }
        //    }
        //}
    }
}