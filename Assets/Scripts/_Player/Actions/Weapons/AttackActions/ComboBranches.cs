using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [System.Serializable]
    public class ComboBranches
    {
        public WeaponAction rb_ComboAttackAction;
        public WeaponAction lb_ComboAttackAction;
        public WeaponAction rt_ComboAttackAction;
        public WeaponAction lt_ComboAttackAction;

        public WeaponAction GetNextAttackAction(StateManager _states)
        {
            if (_states.p_left_trigger && _states.rb)
            {
                return rt_ComboAttackAction;
            }
            else if (_states.p_left_trigger && _states.lb)
            {
                return lt_ComboAttackAction;
            }
            else if (_states.rb)
            {
                return rb_ComboAttackAction;
            }
            else if (_states.lb)
            {
                return lb_ComboAttackAction;
            }

            return null;
        }
    }
}