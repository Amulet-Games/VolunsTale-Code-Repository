using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Weapons/Fist Weapon Item")]
    public class FistWeaponItem : WeaponItem
    {
        [Header("Hold ATK Profiles.")]
        public WA_Effect_Profile hold_halfComp_effect_profile;
        public WA_Effect_Profile hold_fullComp_effect_profile;

        [Header("One hand Actions")]
        public WeaponAttackAction oh_weaponAction_light;
        public WeaponAttackAction oh_weaponAction_heavy;
        public WeaponAttackAction oh_weaponAction_oppose1;
        public WeaponAttackAction oh_weaponAction_oppose2;

        [Header("Two hand Actions")]
        public WeaponAttackAction th_weaponAction_light_1;
        public WeaponAttackAction th_weaponAction_light_2;
        public WeaponAttackAction th_weaponAction_heavy_1;
        public WeaponAttackAction th_weaponAction_heavy_2;

        #region Weapon Input.
        public override void HandleDominantHandWeaponInput(StateManager _states)
        {
            _states.rt_hold = Input.GetButton("RT_Hold");
        }

        public override void HandleOpposeHandWeaponInput(StateManager _states)
        {
            _states.lt_hold = Input.GetButton("LT_Hold");
        }

        public override void HandleTwoHandingWeaponInput(StateManager _states)
        {
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
            if (_states.p_left_trigger && _states.lb || _states._isHoldingLT)
            {
                _states._currentNeglectInputAction = oh_weaponAction_oppose2;
                return true;
            }
            else if (_states.lb)
            {
                _states._currentNeglectInputAction = oh_weaponAction_oppose1;
                return true;
            }

            return false;
        }

        public override bool HandleTwoHandingWeaponAction(StateManager _states)
        {
            if (_states.p_left_trigger && _states.rb)
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
            else if (_states.lb)
            {
                _states._currentNeglectInputAction = th_weaponAction_light_2;
                return true;
            }

            return false;
        }
        #endregion

        #region Override - Get Hold ATK Profiles.
        public override WA_Effect_Profile Get_Hold_HalfComp_Effect()
        {
            return hold_halfComp_effect_profile;
        }

        public override WA_Effect_Profile Get_Hold_FullComp_Effect()
        {
            return hold_fullComp_effect_profile;
        }
        #endregion
    }
}