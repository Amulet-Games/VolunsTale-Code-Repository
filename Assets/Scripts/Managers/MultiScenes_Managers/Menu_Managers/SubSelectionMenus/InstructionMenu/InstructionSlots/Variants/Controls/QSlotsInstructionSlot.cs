using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class QSlotsInstructionSlot : BaseInstructionSlot
    {
        [Header("Page.")]
        public RegularInstructionPage referedPage;

        public override void OnSelectSlot()
        {
            BaseOnSelectSlot();
        }

        public override void SetCurrentInstructPage()
        {
            _instructionMenuManager.currentInstructPage = referedPage;
        }
    }
}
