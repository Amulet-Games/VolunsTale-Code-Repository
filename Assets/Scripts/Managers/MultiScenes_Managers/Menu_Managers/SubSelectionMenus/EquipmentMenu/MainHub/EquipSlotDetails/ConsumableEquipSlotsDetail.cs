using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ConsumableEquipSlotsDetail : ItemEquipSlotsDetail
    {
        [Header("Config.")]
        public ConsumableEquipSlot[] consumableEquipSlots;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] ConsumableEquipSlot _currentConsumableEquipSlot;

        [Header("Actives.")]
        [ReadOnlyInspector] List<ConsumableEquipSlot> activeEquipSlots = new List<ConsumableEquipSlot>();
        [ReadOnlyInspector] List<RuntimeConsumable> emptyRemovedConsumableList = new List<RuntimeConsumable>();

        [Header("Connecting Slot Refs.")]
        [ReadOnlyInspector] public ConsumableReviewSlot _consumableConnectReviewSlot;

        #region Loaded Tick.
        void GetCurrentSlotByInput()
        {
            if (_inp.menu_right_input)
            {
                detailIndex++;
                detailIndex = (detailIndex == activeEquipSlotsCount) ? 0 : detailIndex;

                SetCurrentConsumableSlot();
            }
            else if (_inp.menu_left_input)
            {
                detailIndex--;
                detailIndex = (detailIndex < 0) ? activeEquipSlotsCount - 1 : detailIndex;

                SetCurrentConsumableSlot();
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

                SetCurrentConsumableSlot();
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

                SetCurrentConsumableSlot();
            }
        }

        /// Pointer Event.
        public void GetCurrentSlotByPointerEvent(ConsumableEquipSlot _targetSlot)
        {
            isCursorOverSelection = true;
            if (_currentConsumableEquipSlot != _targetSlot)
            {
                detailIndex = _targetSlot._slotDetailIndex;
                SetCurrentConsumableSlot();
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
            OverwriteConsumableReviewSlot();

            OffLoadedEquipDetail();
            itemHub.OnLoadedEquipDetailQuit();
            equipmentMenuManager.OpenReviewHub();
        }

        void SetCurrentConsumableSlot()
        {
            _currentConsumableEquipSlot.OffCurrentSlot();
            _currentConsumableEquipSlot = activeEquipSlots[detailIndex];
            _currentConsumableEquipSlot.OnCurrentSlot();
        }

        void OverwriteConsumableReviewSlot()
        {
            _consumableConnectReviewSlot.Overwrite_InventoryConsumableSlot(_currentConsumableEquipSlot._referingConsumable);
        }
        #endregion

        #region On Equip Detail.
        public void OnEquipDetail(List<RuntimeConsumable> _allConsumablesPlayerCarrying, ConsumableReviewSlot _consumableReviewSlot)
        {
            GetEmptyRemovedConsumableList(_allConsumablesPlayerCarrying);
            LoadActiveSlots(emptyRemovedConsumableList);
            ConnectConsumableReviewSlot();

            equipmentMenuManager.OpenEquipHub();

            OnLoadedEquipDetail_Base();
            OnEquipDetail_SetFirstSlotAsCurrent();

            AutoSwitchToCompareDetail();

            void ConnectConsumableReviewSlot()
            {
                _consumableConnectReviewSlot = _consumableReviewSlot;
            }

            void AutoSwitchToCompareDetail()
            {
                if (!_consumableConnectReviewSlot._isSlotEmpty)
                    itemHub.SwitchIsShowAlterHubStatus();
            }
        }

        void GetEmptyRemovedConsumableList(List<RuntimeConsumable> allConsumablesPlayerCarrying)
        {
            emptyRemovedConsumableList.Clear();

            for (int i = 0; i < allConsumablesPlayerCarrying.Count; i++)
            {
                if (!allConsumablesPlayerCarrying[i].isCarryingEmpty)
                {
                    emptyRemovedConsumableList.Add(allConsumablesPlayerCarrying[i]);
                }
                else
                {
                    if (allConsumablesPlayerCarrying[i]._baseConsumableItem.GetIsVessel())
                    {
                        emptyRemovedConsumableList.Add(allConsumablesPlayerCarrying[i]);
                    }
                }
            }
        }

        void OnEquipDetail_SetFirstSlotAsCurrent()
        {
            detailIndex = 0;
            _currentConsumableEquipSlot = activeEquipSlots[detailIndex];
            _currentConsumableEquipSlot.OnCurrentSlot();
        }
        
        public void OnEmptyEquipDetail()
        {
            _isDetailEmpty = true;

            equipmentMenuManager.OpenEquipHub();
            itemHub.ShowEmptyConsumableInfoDetails(false);

            OnEmptyEquipDetail_Base();
        }
        #endregion

        #region Off Loaded Detail.
        protected override void OffLoadedEquipDetail()
        {
            _currentConsumableEquipSlot.OffEquipSlotDetail();
            Base_OffLoadedEquipDetail();
        }
        #endregion

        #region Redraws.
        public override void RedrawInfoDetail()
        {
            _currentConsumableEquipSlot.Redraw_EquipSlot_InfoDetails();
        }

        public override void RedrawAlterDetail()
        {
            _currentConsumableEquipSlot.Redraw_Post_AlterDetails();
        }
        #endregion

        #region Load / UnLoad Active Slots.
        void LoadActiveSlots(List<RuntimeConsumable> _carryingConsumable)
        {
            int carryingConsumableCount = _carryingConsumable.Count;
            for (int i = 0; i < carryingConsumableCount; i++)
            {
                consumableEquipSlots[i].LoadConsumableEquipSlot(_carryingConsumable[i]);
                activeEquipSlots.Add(consumableEquipSlots[i]);
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
                    activeEquipSlots[i].UnLoadConsumableEquipSlot();
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
            SetupConsumableEquipSlots();
        }

        void SetupConsumableEquipSlots()
        {
            totalEquipSlotsLength = consumableEquipSlots.Length;
            for (int i = 0; i < totalEquipSlotsLength; i++)
            {
                consumableEquipSlots[i].Setup(this);
                consumableEquipSlots[i]._slotDetailIndex = i;
            }
        }
        #endregion.
    }
}