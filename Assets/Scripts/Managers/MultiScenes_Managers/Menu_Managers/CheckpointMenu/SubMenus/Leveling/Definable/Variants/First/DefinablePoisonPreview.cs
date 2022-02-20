using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DefinablePoisonPreview : DefinableStatsPreview
    {
        public override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_poison_resistance.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        public override void RedrawDecrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_poison_resistance.ToString();

            if (_statsHandler._prev_poison_resistance - _statsHandler.b_poison_resistance < 0.5f)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawConfirmedStatsPreview()
        {
            string _basePoison = _statsHandler.b_poison_resistance.ToString();

            _beforeChangesText.text = _basePoison;
            _afterChangesText.text = _basePoison;
            _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawDefinitionDetail()
        {
            LevelingMenuManager.singleton.definitionHub.OnDefineDetail(_referedDefineDetail);
        }
    }
}
