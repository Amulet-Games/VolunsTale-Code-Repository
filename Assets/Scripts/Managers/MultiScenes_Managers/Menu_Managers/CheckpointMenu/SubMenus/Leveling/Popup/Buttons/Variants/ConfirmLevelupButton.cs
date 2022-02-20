using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ConfirmLevelupButton : PopupButton
    {
        public override void OnButtonClick(PopupMessage _popupMessage)
        {
            _popupMessage.OnConfirmLevelupButtonClick();
        }
    }
}