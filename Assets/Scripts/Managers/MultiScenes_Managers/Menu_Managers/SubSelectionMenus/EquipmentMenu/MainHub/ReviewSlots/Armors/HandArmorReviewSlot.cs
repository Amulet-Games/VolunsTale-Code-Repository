using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class HandArmorReviewSlot : ItemReviewSlot
    {
        [Header("HandArmor Review Refs")]
        [ReadOnlyInspector] public RuntimeHandArmor _referingHandArmor;

        public void RegisterHandArmorReviewSlot(RuntimeHandArmor _runtimeHandArmor)
        {
            _isSlotEmpty = false;
            _referingHandArmor = _runtimeHandArmor;
            DrawItemIcon(_referingHandArmor._referedArmorItem.itemIcon);
        }

        public void UnRegisterHandArmorReviewSlot()
        {
            _isSlotEmpty = true;
            _referingHandArmor = null;
            DrawDefaultIcon();
        }

        #region Overwrite / Empty SavableInventory
        public void Overwrite_InventoryHandArmorSlot(RuntimeHandArmor _runtimeHandArmor)
        {
            if (_isSlotEmpty)
            {
                _inventory.SetupHandArmorEmptySlot(_runtimeHandArmor);
            }
            else
            {
                _inventory.SetupHandArmorTakenSlot(_runtimeHandArmor);
            }
        }

        public override void EmptyInventorySlot()
        {
            _inventory.EmptyHandArmorSlot();
            _itemHub.HideCurrentInfoDetails();
            UnRegisterHandArmorReviewSlot();
            Redraw_EmptySlot_InfoDetails();
        }
        #endregion
        
        #region Redraws.
        protected override void Redraw_LoadedSlot_InfoDetails()
        {
            _itemHub.ShowArmorInfoDetails(_referingHandArmor);
        }

        protected override void Redraw_EmptySlot_InfoDetails()
        {
            _itemHub.ShowDeprivedHandInfoDetails();
        }

        protected override void Redraw_MainHub_TopText()
        {
            _mainHub.quickSlotPositionText.text = quickSlotPositionText;

            if (!_isSlotEmpty)
            {
                _mainHub.currentItemNameText.text = _referingHandArmor.runtimeName;
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
                _itemHub.Redraw_Pre_DeprivedHand_AlterDetails();
            }
            else
            {
                _itemHub.Redraw_Pre_Armor_AlterDetails(_referingHandArmor);
            }
        }

        protected override void Set_Post_AlterDetails()
        {
            _itemHub.Set_Armor_CurPost_AlterDetails();
        }

        public override void DrawEquipHubSlots()
        {
            if (_inventory._isHandCarryingFilled)
            {
                Redraw_Pre_AlterDetails();
                Set_Post_AlterDetails();

                _mainHub.handArmorEquipSlotsDetail.OnEquipDetail(_inventory.allHandsPlayerCarrying, this);
            }
            else
            {
                _mainHub.handArmorEquipSlotsDetail.OnEmptyEquipDetail();
            }
        }
        #endregion
    }
}