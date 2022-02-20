using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RuntimeArrow : RuntimeItem
    {
        [Header("Refs.")]
        [ReadOnlyInspector] public ArrowItem _referedArrowItem;

        public override Item GetReferedItem()
        {
            return _referedArrowItem;
        }
    }
}