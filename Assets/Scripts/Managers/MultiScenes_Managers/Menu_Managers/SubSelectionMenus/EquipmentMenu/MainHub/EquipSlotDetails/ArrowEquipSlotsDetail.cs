using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ArrowEquipSlotsDetail : ItemEquipSlotsDetail
    {
        [Header("Config.")]
        public ArrowEquipSlot[] arrowEquipSlots;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] ArrowEquipSlot _currentArrowEquipSlot;

        [Header("Actives.")]
        [ReadOnlyInspector] List<ArrowEquipSlot> activeEquipSlots = new List<ArrowEquipSlot>();

        [Header("Connecting Slot Refs.")]
        [ReadOnlyInspector] public ArrowReviewSlot _arrowConnectReviewSlot;

        #region Loaded Tick.
        void GetCurrentSlotByInput()
        {
            if (_inp.menu_right_input)
            {
                detailIndex++;
                detailIndex = (detailIndex == activeEquipSlotsCount) ? 0 : detailIndex;

                SetCurrentArrowSlot();
            }
            else if (_inp.menu_left_input)
            {
                detailIndex--;
                detailIndex = (detailIndex < 0) ? activeEquipSlotsCount - 1 : detailIndex;

                SetCurrentArrowSlot();
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

                SetCurrentArrowSlot();
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

                SetCurrentArrowSlot();
            }
        }

        /// Pointer Event.
        public void GetCurrentSlotByPointerEvent(ArrowEquipSlot _targetSlot)
        {
            isCursorOverSelection = true;
            if (_currentArrowEquipSlot != _targetSlot)
            {
                detailIndex = _targetSlot._slotDetailIndex;
                SetCurrentArrowSlot();
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
            OverwriteArrowReviewSlot();

            OffLoadedEquipDetail();
            itemHub.OnLoadedEquipDetailQuit();
            equipmentMenuManager.OpenReviewHub();
        }

        void SetCurrentArrowSlot()
        {
            _currentArrowEquipSlot.OffCurrentSlot();
            _currentArrowEquipSlot = activeEquipSlots[detailIndex];
            _currentArrowEquipSlot.OnCurrentSlot();
        }

        void OverwriteArrowReviewSlot()
        {
            //_arrowConnectReviewSlot.Overwrite_InventoryArrowSlot(_currentArrowEquipSlot._referingArrow);
        }
        #endregion

        #region On Equip Detail.
        public void OnEquipDetail(List<RuntimeArrow> _allArrowsPlayerCarrying, ArrowReviewSlot _arrowReviewSlot)
        {
            LoadActiveSlots(_allArrowsPlayerCarrying);
            ConnectArrowReviewSlot();

            equipmentMenuManager.OpenEquipHub();

            OnLoadedEquipDetail_Base();
            OnEquipDetail_SetFirstSlotAsCurrent();

            AutoSwitchToCompareDetail();

            void ConnectArrowReviewSlot()
            {
                _arrowConnectReviewSlot = _arrowReviewSlot;
            }

            void AutoSwitchToCompareDetail()
            {
                if (!_arrowConnectReviewSlot._isSlotEmpty)
                    itemHub.SwitchIsShowAlterHubStatus();
            }
        }

        void OnEquipDetail_SetFirstSlotAsCurrent()
        {
            detailIndex = 0;
            _currentArrowEquipSlot = activeEquipSlots[detailIndex];
            _currentArrowEquipSlot.OnCurrentSlot();
        }

        public void OnEmptyEquipDetail()
        {
            _isDetailEmpty = true;

            equipmentMenuManager.OpenEquipHub();
            itemHub.ShowEmptyArrowInfoDetails(false);

            OnEmptyEquipDetail_Base();
        }
        #endregion

        #region Off Loaded Detail.
        protected override void OffLoadedEquipDetail()
        {
            _currentArrowEquipSlot.OffEquipSlotDetail();
            Base_OffLoadedEquipDetail();
        }
        #endregion

        #region Redraws.
        public override void RedrawInfoDetail()
        {
            _currentArrowEquipSlot.Redraw_EquipSlot_InfoDetails();
        }

        public override void RedrawAlterDetail()
        {
            _currentArrowEquipSlot.Redraw_Post_AlterDetails();
        }
        #endregion

        #region Load / UnLoad Active Slots.
        void LoadActiveSlots(List<RuntimeArrow> _carryingArrow)
        {
            int carryingCharmCount = _carryingArrow.Count;
            for (int i = 0; i < carryingCharmCount; i++)
            {
                arrowEquipSlots[i].LoadArrowEquipSlot(_carryingArrow[i]);
                activeEquipSlots.Add(arrowEquipSlots[i]);
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
                    activeEquipSlots[i].UnLoadArrowEquipSlot();
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
            SetupArrowEquipSlots();
        }

        void SetupArrowEquipSlots()
        {
            totalEquipSlotsLength = arrowEquipSlots.Length;
            for (int i = 0; i < totalEquipSlotsLength; i++)
            {
                arrowEquipSlots[i].Setup(this);
                arrowEquipSlots[i]._slotDetailIndex = i;
            }
        }
        #endregion.
    }
}