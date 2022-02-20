using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public abstract class AlterableDefinableAttributePreview : DefinableAttributePreview
    {
        [Header("Highlighter.")]
        public RectTransform _highlighterRect;
        public Canvas _highlighterCanvas;

        [Header("D Button.")]
        public Button _decrementButton;
        public Canvas _decrementButtonCanvas;
        public Image _decrementButtonImage;

        [Header("I Button.")]
        public Button _incrementButton;
        public Canvas _incrementButtonCanvas;
        public Image _incrementButtonImage;

        [Header("Tween Config.")]
        public LeanTweenType _previewEaseType = LeanTweenType.easeOutSine;
        public float _previewHighlightFadeSpeed = 1;
        public float _previewPingPongMinValue = 0.2f;
        public float _previewPingPongMaxValue = 0.6f;
        public Vector3 _buttonEnlargeSize = new Vector3(1.6f, 1.6f, 1);
        public Vector3 _buttonOriginalSize = new Vector3(1, 1, 1);
        public float _buttonEnlargeSpeed = 0.1f;
        int _attributeEnglargeId;
        
        [Header("Status.")]
        [ReadOnlyInspector] public bool _hasAlterAttributes;
        [ReadOnlyInspector, SerializeField] bool isDecrementColorChanged;
        [ReadOnlyInspector, SerializeField] bool isIncrementColorChanged;

        [Header("General Attribute Previews.")]
        [ReadOnlyInspector] public DefinablePlayerLevelPreview _playerLevelPreview;
        [ReadOnlyInspector] public DefinableVolunPreview _volunPreview;
        [ReadOnlyInspector] public int _alterDefinablePreviewIndex;

        #region On / Off Attribute Preview.
        public abstract void Tick();

        public abstract void OnAttributePreview();

        public abstract void OffAttributePreview();
        #endregion

        #region Reset On Menu Close.
        public abstract void ResetOnMenuClose();

        public abstract void ResetOnPreviewGroupSwitch(bool _isSwitchPreviewGroup);

        public void BaseResetOnMenuClose()
        {
            // Highlighter.
            CancelHighlighter();
            OffHighlighter();
            
            // Button.
            ReverseIncrementColor();
            ReverseIncrementButtonSize();
            ReverseDecrementButtonSize();
            OffBothButtons();

            // Status;
            _hasAlterAttributes = false;
        }
        #endregion

        #region Reset On Levelup.
        public void ResetOnLevelup()
        {
            _hasAlterAttributes = false;
        }
        #endregion

        #region Redraw Increment / Decrement Button.
        public void RedrawIncrementButton()
        {
            PingPongIncrementButtonSize();

            /// because the attribute is altered, change the 
            ReverseDecrementColor();

            /// If there is not enough point to level up anymore, change the increment button color to unavaliable.
            if (_levelingHub._isNotEnoughVolun)
                ChangeIncrementColor();

            _hasAlterAttributes = true;
        }

        public void RedrawDecrementButton(bool _hasBackToInitialValue)
        {
            PingPongDecrementButtonSize();

            /// because this attribute level is decremented, there should be enough point again to levelup on other attributes.
            ReverseIncrementColor();

            if (_hasBackToInitialValue)
            {
                ChangeDecrementColor();
                _hasAlterAttributes = false;
            }
        }
        #endregion

        #region Highlighter
        protected void HighlighterTick()
        {
            if (_levelingHub._isLoopComplete)
            {
                _levelingHub.pingPongTweenId = LeanTween.alpha(_highlighterRect, _previewPingPongMinValue, _previewHighlightFadeSpeed).setLoopPingPong(1).setEase(_previewEaseType).setOnComplete(RequestNewPingPongLoop).id;
                _levelingHub._isLoopComplete = false;
            }
        }

        protected void OnHighlighter()
        {
            _highlighterCanvas.enabled = true;
            _levelingHub._isLoopComplete = true;
            //Debug.Log("OnHighlighter1");
        }

        protected void OffHighlighter()
        {
            _highlighterCanvas.enabled = false;
        }

        protected void RequestNewPingPongLoop()
        {
            _levelingHub._isLoopComplete = true;
        }

        protected void CancelHighlighter()
        {
            LeanTween.cancel(_levelingHub.pingPongTweenId);
            LeanTween.alpha(_highlighterRect, _previewPingPongMaxValue, 0);
        }
        #endregion

        #region On / Off Buttons.
        protected void OnBothButtons()
        {
            _decrementButtonCanvas.enabled = true;
            _incrementButtonCanvas.enabled = true;

            /// If this attribute hasn't been altered yet, change decrement button color to unavailable.
            if (!_hasAlterAttributes)
                ChangeDecrementColor();

            /// If there is not enough point to level up anymore, change the increment button color to unavaliable.
            if (_levelingHub._isNotEnoughVolun)
                ChangeIncrementColor();
        }

        protected void OffBothButtons()
        {
            _decrementButtonCanvas.enabled = false;
            _incrementButtonCanvas.enabled = false;
            
            ReverseIncrementColor();
        }
        #endregion

        #region Colors.
        void ChangeDecrementColor()
        {
            if (!isDecrementColorChanged)
            {
                _decrementButtonImage.color = LevelingMenuManager.singleton._unavaliableAttriButtonColor;
                isDecrementColorChanged = true;
            }
        }

        void ReverseDecrementColor()
        {
            if (isDecrementColorChanged)
            {
                _decrementButtonImage.color = Color.white;
                isDecrementColorChanged = false;
            }
        }

        void ChangeIncrementColor()
        {
            if (!isIncrementColorChanged)
            {
                _incrementButtonImage.color = LevelingMenuManager.singleton._unavaliableAttriButtonColor;
                isIncrementColorChanged = true;
            }
        }

        void ReverseIncrementColor()
        {
            if (isIncrementColorChanged)
            {
                _incrementButtonImage.color = Color.white;
                isIncrementColorChanged = false;
            }
        }
        #endregion

        #region Sizes.

        #region Attributes Text.
        public void PingPongAttributeTextSize()
        {
            CancelAttributeEnlargement();
            EnlargeAttributeTextSize();
        }

        public void ReverseAttributeTextSize()
        {
            _afterChangesText.rectTransform.localScale = _buttonOriginalSize;
        }

        void EnlargeAttributeTextSize()
        {
            _attributeEnglargeId = LeanTween.scale(_afterChangesText.rectTransform, _buttonEnlargeSize, _buttonEnlargeSpeed).setOnComplete(OnCompleteReturnTextSize).id;
        }

        void OnCompleteReturnTextSize()
        {
            _attributeEnglargeId = LeanTween.scale(_afterChangesText.rectTransform, _buttonOriginalSize, _buttonEnlargeSpeed).id;
        }

        void CancelAttributeEnlargement()
        {
            if (LeanTween.isTweening(_attributeEnglargeId))
                LeanTween.cancel(_attributeEnglargeId);
        }
        #endregion

        #region Increment / Decrement Value.
        void PingPongIncrementButtonSize()
        {
            LeanTween.scale(_incrementButtonImage.rectTransform, _buttonEnlargeSize, _buttonEnlargeSpeed).setLoopPingPong(1);
        }

        void ReverseIncrementButtonSize()
        {
            _incrementButtonImage.rectTransform.localScale = _buttonOriginalSize;
        }

        void ReverseDecrementButtonSize()
        {
            _decrementButtonImage.rectTransform.localScale = _buttonOriginalSize;
        }

        void PingPongDecrementButtonSize()
        {
            LeanTween.scale(_decrementButtonImage.rectTransform, _buttonEnlargeSize, _buttonEnlargeSpeed).setLoopPingPong(1);
        }
        #endregion

        #endregion

        #region Setup.
        public void AlterableSetup()
        {
            OffHighlighter();
            OffBothButtons();
            SetupGeneralPreviews();
        }

        void SetupGeneralPreviews()
        {
            _playerLevelPreview = _levelingHub._playerLevelPreview;
            _volunPreview = _levelingHub._volunPointPreview;
        }
        #endregion
    }
}