using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DefinableMagicPreview : DefinableStatsPreview
    {
        public override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_magic_reduction.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        public override void RedrawDecrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_magic_reduction.ToString();

            if (_statsHandler._prev_magic_reduction - _statsHandler.b_magic_reduction < 0.5f)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawConfirmedStatsPreview()
        {
            string _baseMagic = _statsHandler.b_magic_reduction.ToString();

            _beforeChangesText.text = _baseMagic;
            _afterChangesText.text = _baseMagic;
            _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawDefinitionDetail()
        {
            LevelingMenuManager.singleton.definitionHub.OnDefineDetail(_referedDefineDetail);
        }
    }
}
