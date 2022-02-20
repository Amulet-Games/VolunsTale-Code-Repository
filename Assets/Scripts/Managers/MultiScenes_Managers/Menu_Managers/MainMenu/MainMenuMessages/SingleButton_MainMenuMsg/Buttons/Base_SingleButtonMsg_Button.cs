using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public abstract class Base_SingleButtonMsg_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Drag and Drop Refs.")]
        public Image _buttonImage;
        public RectTransform _buttonRect;
        public Sprite _hoveringButtonSprite;
        public Sprite _pressedButtonSprite;
        
        [Header("Message Refs.")]
        [ReadOnlyInspector] public SingleButton_MainMenuMsg _referedMsg;

        [Header("Button Tween.")]
        public LeanTweenType _buttonPingPongEaseType = LeanTweenType.easeOutSine;
        public float _buttonPingPongFadeTime = 1.2f;
        public float _buttonPingPongFadeMinValue = 0.5f;

        #region Privates.
        int _buttonTweenId;
        #endregion

        #region On Message Open.
        public void OnMessageOpen()
        {
            OnMessageOpen_ResetButtonSprite();
            OnMessageOpen_StartPingPongTween();
        }

        void OnMessageOpen_ResetButtonSprite()
        {
            _buttonImage.sprite = _hoveringButtonSprite;
            _buttonImage.color = _referedMsg._mainMenuManager._fullAlphaColor;
        }
        
        void OnMessageOpen_StartPingPongTween()
        {
            PingPongFadeOutButton();
        }
        #endregion

        #region On Select Button.
        public abstract void OnSelectButton();
        
        protected void ChangeSpriteToPressed()
        {
            _buttonImage.sprite = _pressedButtonSprite;
            _buttonImage.color = _referedMsg._mainMenuManager._pressedDownColor;
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            _referedMsg._isCursorOverSelection = true;
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            _referedMsg._isCursorOverSelection = false;
        }
        #endregion

        #region Ping Pong Button Tween.
        protected void CancelCurrentButtonTween()
        {
            LeanTween.cancel(_buttonTweenId);
        }

        void PingPongFadeInButton()
        {
            _buttonTweenId = LeanTween.alpha(_buttonRect, 1, _buttonPingPongFadeTime).setEase(_buttonPingPongEaseType).setOnComplete(PingPongFadeOutButton).id;
        }

        public void PingPongFadeOutButton()
        {
            _buttonTweenId = LeanTween.alpha(_buttonRect, _buttonPingPongFadeMinValue, _buttonPingPongFadeTime).setEase(_buttonPingPongEaseType).setOnComplete(PingPongFadeInButton).id;
        }
        #endregion
    }
}