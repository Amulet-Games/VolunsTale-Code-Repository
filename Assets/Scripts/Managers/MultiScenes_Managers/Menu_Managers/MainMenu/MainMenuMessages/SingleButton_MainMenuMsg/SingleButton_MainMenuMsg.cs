using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class SingleButton_MainMenuMsg : MonoBehaviour
    {
        [Header("Message (Drops).")]
        public Canvas msgCanvas;

        [Header("Button (Drops).")]
        public Base_SingleButtonMsg_Button okButton;

        [Header("Status.")]
        [ReadOnlyInspector] public bool _isCursorOverSelection;

        [Header("Refs.")]
        [ReadOnlyInspector] public MainMenuManager _mainMenuManager;

        #region Tick.
        public void Tick()
        {
            _mainMenuManager.UpdateInputs_SingleButton();
            
            SelectButtonByInput();

            SelectButtonByCursor();
        }

        void SelectButtonByInput()
        {
            if (_mainMenuManager.menu_select_input)
            {
                okButton.OnSelectButton();
            }
        }

        void SelectButtonByCursor()
        {
            if (_isCursorOverSelection)
            {
                if (_mainMenuManager.menu_select_mouse)
                {
                    okButton.OnSelectButton();
                }
            }
        }
        #endregion

        #region On Message Open.
        public void OnMessageOpen()
        {
            okButton.OnMessageOpen();
            OnMessageOpen_ResetCursorStatus();
            EnableCanvas();
        }

        void OnMessageOpen_ResetCursorStatus()
        {
            _isCursorOverSelection = false;
        }
        #endregion

        #region On / Off Canvas Group.
        void EnableCanvas()
        {
            msgCanvas.enabled = true;
        }

        public void DisableCanvas()
        {
            msgCanvas.enabled = false;
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
            okButton._referedMsg = this;
        }
        #endregion
    }
}