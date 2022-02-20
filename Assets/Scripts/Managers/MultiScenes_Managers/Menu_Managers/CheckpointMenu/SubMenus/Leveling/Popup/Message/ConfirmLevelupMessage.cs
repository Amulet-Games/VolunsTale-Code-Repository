using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ConfirmLevelupMessage : PopupMessage
    {
        [Header("Confirm Levelup Refs.")]
        [ReadOnlyInspector] StateManager _states;

        public void ConfirmLevelupMessageSetup()
        {
            Setup();
            _states = _inp._states;
        }

        public override void OnConfirmLevelupButtonClick()
        {
            _states.OnConfirmLevelup();
            _levelingMenuManager.OnConfirmLevelup();
        }
    }
}