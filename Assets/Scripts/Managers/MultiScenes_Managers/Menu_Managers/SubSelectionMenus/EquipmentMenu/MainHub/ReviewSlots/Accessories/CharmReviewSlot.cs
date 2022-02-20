using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class CharmReviewSlot : ItemReviewSlot
    {
        [Header("Ring Review Refs.")]
        [ReadOnlyInspector] public RuntimeCharm _referingCharm;

        public void RegisterCharmReviewSlot(RuntimeCharm _runtimeCharm)
        {
            _isSlotEmpty = false;
            _referingCharm = _runtimeCharm;
            DrawItemIcon(_referingCharm._referedCharmItem.itemIcon);
        }

        public void UnRegisterCharmReviewSlot()
        {
            _isSlotEmpty = true;
            _referingCharm = null;
            DrawDefaultIcon();
        }

        #region Overwrite / Empty SavableInventory
        public void Overwrite_InventoryCharmSlot(RuntimeCharm _runtimeCharm)
        {
            if (_isSlotEmpty)
            {
                _inventory.SetupCharmEmptySlot(_runtimeCharm);
            }
            else
            {
                _inventory.SetupCharmTakenSlot(_runtimeCharm);
            }

            RegisterCharmReviewSlot(_runtimeCharm);
        }

        public override void EmptyInventorySlot()
        {
            _inventory.EmptyCharmSlot();
            _itemHub.HideCurrentInfoDetails();
            UnRegisterCharmReviewSlot();
            Redraw_EmptySlot_InfoDetails();
        }
        #endregion
        
        #region Redraws.
        protected override void Redraw_LoadedSlot_InfoDetails()
        {
            _itemHub.ShowCharmInfoDetails(_referingCharm);
        }

        protected override void Redraw_EmptySlot_InfoDetails()
        {
            _itemHub.ShowEmptyCharmInfoDetails(true);
        }

        protected override void Redraw_MainHub_TopText()
        {
            _mainHub.quickSlotPositionText.text = quickSlotPositionText;

            if (!_isSlotEmpty)
            {
                _mainHub.currentItemNameText.text = _referingCharm.runtimeName;
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
                _itemHub.Set_Empty_Charm_CurPre_AlterDetails();
            }
            else
            {
                _itemHub.Redraw_Pre_Charm_AlterDetails(_referingCharm);
            }
        }

        protected override void Set_Post_AlterDetails()
        {
            _itemHub.Set_Charm_CurPost_AlterDetails();
        }

        public override void DrawEquipHubSlots()
        {
            if (_inventory._isCharmCarryingFilled)
            {
                Redraw_Pre_AlterDetails();
                Set_Post_AlterDetails();

                _mainHub.charmEquipSlotsDetail.OnEquipDetail(_inventory.allCharmsPlayerCarrying, this);
            }
            else
            {
                _mainHub.charmEquipSlotsDetail.OnEmptyEquipDetail();
            }
        }
        #endregion
    }
}