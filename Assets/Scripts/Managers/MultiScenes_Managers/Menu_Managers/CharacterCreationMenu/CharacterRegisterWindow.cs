using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class CharacterRegisterWindow : MonoBehaviour
    {
        [Header("Window (Drops).")]
        public CanvasGroup _windowGroup;
        public Canvas _windowCanvas;

        [Header("Buttons (Drops).")]
        public Button _clearButton;
        public Button _finishButton;

        [Header("Input Field (Drops).")]
        public TMP_InputField _nameInpField;

        [Header("Window Tween.")]
        public LeanTweenType windowEaseType;
        public float windowFadeSpeed;

        [Header("Refs.")]
        [ReadOnlyInspector, SerializeField] InputManager _inp;

        [Header("Status.")]
        [ReadOnlyInspector] public bool canBeFinish;

        #region Privates.
        string nonValideCharacters;
        #endregion

        #region Buttons.
        public void Finish_UIButton()
        {
            OnFinish_SetCharacterName();

            OnFinish_HideRegistWindow();

            OnFinish_ShowInstructionMenu();

            void OnFinish_SetCharacterName()
            {
                _inp._states.statsHandler.characterName = _nameInpField.text;

                ClearField();
            }

            void OnFinish_HideRegistWindow()
            {
                HideRegistWindow();
            }

            void OnFinish_ShowInstructionMenu()
            {
                _inp.SetIsInCharRegistWindowStatus(false);
            }
        }

        public void Clear_UIButton()
        {
            ClearField();
            SetCanBeFinishToFalse();
            DisableButtons();
        }

        void ClearField()
        {
            _nameInpField.text = "";
        }
        #endregion

        #region Show / Hide Regist Window.
        public void ShowRegistWindow()
        {
            EnableCanvas();
            LeanTween.alphaCanvas(_windowGroup, 1, windowFadeSpeed).setEase(windowEaseType);
        }

        void HideRegistWindow()
        {
            LeanTween.alphaCanvas(_windowGroup, 0, windowFadeSpeed).setEase(windowEaseType).setOnComplete(DisableCanvas);
        }
        
        void EnableCanvas()
        {
            _windowCanvas.enabled = true;
        }

        void DisableCanvas()
        {
            _windowCanvas.enabled = false;
        }
        #endregion

        #region On Input Field Value Chanaged.
        public void On_InputField_Changed()
        {
            if (_nameInpField.text == "")
            {
                if (canBeFinish)
                {
                    SetCanBeFinishToFalse();
                    DisableButtons();
                }
            }
            else
            {
                if (!canBeFinish)
                {
                    SetCanBeFinishToTrue();
                    EnableButtons();
                }
            }
        }
        #endregion

        #region Can Be Finish Status.
        public void SetCanBeFinishToTrue()
        {
            canBeFinish = true;
        }

        void SetCanBeFinishToFalse()
        {
            canBeFinish = false;
        }
        #endregion

        #region Enable / Disable Buttons.
        void EnableButtons()
        {
            _clearButton.interactable = true;
            _finishButton.interactable = true;
        }

        void DisableButtons()
        {
            _clearButton.interactable = false;
            _finishButton.interactable = false;
        }
        #endregion

        #region Valid Characters.
        char ValidateChar(char addedChar)
        {
            if (nonValideCharacters.IndexOf(addedChar) != -1)
            {
                return '\0';
            }
            else
            {
                return addedChar;
            }
        }
        #endregion

        #region Setup.
        public void Setup(InputManager inp)
        {
            _inp = inp;

            SetupValidCharacters();
            SetCanBeFinishToFalse();
        }

        void SetupValidCharacters()
        {
            nonValideCharacters = " '/\"$";

            _nameInpField.onValidateInput = delegate(string input, int charIndex, char addedChar) { return ValidateChar(addedChar); };
        }
        #endregion
    }
}