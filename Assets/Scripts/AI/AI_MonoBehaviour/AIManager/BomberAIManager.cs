using System;
using UnityEngine;

namespace SA
{
    public class BomberAIManager : AIManager
    {
        #region Indicators.
        public GameObject rh_ParryIndicator;
        #endregion

        [Header("IEnemyRollInterval")]
        public RollIntervalMod rollIntervalMod;

        [Header("IEnemyMoveCertainDirection")]
        public FixDirectionMoveMod fixDirectionMoveMod;

        [Header("IEnemyDualActionHolder")]
        public DualWeaponMod dualWeaponMod;

        [Header("IEnemyObservePlayerStatus")]
        public PlayerSpamAttackMod playerSpamAttackMod;

        #region Fix Direction Move Mod Private.
        RaycastHit[] hitResults = new RaycastHit[1];
        [NonSerialized] LayerMask fixDirectionMoveObstacleMask;
        #endregion

        #region Setup.
        public override void SetupAIWeapon()
        {
            ResourcesManager rm = GameManager.singleton._resourcesManager;

            EnemyWeapon firstEnemyWeapon = rm.GetEnemyWeapon(profile.firstWeaponId);
            firstEnemyWeapon.SetupThrowableRuntimeWeapon(this);

            /// Dual Weapon Mod.
            dualWeaponMod.isUsingSecondWeapon = true;
            EnemyWeapon secondEnemyWeapon = rm.GetEnemyWeapon(dualWeaponMod.GetSecondWeaponID());
            dualWeaponMod.secondWeapon = secondEnemyWeapon.SetupRuntimeWeapon(this);
            dualWeaponMod.isUsingSecondWeapon = false;
        }

        public override void SetupAIMods()
        {
            fixDirectionMoveMod.FixDirectionMoveModInit(this);
            dualWeaponMod.DualWeaponModInit(this);
            playerSpamAttackMod.PlayerSpamAttackModInit(playerStates);

            SetupAIModsPrivates();

            void SetupAIModsPrivates()
            {
                fixDirectionMoveObstacleMask = aIStates._layerManager._fixDirectionMoveObstaclesMask;
            }
        }

        public override void AIModsOnEnterAggroFacedPlayer()
        {
            rollIntervalMod.RollIntervalGoesAggroReset();
            fixDirectionMoveMod.FixDirectionMoveGoesAggroReset();
        }
        #endregion

        #region AI Ticks.
        public override void Tick()
        {
            UpdateAggroStateResets();

            ResetUpdateLockOnPosBool();
            
            if (!fixDirectionMoveMod.isMovingInFixDirection)
                dualWeaponMod.CheckIsSwitchWeaponNeeded();

            CheckReacquireThrowableWeapon();

            AttackIntervalTimeCount();

            fixDirectionMoveMod.FixDirectionMoveTimeCount();

            rollIntervalMod.RollIntervalTimeCount();

            playerSpamAttackMod.UpdateSpamAttackCounter();

            GetNextAction();

            MonitorLockOnLocomotionType();

            UpdateAggroStateIK();

            CheckIsForwardMovable();
            
            SetEnemyAnim();
        }

        protected override void UpdateModsDeltas()
        {
            rollIntervalMod._delta = _delta;
            fixDirectionMoveMod._delta = _delta;
            playerSpamAttackMod._delta = _delta;
        }

        public override void CheckReacquireThrowableWeapon()
        {
            if (isReacquireThrowableNeeded)
            {
                dualWeaponMod.CheckReacquireThrowableWeapon();
                isReacquireThrowableNeeded = false;
            }
        }
        
        public override void SetEnemyAnim()
        {
            base.SetEnemyAnim();
            dualWeaponMod.SetEnemyAnim();
        }
        #endregion

        #region isInteracting Ticks.
        public override void IsInteracting_Tick()
        {
            base.IsInteracting_Tick();
            
            rollIntervalMod.RollIntervalTimeCount();

            fixDirectionMoveMod.FixDirectionMoveTimeCount();

            playerSpamAttackMod.UpdateSpamAttackCounter();
        }
        #endregion
        
