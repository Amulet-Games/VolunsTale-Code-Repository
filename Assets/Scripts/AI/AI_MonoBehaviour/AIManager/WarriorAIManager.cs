using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class WarriorAIManager : AIManager
    {
        #region Indicators.
        public GameObject rh_ParryIndicator;
        #endregion

        [Header("IEnemyPerilousAttack")]
        public PerilousAttackMod perilousAttackMod;

        [Header("IEnemyTaunt")]
        public EnemyTauntMod enemyTauntMod;

        [Header("IEnemyThrowReturnalProjectile")]
        public ThrowReturnalProjectileMod throwReturnalProjectileMod;

        #region Init.
        public override void AIModsOnEnterAggroFacedPlayer()
        {
            perilousAttackMod.PerilousAttackGoesAggroReset();
            enemyTauntMod.EnemyTauntGoesAggroReset();
            throwReturnalProjectileMod.ThrowReturnalProjectileGoesAggroReset();
        }

        public override void SetupAIMods()
        {
            throwReturnalProjectileMod.ThrowReturnalProjectileModInit(this);
        }
        #endregion

        #region AI Ticks.
        public override void Tick()
        {
            UpdateAggroStateResets();

            ResetUpdateLockOnPosBool();
            
            AttackIntervalTimeCount();

            enemyTauntMod.EnemyTauntTimeCount();

            perilousAttackMod.PerilousAttackTimeCount();

            throwReturnalProjectileMod.ThrowReturnalProjectileWaitTimeCount();

            GetNextAction();

            MonitorLockOnLocomotionType();

            UpdateAggroStateIK();

            SetEnemyAnim();
        }

        protected override void UpdateModsDeltas()
        {
            enemyTauntMod._delta = _delta;
            perilousAttackMod._delta = _delta;
            throwReturnalProjectileMod._delta = _delta;
        }
        #endregion

        #region isInteracting Tick.
        public override void IsInteracting_Tick()
        {
            base.IsInteracting_Tick();
            
            enemyTauntMod.EnemyTauntTimeCount();

            perilousAttackMod.PerilousAttackTimeCount();

            throwReturnalProjectileMod.MonitorThrownProjectile();
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

        #region On Hit.
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
                Default_PlayArmedDeathAnim();
            }
            else
            {
                Default_PlayUnarmedDeathAnim();
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

        #region Throw Return Projectile Mod.
        public override void SetHasThrownProjectileStatusToTrue()
        {
            throwReturnalProjectileMod.SetIsThrowningProjectileStatus(true);
        }

        public override bool GetHasThrownProjectileStatus()
        {
            return throwReturnalProjectileMod.isThrowingProjectile;
        }

        public override bool GetIsThrowProjectile()
        {
            return throwReturnalProjectileMod.hasThrownProjectile;
        }

        public override void ReturnProjectileWhenHitObstacles()
        {
            throwReturnalProjectileMod.ReturnProjectileWhenHitObstacles();
        }

        public override int GetThrowReturnalProjectileHash()
        {
            return throwReturnalProjectileMod.e_throw_ReturnalProjectile_start_hash;
        }

        public override void ParentReturnProjectileToHand()
        {
            if (currentWeapon != null)
            {
                currentWeapon.ParentEnemyWeaponUnderHand();
            }
        }
        #endregion
        
        public override void OnExitAggroFacedPlayerReset()
        {
            Base_OnExitAggroFacedPlayerResets();

            perilousAttackMod.PerilousAttackExitAggroReset();

            enemyTauntMod.EnemyTauntExitAggroReset();

            throwReturnalProjectileMod.ThrowReturnalProjectileExitAggroReset();
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

        //    // If player is within aggro range.
        //    if (distanceToTarget < aggro_Thershold)
        //    {
        //        // check if player is within the closet limit of aggro.
        //        if (distanceToTarget <= aggro_ClosestThershold)
        //        {
        //            if (CheckTargetIsBehindWall())
        //                return;

        //            OnEnterAggroFacedPlayerState();
        //        }
        //        // if player is not within closet limit, check if angle within aggro angle.
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