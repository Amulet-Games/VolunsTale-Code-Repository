using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class R_RingReviewSlot : ItemReviewSlot
    {
        [Header("Ring Review Refs.")]
        [ReadOnlyInspector] public RuntimeRing _referingRing;
        
        public void RegisterRightRingReviewSlot(RuntimeRing _runtimeRing)
        {
            _isSlotEmpty = false;
            _referingRing = _runtimeRing;
            DrawItemIcon(_referingRing._referedRingItem.itemIcon);
        }

        public void UnRegisterRightRingReviewSlot()
        {
            _isSlotEmpty = true;
            _referingRing = null;
            DrawDefaultIcon();
        }

        #region Overwrite / Empty SavableInventory
        public void Overwrite_InventoryRightRingSlot(RuntimeRing _runtimeRing)
        {
            if (_isSlotEmpty)
            {
                _inventory.SetupRightRingEmptySlot(_runtimeRing);
            }
            else
            {
                _inventory.SetupRightRingTakenSlot(_runtimeRing);
            }
        }

        public override void EmptyInventorySlot()
        {
            _inventory.EmptyRightRingSlot();
            _itemHub.HideCurrentInfoDetails();
            UnRegisterRightRingReviewSlot();
            Redraw_EmptySlot_InfoDetails();
        }
        #endregion

        #region Redraws.
        protected override void Redraw_LoadedSlot_InfoDetails()
        {
            _itemHub.ShowRingInfoDetails(_referingRing);
        }

        protected override void Redraw_EmptySlot_InfoDetails()
        {
            _itemHub.ShowEmptyRingInfoDetails(true);
        }

        protected override void Redraw_MainHub_TopText()
        {
            _mainHub.quickSlotPositionText.text = quickSlotPositionText;

            if (!_isSlotEmpty)
            {
                _mainHub.currentItemNameText.text = _referingRing.runtimeName;
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
                _itemHub.Set_Empty_Ring_CurPre_AlterDetails();
            }
            else
            {
                _itemHub.Redraw_Pre_Ring_AlterDetails(_referingRing);
            }
        }

        protected override void Set_Post_AlterDetails()
        {
            _itemHub.Set_Ring_CurPost_AlterDetails();
        }

        public override void DrawEquipHubSlots()
        {
            if (_inventory._isRingCarryingFilled)
            {
                Redraw_Pre_AlterDetails();
                Set_Post_AlterDetails();

                _mainHub.ringEquipSlotsDetail.OnEquipDetail_R(_inventory.allRingsPlayerCarrying, this);
            }
            else
            {
                _mainHub.ringEquipSlotsDetail.OnEmptyEquipDetail();
            }
        }
        #endregion
    }
}