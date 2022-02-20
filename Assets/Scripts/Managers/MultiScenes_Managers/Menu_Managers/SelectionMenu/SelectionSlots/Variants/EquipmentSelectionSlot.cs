using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class EquipmentSelectionSlot : BaseSelectionSlot
    {
        #region On Current Slot.
        protected override void UpdateTitleText()
        {
            _selectionMenuManager.titleText.text = "Equipment";
        }
        #endregion

        #region On Select Slot.
        public override void OnSelectSlot()
        {
            OffCurrentSlot();
            _selectionMenuManager.QuitSelectionMenu_To_EquipmentMenu();
        }
        #endregion
    }
}