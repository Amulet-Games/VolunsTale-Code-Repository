using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public class PopupMessage : MonoBehaviour, I_LvlMenu_MessageUpdatable
    {
        [Header("Popup Buttons.")]
        public PopupButton[] _popupButtons;

        [Header("Button Tween.")]
        public LeanTweenType _buttonEaseType;
        public float _buttonFadeSpeed = 0.9f;
        public float _buttonPingPongMinValue = 0.7f;
        public float _buttonPingPongMaxValue = 1f;

        [Header("Canvas.")]
        public Canvas _messageCanvas;

        [Header("Cursor.")]
        [SerializeField] float cursorEventCheckInterval = 7;
        [ReadOnlyInspector, SerializeField] GraphicRaycaster _graphicRaycaster;
        List<RaycastResult> raycastResult;
        PointerEventData _pointerEventData;

        [Header("Status.")]
        [ReadOnlyInspector] public PopupButton _currentHoveringButton;
        [ReadOnlyInspector] public int _buttonIndex;
        [ReadOnlyInspector] public bool _isHoveringButton;

        [ReadOnlyInspector] public bool _isLoopComplete;
        [ReadOnlyInspector] public int pingPongTweenId;

        [Header("Managers Refs.")]
        [ReadOnlyInspector] public InputManager _inp;
        [ReadOnlyInspector] public LevelingMenuManager _levelingMenuManager;

        [Header("Private.")]
        int _popupButtonsLength;

        #region Tick.
        public void Tick()
        {
            GetCurrentPopupButtonByInput();

            GetCurrentPopupButtonByCursor();

            SetCurrentPopupButton(_popupButtons[_buttonIndex]);

            SelectCurrentButtonByInput();

            SelectCurrentButtonByCursor();

            HoveringButtonTick();
        }

        void GetCurrentPopupButtonByInput()
        {
            if (_inp.menu_right_input)
            {
                _buttonIndex++;
                _buttonIndex = _buttonIndex > _popupButtonsLength - 1 ? 0 : _buttonIndex;
            }
            else if (_inp.menu_left_input)
            {
                _buttonIndex--;
                _buttonIndex = _buttonIndex < 0 ? _popupButtonsLength - 1 : _buttonIndex;
            }
        }

        void GetCurrentPopupButtonByCursor()
        {
            if (Time.frameCount % cursorEventCheckInterval == 0)
            {
                _isHoveringButton = false;
                raycastResult.Clear();

                _pointerEventData.position = _inp.menu_mouse_position;
                _graphicRaycaster.Raycast(_pointerEventData, raycastResult);

                for (int i = 0; i < raycastResult.Count; i++)
                {
                    PopupButton _popupButton = raycastResult[i].gameObject.GetComponent<PopupButton>();
                    if (_popupButton != null)
                    {
                        _buttonIndex = _popupButton._referedButtonIndex;
                        _isHoveringButton = true;
                    }
                }
            }
        }

        void SetCurrentPopupButton(PopupButton _currentHoveringButton)
        {
            if (_currentHoveringButton != this._currentHoveringButton)
            {
                OffCurrentButton();
                this._currentHoveringButton = _currentHoveringButton;
                OnCurrentButton();
            }
        }

        void SelectCurrentButtonByInput()
        {
            if (_inp.menu_select_input)
            {
                SelectCurrentButton();
            }
        }

        void SelectCurrentButtonByCursor()
        {
            if (_inp.menu_select_mouse)
            {
                if (_isHoveringButton)
                {
                    SelectCurrentButton();
                    _isHoveringButton = false;
                }
            }
        }

        void SelectCurrentButton()
        {
            _currentHoveringButton.OnButtonClick(this);
            _levelingMenuManager.SetIsInMessageStateToFalse();
        }
        #endregion

        #region On Open / Close Reset Message.
        public void OnOpenMessageReset()
        {
            _messageCanvas.enabled = true;

            _buttonIndex = 0;
            _currentHoveringButton = _popupButtons[_buttonIndex];
            OnCurrentButton();

            _isHoveringButton = false;
        }

        public void OnCloseMessageReset()
        {
            _messageCanvas.enabled = false;

            OffCurrentButton();
        }
        #endregion

        #region On / Off Button.
        void OnCurrentButton()
        {
            _currentHoveringButton.ChangeButtonSpriteToHovering();
            OnHighlighter();
        }

        void OffCurrentButton()
        {
            _currentHoveringButton.ChangeButtonSpriteToEnable();
            OffHighlighter();
        }
        #endregion

        #region Highlighter.
        void HoveringButtonTick()
        {
            if (_isLoopComplete)
            {
                pingPongTweenId = LeanTween.alpha(_currentHoveringButton._buttonRect, _buttonPingPongMinValue, _buttonFadeSpeed).setLoopPingPong(1).setEase(_buttonEaseType).setOnComplete(RequestNewPingPongLoop).id;
                _isLoopComplete = false;
            }
        }

        void RequestNewPingPongLoop()
        {
            _isLoopComplete = true;
        }

        void OnHighlighter()
        {
            RequestNewPingPongLoop();
        }

        void OffHighlighter()
        {
            CancelHighlighter();
        }

        void CancelHighlighter()
        {
            LeanTween.cancel(pingPongTweenId);
            LeanTween.alpha(_currentHoveringButton._buttonRect, _buttonPingPongMaxValue, 0);
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            SetupReferences();
            SetupPointerEventSystem();
            SetupCanvas();
            SetupButtons();
        }

        void SetupReferences()
        {
            _levelingMenuManager = LevelingMenuManager.singleton;
            _inp = _levelingMenuManager._inp;
        }

        void SetupPointerEventSystem()
        {
            _graphicRaycaster = GetComponent<GraphicRaycaster>();
            _pointerEventData = _levelingMenuManager._pointerEventData;
            raycastResult = new List<RaycastResult>();
        }

        void SetupCanvas()
        {
            _messageCanvas.enabled = false;
        }

        void SetupButtons()
        {
            _popupButtonsLength = _popupButtons.Length;
            for (int i = 0; i < _popupButtonsLength; i++)
            {
                _popupButtons[i].Setup();
            }
        }
        #endregion

        #region virtual.
        public virtual void OnConfirmLevelupButtonClick()
        {
        }
        #endregion
    }
}