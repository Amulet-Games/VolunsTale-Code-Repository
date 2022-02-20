using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class LegArmorEquipSlotsDetail : ItemEquipSlotsDetail
    {
        [Header("Config.")]
        public LegArmorEquipSlot[] legEquipSlots;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] LegArmorEquipSlot _currentLegEquipSlot;

        [Header("Actives.")]
        [ReadOnlyInspector] List<LegArmorEquipSlot> activeEquipSlots = new List<LegArmorEquipSlot>();

        [Header("Connecting Slot Refs.")]
        [ReadOnlyInspector] public LegArmorReviewSlot _legConnectReviewSlot;

        #region Loaded Tick.
        void GetCurrentSlotByInput()
        {
            if (_inp.menu_right_input)
            {
                detailIndex++;
                detailIndex = (detailIndex == activeEquipSlotsCount) ? 0 : detailIndex;

                SetCurrentLegSlot();
            }
            else if (_inp.menu_left_input)
            {
                detailIndex--;
                detailIndex = (detailIndex < 0) ? activeEquipSlotsCount - 1 : detailIndex;

                SetCurrentLegSlot();
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

                SetCurrentLegSlot();
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

                SetCurrentLegSlot();
            }
        }

        /// Pointer Event.
        public void GetCurrentSlotByPointerEvent(LegArmorEquipSlot _targetSlot)
        {
            isCursorOverSelection = true;
            if (_currentLegEquipSlot != _targetSlot)
            {
                detailIndex = _targetSlot._slotDetailIndex;
                SetCurrentLegSlot();
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
            OverwriteLegReviewSlot();

            OffLoadedEquipDetail();
            itemHub.OnLoadedEquipDetailQuit();
            equipmentMenuManager.OpenReviewHub();
        }

        void SetCurrentLegSlot()
        {
            _currentLegEquipSlot.OffCurrentSlot();
            _currentLegEquipSlot = activeEquipSlots[detailIndex];
            _currentLegEquipSlot.OnCurrentSlot();
        }

        void OverwriteLegReviewSlot()
        {
            _legConnectReviewSlot.Overwrite_InventoryLegArmorSlot(_currentLegEquipSlot._referingLegArmor);
        }
        #endregion

        #region On Loaded Detail.
        public void OnEquipDetail(List<RuntimeLegArmor> _allLegsPlayerCarrying, LegArmorReviewSlot _legArmorReviewSlot)
        {
            LoadActiveSlots(_allLegsPlayerCarrying);
            ConnectLegArmorReviewSlot();

            equipmentMenuManager.OpenEquipHub();

            OnLoadedEquipDetail_Base();
            OnEquipDetail_SetFirstSlotAsCurrent();

            AutoSwitchToCompareDetail();

            void ConnectLegArmorReviewSlot()
            {
                _legConnectReviewSlot = _legArmorReviewSlot;
            }

            void AutoSwitchToCompareDetail()
            {
                if (!_legConnectReviewSlot._isSlotEmpty)
                    itemHub.SwitchIsShowAlterHubStatus();
            }
        }

        void OnEquipDetail_SetFirstSlotAsCurrent()
        {
            detailIndex = 0;
            _currentLegEquipSlot = activeEquipSlots[detailIndex];
            _currentLegEquipSlot.OnCurrentSlot();
        }

        public void OnEmptyEquipDetail()
        {
            _isDetailEmpty = true;

            equipmentMenuManager.OpenEquipHub();
            itemHub.ShowEmptyLegInfoDetails();

            OnEmptyEquipDetail_Base();
        }
        #endregion

        #region Off Loaded Detail.
        protected override void OffLoadedEquipDetail()
        {
            _currentLegEquipSlot.OffEquipSlotDetail();
            Base_OffLoadedEquipDetail();
        }
        #endregion

        #region Redraws.
        public override void RedrawInfoDetail()
        {
            _currentLegEquipSlot.Redraw_EquipSlot_InfoDetails();
        }

        public override void RedrawAlterDetail()
        {
            _currentLegEquipSlot.Redraw_Post_AlterDetails();
        }
        #endregion

        #region On / Off Empty Info / Description.
        void LoadActiveSlots(List<RuntimeLegArmor> _carryingLegArmor)
        {
            int carryingLegCount = _carryingLegArmor.Count;
            for (int i = 0; i < carryingLegCount; i++)
            {
                legEquipSlots[i].LoadLegEquipSlot(_carryingLegArmor[i]);
                activeEquipSlots.Add(legEquipSlots[i]);
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
                    activeEquipSlots[i].UnLoadLegEquipSlot();
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
            SetupLegEquipSlots();
        }

        void SetupLegEquipSlots()
        {
            totalEquipSlotsLength = legEquipSlots.Length;
            for (int i = 0; i < totalEquipSlotsLength; i++)
            {
                legEquipSlots[i].Setup(this);
                legEquipSlots[i]._slotDetailIndex = i;
            }
        }
        #endregion.
    }
}