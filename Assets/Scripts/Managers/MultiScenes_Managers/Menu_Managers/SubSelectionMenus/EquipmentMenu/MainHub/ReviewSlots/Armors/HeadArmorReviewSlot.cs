using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class HeadArmorReviewSlot : ItemReviewSlot
    {
        [Header("HeadArmor Review Refs.")]
        [ReadOnlyInspector] public RuntimeHeadArmor _referingHeadArmor;

        public void RegisterHeadArmorReviewSlot(RuntimeHeadArmor _runtimeHeadArmor)
        {
            _isSlotEmpty = false;
            _referingHeadArmor = _runtimeHeadArmor;
            DrawItemIcon(_referingHeadArmor._referedArmorItem.itemIcon);
        }

        public void UnRegisterHeadArmorReviewSlot()
        {
            _isSlotEmpty = true;
            _referingHeadArmor = null;
            DrawDefaultIcon();
        }

        #region Overwrite / Empty SavableInventory
        public void Overwrite_InventoryHeadArmorSlot(RuntimeHeadArmor _runtimeHeadArmor)
        {
            if (_isSlotEmpty)
            {
                _inventory.SetupHeadArmorEmptySlot(_runtimeHeadArmor);
            }
            else
            {
                _inventory.SetupHeadArmorTakenSlot(_runtimeHeadArmor);
            }
        }

        public override void EmptyInventorySlot()
        {
            _inventory.EmptyHeadArmorSlot();
            _itemHub.HideCurrentInfoDetails();
            UnRegisterHeadArmorReviewSlot();
            Redraw_EmptySlot_InfoDetails();
        }
        #endregion
        
        #region Redraws.
        protected override void Redraw_LoadedSlot_InfoDetails()
        {
            _itemHub.ShowArmorInfoDetails(_referingHeadArmor);
        }

        protected override void Redraw_EmptySlot_InfoDetails()
        {
            _itemHub.ShowDeprivedHeadInfoDetails();
        }

        protected override void Redraw_MainHub_TopText()
        {
            _mainHub.quickSlotPositionText.text = quickSlotPositionText;

            if (!_isSlotEmpty)
            {
                _mainHub.currentItemNameText.text = _referingHeadArmor.runtimeName;
            }
            else
            {
                _mainHub.currentItemNameText.text = "";
            }
        }

        protected override void Redraw_Pre_AlterDetails()
        {
            if (_isSlotEmpty)
            {
                _itemHub.Redraw_Pre_DeprivedHead_AlterDetails();
            }
            else
            {
                _itemHub.Redraw_Pre_Armor_AlterDetails(_referingHeadArmor);
            }
        }

        protected override void Set_Post_AlterDetails()
        {
            _itemHub.Set_Armor_CurPost_AlterDetails();
        }

        public override void DrawEquipHubSlots()
        {
            if (_inventory._isHeadCarryingFilled)
            {
                Redraw_Pre_AlterDetails();
                Set_Post_AlterDetails();

                _mainHub.headArmorEquipSlotsDetail.OnEquipDetail(_inventory.allHeadsPlayerCarrying, this);
            }
            else
            {
                _mainHub.headArmorEquipSlotsDetail.OnEmptyEquipDetail();
            }
        }
        #endregion
    }
}