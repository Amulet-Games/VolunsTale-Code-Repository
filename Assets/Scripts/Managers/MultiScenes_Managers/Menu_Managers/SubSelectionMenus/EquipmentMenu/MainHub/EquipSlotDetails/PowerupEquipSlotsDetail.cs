using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PowerupEquipSlotsDetail : ItemEquipSlotsDetail
    {
        [Header("Config.")]
        public PowerupEquipSlot[] powerupEquipSlots;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] PowerupEquipSlot _currentPowerupEquipSlot;

        [Header("Actives.")]
        [ReadOnlyInspector] List<PowerupEquipSlot> activeEquipSlots = new List<PowerupEquipSlot>();

        [Header("Connecting Slot Refs.")]
        [ReadOnlyInspector] public PowerupReviewSlot _powerupConnectReviewSlot;

        #region Loaded Tick.
        void GetCurrentSlotByInput()
        {
            if (_inp.menu_right_input)
            {
                detailIndex++;
                detailIndex = (detailIndex == activeEquipSlotsCount) ? 0 : detailIndex;

                SetCurrentPowerupSlot();
            }
            else if (_inp.menu_left_input)
            {
                detailIndex--;
                detailIndex = (detailIndex < 0) ? activeEquipSlotsCount - 1 : detailIndex;

                SetCurrentPowerupSlot();
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

                SetCurrentPowerupSlot();
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

                SetCurrentPowerupSlot();
            }
        }

        /// Pointer Event.
        public void GetCurrentSlotByPointerEvent(PowerupEquipSlot _targetSlot)
        {
            isCursorOverSelection = true;
            if (_currentPowerupEquipSlot != _targetSlot)
            {
                detailIndex = _targetSlot._slotDetailIndex;
                SetCurrentPowerupSlot();
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
            OverwritePowerupReviewSlot();

            OffLoadedEquipDetail();
            itemHub.OnLoadedEquipDetailQuit();
            equipmentMenuManager.OpenReviewHub();
        }

        void SetCurrentPowerupSlot()
        {
            _currentPowerupEquipSlot.OffCurrentSlot();
            _currentPowerupEquipSlot = activeEquipSlots[detailIndex];
            _currentPowerupEquipSlot.OnCurrentSlot();
        }

        void OverwritePowerupReviewSlot()
        {
            _powerupConnectReviewSlot.Overwrite_InventoryPowerupSlot(_currentPowerupEquipSlot._referingPowerup);
        }
        #endregion

        #region On Equip Detail.
        public void OnEquipDetail(List<RuntimePowerup> _allPowerupsPlayerCarrying, PowerupReviewSlot _powerupReviewSlot)
        {
            LoadActiveSlots(_allPowerupsPlayerCarrying);
            ConnectPowerupReviewSlot();
            
            equipmentMenuManager.OpenEquipHub();

            OnLoadedEquipDetail_Base();
            OnEquipDetail_SetFirstSlotAsCurrent();

            AutoSwitchToCompareDetail();

            void ConnectPowerupReviewSlot()
            {
                _powerupConnectReviewSlot = _powerupReviewSlot;
            }

            void AutoSwitchToCompareDetail()
            {
                if (!_powerupConnectReviewSlot._isSlotEmpty)
                    itemHub.SwitchIsShowAlterHubStatus();
            }
        }

        void OnEquipDetail_SetFirstSlotAsCurrent()
        {
            detailIndex = 0;
            _currentPowerupEquipSlot = activeEquipSlots[detailIndex];
            _currentPowerupEquipSlot.OnCurrentSlot();
        }

        public void OnEmptyEquipDetail()
        {
            _isDetailEmpty = true;

            equipmentMenuManager.OpenEquipHub();
            itemHub.ShowEmptyPowerupInfoDetails(false);

            OnEmptyEquipDetail_Base();
        }
        #endregion

        #region Off Equip Detail.
        protected override void OffLoadedEquipDetail()
        {
            _currentPowerupEquipSlot.OffEquipSlotDetail();
            Base_OffLoadedEquipDetail();
        }
        #endregion

        #region On / Off Empty Info / Description.
        public override void RedrawInfoDetail()
        {
            _currentPowerupEquipSlot.Redraw_EquipSlot_InfoDetails();
        }
        
        public override void RedrawAlterDetail()
        {
            _currentPowerupEquipSlot.Redraw_Post_AlterDetails();
        }
        #endregion

        #region Load / UnLoad Active Slots.
        void LoadActiveSlots(List<RuntimePowerup> _carryingPowerup)
        {
            int carryingPowerupCount = _carryingPowerup.Count;
            for (int i = 0; i < carryingPowerupCount; i++)
            {
                powerupEquipSlots[i].LoadPowerupEquipSlot(_carryingPowerup[i]);
                activeEquipSlots.Add(powerupEquipSlots[i]);
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
                    activeEquipSlots[i].UnLoadPowerupEquipSlot();
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
            SetupPowerupEquipSlots();
        }

        void SetupPowerupEquipSlots()
        {
            totalEquipSlotsLength = powerupEquipSlots.Length;
            for (int i = 0; i < totalEquipSlotsLength; i++)
            {
                powerupEquipSlots[i].Setup(this);
                powerupEquipSlots[i]._slotDetailIndex = i;
            }
        }
        #endregion.
    }
}