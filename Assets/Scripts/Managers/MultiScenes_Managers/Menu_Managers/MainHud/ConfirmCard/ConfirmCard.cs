using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ConfirmCard : MonoBehaviour
    {
        [Header("Config.")]
        public CanvasGroup cardCanvasGroup;
        public LeanTweenType cardEaseType;
        public float cardFadeInSpeed = 1;
        public float cardFadeOutSpeed = 1;
        int cardTweenOnId;
        int cardTweenOffId;

        [Header("Ref.")]
        Canvas _cardCanvas;

        public void Init()
        {
            InitCardCanvas();
            DisableCardCanvas();
        }

        void EnableCardCanvas()
        {
            _cardCanvas.enabled = true;
        }

        public void ShowConfirmCard()
        {
            if (LeanTween.isTweening(cardTweenOffId))
                LeanTween.cancel(cardTweenOffId);

            EnableCardCanvas();
            cardTweenOnId = LeanTween.alphaCanvas(cardCanvasGroup, 1, cardFadeInSpeed).setEase(cardEaseType).id;
        }

        public void DisableCardCanvas()
        {
            _cardCanvas.enabled = false;
        }

        public void HideConfirmCard()
        {
            if (LeanTween.isTweening(cardTweenOnId))
                LeanTween.cancel(cardTweenOnId);

            cardTweenOffId = LeanTween.alphaCanvas(cardCanvasGroup, 0, cardFadeOutSpeed).setEase(cardEaseType).setOnComplete(DisableCardCanvas).id;
        }

        public void HideConfirmCardOnCompleteAction(Action _OnComplete = null)
        {
            if (LeanTween.isTweening(cardTweenOnId))
                LeanTween.cancel(cardTweenOnId);

            cardTweenOffId = LeanTween.alphaCanvas(cardCanvasGroup, 0, cardFadeOutSpeed).setEase(cardEaseType).setOnComplete(_OnComplete).id;
        }

        #region Init.
        void InitCardCanvas()
        {
            _cardCanvas = GetComponent<Canvas>();
        }
        #endregion
    }
}