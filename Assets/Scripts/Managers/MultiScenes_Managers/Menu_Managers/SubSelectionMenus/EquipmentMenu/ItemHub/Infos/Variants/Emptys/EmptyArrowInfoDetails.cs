using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SA
{
    public class EmptyArrowInfoDetails : ItemInfoDetails
    {
        [Header("Generals.")]
        public TMP_Text itemTitle_Text;

        [Header("Refs.")]
        [ReadOnlyInspector] public ItemHub _itemHub;

        public void ShowEmptyArrowInfo()
        {
            ShowInfoDetails();

            /// General Info
            if (_itemHub.isShowReviewSlotEmptyInfo)
            {
                itemTitle_Text.fontSize = 50;
                itemTitle_Text.text = "Empty Arrow Slot.";
            }
            else
            {
                itemTitle_Text.fontSize = 46;
                itemTitle_Text.text = "No arrows in the bag yet.";
            }
        }
    }
}