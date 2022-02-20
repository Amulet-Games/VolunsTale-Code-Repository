using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class ItemInfoCard : InfoCard
    {
        [Header("Item Info Card Pop up Config.")]
        public float cardMoveSpeed = 0.15f;
        public float cardStartPosY = 2270;
        public float cardFinalPosY = 1660;

        [Header("Item Info Card Highlight Config.")]
        public LeanTweenType _pingPongEaseType;
        public LeanTweenType _MoveUpEaseType;
        public float pingPongDelayTime = 0.2f;
        public float highlightPingPongSpeed;
        public float highlightMaxAlpha;
        public float highlightEndAlpha;

        [Header("Item Info Card Refs.")]
        [SerializeField] TMP_Text itemNameText = null;
        [SerializeField] TMP_Text itemTypeText = null;
        [SerializeField] TMP_Text itemAmountText = null;
        [SerializeField] Image itemIconImage = null;
        [SerializeField] Image hightlighterImage;
        [SerializeField] RectTransform cardRect;
        Vector3 _originalLocalPos;
        bool _isAmountSectionDeactivated;

        public void ShowInfoCard()
        {
            if (LeanTween.isTweening(cardTweenId))
                LeanTween.cancel(cardTweenId);

            EnableInfoCanvas();

            /// Move Y.
            LeanTween.moveY(cardRect, cardFinalPosY, cardMoveSpeed).setEaseOutCirc();

            /// Fade In Card.
            LeanTween.alphaCanvas(cardCanvasGroup, 1, infoCardFadeInSpeed).setEase(infoCardEaseType);

            /// Ping Pong Highlighter.
            LeanTween.alpha(hightlighterImage.rectTransform, highlightMaxAlpha, highlightPingPongSpeed).setDelay(pingPongDelayTime).setEase(_pingPongEaseType).setOnComplete(OnCompleteFadeInHighlighter);

            void OnCompleteFadeInHighlighter()
            {
                LeanTween.alpha(hightlighterImage.rectTransform, highlightEndAlpha, highlightPingPongSpeed).setEase(_pingPongEaseType);
            }
        }

        protected void HideInfoCard()
        {
            if (LeanTween.isTweening(cardTweenId))
                LeanTween.cancel(cardTweenId);
            
            cardTweenId = LeanTween.alphaCanvas(cardCanvasGroup, 0, infoCardFadeOutSpeed).setEase(infoCardEaseType).setOnComplete(OnCompleteHideInfoCard).id;

            void OnCompleteHideInfoCard()
            {
                DisableInfoCanvas();
                cardRect.localPosition = _originalLocalPos;

                Color _tempColor = hightlighterImage.color;
                _tempColor.a = 0;
                hightlighterImage.color = _tempColor;
            }
        }
        
        public void OnItemInfoCard_Consumable(string _referedItemName, int _referedAmount, Sprite _referedItemSprite)
        {
            CheckIsInfoCardDisplaying();
            SetItemInfoCard();
            
            void SetItemInfoCard()
            {
                itemTypeText.text = "Consumables.";
                itemAmountText.text = _referedAmount.ToString();
                itemNameText.text = _referedItemName;

                if (_isAmountSectionDeactivated)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemAmountText.gameObject.SetActive(true);
                    _isAmountSectionDeactivated = false;
                }

                itemIconImage.sprite = _referedItemSprite;
            }
        }

        public void OnItemInfoCard_Weapon(string _referedItemName)
        {
            CheckIsInfoCardDisplaying();
            SetItemInfoCard();

            void SetItemInfoCard()
            {
                itemTypeText.text = "Weapons.";
                itemNameText.text = _referedItemName;
                
                if (!_isAmountSectionDeactivated)
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemAmountText.gameObject.SetActive(false);
                    _isAmountSectionDeactivated = true;
                }
            }
        }

        public void OnItemInfoCard_Armor(string _referedItemName)
        {
            CheckIsInfoCardDisplaying();
            SetItemInfoCard();

            void SetItemInfoCard()
            {
                itemTypeText.text = "Armors.";
                itemNameText.text = _referedItemName;

                if (!_isAmountSectionDeactivated)
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemAmountText.gameObject.SetActive(false);
                    _isAmountSectionDeactivated = true;
                }
            }
        }

        public void OnItemInfoCard_Powerup(string _referedItemName)
        {
            CheckIsInfoCardDisplaying();
            SetItemInfoCard();

            void SetItemInfoCard()
            {
                itemTypeText.text = "Power Ups.";
                itemNameText.text = _referedItemName;

                if (!_isAmountSectionDeactivated)
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemAmountText.gameObject.SetActive(false);
                    _isAmountSectionDeactivated = true;
                }
            }
        }

        public void OnItemInfoCard_Ring(string _referedItemName)
        {
            CheckIsInfoCardDisplaying();
            SetItemInfoCard();

            void SetItemInfoCard()
            {
                itemTypeText.text = "Power Ups.";
                itemNameText.text = _referedItemName;

                if (!_isAmountSectionDeactivated)
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemAmountText.gameObject.SetActive(false);
                    _isAmountSectionDeactivated = true;
                }
            }
        }

        void CheckIsInfoCardDisplaying()
        {
            if (!_mainHudManager._isInfoCardDisplaying)
            {
                ShowInfoCard();
                _mainHudManager._isInfoCardDisplaying = true;
            }
            else
            {
                /// Fade it back in, but without moving upward.
                Default_ShowInfoCard();
                _mainHudManager._infoCardDisplayTimer = 0;
            }
        }

        public override void OffInfoCard()
        {
            HideInfoCard();
        }

        protected override void ChildClassInit()
        {
            _originalLocalPos = cardRect.localPosition;
        }
    }
}