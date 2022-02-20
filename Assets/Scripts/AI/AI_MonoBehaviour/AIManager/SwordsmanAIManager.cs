using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class SwordsmanAIManager : AIManager
    {
        #region Indicators.
        public GameObject rh_ParryIndicator;
        #endregion

        [Header("IEnemyRollInterval")]
        public RollIntervalMod rollIntervalMod;

        [Header("IEnemyTwoStanceCombat")]
        public TwoStanceCombatMod twoStanceCombatMod;

        [Header("IEnemyTaunt")]
        public EnemyTauntMod enemyTauntMod;

        [Header("IEnemyParry")]
        public ParryPlayerMod parryPlayerMod;

        [Header("IEnemyPerilousAttack")]
        public PerilousAttackMod perilousAttackMod;

        [Header("ILimitEnemyTurning")]
        public LimitEnemyTurningMod limitEnemyTurning;

        #region Init.
        public override void AIModsOnEnterAggroFacedPlayer()
        {
            rollIntervalMod.RollIntervalGoesAggroReset();
            twoStanceCombatMod.TwoStanceCombatModEnterAggroReset();
            enemyTauntMod.EnemyTauntGoesAggroReset();
            parryPlayerMod.ParryPlayerGoesAggroReset();
            perilousAttackMod.PerilousAttackGoesAggroReset();
            limitEnemyTurning.LimitEnemyTurningModGoesAggroReset();
        }

        public override void SetupAIMods()
        {
            twoStanceCombatMod.TwoStanceCombatModInit(this);
            parryPlayerMod.ParryModInit(this);
        }
        #endregion

        #region AI Ticks.
        public override void Tick()
        {
            UpdateAggroStateResets();

            ResetUpdateLockOnPosBool();
            
            AttackIntervalTimeCount();

            limitEnemyTurning.TrackIdleAnimTurning();
            
            rollIntervalMod.RollIntervalTimeCount();

            twoStanceCombatMod.MonitorChangeStanceTimer();

            enemyTauntMod.EnemyTauntTimeCount();

            parryPlayerMod.MonitorParryPlayerBools();

            perilousAttackMod.PerilousAttackTimeCount();
            
            GetNextAction();

            MonitorLockOnLocomotionType();

            UpdateAggroStateIK();

            SetEnemyAnim();
        }

        protected override void UpdateModsDeltas()
        {
            rollIntervalMod._delta = _delta;
            twoStanceCombatMod._delta = _delta;
            enemyTauntMod._delta = _delta;
            parryPlayerMod._delta = _delta;
            perilousAttackMod._delta = _delta;
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

            limitEnemyTurning.TrackIdleAnimTurning();
            
            rollIntervalMod.RollIntervalTimeCount();

            twoStanceCombatMod.MonitorChangeStanceTimer();

            enemyTauntMod.EnemyTauntTimeCount();

            parryPlayerMod.MonitorParryPlayerBools();

            perilousAttackMod.PerilousAttackTimeCount();
        }
        #endregion

        #region On Hit.
        protected override void OnHitAIMods()
        {
            parryPlayerMod.OnHitAIMods();
        }

        protected override void SpawnOnHitEffect()
        {
            parryPlayerMod.SpawnOnHitEffect();
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
                twoStanceCombatMod.TwoStanceMod_PlayArmedKnockDownAnim();
            }
            else
            {
                if (!_isSkippingOnHitAnim && !parryPlayerMod.isParryWaiting)
                {
                    twoStanceCombatMod.TwoStanceMod_OnHitAnim();
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
            if (currentWeapon)
            {
                twoStanceCombatMod.PlayTwoStanceOnDeathAniation();
            }
            else
            {
                Default_PlayUnarmedDeathAnim();
            }
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

        protected override void AggroTurnWithRootAnimation()
        {
            if (IsTargetOnLeftSide())
            {
                if (isWeaponOnHand)
                {
                    twoStanceCombatMod.RotateWithRootAnimation_TurnLeft();
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
                    twoStanceCombatMod.RotateWithRootAnimation_TurnRight();
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
                if (!isWeaponOnHand)
                {
                    PlayAnimationCrossFade(hashManager.e_unarmed_turn_left_inplace_hash, 0.2f, true);
                }
                else
                {
                    twoStanceCombatMod.RotateWithInplaceAnimation_TurnLeft();
                }
            }
            else
            {
                if (!isWeaponOnHand)
                {
                    PlayAnimationCrossFade(hashManager.e_unarmed_turn_right_inplace_hash, 0.2f, true);
                }
                else
                {
                    twoStanceCombatMod.RotateWithInplaceAnimation_TurnRight();
                }
            }
        }

        public override void TurningWhileIdleWithIK()
        {
            if (isPausingTurnWithAgent || isMovingToward)
                return;

            if (parryPlayerMod.isParryWaiting)
            {
                iKHandler.TurnOnBodyRigIK();
                ParryingLerpToFaceTarget();
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

            void ParryingLerpToFaceTarget()
            {
                mTransform.rotation = Quaternion.Lerp(mTransform.rotation, Quaternion.LookRotation(dirToTarget), _delta * parryPlayerMod.maxUpperBodyIKParryTurnSpeed);
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

        #region Two Stance Combat Mod.
        public override bool GetIsRightStanceBool()
        {
            return twoStanceCombatMod.isRightStance;
        }

        public override bool GetCheckCombatStanceBool()
        {
            return true;
        }

        public override void SetIsRightStanceBool(bool isRightStance)
        {
            twoStanceCombatMod.SetIsRightStanceBool(isRightStance);
        }
        #endregion

        #region Enemy Taunt Mod.
        public override bool GetTauntedPlayerBool()
        {
            return enemyTauntMod.tauntedPlayer;
        }

        public override void SetTauntedPlayerToTrue()
        {
            enemyTauntMod.SetTauntedPlayerToTrue();
        }
        #endregion

        #region Parry Player Mod.
        public override bool GetIsWaitingToParryBool()
        {
            return parryPlayerMod.isParryWaiting;
        }

        public override bool GetTriedParryPlayerBool()
        {
            return parryPlayerMod.isInParryCooldown;
        }
        
        public override void SetIsWaitingToParryBool(bool isWaitingToParry)
        {
            parryPlayerMod.SetIsWaitingToParryBool(isWaitingToParry);
        }

        public override void OnWaitingToParry()
        {
            skippingScoreCalculation = true;

            if (twoStanceCombatMod.isRightStance)
            {
                PlayAnimationCrossFade(hashManager.e_RS_parry_waiting_start_hash, 0.2f, false);
            }
            else
            {
                PlayAnimationCrossFade(hashManager.e_LS_parry_waiting_start_hash, 0.2f, false);
            }

            anim.SetBool(parryPlayerMod.e_mod_isWaitingToParry_hash, true);
        }

        public override void OffWaitingToParry_Parryable()
        {
            skippingScoreCalculation = false;
            anim.SetBool(parryPlayerMod.e_mod_isWaitingToParry_hash, false);
        }

        public override void OffWaitingToParry_TimesOut()
        {
            if (!isDead)
            {
                skippingScoreCalculation = false;
                anim.SetBool(parryPlayerMod.e_mod_isWaitingToParry_hash, false);

                LeanTween.value(iKHandler.bodyRigWeight, 0, 0.15f).setOnUpdate((value) => iKHandler.bodyRigWeight = value);
                LeanTween.value(iKHandler.headRigWeight, 0, 0.15f).setOnUpdate((value) => iKHandler.headRigWeight = value);
            }
        }

        public override void OffWaitingToParry_HitByChargeAttack()
        {
            if (!isDead)
            {
                OffWaitingToParryWait();
            }

            void OffWaitingToParryWait()
            {
                LeanTween.value(0, 1, 1f).setOnComplete(OnWaitComplete);

                void OnWaitComplete()
                {
                    skippingScoreCalculation = false;
                    anim.SetBool(parryPlayerMod.e_mod_isWaitingToParry_hash, false);
                }
            }
        }

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
        
        public override void OnExitAggroFacedPlayerReset()
        {
            Base_OnExitAggroFacedPlayerResets();
            
            rollIntervalMod.RollIntervalExitAggroReset();

            twoStanceCombatMod.TwoStanceCombatExitAggroReset();

            enemyTauntMod.EnemyTauntExitAggroReset();

            parryPlayerMod.ParryPlayerExitAggroReset();

            perilousAttackMod.PerilousAttackExitAggroReset();
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
