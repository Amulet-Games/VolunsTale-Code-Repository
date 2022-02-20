using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public class CheckpointMenuManager : MonoBehaviour, IMenuManagerUpdatable
    {
        #region Fade Tween.
        [Header("Fade Tween.")]
        public LeanTweenType _menuFadeEaseType;
        public float _menuFadeInSpeed = 1;
        public float _menuFadeOutSpeed = 1;
        #endregion

        #region Flip Tween.
        [Header("Flip Tween.")]
        public LeanTweenType _menuFlippedEaseType;
        public float _menuFlipSpeed = 0.25f;
        #endregion

        #region Option Selector Tween.
        [Header("Option Selector Tween.")]
        public LeanTweenType _selectorEaseType = LeanTweenType.easeOutCirc;
        public float _selectorFadeInSpeed = 0.15f;
        public float _selectorFadeOutSpeed = 0.15f;
        #endregion

        #region Options.
        [Header("Options")]
        public BaseCheckpointOption[] checkpointOptions = new BaseCheckpointOption[4];
        #endregion

        #region Checkpoint Menu Drops.
        [Header("Checkpoint Menu (Drops).")]
        [SerializeField] CanvasGroup checkpointMenuGroup;
        [SerializeField] Canvas checkpointMenuCanvas;
        [SerializeField] RectTransform checkpointMenuRect;
        #endregion

        #region Text.
        [Header("Checkpoint Location Text.")]
        public TMP_Text _locationText;
        #endregion

        #region Status.
        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] int optionIndex;
        [ReadOnlyInspector, SerializeField] int optionsLength;
        [ReadOnlyInspector, SerializeField] bool _isFlipMenu;
        [ReadOnlyInspector] public bool isCursorOverOption;
        #endregion

        #region Refs.
        [Header("Refs.")]
        [ReadOnlyInspector, SerializeField] BaseCheckpointOption _currentOption;
        [ReadOnlyInspector, SerializeField] InputManager inp;
        [ReadOnlyInspector, SerializeField] StateManager states;
        #endregion
        
        #region Private.
        int _cur_menuFadeTweenId;
        int _cur_menuFlipTweenId;
        Vector3 _menuInitPos;
        #endregion

        public static CheckpointMenuManager singleton;
        private void Awake()
        {
            if (singleton == null)
                singleton = this;
            else
                Destroy(this);
        }

        #region Show / Hide Menu.
        public void ShowCheckpointMenu()
        {
            CheckUnFinishTweeningJob();

            EnableCanvas();
            OnMenuOpen();
            _cur_menuFadeTweenId = LeanTween.alphaCanvas(checkpointMenuGroup, 1, _menuFadeInSpeed).setEase(_menuFadeEaseType).id;
        }

        public void HideCheckpointMenu()
        {
            CheckUnFinishTweeningJob();
            ResetOnMenuClose();
            _cur_menuFadeTweenId = LeanTween.alphaCanvas(checkpointMenuGroup, 0, _menuFadeOutSpeed).setEase(_menuFadeEaseType).setOnComplete(DisableCanvas).id;
        }

        void CheckUnFinishTweeningJob()
        {
            if (LeanTween.isTweening(_cur_menuFadeTweenId))
                LeanTween.cancel(_cur_menuFadeTweenId);
        }
        #endregion
        
        #region Enable / Disable Canvas.
        void EnableCanvas()
        {
            checkpointMenuCanvas.enabled = true;
        }

        void DisableCanvas()
        {
            checkpointMenuCanvas.enabled = false;
        }
        #endregion

        #region On Menu Open / Close.
        void OnMenuOpen()
        {
            isCursorOverOption = false;

            optionIndex = 0;
            _currentOption = checkpointOptions[optionIndex];
            _currentOption.OnCurrentOption();

            _isFlipMenu = false;
            checkpointMenuRect.localPosition = _menuInitPos;

            _locationText.text = states._currentSpawnPoint.spawnName;
        }

        void ResetOnMenuClose()
        {
            _currentOption.OffCurrentOption();
        }
        #endregion

        #region Tick.
        public void Tick()
        {
            GetCurrentOptionByInput();
            
            SelectCurrentOptionByInput();

            SelectCurrentOptionByCursor();

            FlipMenuByInput();
        }

        void GetCurrentOptionByInput()
        {
            if (inp.menu_down_input)
            {
                optionIndex++;
                optionIndex = (optionIndex == optionsLength) ? 0 : optionIndex;

                SetCurrentOption();
            }
            else if (inp.menu_up_input)
            {
                optionIndex--;
                optionIndex = (optionIndex < 0) ? optionsLength - 1 : optionIndex;

                SetCurrentOption();
            }
        }

        public void GetCurrentOptionByPointerEvent(BaseCheckpointOption _targetSlot)
        {
            isCursorOverOption = true;
            if (_currentOption != _targetSlot)
            {
                optionIndex = _targetSlot._optionNumber;
                SetCurrentOption();
            }
        }
        
        void SetCurrentOption()
        {
            _currentOption.OffCurrentOption();
            _currentOption = checkpointOptions[optionIndex];
            _currentOption.OnCurrentOption();
        }

        void SelectCurrentOptionByInput()
        {
            if (inp.menu_select_input)
            {
                _currentOption.OnSelectOption();
            }
        }

        void SelectCurrentOptionByCursor()
        {
            if (inp.menu_select_mouse)
            {
                if (isCursorOverOption)
                {
                    _currentOption.OnSelectOption();
                }
            }
        }
        
        void FlipMenuByInput()
        {
            if (inp.menu_switch_input)
            {
                _isFlipMenu = !_isFlipMenu;
                if (_isFlipMenu)
                {
                    FlipMenuFromInitPos();
                }
                else
                {
                    FlipMenuFromTargetPos();
                }
            }
        }
        #endregion

        #region Flip Menu.
        void FlipMenuFromInitPos()
        {
            CancelUnFinisheFlipJob();

            _cur_menuFlipTweenId = LeanTween.alphaCanvas(checkpointMenuGroup, 0, _menuFlipSpeed).setEase(_menuFlippedEaseType).setOnComplete(OnCompleteMoveMenuToFlippedPos).id;
        }

        void OnCompleteMoveMenuToFlippedPos()
        {
            Vector3 _targetPos = _menuInitPos;
            _targetPos.x *= -1;
            checkpointMenuRect.localPosition = _targetPos;

            _cur_menuFlipTweenId = LeanTween.alphaCanvas(checkpointMenuGroup, 1, _menuFlipSpeed).setEase(_menuFlippedEaseType).id;
        }

        void FlipMenuFromTargetPos()
        {
            CancelUnFinisheFlipJob();
            _cur_menuFlipTweenId = LeanTween.alphaCanvas(checkpointMenuGroup, 0, _menuFlipSpeed).setEase(_menuFlippedEaseType).setOnComplete(OnCompleteMoveMenuToInitPos).id;
        }

        void OnCompleteMoveMenuToInitPos()
        {
            checkpointMenuRect.localPosition = _menuInitPos;

            _cur_menuFlipTweenId = LeanTween.alphaCanvas(checkpointMenuGroup, 1, _menuFlipSpeed).setEase(_menuFlippedEaseType).id;
        }

        void CancelUnFinisheFlipJob()
        {
            if (LeanTween.isTweening(_cur_menuFlipTweenId))
                LeanTween.cancel(_cur_menuFlipTweenId);
        }
        #endregion

        #region On Option Selected.
        public void OnLevelingOptionSelected()
        {
            HideCheckpointMenu();
            states.Set_IsLevelUpBegin_AnimParaToTrue();
        }

        public void OnQuitOptionSelected()
        {
            inp.SetIsInCheckpointMenuStatus(false);
            states.OffCheckpointAgentInteraction();
        }
        #endregion

        #region Setup.
        public void Setup(InputManager _inp)
        {
            inp = _inp;
            states = _inp._states;
            
            SetupMenuInitPos();
            SetupCheckpointOptions();
        }
        
        void SetupMenuInitPos()
        {
            _menuInitPos = checkpointMenuRect.localPosition;
        }
        
        void SetupCheckpointOptions()
        {
            optionsLength = checkpointOptions.Length;
            for (int i = 0; i < optionsLength; i++)
            {
                checkpointOptions[i].Setup(this);
                checkpointOptions[i]._optionNumber = i;
            }
        }
        #endregion

        #region On Death Event.
        public void OnDeathOffMenuManager()
        {
            inp.SetIsInCheckpointMenuStatus(false);
        }
        #endregion
    }
}