using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SA
{
    public abstract class DualOptionSlot : BaseOptionSlot
    {
        [Header("Dual Option Image.")]
        public Image _1stOptionImage;
        public Image _2ndOptionImage;

        [Header("Right Raycast Image.")]
        /// '_slotRaycastImage' in base option slot is for _left option.
        public Image _rightRaycastImage;

        [Header("Dual Option Status.")]
        [ReadOnlyInspector] public bool _isInFirstOption;

        [Header("Extra Refs.")]
        [ReadOnlyInspector] public SystemMenuManager _systemMenuManager;

        #region On Menu Open.
        #endregion

        #region Tick.
        protected void GetCurrentOptionByInput()
        {
            if (_systemMenuManager.menu_right_input || _systemMenuManager.menu_left_input)
            {
                _isInFirstOption = !_isInFirstOption;
                if (_isInFirstOption)
                {
                    Change_1st_OptionSprite_Hovering();
                    Change_2nd_OptionSprite_Normal();
                }
                else
                {
                    Change_2nd_OptionSprite_Hovering();
                    Change_1st_OptionSprite_Normal();
                }
            }
        }

        public void PointerEventSet_1st_OptionCurrent()
        {
            _isInFirstOption = true;
            Change_1st_OptionSprite_Hovering();
            Change_2nd_OptionSprite_Normal();
        }

        public void PointerEventSet_2nd_OptionCurrent()
        {
            _isInFirstOption = false;
            Change_2nd_OptionSprite_Hovering();
            Change_1st_OptionSprite_Normal();
        }

        protected void SelectCurrentOptionByInput()
        {
            if (_systemDetail._systemMenuManager.menu_select_input)
            {
                if (_isInFirstOption)
                {
                    On_1st_OptionSelected();
                }
                else
                {
                    On_2nd_OptionSelected();
                }
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("position = " + eventData.position);
            _systemDetail.GetCurrentDualSlotOptionByPointerEvent(eventData.position.x);
        }
        #endregion

        #region On Current Slot.
        public override void OnCurrentSlot()
        {
            _isInFirstOption = true;
            Change_1st_OptionSprite_Hovering();
        }

        public void OnQuitMenuOpenSetSlot()
        {
            _isInFirstOption = true;
            Change_1st_OptionSprite_Hovering();
            Change_2nd_OptionSprite_Normal();
        }
        #endregion

        #region Off Current Slot.
        public override void OffCurrentSlot()
        {
            if (_isInFirstOption)
            {
                Change_1st_OptionSprite_Normal();
            }
            else
            {
                Change_2nd_OptionSprite_Normal();
            }
        }
        #endregion

        #region On Detail Close Reset Slot.
        public override void OnDetailCloseResetSlot()
        {
        }
        #endregion

        #region Change Option Sprite.
        /// No Avabilable.
        public void Change_1st_OptionSprite_NotAvabilable()
        {
            _1stOptionImage.sprite = _systemMenuManager._optionsNotAvabiableSprite;
        }

        public void Change_2nd_OptionSprite_NotAvabilable()
        {
            _2ndOptionImage.sprite = _systemMenuManager._optionsNotAvabiableSprite;
        }

        /// Normal.
        public void Change_1st_OptionSprite_Normal()
        {
            _1stOptionImage.sprite = _systemMenuManager._1stOptionNormalSprite;
        }

        public void Change_2nd_OptionSprite_Normal()
        {
            _2ndOptionImage.sprite = _systemMenuManager._2ndOptionNormalSprite;
        }

        /// Hovering.
        public void Change_1st_OptionSprite_Hovering()
        {
            _1stOptionImage.sprite = _systemMenuManager._1stOptionHoveringSprite;
        }

        public void Change_2nd_OptionSprite_Hovering()
        {
            _2ndOptionImage.sprite = _systemMenuManager._2ndOptionHoveringSprite;
        }

        public void ChangeAllSpriteToNotAvabilable()
        {
            Change_1st_OptionSprite_NotAvabilable();
            Change_2nd_OptionSprite_NotAvabilable();
        }
        #endregion

        #region On Selected.
        public abstract void On_1st_OptionSelected();

        public abstract void On_2nd_OptionSelected();
        #endregion

        #region Set Status.
        public void OnHasChangedStatsStatus()
        {
            ChangeSprite();
            ChangeAllImageRaycastable();

            void ChangeSprite()
            {
                Change_1st_OptionSprite_Normal();
                Change_2nd_OptionSprite_Normal();
            }
        }

        public void OffHasChangedStatsStatus()
        {
            ChangeSprite();
            ChangeAllImageNotRaycastable();

            void ChangeSprite()
            {
                Change_1st_OptionSprite_NotAvabilable();
                Change_2nd_OptionSprite_NotAvabilable();
            }
        }

        public void ChangeAllImageRaycastable()
        {
            _slotRaycastImage.raycastTarget = true;
            _rightRaycastImage.raycastTarget = true;
        }

        public void ChangeAllImageNotRaycastable()
        {
            _slotRaycastImage.raycastTarget = false;
            _rightRaycastImage.raycastTarget = false;
        }
        #endregion

        #region Setup.
        public override void Setup(int _slotIndex, BaseSystemDetail _systemDetail)
        {
            this._slotIndex = _slotIndex;
            this._systemDetail = _systemDetail;

            DualOptionSlot_SetupRef();
            DualOptionSlot_SetupPointerEventImage();
        }

        public void DualOptionSlot_SetupRef()
        {
            _systemMenuManager = _systemDetail._systemMenuManager;
            _systemDetail._currentDualOptionSlot = this;
        }

        public void DualOptionSlot_SetupPointerEventImage()
        {
            _slotRaycastImage.raycastTarget = false;
            _rightRaycastImage.raycastTarget = false;
        }
        #endregion
    }
}