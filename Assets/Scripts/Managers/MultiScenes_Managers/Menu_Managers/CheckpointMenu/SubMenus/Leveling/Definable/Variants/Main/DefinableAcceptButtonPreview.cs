using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class DefinableAcceptButtonPreview : AlterableDefinableAttributePreview, IPointerEnterHandler, IPointerExitHandler
    {
        public override void Tick()
        {
            if (_levelingHub._isInAcceptButton)
            {
                HighlighterTick();
            }
        }

        public override void OnAttributePreview()
        {
            _levelingHub._levelingMenuManager.previewHub.CheckIsShowingPreviewSelector();
            _levelingHub._previewIndex = _alterDefinablePreviewIndex;

            _levelingHub.OnAcceptButton();
            _levelingHub._isLoopComplete = true;
        }

        public override void OffAttributePreview()
        {
            _levelingHub.OffAcceptButton();
            CancelHighlighter();
        }

        public override void ResetOnMenuClose()
        {
            CancelHighlighter();
        }

        public override void AttributePreviewSetup(LevelingHub _levelingHub)
        {
            this._levelingHub = _levelingHub;
            _isAcceptButton = true;
        }

        public override void RedrawConfirmedAttributePreview()
        {
            _levelingHub.SetHasAlteredPlayerLevelStatus(false);
        }

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnAttributePreview();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OffAttributePreview();
        }
        #endregion

        #region Ignored.
        public override void DecrementAttributePreview()
        {
        }

        public override void IncrementAttributePreview()
        {
        }

        protected override void RedrawDecrementPreview(bool _hasBackToInitialValue)
        {
        }

        protected override void RedrawIncrementPreview()
        {
        }

        public override void ResetOnPreviewGroupSwitch(bool _isSwitchPreviewGroup)
        {
        }

        public override void RedrawDefinitionDetail()
        {
        }
        #endregion
    }
}