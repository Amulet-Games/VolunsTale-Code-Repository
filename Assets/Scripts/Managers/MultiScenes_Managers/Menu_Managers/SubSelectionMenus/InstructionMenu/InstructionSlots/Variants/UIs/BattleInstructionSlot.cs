using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BattleInstructionSlot : BaseInstructionSlot
    {
        [Header("Page.")]
        public ScrollableInstructionPage referedPage;

        public override void OnSelectSlot()
        {
            BaseOnSelectSlot();
        }

        public override void SetCurrentInstructPage()
        {
            _instructionMenuManager.currentInstructPage = referedPage;
            _instructionMenuManager.currentScrollableInstructPage = referedPage;
        }
    }
}
