using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Rh_WeaponReviewSlot : ItemReviewSlot
    {
        [Header("Weapon Review Config.")]
        public int rhSlotNumber;

        [Header("Weapon Review Refs.")]
        [ReadOnlyInspector] public RuntimeWeapon _referingRhWeapon;

        [Header("Status Hub.")]
        [ReadOnlyInspector] public StatusHub _statusHub;

        public void RegisterWeaponReviewSlot(RuntimeWeapon _runtimeWeapon)
        {
            _isSlotEmpty = false;
            _referingRhWeapon = _runtimeWeapon;
            DrawItemIcon(_referingRhWeapon._referedWeaponItem.itemIcon);
        }

        public void UnRegisterWeaponReviewSlot()
        {
            _isSlotEmpty = true;
            _referingRhWeapon = null;
            DrawDefaultIcon();
        }
        
        #region Overwrite Inventory Slot.
        public void Overwrite_InventoryRhWeaponSlot(RuntimeWeapon _runtimeWeapon)
        {
            _inventory.OffTwoHandingInMenuBeforeSetupSlot();

            /// If player want to overwrite the current RH Two Handing Weapon.
            if (!_isSlotEmpty)
            {
                if (_referingRhWeapon == _inventory._twoHandingWeapon)
                {
                    _inventory.Setup_TH_RhWeaponTakenSlot(_runtimeWeapon, rhSlotNumber);
                }
                else
                {
                    _inventory.SetupRhWeaponTakenSlot(_runtimeWeapon, rhSlotNumber);
                }
            }
            else
            {
                _inventory.SetupRhWeaponEmptySlot(_runtimeWeapon, rhSlotNumber);
            }

            /// Status Hub Weapon Text Refresh (after inventory is set!)
            _statusHub.Refresh_Rh_WeaponAttacksTextStyle();
        }
        #endregion

        #region Empty Inventory Slot.
        public override void EmptyInventorySlot()
        {
            if (!_inventory._states._isWaitForWeaponSwitch)
            {
                AllowedEmptySlot();
            }
        }
        
        void AllowedEmptySlot()
        {
            _inventory.InMenu_EmptyRhSlot(rhSlotNumber);

            _itemHub.HideCurrentInfoDetails();
            UnRegisterWeaponReviewSlot();
            Redraw_EmptySlot_InfoDetails();

            /// Status Hub Weapon Text Refresh (after inventory is set!)
            _statusHub.Refresh_Rh_WeaponAttacksTextStyle();
        }
        #endregion
        
        #region Redraws.
        protected override void Redraw_LoadedSlot_InfoDetails()
        {
            _itemHub.ShowWeaponInfoDetails(_referingRhWeapon);
        }

        protected override void Redraw_EmptySlot_InfoDetails()
        {
            _itemHub.ShowFistInfoDetails();
        }

        protected override void Redraw_MainHub_TopText()
        {
            _mainHub.quickSlotPositionText.text = quickSlotPositionText;

            if (!_isSlotEmpty)
            {
                _mainHub.currentItemNameText.text = _referingRhWeapon.runtimeName;
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
                _itemHub.Redraw_Pre_Fist_AlterDetails();
            }
            else
            {
                _itemHub.Redraw_Pre_Weapon_AlterDetails(_referingRhWeapon);
            }
        }

        protected override void Set_Post_AlterDetails()
        {
            _itemHub.Set_Weapon_CurPost_AlterDetails();
        }

        public override void DrawEquipHubSlots()
        {
            if (_inventory._isWeaponCarryingFilled)
            {
                Redraw_Pre_AlterDetails();
                Set_Post_AlterDetails();

                _mainHub.weaponEquipSlotsDetail.OnEquipDetail_Rh(_inventory.allWeaponsPlayerCarrying, this);
            }
            else
            {
                _mainHub.weaponEquipSlotsDetail.OnEmptyEquipDetail();
            }   
        }
        #endregion

        #region Setup.
        protected override void GetBaseReferences()
        {
            base.GetBaseReferences();
            _statusHub = reviewSlotDetail.equipmentMenuManager.statusHub;
        }
        #endregion
    }
}