using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SA
{
    public class EmptyRingInfoDetails : ItemInfoDetails
    {
        [Header("Generals.")]
        public TMP_Text itemTitle_Text;

        [Header("Bottom Desc Texts.")]
        [SerializeField] GameObject emptyDescription;

        [Header("Refs.")]
        [ReadOnlyInspector] public ItemHub _itemHub;

        public void ShowEmptyRingInfo()
        {
            ShowInfoDetails();
            /// General Info
            if (_itemHub.isShowReviewSlotEmptyInfo)
            {
                itemTitle_Text.fontSize = 50;
                itemTitle_Text.text = "Empty Ring Slot.";
                emptyDescription.SetActive(true);
            }
            else
            {
                itemTitle_Text.fontSize = 46;
                itemTitle_Text.text = "No rings in the bag yet.";
                emptyDescription.SetActive(false);
            }
        }
    }
}