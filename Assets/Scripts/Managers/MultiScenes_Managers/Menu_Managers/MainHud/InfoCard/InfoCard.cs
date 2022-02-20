using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class InfoCard : MonoBehaviour
    {
        [Header("Config.")]
        public CanvasGroup cardCanvasGroup;
        public LeanTweenType infoCardEaseType;
        public float infoCardFadeInSpeed = 1;
        public float infoCardFadeOutSpeed = 1;
        protected int cardTweenId;

        [Header("Ref.")]
        Canvas _cardCanvas = null;
        protected MainHudManager _mainHudManager = null;

        public void Init(MainHudManager _mainHudManager)
        {
            InitGetRefs();
            DisableInfoCanvas();
            ChildClassInit();

            void InitGetRefs()
            {
                this._mainHudManager = _mainHudManager;
                _cardCanvas = GetComponent<Canvas>();
            }
        }
        
        protected void Default_ShowInfoCard()
        {
            if (LeanTween.isTweening(cardTweenId))
                LeanTween.cancel(cardTweenId);
            
            EnableInfoCanvas();
            cardTweenId = LeanTween.alphaCanvas(cardCanvasGroup, 1, infoCardFadeInSpeed).setEase(infoCardEaseType).id;
        }

        protected void Default_HideInfoCard()
        {
            if (LeanTween.isTweening(cardTweenId))
                LeanTween.cancel(cardTweenId);
            
            DisableInfoCanvas();
            cardTweenId = LeanTween.alphaCanvas(cardCanvasGroup, 0, infoCardFadeOutSpeed).setEase(infoCardEaseType).setOnComplete(DisableInfoCanvas).id;
        }

        protected void EnableInfoCanvas()
        {
            _cardCanvas.enabled = true;
        }

        protected void DisableInfoCanvas()
        {
            _cardCanvas.enabled = false;
        }

        public abstract void OffInfoCard();

        protected virtual void ChildClassInit()
        {
        }
    }
}