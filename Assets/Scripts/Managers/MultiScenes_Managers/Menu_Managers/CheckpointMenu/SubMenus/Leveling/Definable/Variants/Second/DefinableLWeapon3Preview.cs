using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DefinableLWeapon3Preview : DefinableStatsPreview
    {
        [Header("Refered Runtime Weapon.")]
        [ReadOnlyInspector] public RuntimeWeapon _lhWeapon_3;
        [ReadOnlyInspector] public double _menuOpened_attPow;

        public override void RedrawIncrementPreview()
        {
            if (_lhWeapon_3)
            {
                _afterChangesText.text = ((int)_lhWeapon_3.ReturnWeaponPreviewAttackPower()).ToString();
                _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
            }
        }

        public override void RedrawDecrementPreview()
        {
            if (_lhWeapon_3)
            {
                double _edited_attPow = _lhWeapon_3.ReturnWeaponPreviewAttackPower();
                _afterChangesText.text = ((int)_edited_attPow).ToString();

                if (_edited_attPow - _menuOpened_attPow < 0.5f)
                    _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
            }
        }

        public override void RedrawConfirmedStatsPreview()
        {
            _lhWeapon_3 = _statsHandler._states._savableInventory.leftHandSlots[2];
            if (_lhWeapon_3)
            {
                _menuOpened_attPow = _lhWeapon_3.ReturnWeaponPreviewAttackPower();
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
