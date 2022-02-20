using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ChestArmorEquipSlotsDetail : ItemEquipSlotsDetail
    {
        [Header("Config.")]
        public ChestArmorEquipSlot[] chestEquipSlots;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] ChestArmorEquipSlot _currentChestEquipSlot;

        [Header("Actives.")]
        [ReadOnlyInspector] List<ChestArmorEquipSlot> activeEquipSlots = new List<ChestArmorEquipSlot>();

        [Header("Connecting Slot Refs.")]
        [ReadOnlyInspector] public ChestArmorReviewSlot _chestConnectReviewSlot;

        #region Loaded Tick.
        void GetCurrentSlotByInput()
        {
            if (_inp.menu_right_input)
            {
                detailIndex++;
                detailIndex = (detailIndex == activeEquipSlotsCount) ? 0 : detailIndex;

                SetCurrentChestSlot();
            }
            else if (_inp.menu_left_input)
            {
                detailIndex--;
                detailIndex = (detailIndex < 0) ? activeEquipSlotsCount - 1 : detailIndex;

                SetCurrentChestSlot();
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

                SetCurrentChestSlot();
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

                SetCurrentChestSlot();
            }
        }

        /// Pointer Event.
        public void GetCurrentSlotByPointerEvent(ChestArmorEquipSlot _targetSlot)
        {
            isCursorOverSelection = true;
            if (_currentChestEquipSlot != _targetSlot)
            {
                detailIndex = _targetSlot._slotDetailIndex;
                SetCurrentChestSlot();
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
            OverwriteChestReviewSlot();

            OffLoadedEquipDetail();
            itemHub.OnLoadedEquipDetailQuit();
            equipmentMenuManager.OpenReviewHub();
        }

        void SetCurrentChestSlot()
        {
            _currentChestEquipSlot.OffCurrentSlot();
            _currentChestEquipSlot = activeEquipSlots[detailIndex];
            _currentChestEquipSlot.OnCurrentSlot();
        }

        void OverwriteChestReviewSlot()
        {
            _chestConnectReviewSlot.Overwrite_InventoryChestArmorSlot(_currentChestEquipSlot._referingChestArmor);
        }
        #endregion

        #region On Equip Detail.
        public void OnEquipDetail(List<RuntimeChestArmor> _allChestsPlayerCarrying, ChestArmorReviewSlot _chestArmorReviewSlot)
        {
            LoadActiveSlots(_allChestsPlayerCarrying);
            ConnectChestArmorReviewSlot();

            equipmentMenuManager.OpenEquipHub();

            OnLoadedEquipDetail_Base();
            OnEquipDetail_SetFirstSlotAsCurrent();

            AutoSwitchToCompareDetail();

            void ConnectChestArmorReviewSlot()
            {
                _chestConnectReviewSlot = _chestArmorReviewSlot;
            }

            void AutoSwitchToCompareDetail()
            {
                if (!_chestConnectReviewSlot._isSlotEmpty)
                    itemHub.SwitchIsShowAlterHubStatus();
            }
        }

        void OnEquipDetail_SetFirstSlotAsCurrent()
        {
            detailIndex = 0;
            _currentChestEquipSlot = activeEquipSlots[detailIndex];
            _currentChestEquipSlot.OnCurrentSlot();
        }

        public void OnEmptyEquipDetail()
        {
            _isDetailEmpty = true;

            equipmentMenuManager.OpenEquipHub();
            itemHub.ShowEmptyChestInfoDetails();

            OnEmptyEquipDetail_Base();
        }
        #endregion

        #region Off Loaded Detail.
        protected override void OffLoadedEquipDetail()
        {
            _currentChestEquipSlot.OffEquipSlotDetail();
            Base_OffLoadedEquipDetail();
        }
        #endregion

        #region Redraws.
        public override void RedrawInfoDetail()
        {
            _currentChestEquipSlot.Redraw_EquipSlot_InfoDetails();
        }

        public override void RedrawAlterDetail()
        {
            _currentChestEquipSlot.Redraw_Post_AlterDetails();
        }
        #endregion

        #region Load / UnLoad Active Slots.
        void LoadActiveSlots(List<RuntimeChestArmor> _carryingChestArmor)
        {
            int carryingChestCount = _carryingChestArmor.Count;
            for (int i = 0; i < carryingChestCount; i++)
            {
                chestEquipSlots[i].LoadChestEquipSlot(_carryingChestArmor[i]);
                activeEquipSlots.Add(chestEquipSlots[i]);
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
                    activeEquipSlots[i].UnLoadChestEquipSlot();
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
            SetupChestEquipSlots();
        }

        void SetupChestEquipSlots()
        {
            totalEquipSlotsLength = chestEquipSlots.Length;
            for (int i = 0; i < totalEquipSlotsLength; i++)
            {
                chestEquipSlots[i].Setup(this);
                chestEquipSlots[i]._slotDetailIndex = i;
            }
        }
        #endregion.
    }
}