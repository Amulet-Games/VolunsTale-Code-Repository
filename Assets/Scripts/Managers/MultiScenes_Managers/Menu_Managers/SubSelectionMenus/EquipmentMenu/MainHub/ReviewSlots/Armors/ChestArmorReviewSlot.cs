using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ChestArmorReviewSlot : ItemReviewSlot
    {
        [Header("ChestArmor Review Refs.")]
        [ReadOnlyInspector] public RuntimeChestArmor _referingChestArmor;

        public void RegisterChestArmorReviewSlot(RuntimeChestArmor _runtimeChestArmor)
        {
            _isSlotEmpty = false;
            _referingChestArmor = _runtimeChestArmor;
            DrawItemIcon(_referingChestArmor._referedArmorItem.itemIcon);
        }

        public void UnRegisterChestArmorReviewSlot()
        {
            _isSlotEmpty = true;
            _referingChestArmor = null;
            DrawDefaultIcon();
        }

        #region Overwrite / Empty SavableInventory
        public void Overwrite_InventoryChestArmorSlot(RuntimeChestArmor _runtimeChestArmor)
        {
            if (_isSlotEmpty)
            {
                _inventory.SetupChestArmorEmptySlot(_runtimeChestArmor);
            }
            else
            {
                _inventory.SetupChestArmorTakenSlot(_runtimeChestArmor);
            }
        }

        public override void EmptyInventorySlot()
        {
            _inventory.EmptyChestArmorSlot();
            _itemHub.HideCurrentInfoDetails();
            UnRegisterChestArmorReviewSlot();
            Redraw_EmptySlot_InfoDetails();
        }
        #endregion
        
        #region Redraws.
        protected override void Redraw_LoadedSlot_InfoDetails()
        {
            _itemHub.ShowArmorInfoDetails(_referingChestArmor);
        }

        protected override void Redraw_EmptySlot_InfoDetails()
        {
            _itemHub.ShowDeprivedChestInfoDetails();
        }

        protected override void Redraw_MainHub_TopText()
        {
            _mainHub.quickSlotPositionText.text = quickSlotPositionText;

            if (!_isSlotEmpty)
            {
                _mainHub.currentItemNameText.text = _referingChestArmor.runtimeName;
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
                _itemHub.Redraw_Pre_DeprivedChest_AlterDetails();
            }
            else
            {
                _itemHub.Redraw_Pre_Armor_AlterDetails(_referingChestArmor);
            }
        }

        protected override void Set_Post_AlterDetails()
        {
            _itemHub.Set_Armor_CurPost_AlterDetails();
        }

        public override void DrawEquipHubSlots()
        {
            if (_inventory._isChestCarryingFilled)
            {
                Redraw_Pre_AlterDetails();
                Set_Post_AlterDetails();

                _mainHub.chestArmorEquipSlotsDetail.OnEquipDetail(_inventory.allChestsPlayerCarrying, this);
            }
            else
            {
                _mainHub.chestArmorEquipSlotsDetail.OnEmptyEquipDetail();
            }
        }
        #endregion
    }
}