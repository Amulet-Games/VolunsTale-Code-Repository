using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DefinablePoisePreview : DefinableStatsPreview
    {
        public override void RedrawIncrementPreview()
        {
        }

        public override void RedrawDecrementPreview()
        {
        }

        public override void RedrawConfirmedStatsPreview()
        {
            string _basePoise = _statsHandler.b_poise.ToString();

            _beforeChangesText.text = _basePoise;
            _afterChangesText.text = _basePoise;
            _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawDefinitionDetail()
        {
            LevelingMenuManager.singleton.definitionHub.OnDefineDetail(_referedDefineDetail);
        }
    }
}