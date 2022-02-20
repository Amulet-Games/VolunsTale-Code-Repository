using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public abstract class BaseInstructionSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Raycast Target (Drops.)")]
        public Image _raycastTargetImg;

        [Header("Highlighter (Drops.)")]
        public Image highlighterImg;
        public Canvas highlighterCanvas;

        [Header("Selector (Drops.)")]
        public Canvas selectorCanvas;

        [Header("Status.")]
        [ReadOnlyInspector] public int slotNumber;
        [ReadOnlyInspector] public bool isSelected;

        #region Refs.
        [Header("Ref.")]
        [ReadOnlyInspector] public InstructionMenuManager _instructionMenuManager;
        #endregion

        #region On Current Slot.
        public void OnCurrentSlot()
        {
            ShowHighlighter();
        }
        #endregion

        #region Off Current Slot.
        public void OffCurrentSlot()
        {
            if (!isSelected)
                HideHighlighter();
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            _instructionMenuManager.GetCurrentSlotByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            _instructionMenuManager.isCursorOverSelection = false;
        }
        #endregion

        #region On Select Slot.
        public void OnFirstSelectSlot()
        {
            highlighterImg.color = _instructionMenuManager._slotClickOutColor;
            ShowHighlighter();
            ShowSelector();
            OnSelectSlot();
        }

        public abstract void OnSelectSlot();

        protected void BaseOnSelectSlot()
        {
            isSelected = true;
            DisableRaycastTarget();
        }
        #endregion

        #region Off Select Slot.
        public void OffSelectSlot()
        {
            isSelected = false;
            highlighterImg.color = _instructionMenuManager._slotHoverColor;
            HideHighlighter();
            HideSelector();
            EnableRaycastTarget();
        }
        #endregion
        
        #region Show / Hide HighLighter.
        void ShowHighlighter()
        {
            highlighterCanvas.enabled = true;
        }

        void HideHighlighter()
        {
            highlighterCanvas.enabled = false;
        }
        #endregion

        #region Show / Hide Selector.
        public void ShowSelector()
        {
            selectorCanvas.enabled = true;
        }

        public void HideSelector()
        {
            selectorCanvas.enabled = false;
        }
        #endregion

        #region Enable / Disable Raycast.
        void EnableRaycastTarget()
        {
            _raycastTargetImg.raycastTarget = true;
        }

        void DisableRaycastTarget()
        {
            _raycastTargetImg.raycastTarget = false;
        }
        #endregion

        public abstract void SetCurrentInstructPage();
    }
}