using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public abstract class Base_DualButtonMsg_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("2nd Button.")]
        public bool _is2ndButton;

        [Header("Drag and Drop Refs.")]
        public Image _buttonImage;
        public RectTransform _buttonRect;
        public Sprite _enabledButtonSprite;
        public Sprite _hoveringButtonSprite;
        public Sprite _pressedButtonSprite;

        [Header("Message Refs.")]
        [ReadOnlyInspector] public DualButton_MainMenuMsg _dualButtonMsg;
        
        #region On Current Button.
        public void OnCurrentButton()
        {
            ChangeButtonSpriteToHovering();
        }

        public void ChangeButtonSpriteToHovering()
        {
            _buttonImage.sprite = _hoveringButtonSprite;
            _buttonImage.color = _dualButtonMsg._mainMenuManager._fullAlphaColor;
        }
        #endregion

        #region Off Current Button.
        public void OffCurrentButton()
        {
            ChangeButtonSpriteToEnable();
        }
        
        void ChangeButtonSpriteToEnable()
        {
            _buttonImage.sprite = _enabledButtonSprite;
            _buttonImage.color = _dualButtonMsg._mainMenuManager._fullAlphaColor;
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            _dualButtonMsg.GetButtonByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            _dualButtonMsg._isCursorOverSelection = false;
        }
        #endregion

        #region On Select Button.
        public abstract void OnSelectButton();

        protected void Base_OnSelectButton()
        {
            _dualButtonMsg.CancelCurrentButtonTween();
            ChangeSpriteToPressed();
        }
        
        void ChangeSpriteToPressed()
        {
            _buttonImage.sprite = _pressedButtonSprite;
            _buttonImage.color = _dualButtonMsg._mainMenuManager._pressedDownColor;
        }
        #endregion
    }
}