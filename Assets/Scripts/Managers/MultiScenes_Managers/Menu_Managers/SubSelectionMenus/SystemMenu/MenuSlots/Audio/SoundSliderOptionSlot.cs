using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class SoundSliderOptionSlot : SliderOptionSlot
    {
        #region On Sound Slider Changed.
        public void ChangeSoundVolumeFromSlider(float _value)
        {
            _referedSliderText.text = _value.ToString();
        }

        protected override void Increase_Slider_ByInput()
        {
            
        }

        protected override void Decrease_Slider_ByInput()
        {
            
        }
        #endregion

        #region Sound Mixer -> Slider Value Converting.
        #endregion

        #region Setup.
        public override void Setup(int _slotIndex, BaseSystemDetail _systemDetail)
        {
            base.Setup(_slotIndex, _systemDetail);
            Base_SliderOptionSetup();


            SetSliderValueFromSound();
            UpdateSliderText();
        }

        void SetupSound()
        {

        }

        void SetSliderValueFromSound()
        {
            _referedSlider.minValue = 0;
            _referedSlider.maxValue = 100;

            _referedSlider.value = 40;
        }
        #endregion
    }
}