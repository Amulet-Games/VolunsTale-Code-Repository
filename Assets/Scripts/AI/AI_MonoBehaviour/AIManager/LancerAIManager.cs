using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class LancerAIManager : AIManager
    {
        #region Indicators.
        public GameObject rh_ParryIndicator;
        #endregion

        [Header("IEnemyPerilousAttack")]
        public PerilousAttackMod perilousAttackMod;

        [Header("IEnemyRollInterval")]
        public RollIntervalMod rollIntervalMod;

        [Header("IEnemyEvolve")]
        public EnemyEvolveMod enemyEvolveMod;

        [Header("IDamageParticleAttack")]
        public DamageParticleAttackMod damageParticleAttackMod;

        [Header("IL_Leg_DamageCollider")]
        public L_Leg_DamageColliderMod l_leg_damageColliderMod;

        #region Init.
        public override void AIModsOnEnterAggroFacedPlayer()
        {
            perilousAttackMod.PerilousAttackGoesAggroReset();
            rollIntervalMod.RollIntervalGoesAggroReset();
            damageParticleAttackMod.DpAttackGoesAggroReset();
        }

        public override void SetupAIMods()
        {
            enemyEvolveMod.EnemyEvolveModInit(this);
            damageParticleAttackMod.DamagedParticleAttackModInit(this);
            l_leg_damageColliderMod.L_Leg_DamageColliderModInit(this);
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
            UpdateAggroStateResets();

            ResetUpdateLockOnPosBool();
            
            AttackIntervalTimeCount();
            
            perilousAttackMod.PerilousAttackTimeCount();

            rollIntervalMod.RollIntervalTimeCount();
            
            damageParticleAttackMod.DamageParticleAttackTimeCount();

            GetNextAction();

            MonitorLockOnLocomotionType();

            UpdateAggroStateIK();

            SetEnemyAnim();
        }

        protected override void UpdateModsDeltas()
        {
            perilousAttackMod._delta = _delta;
            rollIntervalMod._delta = _delta;
            enemyEvolveMod._delta = _delta;
            damageParticleAttackMod._delta = _delta;
        }
        #endregion

        #region Weapons.
        /// Set Current Weapon.
        public override void SetCurrentFirstWeaponBeforeAggro()
        {
            currentWeapon = firstWeapon;
            enemyEvolveMod.HandleActionHolderChangeIfEvolve();
            
            anim.SetBool(e_IsArmed_hash, true);
        }

        public override void SetCurrentFirstWeaponAfterAggro()
        {
            currentWeapon = firstWeapon;
            enemyEvolveMod.HandleActionHolderChangeIfEvolve();
            
            PlayAnimationCrossFade(e_unsheath_First_hash, 0.2f, true);
            anim.SetBool(e_IsArmed_hash, true);
        }
        #endregion

        #region Turn With Agent.
        protected override void AggroTurnWithRootAnimation()
        {
            base.AggroTurnWithRootAnimation();
            aIStates.Set_AnimMoveRmType_ToRoll();   // Lancer Turn Animation is a roll.
            useInplaceTurningSpeed = false;
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

        #region isInteracting Tick.
        public override void IsInteracting_Tick()
        {
            base.IsInteracting_Tick();

            perilousAttackMod.PerilousAttackTimeCount();

            rollIntervalMod.RollIntervalTimeCount();

            enemyEvolveMod.UpdateEvolveChargeTimer();

            damageParticleAttackMod.DamageParticleAttackTimeCount();
        }
        #endregion

        #region On Hit.
        protected override void OnHitAIMods()
        {
            enemyEvolveMod.CheckIsEvolvable();
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
                if (!enemyEvolveMod.isEvolveStarted)
                {
                    Play_Defualt_ArmedKnockDownAnim();
                }
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

                        #region Play Armed Small Hit Aniamtion.
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
                Default_PlayArmedDeathAnim();
            }
            else
            {
                Default_PlayUnarmedDeathAnim();
            }
        }
        #endregion

        #region On Execution Hit.
        protected override void OnExecutionAIMods()
        {
            enemyEvolveMod.CheckIsEvolvable();
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

        #region Enemy Evolve Mod.
        public override bool GetIsEnemyEvolvable()
        {
            return enemyEvolveMod.isEvolvable;
        }

        public override void SetIsEvolveStartedStatusToTrue()
        {
            enemyEvolveMod.SetIsEvolveStartedStatus(true);
        }
        #endregion

        #region Damage Particle Attack Mod.
        public override bool GetUsedDpAttack()
        {
            return damageParticleAttackMod.usedDpAttack;
        }

        public override void HandleDpAttack(DamageParticleAttackMod.DpAttackAnimStateEnum _targetAnimHash)
        {
            damageParticleAttackMod.HandleDpAttack(_targetAnimHash);
        }
        #endregion

        #region L Leg Damage Collider Mod.
        public override void Enable_L_Leg_DamageCollider()
        {
            l_leg_damageColliderMod.Enable_L_Leg_DamageCollider();
        }

        public override void Disable_L_Leg_DamageCollider()
        {
            l_leg_damageColliderMod.Disable_L_Leg_DamageCollider();
        }
        #endregion

        #region On Enemy Death Turn Off Damage Collider.
        protected override void OnDeathTurnOffDamageCollider()
        {
            l_leg_damageColliderMod.L_Leg_DamageColliderOnDeathReset();
        }
        #endregion

        #region On Parry Execute States Turn Off Damage Collider.
        protected override void OnParryExecuteStateResetColliderStatus()
        {
            if (currentWeapon)
                currentWeapon.SetColliderStatusToFalse();
            else
                l_leg_damageColliderMod.Disable_L_Leg_DamageCollider();
        }
        #endregion

        #region Off Exit Aggro Faced Player.
        public override void OnExitAggroFacedPlayerReset()
        {
            Base_OnExitAggroFacedPlayerResets();

            perilousAttackMod.PerilousAttackExitAggroReset();

            rollIntervalMod.RollIntervalExitAggroReset();

            enemyEvolveMod.EnemyEvolveExitAggroReset();

            damageParticleAttackMod.DpAttackExitAggroReset();

            l_leg_damageColliderMod.L_Leg_DamageColliderOnDeathReset();
        }
        #endregion

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

        #region On Checkpoint Refresh Mods (General).
        protected override void OnCheckpointRefresh_Mods()
        {
            enemyEvolveMod.DevolveEnemy();
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