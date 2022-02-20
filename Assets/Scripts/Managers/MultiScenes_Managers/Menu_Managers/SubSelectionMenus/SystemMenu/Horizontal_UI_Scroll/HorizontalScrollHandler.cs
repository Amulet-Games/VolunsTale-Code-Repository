using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SA
{
    public class HorizontalScrollHandler : MonoBehaviour
    {
        [Header("Text Size Tween Config.")]
        public float _normalTextSize;
        public float _currentTextSize;
        public float _elementTextResizeSpeed;

        [Header("Text Color Tween Config.")]
        public Color _normalTextColor;
        public Color _currentTextColor;
        public float _elementTextRecolorSpeed;

        [Header("Arrows Tween Config.")]
        public CanvasGroup _arrowsGroup;
        public RectTransform _l_arrow_Rect;
        public RectTransform _r_arrow_Rect;
        public LeanTweenType _arrowsEaseType;
        public float _arrowsFadeSpeed;
        public float _arrowsFadeDelaySecond;

        [Header("Element Icon Tween Config.")]
        public float _elementIconFadeSpeed;

        [Header("Element Tween Config.")]
        public LeanTweenType _elementScrollEaseType;
        public float _elementScrollSpeed;
        public float _scrollDistance;
        public float _teleportOffset;

        [Header("Elements.")]
        public BaseScrollElement[] _elements;
        [ReadOnlyInspector] public BaseScrollElement _cur_element;
        [ReadOnlyInspector] public BaseScrollElement _next_element;
        [ReadOnlyInspector] public BaseScrollElement _second_prev_element;

        [Header("Manager Refs.")]
        [ReadOnlyInspector] public SystemMenuManager _systemMenuManager;

        [Header("Status.")]
        [ReadOnlyInspector] public bool _isForbiddenToScroll;
        [ReadOnlyInspector] public int _cur_element_index;
        [ReadOnlyInspector] public int _elementsLength;

        #region Non Serialized.
        Vector3 _pivotPosition = new Vector3(0, 0, 0);
        #endregion

        public void Tick()
        {
            /// This is Forward Scrolling.
            if (_systemMenuManager.menu_scroll_fwd_input)
            {
                OnScrollForward();
            }
            /// This is Backward Scrolling.
            else if (_systemMenuManager.menu_scroll_bwd_input)
            {
                OnScrollBackward();
            }
        }

        #region Teleport Element.
        /// When scroll backward the second last element need to be teleported in the front.
        void TeleportElementToTheFront(BaseScrollElement _sec_last_element)
        {
            Vector3 _targetPos = _sec_last_element._elementRect.localPosition;
            _targetPos.x = _cur_element._elementRect.localPosition.x + _teleportOffset;
            _sec_last_element._elementRect.localPosition = _targetPos;
        }

        /// When scroll forward the second next element need to be teleported in the back.
        void TeleportElementToTheBack(int _second_next_element_index)
        {
            BaseScrollElement _sec_next_element = _elements[_second_next_element_index];

            Vector3 _targetPos = _sec_next_element._elementRect.localPosition;
            _targetPos.x = _cur_element._elementRect.localPosition.x - _teleportOffset;
            _sec_next_element._elementRect.localPosition = _targetPos;
        }
        #endregion

        #region On Scroll Forward / Backward.
        public void OnScrollForward()
        {
            if (_isForbiddenToScroll)
                return;

            PreSetNewValueTween();

            int _second_next_element_index = _cur_element_index + 2;
            _second_next_element_index = _second_next_element_index >= _elementsLength ? _second_next_element_index - _elementsLength : _second_next_element_index;
            _elements[_second_next_element_index].ShowElement();
            TeleportElementToTheBack(_second_next_element_index);

            int _next_element_index = _cur_element_index + 1;
            _next_element_index = _next_element_index == _elementsLength ? 0 : _next_element_index;
            _next_element = _elements[_next_element_index];

            _cur_element_index--;
            _cur_element_index = _cur_element_index < 0 ? _elementsLength - 1 : _cur_element_index;
            _cur_element = _elements[_cur_element_index];

            /// Setting Current System Detail.
            _systemMenuManager.SetCurrentSystemDetail(_cur_element.GetReferedElementDetail());

            _second_prev_element = _next_element;

            PostSetNewValueTween();

            void PreSetNewValueTween()
            {
                ChangeTextColorToNormal();
                ChangeTextSizeToNormal();
                FadeOutCurrentIcon();
                _systemMenuManager.FadeOutCurrentDetail();
            }

            void PostSetNewValueTween()
            {
                FadeOutArrows();
                ScrollElementsForward();
                ChangeTextColorToCurrent();
                ChangeTextSizeToCurrent();
            }
        }

        public void OnScrollBackward()
        {
            if (_isForbiddenToScroll)
                return;

            PreSetNewValueTween();

            _second_prev_element.ShowElement();
            TeleportElementToTheFront(_second_prev_element);

            _cur_element_index++;
            _cur_element_index = _cur_element_index == _elementsLength ? 0 : _cur_element_index;
            _cur_element = _elements[_cur_element_index];
            
            int _new_second_prev_element_index = _cur_element_index - 2;
            _new_second_prev_element_index = _new_second_prev_element_index < 0 ? _elementsLength + _new_second_prev_element_index : _new_second_prev_element_index;
            _second_prev_element = _elements[_new_second_prev_element_index];

            /// Setting Current System Detail.
            _systemMenuManager.SetCurrentSystemDetail(_cur_element.GetReferedElementDetail());

            PostSetNewValueTween();

            void PreSetNewValueTween()
            {
                ChangeTextColorToNormal();
                ChangeTextSizeToNormal();
                FadeOutCurrentIcon();
                _systemMenuManager.FadeOutCurrentDetail();
            }

            void PostSetNewValueTween()
            {
                FadeOutArrows();
                ScrollElementBackward();
                ChangeTextColorToCurrent();
                ChangeTextSizeToCurrent();
            }
        }
        #endregion

        #region Scroll Tween.
        void ScrollElementsForward()
        {
            _isForbiddenToScroll = true;

            for (int i = 0; i < _elementsLength; i++)
            {
                if (i == _elementsLength - 1)
                {
                    LeanTween.moveX(_elements[i]._elementRect, _elements[i]._elementRect.localPosition.x + _scrollDistance, _elementScrollSpeed).setEase(_elementScrollEaseType).setOnComplete(OnCompleteScrollForward);
                }
                else
                {
                    LeanTween.moveX(_elements[i]._elementRect, _elements[i]._elementRect.localPosition.x + _scrollDistance, _elementScrollSpeed).setEase(_elementScrollEaseType);
                }
            }
        }
        
        void ScrollElementBackward()
        {
            _isForbiddenToScroll = true;

            for (int i = 0; i < _elementsLength; i++)
            {
                if (i == _elementsLength - 1)
                {
                    LeanTween.moveX(_elements[i]._elementRect, _elements[i]._elementRect.localPosition.x - _scrollDistance, _elementScrollSpeed).setEase(_elementScrollEaseType).setOnComplete(OnCompleteScrollBackward);
                }
                else
                {
                    LeanTween.moveX(_elements[i]._elementRect, _elements[i]._elementRect.localPosition.x - _scrollDistance, _elementScrollSpeed).setEase(_elementScrollEaseType);
                }
            }
        }

        void OnCompleteScrollForward()
        {
            _next_element.HideElement();
        }

        void OnCompleteScrollBackward()
        {
            _second_prev_element.HideElement();
        }
        #endregion

        #region Arrow Tween.
        void FadeOutArrows()
        {
            LeanTween.alphaCanvas(_arrowsGroup, 0, _arrowsFadeSpeed).setEase(_arrowsEaseType);
            LeanTween.delayedCall(_arrowsFadeDelaySecond, FadeInArrows);
        }

        void FadeInArrows()
        {
            LeanTween.alphaCanvas(_arrowsGroup, 1, _arrowsFadeSpeed).setEase(_arrowsEaseType).setOnComplete(OnCompleteFadeInArrows);

            /// Fade In Icon the same time with arrows.
            FadeInCurrentIcon();
            _systemMenuManager.FadeInCurrentDetail();
        }

        void OnCompleteFadeInArrows()
        {
            _isForbiddenToScroll = false;
        }
        #endregion

        #region Color Tween.
        void ChangeTextColorToCurrent()
        {
            _cur_element._elementText.LeanTMPTextColor(_currentTextColor, _elementTextRecolorSpeed);
        }

        void ChangeTextColorToNormal()
        {
            _cur_element._elementText.LeanTMPTextColor(_normalTextColor, _elementTextRecolorSpeed);
        }
        #endregion

        #region Icon Tween.
        void FadeOutCurrentIcon()
        {
            BaseScrollElement _prev_cur_element = _cur_element;
            LeanTween.alpha(_cur_element._elementIconRect, 0, _elementIconFadeSpeed).setOnComplete(OnCompleteFadeOutIcon);

            void OnCompleteFadeOutIcon()
            {
                _prev_cur_element.DisableIconCanvas();
            }
        }

        void FadeInCurrentIcon()
        {
            _cur_element.EnableIconCanvas();
            LeanTween.alpha(_cur_element._elementIconRect, 1, _elementIconFadeSpeed);
        }
        #endregion

        #region Text Size Tween.
        void ChangeTextSizeToCurrent()
        {
            TMP_Text _text = _cur_element._elementText;
            LeanTween.value(_text.fontSize, _currentTextSize, _elementTextResizeSpeed).setOnUpdate((value) => _text.fontSize = value);
        }

        void ChangeTextSizeToNormal()
        {
            TMP_Text _text = _cur_element._elementText;
            LeanTween.value(_text.fontSize, _normalTextSize, _elementTextResizeSpeed).setOnUpdate((value) => _text.fontSize = value);
        }
        #endregion
        
        #region On Before Menu Open.
        public void OnBeforeMenuOpen()
        {
            OnMenuOpenSetupCurrentElement();
            OnMenuOpenTeleportElement();
            OnMenuOpenShowIconNoTween();
        }

        void OnMenuOpenSetupCurrentElement()
        {
            _cur_element_index = 0;
            _cur_element = _elements[_cur_element_index];

            _systemMenuManager.SetCurrentSystemDetail(_cur_element.GetReferedElementDetail());

            _second_prev_element = _elements[_elementsLength - 2];
            _next_element = _elements[_cur_element_index + 1];
        }

        void OnMenuOpenTeleportElement()
        {
            _cur_element._elementRect.localPosition = _pivotPosition;

            Vector3 _tempPosition = new Vector3(_scrollDistance, 0, 0);
            _elements[1]._elementRect.localPosition = _pivotPosition + _tempPosition;
            _elements[3]._elementRect.localPosition = _pivotPosition - _tempPosition;

            _tempPosition.x = _teleportOffset;
            _elements[2]._elementRect.localPosition = _pivotPosition + _tempPosition;
        }

        void OnMenuOpenShowIconNoTween()
        {
            LeanTween.alpha(_cur_element._elementIconRect, 1, 0);
            _cur_element._elementIconCanvas.enabled = true;
        }
        #endregion

        #region On After Menu Close.
        public void OnAfterMenuClose()
        {
            _next_element.ShowElement();
            _second_prev_element.ShowElement();

            OnAfterMenuCloseChangeTextColor();
            OnAfterMenuCloseChangeTextSize();
            OnAfterMenuCloseFadeOutIcon();
        }
        
        void OnAfterMenuCloseChangeTextColor()
        {
            _cur_element._elementText.LeanTMPTextColor(_normalTextColor, 0);
        }

        void OnAfterMenuCloseChangeTextSize()
        {
            _cur_element._elementText.fontSize = _normalTextSize;
        }

        void OnAfterMenuCloseFadeOutIcon()
        {
            LeanTween.alpha(_cur_element._elementIconRect, 0, 0);
            _cur_element._elementIconCanvas.enabled = false;
        }
        #endregion

        #region Setup.
        public void Setup(SystemMenuManager _systemMenuManager)
        {
            this._systemMenuManager = _systemMenuManager;

            SetupElements();
        }

        void SetupElements()
        {
            _elementsLength = _elements.Length;
            for (int i = 0; i < _elementsLength; i++)
            {
                _elements[i].Setup(this);
            }
        }
        #endregion
    }
}