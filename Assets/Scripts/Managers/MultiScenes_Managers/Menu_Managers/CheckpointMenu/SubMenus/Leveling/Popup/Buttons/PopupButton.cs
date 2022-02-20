using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public abstract class PopupButton : MonoBehaviour
    {
        [Header("Button Index.")]
        public int _referedButtonIndex;

        [Header("UIs.")]
        public Image _buttonImage;
        public Sprite _enabledButtonSprite;
        public Sprite _hoveringButtonSprite;

        [Header("Manager Ref.")]
        [ReadOnlyInspector] public RectTransform _buttonRect;
        
        #region Sprite.
        public void ChangeButtonSpriteToEnable()
        {
            _buttonImage.sprite = _enabledButtonSprite;
        }
        
        public void ChangeButtonSpriteToHovering()
        {
            _buttonImage.sprite = _hoveringButtonSprite;
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            _buttonRect = _buttonImage.rectTransform;
        }
        #endregion

        public abstract void OnButtonClick(PopupMessage _popupMessage);
    }
}