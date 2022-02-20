using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Weapons/Shield Weapon Item")]
    public class ShieldWeaponItem : WeaponItem
    {
        [Header("One hand Actions")]
        public NormalAttackAction oh_weaponAction_light;
        public HoldAttackAction oh_weaponAction_heavy;
        public WeaponParryAction oh_weaponAction_oppose2;

        [Header("Two hand Actions")]
        public NormalAttackAction th_weaponAction_light_1;
        public HoldAttackAction th_weaponAction_heavy_1;
        public WeaponParryAction th_weaponAction_heavy_2;

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
            else if(_states.rb)
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
    }
}