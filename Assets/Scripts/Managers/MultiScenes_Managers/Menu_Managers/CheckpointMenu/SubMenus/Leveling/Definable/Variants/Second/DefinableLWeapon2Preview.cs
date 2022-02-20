using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DefinableLWeapon2Preview : DefinableStatsPreview
    {
        [Header("Refered Runtime Weapon.")]
        [ReadOnlyInspector] public RuntimeWeapon _lhWeapon_2;
        [ReadOnlyInspector] public double _menuOpened_attPow;

        public override void RedrawIncrementPreview()
        {
            if (_lhWeapon_2)
            {
                _afterChangesText.text = ((int)_lhWeapon_2.ReturnWeaponPreviewAttackPower()).ToString();
                _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
            }
        }

        public override void RedrawDecrementPreview()
        {
            if (_lhWeapon_2)
            {
                double _edited_attPow = _lhWeapon_2.ReturnWeaponPreviewAttackPower();
                _afterChangesText.text = ((int)_edited_attPow).ToString();

                if (_edited_attPow - _menuOpened_attPow < 0.5f)
                    _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
            }
        }

        public override void RedrawConfirmedStatsPreview()
        {
            _lhWeapon_2 = _statsHandler._states._savableInventory.leftHandSlots[1];
            if (_lhWeapon_2)
            {
                _menuOpened_attPow = _lhWeapon_2.ReturnWeaponPreviewAttackPower();
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
