using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AudioSystemDetail : BaseSystemDetail
    {
        [Header("PingPong Color Config.")]
        public LeanTweenType _pingPongColorEaseType;
        public float _pingPongColorSpeed;
        
        #region Non Serialized.
        int _pingPongColorTweenId = 0;
        #endregion

        #region On Detail Open.
        public override void OnDetailOpen()
        {
            OnDetailOpenSetCurrentSlot();
            OnDetailOpenEnableExtraInputs();
            SetDetailAsCurrentUpdatable();
            EnableSlotsRaycastable();
        }
        #endregion

        #region On Detail Close
        public override void OnDetailClose()
        {
            OnDetailCloseResetSlot();
            OnDetailCloseResetHasChangedStats();
            OnDetailCloseDisableExtraInputs();
            DisableSlotsRaycastable();
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
            GetCurrentSlotByInput();

            _systemMenuManager.CloseMenuByInput();

            _currentSlot.Tick();
        }

        void GetCurrentSlotByInput()
        {
            if (_systemMenuManager.menu_down_input)
            {
                _slotIndex++;
                _slotIndex = _slotIndex == _slotsLength ? 0 : _slotIndex;
                
                if (_optionSlots[_slotIndex] == _currentDualOptionSlot)
                {
                    if (!_hasChangedStats)
                    {
                        _slotIndex = 0;
                    }
                }

                SetCurrentSlot(_optionSlots[_slotIndex]);
            }
            else if (_systemMenuManager.menu_up_input)
            {
                _slotIndex--;
                _slotIndex = _slotIndex < 0 ? _slotsLength - 1 : _slotIndex;
                
                if (_optionSlots[_slotIndex] == _currentDualOptionSlot)
                {
                    if (!_hasChangedStats)
                    {
                        _slotIndex = _currentDualOptionSlot._slotIndex - 1;
                    }
                }

                SetCurrentSlot(_optionSlots[_slotIndex]);
            }
        }
        #endregion

        #region Set Current Slot.
        public override void SetCurrentSlot(BaseOptionSlot _optionSlot)
        {
            OffCurrentSlot();
            _currentSlot = _optionSlot;
            OnCurrentSlot();
        }

        public override void OnCurrentSlot()
        {
            _currentSlot.OnCurrentSlot();
        }

        public override void OffCurrentSlot()
        {
            _currentSlot.OffCurrentSlot();
        }
        #endregion
        
        #region On Select Change Shadow Color.
        public override void OnSelectChangeShadowColor()
        {
            PingPongTweenSlotColor();
        }

        void PingPongTweenSlotColor()
        {
            LeanTween.color(_currentSlot._shadowImage.rectTransform, _systemMenuManager._pressedColor, _pingPongColorSpeed).setEase(_pingPongColorEaseType).setLoopPingPong(1);
        }
        #endregion

        #region Setup.
        public override void Setup(SystemMenuManager _systemMenuManager)
        {
            this._systemMenuManager = _systemMenuManager;
            BaseSetup();
            SetupSlots();
        }
        
        public override void SetupSlots()
        {
            _slotsLength = _optionSlots.Length;
            for (int i = 0; i < _slotsLength; i++)
            {
                _optionSlots[i].Setup(i, this);
            }
        }
        #endregion
    }
}