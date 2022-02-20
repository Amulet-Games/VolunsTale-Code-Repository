using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public abstract class BaseSelectionSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Highlighter (Drops).")]
        public RectTransform _highlighterRect;
        public Canvas _highlighterCanvas;
        
        [Header("Status.")]
        [ReadOnlyInspector] public int slotNumber;
        [ReadOnlyInspector, SerializeField] bool _isLoopComplete;

        #region Refs.
        [Header("Ref.")]
        [ReadOnlyInspector] public SelectionMenuManager _selectionMenuManager;
        #endregion
        
        #region Tick.
        public void Tick()
        {
            if (_isLoopComplete)
            {
                _selectionMenuManager.CurrentSlotPingPongTween();
                _isLoopComplete = false;
            }
        }

        public void RequestNewPingPongLoop()
        {
            _isLoopComplete = true;
        }
        #endregion

        #region On Current Slot.
        public void OnCurrentSlot()
        {
            ShowHighlighter();
            UpdateTitleText();
        }

        protected abstract void UpdateTitleText();
        #endregion

        #region Off Current Slot.
        public void OffCurrentSlot()
        {
            HideHighlighter();
            _selectionMenuManager.CancelCurrentSlotHighlighterTween();
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            _selectionMenuManager.GetCurrentSlotByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            _selectionMenuManager.isCursorOverSelection = false;
        }
        #endregion

        #region On Select Slot.
        public abstract void OnSelectSlot();
        #endregion

        #region Show / Hide HighLighter.
        void ShowHighlighter()
        {
            _highlighterCanvas.enabled = true;
            _isLoopComplete = true;
        }

        void HideHighlighter()
        {
            _highlighterCanvas.enabled = false;
        }
        #endregion
    }
}