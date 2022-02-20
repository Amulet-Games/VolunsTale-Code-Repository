using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class HandArmorEquipSlotsDetail : ItemEquipSlotsDetail
    {
        [Header("Config.")]
        public HandArmorEquipSlot[] handEquipSlots;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] HandArmorEquipSlot _currentHandEquipSlot;

        [Header("Actives.")]
        [ReadOnlyInspector] List<HandArmorEquipSlot> activeEquipSlots = new List<HandArmorEquipSlot>();

        [Header("Connecting Slot Refs.")]
        [ReadOnlyInspector] public HandArmorReviewSlot _handConnectReviewSlot;

        #region Loaded Tick.
        void GetCurrentSlotByInput()
        {
            if (_inp.menu_right_input)
            {
                detailIndex++;
                detailIndex = (detailIndex == activeEquipSlotsCount) ? 0 : detailIndex;

                SetCurrentHandSlot();
            }
            else if (_inp.menu_left_input)
            {
                detailIndex--;
                detailIndex = (detailIndex < 0) ? activeEquipSlotsCount - 1 : detailIndex;

                SetCurrentHandSlot();
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

                SetCurrentHandSlot();
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

                SetCurrentHandSlot();
            }
        }

        /// Pointer Event.
        public void GetCurrentSlotByPointerEvent(HandArmorEquipSlot _targetSlot)
        {
            isCursorOverSelection = true;
            if (_currentHandEquipSlot != _targetSlot)
            {
                detailIndex = _targetSlot._slotDetailIndex;
                SetCurrentHandSlot();
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
            OverwriteHandReviewSlot();

            OffLoadedEquipDetail();
            itemHub.OnLoadedEquipDetailQuit();
            equipmentMenuManager.OpenReviewHub();
        }

        void SetCurrentHandSlot()
        {
            _currentHandEquipSlot.OffCurrentSlot();
            _currentHandEquipSlot = activeEquipSlots[detailIndex];
            _currentHandEquipSlot.OnCurrentSlot();
        }

        void OverwriteHandReviewSlot()
        {
            _handConnectReviewSlot.Overwrite_InventoryHandArmorSlot(_currentHandEquipSlot._referingHandArmor);
        }
        #endregion

        #region On Equip Detail.
        public void OnEquipDetail(List<RuntimeHandArmor> _allHandsPlayerCarrying, HandArmorReviewSlot _handArmorReviewSlot)
        {
            LoadActiveSlots(_allHandsPlayerCarrying);
            ConnectHandArmorReviewSlot();

            equipmentMenuManager.OpenEquipHub();

            OnLoadedEquipDetail_Base();
            OnEquipDetail_SetFirstSlotAsCurrent();

            AutoSwitchToCompareDetail();

            void ConnectHandArmorReviewSlot()
            {
                _handConnectReviewSlot = _handArmorReviewSlot;
            }

            void AutoSwitchToCompareDetail()
            {
                if (!_handConnectReviewSlot._isSlotEmpty)
                    itemHub.SwitchIsShowAlterHubStatus();
            }
        }

        void OnEquipDetail_SetFirstSlotAsCurrent()
        {
            detailIndex = 0;
            _currentHandEquipSlot = activeEquipSlots[detailIndex];
            _currentHandEquipSlot.OnCurrentSlot();
        }

        public void OnEmptyEquipDetail()
        {
            _isDetailEmpty = true;

            equipmentMenuManager.OpenEquipHub();
            itemHub.ShowEmptyHandInfoDetails();

            OnEmptyEquipDetail_Base();
        }
        #endregion

        #region Off Equip Detail.
        protected override void OffLoadedEquipDetail()
        {
            _currentHandEquipSlot.OffEquipSlotDetail();
            Base_OffLoadedEquipDetail();
        }
        #endregion

        #region Redraws.
        public override void RedrawInfoDetail()
        {
            _currentHandEquipSlot.Redraw_EquipSlot_InfoDetails();
        }

        public override void RedrawAlterDetail()
        {
            _currentHandEquipSlot.Redraw_Post_AlterDetails();
        }
        #endregion

        #region Load / UnLoad Equip Slots.
        void LoadActiveSlots(List<RuntimeHandArmor> _carryingHandArmor)
        {
            int carryingHandCount = _carryingHandArmor.Count;
            for (int i = 0; i < carryingHandCount; i++)
            {
                handEquipSlots[i].LoadHandEquipSlot(_carryingHandArmor[i]);
                activeEquipSlots.Add(handEquipSlots[i]);
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
                    activeEquipSlots[i].UnLoadHandEquipSlot();
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
            SetupHandEquipSlots();
        }

        void SetupHandEquipSlots()
        {
            totalEquipSlotsLength = handEquipSlots.Length;
            for (int i = 0; i < totalEquipSlotsLength; i++)
            {
                handEquipSlots[i].Setup(this);
                handEquipSlots[i]._slotDetailIndex = i;
            }
        }
        #endregion.
    }
}
