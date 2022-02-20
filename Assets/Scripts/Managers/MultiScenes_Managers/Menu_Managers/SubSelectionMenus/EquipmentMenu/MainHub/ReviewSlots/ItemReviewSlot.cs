using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public abstract class ItemReviewSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Config, Position.")]
        public int x_pos;
        public int y_pos;
        
        [Header("Text Config.")]
        public string quickSlotPositionText;

        [Header("Image Config.")]
        public Image slotItemIcon;
        public Image slotDefaultIcon;

        [Header("Highlighter Config.")]
        [SerializeField] public RectTransform _highlighterRect;
        [SerializeField] Canvas _highlighterCanvas;
        
        [Header("Status.")]
        [ReadOnlyInspector] public bool _isSlotEmpty;

        [Header("Refs.")]
        [NonSerialized] protected SavableInventory _inventory;
        protected ItemsReviewSlotDetail reviewSlotDetail;
        protected ItemHub _itemHub;
        protected MainHub _mainHub;

        protected abstract void Redraw_LoadedSlot_InfoDetails();

        protected abstract void Redraw_EmptySlot_InfoDetails();

        protected abstract void Redraw_MainHub_TopText();

        protected abstract void Redraw_Pre_AlterDetails();

        protected abstract void Set_Post_AlterDetails();

        public abstract void DrawEquipHubSlots();

        public abstract void EmptyInventorySlot();
        
        #region On Pointer Enter / Exit.
        public void OnPointerEnter(PointerEventData eventData)
        {
            reviewSlotDetail.GetCurrentReviewSlotByPointerEvent(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            reviewSlotDetail.isCursorOverSelection = false;
        }
        #endregion
        
        #region On / Off Slot
        public void OnCurrentSlot()
        {
            OnHighlighter();

            if (!_isSlotEmpty)
            {
                Redraw_LoadedSlot_InfoDetails();
            }
            else
            {
                Redraw_EmptySlot_InfoDetails();
            }

            Redraw_MainHub_TopText();

            _mainHub._equipmentMenuManager._currentItemReviewSlot = this;
        }

        public void OffCurrentSlot()
        {
            _itemHub.HideCurrentInfoDetails();
            OffHighlighter();
            _mainHub._equipmentMenuManager.ReviewSlot_CancelHighlighter();
        }
        #endregion

        #region On / Off Highlighter.
        void OnHighlighter()
        {
            _highlighterCanvas.enabled = true;
            _mainHub._equipmentMenuManager.RequestNewPingPongLoop();
        }

        protected void OffHighlighter()
        {
            _highlighterCanvas.enabled = false;
        }
        #endregion

        #region Set Slot.
        protected void DrawItemIcon(Sprite itemIconSprite)
        {
            slotItemIcon.sprite = itemIconSprite;
            slotItemIcon.enabled = true;
            slotDefaultIcon.enabled = false;
        }

        protected void DrawDefaultIcon()
        {
            slotItemIcon.enabled = false;
            slotDefaultIcon.enabled = true;
        }
        #endregion

        #region Setup.
        public void Setup(ItemsReviewSlotDetail _reviewSlotDetail)
        {
            reviewSlotDetail = _reviewSlotDetail;

            GetBaseReferences();
            SetTotalSlots2dArrayValue();
        }

        protected virtual void GetBaseReferences()
        {
            _inventory = reviewSlotDetail._inventory;
            _itemHub = reviewSlotDetail.equipmentMenuManager.itemHub;
            _mainHub = reviewSlotDetail.equipmentMenuManager.mainHub;
        }

        void SetTotalSlots2dArrayValue()
        {
            reviewSlotDetail.reviewSlots2dArray[x_pos, y_pos] = this;
        }
        #endregion
    }
}