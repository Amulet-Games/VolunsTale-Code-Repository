using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ConsumableReviewSlot : ItemReviewSlot
    {
        [Header("Consumable Review Config.")]
        public int consumableSlotNumber;

        [Header("Consumable Review Refs.")]
        [ReadOnlyInspector] public RuntimeConsumable _referingConsumable;

        public void RegisterConsumableReviewSlot(RuntimeConsumable _runtimeConsumable)
        {
            _isSlotEmpty = false;
            _referingConsumable = _runtimeConsumable;

            if (_runtimeConsumable.isCarryingEmpty)
            {
                DrawItemIcon(_referingConsumable._baseConsumableItem.GetEmptyConsumableSprite());
            }
            else
            {
                DrawItemIcon(_referingConsumable._baseConsumableItem.itemIcon);
            }

        }

        public void UnRegisterConsumableReviewSlot()
        {
            _isSlotEmpty = true;
            _referingConsumable = null;
            DrawDefaultIcon();
        }

        #region Overwrite / Empty SavableInventory
        public void Overwrite_InventoryConsumableSlot(RuntimeConsumable _runtimeConsumable)
        {
            if (_isSlotEmpty)
            {
                _inventory.SetupConsumableEmptySlot(_runtimeConsumable, consumableSlotNumber);
            }
            else
            {
                _inventory.SetupConsumableTakenSlot(_runtimeConsumable, consumableSlotNumber);
            }

            RegisterConsumableReviewSlot(_runtimeConsumable);
        }

        public override void EmptyInventorySlot()
        {
            _inventory.EmptyConsumableSlotInMenu(consumableSlotNumber);
            _itemHub.HideCurrentInfoDetails();
            UnRegisterConsumableReviewSlot();
            Redraw_EmptySlot_InfoDetails();
        }
        #endregion
        
        #region Redraws.
        protected override void Redraw_LoadedSlot_InfoDetails()
        {
            _itemHub.ShowConsumableInfoDetails(_referingConsumable);
        }

        protected override void Redraw_EmptySlot_InfoDetails()
        {
            _itemHub.ShowEmptyConsumableInfoDetails(true);
        }

        protected override void Redraw_MainHub_TopText()
        {
            _mainHub.quickSlotPositionText.text = quickSlotPositionText;

            if (!_isSlotEmpty)
            {
                _mainHub.currentItemNameText.text = _referingConsumable.runtimeName;
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
                _itemHub.Set_Empty_Consumable_CurPre_AlterDetails();
            }
            else
            {
                _itemHub.Redraw_Pre_Consumable_AlterDetails(_referingConsumable);
            }
        }

        protected override void Set_Post_AlterDetails()
        {
            _itemHub.Set_StatsEffect_CurPost_AlterDetails();
        }

        public override void DrawEquipHubSlots()
        {
            Redraw_Pre_AlterDetails();
            Set_Post_AlterDetails();

            _mainHub.consumableEquipSlotsDetail.OnEquipDetail(_inventory.allConsumablesPlayerCarrying, this);
        }
        #endregion
    }
}