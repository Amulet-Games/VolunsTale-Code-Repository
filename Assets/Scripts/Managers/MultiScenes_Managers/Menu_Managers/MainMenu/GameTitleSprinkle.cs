using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class GameTitleSprinkle : MonoBehaviour
    {
        [Header("Enlarge Config.")]
        public LeanTweenType _sprinkleEnlargeEaseType;
        public float _sprinkleEnlargeSpeed;
        public Vector2 _sprinkleMaxSize;
        public Vector2 _sprinkleMinSize;

        [Header("Fade Config.")]
        public LeanTweenType _sprinkleFadeEaseType;
        public float _sprinkleFadeSpeed;
        public float _sprinkleMinFadeValue;

        [Header("Rotate Config.")]
        public float _rotateSpeed;

        [Header("UI (Drops).")]
        public Canvas _sprinkleCanvas;
        public RectTransform m_RectTransform;

        [Header("Refs.")]
        [ReadOnlyInspector] public MainMenuManager _mainMenuManager;

        int _enlargeTweenId;
        int _fadingTweenId;

        public void CancelSprinkleJobs()
        {
            LeanTween.cancel(_fadingTweenId);
            LeanTween.cancel(_enlargeTweenId);

            _sprinkleCanvas.enabled = false;
        }

        #region Tween Fade.
        public void FadeInSprinkle()
        {
            _fadingTweenId = LeanTween.alpha(m_RectTransform, 1, _sprinkleFadeSpeed).setEase(_sprinkleFadeEaseType).setOnComplete(FadeOutSprinkle).id;
        }

        public void FadeOutSprinkle()
        {
            _fadingTweenId = LeanTween.alpha(m_RectTransform, _sprinkleMinFadeValue, _sprinkleFadeSpeed).setEase(_sprinkleFadeEaseType).setOnComplete(FadeInSprinkle).id;
        }
        #endregion

        #region Tween Enlarge.
        public void EnlargeSprinkle()
        {
            _enlargeTweenId = LeanTween.size(m_RectTransform, _sprinkleMaxSize, _sprinkleEnlargeSpeed).setEase(_sprinkleEnlargeEaseType).setOnComplete(MinimizeSprinkle).id;
        }

        public void MinimizeSprinkle()
        {
            _enlargeTweenId = LeanTween.size(m_RectTransform, _sprinkleMinSize, _sprinkleEnlargeSpeed).setEase(_sprinkleEnlargeEaseType).setOnComplete(EnlargeSprinkle).id;
        }
        #endregion

        #region Rotate Sprinkle.
        public void RotateSprinkle()
        {
            m_RectTransform.RotateAround(m_RectTransform.position, _mainMenuManager.vector3Fwd, _rotateSpeed * _mainMenuManager._delta);
        }
        #endregion
    }
}