        #region On Hit.
        protected override void OnHitAIMods()
        {
            if (_isHitByChargeAttack)
                fixDirectionMoveMod.OnChargedAttackHit();
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
                HandleArmedKnockDownDirection();
            }
            else
            {
                if (!_isSkippingOnHitAnim)
                {
                    dualWeaponMod.DualWeapon_PlayRegularOnHitAnim();
                }
            }

            void HandleArmedKnockDownDirection()
            {
                if (fixDirectionMoveMod.isHitByChargeAttackWhenMoving)
                {
                    if (Vector3.Dot(mTransform.forward, Vector3.Normalize(dirToTarget)) > 0.75f)
                    {
                        dualWeaponMod.DualWeapon_PlayArmedKnockDown_HitFromFront_Anim();
                    }
                    else
                    {
                        dualWeaponMod.DualWeapon_PlayArmedKnockDown_HitFromBack_Anim();
                    }

                    fixDirectionMoveMod.isHitByChargeAttackWhenMoving = false;
                }
                else
                {
                    dualWeaponMod.DualWeapon_PlayArmedKnockDown_HitFromFront_Anim();
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
                dualWeaponMod.DualWeapon_PlayOnDeathAnim();
            }
            else
            {
                NoSpecificLayer_PlayDeathAnim(e_unarmed_death_hash);
            }
        }
        #endregion

        #region Turn With Agent.
        protected override void AggroTurnWithRootAnimation()
        {
            if (IsTargetOnLeftSide())
            {
                if (isWeaponOnHand)
                {
                    dualWeaponMod.RotateWithRootAnimation_TurnLeft();
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
                    dualWeaponMod.RotateWithRootAnimation_TurnRight();
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
                    dualWeaponMod.RotateWithInplaceAnimation_TurnLeft();
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
                    dualWeaponMod.RotateWithInplaceAnimation_TurnRight();
                }
                else
                {
                    PlayAnimationCrossFade(hashManager.e_unarmed_turn_right_inplace_hash, 0.2f, true);
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
                    UpdateLocomotionSingleDis();
                }
                else if (isLockOnMoveAround)
                {
                    Bomber_UpdateLockOnLocomotion();
                }
                else if (fixDirectionMoveMod.isMovingInFixDirection)
                {
                    UpdateFixDirectionLocomotion();
                }
            }
        }

