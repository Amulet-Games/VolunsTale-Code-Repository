using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class ResolutionDropdownDetail : BaseChoiceDetail
    {
        [Header("Resolutions.")]
        public Resolution[] _resolutions;

        [Header("ScrollBar.")]
        public Scrollbar _resolutionScrollbar;
        public float _scrollbarMaxLimit = -300;
        public float _scrollbarMinLimit = -1200;

        [Header("Status.")]
        [ReadOnlyInspector] public int _currentResIndex;

        [Header("Refs.")]
        [ReadOnlyInspector] public ResolutionDropdownOptionSlot _resolutionOptionSlot;

        #region On Detail Preview.
        public override void OnDetailPreview()
        {
            OnDetailPreviewUpdateBarPosition();
            ShowChoiceDetail();
        }
        #endregion
        
        #region On Detail Open.
        public override void OnDetailOpen()
        {
            OnDetailOpenSetDetailUpdatable();
            OnDetailOpenSetCurrentSlot();
            OnDetailOpenEnableRaycastTarget();
            OnDetailOpenEnableScrollInteractive();
        }

        void OnDetailOpenSetDetailUpdatable()
        {
            _systemMenuManager._currentDetailUpdatable = this;
        }

        void OnDetailOpenSetCurrentSlot()
        {
            _slotIndex = _currentChosenSlot._slotIndex + 1;
            _slotIndex = _slotIndex == _slotsLength ? 0 : _slotIndex;
            _currentSlot = _choiceSlots[_slotIndex];

            OnCurrentSlot();
        }
        #endregion

        #region On Detail Close.
        public override void OnDetailClose()
        {
            OnDetailCloseResetColor();
            OnDetailCloseFadeOutDetail();
            OnDetailCloseDisableRaycastTarget();
            OnDetailCloseDisableBarInteractive();
        }

        public void OnDetailCloseAfterSelect()
        {
            OnDetailCloseFadeOutDetail();
            OnDetailCloseDisableRaycastTarget();
            OnDetailCloseDisableBarInteractive();
        }

        void OnDetailCloseResetColor()
        {
            OffCurrentSlot();
        }
        
        void OnDetailCloseFadeOutDetail()
        {
            _resolutionOptionSlot._graphicSystemDetail.OffDropDownSelect();
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
            if (_systemMenuManager.menu_down_input)
            {
                bool _isPutScrollbarOnTop = false;

                _slotIndex++;
                if (_slotIndex == _currentResIndex)
                {
                    _slotIndex++;
                    if (_slotIndex == _slotsLength)
                    {
                        _slotIndex = 0;
                        _isPutScrollbarOnTop = true;
                    }
                }
                else
                {
                    if (_slotIndex == _slotsLength)
                    {
                        _slotIndex = 0;
                        _isPutScrollbarOnTop = true;
                        if (_slotIndex == _currentResIndex)
                        {
                            _slotIndex++;
                        }
                    }
                }

                BaseChoiceSlot _nextChoiceSlot = _choiceSlots[_slotIndex];

                /// Update Scrollbar Position.
                if (_isPutScrollbarOnTop)
                {
                    PutScrollbarOnTop();
                }
                else if (_resolutionScrollbar.value != 0)
                {
                    if (_nextChoiceSlot.GetSlotLocalPosition().y < _scrollbarMinLimit)
                    {
                        OnIndexDropUpdateBarPosition();
                    }
                }

                /// Set to Current Choice Slot.
                SetCurrentSlot(_nextChoiceSlot);
            }
            else if (_systemMenuManager.menu_up_input)
            {
                bool _isPutScrollbarToBottom = false;

                _slotIndex--;
                if (_slotIndex == _currentResIndex)
                {
                    _slotIndex--;
                    if (_slotIndex < 0)
                    {
                        _slotIndex = _slotsLength - 1;
                        _isPutScrollbarToBottom = true;
                    }
                }
                else
                {
                    if (_slotIndex < 0)
                    {
                        _slotIndex = _slotsLength - 1;
                        _isPutScrollbarToBottom = true;
                        if (_slotIndex == _currentResIndex)
                        {
                            _slotIndex--;
                        }
                    }
                }

                BaseChoiceSlot _nextChoiceSlot = _choiceSlots[_slotIndex];

                /// Update Scrollbar Position.
                if (_isPutScrollbarToBottom)
                {
                    PutScrollbarToBottom();
                }
                else if (_resolutionScrollbar.value != 1)
                {
                    if (_nextChoiceSlot.GetSlotLocalPosition().y > _scrollbarMaxLimit)
                    {
                        OnIndexRiseUpdateBarPosition();
                    }
                }

                /// Set to Current Choice Slot.
                SetCurrentSlot(_nextChoiceSlot);
            }
            else if (_systemMenuManager.menu_scroll_bwd_input)
            {
                OnDetailClose();
            }
            else if (_systemMenuManager.menu_select_input)
            {
                SetResolutionSlotByInput();
            }
        }
        #endregion

        #region ScrollBar.
        void OnDetailOpenEnableScrollInteractive()
        {
            _resolutionScrollbar.interactable = true;
        }

        void OnDetailCloseDisableBarInteractive()
        {
            _resolutionScrollbar.interactable = false;
        }

        void OnDetailPreviewUpdateBarPosition()
        {
            /// Because the bar starter value is 1
            _resolutionScrollbar.value = 1 - (_currentResIndex / _slotsLength);
        }

        void OnIndexDropUpdateBarPosition()
        {
            /// 1 - Number of Slots In Between Current Slot To The End of the List.
            /// _slotsLength - 1 is because we use array Index to calculate reduction.
            float _barMoveAmountInPerc = 1 / (_slotsLength - 1 - _currentSlot._slotIndex);
            _resolutionScrollbar.value -= _barMoveAmountInPerc;
        }

        void PutScrollbarOnTop()
        {
            _resolutionScrollbar.value = 1;
        }

        void PutScrollbarToBottom()
        {
            _resolutionScrollbar.value = 0;
        }

        public void OnIndexRiseUpdateBarPosition()
        {
            float _barMoveAmountInPerc = 1 / _currentSlot._slotIndex;
            _resolutionScrollbar.value += _barMoveAmountInPerc;
        }
        #endregion

        #region Set Current Slot.
        public override void SetCurrentSlot(BaseChoiceSlot _optionSlot)
        {
            OffCurrentSlot();
            _currentSlot = _optionSlot;
            OnCurrentSlot();
        }

        void OnCurrentSlot()
        {
            _currentSlot.OnCurrentSlot();
        }

        void OffCurrentSlot()
        {
            _currentSlot.OffCurrentSlot();
        }
        #endregion

        #region Set Current Resolution Slot.
        public void SetResolutionSlotByInput()
        {
            OnSelectHideFormerResolutionSlotShadow();

            _currentChosenSlot = _currentSlot;
            _currentResIndex = _currentSlot._slotIndex;

            OnSelectChangeShadowColor();
            OnSelectChangeOptionSlotText();
            OnSelectSetHasChangedStatsStatus();
            CloseChoiceDetailAfterSelect();
        }

        public override void SetCurrentSlotByPointerEvent()
        {
            OnSelectHideFormerResolutionSlotShadow();

            _currentChosenSlot = _currentSlot;
            _currentResIndex = _currentSlot._slotIndex;

            OnSelectChangeShadowColor();
            OnSelectChangeOptionSlotText();
            CloseChoiceDetailAfterSelect();
        }

        void OnSelectHideFormerResolutionSlotShadow()
        {
            _currentChosenSlot._shadowCanvas.enabled = false;
        }

        void OnSelectChangeShadowColor()
        {
            _currentChosenSlot.ChangeShadowToPressed();
        }

        void OnSelectChangeOptionSlotText()
        {
            _resolutionOptionSlot._dropDownTitleText.text = _currentChosenSlot.GetSlotTitleText().text;
        }

        void OnSelectSetHasChangedStatsStatus()
        {
            _resolutionOptionSlot._graphicSystemDetail.SetHasChangedStatsStatusToTrue();
        }

        void CloseChoiceDetailAfterSelect()
        {
            OnDetailCloseAfterSelect();
        }
        #endregion
        
        #region Setup.
        public void Setup(SystemMenuManager _systemMenuManager, ResolutionDropdownOptionSlot _resolutionOptionSlot)
        {
            this._systemMenuManager = _systemMenuManager;
            this._resolutionOptionSlot = _resolutionOptionSlot;

            BaseSetup();

            SetupResolutions();
            SetupSlots();
            SetupScrollBar();
        }

        void SetupResolutions()
        {
            /// Get All the avaliable resolutions(filtered by Supported Aspect Ratios)
            _resolutions = Screen.resolutions;
            _slotsLength = _resolutions.Length;

            /// Get current resolution of the game.
            int screen_w = Screen.width;
            int screen_h = Screen.height;

            /// Cache temp variables.
            int temp_w = 0;
            int temp_h = 0;
            
            StringBuilder _choiceText = new StringBuilder();

            for (int i = 0; i < _slotsLength; i++)
            {
                temp_w = _resolutions[i].width;
                temp_h = _resolutions[i].height;

                _choiceText.Clear();
                _choiceText.Append(temp_w).Append(" x ").Append(temp_h);

                /// Change Choice Title Text.
                _choiceSlots[i].GetSlotTitleText().text = _choiceText.ToString();

                if (temp_w == screen_w && temp_h == screen_h)
                {
                    _resolutionOptionSlot._dropDownTitleText.text = _choiceText.ToString();
                    _currentChosenSlot = _choiceSlots[i];
                    _currentResIndex = i;
                }
            }
        }

        public override void SetupSlots()
        {
            for (int i = 0; i < _slotsLength; i++)
            {
                _choiceSlots[i].Setup(i, this);
            }
            
            SetupCurrentResolutionSlot();

            void SetupCurrentResolutionSlot()
            {
                _currentChosenSlot._shadowCanvas.enabled = true;
                _currentChosenSlot.ChangeShadowToPressed();
            }
        }

        void SetupScrollBar()
        {
            OnDetailCloseDisableBarInteractive();
        }
        #endregion
    }
}