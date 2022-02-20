using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class InteractionCard : MonoBehaviour
    {
        [Header("Card Group.")]
        public CanvasGroup cardCanvasGroup;
        public RectTransform cardRect;
        public Image cardImage;

        [Header("Fade Config")]
        public float cardFadeOutSpeed = 0.2f;

        [Header("Move Config")]
        public float cardMoveSpeed = 0.15f;
        public float cardStartPosX = 2270;
        public float cardFinalPosX = 1660;
        int cardTweenId;

        [Header("Color Switch Config.")]
        public float cardColorSwitchSpeed = 0.125f;
        public float cardColorSwitchDelayTime = 0.2f;
        public Color cardOriginalColor;
        public Color cardBlinkColor;

        [Header("Ref.")]
        Canvas _cardCanvas;
        
        public void ShowInteractionCard()
        {
            if (LeanTween.isTweening(cardTweenId))
                LeanTween.cancel(cardTweenId);

            EnableCardCanvas();
            cardTweenId = LeanTween.moveX(cardRect, cardFinalPosX, cardMoveSpeed).setEaseOutCirc().id;

            LeanTween.value(0, 1, cardColorSwitchSpeed).setDelay(cardColorSwitchDelayTime).setEaseOutCirc().setOnUpdate
                (
                    (value) =>
                    cardImage.color = Color.Lerp(cardOriginalColor, cardBlinkColor, value)
                )
                .setLoopPingPong(1);
        }
        
        /// Show / Hide Card.
        public void HideInterCard_MoveOut_WithOnCompleteAction(Action _OnComplete)
        {
            if (LeanTween.isTweening(cardTweenId))
                LeanTween.cancel(cardTweenId);

            cardTweenId = LeanTween.moveX(cardRect, cardStartPosX, cardMoveSpeed).setEaseOutCirc().setOnComplete(OnCompleteMoveOut).id;

            void OnCompleteMoveOut()
            {
                cardCanvasGroup.alpha = 0;
                cardImage.color = cardOriginalColor;

                _OnComplete.Invoke();
            }
        }

        public void HideInterCard_FadeOut_WithOnCompleteAction(Action _OnComplete)
        {
            if (LeanTween.isTweening(cardTweenId))
                LeanTween.cancel(cardTweenId);
            
            cardTweenId = LeanTween.alphaCanvas(cardCanvasGroup, 0, cardFadeOutSpeed).setEaseOutCirc().setOnComplete(OnCompleteFadeOut).id;

            void OnCompleteFadeOut()
            {
                Vector3 _startPos = cardRect.localPosition;
                _startPos.x = cardStartPosX;
                cardRect.localPosition = _startPos;

                cardImage.color = cardOriginalColor;

                _OnComplete.Invoke();
            }
        }

        /// Enable / Disable Canvas.
        void EnableCardCanvas()
        {
            _cardCanvas.enabled = true;
            cardCanvasGroup.alpha = 1;
        }

        public void DisableCardCanvas()
        {
            _cardCanvas.enabled = false;
        }

        #region Setup.
        public void Setup()
        {
            SetupCardCanvas();
        }

        void SetupCardCanvas()
        {
            _cardCanvas = GetComponent<Canvas>();
        }
        #endregion
    }
}