using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UISoftMask;
using TMPro;
using Random = UnityEngine.Random;

namespace SA
{
    public class LoadingScreenHandler : MonoBehaviour
    {
        #region Opening Mask.
        [Header("Opening Mask Config.")]
        public LeanTweenType _openingMaskEaseType = LeanTweenType.easeInCirc;
        public float _onTitleScreenMaskFadeSpeed = 1.5f;
        public float _onLevelMaskFadeSpeed = 0.95f;
        public SoftMask _openingSoftMask;
        public Canvas _openingSoftMaskCanvas;
        #endregion

        [Header("Backgrounds.")]
        public Image _loadingBackgroundImage;
        public Sprite[] _loadingBackgrounds = new Sprite[5];

        [Header("Tips.")]
        [ReadOnlyInspector] public GameObject _currentTipsContent;
        public GameObject[] _TipsContents;

        [Header("Slider.")]
        public Slider _loadingSlider;
        [ReadOnlyInspector, SerializeField] float _loadingSliderMaxValue;
        [ReadOnlyInspector] public int _asyncJobsAmount;

        [Header("Slider Text.")]
        public TMP_Text _loadingText;

        [Header("Loading Screen Tween Config.")]
        public CanvasGroup _loadingScreenGroup;
        public LeanTweenType _loadingScreenEaseType;
        public float _loadingScreenFadeSpeed;

        [Header("Refs.")]
        [ReadOnlyInspector] public SessionManager _sessionManager;
        [ReadOnlyInspector] public Canvas _loadingScreenCanvas;

        public event Action _asyncActionToLoad;

        public static LoadingScreenHandler singleton;
        void Awake()
        {
            if (singleton != null)
                Destroy(this);
            else
                singleton = this;
        }

        private void Start()
        {
            SetupReferences();
            SetupCanvas();
            SetupSlider();
        }

        #region Show / Hide Loading Screen.
        public void ShowLoadingScreen(int _asyncJobsAmount, string _loadingText, Action _targetAsyncAction)
        {
            _asyncActionToLoad = _targetAsyncAction;

            RefreshBackgroundImage();
            ActivateRandomTipsContent();
            this._loadingText.text = _loadingText;

            OnShowLoadingSlider(_asyncJobsAmount);
            FadeInLoadingScreen();

            void RefreshBackgroundImage()
            {
                _loadingBackgroundImage.sprite = GetRandomLoadingBackground();
            }
        }
        
        public void HideLoadingScreen(bool _autoOpenMask)
        {
            QuickShowOpeningMask();
            QuickHideLoadingScreen();
            DeactivateCurrentTipsContent();

            if (_autoOpenMask)
                OnLevelFadeOutOpeningMask();
        }

        #region Refresh Loading Screen.
        public void RefreshLoadingScreen()
        {
            DecrementAsyncJobsAmount();
        }

        public void RefreshLoadingScreen(float _targetLoadingValue)
        {
            DecrementAsyncJobsAmount();
            _loadingSlider.value = _targetLoadingValue;
        }

        public void RefreshLoadingScreen(string _targetLoadingText)
        {
            DecrementAsyncJobsAmount();
            _loadingText.text = _targetLoadingText;
        }

        public void RefreshLoadingScreen(float _targetLoadingValue, string _targetLoadingText)
        {
            DecrementAsyncJobsAmount();
            _loadingSlider.value = _targetLoadingValue;
            _loadingText.text = _targetLoadingText;
        }
        #endregion

        #endregion

        #region Tween Loading Screen.
        void FadeInLoadingScreen()
        {
            EnableLoadingScreen();
            LeanTween.alphaCanvas(_loadingScreenGroup, 1, _loadingScreenFadeSpeed).setEase(_loadingScreenEaseType).setOnComplete(OnCompleteFadeInLoadingScreen);
        }

        void OnCompleteFadeInLoadingScreen()
        {
            if (_asyncActionToLoad != null)
            {
                _asyncActionToLoad.Invoke();
                _asyncActionToLoad = null;
            }
        }