        protected void Bomber_UpdateLockOnLocomotion()
        {
            float agentVel = Mathf.Clamp01(_currentAgentVelocity);
            switch (currentLockOnLocomotionType)
            {
                case LockOnLocomotionTypeEnum.forward:
                    anim.SetFloat(horizontal_hash, agentVel * 0, 0.2f, _delta);
                    anim.SetFloat(vertical_hash, agentVel * 0.5f, 0.2f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.forward_left:
                    anim.SetFloat(horizontal_hash, agentVel * -0.5f, 0.2f, _delta);
                    anim.SetFloat(vertical_hash, agentVel * 0.5f, 0.2f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.forward_right:
                    anim.SetFloat(horizontal_hash, agentVel * 0.5f, 0.2f, _delta);
                    anim.SetFloat(vertical_hash, agentVel * 0.5f, 0.2f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.left:
                    anim.SetFloat(horizontal_hash, agentVel * -0.5f, 0.2f, _delta);
                    anim.SetFloat(vertical_hash, 0, 0.2f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.right:
                    anim.SetFloat(horizontal_hash, agentVel * 0.5f, 0.2f, _delta);
                    anim.SetFloat(vertical_hash, 0, 0.2f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.backward_left:
                    anim.SetFloat(horizontal_hash, agentVel * -0.5f, 0.2f, _delta);
                    anim.SetFloat(vertical_hash, agentVel * -0.5f, 0.2f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.backward_right:
                    anim.SetFloat(horizontal_hash, agentVel * 0.5f, 0.2f, _delta);
                    anim.SetFloat(vertical_hash, agentVel * -0.5f, 0.2f, _delta);
                    break;
                case LockOnLocomotionTypeEnum.backward:
                    anim.SetFloat(horizontal_hash, 0, 0.2f, _delta);
                    anim.SetFloat(vertical_hash, agentVel * -0.5f, 0.2f, _delta);
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
                UpdateMoveTowardAgent();
            }
            else if (isLockOnMoveAround)
            {
                UpdateAgentSpeedLockonMove();
                UpdateLockOnAgent();
            }
            else if (fixDirectionMoveMod.isMovingInFixDirection)
            {
                UpdateFixDirectionAgent();
            }

            MonitorIsMovementChanged();
        }

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

        #region Fix Direction.
        protected void UpdateFixDirectionAgent()
        {
            SetFixDirectionMoveDestination();
            UpdateFixDirectionMovePosition();

            isMovementChanged = true;
        }

        protected void SetFixDirectionMoveDestination()
        {
            if (fixDirectionMoveMod.applyFixDirMoveRootMotion)
            {
                agent.SetDestination(mTransform.position + mTransform.forward);
            }
        }

        protected void UpdateFixDirectionMovePosition()
        {
            mTransform.position = agent.nextPosition;
        }

        void ChangeAgentStats_FixDirectionMove()
        {
            agent.isStopped = false;
            agent.stoppingDistance = 0;
            agent.acceleration = _currentAgentAccelSpeed * 1.35f;
            agent.speed = _currentAgentMoveSpeed * 1.35f;
        }
        #endregion

        #endregion

        #region Root Motions.
        /// Tick.
        public override void ApplyTurnRootMotion()
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
                    Vector3 tarDir = vector3Zero;

                    if (isTrackingPlayer)
                    {
                        float targetPredictOffset = playerStates._isRunning ? currentPlayerPredictOffset + runningPredictAddonValue : currentPlayerPredictOffset;
                        tarDir = StaticHelper.GetNewRotatedVector3(dirToTarget, -targetPredictOffset * playerStates.horizontal);
                        //Debug.DrawRay(_mTransform.position, tarDir, Color.black);
                    }
                    else if (fixDirectionMoveMod.isMovingInFixDirection)
                    {
                        tarDir = fixDirectionMoveMod.calculatedFixDirection;
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
        public override void AI_HandleRootMotions_FixedUpdate()
        {
            if (applyAttackArtifMotion)
            {
                aIStates.ApplyAttackArtifiMotion();
            }
            else
            {
                aIStates.ApplyFallingRootMotions();
            }
        }
        #endregion

        #region Weapons / Dual Weapon Mod.
        /// General.
        public override void PlaySheathAnimation()
        {
            if (dualWeaponMod.isUsingSecondWeapon)
            {
                PlayAnimation(dualWeaponMod.e_sheath_Second_hash, true);
            }
            else
            {
                PlayAnimation(e_sheath_First_hash, true);
            }
        }

        public override void SetupWeaponToPosition(Transform _runtimeWeaponTransform)
        {
            if (dualWeaponMod.isUsingSecondWeapon)
            {
                AISheathTransform secondSheathTransform = dualWeaponMod.secondSheathTransform;

                _runtimeWeaponTransform.parent = anim.GetBoneTransform(HumanBodyBones.Hips);
                _runtimeWeaponTransform.localPosition = secondSheathTransform.pos;
                _runtimeWeaponTransform.localEulerAngles = secondSheathTransform.eulers;
                _runtimeWeaponTransform.localScale = secondSheathTransform.scale;
            }
            else
            {
                _runtimeWeaponTransform.parent = anim.GetBoneTransform(HumanBodyBones.Spine);
                _runtimeWeaponTransform.localPosition = firstSheathTransform.pos;
                _runtimeWeaponTransform.localEulerAngles = firstSheathTransform.eulers;
                _runtimeWeaponTransform.localScale = firstSheathTransform.scale;
            }
        }

        public override void SheathCurrentWeaponToPosition()
        {
            Transform _currentWeaponTransform = currentWeapon.transform;

            if (dualWeaponMod.isUsingSecondWeapon)
            {
                AISheathTransform secondSheathTransform = dualWeaponMod.secondSheathTransform;

                _currentWeaponTransform.parent = anim.GetBoneTransform(HumanBodyBones.Hips);
                _currentWeaponTransform.localPosition = secondSheathTransform.pos;
                _currentWeaponTransform.localEulerAngles = secondSheathTransform.eulers;
                _currentWeaponTransform.localScale = secondSheathTransform.scale;

                dualWeaponMod.isUsingSecondWeapon = false;
            }
            else
            {
                _currentWeaponTransform.parent = anim.GetBoneTransform(HumanBodyBones.Spine);
                _currentWeaponTransform.localPosition = firstSheathTransform.pos;
                _currentWeaponTransform.localEulerAngles = firstSheathTransform.eulers;
                _currentWeaponTransform.localScale = firstSheathTransform.scale;
            }

            currentWeapon = null;
        }
        
        /// Throwable Weapon.
        public override void ClearThrowableWeaponRefs()
        {
            currentWeapon = null;
            currentThrowableWeapon = null;

            dualWeaponMod.ClearThrowableReference();
            isReacquireThrowableNeeded = true;
        }

        /// Second Weapon.
        public override void ParentSecondWeaponUnderHand()
        {
            dualWeaponMod.secondWeapon.ParentEnemyWeaponUnderHand();
        }

        public override void SetCurrentSecondWeaponBeforeAggro()
        {
            currentWeapon = dualWeaponMod.secondWeapon;
            currentActionHolder = dualWeaponMod.secondWeaponActionHolder;

            anim.SetBool(e_IsArmed_hash, true);
        }

        public override void SetCurrentSecondWeaponAfterAggro()
        {
            currentWeapon = dualWeaponMod.secondWeapon;
            currentActionHolder = dualWeaponMod.secondWeaponActionHolder;
            
            PlayAnimationCrossFade(dualWeaponMod.e_unsheath_Second_hash, 0.2f, true);
        }

        public override bool GetIsUsingSecondWeaponBool()
        {
            return dualWeaponMod.isUsingSecondWeapon;
        }

        /// Switch Weapon Mod.
        public override void SwitchTo_FW_SetStatus()
        {
            _isSkippingOnHitAnim = true;

            fixDirectionMoveMod.SwitchTo_FW_SetStatus();
        }

        public override void SwitchTo_SW_SetStatus()
        {
            _isSkippingOnHitAnim = true;

            fixDirectionMoveMod.SwitchTo_SW_SetStatus();
        }

        /// Set Current Weapon.
        public override void SetCurrentFirstWeaponBeforeAggro()
        {
            currentActionHolder = firstWeaponActionHolder;
            anim.SetBool(e_IsArmed_hash, true);
        }
        
        public override void SetCurrentFirstWeaponAfterAggro()
        {
            currentWeapon = firstWeapon;
            currentActionHolder = firstWeaponActionHolder;
            currentWeapon.SetAICurrentThrowable();

            PlayAnimationCrossFade(e_unsheath_First_hash, 0.2f, true);
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

        #region Move In Fix Direction Mod.
        void CheckIsForwardMovable()
        {
            if (fixDirectionMoveMod.applyFixDirMoveRootMotion)
            {
                if (Physics.SphereCastNonAlloc(mTransform.position + vector3Up, 0.4f, mTransform.forward, hitResults, 0.2f, fixDirectionMoveObstacleMask) > 1)
                {
                    fixDirectionMoveMod.OffFixDirectionMove();
                }
            }
        }

        public override void OnFixDirectionMove()
        {
            skippingScoreCalculation = true;

            aIStates.ClearAgentPath();
            ChangeAgentStats_FixDirectionMove();
        }

        public override void OffFixDirectionMove()
        {
            skippingScoreCalculation = false;

            currentAction = null;
            aIStates.ClearAgentPath();
            agent.isStopped = true;
        }

        public override bool GetIsFixDirectionInCooldownBool()
        {
            return fixDirectionMoveMod.isFixDirectionInCooldown;
        }

        public override bool GetIsMovingInFixDirectionBool()
        {
            return fixDirectionMoveMod.isMovingInFixDirection;
        }

        public override void SetIsMovingFixDirectionToTrue()
        {
            fixDirectionMoveMod.SetIsMovingFixDirectionToTrue();
        }

        public override void SetApplyFixDirMoveRootMotionToTrue()
        {
            fixDirectionMoveMod.applyFixDirMoveRootMotion = true;
        }
        #endregion

        #region Player Spam Attack Mod.
        public override void ResetSpammedAttackingStatus()
        {
            playerSpamAttackMod.ResetSpammedAttackingStatus();
        }
        
        public override bool GetHasSpammedAttackingBool()
        {
            return playerSpamAttackMod.GetHasSpammedAttackingBool();
        }
        #endregion

        #region On Enter Aggro States / Transition.
        protected override void OnAggroEnemyUnSheathWeapon()
        {
            if (!isWeaponUnSheathAnimExecuted)
            {
                if (distanceToTarget <= dualWeaponMod.switchDistance - dualWeaponMod.switchDistanceBuffer)
                {
                    dualWeaponMod.isUsingSecondWeapon = true;

                    SetCurrentSecondWeaponBeforeAggro();
                    PlayAnimation_NoNeglect(dualWeaponMod.e_unsheath_Second_hash, true);
                }
                else
                {
                    SetCurrentFirstWeaponBeforeAggro();
                    PlayAnimation_NoNeglect(e_unsheath_First_hash, true);
                }

                isWeaponUnSheathAnimExecuted = true;
            }
        }
        #endregion

        #region Off Exit Aggro Faced Player States / Transition.
        public override void OnExitAggroFacedPlayerReset()
        {
            Base_OnExitAggroFacedPlayerResets();

            rollIntervalMod.RollIntervalExitAggroReset();

            fixDirectionMoveMod.FixDirectionMoveExitAggroReset();

            dualWeaponMod.DualWeaponExitAggroReset();

            playerSpamAttackMod.PlayerSpamAttackExitAggroReset();
        }

        protected override void OffExitAggroFacePlayerSheathWeapon()
        {
            if (!isWeaponSheathAnimExecuted)
            {
                if (currentWeapon)
                {
                    PlaySheathAnimation();
                }
                else if (isReacquireThrowableNeeded)
                {
                    ReacquireFirstThrowable();
                    isReacquireThrowableNeeded = false;
                    anim.SetBool(e_IsFacedPlayer_hash, true);
                }
                else
                {
                    anim.SetBool(e_IsFacedPlayer_hash, true);
                }
                
                anim.SetBool(e_IsArmed_hash, false);
                isWeaponSheathAnimExecuted = true;
            }
        }
        #endregion

        #region AI Passive Actions.
        public override void ReacquireSecondThrowableWithAnim()
        {
            ReacquireSecondThrowable();
            PlayAnimationCrossFade(dualWeaponMod.e_unsheath_Second_hash, 0.2f, true);
        }

        public override void ReacquireSecondThrowable()
        {
            ThrowableEnemyRuntimeWeapon newThrowable = dualWeaponMod.secondThrowableWeaponPool.Get();

            dualWeaponMod.secondWeapon = newThrowable;
            currentWeapon = newThrowable;
            currentThrowableWeapon = newThrowable;

            newThrowable._ai = this;

            if (newThrowable.isThrowableInited)
            {
                newThrowable.ReSetupThrowableRuntimeWeapon();
            }
            else
            {
                newThrowable.SetupThrowableRuntimeWeapon(dualWeaponMod.secondThrowableWeaponPool);
            }
        }
        #endregion

        #region Indicators.
        public override void Play_RH_ParryIndicator()
        {
            if (canPlayIndicator)
                rh_ParryIndicator.SetActive(true);
        }
        #endregion

        #region On Checkpoint Refresh Mods (General).
        protected override void OnCheckpointRefresh_Mods()
        {
            fixDirectionMoveMod.applyFixDirMoveRootMotion = false;
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


//public override void ParentSecondSideArmUnderHand()
//{
//    Transform _secondWeaponSideArmTrans = dualWeaponMod.secondWeapon.GetReferSidearm().transform;

//    _secondWeaponSideArmTrans.parent = anim.GetBoneTransform(HumanBodyBones.LeftHand);
//    _secondWeaponSideArmTrans.localPosition = vector3Zero;
//    _secondWeaponSideArmTrans.localEulerAngles = vector3Zero;
//    _secondWeaponSideArmTrans.localScale = vector3One;
//}

