using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ExitSelectionSlot : BaseSelectionSlot
    {
        #region On Current Slot.
        protected override void UpdateTitleText()
        {
            _selectionMenuManager.titleText.text = "Exit To Main Menu";
        }
        #endregion

        #region On Select Slot.
        public override void OnSelectSlot()
        {
            OffCurrentSlot();
            _selectionMenuManager.QuitSelectionMenu_ExitGame();
        }
        #endregion
    }
}