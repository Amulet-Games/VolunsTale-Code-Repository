using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public abstract class SliderOptionSlot : BaseOptionSlot
    {
        [Header("Slider UI.")]
        public Slider _referedSlider;

        [Header("Text UI.")]
        public TMP_Text _referedSliderText;

        [Header("Slider Status.")]
        [ReadOnlyInspector] public bool _isPressingRight;
        [ReadOnlyInspector] public bool _isPressingLeft;
        [ReadOnlyInspector] public float _prev_value;

        [Header("Slider Option Slot Refs.")]
        [ReadOnlyInspector] public SystemMenuManager _systemMenuManager;

        #region Tick.
        public override void Tick()
        {
            if (_systemMenuManager.menu_right_input_buttonPressing)
            {
                SetIsPressingRightStatus(true);
            }
            else if (_systemMenuManager.menu_left_input_buttonPressing)
            {
                SetIsPressingLeftStatus(true);
            }
            else
            {
                SetIsPressingRightStatus(false);
                SetIsPressingLeftStatus(false);
            }
        }
        #endregion

        #region On / Off Slot.
        public override void OnCurrentSlot()
        {
            ChangeShadowToHovering();
        }

        public override void OffCurrentSlot()
        {
            ChangeShadowToNormal();
        }
        #endregion

        #region On Detail Close Reset Slot.
        public override void OnDetailCloseResetSlot()
        {
            ChangeShadowToNormal();
        }
        #endregion

        #region On Slider Changed
        protected abstract void Increase_Slider_ByInput();

        protected abstract void Decrease_Slider_ByInput();
        #endregion

        #region Set Status.
        public void SetIsPressingRightStatus(bool _isPressingRight)
        {
            if (_isPressingRight)
            {
                if (!this._isPressingRight)
                {
                    this._isPressingRight = true;

                    ChangeShadowToPressed();
                    _systemDetail.SetHasChangedStatsStatusToTrue();
                }
                else
                {
                    /// Keep decrease value.
                    Increase_Slider_ByInput();
                }
            }
            else
            {
                if (this._isPressingRight)
                {
                    this._isPressingRight = false;

                    ChangeShadowToHovering();
                }
            }
        }

        public void SetIsPressingLeftStatus(bool _isPressingLeft)
        {
            if (_isPressingLeft)
            {
                if (!this._isPressingLeft)
                {
                    this._isPressingLeft = true;

                    ChangeShadowToPressed();
                    _systemDetail.SetHasChangedStatsStatusToTrue();
                }
                else
                {
                    /// Keep decrease value.
                    Decrease_Slider_ByInput();
                }
            }
            else
            {
                if (this._isPressingLeft)
                {
                    this._isPressingLeft = false;

                    ChangeShadowToHovering();
                }
            }
        }
        #endregion

        #region Slider Value Converting.
        protected void UpdateSliderText()
        {
            _referedSliderText.text = _referedSlider.value.ToString();
        }
        #endregion

        #region Setup.
        public void Base_SliderOptionSetup()
        {
            _systemMenuManager = _systemDetail._systemMenuManager;
        }
        #endregion
    }
}