using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DefinableThrustPreview : DefinableStatsPreview
    {
        public override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_thrust_reduction.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        public override void RedrawDecrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_thrust_reduction.ToString();

            if (_statsHandler._prev_thrust_reduction - _statsHandler.b_thrust_reduction < 0.5f)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawConfirmedStatsPreview()
        {
            string _baseThrust = _statsHandler.b_thrust_reduction.ToString();

            _beforeChangesText.text = _baseThrust;
            _afterChangesText.text = _baseThrust;
            _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }
        
        public override void RedrawDefinitionDetail()
        {
            LevelingMenuManager.singleton.definitionHub.OnDefineDetail(_referedDefineDetail);
        }
    }
}
