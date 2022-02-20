using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace SA
{
    public abstract class BaseChoiceSlot : MonoBehaviour, IPointerEnterHandler
    {
        [Header("Status.")]
        [ReadOnlyInspector] public int _slotIndex;

        [Header("Drag and Drop Refs.")]
        public Image _slotRaycastImage;
        public Image _shadowImage;
        public Canvas _shadowCanvas;

        [Header("Refs.")]
        [ReadOnlyInspector] public BaseChoiceDetail _choiceDetail;
        
        public void OnCurrentSlot()
        {
            _shadowCanvas.enabled = true;
            ChangeShadowToHovering();
        }

        public void OffCurrentSlot()
        {
            _shadowCanvas.enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _choiceDetail.GetCurrentSlotByPointerEvent(this);
        }

        public abstract void OnChoiceSelected();

        public virtual Vector3 GetSlotLocalPosition()
        {
            return Vector3.zero;
        }

        public virtual TMP_Text GetSlotTitleText()
        {
            return null;
        }

        #region Change Shadow Color.
        public void ChangeShadowToHovering()
        {
            _shadowImage.color = _choiceDetail._hoveringColor;
        }

        public void ChangeShadowToNormal()
        {
            _shadowImage.color = _choiceDetail._normalColor;
        }

        public void ChangeShadowToPressed()
        {
            _shadowImage.color = _choiceDetail._pressedColor;
        }
        #endregion

        #region Events.
        public void EnableRaycastTarget()
        {
            _slotRaycastImage.raycastTarget = true;
        }

        public void DisableRaycastTarget()
        {
            _slotRaycastImage.raycastTarget = false;
        }
        #endregion

        #region Setup.
        public virtual void Setup(int _slotIndex, BaseChoiceDetail _choiceDetail)
        {
            this._slotIndex = _slotIndex;
            this._choiceDetail = _choiceDetail;

            /// Activate GameObjects but set them not pointer event raycastable. 
            gameObject.SetActive(true);

            /// Raycast Target Event.
            DisableRaycastTarget();
            _choiceDetail._slotsRaycastTargetEnableEvent += EnableRaycastTarget;
            _choiceDetail._slotsRaycastTargetDisableEvent += DisableRaycastTarget;
        }
        #endregion
    }
}