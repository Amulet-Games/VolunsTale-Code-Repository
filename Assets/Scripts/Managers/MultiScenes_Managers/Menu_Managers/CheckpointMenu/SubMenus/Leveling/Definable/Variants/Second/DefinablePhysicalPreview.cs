using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DefinablePhysicalPreview : DefinableStatsPreview
    {
        public override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_physical_reduction.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        public override void RedrawDecrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_physical_reduction.ToString();

            if (_statsHandler._prev_physical_reduction - _statsHandler.b_physical_reduction < 0.5f)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor; 
        }

        public override void RedrawConfirmedStatsPreview()
        {
            string _basePhysical = _statsHandler.b_physical_reduction.ToString();

            _beforeChangesText.text = _basePhysical;
            _afterChangesText.text = _basePhysical;
            _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawDefinitionDetail()
        {
            LevelingMenuManager.singleton.definitionHub.OnDefineDetail(_referedDefineDetail);
        }
    }
}