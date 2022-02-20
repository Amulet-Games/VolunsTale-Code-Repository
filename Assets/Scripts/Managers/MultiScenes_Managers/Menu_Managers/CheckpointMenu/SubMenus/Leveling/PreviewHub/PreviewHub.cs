using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class PreviewHub : MonoBehaviour
    {
        [Header("Preview Groups (Drops).")]
        public PreviewGroup firstPreviewGroup;
        public CanvasGroup firstPreviewCanvasGroup;
        public Canvas firstPreviewGroupCanvas;

        [Space(10)]
        public PreviewGroup secondPreviewGroup;
        public CanvasGroup secondPreviewCanvasGroup;
        public Canvas secondPreviewGroupCanvas;

        [Header("Groups Tween.")]
        public LeanTweenType _previewGroupEaseType = LeanTweenType.easeOutExpo;
        public float _previewGroupFadeInSpeed = 0.25f;
        public float _previewGroupFadeOutSpeed = 0.25f;

        [Header("Refs.")]
        [ReadOnlyInspector] public PreviewGroup _currentPreviewGroup;
        [ReadOnlyInspector] public DefinableStatsPreview _currentStatsPreview;

        [Header("Status.")]
        public bool isShowingPreviewSelector;

        #region Private.
        int _cur_previewGroupTweenId;
        #endregion

        #region On Preview Hub Open.
        public void OnPreviewHubOpen()
        {
            RedrawsConfirmedPreviewStats();
            OnMenuOpen_SetFirstDefinablePreview();
        }

        /// Draw: when leveling menu opens, when player pressed confirm at the end.
        void RedrawsConfirmedPreviewStats()
        {
            firstPreviewGroup.RedrawConfirmedStatsPreview();
            secondPreviewGroup.RedrawConfirmedStatsPreview();
        }

        void OnMenuOpen_SetFirstDefinablePreview()
        {
            _currentStatsPreview = firstPreviewGroup._statsPreviews[0];
        }
        #endregion

        #region On Preview Hub Close.
        public void OnPreviewHubClose()
        {
            if (_currentPreviewGroup == secondPreviewGroup)
            {
                LevelingMenuManager.singleton.SetIsSwitchPreviewGroupStatus(false);
            }
        }
        #endregion
        
        #region Pointer Event.
        public void GetCurrentDefinableStatsByCursor(DefinableStatsPreview _targetDefinable)
        {
            if (_currentStatsPreview != _targetDefinable)
            {
                _currentStatsPreview.OffSelector();
                _currentStatsPreview = _targetDefinable;
                _currentStatsPreview.OnStatsPreview();

                isShowingPreviewSelector = true;
            }
        }
        #endregion

        #region Is Showing Preview Selector.
        public void CheckIsShowingPreviewSelector()
        {
            if (isShowingPreviewSelector)
            {
                _currentStatsPreview.OffSelector();
                isShowingPreviewSelector = false;
            }
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            SetupPreviewGroup();
        }
        
        void SetupPreviewGroup()
        {
            firstPreviewGroup.Setup();
            secondPreviewGroup.Setup();

            _currentPreviewGroup = firstPreviewGroup;
            _currentStatsPreview = firstPreviewGroup._statsPreviews[0];
        }
        #endregion

        #region Show / Hide Preview Group.

        #region First.
        public void ShowFirstPreviewGroup()
        {
            _currentPreviewGroup = firstPreviewGroup;

            if (secondPreviewGroupCanvas.enabled)
            {
                HideSecondPreviewGroup(true);
            }
            else
            {
                TweenOnFirstPreviewGroup();
            }
        }

        void OnCompleteTweenOnFirstGroup()
        {
            secondPreviewGroupCanvas.enabled = false;
            TweenOnFirstPreviewGroup();
        }

        void HideFirstPreviewGroup(bool _tweenOnSecondHubOnComplete)
        {
            CancelUnFinishedTweenJob();

            if (!_tweenOnSecondHubOnComplete)
            {
                _cur_previewGroupTweenId = LeanTween.alphaCanvas(firstPreviewCanvasGroup, 0, _previewGroupFadeOutSpeed).setEase(_previewGroupEaseType).setOnComplete(OnCompleteHideFirstGroup).id;
            }
            else
            {
                _cur_previewGroupTweenId = LeanTween.alphaCanvas(firstPreviewCanvasGroup, 0, _previewGroupFadeOutSpeed).setEase(_previewGroupEaseType).setOnComplete(OnCompleteTweenOnSecondGroup).id;
            }
        }

        void OnCompleteHideFirstGroup()
        {
            firstPreviewGroupCanvas.enabled = false;
        }
        #endregion

        #region Second.
        public void ShowSecondPreviewGroup()
        {
            _currentPreviewGroup = secondPreviewGroup;

            if (firstPreviewGroupCanvas.enabled)
            {
                HideFirstPreviewGroup(true);
            }
            else
            {
                TweenOnSecondPreviewGroup();
            }
        }

        void OnCompleteTweenOnSecondGroup()
        {
            firstPreviewGroupCanvas.enabled = false;
            TweenOnSecondPreviewGroup();
        }

        void HideSecondPreviewGroup(bool _tweenOnFirstHubOnComplete)
        {
            CancelUnFinishedTweenJob();

            if (!_tweenOnFirstHubOnComplete)
            {
                _cur_previewGroupTweenId = LeanTween.alphaCanvas(secondPreviewCanvasGroup, 0, _previewGroupFadeOutSpeed).setEase(_previewGroupEaseType).setOnComplete(OnCompleteHideSecondGroup).id;
            }
            else
            {
                _cur_previewGroupTweenId = LeanTween.alphaCanvas(secondPreviewCanvasGroup, 0, _previewGroupFadeOutSpeed).setEase(_previewGroupEaseType).setOnComplete(OnCompleteTweenOnFirstGroup).id;
            }
        }

        void OnCompleteHideSecondGroup()
        {
            secondPreviewGroupCanvas.enabled = false;
        }
        #endregion

        void TweenOnFirstPreviewGroup()
        {
            firstPreviewGroupCanvas.enabled = true;
            _cur_previewGroupTweenId = LeanTween.alphaCanvas(firstPreviewCanvasGroup, 1, _previewGroupFadeInSpeed).setEase(_previewGroupEaseType).id;
        }

        void TweenOnSecondPreviewGroup()
        {
            secondPreviewGroupCanvas.enabled = true;
            _cur_previewGroupTweenId = LeanTween.alphaCanvas(secondPreviewCanvasGroup, 1, _previewGroupFadeInSpeed).setEase(_previewGroupEaseType).id;
        }

        void CancelUnFinishedTweenJob()
        {
            if (LeanTween.isTweening(_cur_previewGroupTweenId))
                LeanTween.cancel(_cur_previewGroupTweenId);
        }
        #endregion
    }
}