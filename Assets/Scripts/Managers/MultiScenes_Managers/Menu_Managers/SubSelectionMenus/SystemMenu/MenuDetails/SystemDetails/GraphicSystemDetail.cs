using System;
using UnityEngine;

namespace SA
{
    public class GraphicSystemDetail : BaseSystemDetail
    {
        [Header("Drag and Drop Refs.")]
        public CanvasGroup _graphicDetailGroup;
        public CanvasGroup _choiceDetailGroup;
        public CanvasGroup _graphicHintGroup;
        public CanvasGroup _choiceHintGroup;
        public Canvas _graphicHintCanvas;
        public Canvas _choiceHintCanvas;
        [ReadOnlyInspector] public Canvas[] _dropDownCanvases = new Canvas[2];

        [Header("PingPong Color Config.")]
        public LeanTweenType _pingPongColorEaseType;
        public float _pingPongColorSpeed;

        [Header("Detail Tween.")]
        public LeanTweenType _detailFadeEaseType;
        public float _detailFadeSpeed;
        public float _detailFadeMinValue;

        [Header("Hint Tween.")]
        public float _hintFadeSpeed;

        #region Non Serialized.
        int _pingPongColorTweenId = 0;
        int _graphicDetailTweenId = 0;
        int _choiceDetailTweenId = 0;
        int _hintGroupTweenId = 0;
        #endregion

        #region On Detail Open.
        public override void OnDetailOpen()
        {
            OnDetailOpenSetCurrentSlot();
            OnDetailOpenEnableExtraInputs();
            SetDetailAsCurrentUpdatable();
            EnableSlotsRaycastable();
        }
        #endregion

        #region On Detail Close
        public override void OnDetailClose()
        {
            OnDetailCloseResetSlot();
            OnDetailCloseResetHasChangedStats();
            OnDetailCloseDisableExtraInputs();
            DisableSlotsRaycastable();
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
            GetCurrentSlotByInput();

            _systemMenuManager.CloseMenuByInput();

            _currentSlot.Tick();
        }

        void GetCurrentSlotByInput()
        {
            if (_systemMenuManager.menu_down_input)
            {
                _slotIndex++;
                _slotIndex = _slotIndex == _slotsLength ? 0 : _slotIndex;
                
                if (_optionSlots[_slotIndex] == _currentDualOptionSlot)
                {
                    if (!_hasChangedStats)
                    {
                        _slotIndex = 0;
                    }
                }

                SetCurrentSlot(_optionSlots[_slotIndex]);
            }
            else if (_systemMenuManager.menu_up_input)
            {
                _slotIndex--;
                _slotIndex = _slotIndex < 0 ? _slotsLength - 1 : _slotIndex;
                
                if (_optionSlots[_slotIndex] == _currentDualOptionSlot)
                {
                    if (!_hasChangedStats)
                    {
                        _slotIndex = _currentDualOptionSlot._slotIndex - 1;
                    }
                }

                SetCurrentSlot(_optionSlots[_slotIndex]);
            }
        }
        #endregion

        #region Set Current Slot.
        public override void SetCurrentSlot(BaseOptionSlot _optionSlot)
        {
            /// This is when you need to set a new dropdown slot but the previous slot is also a dropdown slot.
            if (_optionSlot._isDropdownOptionSlot && _currentSlot._isDropdownOptionSlot)
            {
                Canvas _temp_canvas = _currentSlot.GetDropdownDetailCanvas();

                FadeOutChoiceDetail_Full_DropdownConnected(RefreshChoiceDetailOnComplete);

                _currentSlot.DropdownConnected_OffCurrentSlot();
                _currentSlot = _optionSlot;
                _currentSlot.DropdownsConnected_OnCurrentSlot_Part1();

                void RefreshChoiceDetailOnComplete()
                {
                    _temp_canvas.enabled = false;
                    _optionSlot.DropdownsConnected_OnCurrentSlot_Part2();
                }
            }
            /// This is how usually set current slot.
            else
            {
                OffCurrentSlot();
                _currentSlot = _optionSlot;
                OnCurrentSlot();
            }
        }

        public override void OnCurrentSlot()
        {
            _currentSlot.OnCurrentSlot();
        }

        public override void OffCurrentSlot()
        {
            _currentSlot.OffCurrentSlot();
        }
        #endregion

        #region On / Off DropDown Select Fade.
        public void OnDropDownSelect()
        {
            FadeOutGraphicDetail_Half();
            FadeInChoiceDetail_Full();
            SwitchToChoiceHintGroup();
            DisableSlotsRaycastable();
            DisableHorizontalScrollable();
        }

        public void OffDropDownSelect()
        {
            FadeOutChoiceDetail_Half();
            FadeInGraphicDetail_Full();
            SwitchToGraphicHintGroup();
            SetDetailAsCurrentUpdatable();
            EnableSlotsRaycastable();
            EnableHorizontalScrollable();
        }
        #endregion

        #region Tween Graphic Detail Group.
        void FadeOutGraphicDetail_Half()
        {
            if (LeanTween.isTweening(_graphicDetailTweenId))
                LeanTween.cancel(_graphicDetailTweenId);

            _graphicDetailTweenId = LeanTween.alphaCanvas(_graphicDetailGroup, _detailFadeMinValue, _detailFadeSpeed).setEase(_detailFadeEaseType).id;
        }

