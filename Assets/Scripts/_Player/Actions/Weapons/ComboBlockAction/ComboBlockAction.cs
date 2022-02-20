using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "WeaponAction/Combo Action/Combo Block Action")]
    public class ComboBlockAction : WeaponAttackAction
    {
        public override void Execute(StateManager _states)
        {
            _states.ExecuteComboBlockAction();
        }
    }
}