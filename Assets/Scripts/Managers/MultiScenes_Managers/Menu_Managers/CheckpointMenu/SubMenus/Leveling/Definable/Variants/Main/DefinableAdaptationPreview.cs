using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class DefinableAdaptationPreview : AlterableDefinableAttributePreview, IPointerEnterHandler
    {
        [Header("Definable Stats.")]
        public DefinableAttunementSlotPreview _attunementSlotPreview;
        public DefinableFocusPreview _focusPreview;

        #region On / Off / Tick Attribute Preview.
        public override void Tick()
        {
            HighlighterTick();

            if (!LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                _attunementSlotPreview.HighlighterTick();
                _focusPreview.HighlighterTick();
            }
        }

        public override void OnAttributePreview()
        {
            OnHighlighter();

            if (!_levelingHub._isLackingVolunWhenMenuOpen)
                OnBothButtons();

            if (!LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                _attunementSlotPreview.OnHighlighter();
                _focusPreview.OnHighlighter();
            }
        }

        public override void OffAttributePreview()
        {
            OffHighlighter();
            CancelHighlighter();

            if (!_levelingHub._isLackingVolunWhenMenuOpen)
                OffBothButtons();

            if (!LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                _attunementSlotPreview.OffHighlighter();
                _focusPreview.OffHighlighter();
            }
        }
        #endregion

        #region Reset On Menu Close.
        public override void ResetOnMenuClose()
        {
            BaseResetOnMenuClose();
            ReverseAttributeTextSize();

            if (!LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                _attunementSlotPreview.ResetOnMenuClose();
                _focusPreview.ResetOnMenuClose();
            }
        }
        #endregion

        #region Reset On Preview Group Switch.
        public override void ResetOnPreviewGroupSwitch(bool isSwitchPreviewGroup)
        {
            if (isSwitchPreviewGroup)
            {
                _attunementSlotPreview.ResetOnMenuClose();
                _focusPreview.ResetOnMenuClose();
            }
            else
            {
                _attunementSlotPreview.OnHighlighter();
                _focusPreview.OnHighlighter();
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

            _statsHandler._prev_adaptation++;

            RedrawIncrementPreview();
            RedrawIncrementButton();
            PingPongAttributeTextSize();

            _statsHandler.Modifly_Prev_attunementSlots();
            if (_statsHandler._prev_attunement_slots != _statsHandler.b_attunement_slots)
            {
                _attunementSlotPreview.RedrawIncrementPreview();
            }

            _statsHandler.Modifly_Prev_Focus();
            _focusPreview.RedrawIncrementPreview();

            _playerLevelPreview.IncrementAttributePreview();
        }

        public override void DecrementAttributePreview()
        {
            _volunPreview.DecrementAttributePreview();

            _statsHandler._prev_adaptation--;

            bool _hasBackToInitialValue = _statsHandler.adaptation == _statsHandler._prev_adaptation ? true : false;
            RedrawDecrementPreview(_hasBackToInitialValue);
            RedrawDecrementButton(_hasBackToInitialValue);
            PingPongAttributeTextSize();

            _statsHandler.Modifly_Prev_attunementSlots();
            _attunementSlotPreview.RedrawDecrementPreview();

            _statsHandler.Modifly_Prev_Focus();
            _focusPreview.RedrawDecrementPreview();

            _playerLevelPreview.DecrementAttributePreview();
        }

        protected override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_adaptation.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        protected override void RedrawDecrementPreview(bool _hasBackToInitialValue)
        {
            _afterChangesText.text = _statsHandler._prev_adaptation.ToString();

            if (_hasBackToInitialValue)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawConfirmedAttributePreview()
        {
            string _attunemntText = _statsHandler.adaptation.ToString();

            _beforeChangesText.text = _attunemntText;
            _afterChangesText.text = _attunemntText;
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