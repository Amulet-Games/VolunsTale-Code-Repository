using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class UnConfirmLevelupButton : PopupButton
    {
        public override void OnButtonClick(PopupMessage _popupMessage)
        {
            _popupMessage._levelingMenuManager.SetIsInMessageStateToFalse();
        }
    }
}