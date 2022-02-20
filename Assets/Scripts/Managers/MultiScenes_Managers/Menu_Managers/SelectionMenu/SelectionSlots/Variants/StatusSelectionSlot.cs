using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class StatusSelectionSlot : BaseSelectionSlot
    {
        #region On Current Slot.
        protected override void UpdateTitleText()
        {
            _selectionMenuManager.titleText.text = "Status";
        }
        #endregion

        #region On Select Slot.
        public override void OnSelectSlot()
        {
            OffCurrentSlot();
            _selectionMenuManager.ResetToFirstSlot();
        }
        #endregion
    }
}