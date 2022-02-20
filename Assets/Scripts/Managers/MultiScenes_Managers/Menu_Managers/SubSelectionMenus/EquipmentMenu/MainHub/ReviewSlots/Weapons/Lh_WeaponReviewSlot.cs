using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Lh_WeaponReviewSlot : ItemReviewSlot
    {
        [Header("Weapon Review Config.")]
        public int lhSlotNumber;

        [Header("Weapon Review Refs.")]
        [ReadOnlyInspector] public RuntimeWeapon _referingLhWeapon;

        [Header("Status Hub.")]
        [ReadOnlyInspector] public StatusHub _statusHub;

        public void RegisterWeaponReviewSlot(RuntimeWeapon _runtimeWeapon)
        {
            _isSlotEmpty = false;
            _referingLhWeapon = _runtimeWeapon;
            DrawItemIcon(_referingLhWeapon._referedWeaponItem.itemIcon);
        }

        public void UnRegisterWeaponReviewSlot()
        {
            _isSlotEmpty = true;
            _referingLhWeapon = null;
            DrawDefaultIcon();
        }

        #region Overwrite Inventory Slot.
        public void Overwrite_InventoryLhWeaponSlot(RuntimeWeapon _runtimeWeapon)
        {
            _inventory.OffTwoHandingInMenuBeforeSetupSlot();

            if (!_isSlotEmpty)
            {
                if (_referingLhWeapon == _inventory._twoHandingWeapon)
                {
                    _inventory.Setup_TH_LhWeaponTakenSlot(_runtimeWeapon, lhSlotNumber);
                }
                else
                {
                    _inventory.SetupLhWeaponTakenSlot(_runtimeWeapon, lhSlotNumber);
                }
            }
            else
            {
                _inventory.SetupLhWeaponEmptySlot(_runtimeWeapon, lhSlotNumber);
            }

            /// Status Hub Weapon Text Refresh (after inventory is set!)
            _statusHub.Refresh_Lh_WeaponAttacksTextStyle();
        }
        #endregion

        #region Empty Inventory Slot.
        public override void EmptyInventorySlot()
        {
            if (_inventory._states._isWaitForWeaponSwitch)
            {
                WeaponSwitchWait_NotAllowedEmptySlot();
            }
            else
            {
                AllowedEmptySlot();
            }
        }

        void WeaponSwitchWait_NotAllowedEmptySlot()
        {

        }

        void AllowedEmptySlot()
        {
            _inventory.InMenu_EmptyLhSlot(lhSlotNumber);

            _itemHub.HideCurrentInfoDetails();
            UnRegisterWeaponReviewSlot();
            Redraw_EmptySlot_InfoDetails();

            /// Status Hub Weapon Text Refresh (after inventory is set!)
            _statusHub.Refresh_Lh_WeaponAttacksTextStyle();
        }
        #endregion
        
        #region Redraws.
        protected override void Redraw_LoadedSlot_InfoDetails()
        {
            _itemHub.ShowWeaponInfoDetails(_referingLhWeapon);
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
                _mainHub.currentItemNameText.text = _referingLhWeapon.runtimeName;
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
                _itemHub.Redraw_Pre_Weapon_AlterDetails(_referingLhWeapon);
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

                _mainHub.weaponEquipSlotsDetail.OnEquipDetail_Lh(_inventory.allWeaponsPlayerCarrying, this);
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
