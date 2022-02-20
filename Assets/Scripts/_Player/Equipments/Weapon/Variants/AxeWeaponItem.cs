using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Weapons/Axe Weapon Item")]
    public class AxeWeaponItem : WeaponItem
    {
        [Header("Charge ATK Profiles.")]
        public WA_Effect_Profile chargeEnchant_profile;
        public WA_Effect_Profile chargeAttack_profile;

        [Header("One hand Actions")]
        public NormalAttackAction oh_weaponAction_light;
        public HoldAttackAction oh_weaponAction_heavy;
        public NormalAttackAction oh_weaponAction_oppose2;

        [Header("Two hand Actions")]
        public NormalAttackAction th_weaponAction_light_1;
        public WeaponChargeAction th_weaponAction_heavy_1;
        public AxeBuffAction th_weaponAction_heavy_2;

        #region Weapon Input.
        public override void HandleDominantHandWeaponInput(StateManager _states)
        {
            _states.rt_hold = Input.GetButton("RT_Hold");
        }

        public override void HandleOpposeHandWeaponInput(StateManager _states)
        {
            _states.lb_hold = Input.GetButton("LB_Hold");
        }

        public override void HandleTwoHandingWeaponInput(StateManager _states)
        {
            _states.rt_hold = Input.GetButton("RT_Hold");
            _states.lb_hold = Input.GetButton("LB_Hold");
        }
        #endregion

        #region Weapon Action.
        public override bool HandleDominantHandWeaponAction(StateManager _states)
        {
            if (_states.p_left_trigger && _states.rb || _states._isHoldingRT)
            {
                _states._currentNeglectInputAction = oh_weaponAction_heavy;
                return true;
            }
            else if (_states.rb)
            {
                _states._currentNeglectInputAction = oh_weaponAction_light;
                return true;
            }

            return false;
        }

        public override bool HandleOpposeHandWeaponAction(StateManager _states)
        {
            if (_states.p_left_trigger && _states.lb)
            {
                _states._currentNeglectInputAction = oh_weaponAction_oppose2;
                return true;
            }
            else if (_states.lb || _states._isHoldingLB)
            {
                _states.SetIsBlockingStatus(true);
            }

            return false;
        }

        public override bool HandleTwoHandingWeaponAction(StateManager _states)
        {
            if (_states.p_left_trigger && _states.rb || _states._isHoldingRT)
            {
                _states._currentNeglectInputAction = th_weaponAction_heavy_1;
                return true;
            }
            else if (_states.p_left_trigger && _states.lb)
            {
                _states._currentNeglectInputAction = th_weaponAction_heavy_2;
                return true;
            }
            else if (_states.rb)
            {
                _states._currentNeglectInputAction = th_weaponAction_light_1;
                return true;
            }
            else if (_states.lb || _states._isHoldingLB)
            {
                _states.SetIsBlockingStatus(true);
            }

            return false;
        }
        #endregion

        #region Override - Get Charge ATK Profiles.
        public override WA_Effect_Profile Get_ChargeEnchant_Effect()
        {
            return chargeEnchant_profile;
        }

        public override WA_Effect_Profile Get_ChargeAttack_Effect()
        {
            return chargeAttack_profile;
        }
        #endregion
    }
}