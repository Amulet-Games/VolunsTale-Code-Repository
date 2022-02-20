using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace SA
{
    public class LevelingMenuManager : MonoBehaviour, IMenuManagerUpdatable
    {
        [Header("Menu Tween.")]
        public LeanTweenType _menuEaseType;
        public float _menuFadeInSpeed = 1;
        public float _menuFadeOutSpeed = 1;

        [Header("SideHub Tween.")]
        public LeanTweenType sideHubEaseType;
        public float sideHubFadeInSpeed = 1;
        public float sideHubFadeOutSpeed = 1;

        [Header("Popup Tween.")]
        public LeanTweenType popupEaseType;
        public float popupFadeInSpeed = 1;
        public float popupFadeOutSpeed = 1;

        [Header("Group.")]
        [SerializeField] CanvasGroup levelingMenuGroup = null;
        [SerializeField] CanvasGroup sideHubGroup = null;
        [SerializeField] CanvasGroup PopupGroup = null;

        [Header("Hubs.")]
        public LevelingHub levelingHub;
        public DefinitionHub definitionHub;
        public PreviewHub previewHub;

        [Header("Messages.")]
        [SerializeField] ConfirmLevelupMessage confirmingMessage;
        [SerializeField] PopupMessage quittingMessage;
        I_LvlMenu_MessageUpdatable _iMessageUpdatable;

        [Header("Fonts.")]
        public Color _beforeChangesFontColor;
        public Color _afterChangesFontColor;

        [Header("Buttons.")]
        public Color _unavaliableAttriButtonColor;
        
        [Header("Options Status.")]
        [ReadOnlyInspector] public bool isShowSideHub;
        [ReadOnlyInspector] public bool isSwitchPreviewGroup;
        [ReadOnlyInspector] public bool isInMessage;

        [Header("Manager Refs.")]
        [ReadOnlyInspector] public InputManager _inp;
        public PointerEventData _pointerEventData;
        
        [Header("Canvas Refs.")]
        Canvas _levelingMenuCanvas;
        Canvas _sideHubCanvas;
        Canvas _popupCanvas;

        int _cur_levelingMenuTweenId;
        int _cur_sideHubTweenId;
        int _cur_popupTweenId;

        public static LevelingMenuManager singleton;
        private void Awake()
        {
            if (singleton == null)
                singleton = this;
            else
                Destroy(this);
        }

        public void Setup(InputManager _inp)
        {
            this._inp = _inp;
            
            SetupCanvas();

            SetupPointerEvent();

            SetupLevelingHub();

            SetupPreviewHub();

            SetupMessages();
        }

        public void Tick()
        {
            if (!isInMessage)
            {
                if (_inp.menu_switch_input)
                {
                    SwitchIsSwitchPreviewGroupStatus();
                }
                else if (_inp.menu_hide_input)
                {
                    SwitchIsShowSideHubStatus();
                }
                else if (_inp.menu_quit_input)
                {
                    SetIsInQuittingStateToTrue();
                }

                levelingHub.Tick();
            }
            else
            {
                if (_inp.menu_quit_input)
                {
                    SetIsInMessageStateToFalse();
                }

                _iMessageUpdatable.Tick();
            }
        }
        
        #region Show / Hide Leveling Menu Manager.
        public void ShowLevelingMenu()
        {
            CancelUnFinishTweeningJob();

            _levelingMenuCanvas.enabled = true;
            _cur_levelingMenuTweenId = LeanTween.alphaCanvas(levelingMenuGroup, 1, _menuFadeInSpeed).setEase(_menuEaseType).id;

            OnShowMenuCanvas();
        }

        void OnShowMenuCanvas()
        {
            SetIsShowSideHubStatus(true);

            levelingHub.ResetHubOnMenuOpen();
            previewHub.OnPreviewHubOpen();
        }

        public void HideLevelingMenu()
        {
            CancelUnFinishTweeningJob();

            _cur_levelingMenuTweenId = LeanTween.alphaCanvas(levelingMenuGroup, 0, _menuFadeOutSpeed).setEase(_menuEaseType).setOnComplete(OnCompleteHideMenuCanvas).id;
        }

        void OnCompleteHideMenuCanvas()
        {
            _levelingMenuCanvas.enabled = false;

            levelingHub.ResetHubOnMenuClose();
            previewHub.OnPreviewHubClose();
        }

        void CancelUnFinishTweeningJob()
        {
            if (LeanTween.isTweening(_cur_levelingMenuTweenId))
                LeanTween.cancel(_cur_levelingMenuTweenId);
        }
        #endregion

        #region Show / Hide SideHub.
        void ShowSideHubGroup()
        {
            CancelUnFinishSideHubTweeningJob();

            _sideHubCanvas.enabled = true;
            _cur_sideHubTweenId = LeanTween.alphaCanvas(sideHubGroup, 1, sideHubFadeInSpeed).setEase(sideHubEaseType).id;
        }

        void HideSideHubGroup()
        {
            CancelUnFinishSideHubTweeningJob();

            _cur_sideHubTweenId = LeanTween.alphaCanvas(sideHubGroup, 0, sideHubFadeOutSpeed).setEase(sideHubEaseType).setOnComplete(OnCompleteHideSideHubCanvas).id;
        }

        void OnCompleteHideSideHubCanvas()
        {
            _sideHubCanvas.enabled = false;
        }

        void CancelUnFinishSideHubTweeningJob()
        {
            if (LeanTween.isTweening(_cur_sideHubTweenId))
                LeanTween.cancel(_cur_sideHubTweenId);
        }
        #endregion

        #region Show / Hide Popup.
        void ShowPopupGroup()
        {
            CancelUnFinishPopupTweeningJob();

            _popupCanvas.enabled = true;
            _cur_popupTweenId = LeanTween.alphaCanvas(PopupGroup, 1, popupFadeInSpeed).setEase(popupEaseType).id;
        }

        void HidePopupGroup()
        {
            CancelUnFinishPopupTweeningJob();

            _cur_popupTweenId = LeanTween.alphaCanvas(PopupGroup, 0, popupFadeOutSpeed).setEase(popupEaseType).setOnComplete(OnCompleteHidePopupCanvas).id;
        }

        void OnCompleteHidePopupCanvas()
        {
            _popupCanvas.enabled = false;
        }

        void CancelUnFinishPopupTweeningJob()
        {
            if (LeanTween.isTweening(_cur_popupTweenId))
                LeanTween.cancel(_cur_popupTweenId);
        }
        #endregion

        #region Set Status.
        void SetIsShowSideHubStatus(bool _isShowSideHub)
        {
            if (_isShowSideHub)
            {
                isShowSideHub = true;
                ShowSideHubGroup();
            }
            else
            {
                isShowSideHub = false;
                HideSideHubGroup();
            }
        }

        void SwitchIsShowSideHubStatus()
        {
            isShowSideHub = !isShowSideHub;
            if (isShowSideHub)
            {
                ShowSideHubGroup();
            }
            else
            {
                HideSideHubGroup();
            }
        }

        public void SetIsSwitchPreviewGroupStatus(bool _isSwitchPreviewGroup)
        {
            if (_isSwitchPreviewGroup)
            {
                isSwitchPreviewGroup = true;
                previewHub.ShowSecondPreviewGroup();
            }
            else
            {
                isSwitchPreviewGroup = false;
                previewHub.ShowFirstPreviewGroup();
            }
        }

        void SwitchIsSwitchPreviewGroupStatus()
        {
            isSwitchPreviewGroup = !isSwitchPreviewGroup;
            levelingHub._currentAttributePreview.ResetOnPreviewGroupSwitch(isSwitchPreviewGroup);

            if (isSwitchPreviewGroup)
            {
                previewHub.ShowSecondPreviewGroup();
            }
            else
            {
                previewHub.ShowFirstPreviewGroup();
            }
        }

        public void SetIsInConfirmingStateToTrue()
        {
            isInMessage = true;

            _iMessageUpdatable = confirmingMessage;
            _iMessageUpdatable.OnOpenMessageReset();

            ShowPopupGroup();
        }

        public void SetIsInQuittingStateToTrue()
        {
            isInMessage = true;

            _iMessageUpdatable = quittingMessage;
            _iMessageUpdatable.OnOpenMessageReset();

            ShowPopupGroup();
        }

        public void SetIsInMessageStateToFalse()
        {
            isInMessage = false;

            _iMessageUpdatable.OnCloseMessageReset();

            HidePopupGroup();
        }
        #endregion

        #region Setup.
        void SetupCanvas()
        {
            _levelingMenuCanvas = levelingMenuGroup.GetComponent<Canvas>();
            _sideHubCanvas = sideHubGroup.GetComponent<Canvas>();
            _popupCanvas = PopupGroup.GetComponent<Canvas>();
        }

        void SetupPointerEvent()
        {
            _pointerEventData = _inp._pointerEventData;
        }

        void SetupLevelingHub()
        {
            levelingHub.Setup();
        }

        void SetupPreviewHub()
        {
            previewHub.Setup();
        }

        void SetupMessages()
        {
            confirmingMessage.ConfirmLevelupMessageSetup();
            quittingMessage.Setup();
        }
        #endregion

        /// On Confirm Level Up.
        public void OnConfirmLevelup()
        {
            SetIsInMessageStateToFalse();
            levelingHub.ResetHubOnLevelup();
            previewHub.OnPreviewHubOpen();
        }

        /// ON DEATH.
        public void OnDeathOffMenuManager()
        {
            //_inp.SetIsInLevelingMenuStatus(false);
        }
    }

    public interface I_LvlMenu_MessageUpdatable
    {
        void Tick();

        void OnOpenMessageReset();

        void OnCloseMessageReset();
    }
}