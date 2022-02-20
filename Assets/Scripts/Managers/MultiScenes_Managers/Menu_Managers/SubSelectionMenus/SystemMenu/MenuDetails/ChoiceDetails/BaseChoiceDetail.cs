using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public abstract class BaseChoiceDetail : MonoBehaviour, ISystemMenuDetailUpdatable
    {
        [Header("Slots.")]
        public BaseChoiceSlot[] _choiceSlots;
        
        [Header("Slot Color Config.")]
        public Color _normalColor;
        public Color _hoveringColor;
        public Color _pressedColor;
        
        [Header("Status.")]
        [ReadOnlyInspector] public int _slotsLength;
        [ReadOnlyInspector] public int _slotIndex;
        [ReadOnlyInspector] public BaseChoiceSlot _currentSlot;
        [ReadOnlyInspector] public BaseChoiceSlot _currentChosenSlot;
        
        [Header("Refs.")]
        [ReadOnlyInspector] public Canvas _choiceDetailCanvas;
        [ReadOnlyInspector] public SystemMenuManager _systemMenuManager;

        public event Action _slotsRaycastTargetEnableEvent;
        public event Action _slotsRaycastTargetDisableEvent;

        public abstract void OnDetailPreview();
        
        public abstract void OnDetailOpen();

        public abstract void OnDetailClose();

        public abstract void Tick();

        #region Set Current Slot.
        public abstract void SetCurrentSlot(BaseChoiceSlot _optionSlot);
        #endregion

        #region Pointer Events.
        public void GetCurrentSlotByPointerEvent(BaseChoiceSlot _targetSlot)
        {
            if (_currentSlot != _targetSlot && _currentChosenSlot != _targetSlot)
            {
                _slotIndex = _targetSlot._slotIndex;
                SetCurrentSlot(_targetSlot);
            }
        }

        public abstract void SetCurrentSlotByPointerEvent();
        #endregion

        #region Show / Hide Choice Detail.
        public void ShowChoiceDetail()
        {
            _choiceDetailCanvas.enabled = true;
        }

        public void HideChoiceDetail()
        {
            _choiceDetailCanvas.enabled = false;
        }
        #endregion

        #region Enable / Disable RaycastTarget.
        protected void OnDetailOpenEnableRaycastTarget()
        {
            _slotsRaycastTargetEnableEvent.Invoke();
        }

        protected void OnDetailCloseDisableRaycastTarget()
        {
            _slotsRaycastTargetDisableEvent.Invoke();
        }
        #endregion

        #region Setup.
        public abstract void SetupSlots();

        protected void BaseSetup()
        {
            SetupBaseDetailCanvas();
        }

        void SetupBaseDetailCanvas()
        {
            _choiceDetailCanvas = GetComponent<Canvas>();
        }
        #endregion
    }
}