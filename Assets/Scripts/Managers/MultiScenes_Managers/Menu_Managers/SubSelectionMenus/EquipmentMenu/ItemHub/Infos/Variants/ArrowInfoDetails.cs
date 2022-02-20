using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class ArrowInfoDetails : ItemInfoDetails
    {
        [Header("General Loaded Img.")]
        public Image itemIcon_Image;

        [Header("Refs.")]
        [ReadOnlyInspector] public ItemHub _itemHub;

        RuntimeArrow runtimeArrow;
        ArrowItem _referedArrow;
        
        public void ShowArrowInfoDetails(RuntimeArrow _runtimeArrow)
        {
            runtimeArrow = _runtimeArrow;
            _referedArrow = _runtimeArrow._referedArrowItem;

            ShowInfoDetails();
        }
    }
}