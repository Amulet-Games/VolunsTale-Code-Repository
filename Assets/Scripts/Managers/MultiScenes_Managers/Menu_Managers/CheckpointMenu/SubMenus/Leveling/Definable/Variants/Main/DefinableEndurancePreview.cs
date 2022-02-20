using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class DefinableEndurancePreview : AlterableDefinableAttributePreview, IPointerEnterHandler
    {
        [Header("Definable Stats.")]
        public DefinableLightningPreview _lightningPreview;
        public DefinableStaminaPreview _staminaPreview;

        #region On / Off / Tick Attribute Preview.
        public override void Tick()
        {
            HighlighterTick();

            if (LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                _lightningPreview.HighlighterTick();
            }
            else
            {
                _staminaPreview.HighlighterTick();
            }
        }

        public override void OnAttributePreview()
        {
            OnHighlighter();

            if (!_levelingHub._isLackingVolunWhenMenuOpen)
                OnBothButtons();

            if (LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                _lightningPreview.OnHighlighter();
            }
            else
            {
                _staminaPreview.OnHighlighter();
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
                _lightningPreview.OffHighlighter();
            }
            else
            {
                _staminaPreview.OffHighlighter();
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
                _lightningPreview.ResetOnMenuClose();
            }
            else
            {
                _staminaPreview.ResetOnMenuClose();
            }
        }
        #endregion

        #region Reset On Preview Group Switch.
        public override void ResetOnPreviewGroupSwitch(bool isSwitchPreviewGroup)
        {
            if (isSwitchPreviewGroup)
            {
                _staminaPreview.ResetOnMenuClose();
                _lightningPreview.OnHighlighter();
            }
            else
            {
                _lightningPreview.ResetOnMenuClose();
                _staminaPreview.OnHighlighter();
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

            _statsHandler._prev_endurance++;

            RedrawIncrementPreview();
            RedrawIncrementButton();
            PingPongAttributeTextSize();

            _statsHandler.Modifly_Prev_LightningReduct();
            if (_statsHandler._prev_lightning_reduction - _statsHandler.b_lightning_reduction > 0.5f)
            {
                _lightningPreview.RedrawIncrementPreview();
            }

            _statsHandler.Modifly_Prev_Stamina();
            _staminaPreview.RedrawIncrementPreview();

            _playerLevelPreview.IncrementAttributePreview();
        }

        public override void DecrementAttributePreview()
        {
            _volunPreview.DecrementAttributePreview();

            _statsHandler._prev_endurance--;

            bool _hasBackToInitialValue = _statsHandler.endurance == _statsHandler._prev_endurance ? true : false;
            RedrawDecrementPreview(_hasBackToInitialValue);
            RedrawDecrementButton(_hasBackToInitialValue);
            PingPongAttributeTextSize();

            _statsHandler.Modifly_Prev_LightningReduct();
            _lightningPreview.RedrawDecrementPreview();

            _statsHandler.Modifly_Prev_Stamina();
            _staminaPreview.RedrawDecrementPreview();

            _playerLevelPreview.DecrementAttributePreview();
        }

        protected override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_endurance.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        protected override void RedrawDecrementPreview(bool _hasBackToInitialValue)
        {
            _afterChangesText.text = _statsHandler._prev_endurance.ToString();

            if (_hasBackToInitialValue)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawConfirmedAttributePreview()
        {
            string _enduranceText = _statsHandler.endurance.ToString();

            _beforeChangesText.text = _enduranceText;
            _afterChangesText.text = _enduranceText;
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