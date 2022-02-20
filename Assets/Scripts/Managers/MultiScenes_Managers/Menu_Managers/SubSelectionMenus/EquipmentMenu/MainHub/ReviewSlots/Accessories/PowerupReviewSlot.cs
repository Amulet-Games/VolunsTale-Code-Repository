using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PowerupReviewSlot : ItemReviewSlot
    {
        [Header("Powerup Review Refs.")]
        [ReadOnlyInspector] public RuntimePowerup _referingPowerup;

        public void RegisterPowerupReviewSlot(RuntimePowerup _runtimePowerup)
        {
            _isSlotEmpty = false;
            _referingPowerup = _runtimePowerup;
            DrawItemIcon(_referingPowerup._referedPowerupItem.itemIcon);
        }

        public void UnRegisterPowerupReviewSlot()
        {
            _isSlotEmpty = true;
            _referingPowerup = null;
            DrawDefaultIcon();
        }
        
        #region Overwrite / Empty SavableInventory
        public void Overwrite_InventoryPowerupSlot(RuntimePowerup _runtimePowerup)
        {
            if (_isSlotEmpty)
            {
                _inventory.SetupPowerupEmptySlot(_runtimePowerup);
            }
            else
            {
                _inventory.SetupPowerupTakenSlot(_runtimePowerup);
            }
        }

        public override void EmptyInventorySlot()
        {
            _inventory.EmptyPowerupSlot();
            _itemHub.HideCurrentInfoDetails();
            UnRegisterPowerupReviewSlot();
            Redraw_EmptySlot_InfoDetails();
        }
        #endregion
        
        #region Redraws.
        protected override void Redraw_LoadedSlot_InfoDetails()
        {
            _itemHub.ShowPowerupInfoDetails(_referingPowerup);
        }

        protected override void Redraw_EmptySlot_InfoDetails()
        {
            _itemHub.ShowEmptyPowerupInfoDetails(true);
        }

        protected override void Redraw_MainHub_TopText()
        {
            _mainHub.quickSlotPositionText.text = quickSlotPositionText;

            if (!_isSlotEmpty)
            {
                _mainHub.currentItemNameText.text = _referingPowerup.runtimeName;
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
                _itemHub.Set_Empty_Powerup_CurPre_AlterDetails();
            }
            else
            {
                _itemHub.Redraw_Pre_Powerup_AlterDetails(_referingPowerup);
            }
        }

        protected override void Set_Post_AlterDetails()
        {
            _itemHub.Set_Powerup_CurPost_AlterDetails();
        }

        public override void DrawEquipHubSlots()
        {
            if (_inventory._isPowerupCarryingFilled)
            {
                Redraw_Pre_AlterDetails();
                Set_Post_AlterDetails();

                _mainHub.powerupEquipSlotsDetail.OnEquipDetail(_inventory.allPowerupsPlayerCarrying, this);
            }
            else
            {
                _mainHub.powerupEquipSlotsDetail.OnEmptyEquipDetail();
            }
        }
        #endregion
    }
}