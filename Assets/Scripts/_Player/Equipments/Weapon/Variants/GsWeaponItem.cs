using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Weapons/Gs Weapon Item")]
    public class GsWeaponItem : WeaponItem
    {
        [Header("One hand Actions")]
        public NormalAttackAction oh_weaponAction_light;
        public NormalAttackAction oh_weaponAction_heavy;
        public NormalAttackAction oh_weaponAction_oppose2;

        [Header("Two hand Actions")]
        public NormalAttackAction th_weaponAction_light_1;
        public NormalAttackAction th_weaponAction_heavy_1;
        public NormalAttackAction th_weaponAction_heavy_2;

        #region Weapon Input.
        public override void HandleDominantHandWeaponInput(StateManager _states)
        {
            //_states.rb = Input.GetButtonDown("RB");
            //_states.rt = Input.GetButton("RT");
        }

        public override void HandleOpposeHandWeaponInput(StateManager _states)
        {
            //_states.lb = Input.GetButton("LB");
            //_states.lt = Input.GetButton("LT");
        }

        public override void HandleTwoHandingWeaponInput(StateManager _states)
        {
            //_states.rb = Input.GetButtonDown("RB");
            //_states.rt = Input.GetButton("RT");
            //_states.lb = Input.GetButton("LB");
            //_states.lt = Input.GetButton("LT");
        }
        #endregion

        #region Weapon Action.
        public override bool HandleDominantHandWeaponAction(StateManager _states)
        {
            if (_states.p_left_trigger && _states.rb)
            {
                Debug.Log("Rt");
                return true;
            }
            else if (_states.rb)
            {
                Debug.Log("Rb");
                return true;
            }

            return false;
        }

        public override bool HandleOpposeHandWeaponAction(StateManager _states)
        {
            if (_states.p_left_trigger && _states.lb)
            {
                Debug.Log("Lt");
                return true;
            }
            else if (_states.lb)
            {
                Debug.Log("Lb");
                return true;
            }

            return false;
        }

        public override bool HandleTwoHandingWeaponAction(StateManager _states)
        {
            return false;
        }
        #endregion
    }
}