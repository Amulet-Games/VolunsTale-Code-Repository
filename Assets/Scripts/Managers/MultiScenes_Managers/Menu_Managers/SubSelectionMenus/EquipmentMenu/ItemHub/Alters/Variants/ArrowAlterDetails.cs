using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class ArrowAlterDetails : ItemAlterDetails
    {
        [Header("General.")]
        public TMP_Text itemTitle_Text;
        public Image itemIcon_Image;

        [Header("Refs.")]
        [ReadOnlyInspector] public ItemHub _itemHub;

        RuntimeArrow runtimeArrow;
        ArrowItem _referedArrow;

        public void RedrawArrowAlterDetails(RuntimeArrow _runtimeArrow)
        {
            runtimeArrow = _runtimeArrow;
            _referedArrow = _runtimeArrow._referedArrowItem;
        }
    }
}