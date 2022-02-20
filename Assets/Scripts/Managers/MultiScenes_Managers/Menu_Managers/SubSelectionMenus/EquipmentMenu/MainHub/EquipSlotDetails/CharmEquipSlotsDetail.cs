using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class CharmEquipSlotsDetail : ItemEquipSlotsDetail
    {
        [Header("Config.")]
        public CharmEquipSlot[] charmEquipSlots;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] CharmEquipSlot _currentCharmEquipSlot;

        [Header("Actives.")]
        [ReadOnlyInspector] List<CharmEquipSlot> activeEquipSlots = new List<CharmEquipSlot>();

        [Header("Connecting Slot Refs.")]
        [ReadOnlyInspector] public CharmReviewSlot _charmConnectReviewSlot;

        #region Loaded Tick.
        void GetCurrentSlotByInput()
        {
            if (_inp.menu_right_input)
            {
                detailIndex++;
                detailIndex = (detailIndex == activeEquipSlotsCount) ? 0 : detailIndex;

                SetCurrentCharmSlot();
            }
            else if (_inp.menu_left_input)
            {
                detailIndex--;
                detailIndex = (detailIndex < 0) ? activeEquipSlotsCount - 1 : detailIndex;

                SetCurrentCharmSlot();
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

                SetCurrentCharmSlot();
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

                SetCurrentCharmSlot();
            }
        }

        /// Pointer Event.
        public void GetCurrentSlotByPointerEvent(CharmEquipSlot _targetSlot)
        {
            isCursorOverSelection = true;
            if (_currentCharmEquipSlot != _targetSlot)
            {
                detailIndex = _targetSlot._slotDetailIndex;
                SetCurrentCharmSlot();
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
            OverwriteCharmReviewSlot();

            OffLoadedEquipDetail();
            itemHub.OnLoadedEquipDetailQuit();
            equipmentMenuManager.OpenReviewHub();
        }

        void SetCurrentCharmSlot()
        {
            _currentCharmEquipSlot.OffCurrentSlot();
            _currentCharmEquipSlot = activeEquipSlots[detailIndex];
            _currentCharmEquipSlot.OnCurrentSlot();
        }

        void OverwriteCharmReviewSlot()
        {
            _charmConnectReviewSlot.Overwrite_InventoryCharmSlot(_currentCharmEquipSlot._referingCharm);
        }
        #endregion

        #region On Equip Detail.
        public void OnEquipDetail(List<RuntimeCharm> _allCharmsPlayerCarrying, CharmReviewSlot _charmReviewSlot)
        {
            LoadActiveSlots(_allCharmsPlayerCarrying);
            ConnectCharmReviewSlot();

            equipmentMenuManager.OpenEquipHub();

            OnLoadedEquipDetail_Base();
            OnEquipDetail_SetFirstSlotAsCurrent();

            AutoSwitchToCompareDetail();

            void ConnectCharmReviewSlot()
            {
                _charmConnectReviewSlot = _charmReviewSlot;
            }

            void AutoSwitchToCompareDetail()
            {
                if (!_charmConnectReviewSlot._isSlotEmpty)
                    itemHub.SwitchIsShowAlterHubStatus();
            }
        }

        void OnEquipDetail_SetFirstSlotAsCurrent()
        {
            detailIndex = 0;
            _currentCharmEquipSlot = activeEquipSlots[detailIndex];
            _currentCharmEquipSlot.OnCurrentSlot();
        }

        public void OnEmptyEquipDetail()
        {
            _isDetailEmpty = true;

            equipmentMenuManager.OpenEquipHub();
            itemHub.ShowEmptyCharmInfoDetails(false);

            OnEmptyEquipDetail_Base();
        }
        #endregion

        #region Off Loaded Detail.
        protected override void OffLoadedEquipDetail()
        {
            _currentCharmEquipSlot.OffEquipSlotDetail();
            Base_OffLoadedEquipDetail();
        }
        #endregion

        #region Redraws.
        public override void RedrawInfoDetail()
        {
            _currentCharmEquipSlot.Redraw_EquipSlot_InfoDetails();
        }

        public override void RedrawAlterDetail()
        {
            _currentCharmEquipSlot.Redraw_Post_AlterDetails();
        }
        #endregion

        #region Load / UnLoad Active Slots.
        void LoadActiveSlots(List<RuntimeCharm> _carryingCharm)
        {
            int carryingCharmCount = _carryingCharm.Count;
            for (int i = 0; i < carryingCharmCount; i++)
            {
                charmEquipSlots[i].LoadCharmEquipSlot(_carryingCharm[i]);
                activeEquipSlots.Add(charmEquipSlots[i]);
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
                    activeEquipSlots[i].UnLoadCharmEquipSlot();
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
            SetupCharmEquipSlots();
        }

        void SetupCharmEquipSlots()
        {
            totalEquipSlotsLength = charmEquipSlots.Length;
            for (int i = 0; i < totalEquipSlotsLength; i++)
            {
                charmEquipSlots[i].Setup(this);
                charmEquipSlots[i]._slotDetailIndex = i;
            }
        }
        #endregion.
    }
}