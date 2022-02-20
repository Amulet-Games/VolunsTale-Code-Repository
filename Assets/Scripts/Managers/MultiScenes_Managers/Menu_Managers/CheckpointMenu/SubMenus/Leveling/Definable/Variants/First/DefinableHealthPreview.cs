using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DefinableHealthPreview : DefinableStatsPreview
    {
        public override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_hp.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        public override void RedrawDecrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_hp.ToString();

            if (_statsHandler._prev_hp - _statsHandler.b_hp < 0.5f)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawConfirmedStatsPreview()
        {
            string _baseHp = _statsHandler.b_hp.ToString();

            _beforeChangesText.text = _baseHp;
            _afterChangesText.text = _baseHp;
            _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawDefinitionDetail()
        {
            LevelingMenuManager.singleton.definitionHub.OnDefineDetail(_referedDefineDetail);
        }
    }
}