        void FadeInGraphicDetail_Full()
        {
            if (LeanTween.isTweening(_graphicDetailTweenId))
                LeanTween.cancel(_graphicDetailTweenId);

            _graphicDetailTweenId = LeanTween.alphaCanvas(_graphicDetailGroup, 1, _detailFadeSpeed).setEase(_detailFadeEaseType).id;
        }
        #endregion

        #region Tween Choice Detail Group.
        public void FadeOutChoiceDetail_Full_DropdownConnected(Action _onCompleteAction)
        {
            if (LeanTween.isTweening(_choiceDetailTweenId))
                LeanTween.cancel(_choiceDetailTweenId);

            _choiceDetailTweenId = LeanTween.alphaCanvas(_choiceDetailGroup, 0, _detailFadeSpeed).setEase(_detailFadeEaseType).setOnComplete(_onCompleteAction).id;
        }
        
        public void FadeOutChoiceDetail_Full_Single()
        {
            if (LeanTween.isTweening(_choiceDetailTweenId))
                LeanTween.cancel(_choiceDetailTweenId);

            _choiceDetailTweenId = LeanTween.alphaCanvas(_choiceDetailGroup, 0, _detailFadeSpeed).setEase(_detailFadeEaseType).setOnComplete(HideAllDropdownCanvas).id;
        }

        public void FadeOutChoiceDetail_Half()
        {
            if (LeanTween.isTweening(_choiceDetailTweenId))
                LeanTween.cancel(_choiceDetailTweenId);

            _choiceDetailTweenId = LeanTween.alphaCanvas(_choiceDetailGroup, _detailFadeMinValue, _detailFadeSpeed).setEase(_detailFadeEaseType).id;
        }

        public void FadeInChoiceDetail_Full()
        {
            if (LeanTween.isTweening(_choiceDetailTweenId))
                LeanTween.cancel(_choiceDetailTweenId);

            _choiceDetailTweenId = LeanTween.alphaCanvas(_choiceDetailGroup, 1, _detailFadeSpeed).setEase(_detailFadeEaseType).id;
        }

        public void FadeInChoiceDetail_Half()
        {
            if (LeanTween.isTweening(_choiceDetailTweenId))
                LeanTween.cancel(_choiceDetailTweenId);

            _choiceDetailTweenId = LeanTween.alphaCanvas(_choiceDetailGroup, _detailFadeMinValue, _detailFadeSpeed).setEase(_detailFadeEaseType).id;
        }

        void HideAllDropdownCanvas()
        {
            _dropDownCanvases[0].enabled = false;
            _dropDownCanvases[1].enabled = false;
        }
        #endregion

        #region Tween Hint Group.
        public void SwitchToChoiceHintGroup()
        {
            if (LeanTween.isTweening(_hintGroupTweenId))
                LeanTween.cancel(_hintGroupTweenId);

            _hintGroupTweenId = LeanTween.alphaCanvas(_graphicHintGroup, 0, _hintFadeSpeed).setEase(_detailFadeEaseType).setOnComplete(OnCompleteFadeOutGraphicHintGroup).id;
        }

        void OnCompleteFadeOutGraphicHintGroup()
        {
            _graphicHintCanvas.enabled = false;
            _choiceHintCanvas.enabled = true;

            LeanTween.alphaCanvas(_choiceHintGroup, 1, _hintFadeSpeed).setEase(_detailFadeEaseType);
        }

        public void SwitchToGraphicHintGroup()
        {
            if (LeanTween.isTweening(_hintGroupTweenId))
                LeanTween.cancel(_hintGroupTweenId);

            _hintGroupTweenId = LeanTween.alphaCanvas(_choiceHintGroup, 0, _hintFadeSpeed).setEase(_detailFadeEaseType).setOnComplete(OnCompleteFadeOutChoiceHintGroup).id;
        }

        void OnCompleteFadeOutChoiceHintGroup()
        {
            _choiceHintCanvas.enabled = false;
            _graphicHintCanvas.enabled = true;

            LeanTween.alphaCanvas(_graphicHintGroup, 1, _hintFadeSpeed).setEase(_detailFadeEaseType);
        }
        #endregion

        #region On Select Change Shadow Color.
        public override void OnSelectChangeShadowColor()
        {
            PingPongTweenSlotColor();
        }

        void PingPongTweenSlotColor()
        {
            LeanTween.color(_currentSlot._shadowImage.rectTransform, _systemMenuManager._pressedColor, _pingPongColorSpeed).setEase(_pingPongColorEaseType).setLoopPingPong(1);
        }
        #endregion

        #region Setup.
        public override void Setup(SystemMenuManager _systemMenuManager)
        {
            this._systemMenuManager = _systemMenuManager;
            BaseSetup();
            SetupSlots();
        }

        public override void SetupSlots()
        {
            _slotsLength = _optionSlots.Length;
            for (int i = 0; i < _slotsLength; i++)
            {
                _optionSlots[i].Setup(i, this);
            }
        }

        /// Graphic Detail Group.
        public override GraphicSystemDetail GetGraphicSystemDetail()
        {
            return this;
        }
        #endregion
    }
} 