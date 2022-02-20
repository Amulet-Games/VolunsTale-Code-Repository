using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public abstract class DefinableStatsPreview : BaseDefinablePreview, IPointerEnterHandler
    {
        [Header("Highlighter.")]
        public Image _highlighterImage;
        public LeanTweenType _highlighterEaseType = LeanTweenType.easeOutSine;
        public float _highlighterFadeSpeed = 1;
        public float _hightlighterMaxValue = 1f;

        [Header("Private.")]
        Color _highlighterInitColor;
        Color _highlighterMinColor;

        [Header("Selector.")]
        public Canvas _selectorCanvas;
        
        [Header("Highlighter Status.")]
        [ReadOnlyInspector] public bool _isLoopComplete;
        [ReadOnlyInspector] public int pingPongTweenId;
        
        [Header("Refs.")]
        [ReadOnlyInspector] public PreviewHub _previewHub;

        #region Redraws.
        /// Only redraw the after changes text, when player increase or decrease certain attributes.
        public abstract void RedrawIncrementPreview();

        public abstract void RedrawDecrementPreview();

        /// Redraw both before changes text and after changes text, when player confirm the point distribution at the end or when leveling menu opened.
        public abstract void RedrawConfirmedStatsPreview();
        #endregion

        #region Reset On Menu Close.
        public void ResetOnMenuClose()
        {
            CancelHighlighter();
        }
        #endregion

        #region Highlighter.
        public void HighlighterTick()
        {
            if (_isLoopComplete)
            {
                pingPongTweenId = LeanTween.alpha(_highlighterImage.rectTransform, _hightlighterMaxValue, _highlighterFadeSpeed).setLoopPingPong(1).setEase(_highlighterEaseType).setOnComplete(RequestNewPingPongLoop).id;
                _isLoopComplete = false;
            }
        }

        public void OnHighlighter()
        {
            //Debug.Log("On StatsPreviewHighlighter");
            _highlighterImage.color = _highlighterMinColor;
            RequestNewPingPongLoop();
        }

        public void OffHighlighter()
        {
            //Debug.Log("Off StatsPreviewHighlighter");
            CancelHighlighter();
        }

        public void RequestNewPingPongLoop()
        {
           _isLoopComplete = true;
        }

        public void CancelHighlighter()
        {
            LeanTween.cancel(pingPongTweenId);
            _highlighterImage.color = _highlighterInitColor;
        }
        #endregion

        #region On Stats Preview.
        public void OnStatsPreview()
        {
            RedrawDefinitionDetail();
            OnSelector();
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            _previewHub.GetCurrentDefinableStatsByCursor(this);
        }
        #endregion
        
        #region Selector.
        public void OnSelector()
        {
            _selectorCanvas.enabled = true;
        }

        public void OffSelector()
        {
            _selectorCanvas.enabled = false;
        }
        #endregion

        #region Setup.
        public void StatsPreviewSetup(StatsAttributeHandler statsHandler, PreviewHub previewHub)
        {
            _previewHub = previewHub;
            _statsHandler = statsHandler;

            SetupHighlighterColors();
        }

        void SetupHighlighterColors()
        {
            /// Init Color  (a = 0.2f)
            _highlighterInitColor = _highlighterImage.color;

            /// Min Alpha Color (a = 0.6f)
            _highlighterMinColor = _highlighterInitColor;
            _highlighterMinColor.a = 0.6f;
        }
        #endregion
    }
}