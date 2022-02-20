using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Variables/Player Variables/Amulet Held Transform")]
    public class AmuletHeldTransform : ScriptableObject
    {
        /// Always going to be held on right hand.
        [Header("Transform")]
        public Vector3 pos;
        public Vector3 eulers;
    }
}