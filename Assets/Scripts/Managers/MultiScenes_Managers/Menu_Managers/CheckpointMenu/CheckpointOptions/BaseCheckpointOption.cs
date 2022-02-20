using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public abstract class BaseCheckpointOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Option (Drops).")]
        public CanvasGroup _optionSelectorGroup;
        public Canvas _optionSelectorCanvas;
        
        [Header("Status.")]
        [ReadOnlyInspector] public int _optionNumber;

        #region Refs.
        [ReadOnlyInspector] public CheckpointMenuManager checkpointMenuManager;
        #endregion

        #region Private.
        int _fadingTweenId;
        #endregion

        #region On Current Slot.
        public void OnCurrentOption()
        {
            CancelFadingSelector();

            ShowSelector();
            _fadingTweenId = LeanTween.alphaCanvas(_optionSelectorGroup, 1, checkpointMenuManager._selectorFadeInSpeed).setEase(checkpointMenuManager._selectorEaseType).id;
        }
        #endregion

        #region Off Current Slot.
        public void OffCurrentOption()
        {
            CancelFadingSelector();

            _fadingTweenId = LeanTween.alphaCanvas(_optionSelectorGroup, 0, checkpointMenuManager._selectorFadeOutSpeed).setEase(checkpointMenuManager._selectorEaseType).setOnComplete(HideSelector).id;
        }

        void CancelFadingSelector()
        {
            if (LeanTween.isTweening(_fadingTweenId))
                LeanTween.cancel(_fadingTweenId);
        }
        #endregion

        #region Show / Hide Select Slot.
        void ShowSelector()
        {
            _optionSelectorCanvas.enabled = true;
        }

        void HideSelector()
        {
            _optionSelectorCanvas.enabled = false;
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            checkpointMenuManager.GetCurrentOptionByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            checkpointMenuManager.isCursorOverOption = false;
        }
        #endregion

        #region On Select Option.
        public abstract void OnSelectOption();
        #endregion

        #region Setup.
        public void Setup(CheckpointMenuManager _checkpointMenuManager)
        {
            checkpointMenuManager = _checkpointMenuManager;

            SetupSelector();
        }
        
        void SetupSelector()
        {
            _optionSelectorGroup.alpha = 0;
        }
        #endregion
    }
}