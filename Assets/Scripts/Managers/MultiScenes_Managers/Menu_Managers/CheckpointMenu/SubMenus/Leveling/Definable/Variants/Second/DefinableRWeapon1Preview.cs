using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DefinableRWeapon1Preview : DefinableStatsPreview
    {
        [Header("Refered Runtime Weapon.")]
        [ReadOnlyInspector] public RuntimeWeapon _rhWeapon_1;
        [ReadOnlyInspector] public double _menuOpened_attPow;

        public override void RedrawIncrementPreview()
        {
            if (_rhWeapon_1)
            {
                _afterChangesText.text = ((int)_rhWeapon_1.ReturnWeaponPreviewAttackPower()).ToString();
                _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
            }
        }

        public override void RedrawDecrementPreview()
        {
            if (_rhWeapon_1)
            {
                double _edited_attPow = _rhWeapon_1.ReturnWeaponPreviewAttackPower();
                _afterChangesText.text = ((int)_edited_attPow).ToString();

                if (_edited_attPow - _menuOpened_attPow < 0.5f)
                    _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
            }
        }

        public override void RedrawConfirmedStatsPreview()
        {
            _rhWeapon_1 = _statsHandler._states._savableInventory.rightHandSlots[0];
            if (_rhWeapon_1)
            {
                _menuOpened_attPow = _rhWeapon_1.ReturnWeaponPreviewAttackPower();
                string _rawAttPower = ((int)_menuOpened_attPow).ToString();
                _beforeChangesText.text = _rawAttPower;
                _afterChangesText.text = _rawAttPower;
            }
            else
            {
                _beforeChangesText.text = "-";
                _afterChangesText.text = "-";
            }

            _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawDefinitionDetail()
        {
            LevelingMenuManager.singleton.definitionHub.OnDefineDetail(_referedDefineDetail);
        }
    }
}