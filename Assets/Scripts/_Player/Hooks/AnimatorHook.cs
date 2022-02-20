using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    ///* MUST BE DISABLED IN PREFAB.
    public class AnimatorHook : MonoBehaviour
    {
        [Header("Refs")]
        [ReadOnlyInspector] public Animator anim = null;
        [ReadOnlyInspector] public StateManager _states = null;
        [ReadOnlyInspector] public PlayerIKHandler playerIKHandler = null;
        [ReadOnlyInspector] public WA_TrailFx_Handler currentTrailFxHandler;
        [ReadOnlyInspector] public FootStepHook f_hook = null;

        [Header("Non Serialized.")]
        [NonSerialized] public SavableInventory _inventory = null;

        [Header("Linear Animation Job.")]
        public List<LinearAnimationJob> currentAnimationJobs = new List<LinearAnimationJob>();
        public bool _isAnimationJobsEmpty;
        
        public void Init(StateManager _states)
        {
            this._states = _states;
            
            InitAnimator();

            InitIKHandler();
            
            InitAnimationJobStatus();
            
            OnFinishSetupEnableComponent();
        }
        
        #region Callbacks.
        private void OnAnimatorMove()
        {
            _states.ApplyRootMotions_OnAnimMove();
        }

        private void OnAnimatorIK()
        {
            playerIKHandler.OnAnimatorIKTick();
        }
        #endregion

        #region Mono Tick.
        public void MonitorAnimationJobs()
        {
            if (!_isAnimationJobsEmpty)
            {
                if (anim.GetBool(_states.p_IsAnimationJobFinished_hash))
                {
                    if (currentAnimationJobs.Count > 0)
                    {
                        ExecuteNewAnimationJob();
                    }
                    else
                    {
                        _isAnimationJobsEmpty = true;
                    }
                }
            }

            playerIKHandler.AnimatorHookTick();
        }
        #endregion

        #region Animation Events.

        #region Weapons.
        // Two Handing
        public void BringOpposeWeaponToRightHand()
        {
            _inventory.BringOpposeWeaponToRightHand();
        }

        public void ReturnOpposeWeaponToLeftHand()
        {
            _inventory.ParentLhCurrentUnderHand();
        }

        // Inventory Menu Off TwoHanding.
        public void PassRhWeaponToLeftHand()
        {
            _inventory.PassRhWeaponToLeftHand();
        }

        public void PassLhWeaponToRightHand()
        {
            _inventory.PassLhWeaponToRightHand();
        }

        // Play Two Hand Fist Locomotion.
        public void BeginPlayThFistLocomotion()
        {
            _states.CrossFadeAnimWithMoveDir(_inventory._twoHandingWeapon_referedItem.GetThLocomotionHashByType(), false, false);
        }

        // Damage Colliders.
        public void SetRhDamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _inventory._rightHandWeapon.weaponHook.SetColliderStatusToTrue();
            }
            else
            {
                _inventory._rightHandWeapon.weaponHook.SetColliderStatusToFalse();
            }
        }

        public void SetLhDamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _inventory._leftHandWeapon.weaponHook.SetColliderStatusToTrue();
            }
            else
            {
                _inventory._leftHandWeapon.weaponHook.SetColliderStatusToFalse();
            }
        }

        public void SetThDamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _inventory._twoHandingWeapon.weaponHook.SetColliderStatusToTrue();
            }
            else
            {
                _inventory._twoHandingWeapon.weaponHook.SetColliderStatusToFalse();
            }
        }

        // Right Elbow Damage Collider.
        public void Set_Rh_RightElbow_DamageColiderStatus(int v)
        {
            if (v == 1)
            {
                _states.SetRightElbowDmgColliderToTrue(_inventory._rightHandWeapon);
            }
            else
            {
                _states.SetRightElbowDmgColliderToFalse();
            }
        }

        public void Set_Lh_RightElbow_DamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _states.SetRightElbowDmgColliderToTrue(_inventory._leftHandWeapon);
            }
            else
            {
                _states.SetRightElbowDmgColliderToFalse();
            }
        }

        public void Set_Th_RightElbow_DamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _states.SetRightElbowDmgColliderToTrue(_inventory._twoHandingWeapon);
            }
            else
            {
                _states.SetRightElbowDmgColliderToFalse();
            }
        }

        // Left Elbow Damage Collider.
        public void Set_Rh_LeftElbow_DamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _states.SetLeftElbowDmgColliderToTrue(_inventory._rightHandWeapon);
            }
            else
            {
                _states.SetLeftElbowDmgColliderToFalse();
            }
        }

        public void Set_Lh_LeftElbow_DamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _states.SetLeftElbowDmgColliderToTrue(_inventory._leftHandWeapon);
            }
            else
            {
                _states.SetLeftElbowDmgColliderToFalse();
            }
        }

        public void Set_Th_LeftElbow_DamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _states.SetLeftElbowDmgColliderToTrue(_inventory._twoHandingWeapon);
            }
            else
            {
                _states.SetLeftElbowDmgColliderToFalse();
            }
        }

        // Right Leg Damage Collider.
        public void Set_Rh_RightLeg_DamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _states.SetRightLegDmgColliderToTrue(_inventory._rightHandWeapon);
            }
            else
            {
                _states.SetRightLegDmgColliderToFalse();
            }
        }

        public void Set_Lh_RightLeg_DamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _states.SetRightLegDmgColliderToTrue(_inventory._leftHandWeapon);
            }
            else
            {
                _states.SetRightLegDmgColliderToFalse();
            }
        }

        public void Set_Th_RightLeg_DamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _states.SetRightLegDmgColliderToTrue(_inventory._twoHandingWeapon);
            }
            else
            {
                _states.SetRightLegDmgColliderToFalse();
            }
        }

        // Left Leg Damage Collider.
        public void Set_Rh_LeftLeg_DamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _states.SetLeftLegDmgColliderToTrue(_inventory._rightHandWeapon);
            }
            else
            {
                _states.SetLeftLegDmgColliderToFalse();
            }
        }

        public void Set_Lh_LeftLeg_DamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _states.SetLeftLegDmgColliderToTrue(_inventory._leftHandWeapon);
            }
            else
            {
                _states.SetLeftLegDmgColliderToFalse();
            }
        }

        public void Set_Th_LeftLeg_DamageColliderStatus(int v)
        {
            if (v == 1)
            {
                _states.SetLeftLegDmgColliderToTrue(_inventory._twoHandingWeapon);
            }
            else
            {
                _states.SetLeftLegDmgColliderToFalse();
            }
        }

        // Parent Transform.
        public void ParentRhWeaponUnderHand()
        {
            _inventory.ParentRhCurrentUnderHand();
        }

        public void ParentLhWeaponUnderHand()
        {
            _inventory.ParentLhCurrentUnderHand();
        }

        public void ParentRhWeaponUnderSheath()
        {
            _inventory.ParentRhListWeaponUnderSheath();
        }

        public void ParentLhWeaponUnderSheath()
        {
            _inventory.ParentLhListWeaponUnderSheath();
        }

        // Return Weapon To Backpack In Anim.
        public void ExecuteBackpackOperationInAnim()
        {
            _inventory.ExecuteBackpackOperationInAnim();
        }
        
        // Show / Hide Visible Weapons.
        public void HideCurrentVisibleShields()
        {
            _inventory.HideCurrentVisibleShields();
        }

        public void ShowCurrentVisibleShields()
        {
            _inventory.ShowCurrentVisibleShields();
        }

        public void HideCurrent_Lh_Shields()
        {
            _inventory.Hide_Lh_CurrentShield();
        }

        public void ShowCurrent_Lh_Shields()
        {
            _inventory.Show_Lh_CurrentShield();
        }

        public void HideCurrent_Rh_Shields()
        {
            _inventory.Hide_Rh_CurrentShield();
        }

        public void ShowCurrent_Rh_Shields()
        {
            _inventory.Show_Rh_CurrentShield();
        }

        public void HideBothCurrentWeapons()
        {
            _inventory.HideBothCurrentWeapons();
        }

        public void ShowBothCurrentWeapons()
        {
            _inventory.ShowBothCurrentWeapons();
        }

        public void Hide_Rh_Th_CurrentWeapon()
        {
            _inventory.Hide_Rh_Th_CurrentWeapon();
        }

        public void Show_Rh_Th_CurrentWeapon()
        {
            _inventory.Show_Rh_Th_CurrentWeapon();
        }

        public void HideLhCurrentWeapon()
        {
            _inventory.HideLhCurrentWeapon();
        }

        public void ShowLhCurrentWeapon()
        {
            _inventory.ShowLhCurrentWeapon();
        }

        // Show / Hide Amulet.
        public void IngiteBonfireShowAmulet()
        {
            _inventory.runtimeAmulet.ShowWhenIgnite();
        }

        public void LevelupShowAmulet()
        {
            _inventory.runtimeAmulet.ShowWhenLevelup();
        }

        public void HideAmulet()
        {
            _inventory.runtimeAmulet.SheathAmulet();
        }
        #endregion

        #region WA Slash Effects.

        #region Normal Attack.
        public void Play_1st_WA_Effect()
        {
            _states._currentAttackAction.Play_1st_Effect();
        }

        public void Play_2nd_WA_Effect()
        {
            _states._currentAttackAction.Play_2nd_Effect();
        }

        public void Play_3rd_WA_Effect()
        {
            _states._currentAttackAction.Play_3rd_Effect();
        }

        public void Play_4th_WA_Effect()
        {
            _states._currentAttackAction.Play_4th_Effect();
        }
        #endregion

        #region Hold Attack.
        public void Play_HoldATK_Loop_Effect()
        {
            _states.Play_HoldATK_Loop_Effect();
        }

        public void Play_HoldATK_Comp_Effect()
        {
            _states.Play_HoldATK_Comp_Effect();
        }
        #endregion

        #region Charge Attack.
        public void Play_TH_ChargeAttack_WA_Effect()
        {
            _states.Play_Charge_Attack_Effect();
        }
        #endregion

        #endregion

        #region WA Trail Fx Effects.

        #region Buff.
        public void Play_TH_BuffTrailFx()
        {
            _inventory._twoHandingWeapon._trailFxHandler.Play_Buff_TrailFx();
        }

        public void Stop_TH_BuffTrailFx()
        {
            currentTrailFxHandler.Play_NullState_TrailFx();
        }
        #endregion

        #region Charge Enchant.
        public void Play_ChargeATK_Loop_Effect()
        {
            _states.Play_ChargeATK_Loop_Effect();
        }

        public void Stop_ChargeATK_Loop_Effect()
        {
            _states.Stop_HoldATK_Loop_Effect();
        }
        
        public void Stop_TH_ChargeAttackTrailFx()
        {
            currentTrailFxHandler.Play_NullState_TrailFx();
        }
        #endregion

        #region Hold Ready.
        public void Play_RH_HoldReadyTrailFx()
        {
            _inventory._rightHandWeapon._trailFxHandler.Play_HoldReady_TrailFx();
        }
        
        public void Play_LH_HoldReadyTrailFx()
        {
            _inventory._leftHandWeapon._trailFxHandler.Play_HoldReady_TrailFx();
        }
        
        public void Play_TH_HoldReadyTrailFx()
        {
            _inventory._twoHandingWeapon._trailFxHandler.Play_HoldReady_TrailFx();
        }
        #endregion

        #region Hold Attack.
        public void Play_RH_HoldAttackTrailFx()
        {
            _states.Stop_HoldATK_Loop_Effect();
            _inventory._rightHandWeapon._trailFxHandler.Play_HoldATK_TrailFx();
        }

        public void Stop_RH_HoldAttackTrailFx()
        {
            currentTrailFxHandler.Play_NullState_TrailFx();
        }

        public void Play_LH_HoldAttackTrailFx()
        {
            _states.Stop_HoldATK_Loop_Effect();
            _inventory._leftHandWeapon._trailFxHandler.Play_HoldATK_TrailFx();
        }

        public void Stop_LH_HoldAttackTrailFx()
        {
            currentTrailFxHandler.Play_NullState_TrailFx();
        }

        public void Play_TH_HoldAttackTrailFx()
        {
            _states.Stop_HoldATK_Loop_Effect();
            _inventory._twoHandingWeapon._trailFxHandler.Play_HoldATK_TrailFx();
        }

        public void Stop_TH_HoldAttackTrailFx()
        {
            currentTrailFxHandler.Play_NullState_TrailFx();
        }
        #endregion

        #region Normal Attack.
        public void Play_RH_NormalAttackTrailFx()
        {
            _inventory._rightHandWeapon._trailFxHandler.Play_NormalATK_TrailFx();
        }

        public void Stop_RH_NormalAttackTrailFx()
        {
            currentTrailFxHandler.Play_NullState_TrailFx();
        }

        public void Play_LH_NormalAttackTrailFx()
        {
            _inventory._leftHandWeapon._trailFxHandler.Play_NormalATK_TrailFx();
        }

        public void Stop_LH_NormalAttackTrailFx()
        {
            currentTrailFxHandler.Play_NullState_TrailFx();
        }

        public void Play_TH_NormalAttackTrailFx()
        {
            _inventory._twoHandingWeapon._trailFxHandler.Play_NormalATK_TrailFx();
        }

        public void Stop_TH_NormalAttackTrailFx()
        {
            currentTrailFxHandler.Play_NullState_TrailFx();
        }
        #endregion

        #region Parry.
        public void Play_LH_ParryTrailFx()
        {
            _inventory._leftHandWeapon._trailFxHandler.Play_Parry_TrailFx();
        }

        public void Stop_LH_ParryTrailFx()
        {
            currentTrailFxHandler.Play_NullState_TrailFx();
        }

        public void Play_TH_ParryTrailFx()
        {
            _inventory._twoHandingWeapon._trailFxHandler.Play_Parry_TrailFx();
        }

        public void Stop_TH_ParryTrailFx()
        {
            currentTrailFxHandler.Play_NullState_TrailFx();
        }
        #endregion

        #endregion

        #region Spell.
        //public void InitiateThrowForProjectile()
        //{
        //    _states.ThrowSpellProjectile();
        //}

        //public void CreateSpellParticle()
        //{
        //    _states.CreateSpellParticle();
        //}
        #endregion

        #region Consumables.
        public void ExecuteConsumableEffect()
        {
            _inventory.UseConsumableInAnim();
        }

        public void HideCurrentConsumableInAnim()
        {
            _states._savableInventory.HideCurrentConsumableInAnim();
        }

        public void SetIsUsingConsumableToFalse()
        {
            _states._savableInventory.SetIsUsingConsumableStatusToFalse();
        }
        #endregion

        #region Attack Root Motion.
        public void SetAttackRootMotionWhenAttackIsOver(float v)
        {
            if (_states.isComboAvailable)
                _states.attackRootMotion = v;
        }

        public void SetUseAttackRootMotionToFalse()
        {
            _states.SetUseAttackRootMotionToFalse();
        }
        #endregion

        #region Turn Root Motion.
        public void SetCurrentTurningTypeStatusToFalse()
        {
            _states.SetCurrentTurningTypeStatusToFalse();
        }

        public void SetMoveDirTurnRootMotionStatus(int v)
        {
            if (v == 1)
            {
                _states.applyTurningWithMoveDir = true;
            }
            else
            {
                _states.applyTurningWithMoveDir = false;
            }
        }

        public void SetLockonDirTurnRootMotionStatus(int v)
        {
            if (v == 1)
            {
                _states.applyTurningWithLockonDir = true;
            }
            else
            {
                _states.applyTurningWithLockonDir = false;
            }
        }

        public void SetInverseMoveDirTurnRootMotionStatus(int v)
        {
            if (v == 1)
            {
                _states.applyTurningWithInverseMoveDir = true;
            }
            else
            {
                _states.applyTurningWithInverseMoveDir = false;
            }
        }
        #endregion

        #region Quit Animation.
        public void SetCanQuitNeglectStateToTrue()
        {
            _states.SetCanQuitNeglectStateToTrue();
        }

        public void SetIsComboAvailableStatusToFalse()
        {
            _states.isComboAvailable = false;
        }
        #endregion

        #region Checkpoint.
        public void StartRestingAtBonfire()
        {
            _states.OnCheckpointAgentInteraction();
        }
        
        public void EndRestingAtBonfire()
        {
            _states._inp.SetIsInMainHudStatus(true);
        }

        public void SnapToBonfireSeatPosition()
        {
            _states.SnapToBonfireSeatPosition();
        }
        
        public void SnapToLevelupPosition()
        {
            _states.SnapToLevelupPosition();
        }

        public void SnapToExitBonfirePosition()
        {
            _states.SnapToExitBonfirePosition();
        }

        public void OpenCheckpointMenu()
        {
            _states._inp.SetIsInCheckpointMenuStatus(true);
        }

        public void OpenLevelupMenu()
        {
            _states._inp.SetIsInLevelingMenuStatus(true);
        }
        #endregion

        #region Cant Be Damaged / Death.
        public void SetIsCantBeDamagedStatus(int value)
        {
            if (value == 1)
            {
                _states.isCantBeDamaged = true;
            }
            else
            {
                _states.isCantBeDamaged = false;
            }
        }

        public void OnDeathHideMainHud()
        {
            _states._inp.OnDeathHideMainHud();
        }

        public void OnDeathBeginBlur()
        {
            _states.BlurInScreenOnDeath();
        }

        public void OnDeathBeginDissolve()
        {
            _states.DissolveOutPlayerOnDeath();
        }
        #endregion

        #region Roll / Jump.
        public void SetCanQuitOffGroundEarlyToTrue()
        {
            _states.canQuitOffGroundEarly = true;
        }

        public void GetOffGroundPoint()
        {
            _states.GetOffGroundPointWhenJumping();
        }
        #endregion

        #region Sprinting.
        public void SprintingEndSetIsSprintingFalse()
        {
            _states.SprintingEndSetIsSprintingFalse();
        }
        #endregion

        #region Interactions.
        public void OnAnimEventInteract()
        {
            _states._currentInteractable.OnAnimEventInteract();
        }
        #endregion

        #region Executions.
        public void AlertEnemyReceiveParryExecution()
        {
            _states.AlertEnemyReceiveParryExecution();
        }

        public void ParentExecuteTargetToWeapon()
        {
            _states.ParentExecuteTargetToWeapon();
        }

        public void UnParentExecuteTargetFromWeapon()
        {
            _states.UnParentExecuteTargetFromWeapon();
        }

        public void Switch_HitSourceColliderTransform_To_L_Lower_Leg()
        {
            _states.Switch_HitSourceColliderTransform_To_L_Lower_Leg();
        }
        #endregion

        #region Weapon Action.
        public void ManualSetAttackHoldSpeed()
        {
            _states.SetHoldAttackSpeedFromAnimEvent();
        }

        public void SetIsReadyForChargeReleaseToTrue()
        {
            _states.SetIsReadyForChargeReleaseToTrue();
        }
        #endregion

        #region Knock Back Hit / Execution.
        public void ExecutionUnParentTransform()
        {
            _states.transform.parent = null;
        }

        public void OnKnockBackResetRotation()
        {
            _states.OnKnockBackResetRotation();
        }

        public void KnockBackStartGetupCounter()
        {
            _states.KnockBackStartGetupCounter();
        }

        public void DepleteHealthFromExecution_2ndHit()
        {
            _states.DepleteHealthFromExecution_2ndHit();
        }
        #endregion

        #region Comments.
        public void CommentOnLockedDoor()
        {
            _states._commentHandler.RegisterLockedDoorCommentMoment();
        }

        public void CommentOnOpenedDoor()
        {
            _states._commentHandler.RegisterOpenedDoorCommentMoment();
        }

        public void CommentOnPickup()
        {
            _states._commentHandler.RegisterPickupItemCommentMoment();
        }
        #endregion

        #region Foot Step.
        public void Play_L_FootStepInAnim()
        {
            f_hook.Play_L_FootStep_Immediate();
        }

        public void Play_R_FootStepInAnim()
        {
            f_hook.Play_R_FootStep_Immediate();
        }

        public void Play_Both_FootStepInAnim()
        {
            f_hook.Play_Both_FootStep_Immediate();
        }
        #endregion

        #endregion

        #region Init.
        void InitAnimator()
        {
            anim = GetComponent<Animator>();
            _states.anim = anim;
            anim.applyRootMotion = false;
        }

        void InitIKHandler()
        {
            playerIKHandler = GetComponent<PlayerIKHandler>();
            playerIKHandler.anim = anim;

            playerIKHandler.Init(_states);
        }
        
        void InitAnimationJobStatus()
        {
            currentAnimationJobs.Clear();
        }

        void OnFinishSetupEnableComponent()
        {
            enabled = true;
        }
        #endregion

        #region Linear Animation Job.
        public void RegisterNewAnimationJob(int _animHash, bool _isNeglectInput)
        {
            currentAnimationJobs.Add(new LinearAnimationJob(_animHash, _isNeglectInput));
            _isAnimationJobsEmpty = false;
        }
        
        void ExecuteNewAnimationJob()
        {
            _states.CrossFadeAnimWithMoveDir(currentAnimationJobs[0]._animHash, false, currentAnimationJobs[0]._isNeglectInput);
            currentAnimationJobs.Remove(currentAnimationJobs[0]);
            anim.SetBool(_states.p_IsAnimationJobFinished_hash, false);
        }

        public void SetIsAnimationJobFinishedToTrue()
        {
            anim.SetBool(_states.p_IsAnimationJobFinished_hash, true);
        }
        #endregion
    }

    public struct LinearAnimationJob
    {
        public int _animHash;
        public bool _isNeglectInput;

        public LinearAnimationJob(int _animHash, bool _isNeglectInput)
        {
            this._animHash = _animHash;
            this._isNeglectInput = _isNeglectInput;
        }
    }

    public struct HandleIKJob
    {
        public IKUsageType _IKUsageType;
        public float _desiredLookAtWeight;      /// If IK Usage is not 'NotUseIK', this value will overwrite the current look at weight in IK Handler.
        public Vector3 _lookAtDesirePosition;   /// To not change the LookAt position, register this position as vector3.zero.

        public HandleIKJob(IKUsageType _IKUsageType, float _desiredLookAtWeight, Vector3 _lookAtDesirePosition)
        {
            this._IKUsageType = _IKUsageType;
            this._desiredLookAtWeight = _desiredLookAtWeight;
            this._lookAtDesirePosition = _lookAtDesirePosition;
        }
    }

    public enum IKUsageType
    {
        NotUseIK,
        HeadOnly,
        UpperBody,
        OnlyLeftArm
    }
}
