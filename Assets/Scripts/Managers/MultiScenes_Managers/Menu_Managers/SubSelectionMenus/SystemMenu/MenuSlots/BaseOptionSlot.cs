using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public abstract class BaseOptionSlot : MonoBehaviour, IPointerEnterHandler
    {
        [Header("Status.")]
        [ReadOnlyInspector] public int _slotIndex;
        [ReadOnlyInspector] public bool _isDropdownOptionSlot;

        [Header("Drag and Drop Refs.")]
        public Image _shadowImage;
        public Image _slotRaycastImage;

        [Header("Refs.")]
        [ReadOnlyInspector] public BaseSystemDetail _systemDetail;

        public abstract void Tick();

        public abstract void OnCurrentSlot();

        public abstract void OffCurrentSlot();

        public abstract void OnDetailCloseResetSlot();

        public virtual void DropdownsConnected_OnCurrentSlot_Part1()
        {
        }

        public virtual void DropdownsConnected_OnCurrentSlot_Part2()
        {
        }

        public virtual void DropdownConnected_OffCurrentSlot()
        {
        }

        public virtual Canvas GetDropdownDetailCanvas()
        {
            return null;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            _systemDetail.GetCurrentSlotByPointerEvent(this);
        }

        #region Change Shadow Color.
        public void ChangeShadowToHovering()
        {
            _shadowImage.color = _systemDetail._systemMenuManager._hoveringColor;
        }
        
        public void ChangeShadowToNormal()
        {
            _shadowImage.color = _systemDetail._systemMenuManager._normalColor;
        }

        public void ChangeShadowToPressed()
        {
            _shadowImage.color = _systemDetail._systemMenuManager._pressedColor;
        }
        #endregion

        #region Setup.
        public virtual void Setup(int _slotIndex, BaseSystemDetail _systemDetail)
        {
            this._slotIndex = _slotIndex;
            this._systemDetail = _systemDetail;

            SetupPointerEventImage();
        }

        /// Dual Option Slot Didn't setup pointer event image here.
        void SetupPointerEventImage()
        {
            _systemDetail.AddRaycastableImage(_slotRaycastImage);
            _slotRaycastImage.raycastTarget = false;
        }
        #endregion
    }
}