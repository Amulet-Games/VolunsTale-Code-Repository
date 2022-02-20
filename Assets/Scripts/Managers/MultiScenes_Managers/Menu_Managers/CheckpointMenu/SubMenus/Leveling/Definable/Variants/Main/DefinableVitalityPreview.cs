using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class DefinableVitalityPreview : AlterableDefinableAttributePreview, IPointerEnterHandler
    {
        [Header("Definable Stats.")]
        public DefinablePhysicalPreview _physicalPreview;
        public DefinableStrikePreview _strikePreview;
        public DefinableThrustPreview _thrustPreview;
        public DefinableSlashPreview _slashPreview;
        public DefinableEquipLoadPreview _equipLoadPreview;

        #region On / Off / Tick Attribute Preview.
        public override void Tick()
        {
            HighlighterTick();

            if (LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                _physicalPreview.HighlighterTick();
                _strikePreview.HighlighterTick();
                _thrustPreview.HighlighterTick();
                _slashPreview.HighlighterTick();
            }
            else
            {
                _equipLoadPreview.HighlighterTick();
            }
        }

        public override void OnAttributePreview()
        {
            OnHighlighter();

            if (!_levelingHub._isLackingVolunWhenMenuOpen)
                OnBothButtons();

            if (LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                _physicalPreview.OnHighlighter();
                _strikePreview.OnHighlighter();
                _thrustPreview.OnHighlighter();
                _slashPreview.OnHighlighter();
            }
            else
            {
                _equipLoadPreview.OnHighlighter();
            }
        }

        public override void OffAttributePreview()
        {
            OffHighlighter();
            CancelHighlighter();

            if (!_levelingHub._isLackingVolunWhenMenuOpen)
                OffBothButtons();

            if (LevelingMenuManager.singleton.isSwitchPreviewGroup)
            { 
                _physicalPreview.OffHighlighter();
                _strikePreview.OffHighlighter();
                _thrustPreview.OffHighlighter();
                _slashPreview.OffHighlighter();
            }
            else
            {
                _equipLoadPreview.OffHighlighter();
            }
        }
        #endregion

        #region Reset On Menu Close.
        public override void ResetOnMenuClose()
        {
            BaseResetOnMenuClose();
            ReverseAttributeTextSize();

            if (LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                _physicalPreview.ResetOnMenuClose();
                _strikePreview.ResetOnMenuClose();
                _thrustPreview.ResetOnMenuClose();
                _slashPreview.ResetOnMenuClose();
            }
            else
            {
                _equipLoadPreview.ResetOnMenuClose();
            }
        }
        #endregion

        #region Reset On Preview Group Switch.
        public override void ResetOnPreviewGroupSwitch(bool isSwitchPreviewGroup)
        {
            if (isSwitchPreviewGroup)
            {
                _equipLoadPreview.ResetOnMenuClose();

                _physicalPreview.OnHighlighter();
                _strikePreview.OnHighlighter();
                _thrustPreview.OnHighlighter();
                _slashPreview.OnHighlighter();
            }
            else
            {
                _physicalPreview.ResetOnMenuClose();
                _strikePreview.ResetOnMenuClose();
                _thrustPreview.ResetOnMenuClose();
                _slashPreview.ResetOnMenuClose();

                _equipLoadPreview.OnHighlighter();
            }
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            _levelingHub.GetCur_AlterDefinablePreview_ByCursor(this);
        }
        #endregion

        public override void IncrementAttributePreview()
        {
            _volunPreview.IncrementAttributePreview();

            _statsHandler._prev_vitality++;

            RedrawIncrementPreview();
            RedrawIncrementButton();
            PingPongAttributeTextSize();

            _statsHandler.Modifly_Prev_PhyReduct();
            if (_statsHandler._prev_physical_reduction - _statsHandler.b_physical_reduction > 0.5f)
            {
                _physicalPreview.RedrawIncrementPreview();
                _strikePreview.RedrawIncrementPreview();
                _thrustPreview.RedrawIncrementPreview();
                _slashPreview.RedrawIncrementPreview();
            }

            _statsHandler.Modifly_Prev_EquipLoad();
            if (_statsHandler._prev_total_equip_load - _statsHandler.b_total_equip_load > 0.5f)
            {
                _equipLoadPreview.RedrawIncrementPreview();
            }

            _playerLevelPreview.IncrementAttributePreview();
        }

        public override void DecrementAttributePreview()
        {
            _volunPreview.DecrementAttributePreview();

            _statsHandler._prev_vitality--;

            bool _hasBackToInitialValue = _statsHandler.vitality == _statsHandler._prev_vitality ? true : false;
            RedrawDecrementPreview(_hasBackToInitialValue);
            RedrawDecrementButton(_hasBackToInitialValue);
            PingPongAttributeTextSize();

            _statsHandler.Modifly_Prev_PhyReduct();

            _physicalPreview.RedrawDecrementPreview();
            _strikePreview.RedrawDecrementPreview();
            _thrustPreview.RedrawDecrementPreview();
            _slashPreview.RedrawDecrementPreview();

            _statsHandler.Modifly_Prev_EquipLoad();
            _equipLoadPreview.RedrawDecrementPreview();

            _playerLevelPreview.DecrementAttributePreview();
        }

        protected override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_vitality.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        protected override void RedrawDecrementPreview(bool _hasBackToInitialValue)
        {
            _afterChangesText.text = _statsHandler._prev_vitality.ToString();

            if (_hasBackToInitialValue)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawConfirmedAttributePreview()
        {
            string _vitalityText = _statsHandler.vitality.ToString();

            _beforeChangesText.text = _vitalityText;
            _afterChangesText.text = _vitalityText;
            _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawDefinitionDetail()
        {
            LevelingMenuManager.singleton.definitionHub.OnDefineDetail(_referedDefineDetail);
        }

        public override void AttributePreviewSetup(LevelingHub _levelingHub)
        {
            this._levelingHub = _levelingHub;
            _statsHandler = _levelingHub._statsHandler;

            AlterableSetup();
        }
    }
}