using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class LegArmorReviewSlot : ItemReviewSlot
    {
        [Header("LegArmor Review Refs.")]
        [ReadOnlyInspector] public RuntimeLegArmor _referingLegArmor;

        public void RegisterLegArmorReviewSlot(RuntimeLegArmor _runtimeLegArmor)
        {
            _isSlotEmpty = false;
            _referingLegArmor = _runtimeLegArmor;
            DrawItemIcon(_referingLegArmor._referedArmorItem.itemIcon);
        }

        public void UnRegisterLegArmorReviewSlot()
        {
            _isSlotEmpty = true;
            _referingLegArmor = null;
            DrawDefaultIcon();
        }

        #region Overwrite / Empty SavableInventory
        public void Overwrite_InventoryLegArmorSlot(RuntimeLegArmor _runtimeLegArmor)
        {
            if (_isSlotEmpty)
            {
                _inventory.SetupLegArmorEmptySlot(_runtimeLegArmor);
            }
            else
            {
                _inventory.SetupLegArmorTakenSlot(_runtimeLegArmor);
            }
        }

        public override void EmptyInventorySlot()
        {
            _inventory.EmptyLegArmorSlot();
            _itemHub.HideCurrentInfoDetails();
            UnRegisterLegArmorReviewSlot();
            Redraw_EmptySlot_InfoDetails();
        }
        #endregion
        
        #region Redraws.
        protected override void Redraw_LoadedSlot_InfoDetails()
        {
            _itemHub.ShowArmorInfoDetails(_referingLegArmor);
        }

        protected override void Redraw_EmptySlot_InfoDetails()
        {
            _itemHub.ShowDeprivedLegInfoDetails();
        }

        protected override void Redraw_MainHub_TopText()
        {
            _mainHub.quickSlotPositionText.text = quickSlotPositionText;

            if (!_isSlotEmpty)
            {
                _mainHub.currentItemNameText.text = _referingLegArmor.runtimeName;
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
                _itemHub.Redraw_Pre_DeprivedLeg_AlterDetails();
            }
            else
            {
                _itemHub.Redraw_Pre_Armor_AlterDetails(_referingLegArmor);
            }
        }

        protected override void Set_Post_AlterDetails()
        {
            _itemHub.Set_Armor_CurPost_AlterDetails();
        }

        public override void DrawEquipHubSlots()
        {
            if (_inventory._isLegCarryingFilled)
            {
                Redraw_Pre_AlterDetails();
                Set_Post_AlterDetails();

                _mainHub.legArmorEquipSlotsDetail.OnEquipDetail(_inventory.allLegsPlayerCarrying, this);
            }
            else
            {
                _mainHub.legArmorEquipSlotsDetail.OnEmptyEquipDetail();
            }
        }
        #endregion
    }
}