using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DefinableStrikePreview : DefinableStatsPreview
    {
        public override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_strike_reduction.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        public override void RedrawDecrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_strike_reduction.ToString();

            if (_statsHandler._prev_strike_reduction - _statsHandler.b_strike_reduction < 0.5f)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;  
        }

        public override void RedrawConfirmedStatsPreview()
        {
            string _baseStrike = _statsHandler.b_strike_reduction.ToString();

            _beforeChangesText.text = _baseStrike;
            _afterChangesText.text = _baseStrike;
            _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawDefinitionDetail()
        {
            LevelingMenuManager.singleton.definitionHub.OnDefineDetail(_referedDefineDetail);
        }
    }
}