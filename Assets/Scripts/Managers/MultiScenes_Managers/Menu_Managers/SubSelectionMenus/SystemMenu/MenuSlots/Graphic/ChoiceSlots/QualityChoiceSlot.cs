using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class QualityChoiceSlot : BaseChoiceSlot
    {
        public override void OnChoiceSelected()
        {
            _choiceDetail.SetCurrentSlotByPointerEvent();
        }
    }
}
