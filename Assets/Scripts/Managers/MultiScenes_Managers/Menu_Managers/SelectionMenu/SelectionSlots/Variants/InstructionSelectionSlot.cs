using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class InstructionSelectionSlot : BaseSelectionSlot
    {
        #region On Current Slot.
        protected override void UpdateTitleText()
        {
            _selectionMenuManager.titleText.text = "Instruction";
        }
        #endregion

        #region On Select Slot.
        public override void OnSelectSlot()
        {
            OffCurrentSlot();
            _selectionMenuManager.QuitSelectionMenu_To_InstructionMenu();
        }
        #endregion
    }
}