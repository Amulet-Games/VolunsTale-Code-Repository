using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class EnemyAnimatorHook : MonoBehaviour
    {
        [ReadOnlyInspector] public AIManager _ai;
        [ReadOnlyInspector] public AIStateManager _aiStates;
        [ReadOnlyInspector] public EnemyIKHandler _ikHandler;
        
        public void Init(AIManager ai)
        {
            _ai = ai;
            _aiStates = _ai.aIStates;
            _ikHandler = _aiStates.iKHandler;
        }

        #region Callbacks.
        private void OnAnimatorMove()
        {
            _aiStates.Apply_AnimMoveRootMotion_ByType();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            _ikHandler.Tick();
        }
        #endregion

        #region Animation Events.
        public void SetEnemyIsInteractingStatus(int value)
        {
            if (value == 1)
            {
                _ai.anim.SetBool(_aiStates.e_IsInteracting_hash, true);
            }
            else
            {
                _ai.anim.SetBool(_aiStates.e_IsInteracting_hash, false);
            }
        }

        public void SetIsSkippingOnHitAnimStatus(int value)
        {
            if (value == 1)
            {
                _ai._isSkippingOnHitAnim = true;
            }
            else
            {
                _ai._isSkippingOnHitAnim = false;
            }
        }

        #region Rigidbody / Collider.
        public void SwitchOffColliderAndRb()
        {
            _aiStates.e_rb.useGravity = false;
            _aiStates.e_root_Collider.isTrigger = true;
        }

        public void SwitchOnColliderAndRb()
        {
            _aiStates.e_rb.useGravity = true;
            _aiStates.e_root_Collider.isTrigger = false;
        }

        public void SwitchOnCollider()
        {
            _aiStates.e_root_Collider.isTrigger = false;
        }

        public void SwitchOffGravity()
        {
            _aiStates.e_rb.useGravity = false;
        }

        public void SwitchOnGravity()
        {
            _aiStates.e_rb.useGravity = true;
        }
        #endregion

        #region Combo.
        public void SetMultiStageAttackAvailable()
        {
            _ai.isMultiStageAttackAvailable = true;
        }
        #endregion

        #region Apply Root Motions.
        public void SetApplyAttackRootMotionStatus(int value)
        {
            if (value == 1)
            {
                _aiStates.Set_AnimMoveRmType_ToAttack();
            }
            else
            {
                _aiStates.Set_AnimMoveRmType_ToNull();
            }
        }

        public void SetApplyTurnRootMotion(int value)
        {
            if (value == 1)
            {
                _ai.applyTurnRootMotion = true;
            }
            else
            {
                _ai.applyTurnRootMotion = false;
            }
        }

        public void SetApplyTurnRootMotionSpecificPredict(int v)
        {
            _ai.applyTurnRootMotion = true;
            _ai.currentPlayerPredictOffset = v;
        }

        public void SetIsTrackingPlayerToFalse()
        {
            _ai.isTrackingPlayer = false;
        }

        public void SetApplyFallbackRootMotion(int value)
        {
            /// Used in Egil P2 Fallback
            if (value == 1)
            {
                _aiStates.Set_AnimMoveRmType_ToFallback();
            }
            else
            {
                _aiStates.Set_AnimMoveRmType_ToNull();
            }
        }
        #endregion

        #region Set Invincible.
        public void SetEnemyIsInvincibleStatus(int value)
        {
            if (value == 1)
            {
                _ai._isInvincible = true;
            }
            else
            {
                _ai._isInvincible = false;
            }
        }
        #endregion

        #region Set Parryable.
        public void Play_RH_ParryableIndicator()
        {
            _ai.Play_RH_ParryIndicator();
        }

        public void Play_LH_ParraybleIndicator()
        {
            _ai.Play_LH_ParryIndicator();
        }

        public void SetIsParryableStatus(int v)
        {
            /// You don't need to set PW's isParryable to true in here, it's already set Within "PowerWeaponDamageColliderStatus" (true) event.
            /// To st PW's isParryable to false, plz use this event.

            if (v == 1)
            {
                _ai._isParryable = true;
            }
            else
            {
                _ai._isParryable = false;
            }
        }
        #endregion

        #region Sheath Weapon/ Parent Weapon
        public void SheathCurrentEnemyWeapon()
        {
            _ai.SheathCurrentWeaponInAnim();
        }

        public void SheathCurrentEnemySidearm()
        {
            _ai.SheathCurrentSidearmToPosition();
        }

        public void ParentEnemyFirstWeaponUnderHand()
        {
            _ai.firstWeapon.ParentEnemyWeaponUnderHand();
        }

        public void ParentEnemyFirstSidearmUnderHander()
        {
            _ai.firstWeapon.ParentEnemySidearmUnderHand();
        }

        public void SetIsWeaponOnHandStatusToTrue()
        {
            /// For Marksman unsheath fist weapon.
            _ai.isWeaponOnHand = true;
        }
        #endregion

        #region Throwable Weapon.
        public void ThrowEnemyWeapon()
        {
            _ai.currentThrowableWeapon.ThrowWeapon();
        }

        public void ClearThrowableWeaponRefs()
        {
            _ai.ClearThrowableWeaponRefs();
        }
        #endregion

        #region Power Weapon.
        public void SetIsInGettingInterAnimToTrue()
        {
            _ai.SetIsInGettingInterAnimToTrue();
        }

        public void GetThrowablePowerWeapon()
        {
            _ai.GetThrowablePowerWeapon();
        }

        public void SetSwitchTargetToInteractableStatusToFalse()
        {
            _ai.SetSwitchTargetToInteractableToFalse();
        }

        public void ClearThrowablePowerWeaponRefs()
        {
            _ai.ClearPowerWeaponRefsAfterThrown();
        }

        public void BreakPowerWeapon()
        {
            _ai.BreakPowerWeapon();
        }

        public void BreakPowerWeaponByChargeAttack()
        {
            _ai.BreakPowerWeaponByChargeAttack();
        }

        public void SetPowerWeapon_MSA_Available()
        {
            _ai.SetPowerWeaponMSA_Available();
        }

        public void PowerWeaponDamageColliderStatus(int value)
        {
            /// Included PW isParryable (True) if you set Damage Collider Status to true.
            _ai.PowerWeaponDamageColliderStatus(value);
        }
        #endregion

        #region Throw Returnal Projectile.
        public void ThrowReturnalProjetile()
        {
            _ai.SetHasThrownProjectileStatusToTrue();
            _ai.currentWeapon.SetColliderStatusToTrue();
            _ai.applyTurnRootMotion = false;
        }
        #endregion

        #region Damage Colliders.
        public void WeaponDamageColliderStatus(int value)
        {
            if (value == 1)
            {
                if (!_ai._isInParryExecuteWindow)
                {
                    _ai.currentWeapon.SetColliderStatusToTrue();
                }
            }
            else
            {
                _ai.currentWeapon.SetColliderStatusToFalse();
            }
        }

        public void SidearmDamageColliderStatus(int value)
        {
            if (value == 1)
            {
                _ai.currentWeapon.SetSidearmColliderStatusToTrue();
            }
            else
            {
                _ai.currentWeapon.SetSidearmColliderStatusToFalse();
            }
        }

        #region Public Damage Collider.

        /// L_Leg_DamageColliderMod.
        public void L_Leg_DC_Mod_Status(int value)
        {
            if (value == 1)
            {
                _ai.Enable_L_Leg_DamageCollider();
            }
            else
            {
                _ai.Disable_L_Leg_DamageCollider();
            }
        }

        /// R_Leg_DamageColliderMod.
        public void R_Leg_DC_Mod_Status(int value)
        {
            if (value == 1)
            {
                _ai.Enable_R_Leg_DamageCollider();
            }
            else
            {
                _ai.Disable_R_Leg_DamageCollider();
            }
        }

        /// L_Shoulder_DamageColliderMod.
        public void L_Shoulder_DC_Mod_Status(int value)
        {
            if (value == 1)
            {
                _ai.Enable_L_Shoulder_DamageCollider();
            }
            else
            {
                _ai.Disable_L_Shoulder_DamageCollider();
            }
        }

        #region FullBody Damage Collider.
        public void FullBody_DC_Mod_R_Arm_Status(int value)
        {
            if (value == 1)
            {
                _ai.FullBody_DC_Mod_Enable_R_Arm_DC();
            }
            else
            {
                _ai.FullBody_DC_Mod_Disable_R_Arm_DC();
            }
        }

        public void FullBody_DC_Mod_L_Arm_Status(int value)
        {
            if (value == 1)
            {
                _ai.FullBody_DC_Mod_Enable_L_Arm_DC();
            }
            else
            {
                _ai.FullBody_DC_Mod_Disable_L_Arm_DC();
            }
        }

        public void FullBody_DC_Mod_R_Leg_Status(int value)
        {
            if (value == 1)
            {
                _ai.FullBody_DC_Mod_Enable_R_Leg_DC();
            }
            else
            {
                _ai.FullBody_DC_Mod_Disable_R_Leg_DC();
            }
        }

        public void FullBody_DC_Mod_L_Leg_Status(int value)
        {
            if (value == 1)
            {
                _ai.FullBody_DC_Mod_Enable_L_Leg_DC();
            }
            else
            {
                _ai.FullBody_DC_Mod_Disable_L_Leg_DC();
            }
        }
        #endregion

        #endregion

        #region Bomber Melee Attack Status (Attack Root Motion + Damage Collider.)
        public void SetWeaponDamageColliderWithAtttackRootMotion(int v)
        {
            if (v == 1)
            {
                if (!_ai._isInParryExecuteWindow)
                {
                    _ai.currentWeapon.SetColliderStatusToTrue();
                    _aiStates.Set_AnimMoveRmType_ToAttack();
                }
            }
            else
            {
                _ai.currentWeapon.SetColliderStatusToFalse();
                _aiStates.Set_AnimMoveRmType_ToNull();
            }
        }
        #endregion

        #endregion
        
        #region Damage Particle Logic.
        public void ShowDamageParticleAttackEffect()
        {
            _ai._currentDamageParticle.OnDamageParticleEffect();
        }
        #endregion
        
        #region Knocked Down.
        public void CheckEnemyDeathWhenKnockedDown()
        {
            if (_ai.isDead)
            {
                _aiStates.OnDeathSwitchLayer();
                _aiStates.OnEnemyDeath();
            }
            else
            {
                _aiStates.ReEnableAgent();
                _aiStates.Set_AnimMoveRmType_ToNull();
            }
        }
        #endregion

        #region On Hit.
        public void OnSmallHitEndRenableAgent()
        {
            _aiStates.ReEnableAgent();
        }
        #endregion

        #region Enemy Death.
        public void OnEnemyDeath()
        {
            _aiStates.OnEnemyDeath();
        }
        #endregion

        #region Boss Death.
        public void OnBossDeathShowDeathFx()
        {
            _aiStates._aiSessionManager.OnBossDeathShowEgilDeathFx();
        }

        public void OnBossDeathHideAura()
        {
            _aiStates._aiSessionManager.HideEgil3rdPhaseAura();
        }

        public void OnBossDeath()
        {
            _aiStates.OnBossDeath();
        }
        #endregion

        #region Parry Execution.
        public void OnParryExecutionFirstHit()
        {
            _ai.OnParryExecutionFirstHit();
        }

        public void OnParryExecutionSecondHit()
        {
            _ai.OnParryExecutionSecondHit();
        }

        public void OnParryExecutionThirdHit()
        {
            _ai.OnParryExecutionThirdHit();
        }

        public void OnExecutionFinished()
        {
            _ai.OnExecutionFinished();
        }
        #endregion

        #region Perlious Attack.
        public void Play_PerliousATKIndicator()
        {
            _ai.Play_PerliousAttackIndicator();
        }
        #endregion

        #region Move In Fix Direction Mod.
        public void SetApplyFixDirectionRootMotionToTrue()
        {
            _ai.SetApplyFixDirMoveRootMotionToTrue();
        }
        #endregion

        #region Aiming Player Mod
        public void CorrectWeaponTransformWhenAiming()
        {
            _ai.CorrectWeaponTransformWhenAiming();
        }

        public void ReverseWeaponTransformQuitAiming()
        {
            _ai.ReverseWeaponTransformQuitAiming();
        }

        public void SetIsAimingStatusToTrue()
        {
            _ai.OnAiming();
        }
        #endregion

        #region Two Stance Combat Mod.
        public void SetIsRightStanceStatus()
        {
            _ai.SetIsRightStanceBool(true);
        }

        public void SetIsLeftStanceStatus()
        {
            _ai.SetIsRightStanceBool(false);
        }
        #endregion

        #region Dual Weapon Mod.
        public void ParentEnemySecondWeaponUnderHand()
        {
            _ai.ParentSecondWeaponUnderHand();
        }

        //public void ParentEnemySecondSidearmUnderHander()
        //{
        //    ai.ParentSecondSideArmUnderHand();
        //}
        #endregion

        #region Enemy Blocking Mod.
        public void SetOnHitEnemyBlockingBreak()
        {
            _ai.OnHitBlockingBreak();
        }

        public void ResumeEnemyBlockingAfterAttack()
        {
            _ai.ResumeEnemyBlockingAfterAttack();
        }
        #endregion

        #region Egil Execution.
        public void UnParentPlayerFromExecution()
        {
            _ai.playerStates.transform.parent = null;
        }

        public void ThrowPlayerAwayFromExecution()
        {
            _ai.playerStates.KnockBackFromExecution();
            _ai.SetIsKnockedDownPlayerToTrue();
        }

        public void DealSecondDamagesToPlayer()
        {
            _ai.playerStates.DepleteHealthFromExecution_2ndHit();
        }

        public void ExecutionDamageColliderStatus(int i)
        {
            if (i == 1)
            {
                _ai.SetExecutionDamageColldierToTrue();
            }
            else
            {
                _ai.SetExecutionDamageColldierToFalse();
            }
        }
        #endregion

        #region Egil Kinematic Motion Attack.
        public void KMJ_ApplyRootMotion_InEvent()
        {
            _ai.KMJ_ApplyRootMotion_InEvent();
        }

        public void KMA_ApplyRootMotion_InEvent()
        {
            _ai.KMA_ApplyRootMotion_InEvent();
        }

        public void KMA_ResetTopDownRotation()
        {
            _ai.KMA_ResetTopDownRotation();
        }
        
        public void Egil_KMA_BeginTrackPlayerFromAbove()
        {
            _ai.isPausingTurnWithAgent = false;
        }
        
        public void Set_KMA_EnemyDamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _ai.Set_KMA_EnemyDamageColliderStatusToTrue();
            }
            else
            {
                _ai.Set_KMA_EnemyDamageColliderStatusToFalse();
            }
        }

        /// Execute in Egil KMA Mod.
        public void Egil_Execute_KMJ_AfterPhaseChanged()
        {
            _ai.PhaseChange_Execute_KMJ(false, false);
        }
        #endregion

        #region Enemy Enumerable Phase Mod.
        public void On2ndPhaseChangeEnd()
        {
            _ai.On2ndPhaseChangeEnd();
        }

        public void On3rdPhaseChangeEnd()
        {
            _ai.On3rdPhaseChangeEnd();
        }

        public void Egil3rdPhaseChange_SheathWeapon()
        {
            _ai.SetupWeaponToPosition(_ai.currentWeapon.transform);
        }

        public void Egil3rdPhaseChange_PlayChangePhase_Chain3_Anim()
        {
            _ai.Egil3rdPhaseChange_PlayChangePhase_Chain3_Anim();
        }

        public void Egil3rdPhaseChangeEnd_ParentWeaponOnHand()
        {
            _ai.Egil3rdPhaseChangeEnd_ParentWeaponOnHand();
        }

        public void Egil3rdPhaseChange_SwitchAmuletAndPlayOpeningFx()
        {
            _aiStates._aiSessionManager._aiBossManagable.Egil3rdPhaseChange_SwitchAmuletAndPlayOpeningFX();
        }
        
        public void On3rdPhaseChangeExecutePassiveAction()
        {
            _ai.On3rdPhaseChangeExecutePassiveAction();
        }
        #endregion

        #endregion
    }
}