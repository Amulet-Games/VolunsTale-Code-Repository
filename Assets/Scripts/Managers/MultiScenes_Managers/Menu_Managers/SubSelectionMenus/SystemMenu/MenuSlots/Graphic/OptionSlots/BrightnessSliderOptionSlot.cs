using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SA
{
    public class BrightnessSliderOptionSlot : SliderOptionSlot
    {
        [Header("Post Exposure Para.")]
        /// Brightness is 'Post Exposure' value in 'Color Grading'.
        FloatParameter _WF_postExposurePara;
        
        [Header("Brightness Config.")]     
        public float _maxBrightness = 1.2f;
        /// If this is a non negative value, 
        /// you need to change the formula of converting "Slider value" to "actual exposure value".
        public float _minBrightness = -1.2f;
        public float _bringhtnessChangeRate = 0.05f;

        #region On Brightness Slider Changed.
        /// This is executed in slider interactable.
        public void Change_WF_BrightnessFromSlider(float _value)
        {
            _referedSliderText.text = _value.ToString();

            float t = _value / 100;
            _WF_postExposurePara.Override((t * (_maxBrightness - _minBrightness)) + _minBrightness);
        }

        protected override void Increase_Slider_ByInput()
        {
            _WF_postExposurePara.Override(_WF_postExposurePara.value + _bringhtnessChangeRate);

            WF_UpdateSliderFromPostExposure();
            UpdateSliderText();
        }

        protected override void Decrease_Slider_ByInput()
        {
            _WF_postExposurePara.Override(_WF_postExposurePara.value - _bringhtnessChangeRate);

            WF_UpdateSliderFromPostExposure();
            UpdateSliderText();
        }
        #endregion
        
        #region Actual Exposure -> Slider Value Converting.
        void WF_UpdatePostExposureFromSlider()
        {
            float t = _referedSlider.value / 100;
            _WF_postExposurePara.Override((t * (_maxBrightness - _minBrightness)) + _minBrightness);
        }

        void WF_UpdateSliderFromPostExposure()
        {
            _referedSlider.value = (_WF_postExposurePara.value - _minBrightness) / (_maxBrightness - _minBrightness) * 100;
        }
        #endregion

        #region Setup.
        public override void Setup(int _slotIndex, BaseSystemDetail _systemDetail)
        {
            base.Setup(_slotIndex, _systemDetail);
            Base_SliderOptionSetup();

            SetupBrightness();
            SetSliderValueFromBrightness();
            UpdateSliderText();
        }

        void SetupBrightness()
        {
            PostProcessManager.singleton._WF_PPV.profile.TryGet(out ColorAdjustments _colorAdjustment);
            _WF_postExposurePara = _colorAdjustment.postExposure;
        }

        void SetSliderValueFromBrightness()
        {
            _referedSlider.minValue = 0;
            _referedSlider.maxValue = 100;
            WF_UpdateSliderFromPostExposure();
        }
        #endregion
    }
}