using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "WeaponAction/Weapon Charge Action")]
    public class WeaponChargeAction : WeaponAction
    {
        [Header("Charge Attack.")]
        public NormalAttackAction _releasedAttack;
        public AnimStateVariable _chargeAttackEndAnim;

        [Header("Charge Config.")]
        public float maxChargeAmount;
        
        [NonSerialized] StateManager _states;

        public override void Execute(StateManager _states)
        {
            this._states = _states;
            _states._isActionRequireUpdateInNeglectState = true;

            if (_states.p_left_trigger && _states.rt_hold)
            {
                UpdateInputChargeAmount();
                SetIsAttackChargingStatus(true);
            }
            else
            {
                SetIsAttackChargingStatus(false);
            }
        }

        void UpdateInputChargeAmount()
        {
            _states.UpdateChargeAttackInputAmount();
        }

        public void SetIsAttackChargingStatus(bool _isAttackCharging)
        {
            if (_isAttackCharging)
            {
                if (!_states._isAttackCharging)
                {
                    _states.Base_SetIsAttackChargingToTrue();
                    PlayChargeStartAnimation();
                }
                else
                {
                    CheckIsChargeAmountReachMaxValue();
                }
            }
            else
            {
                if (_states._isAttackCharging)
                {
                    _states.Base_SetIsAttackChargingToFalse();
                    PlayChargeEndAnimation();
                }
            }
        }

        void CheckIsChargeAmountReachMaxValue()
        {
            if (_states._inputChargeAmount >= maxChargeAmount)
            {
                CheckChargeAttackInput();
                EnchantChargeAttack();
            }
        }
        
        void CheckChargeAttackInput()
        {
            if (_states.lb && _states._isReadyForChargeRelease)
            {
                _states.Base_SetIsAttackChargingToFalse();
                _releasedAttack.Execute(_states);
            }
        }

        void PlayChargeStartAnimation()
        {
            _states.CrossFadeAnimWithMoveDir(targetAnimState.animStateHash, false, true);
        }
        
        void PlayChargeEndAnimation()
        {
            _states.CrossFadeAnimWithMoveDir(_chargeAttackEndAnim.animStateHash, false, true);
        }

        void EnchantChargeAttack()
        {
            _states.EnchantChargeAttack();
        }
    }
}