using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DualButton_MainMenuMsg : MonoBehaviour
    {
        [Header("Message (Drops).")]
        public Canvas msgCanvas;

        [Header("Buttons (Drops).")]
        public Base_DualButtonMsg_Button _1stButton;
        public Base_DualButtonMsg_Button _2ndButton;
        
        [Header("Button Tween.")]
        public LeanTweenType _buttonPingPongEaseType;
        public float _buttonPingPongFadeTime;
        public float _buttonPingPongFadeMinValue;
        
        [Header("Status.")]
        [ReadOnlyInspector] public bool _isCursorOverSelection;
        [ReadOnlyInspector] public bool _isInQuitButton;

        [Header("Refs.")]
        [ReadOnlyInspector] public Base_DualButtonMsg_Button _currentButton;
        [ReadOnlyInspector] public MainMenuManager _mainMenuManager;

        #region Privates.
        int _buttonTweenId;
        #endregion

        #region Tick.
        public void Tick()
        {
            _mainMenuManager.UpdateInputs_DualButton();

            GetButtonByInput();

            SelectButtonByInput();

            SelectButtonByCursor();
        }
        
        void GetButtonByInput()
        {
            if (_mainMenuManager.menu_left_input || _mainMenuManager.menu_right_input)
            {
                _isInQuitButton = !_isInQuitButton;
                SetCurrentButton();
            }
        }

        // Pointer Event.
        public void GetButtonByPointerEvent(Base_DualButtonMsg_Button _targetButton)
        {
            _isCursorOverSelection = true;
            if (_currentButton != _targetButton)
            {
                _isInQuitButton = _targetButton._is2ndButton;
                SetCurrentButton();
            }
        }

        void SelectButtonByInput()
        {
            if (_mainMenuManager.menu_select_input)
            {
                _currentButton.OnSelectButton();
            } 
        }

        void SelectButtonByCursor()
        {
            if (_isCursorOverSelection)
            {
                if (_mainMenuManager.menu_select_mouse)
                {
                    _currentButton.OnSelectButton();
                }
            }
        }
        #endregion

        #region On Message Open.
        public void OnMessageOpen()
        {
            OnMessageOpen_ResetButtonsColor();

            OnMessageOpen_SetFirstButton();

            OnMessageOpen_ResetCursorStatus();
            
            EnableCanvas();
        }

        void OnMessageOpen_ResetButtonsColor()
        {
            _1stButton._buttonImage.color = _mainMenuManager._fullAlphaColor;
            _2ndButton._buttonImage.color = _mainMenuManager._fullAlphaColor;
        }

        void OnMessageOpen_SetFirstButton()
        {
            _currentButton = _1stButton;
            _isInQuitButton = false;

            OnCurrentButton();
        }

        void OnMessageOpen_ResetCursorStatus()
        {
            _isCursorOverSelection = false;
        }
        #endregion

        #region On Message Close.
        public void OnMessageClose()
        {
            _currentButton.OffCurrentButton();
        }
        #endregion

        #region On Sub Message Close.
        public void OnSubMessageClose()
        {
            _currentButton.OffCurrentButton();
            DisableCanvas();
        }
        #endregion

        #region On / Off Button.
        void SetCurrentButton()
        {
            OffCurrentButton();

            if (_isInQuitButton)
                _currentButton = _2ndButton;
            else
                _currentButton = _1stButton;

            OnCurrentButton();
        }

        void OnCurrentButton()
        {
            _currentButton.OnCurrentButton();
            PingPongFadeOutButton();
        }

        void OffCurrentButton()
        {
            CancelCurrentButtonTween();
            _currentButton.OffCurrentButton();
        }
        #endregion

        #region On / Off Canvas.
        void EnableCanvas()
        {
            msgCanvas.enabled = true;
        }

        public void DisableCanvas()
        {
            msgCanvas.enabled = false;
        }
        #endregion

        #region Ping Pong Button Tween.
        public void CancelCurrentButtonTween()
        {
            LeanTween.cancel(_buttonTweenId);
        }

        void PingPongFadeInButton()
        {
            _buttonTweenId = LeanTween.alpha(_currentButton._buttonRect, 1, _buttonPingPongFadeTime).setEase(_buttonPingPongEaseType).setOnComplete(PingPongFadeOutButton).id;
        }
        
        void PingPongFadeOutButton()
        {
            _buttonTweenId = LeanTween.alpha(_currentButton._buttonRect, _buttonPingPongFadeMinValue, _buttonPingPongFadeTime).setEase(_buttonPingPongEaseType).setOnComplete(PingPongFadeInButton).id;
        }
        #endregion
        
        #region Setup.
        public void Setup(MainMenuManager mainMenuManager)
        {
            _mainMenuManager = mainMenuManager;

            SetupButtons();
        }

        void SetupButtons()
        {
            _2ndButton._dualButtonMsg = this;
            _1stButton._dualButtonMsg = this;
        }
        #endregion
    }
}