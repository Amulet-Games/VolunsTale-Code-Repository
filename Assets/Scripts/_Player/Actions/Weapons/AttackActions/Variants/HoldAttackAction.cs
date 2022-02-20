using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "WeaponAction/Attack Action/Hold Attack")]
    public class HoldAttackAction : WeaponAttackAction
    {
        [Header("Hold Config.")]
        public bool isLTHoldAttack;
        public bool isUpdateHoldAmountFromEvent;
        public float minAnimSpeed;
        public float attackSpeed = 1.5f;
        public float fullyHeldAttackRootMotion = 200;
        public float maxInputHoldAmount;
        
        [Header("Status.")]
        [NonSerialized] float _inputHoldAmount;
        [NonSerialized] bool _isHoldingTrigger;
        [NonSerialized] StateManager _states;

        public override void Execute(StateManager _states)
        {
            this._states = _states;
            _states._isActionRequireUpdateInNeglectState = true;

            if (isLTHoldAttack)
            {
                // if player is pressing lt and weapon charge.
                if (_states.lt_hold && _states.p_left_trigger)
                {
                    UpdateInputHoldAmount();
                    SetIsHoldingTriggerStatus(true);
                }
                else
                {
                    SetIsHoldingTriggerStatus(false);
                }
            }
            else
            {
                if (_states.rt_hold && _states.p_left_trigger)
                {
                    UpdateInputHoldAmount();
                    SetIsHoldingTriggerStatus(true);
                }
                else
                {
                    SetIsHoldingTriggerStatus(false);
                }
            }

            void UpdateInputHoldAmount()
            {
                _inputHoldAmount += _states._delta;
            }
        }
        
        void SetIsHoldingTriggerStatus(bool _isHoldingTrigger)
        {
            if (_isHoldingTrigger)
            {
                if (!this._isHoldingTrigger)
                {
                    /// First time holded LT / RT button.
                    this._isHoldingTrigger = true;

                    /// Set is holding LT / RT to true.
                    /// Set Current Hold Attack Weapon.
                    SetCurrentHoldingWeapon();

                    SetRootMotionTriggerHeld();
                    SetAttackPowerMultipier();
                    SetStateStatusTriggerHeld();
                    DelpeteStateStamina();
                    PlayAttackAnimation();
                    Play_HoldATK_Loop_Effect();
                }
                else
                {
                    /// Holding Button.
                    CheckIsHoldAmountReachMaxValue();
                }

                void SetRootMotionTriggerHeld()
                {
                    _states.attackRootMotion = 0;
                }

                void SetAttackPowerMultipier()
                {
                    _states.statsHandler._attPowMulti_weaponAction = 1;
                }

                void SetStateStatusTriggerHeld()
                {
                    _states._isAttacking = true;
                    _states.applyAttackRootMotion = true;
                    _states._currentAttackAction = this;

                    _states.SetIsTwoHandFistAttacking();
                    _states.OnAttackActionCheckIsRunningStatus();
                    _states.OnAttackActionInterruptComment();

                    if (!isUpdateHoldAmountFromEvent)
                    {
                        _states.SetHoldAttackSpeedMultiAnimFloat(minAnimSpeed);
                    }
                    else
                    {
                        _states.RegisterManualHoldAttackSpeed(minAnimSpeed);
                    }

                    _states.WeaponActionResetVerticalHorizontalPara();  
                }

                void DelpeteStateStamina()
                {
                    _states.statsHandler.DecrementPlayerStaminaWhenAttacking(staminaUsage);
                }

                void PlayAttackAnimation()
                {
                    if (_states.isLockingOn)
                    {
                        if (isTurnWithMoveDirWhenLockon)
                        {
                            _states.PlayMoveDirAttackAnimation(targetAnimState.animStateHash);
                        }
                        else
                        {
                            _states.PlayLockonDirAttackAnimation(targetAnimState.animStateHash);
                        }
                    }
                    else
                    {
                        _states.PlayMoveDirAttackAnimation(targetAnimState.animStateHash);
                    }
                }

                void Play_HoldATK_Loop_Effect()
                {
                    if (!isUpdateHoldAmountFromEvent)
                        _states.Play_HoldATK_Loop_Effect();
                }

                void CheckIsHoldAmountReachMaxValue()
                {
                    if (_inputHoldAmount >= maxInputHoldAmount)
                    {
                        _states.SetIsTriggerFullyHeldToTrue();
                        SetIsHoldingTriggerStatus(false);
                    }
                }
            }
            else
            {
                if (this._isHoldingTrigger)
                {
                    /// Released Button.
                    this._isHoldingTrigger = false;

                    /// Set is Holding LT / RT to false.
                    if (isLTHoldAttack)
                        _states._isHoldingLT = false;
                    else
                        _states._isHoldingRT = false;

                    ResetInputHoldAmount();
                    SetStateStatusTriggerReleased();
                    SetRootMotionTriggerReleased();
                }

                void SetRootMotionTriggerReleased()
                {
                    _states.ignoreAttackRootCalculate = ignoreRootCalculate;

                    if (_states._hasHoldAtkReachedMaximum)
                        _states.attackRootMotion = fullyHeldAttackRootMotion;
                    else
                        _states.attackRootMotion = actionRootMotion;
                }

                void SetStateStatusTriggerReleased()
                {
                    _states._isActionRequireUpdateInNeglectState = false;
                    _states._currentNeglectInputAction = null;

                    _states.SetHoldAttackSpeedMultiAnimFloat(attackSpeed);
                }
            }

            void SetCurrentHoldingWeapon()
            {
                if (_states._isTwoHanding)
                {
                    _states.SetCurrent_2H_HoldAttackWeapon();
                }
                else
                {
                    _states.SetCurrent_1H_HoldAttackWeapon(isLTHoldAttack);
                }
            }

            void ResetInputHoldAmount()
            {
                _inputHoldAmount = 0;
            }
        }
    }
}