        void FadeOutLoadingScreen()
        {
            LeanTween.alphaCanvas(_loadingScreenGroup, 0, _loadingScreenFadeSpeed).setEase(_loadingScreenEaseType).setOnComplete(DisableLoadingScreen);
        }

        void QuickHideLoadingScreen()
        {
            _loadingScreenGroup.alpha = 0;
            DisableLoadingScreen();
        }

        public void EnableLoadingScreen()
        {
            _loadingScreenCanvas.enabled = true;
        }

        public void DisableLoadingScreen()
        {
            _loadingScreenCanvas.enabled = false;
        }
        #endregion

        #region Loading Slider.
        public void OnUpdatingLoadingSlider(float _progress)
        {
            float _curLoadingSliderValue = _loadingSlider.value;
            float _progressLimit = _loadingSliderMaxValue / _asyncJobsAmount;

            if (_curLoadingSliderValue < _progressLimit)
            {
                _curLoadingSliderValue += _progress / _asyncJobsAmount;
                _curLoadingSliderValue = _curLoadingSliderValue > _progressLimit ? _progressLimit : _curLoadingSliderValue;
                _loadingSlider.value = _curLoadingSliderValue;
            }
        }

        public void SetLoadingSliderValueImmidiate(float _progress)
        {
            _loadingSlider.value = _progress;
        }

        void DecrementAsyncJobsAmount()
        {
            _asyncJobsAmount--;
        }
        
        void OnShowLoadingSlider(int _asyncJobsAmount)
        {
            this._asyncJobsAmount = _asyncJobsAmount;
            ResetLoadingSliderValue();
        }
        
        void ResetLoadingSliderValue()
        {
            _loadingSlider.value = 0;
        }
        #endregion
        
        #region Opening Mask Tween.
        public void QuickShowOpeningMask()
        {
            _openingSoftMask.gameObject.SetActive(true);
            _openingSoftMaskCanvas.enabled = true;
            _openingSoftMask.alpha = 0;
        }

        public void OnTitleScreenFadeOutOpeningMask()
        {
            LeanTween.value(_openingSoftMask.alpha, 1, _onTitleScreenMaskFadeSpeed).setEase(_openingMaskEaseType).setOnUpdate((value) => _openingSoftMask.alpha = value);
        }
        
        public void OnLevelFadeOutOpeningMask()
        {
            LeanTween.value(_openingSoftMask.alpha, 1, _onLevelMaskFadeSpeed).setEase(_openingMaskEaseType).setOnUpdate((value) => _openingSoftMask.alpha = value).setOnComplete(DisableOpeningMask);
        }

        public void OnReviveFadeOutOpeningMask()
        {
            LeanTween.value(_openingSoftMask.alpha, 1, _onLevelMaskFadeSpeed).setEase(_openingMaskEaseType).setOnUpdate((value) => _openingSoftMask.alpha = value);
        }

        public void DisableOpeningMask()
        {
            _openingSoftMask.gameObject.SetActive(false);
            _openingSoftMaskCanvas.enabled = false;
        }
        #endregion
        
        #region Setup.
        void SetupReferences()
        {
            _sessionManager = SessionManager.singleton;
            _sessionManager._loadingScreenHandler = this;
        }

        void SetupCanvas()
        {
            _loadingScreenCanvas = GetComponent<Canvas>();
            DisableLoadingScreen();
        }

        void SetupSlider()
        {
            _loadingSlider.maxValue = 1;
            _loadingSliderMaxValue = 0.9f;
            _loadingSlider.value = 0;
        }
        #endregion

        private Sprite GetRandomLoadingBackground()
        {
            return _loadingBackgrounds[Random.Range(0, _loadingBackgrounds.Length)];
        }

        private void ActivateRandomTipsContent()
        {
            _currentTipsContent = _TipsContents[Random.Range(0, _TipsContents.Length)];
            _currentTipsContent.SetActive(true);
        }

        private void DeactivateCurrentTipsContent()
        {
            _currentTipsContent.SetActive(false);
        }
    }
}