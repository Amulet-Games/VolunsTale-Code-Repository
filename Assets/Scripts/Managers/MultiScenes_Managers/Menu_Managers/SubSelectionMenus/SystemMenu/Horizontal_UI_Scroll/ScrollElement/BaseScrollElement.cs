using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public abstract class BaseScrollElement : MonoBehaviour
    {
        [Header("Image.")]
        public RectTransform _elementRect;
        public RectTransform _elementIconRect;
        public TMP_Text _elementText;

        [Header("Refs.")]
        [ReadOnlyInspector] public HorizontalScrollHandler _scrollHandler;
        [ReadOnlyInspector] public Canvas _elementCanvas;
        [ReadOnlyInspector] public Canvas _elementIconCanvas;

        public abstract BaseSystemDetail GetReferedElementDetail();
        
        #region On / Off Canvas.
        public void ShowElement()
        {
            _elementCanvas.enabled = true;
        }

        public void HideElement()
        {
            _elementCanvas.enabled = false;
        }

        public void EnableIconCanvas()
        {
            _elementIconCanvas.enabled = true;
        }

        public void DisableIconCanvas()
        {
            _elementIconCanvas.enabled = false;
        }
        #endregion

        #region Setup.
        public abstract void Setup(HorizontalScrollHandler _scrollHandler);

        protected void BaseSetup()
        {
            SetupCanvas();
        }

        void SetupCanvas()
        {
            _elementCanvas = _elementRect.GetComponent<Canvas>();
            _elementIconCanvas = _elementIconRect.GetComponent<Canvas>();
        }
        #endregion
    }
}