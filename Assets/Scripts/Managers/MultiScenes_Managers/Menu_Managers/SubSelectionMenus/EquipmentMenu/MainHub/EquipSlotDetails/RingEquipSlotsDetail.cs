using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RingEquipSlotsDetail : ItemEquipSlotsDetail
    {
        [Header("Config.")]
        public RingEquipSlot[] ringEquipSlots;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] RingEquipSlot _currentRingEquipSlot;

        [Header("Actives.")]
        [ReadOnlyInspector] List<RingEquipSlot> activeEquipSlots = new List<RingEquipSlot>();

        [Header("Connecting Slot Refs.")]
        [ReadOnlyInspector] public R_RingReviewSlot _r_ringConnectReviewSlot;
        [ReadOnlyInspector] public L_RingReviewSlot _l_ringConnectReviewSlot;
        [ReadOnlyInspector] public bool isRhSlotConnected;

        #region Loaded Tick.
        void GetCurrentSlotByInput()
        {
            if (_inp.menu_right_input)
            {
                detailIndex++;
                detailIndex = (detailIndex == activeEquipSlotsCount) ? 0 : detailIndex;

                SetCurrentRingSlot();
            }
            else if (_inp.menu_left_input)
            {
                detailIndex--;
                detailIndex = (detailIndex < 0) ? activeEquipSlotsCount - 1 : detailIndex;

                SetCurrentRingSlot();
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

                SetCurrentRingSlot();
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

                SetCurrentRingSlot();
            }
        }

        /// Pointer Event.
        public void GetCurrentSlotByPointerEvent(RingEquipSlot _targetSlot)
        {
            isCursorOverSelection = true;
            if (_currentRingEquipSlot != _targetSlot)
            {
                detailIndex = _targetSlot._slotDetailIndex;
                SetCurrentRingSlot();
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
            OverwriteRingReviewSlot();

            OffLoadedEquipDetail();
            itemHub.OnLoadedEquipDetailQuit();
            equipmentMenuManager.OpenReviewHub();
        }

        void SetCurrentRingSlot()
        {
            _currentRingEquipSlot.OffCurrentSlot();
            _currentRingEquipSlot = activeEquipSlots[detailIndex];
            _currentRingEquipSlot.OnCurrentSlot();
        }

        void OverwriteRingReviewSlot()
        {
            if (isRhSlotConnected)
            {
                _r_ringConnectReviewSlot.Overwrite_InventoryRightRingSlot(_currentRingEquipSlot._referingRing);
            }
            else
            {
                _l_ringConnectReviewSlot.Overwrite_InventoryLeftRingSlot(_currentRingEquipSlot._referingRing);
            }
        }
        #endregion

        #region On Equip Detail.
        public void OnEquipDetail_R(List<RuntimeRing> _allRingsPlayerCarrying, R_RingReviewSlot _r_ringReviewSlot)
        {
            LoadActiveSlots(_allRingsPlayerCarrying);
            ConnectR_RingReviewSlot();

            equipmentMenuManager.OpenEquipHub();

            OnLoadedEquipDetail_Base();
            OnEquipDetail_SetFirstSlotAsCurrent();

            AutoSwitchToCompareDetail();

            void ConnectR_RingReviewSlot()
            {
                _r_ringConnectReviewSlot = _r_ringReviewSlot;
                _l_ringConnectReviewSlot = null;
                isRhSlotConnected = true;
            }

            void AutoSwitchToCompareDetail()
            {
                if (!_r_ringConnectReviewSlot._isSlotEmpty)
                    itemHub.SwitchIsShowAlterHubStatus();
            }
        }

        public void OnEquipDetail_L(List<RuntimeRing> _allRingsPlayerCarrying, L_RingReviewSlot _l_ringReviewSlot)
        {
            LoadActiveSlots(_allRingsPlayerCarrying);
            ConnectL_RingReviewSlot();

            equipmentMenuManager.OpenEquipHub();

            OnLoadedEquipDetail_Base();
            OnEquipDetail_SetFirstSlotAsCurrent();

            AutoSwitchToCompareDetail();

            void ConnectL_RingReviewSlot()
            {
                _r_ringConnectReviewSlot = null;
                _l_ringConnectReviewSlot = _l_ringReviewSlot;
                isRhSlotConnected = false;
            }

            void AutoSwitchToCompareDetail()
            {
                if (!_l_ringConnectReviewSlot._isSlotEmpty)
                    itemHub.SwitchIsShowAlterHubStatus();
            }
        }

        void OnEquipDetail_SetFirstSlotAsCurrent()
        {
            detailIndex = 0;
            _currentRingEquipSlot = activeEquipSlots[detailIndex];
            _currentRingEquipSlot.OnCurrentSlot();
        }

        public void OnEmptyEquipDetail()
        {
            _isDetailEmpty = true;

            equipmentMenuManager.OpenEquipHub();
            itemHub.ShowEmptyRingInfoDetails(false);

            OnEmptyEquipDetail_Base();
        }
        #endregion

        #region Off Equip Detail.
        protected override void OffLoadedEquipDetail()
        {
            _currentRingEquipSlot.OffEquipSlotDetail();
            Base_OffLoadedEquipDetail();
        }
        #endregion

        #region Redraws.
        public override void RedrawInfoDetail()
        {
            _currentRingEquipSlot.Redraw_EquipSlot_InfoDetails();
        }

        public override void RedrawAlterDetail()
        {
            _currentRingEquipSlot.Redraw_Post_AlterDetails();
        }
        #endregion

        #region Load / UnLoad Active Slots.
        void LoadActiveSlots(List<RuntimeRing> _carryingRing)
        {
            int carryingRingCount = _carryingRing.Count;
            for (int i = 0; i < carryingRingCount; i++)
            {
                ringEquipSlots[i].LoadRingEquipSlot(_carryingRing[i]);
                activeEquipSlots.Add(ringEquipSlots[i]);
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
                    activeEquipSlots[i].UnLoadRingEquipSlot();
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
            SetupRingEquipSlots();
        }

        void SetupRingEquipSlots()
        {
            totalEquipSlotsLength = ringEquipSlots.Length;
            for (int i = 0; i < totalEquipSlotsLength; i++)
            {
                ringEquipSlots[i].Setup(this);
                ringEquipSlots[i]._slotDetailIndex = i;
            }
        }
        #endregion.
    }
}