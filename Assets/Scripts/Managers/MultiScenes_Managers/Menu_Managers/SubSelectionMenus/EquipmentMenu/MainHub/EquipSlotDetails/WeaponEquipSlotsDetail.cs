using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public class WeaponEquipSlotsDetail : ItemEquipSlotsDetail
    {
        [Header("Config.")]
        public WeaponEquipSlot[] weaponEquipSlots;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] WeaponEquipSlot _currentWeaponEquipSlot;

        [Header("Actives.")]
        [ReadOnlyInspector] List<WeaponEquipSlot> activeEquipSlots = new List<WeaponEquipSlot>();

        [Header("Connecting Slot Refs.")]
        [ReadOnlyInspector] public Rh_WeaponReviewSlot rhConnectReviewSlot;
        [ReadOnlyInspector] public Lh_WeaponReviewSlot lhConnectReviewSlot;
        [ReadOnlyInspector] public bool isRhSlotConnected;

        #region Loaded Tick.
        void GetCurrentSlotByInput()
        {
            if (_inp.menu_right_input)
            {
                detailIndex++;
                detailIndex = (detailIndex == activeEquipSlotsCount) ? 0 : detailIndex;

                SetCurrentWeaponSlot();
            }
            else if (_inp.menu_left_input)
            {
                detailIndex--;
                detailIndex = (detailIndex < 0) ? activeEquipSlotsCount - 1 : detailIndex;

                SetCurrentWeaponSlot();
            }
            else if (_inp.menu_up_input)
            {
                detailIndex -= 5;
                if (detailIndex < 0)
                {
                    double column = Math.Truncate((activeEquipSlotsCount - 1) / 5f);
                    column = column == 0 ? 1 : column;
                    detailIndex = (int)(detailIndex + (column * 5));
                    detailIndex = (detailIndex > activeEquipSlotsCount - 1) ? detailIndex - 5 : detailIndex;
                }

                SetCurrentWeaponSlot();
            }
            else if (_inp.menu_down_input)
            {
                detailIndex += 5;
                if (detailIndex > activeEquipSlotsCount - 1)
                {
                    double column = Math.Truncate(detailIndex / 5f);
                    column = column == 0 ? 1 : column;
                    detailIndex = (int)(detailIndex - (column * 5));
                }

                SetCurrentWeaponSlot();
            }
        }
        
        /// Pointer Event.
        public void GetCurrentSlotByPointerEvent(WeaponEquipSlot _targetSlot)
        {
            isCursorOverSelection = true;
            if (_currentWeaponEquipSlot != _targetSlot)
            {
                detailIndex = _targetSlot._slotDetailIndex;
                SetCurrentWeaponSlot();
            }
        }

        protected override void LoadedEquipSlotsDetailTick()
        {
            GetCurrentSlotByInput();

            ChangeItemHubContentByInput();

            QuitLoadedEquipSlotDetailByInput();

            SelectCurrentSlotByInput();

            SelectCurrentSlotByCursor();

            equipmentMenuManager.EquipSlot_HighlighterTick();
        }

        protected override void SelectCurrentSlot()
        {
            OverwriteWeaponReviewSlot();

            OffLoadedEquipDetail();
            itemHub.OnLoadedEquipDetailQuit();
            equipmentMenuManager.OpenReviewHub();
        }

        void SetCurrentWeaponSlot()
        {
            _currentWeaponEquipSlot.OffCurrentSlot();
            _currentWeaponEquipSlot = activeEquipSlots[detailIndex];
            _currentWeaponEquipSlot.OnCurrentSlot();
        }

        void OverwriteWeaponReviewSlot()
        {
            if (isRhSlotConnected)
            {
                rhConnectReviewSlot.Overwrite_InventoryRhWeaponSlot(_currentWeaponEquipSlot._referingWeapon);
            }
            else
            {
                lhConnectReviewSlot.Overwrite_InventoryLhWeaponSlot(_currentWeaponEquipSlot._referingWeapon);
            }
        }
        #endregion

        #region On Equip Detail.
        public void OnEquipDetail_Rh(List<RuntimeWeapon> _allWeaponsPlayerCarrying, Rh_WeaponReviewSlot _rh_weaponReviewSlot)
        {
            LoadActiveSlots(_allWeaponsPlayerCarrying);
            ConnectRhWeaponReviewSlot();

            equipmentMenuManager.OpenEquipHub();

            OnLoadedEquipDetail_Base();
            OnEquipDetail_SetFirstSlotAsCurrent();

            AutoSwitchToCompareDetail();

            void ConnectRhWeaponReviewSlot()
            {
                rhConnectReviewSlot = _rh_weaponReviewSlot;
                lhConnectReviewSlot = null;
                isRhSlotConnected = true;
            }

            void AutoSwitchToCompareDetail()
            {
                if (!rhConnectReviewSlot._isSlotEmpty)
                    itemHub.SwitchIsShowAlterHubStatus();
            }
        }

        public void OnEquipDetail_Lh(List<RuntimeWeapon> _allWeaponsPlayerCarrying, Lh_WeaponReviewSlot _lh_weaponReviewSlot)
        {
            LoadActiveSlots(_allWeaponsPlayerCarrying);
            ConnectLhWeaponReviewSlot();

            equipmentMenuManager.OpenEquipHub();

            OnLoadedEquipDetail_Base();
            OnEquipDetail_SetFirstSlotAsCurrent();

            AutoSwitchToCompareDetail();
            
            void ConnectLhWeaponReviewSlot()
            {
                rhConnectReviewSlot = null;
                lhConnectReviewSlot = _lh_weaponReviewSlot;
                isRhSlotConnected = false;
            }
            
            void AutoSwitchToCompareDetail()
            {
                if (!lhConnectReviewSlot._isSlotEmpty)
                    itemHub.SwitchIsShowAlterHubStatus();
            }
        }
        
        void OnEquipDetail_SetFirstSlotAsCurrent()
        {
            detailIndex = 0;
            _currentWeaponEquipSlot = activeEquipSlots[detailIndex];
            _currentWeaponEquipSlot.OnCurrentSlot();
        }

        public void OnEmptyEquipDetail()
        {
            _isDetailEmpty = true;

            equipmentMenuManager.OpenEquipHub();
            itemHub.ShowEmptyWeaponInfoDetails();

            OnEmptyEquipDetail_Base();
        }
        #endregion

        #region Off Equip Detail.
        protected override void OffLoadedEquipDetail()
        {
            _currentWeaponEquipSlot.OffEquipSlotDetail();
            Base_OffLoadedEquipDetail();
        }
        #endregion

        #region Redraws.
        public override void RedrawInfoDetail()
        {
            _currentWeaponEquipSlot.Redraw_EquipSlot_InfoDetails();
        }

        public override void RedrawAlterDetail()
        {
            _currentWeaponEquipSlot.Redraw_Post_AlterDetails();
        }
        #endregion

        #region Load / UnLoad Active Slots.
        void LoadActiveSlots(List<RuntimeWeapon> _carryingWeapons)
        {
            int carryingWeaponCount = _carryingWeapons.Count;
            for (int i = 0; i < carryingWeaponCount; i++)
            {
                weaponEquipSlots[i].LoadWeaponEquipSlot(_carryingWeapons[i]);
                activeEquipSlots.Add(weaponEquipSlots[i]);
                activeEquipSlotsCount++;
            }
        }

        protected override void UnLoadActiveSlots()
        {
            int activeSlotCount = activeEquipSlotsCount;
            if (activeSlotCount > 0)
            {
                for (int i = 0; i < activeSlotCount; i++)
                {
                    activeEquipSlots[i].UnLoadWeaponEquipSlot();
                    activeEquipSlotsCount--;
                }

                activeEquipSlots.Clear();
            }
        }
        #endregion
        
        #region Setup.
        public void Setup(EquipmentMenuManager _equipmentMenuManager)
        {
            equipmentMenuManager = _equipmentMenuManager;
            
            base_Setup();
            SetupWeaponEquipSlots();
        }
        
        void SetupWeaponEquipSlots()
        {
            totalEquipSlotsLength = weaponEquipSlots.Length;
            for (int i = 0; i < totalEquipSlotsLength; i++)
            {
                weaponEquipSlots[i].Setup(this);
                weaponEquipSlots[i]._slotDetailIndex = i;
            }
        }
        #endregion
    }
}