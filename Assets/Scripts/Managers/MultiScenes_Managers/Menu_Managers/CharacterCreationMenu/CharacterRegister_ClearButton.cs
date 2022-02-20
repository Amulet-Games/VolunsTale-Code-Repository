using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SA
{
    public class CharacterRegister_ClearButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("Window (Drops).")]
        public CharacterRegisterWindow _window;

        [Header("Image (Drops).")]
        public Image _buttonImage;

        [Header("Sprite (Drops).")]
        public Sprite _normalSprite;
        public Sprite _hoveringSprite;
        public Sprite _pressedSprite;

        public void OnPointerClick(PointerEventData eventData)
        {
            _buttonImage.sprite = _pressedSprite;

            LeanTween.value(0, 1, 0.12f).setOnComplete(OnCompleteWait);

            void OnCompleteWait()
            {
                _window.Clear_UIButton();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _buttonImage.sprite = _hoveringSprite;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _buttonImage.sprite = _normalSprite;
        }
    }
}