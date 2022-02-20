using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ShieldmanAIManager : AIManager
    {
        #region Indicators.
        public GameObject rh_ParryIndicator;
        #endregion

        [Header("IEnemyBlocking")]
        public EnemyBlockingMod enemyBlockingMod;

        [Header("IEnemyPerilousAttack")]
        public PerilousAttackMod perilousAttackMod;

        [Header("IEnemyObservePlayerStatus")]
        public PlayerSpamBlockingMod playerSpamBlockingMod;

        [Header("IR_Leg_DamageCollider")]
        public R_Leg_DamageColliderMod r_leg_damageColliderMod;

        #region Init.
        public override void AIModsOnEnterAggroFacedPlayer()
        {
            perilousAttackMod.PerilousAttackGoesAggroReset();
            enemyBlockingMod.EnemyBlockingGoesAggroReset();
        }

        public override void SetupAIMods()
        {
            enemyBlockingMod.EnemyBlockingModInit(this);
            playerSpamBlockingMod.ObservePlayerModInit(playerStates);
            r_leg_damageColliderMod.R_Leg_DamageColliderModInit(this);
        }
        #endregion
        
        #region AI Ticks.
        public override void Tick()
        {
            UpdateAggroStateResets();

            ResetUpdateLockOnPosBool();
            
            AttackIntervalTimeCount();

            enemyBlockingMod.EnemyBlockingTick();

            perilousAttackMod.PerilousAttackTimeCount();

            playerSpamBlockingMod.UpdateSpammingCounter();

            GetNextAction();

            MonitorLockOnLocomotionType();

            UpdateAggroStateIK();

            SetEnemyAnim();
        }

        protected override void UpdateAggroStateResets()
        {
            Base_UpdateAggroStateResets();
        }

        protected override void UpdateModsDeltas()
        {
            enemyBlockingMod._delta = _delta;
            perilousAttackMod._delta = _delta;
            playerSpamBlockingMod._delta = _delta;
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

            perilousAttackMod.PerilousAttackTimeCount();

            playerSpamBlockingMod.UpdateSpammingCounter();
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
            else
            {
                TurningWhileIdleWithAnim();
            }
        }

        public override void TurningWhileIdleWithIK()
        {
            if (isPausingTurnWithAgent || isMovingToward)
                return;

            if (enemyBlockingMod.isEnemyBlocking)
            {
                iKHandler.TurnOnBodyRigIK();
                BlockingLerpToFaceTarget();
            }
            else if (angleToTarget < animInplaceRotateThershold)
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

            void BlockingLerpToFaceTarget()
            {
                mTransform.rotation = Quaternion.Lerp(mTransform.rotation, Quaternion.LookRotation(dirToTarget), _delta * enemyBlockingMod.maxUpperBodyIKBlockingTurnSpeed);
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

        #region Weapons.
        public override void SheathCurrentSidearmToPosition()
        {
            firstWeapon.SheathSidearmToPosition();
        }
        #endregion

        #region Play Animations.

        #region Attack On Interval.
        public override void PlayDefaultAttackAnim(BaseAttackOnInterval.AttackAnimStateEnum _targetAnimHash)
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayerTargetAnim();
            SetEnemyAttackedBoolToTrue();

            // Mods.
            enemyBlockingMod.SetIsInAttackBlockingToTrue();

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

        public override void CrossFadeDefaultAttackAnim(BaseAttackOnInterval.AttackAnimStateEnum _targetAnimHash, float _crossFadeValue)
        {
            isTrackingPlayer = true;
            _isSkippingOnHitAnim = true;

            PlayerTargetAnim();
            SetEnemyAttackedBoolToTrue();

            // Mods.
            enemyBlockingMod.SetIsInAttackBlockingToTrue();

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
        #endregion

        #region Ready.
        public override void PlayRollAttackReadyAnim(RollAttackReady.RollAttackReadyAnimStateEnum _targetAnimHash)
        {
            anim.SetFloat(vertical_hash, 0);
            anim.SetFloat(horizontal_hash, 0);

            PlayTargetAnim();

            // Mods.
            enemyBlockingMod.SetIsInAttackBlockingToTrue();

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
        #endregion

        #endregion

        #region On Hit.
        protected override void DepleteHealthFromDamage()
        {
            enemyBlockingMod.DepleteHealth_Blocking();
        }

        protected override void SpawnOnHitEffect()
        {
            enemyBlockingMod.SpawnOnHitEffect();
        }

        protected override void PlayOnHitAnimation()
        {
            if (isWeaponOnHand)
            {
                enemyBlockingMod.PlayBlockingOnHitAnimation();
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
                if (_isSkippingOnHitAnim)
                    return;

                PlayArmedGetHitAnim();
            }

            void PlayArmedGetHitAnim()
            {
                #region Play Hit Animation.
                switch (_hitSourceAttackRefs._attackActionType)
                {
                    case Player_AttackRefs.AttackActionTypeEnum.Normal:

                        #region Play Armed Small Hit Aniamtion.
                        switch (_hitSourceAttackRefs._attackDirectionType)
                        {
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                PlayAnimation_NoNeglect(e_armed_hit_small_r_hash, true);
                                break;
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                PlayAnimation_NoNeglect(e_armed_hit_small_l_hash, true);
                                break;
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                PlayAnimation_NoNeglect(e_armed_hit_small_f_hash, true);
                                break;
                        }
                        #endregion

                        break;

                    case Player_AttackRefs.AttackActionTypeEnum.Hold:

                        if (playerStates._hasHoldAtkReachedMaximum)
                        {
                            #region Play Armed Big Hit Aniamtion.
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
                            #region Play Armed Small Hit Aniamtion.
                            switch (_hitSourceAttackRefs._attackDirectionType)
                            {
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                    PlayAnimation_NoNeglect(e_armed_hit_small_r_hash, true);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                    PlayAnimation_NoNeglect(e_armed_hit_small_l_hash, true);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                    PlayAnimation_NoNeglect(e_armed_hit_small_f_hash, true);
                                    break;
                            }
                            #endregion
                        }
                        break;
                }
            }
            #endregion
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
                Default_PlayArmedDeathAnim();
            }
            else
            {
                Default_PlayUnarmedDeathAnim();
            }
        }
        #endregion

        #region Enemy Blocking Mod.
        public override bool GetIsEnemyBlockingBool()
        {
            return enemyBlockingMod.isEnemyBlocking;
        }

        public override bool GetEnemyBlockedBool()
        {
            return enemyBlockingMod.enemyBlocked;
        }

        public override void OnIsEnemyBlockingMoveAround()
        {
            enemyBlockingMod.OnIsEnemyBlocking();
            SetNewLockonPositionToAgent();          /// If Enemy Don't want to move aronud, just delete this line of code.
        }

        public override void OnBlockingChangeStatus()
        {
            iKHandler.iKRetargetStoppingDistance = 0;

            _currentAgentAccelSpeed = 5;
            _currentAgentMoveSpeed = 5;

            maxAttackIntervalRate = enemyBlockingMod.blockingAttackIntervalRate;
            minAttackIntervalRate = enemyBlockingMod.blockingAttackIntervalRandomizeAmount;

            anim.SetBool(hashManager.e_mod_IsBlocking_hash, true);
        }

        public override void OffBlockingReverseStatus()
        {
            iKHandler.iKRetargetStoppingDistance = enemyBlockingMod._defaultIKRetargetStoppingDistance;

            _currentAgentAccelSpeed = agentAccelSpeed;
            _currentAgentMoveSpeed = agentMoveSpeed;

            anim.SetBool(hashManager.e_mod_IsBlocking_hash, false);

            maxAttackIntervalRate = enemyBlockingMod._defaultAttackIntervalRate;
            minAttackIntervalRate = enemyBlockingMod._defaultAttackIntervalRandomizeAmount;

            isLockOnMoveAround = false;
            anim.SetBool(e_IsLockOnMoveAround_hash, false);         /// If Enemy Don't want to move aronud, just delete this line of code.
        }
        
        public override bool GetIsWithinBlockingAngle()
        {
            float _tempAngle = angleToTarget;

            if (Vector3.Dot(mTransform.right, dirToTarget) < 0)
                _tempAngle *= -1;

            if (_tempAngle < 10 && _tempAngle > -50)
                return true;

            return false;
        }

        #region Anim Event.
        /// On Hit Blocking Break.
        public override void OnHitBlockingBreak()
        {
            enemyBlockingMod.OnHitBlockingBreak();
        }
        
        public override void ResumeEnemyBlockingAfterAttack()
        {
            enemyBlockingMod.SetIsInAttackBlockingToFalse();
        }
        #endregion

        #endregion

        #region Perilous Attack Mod.
        public override bool GetUsedPerilousAttackBool()
        {
            return perilousAttackMod.usedPerilousAttack;
        }

        public override void SetUsedPerilousAttackToTrue()
        {
            perilousAttackMod.SetUsedPerilousAttackToTrue();
        }
        #endregion

        #region Player Spam Blocking Mod.
        public override void ResetSpammedBlockingStatus()
        {
            playerSpamBlockingMod.ResetSpammedBlockingStatus();
        }
        public override bool GetHasSpammedBlockingBool()
        {
            return playerSpamBlockingMod.GetHasSpammedBlockingBool();
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

        #region On Enemy Death Turn Off Damage Collider.
        protected override void OnDeathTurnOffDamageCollider()
        {
            base.OnDeathTurnOffDamageCollider();
            currentWeapon.SetSidearmColliderStatusToFalse();
            r_leg_damageColliderMod.R_Leg_DamageColliderOnDeathReset();
        }
        #endregion
        
        public override void OnExitAggroFacedPlayerReset()
        {
            Base_OnExitAggroFacedPlayerResets();

            enemyBlockingMod.EnemyBlockingExitAggroReset();

            perilousAttackMod.PerilousAttackExitAggroReset();

            playerSpamBlockingMod.PlayerSpamBlockingExitAggroReset();

            r_leg_damageColliderMod.R_Leg_DamageColliderOnDeathReset();
        }

        #region Indicators.
        public override void Play_RH_ParryIndicator()
        {
            if (canPlayIndicator)
                rh_ParryIndicator.SetActive(true);
        }

        public override void Play_PerliousAttackIndicator()
        {
            if (canPlayIndicator)
                perilousAttackMod.perilousATKIndicator.SetActive(true);
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