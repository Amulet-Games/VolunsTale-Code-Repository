using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace SA
{
    public class QualityDropdownDetail : BaseChoiceDetail
    {
        [Header("Render assets.")]
        public RenderPipelineAsset[] _pipelineAssets;
        [ReadOnlyInspector] public string[] _qualityChoiceTexts;

        [Header("Status.")]
        [ReadOnlyInspector] public int _currentQualityIndex;

        [Header("Refs.")]
        [ReadOnlyInspector] public QualityDropdownOptionSlot _qualityOptionSlot;

        #region On Detail Preview.
        public override void OnDetailPreview()
        {
            ShowChoiceDetail();
        }
        #endregion

        #region On Detail Open.
        public override void OnDetailOpen()
        {
            OnDetailOpenSetDetailUpdatable();
            OnDetailOpenSetCurrentSlot();
            OnDetailOpenEnableRaycastTarget();
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
        }

        public void OnDetailCloseAfterSelect()
        {
            OnDetailCloseFadeOutDetail();
            OnDetailCloseDisableRaycastTarget();
        }

        void OnDetailCloseResetColor()
        {
            OffCurrentSlot();
        }
        
        void OnDetailCloseFadeOutDetail()
        {
            _qualityOptionSlot._graphicSystemDetail.OffDropDownSelect();
        }

        void OnDetailCloseChangeDetailUpdatable()
        {
            _systemMenuManager._currentDetailUpdatable = _qualityOptionSlot._graphicSystemDetail;
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
            if (_systemMenuManager.menu_down_input)
            {
                _slotIndex++;
                if (_slotIndex == _currentQualityIndex)
                {
                    _slotIndex++;
                    _slotIndex = _slotIndex == _slotsLength ? 0 : _slotIndex;
                }
                else
                {
                    if (_slotIndex == _slotsLength)
                    {
                        _slotIndex = 0;
                        if (_slotIndex == _currentQualityIndex)
                        {
                            _slotIndex++;
                        }
                    }
                }
                
                /// Set to Current Choice Slot.
                SetCurrentSlot(_choiceSlots[_slotIndex]);
            }
            else if (_systemMenuManager.menu_up_input)
            {
                _slotIndex--;
                if (_slotIndex == _currentQualityIndex)
                {
                    _slotIndex--;
                    _slotIndex = _slotIndex < 0 ? _slotsLength - 1 : _slotIndex;
                }
                else
                {
                    if (_slotIndex < 0)
                    {
                        _slotIndex = _slotsLength - 1;
                        if (_slotIndex == _currentQualityIndex)
                        {
                            _slotIndex--;
                        }
                    }
                }
                
                /// Set to Current Choice Slot.
                SetCurrentSlot(_choiceSlots[_slotIndex]);
            }
            else if (_systemMenuManager.menu_scroll_bwd_input)
            {
                OnDetailClose();
            }
            else if (_systemMenuManager.menu_select_input)
            {
                SetQualitySlotByInput();
            }
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

        #region Set Current Quality Slot.
        public void SetQualitySlotByInput()
        {
            OnSelectHideFormerQualitySlotShadow();

            _currentChosenSlot = _currentSlot;
            _currentQualityIndex = _currentSlot._slotIndex;

            OnSelectChangeShadowColor();
            OnSelectChangeOptionSlotText();
            OnSelectSetHasChangedStatsStatus();
            CloseChoiceDetailAfterSelect();
        }

        public override void SetCurrentSlotByPointerEvent()
        {
            OnSelectHideFormerQualitySlotShadow();

            _currentChosenSlot = _currentSlot;
            _currentQualityIndex = _currentSlot._slotIndex;

            OnSelectChangeShadowColor();
            OnSelectChangeOptionSlotText();
            CloseChoiceDetailAfterSelect();
        }

        void OnSelectHideFormerQualitySlotShadow()
        {
            _currentChosenSlot._shadowCanvas.enabled = false;
        }

        void OnSelectChangeShadowColor()
        {
            _currentChosenSlot.ChangeShadowToPressed();
        }

        void OnSelectChangeOptionSlotText()
        {
            _qualityOptionSlot._dropDownTitleText.text = _qualityChoiceTexts[_currentQualityIndex];
        }

        void OnSelectSetHasChangedStatsStatus()
        {
            _qualityOptionSlot._graphicSystemDetail.SetHasChangedStatsStatusToTrue();
        }

        void CloseChoiceDetailAfterSelect()
        {
            OnDetailCloseAfterSelect();
        }
        #endregion

        #region Setup.
        public void Setup(SystemMenuManager _systemMenuManager, QualityDropdownOptionSlot _qualityOptionSlot)
        {
            this._systemMenuManager = _systemMenuManager;
            this._qualityOptionSlot = _qualityOptionSlot;

            BaseSetup();

            SetupCurrentQuality();
            SetupSlots();
        }

        void SetupCurrentQuality()
        {
            _currentQualityIndex = QualitySettings.GetQualityLevel();
            _qualityChoiceTexts = QualitySettings.names;

            _currentChosenSlot = _choiceSlots[_currentQualityIndex];
            _qualityOptionSlot._dropDownTitleText.text = _qualityChoiceTexts[_currentQualityIndex];

            QualitySettings.renderPipeline = _pipelineAssets[_currentQualityIndex];
        }

        public override void SetupSlots()
        {
            _slotsLength = 3;
            for (int i = 0; i < _slotsLength; i++)
            {
                _choiceSlots[i].Setup(i, this);
            }
            
            SetupCurrentQualitySlot();

            void SetupCurrentQualitySlot()
            {
                _currentChosenSlot._shadowCanvas.enabled = true;
                _currentChosenSlot.ChangeShadowToPressed();
            }
        }
        #endregion
    }
}