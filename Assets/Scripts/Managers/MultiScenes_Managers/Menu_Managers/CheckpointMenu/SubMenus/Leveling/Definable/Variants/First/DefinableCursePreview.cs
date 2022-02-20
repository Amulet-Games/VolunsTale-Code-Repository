using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DefinableCursePreview : DefinableStatsPreview
    {
        public override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_curse_resistance.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        public override void RedrawDecrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_curse_resistance.ToString();

            if (_statsHandler._prev_curse_resistance - _statsHandler.b_curse_resistance < 0.5f)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawConfirmedStatsPreview()
        {
            string _baseCurse = _statsHandler.b_curse_resistance.ToString();

            _beforeChangesText.text = _baseCurse;
            _afterChangesText.text = _baseCurse;
            _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawDefinitionDetail()
        {
            LevelingMenuManager.singleton.definitionHub.OnDefineDetail(_referedDefineDetail);
        }
    }
}
