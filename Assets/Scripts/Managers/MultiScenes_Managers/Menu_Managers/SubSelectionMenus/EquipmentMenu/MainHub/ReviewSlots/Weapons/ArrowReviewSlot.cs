using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class ArrowReviewSlot : ItemReviewSlot, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Arrow Review Refs.")]
        [ReadOnlyInspector] public RuntimeArrow _referingArrow;
        
        public void RegisterArrowReviewSlot(RuntimeArrow _runtimeArrow)
        {
            _isSlotEmpty = false;
            _referingArrow = _runtimeArrow;
            DrawItemIcon(_referingArrow._referedArrowItem.itemIcon);
        }

        public void UnRegisterArrowReviewSlot()
        {
            _isSlotEmpty = true;
            _referingArrow = null;
            DrawDefaultIcon();
        }
        
        #region Overwrite / Empty SavableInventory
        public void Overwrite_InventoryPowerupSlot(RuntimeArrow _runtimeArrow)
        {
            if (_isSlotEmpty)
            {
                _inventory.SetupArrowEmptySlot(_runtimeArrow);
            }
            else
            {
                _inventory.SetupArrowTakenSlot(_runtimeArrow);
            }

            RegisterArrowReviewSlot(_runtimeArrow);
        }

        public override void EmptyInventorySlot()
        {
            _inventory.EmptyArrowSlot();
            _itemHub.HideCurrentInfoDetails();
            UnRegisterArrowReviewSlot();
            Redraw_EmptySlot_InfoDetails();
        }
        #endregion

        #region Redraws.
        protected override void Redraw_LoadedSlot_InfoDetails()
        {
            _itemHub.ShowArrowInfoDetails(_referingArrow);
        }

        protected override void Redraw_EmptySlot_InfoDetails()
        {
            _itemHub.ShowEmptyArrowInfoDetails(true);
        }

        protected override void Redraw_MainHub_TopText()
        {
            _mainHub.quickSlotPositionText.text = quickSlotPositionText;

            if (!_isSlotEmpty)
            {
                _mainHub.currentItemNameText.text = _referingArrow.runtimeName;
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
                _itemHub.Set_Empty_Arrow_CurPre_AlterDetails();
            }
            else
            {
                _itemHub.Redraw_Pre_Arrow_AlterDetails(_referingArrow);
            }
        }

        protected override void Set_Post_AlterDetails()
        {
            _itemHub.Set_Arrow_CurPost_AlterDetails();
        }

        public override void DrawEquipHubSlots()
        {
            if (_inventory._isArrowCarryingFilled)
            {
                Redraw_Pre_AlterDetails();
                Set_Post_AlterDetails();

                _mainHub.arrowEquipSlotsDetail.OnEquipDetail(_inventory.allArrowsPlayerCarrying, this);
            }
            else
            {
                _mainHub.arrowEquipSlotsDetail.OnEmptyEquipDetail();
            }
        }
        #endregion
    }
}