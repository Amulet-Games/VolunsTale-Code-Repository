using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class HeadArmorEquipSlotsDetail : ItemEquipSlotsDetail
    {
        [Header("Config.")]
        public HeadArmorEquipSlot[] headEquipSlots;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] HeadArmorEquipSlot _currentHeadEquipSlot;

        [Header("Actives.")]
        [ReadOnlyInspector] List<HeadArmorEquipSlot> activeEquipSlots = new List<HeadArmorEquipSlot>();

        [Header("Connecting Slot Refs.")]
        [ReadOnlyInspector] public HeadArmorReviewSlot _headConnectReviewSlot;

        #region Loaded Tick.
        void GetCurrentSlotByInput()
        {
            if (_inp.menu_right_input)
            {
                detailIndex++;
                detailIndex = (detailIndex == activeEquipSlotsCount) ? 0 : detailIndex;

                SetCurrentHeadSlot();
            }
            else if (_inp.menu_left_input)
            {
                detailIndex--;
                detailIndex = (detailIndex < 0) ? activeEquipSlotsCount - 1 : detailIndex;

                SetCurrentHeadSlot();
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

                SetCurrentHeadSlot();
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

                SetCurrentHeadSlot();
            }
        }

        /// Pointer Event.
        public void GetCurrentSlotByPointerEvent(HeadArmorEquipSlot _targetSlot)
        {
            isCursorOverSelection = true;
            if (_currentHeadEquipSlot != _targetSlot)
            {
                detailIndex = _targetSlot._slotDetailIndex;
                SetCurrentHeadSlot();
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
            OverwriteHeadReviewSlot();

            OffLoadedEquipDetail();
            itemHub.OnLoadedEquipDetailQuit();
            equipmentMenuManager.OpenReviewHub();
        }

        void SetCurrentHeadSlot()
        {
            _currentHeadEquipSlot.OffCurrentSlot();
            _currentHeadEquipSlot = activeEquipSlots[detailIndex];
            _currentHeadEquipSlot.OnCurrentSlot();
        }

        void OverwriteHeadReviewSlot()
        {
            _headConnectReviewSlot.Overwrite_InventoryHeadArmorSlot(_currentHeadEquipSlot._referingHeadArmor);
        }
        #endregion

        #region On Equip Detail.
        public void OnEquipDetail(List<RuntimeHeadArmor> _allHeadsPlayerCarrying, HeadArmorReviewSlot _headArmorReviewSlot)
        {
            LoadActiveSlots(_allHeadsPlayerCarrying);
            ConnectHeadArmorReviewSlot();

            equipmentMenuManager.OpenEquipHub();

            OnLoadedEquipDetail_Base();
            OnEquipDetail_SetFirstSlotAsCurrent();

            AutoSwitchToCompareDetail();

            void ConnectHeadArmorReviewSlot()
            {
                _headConnectReviewSlot = _headArmorReviewSlot;
            }

            void AutoSwitchToCompareDetail()
            {
                if (!_headConnectReviewSlot._isSlotEmpty)
                    itemHub.SwitchIsShowAlterHubStatus();
            }
        }

        void OnEquipDetail_SetFirstSlotAsCurrent()
        {
            detailIndex = 0;
            _currentHeadEquipSlot = activeEquipSlots[detailIndex];
            _currentHeadEquipSlot.OnCurrentSlot();
        }

        public void OnEmptyEquipDetail()
        {
            _isDetailEmpty = true;

            equipmentMenuManager.OpenEquipHub();
            itemHub.ShowEmptyHeadInfoDetails();

            OnEmptyEquipDetail_Base();
        }
        #endregion

        #region Off Equip Detail.
        protected override void OffLoadedEquipDetail()
        {
            _currentHeadEquipSlot.OffEquipSlotDetail();
            Base_OffLoadedEquipDetail();
        }
        #endregion

        #region Redraws.
        public override void RedrawInfoDetail()
        {
            _currentHeadEquipSlot.Redraw_EquipSlot_InfoDetails();
        }

        public override void RedrawAlterDetail()
        {
            _currentHeadEquipSlot.Redraw_Post_AlterDetails();
        }
        #endregion

        #region Load / UnLoad Active Slots.
        void LoadActiveSlots(List<RuntimeHeadArmor> _carryingHeadArmor)
        {
            int carryingHeadCount = _carryingHeadArmor.Count;
            for (int i = 0; i < carryingHeadCount; i++)
            {
                headEquipSlots[i].LoadHeadEquipSlot(_carryingHeadArmor[i]);
                activeEquipSlots.Add(headEquipSlots[i]);
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
                    activeEquipSlots[i].UnLoadHeadEquipSlot();
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
            SetupHeadEquipSlots();
        }

        void SetupHeadEquipSlots()
        {
            totalEquipSlotsLength = headEquipSlots.Length;
            for (int i = 0; i < totalEquipSlotsLength; i++)
            {
                headEquipSlots[i].Setup(this);
                headEquipSlots[i]._slotDetailIndex = i;
            }
        }
        #endregion.
    }
}