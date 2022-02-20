using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public abstract class ItemEquipSlot : MonoBehaviour
    {
        [Header("Image Config.")]
        public Image slotItemIcon;
        public Image slotImage;

        [Header("Highlighter Config.")]
        public RectTransform _highlighterRect;
        [SerializeField] Canvas _highlighterCanvas;
        
        [Header("Slot Status.")]
        [ReadOnlyInspector] public int _slotDetailIndex;

        [Header("Refs.")]
        [ReadOnlyInspector] protected ItemHub itemHub;
        [ReadOnlyInspector] protected MainHub mainHub;
        [ReadOnlyInspector] protected EquipmentMenuManager equipmentMenuManager;

        public abstract void Redraw_EquipSlot_InfoDetails();

        public abstract void Redraw_Post_AlterDetails();

        protected abstract void Redraw_MainHub_TopText();
        
        public void OnCurrentSlot()
        {
            OnHighlighter();

            RedrawCurrentSlotDetail();

            Redraw_MainHub_TopText();

            equipmentMenuManager._currentItemEquipSlot = this;
        }

        public void OffCurrentSlot()
        {
            OffHighlighter();
            equipmentMenuManager.EquipSlot_CancelHighlighter();
        }

        public void OffEquipSlotDetail()
        {
            OffHighlighter();
        }

        public void RedrawCurrentSlotDetail()
        {
            if (itemHub.isShowAlterHub)
            {
                Redraw_Post_AlterDetails();
            }
            else
            {
                Redraw_EquipSlot_InfoDetails();
            }
        }
        
        #region Hide / Show Slot.
        protected void ShowEquipSlot(Sprite _itemSprite)
        {
            slotImage.raycastTarget = true;
            slotItemIcon.enabled = true;
            slotItemIcon.sprite = _itemSprite;
        }

        protected void HideEquipSlot()
        {
            slotImage.raycastTarget = false;
            slotItemIcon.enabled = false;
        }
        #endregion

        #region On / Off Highlighter.
        protected void OnHighlighter()
        {
            _highlighterCanvas.enabled = true;
            equipmentMenuManager.RequestNewPingPongLoop();
        }

        protected void OffHighlighter()
        {
            _highlighterCanvas.enabled = false;
        }
        #endregion
    }
}