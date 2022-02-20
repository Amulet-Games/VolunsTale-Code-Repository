using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class DefinableVolunPreview : DefinableAttributePreview, IPointerEnterHandler
    {
        [Header("Insufficient Volun Color.")]
        public Color _insufficientColor;

        public override void IncrementAttributePreview()
        {
            _statsHandler._prev_voluns -= _statsHandler.GetNextPreviewLevelupRequireFromTable();

            int _nextLevelRequire = _statsHandler.GetLevelupRequireAfterNextLevelFromTable();

            if (_statsHandler._prev_voluns < _nextLevelRequire)
                _levelingHub.SetIsNotEnoughVolunToTrue();

            _levelingHub._nextLevelRequiresText.text = _nextLevelRequire.ToString();

            RedrawIncrementPreview();
        }

        protected override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_voluns.ToString();
        }

        public override void DecrementAttributePreview()
        {
            int _currentLevelRequire = _statsHandler.GetLevelUpRequireFromTable(_statsHandler._prev_playerLevel);
            _statsHandler._prev_voluns += _currentLevelRequire;

            _levelingHub.SetIsNotEnoughVolunToFalse();
            _levelingHub._nextLevelRequiresText.text = _currentLevelRequire.ToString();

            RedrawVolunDecrementPreview();
        }

        void RedrawVolunDecrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_voluns.ToString();
            ChangeBackToNormalColor();
        }

        protected override void RedrawDecrementPreview(bool _hasBackToInitialValue)
        {
            
        }

        public override void RedrawConfirmedAttributePreview()
        {
            string _volunText = _statsHandler.voluns.ToString();

            _beforeChangesText.text = _volunText;
            _afterChangesText.text = _volunText;

            if (!_levelingHub._isNotEnoughVolun)
                ChangeBackToNormalColor();
        }

        public void ChangeToInsufficientColor()
        {
            _afterChangesText.color = _insufficientColor;
        }

        public void ChangeBackToNormalColor()
        {
            _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawDefinitionDetail()
        {
            LevelingMenuManager.singleton.definitionHub.OnDefineDetail(_referedDefineDetail);
        }

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            _levelingHub._levelingMenuManager.previewHub.CheckIsShowingPreviewSelector();
            RedrawDefinitionDetail();
        }
        #endregion

        public override void AttributePreviewSetup(LevelingHub _levelingHub)
        {
            this._levelingHub = _levelingHub;
            _statsHandler = _levelingHub._statsHandler;
        }
    }
}