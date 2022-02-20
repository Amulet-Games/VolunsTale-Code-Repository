using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public class EquipmentMenuManager : MonoBehaviour, IMenuManagerUpdatable
    {
        #region Meun Tween
        [Header("Menu Tween.")]
        public LeanTweenType _menuEaseType;
        public float _menuFadeInSpeed = 1;
        public float _menuFadeInDelayTime = 0.75f;
        public float _menuFadeOutSpeed = 1;
        #endregion

        #region SideHub Tween
        [Header("SideHub Tween")]
        public LeanTweenType sideHubEaseType;
        public float sideHubFadeInSpeed = 1;
        public float sideHubFadeOutSpeed = 1;
        #endregion

        #region EquipSlot Tween.
        [Header("EquipSlot Tween.")]
        public LeanTweenType _equipSlot_EaseType = LeanTweenType.easeOutSine;
        public float _equipSlot_HighlightFadeSpeed = 1;
        public float _equipSlot_PingPongMinValue = 0.2f;
        public float _equipSlot_PingPongMaxValue = 0.6f;
        #endregion

        #region ReviewSlot Tween.
        [Header("ReviewSlot Tween.")]
        public LeanTweenType _reviewSlot_EaseType = LeanTweenType.pingPong;
        public float _reviewSlot_HighlightFadeSpeed = 1;
        public float _reviewSlot_PingPongMinValue = 0.2f;
        public float _reviewSlot_PingPongMaxValue = 0.6f;
        #endregion

        #region Equipment Menu Drops.
        [Header("Equipment Menu (Drops).")]
        [SerializeField] CanvasGroup equipmentMenuGroup = null;
        [SerializeField] Canvas _equipmentMenuCanvas;
        #endregion

        #region Hubs.
        [Header("Hubs (Drops).")]
        [SerializeField] CanvasGroup sideHubGroup = null;
        [SerializeField] Canvas _sideHubCanvas;

        [Space(10)]
        public MainHub mainHub;
        public ItemHub itemHub;
        public StatusHub statusHub;
        #endregion

        #region Status.
        [Header("Status.")]
        [ReadOnlyInspector] public bool isInEquipHub;
        [ReadOnlyInspector] public bool isShowSideHub;
        [ReadOnlyInspector, SerializeField] bool isSwitchPlayerStatusHub;
        #endregion

        #region Refs.
        [Header("Refs.")]
        [ReadOnlyInspector] public InputManager _inp;
        [ReadOnlyInspector] public ItemEquipSlot _currentItemEquipSlot;
        [ReadOnlyInspector] public ItemReviewSlot _currentItemReviewSlot;
        #endregion

        #region Privates.
        int _cur_equipmentMenuTweenId;
        int _cur_sideHubMenuTweenId;
        [HideInInspector] public int _cur_equipSlotTweenId;
        [HideInInspector] public int _cur_reviewSlotTweenId;
        [HideInInspector] public bool _isLoopComplete;
        #endregion
        
        public void Tick()
        {
            if (_inp.menu_switch_input)
            {
                SwitchIsSwitchPlayerStatusHubStatus();
            }
            else if (_inp.menu_hide_input)
            {
                SwitchIsShowSideHubStatus();
            }

            mainHub.Tick();
            statusHub.Tick();
        }

        #region SHOW / HIDE EQUIPMENT MENU.
        public void ShowEquipmentMenu()
        {
            CancelUnFinishEquipmentMenuTweeningJob();

            _equipmentMenuCanvas.enabled = true;
            _cur_equipmentMenuTweenId = LeanTween.alphaCanvas(equipmentMenuGroup, 1, _menuFadeInSpeed).setDelay(_menuFadeInDelayTime).setEase(_menuEaseType).id;

            OnShowMenuCanvas();
        }

        void OnShowMenuCanvas()
        {
            SetIsShowSideHubStatus(true);
            mainHub.ResetHubOnMenuOpen();
            statusHub.ResetHubOnMenuOpen();
        }

        public void HideEquipmentMenu()
        {
            CancelUnFinishEquipmentMenuTweeningJob();

            _cur_equipmentMenuTweenId = LeanTween.alphaCanvas(equipmentMenuGroup, 0, _menuFadeOutSpeed).setEase(_menuEaseType).setOnComplete(OnCompleteHideMenuCanvas).id;
        }

        void OnCompleteHideMenuCanvas()
        {
            _equipmentMenuCanvas.enabled = false;

            mainHub.ResetHubOnMenuClose();
            itemHub.ResetHubOnMenuClose();
            statusHub.ResetHubOnMenuClose();
        }

        void CancelUnFinishEquipmentMenuTweeningJob()
        {
            if (LeanTween.isTweening(_cur_equipmentMenuTweenId))
                LeanTween.cancel(_cur_equipmentMenuTweenId);
        }
        #endregion

        #region SHOW / HIDE SIDEHUB.
        void ShowSideHubCanvas()
        {
            CancelUnFinishSideHubTweeningJob();

            _sideHubCanvas.enabled = true;
            _cur_sideHubMenuTweenId = LeanTween.alphaCanvas(sideHubGroup, 1, sideHubFadeInSpeed).setEase(sideHubEaseType).id;
        }

        void HideSideHubCanvas()
        {
            CancelUnFinishSideHubTweeningJob();

            _cur_sideHubMenuTweenId = LeanTween.alphaCanvas(sideHubGroup, 0, sideHubFadeOutSpeed).setEase(sideHubEaseType).setOnComplete(OnCompleteHideSideHubCanvas).id;
        }

        void OnCompleteHideSideHubCanvas()
        {
            _sideHubCanvas.enabled = false;
        }

        void CancelUnFinishSideHubTweeningJob()
        {
            if (LeanTween.isTweening(_cur_sideHubMenuTweenId))
                LeanTween.cancel(_cur_sideHubMenuTweenId);
        }
        #endregion

        #region Set Bool Status.
        public void OpenEquipHub()
        {
            mainHub.OnEquipHub();
        }

        public void OpenReviewHub()
        {
            mainHub.OnReviewHub();
        }

        void SetIsShowSideHubStatus(bool _isShowSideHub)
        {
            if (_isShowSideHub)
            {
                isShowSideHub = true;
                ShowSideHubCanvas();
            }
            else
            {
                isShowSideHub = false;
                HideSideHubCanvas();
            }
        }

        void SwitchIsShowSideHubStatus()
        {
            isShowSideHub = !isShowSideHub;
            if (isShowSideHub)
            {
                ShowSideHubCanvas();
            }
            else
            {
                HideSideHubCanvas();
            }
        }

        public void SetIsSwitchPlayerStatusHubStatus(bool _isSwitchPlayerStatusHub)
        {
            if (_isSwitchPlayerStatusHub)
            {
                isSwitchPlayerStatusHub = true;
                statusHub.OnSecondStatusHub();
            }
            else
            {
                isSwitchPlayerStatusHub = false;
                statusHub.OnFirstStatusHub();
            }
        }

        void SwitchIsSwitchPlayerStatusHubStatus()
        {
            isSwitchPlayerStatusHub = !isSwitchPlayerStatusHub;
            if (isSwitchPlayerStatusHub)
            {
                statusHub.OnSecondStatusHub();
            }
            else
            {
                statusHub.OnFirstStatusHub();
            }
        }
        #endregion

        #region On / Off EquipSlot Highlighters.

        #region Equip Slot.
        public void EquipSlot_HighlighterTick()
        {
            if (_isLoopComplete)
            {
                _cur_equipSlotTweenId = LeanTween.alpha(_currentItemEquipSlot._highlighterRect, _equipSlot_PingPongMinValue, _equipSlot_HighlightFadeSpeed).setLoopPingPong(1).setEase(_equipSlot_EaseType).setOnComplete(RequestNewPingPongLoop).id;
                _isLoopComplete = false;
            }
        }

        public void EquipSlot_CancelHighlighter()
        {
            LeanTween.cancel(_cur_equipSlotTweenId);
            LeanTween.alpha(_currentItemEquipSlot._highlighterRect, _equipSlot_PingPongMaxValue, 0);
        }
        #endregion

        #region Review Slot.
        public void ReviewSlot_HighlighterTick()
        {
            if (_isLoopComplete)
            {
                _cur_reviewSlotTweenId = LeanTween.alpha(_currentItemReviewSlot._highlighterRect, _reviewSlot_PingPongMinValue, _reviewSlot_HighlightFadeSpeed).setLoopPingPong(1).setEase(_reviewSlot_EaseType).setOnComplete(RequestNewPingPongLoop).id;
                _isLoopComplete = false;
            }
        }

        public void ReviewSlot_CancelHighlighter()
        {
            LeanTween.cancel(_cur_reviewSlotTweenId);
            LeanTween.alpha(_currentItemReviewSlot._highlighterRect, _reviewSlot_PingPongMaxValue, 0);
        }
        #endregion

        public void RequestNewPingPongLoop()
        {
            _isLoopComplete = true;
        }
        #endregion

        #region Setup.
        public void Setup(InputManager _inp)
        {
            this._inp = _inp;
            
            SetupMainHub();

            SetupItemHub();

            SetupStatusHub();
        }
        
        void SetupMainHub()
        {
            mainHub.Setup(this);
        }

        void SetupItemHub()
        {
            itemHub.Setup(this);
        }

        void SetupStatusHub()
        {
            statusHub.Setup(this);
        }
        #endregion

        /// ON DEATH.
        public void OnDeathOffMenuManager()
        {
            _inp.SetIsInEquipmentMenuStatus(false);
        }
    }
}