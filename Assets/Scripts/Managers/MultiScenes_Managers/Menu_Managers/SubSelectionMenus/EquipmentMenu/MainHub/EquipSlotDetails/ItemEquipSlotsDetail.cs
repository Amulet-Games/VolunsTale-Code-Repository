using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public abstract class ItemEquipSlotsDetail : MonoBehaviour
    {
        #region Refs.
        [Header("Canvas (Drops).")]
        [SerializeField] protected Canvas detailCanvas;

        [Header("Manager Refs.")]
        [ReadOnlyInspector] public EquipmentMenuManager equipmentMenuManager;
        [ReadOnlyInspector] public ItemHub itemHub;
        [ReadOnlyInspector] protected InputManager _inp;
        #endregion
        
        #region Status.
        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] protected int detailIndex;
        [ReadOnlyInspector, SerializeField] protected int totalEquipSlotsLength = 0;
        [ReadOnlyInspector, SerializeField] protected int activeEquipSlotsCount = 0;
        [ReadOnlyInspector] public bool _isDetailEmpty;
        [ReadOnlyInspector] public bool isCursorOverSelection;
        #endregion

        #region Empty Tick.
        protected void EmptyEquipSlotsDetailTick()
        {
            QuitEmptyEquipSlotDetailByInput();
        }

        void QuitEmptyEquipSlotDetailByInput()
        {
            if (_inp.menu_quit_input)
            {
                OffEmptyEquipDetail();
                itemHub.OnEmptyEquipDetailQuit();
                equipmentMenuManager.OpenReviewHub();
            }
        }
        #endregion

        #region Loaded Tick.
        protected abstract void LoadedEquipSlotsDetailTick();
        
        protected void ChangeItemHubContentByInput()
        {
            if (_inp.menu_remove_input)
            {
                itemHub.SwitchIsShowAlterHubStatus();
            }
        }

        protected void QuitLoadedEquipSlotDetailByInput()
        {
            if (_inp.menu_quit_input)
            {
                OffLoadedEquipDetail();
                itemHub.OnLoadedEquipDetailQuit();
                equipmentMenuManager.OpenReviewHub();
            }
        }
        #endregion

        #region Tick.
        public void Tick()
        {
            if (_isDetailEmpty)
            {
                EmptyEquipSlotsDetailTick();
            }
            else
            {
                LoadedEquipSlotsDetailTick();
            }
        }

        protected abstract void SelectCurrentSlot();
        
        protected void SelectCurrentSlotByInput()
        {
            if (_inp.menu_select_input)
            {
                SelectCurrentSlot();
            }
        }

        protected void SelectCurrentSlotByCursor()
        {
            if (_inp.menu_select_mouse)
            {
                if (isCursorOverSelection)
                {
                    SelectCurrentSlot();
                }
            }
        }
        #endregion

        #region Off Empty Detail.
        protected void OffEmptyEquipDetail()
        {
            HideEquipDetail();
            _isDetailEmpty = false;
        }
        #endregion

        #region On Loaded Equip Detail
        protected void OnLoadedEquipDetail_Base()
        {
            itemHub.ShowLoadedHint();
            ShowEquipDetail();
            SetEquipDetailAsCurrent();
        }

        protected void OnEmptyEquipDetail_Base()
        {
            itemHub.ShowEmptyHint();
            ShowEquipDetail();
            SetEquipDetailAsCurrent();
        }

        void ShowEquipDetail()
        {
            detailCanvas.enabled = true;
        }

        void SetEquipDetailAsCurrent()
        {
            equipmentMenuManager.mainHub._currentEquipSlotsDetail = this;
            itemHub._cur_EquipSlotsDetail = this;
        }
        #endregion

        #region Off Loaded Equip Detail
        protected abstract void OffLoadedEquipDetail();

        protected void Base_OffLoadedEquipDetail()
        {
            HideEquipDetail();
            UnLoadActiveSlots();
            ResetCursorEventStatus();
        }

        void HideEquipDetail()
        {
            detailCanvas.enabled = false;
        }
        
        void ResetCursorEventStatus()
        {
            isCursorOverSelection = false;
        }
        #endregion

        #region Redraws.
        public abstract void RedrawInfoDetail();

        public abstract void RedrawAlterDetail();
        #endregion

        #region Load / UnLoad Active Slots.
        protected abstract void UnLoadActiveSlots();
        #endregion

        #region Setup.
        protected void base_Setup()
        {
            itemHub = equipmentMenuManager.itemHub;
            _inp = equipmentMenuManager._inp;
        }
        #endregion
    }
}