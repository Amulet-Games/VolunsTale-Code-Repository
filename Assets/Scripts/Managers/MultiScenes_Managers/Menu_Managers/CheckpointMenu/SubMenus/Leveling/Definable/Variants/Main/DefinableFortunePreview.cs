using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class DefinableFortunePreview : AlterableDefinableAttributePreview, IPointerEnterHandler
    {
        [Header("Dark Reduct Definable Stats.")]
        public DefinableItemDiscoveryPreview _itemDiscoverPreview;

        #region On / Off / Tick Attribute Preview.
        public override void Tick()
        {
            HighlighterTick();

            if (!LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                _itemDiscoverPreview.HighlighterTick();
            }
        }

        public override void OnAttributePreview()
        {
            OnHighlighter();

            if (!_levelingHub._isLackingVolunWhenMenuOpen)
                OnBothButtons();

            if (!LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                _itemDiscoverPreview.OnHighlighter();
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
                _itemDiscoverPreview.OffHighlighter();
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
                _itemDiscoverPreview.ResetOnMenuClose();
            }
        }
        #endregion

        #region Reset On Preview Group Switch.
        public override void ResetOnPreviewGroupSwitch(bool isSwitchPreviewGroup)
        {
            if (isSwitchPreviewGroup)
            {
                _itemDiscoverPreview.ResetOnMenuClose();
            }
            else
            {
                _itemDiscoverPreview.OnHighlighter();
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

            _statsHandler._prev_fortune++;

            RedrawIncrementPreview();
            RedrawIncrementButton();
            PingPongAttributeTextSize();

            _statsHandler.Increment_Prev_itemDiscover();
            _itemDiscoverPreview.RedrawIncrementPreview();

            _playerLevelPreview.IncrementAttributePreview();
        }

        public override void DecrementAttributePreview()
        {
            _volunPreview.DecrementAttributePreview();

            _statsHandler._prev_fortune--;

            bool _hasBackToInitialValue = _statsHandler.fortune == _statsHandler._prev_fortune ? true : false;
            RedrawDecrementPreview(_hasBackToInitialValue);
            RedrawDecrementButton(_hasBackToInitialValue);
            PingPongAttributeTextSize();

            _statsHandler.Decrement_Prev_itemDiscover();
            _itemDiscoverPreview.RedrawDecrementPreview();

            _playerLevelPreview.DecrementAttributePreview();
        }

        protected override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_fortune.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        protected override void RedrawDecrementPreview(bool _hasBackToInitialValue)
        {
            _afterChangesText.text = _statsHandler._prev_fortune.ToString();

            if (_hasBackToInitialValue)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawConfirmedAttributePreview()
        {
            string _fortuneText = _statsHandler.fortune.ToString();

            _beforeChangesText.text = _fortuneText;
            _afterChangesText.text = _fortuneText;
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