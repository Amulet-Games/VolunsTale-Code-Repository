using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SA
{
    public class ResolutionChoiceSlot : BaseChoiceSlot
    {
        public RectTransform _choiceSlotRect;
        public TMP_Text _slotTitleText;

        public override void OnChoiceSelected()
        {
            _choiceDetail.SetCurrentSlotByPointerEvent();
        }

        public override Vector3 GetSlotLocalPosition()
        {
            return _choiceSlotRect.localPosition;
        }

        public override TMP_Text GetSlotTitleText()
        {
            return _slotTitleText;
        }
    }
}